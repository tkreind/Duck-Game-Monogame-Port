// Decompiled with JetBrains decompiler
// Type: DuckGame.ModLoader
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DuckGame
{
  /// <summary>
  /// The class that handles mods to load, and allows mods to retrieve Mod objects.
  /// </summary>
  public static class ModLoader
  {
    private static readonly Dictionary<string, Mod> _loadedMods = new Dictionary<string, Mod>();
    internal static readonly Dictionary<string, Mod> _modAssemblyNames = new Dictionary<string, Mod>();
    internal static readonly Dictionary<Assembly, Mod> _modAssemblies = new Dictionary<Assembly, Mod>();
    internal static readonly Dictionary<System.Type, Mod> _modTypes = new Dictionary<System.Type, Mod>();
    private static IList<Mod> _sortedMods = (IList<Mod>) new List<Mod>();
    private static IList<Mod> _sortedAccessibleMods = (IList<Mod>) new List<Mod>();
    private static readonly List<Tuple<string, Exception>> _modLoadErrors = new List<Tuple<string, Exception>>();
    private static readonly Dictionary<string, System.Type> _typesByName = new Dictionary<string, System.Type>();
    /// <summary>
    /// Returns whether or not any mods are present and not disabled.
    /// </summary>
    private static int _numModsEnabled = 0;
    private static int _numModsTotal = 0;
    internal static string _modString;
    private static CSharpCodeProvider _provider = (CSharpCodeProvider) null;
    private static CompilerParameters _parameters = (CompilerParameters) null;
    internal static HashSet<string> disabledMods;
    internal static bool forceRecompilation = false;
    internal static string modDirectory;
    private static Dictionary<string, ModConfiguration> loadableMods;
    public static string currentModLoadString = "";

    internal static void AddMod(object p) => throw new NotImplementedException();

    internal static List<Tuple<string, Exception>> modLoadErrors => ModLoader._modLoadErrors;

    /// <summary>Get an iterable list of Mods</summary>
    public static IList<Mod> accessibleMods => ModLoader._sortedAccessibleMods;

    public static IList<Mod> allMods => ModLoader._sortedMods;

    public static int numModsEnabled => ModLoader._numModsEnabled;

    public static bool modsEnabled => ModLoader._numModsEnabled != 0;

    public static int numModsTotal => ModLoader._numModsTotal;

    /// <summary>Get a loaded Mod instance from its unique name.</summary>
    /// <typeparam name="T">The special Mod subclass to cast to.</typeparam>
    /// <returns>The Mod instance, or null.</returns>
    public static T GetMod<T>() where T : Mod
    {
      Mod mod = (Mod) null;
      ModLoader._modTypes.TryGetValue(typeof (T), out mod);
      return (T) mod;
    }

    /// <summary>Get a loaded Mod instance from its unique name.</summary>
    /// <param name="name">The name of the mod.</param>
    /// <returns>The Mod instance, or null.</returns>
    public static Mod GetMod(string name)
    {
      Mod mod;
      return ModLoader._loadedMods.TryGetValue(name, out mod) ? mod : (Mod) null;
    }

    public static void DisableModsAndRestart()
    {
      foreach (Mod allMod in (IEnumerable<Mod>) ModLoader.allMods)
        allMod.configuration.Disable();
      ModLoader.RestartGame();
    }

    public static void RestartGame()
    {
      Process.Start(Application.ExecutablePath, Program.commandLine);
      Application.Exit();
    }

    internal static void AddMod(Mod mod)
    {
      ModLoader._loadedMods.Add(mod.configuration.uniqueID, mod);
      if (mod.configuration.disabled)
        return;
      ModLoader._modAssemblyNames.Add(mod.configuration.assembly.FullName, mod);
      ModLoader._modAssemblies.Add(mod.configuration.assembly, mod);
      ModLoader._modTypes.Add(mod.GetType(), mod);
    }

    private static Mod GetOrLoad(
      ModConfiguration modConfig,
      ref Stack<string> modLoadStack,
      ref Dictionary<string, ModConfiguration> loadableMods)
    {
      if (modLoadStack.Contains(modConfig.uniqueID))
        throw new ModCircularDependencyException(modLoadStack);
      modLoadStack.Push(modConfig.uniqueID);
      try
      {
        Mod mod1;
        if (ModLoader._loadedMods.TryGetValue(modConfig.uniqueID, out mod1))
          return mod1;
        Mod mod2;
        if (modConfig.disabled)
        {
          mod2 = (Mod) new DisabledMod();
        }
        else
        {
          foreach (string hardDependency in modConfig.hardDependencies)
          {
            ModConfiguration modConfig1;
            if (!loadableMods.TryGetValue(hardDependency, out modConfig1))
              throw new ModDependencyNotFoundException(modConfig.uniqueID, hardDependency);
            if (modConfig1.disabled)
              throw new ModDependencyNotFoundException(modConfig.uniqueID, hardDependency);
            ModLoader.GetOrLoad(modConfig1, ref modLoadStack, ref loadableMods);
          }
          foreach (string softDependency in modConfig.softDependencies)
          {
            ModConfiguration modConfig1;
            if (loadableMods.TryGetValue(softDependency, out modConfig1) && !modConfig1.disabled)
              ModLoader.GetOrLoad(modConfig1, ref modLoadStack, ref loadableMods);
          }
          modConfig.assembly = Assembly.Load(File.ReadAllBytes(modConfig.isDynamic ? modConfig.tempAssemblyPath : modConfig.assemblyPath));
          System.Type[] array1 = ((IEnumerable<System.Type>) modConfig.assembly.GetExportedTypes()).Where<System.Type>((Func<System.Type, bool>) (type => type.IsSubclassOf(typeof (IManageContent)) && type.IsPublic && type.IsClass && !type.IsAbstract)).ToArray<System.Type>();
          modConfig.contentManager = array1.Length <= 1 ? ContentManagers.GetContentManager(array1.Length == 1 ? array1[0] : (System.Type) null) : throw new ModTypeMissingException(modConfig.uniqueID + " has more than one content manager class");
          System.Type[] array2 = ((IEnumerable<System.Type>) modConfig.assembly.GetExportedTypes()).Where<System.Type>((Func<System.Type, bool>) (type => type.IsSubclassOf(typeof (Mod)) && !type.IsAbstract)).ToArray<System.Type>();
          if (array2.Length != 1)
            throw new ModTypeMissingException(modConfig.uniqueID + " is missing or has more than one Mod subclass");
          if (MonoMain.preloadModContent && modConfig.preloadContent)
            modConfig.content.PreloadContent();
          else
            modConfig.content.PreloadContentPaths();
          mod2 = (Mod) Activator.CreateInstance(array2[0]);
        }
        mod2.configuration = modConfig;
        ModLoader.AddMod(mod2);
        return mod2;
      }
      finally
      {
        modLoadStack.Pop();
      }
    }

    private static string GetModHash()
    {
      if (!ModLoader.modsEnabled)
        return "nomods";
      using (SHA256 shA256Cng = SHA256.Create())
      {
        ModLoader._modString = string.Join("|", ModLoader._sortedAccessibleMods.Where<Mod>((Func<Mod, bool>) (a => !(a is CoreMod) && !(a is DisabledMod))).Select<Mod, string>((Func<Mod, string>) (a => a.configuration.uniqueID + "_" + (object) a.configuration.version)));
        return string.IsNullOrEmpty(ModLoader._modString) ? "nomods" : Convert.ToBase64String(shA256Cng.ComputeHash(Encoding.ASCII.GetBytes(ModLoader._modString)));
      }
    }

    internal static string modHash { get; private set; }

    public static string SmallTypeName(string fullName)
    {
      int length = fullName.IndexOf(',', fullName.IndexOf(',') + 1);
      return length == -1 ? (string) null : fullName.Substring(0, length);
    }

    internal static string SmallTypeName(System.Type type) => ModLoader.SmallTypeName(type.AssemblyQualifiedName);

    private static bool AttemptCompile(ModConfiguration config)
    {
      if (config.noCompilation)
        return false;
      List<string> filesNoCloud = DuckFile.GetFilesNoCloud(config.directory, "*.cs", SearchOption.AllDirectories);
      if (filesNoCloud.Count == 0)
        return false;
      config.isDynamic = true;
      CRC32 crC32 = new CRC32();
      byte[] numArray = new byte[2048];
      foreach (string path in filesNoCloud)
      {
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
          while (fileStream.Position != fileStream.Length)
          {
            int blockLen = fileStream.Read(numArray, 0, numArray.Length);
            crC32.ProcessBlock(numArray, blockLen);
          }
        }
      }
      uint num = crC32.Finalize();
      if (!ModLoader.forceRecompilation && File.Exists(config.hashPath))
      {
        if (File.Exists(config.tempAssemblyPath))
        {
          try
          {
            if ((int) BitConverter.ToUInt32(File.ReadAllBytes(config.hashPath), 0) == (int) num)
              return true;
          }
          catch
          {
          }
        }
      }
      File.WriteAllBytes(config.hashPath, BitConverter.GetBytes(num));
      if (ModLoader._provider == null)
      {
        ModLoader._provider = new CSharpCodeProvider();
        ModLoader._parameters = new CompilerParameters(((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Select<Assembly, string>((Func<Assembly, string>) (assembly => assembly.Location)).ToArray<string>());
        ModLoader._parameters.GenerateExecutable = ModLoader._parameters.GenerateInMemory = false;
      }
      if (File.Exists(config.buildLogPath))
      {
        File.SetAttributes(config.buildLogPath, FileAttributes.Normal);
        File.Delete(config.buildLogPath);
      }
      ModLoader._parameters.OutputAssembly = config.tempAssemblyPath;
      CompilerResults compilerResults = ModLoader._provider.CompileAssemblyFromFile(ModLoader._parameters, filesNoCloud.ToArray());
      if (compilerResults.Errors.Count == 0)
        return true;
      File.WriteAllLines(config.buildLogPath, compilerResults.Output.OfType<string>());
      return false;
    }

    private static ModConfiguration AttemptModLoad(string folder)
    {
      ModConfiguration modConfiguration = new ModConfiguration()
      {
        directory = folder
      };
      modConfiguration.contentDirectory = modConfiguration.directory + "/content/";
      modConfiguration.name = Path.GetFileNameWithoutExtension(folder);
      modConfiguration.content = new ContentPack(modConfiguration);
      try
      {
        if (modConfiguration.name == "DuckGame")
          return (ModConfiguration) null;
        ModLoader.currentModLoadString = modConfiguration.name;
        MonoMain.loadMessage = "COMPILING MOD " + ModLoader.currentModLoadString;
        modConfiguration.LoadConfiguration();
        if (!modConfiguration.disabled)
        {
          if (!File.Exists(modConfiguration.assemblyPath) && !ModLoader.AttemptCompile(modConfiguration))
            return (ModConfiguration) null;
          ++ModLoader._numModsEnabled;
        }
        ++ModLoader._numModsTotal;
        return modConfiguration;
      }
      catch (Exception ex)
      {
        ModLoader._modLoadErrors.Add(Tuple.Create<string, Exception>(modConfiguration.uniqueID, ex));
      }
      return (ModConfiguration) null;
    }

    internal static void LoadConfig()
    {
      XmlDocument xmlDocument = (XmlDocument) null;
      XmlElement xmlElement = (XmlElement) null;
      bool flag = File.Exists(ModLoader.modConfigFile);
      if (flag)
      {
        try
        {
          xmlDocument = new XmlDocument();
          xmlDocument.Load(ModLoader.modConfigFile);
          xmlElement = xmlDocument["Mods"];
        }
        catch (Exception ex)
        {
          ModLoader.LogModFailure("Failure loading main mod config file. Recreating file.");
          flag = false;
        }
      }
      if (!flag)
      {
        xmlDocument = new XmlDocument();
        xmlElement = xmlDocument.CreateElement("Mods");
        xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("Disabled"));
        xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("CompiledFor"));
        xmlElement["CompiledFor"].InnerText = DG.version;
        xmlDocument.AppendChild((XmlNode) xmlElement);
        xmlDocument.Save(ModLoader.modConfigFile);
      }
      if (xmlElement["Disabled"] != null)
        ModLoader.disabledMods = new HashSet<string>(((IEnumerable<string>) xmlElement["Disabled"].InnerText.Split(new char[1]
        {
          '|'
        }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (a => a.Trim())));
      else
        ModLoader.disabledMods = new HashSet<string>();
      if (xmlElement["CompiledFor"] == null)
        xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("CompiledFor"));
      if (!(xmlElement["CompiledFor"].InnerText != DG.version))
        return;
      ModLoader.forceRecompilation = true;
      xmlElement["CompiledFor"].InnerText = DG.version;
      xmlDocument.Save(ModLoader.modConfigFile);
    }

    internal static void DisabledModsChanged()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(ModLoader.modConfigFile);
      XmlElement xmlElement = xmlDocument["Mods"];
      if (xmlElement["Disabled"] == null)
        xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("Disabled"));
      xmlElement["Disabled"].InnerText = string.Join("|", ModLoader._sortedMods.Where<Mod>((Func<Mod, bool>) (a => a.configuration.disabled)).Select<Mod, string>((Func<Mod, string>) (a => a.configuration.uniqueID)));
      xmlDocument.Save(ModLoader.modConfigFile);
    }

    internal static string modConfigFile => ModLoader.modDirectory + "/mods.conf";

    private static void ResultFetched(object value0, WorkshopQueryResult result)
    {
      if (result == null || result.details == null)
        return;
      WorkshopItem publishedFile = result.details.publishedFile;
      if (publishedFile == null)
        return;
      try
      {
        if ((publishedFile.stateFlags & WorkshopItemState.Installed) == WorkshopItemState.None || !Directory.Exists(publishedFile.path))
          return;
        foreach (string folder in DuckFile.GetDirectoriesNoCloud(publishedFile.path))
        {
          ModConfiguration modConfiguration = ModLoader.AttemptModLoad(folder);
          if (modConfiguration != null)
          {
            try
            {
              modConfiguration.isWorkshop = true;
              ModLoader.loadableMods.Add(modConfiguration.uniqueID, modConfiguration);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    internal static void LoadMods(string dir)
    {
      ModLoader.modDirectory = dir;
      ModLoader.LoadConfig();
      Stack<string> modLoadStack = new Stack<string>();
      ModLoader.loadableMods = new Dictionary<string, ModConfiguration>();
      if (Directory.Exists(ModLoader.modDirectory))
      {
        if (Steam.IsInitialized())
        {
          bool done = false;
          WorkshopQueryUser queryUser = Steam.CreateQueryUser(Steam.user.id, WorkshopList.Subscribed, WorkshopType.UsableInGame, WorkshopSortOrder.TitleAsc);
          queryUser.requiredTags.Add("Mod");
          queryUser.onlyQueryIDs = true;
          queryUser.QueryFinished += (WorkshopQueryFinished) (sender => done = true);
          queryUser.ResultFetched += new WorkshopQueryResultFetched(ModLoader.ResultFetched);
          queryUser.Request();
          while (!done)
          {
            Steam.Update();
            Thread.Sleep(13);
          }
        }
        List<string> directoriesNoCloud = DuckFile.GetDirectoriesNoCloud(ModLoader.modDirectory);
        MonoMain.totalLoadyBits += directoriesNoCloud.Count<string>() * 2;
        foreach (string folder in directoriesNoCloud)
        {
          ModConfiguration modConfiguration = ModLoader.AttemptModLoad(folder);
          MonoMain.loadyBits += 2;
          if (modConfiguration != null)
          {
            if (ModLoader.loadableMods.ContainsKey(modConfiguration.uniqueID))
            {
              if (ModLoader.loadableMods[modConfiguration.uniqueID].disabled && !modConfiguration.disabled)
                ModLoader.loadableMods.Remove(modConfiguration.uniqueID);
              else
                continue;
            }
            ModLoader.loadableMods.Add(modConfiguration.uniqueID, modConfiguration);
          }
        }
      }
      MonoMain.totalLoadyBits += ModLoader.loadableMods.Count * 2;
      int num = 0;
      foreach (ModConfiguration modConfig in ModLoader.loadableMods.Values)
      {
        try
        {
          ModLoader.currentModLoadString = modConfig.name;
          MonoMain.loadMessage = "LOADING MOD " + ModLoader.currentModLoadString;
          ModLoader.GetOrLoad(modConfig, ref modLoadStack, ref ModLoader.loadableMods);
          MonoMain.loadyBits += 2;
          ++num;
          if (num == 10)
          {
            num = 0;
            Thread.Sleep(50);
          }
        }
        catch (Exception ex)
        {
          ModLoader._modLoadErrors.Add(Tuple.Create<string, Exception>(modConfig.uniqueID, ex));
          MonoMain.loadyBits += 2;
        }
      }
      ModLoader._sortedMods = (IList<Mod>) ModLoader._loadedMods.Values.OrderBy<Mod, Priority>((Func<Mod, Priority>) (mod => mod.priority)).ToList<Mod>();
      ModLoader._sortedAccessibleMods = (IList<Mod>) ModLoader._sortedMods.Where<Mod>((Func<Mod, bool>) (mod => !mod.configuration.disabled)).ToList<Mod>();
      foreach (Mod mod in ModLoader._sortedMods.Where<Mod>((Func<Mod, bool>) (a => a.configuration.disabled)))
      {
        if (mod != null && mod.configuration != null)
          ModLoader._loadedMods.Remove(mod.configuration.uniqueID);
      }
      foreach (Mod sortedAccessibleMod in (IEnumerable<Mod>) ModLoader._sortedAccessibleMods)
        sortedAccessibleMod.InvokeOnPreInitialize();
      ModLoader.modHash = ModLoader.GetModHash();
      foreach (Mod sortedAccessibleMod in (IEnumerable<Mod>) ModLoader._sortedAccessibleMods)
      {
        foreach (System.Type type in sortedAccessibleMod.configuration.assembly.GetTypes())
          ModLoader._typesByName[ModLoader.SmallTypeName(type)] = type;
      }
    }

    private static void LogModFailure(string s)
    {
      try
      {
        Program.LogLine("Mod Load Failure (Did not cause crash)\n================================================\n " + s + "\n================================================\n");
      }
      catch (Exception ex)
      {
      }
    }

    internal static void PostLoadMods()
    {
      foreach (Mod sortedAccessibleMod in (IEnumerable<Mod>) ModLoader._sortedAccessibleMods)
        sortedAccessibleMod.InvokeOnPostInitialize();
    }

    /// <summary>
    /// Searches core and mods for a fully qualified or short type name.
    /// </summary>
    /// <param name="typeName">Fully qualified, or short, name of the type.</param>
    /// <returns>The type, or null.</returns>
    internal static System.Type GetType(string typeName)
    {
      if (typeName == null)
        return (System.Type) null;
      if (typeName.LastIndexOf(',') != typeName.IndexOf(','))
        typeName = ModLoader.SmallTypeName(typeName);
      if (typeName == null)
        return (System.Type) null;
      System.Type type;
      return ModLoader._typesByName.TryGetValue(typeName, out type) ? type : System.Type.GetType(typeName);
    }

    /// <summary>Gets a mod from a type.</summary>
    /// <param name="type">The type.</param>
    /// <returns>The mod</returns>
    public static Mod GetModFromType(System.Type type)
    {
      if (type == (System.Type) null)
        return (Mod) null;
      Mod mod;
      return ModLoader._modTypes.TryGetValue(type, out mod) || ModLoader._modAssemblies.TryGetValue(type.Assembly, out mod) ? mod : (Mod) null;
    }
  }
}
