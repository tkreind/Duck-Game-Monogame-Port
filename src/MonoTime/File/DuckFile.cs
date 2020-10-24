// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckFile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DuckGame
{
  public class DuckFile
  {
    public const string cloudString = "nq403216_";
    private static string _saveRoot;
    private static string _saveDirectory;
    private static string _levelDirectory;
    private static string _workshopDirectory;
    private static string _onlineLevelDirectory;
    private static string _optionsDirectory;
    private static string _albumDirectory;
    private static string _profileDirectory;
    private static string _challengeDirectory;
    private static string _modsDirectory;
    private static string _scriptsDirectory;
    private static string _customBlockDirectory;
    private static string _downloadedBlockDirectory;
    private static string _customBackgroundDirectory;
    private static string _downloadedBackgroundDirectory;
    private static string _customPlatformDirectory;
    private static string _downloadedPlatformDirectory;
    private static string _customParallaxDirectory;
    private static string _downloadedParallaxDirectory;
    private static string _customArcadeDirectory;
    private static List<string> _allPaths = new List<string>();
    private static Dictionary<char, string> _invalidPathCharConversions = new Dictionary<char, string>()
    {
      {
        '/',
        "!53029662!"
      },
      {
        '\\',
        "!52024921!"
      },
      {
        '?',
        "!54030923!"
      },
      {
        '%',
        "!50395932!"
      },
      {
        '*',
        "!31040256!"
      },
      {
        ':',
        "!40205341!"
      },
      {
        '|',
        "!95302943!"
      },
      {
        '"',
        "!41302950!"
      },
      {
        '<',
        "!21493928!"
      },
      {
        '>',
        "!95828381!"
      },
      {
        '.',
        "!34910294!"
      }
    };
    private static SearchOption _getFilesOption;
    private static Dictionary<uint, string> _conversionGUIDMap = new Dictionary<uint, string>();
    public static volatile bool LegacyLoadLock = false;
    private static bool needsCloudInit = true;
    public static bool cloudOverload = false;
    private static long _specialSteamCloudID = 9052345928876931670;

    public static string saveRoot => DuckFile._saveRoot;

    public static string saveDirectory => DuckFile._saveRoot + DuckFile._saveDirectory;

    public static string levelDirectory => DuckFile.saveDirectory + DuckFile._levelDirectory;

    public static string workshopDirectory => DuckFile.saveDirectory + DuckFile._workshopDirectory;

    public static string onlineLevelDirectory => DuckFile.saveDirectory + DuckFile._onlineLevelDirectory;

    public static string optionsDirectory => DuckFile.saveDirectory + DuckFile._optionsDirectory;

    public static string albumDirectory => DuckFile.saveDirectory + DuckFile._albumDirectory;

    public static string profileDirectory => DuckFile.saveDirectory + DuckFile._profileDirectory;

    public static string challengeDirectory => DuckFile.saveDirectory + DuckFile._challengeDirectory;

    public static string modsDirectory => DuckFile.saveDirectory + DuckFile._modsDirectory;

    public static string scriptsDirectory => DuckFile.saveDirectory + DuckFile._scriptsDirectory;

    public static string customBlockDirectory => DuckFile.saveDirectory + DuckFile._customBlockDirectory;

    public static string downloadedBlockDirectory => DuckFile.saveDirectory + DuckFile._downloadedBlockDirectory;

    public static string customBackgroundDirectory => DuckFile.saveDirectory + DuckFile._customBackgroundDirectory;

    public static string downloadedBackgroundDirectory => DuckFile.saveDirectory + DuckFile._downloadedBackgroundDirectory;

    public static string customPlatformDirectory => DuckFile.saveDirectory + DuckFile._customPlatformDirectory;

    public static string downloadedPlatformDirectory => DuckFile.saveDirectory + DuckFile._downloadedPlatformDirectory;

    public static string customParallaxDirectory => DuckFile.saveDirectory + DuckFile._customParallaxDirectory;

    public static string downloadedParallaxDirectory => DuckFile.saveDirectory + DuckFile._downloadedParallaxDirectory;

    public static string customArcadeDirectory => DuckFile.saveDirectory + DuckFile._customArcadeDirectory;

    public static string GetCustomDownloadDirectory(CustomType t)
    {
      switch (t)
      {
        case CustomType.Block:
          return DuckFile.downloadedBlockDirectory;
        case CustomType.Platform:
          return DuckFile.downloadedPlatformDirectory;
        case CustomType.Background:
          return DuckFile.downloadedBackgroundDirectory;
        case CustomType.Parallax:
          return DuckFile.downloadedParallaxDirectory;
        default:
          return "";
      }
    }

    public static string GetCustomDirectory(CustomType t)
    {
      switch (t)
      {
        case CustomType.Block:
          return DuckFile.customBlockDirectory;
        case CustomType.Platform:
          return DuckFile.customPlatformDirectory;
        case CustomType.Background:
          return DuckFile.customBackgroundDirectory;
        case CustomType.Parallax:
          return DuckFile.customParallaxDirectory;
        default:
          return "";
      }
    }

    public static void Initialize()
    {
      DuckFile._saveRoot = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/";
      DuckFile._saveRoot = DuckFile._saveRoot.Replace('\\', '/');
      DuckFile._saveDirectory = "DuckGame/";
      DuckFile._levelDirectory = "Levels/";
      DuckFile._allPaths.Add(DuckFile._levelDirectory);
      DuckFile._onlineLevelDirectory = "Online/Levels/";
      DuckFile._allPaths.Add(DuckFile._onlineLevelDirectory);
      DuckFile._optionsDirectory = "Options/";
      DuckFile._allPaths.Add(DuckFile._optionsDirectory);
      DuckFile._albumDirectory = "Album/";
      DuckFile._allPaths.Add(DuckFile._albumDirectory);
      DuckFile._profileDirectory = "Profiles/";
      DuckFile._allPaths.Add(DuckFile._profileDirectory);
      DuckFile._challengeDirectory = "ChallengeData/";
      DuckFile._allPaths.Add(DuckFile._challengeDirectory);
      DuckFile._modsDirectory = "Mods/";
      DuckFile._allPaths.Add(DuckFile._modsDirectory);
      DuckFile._scriptsDirectory = "Scripts/";
      DuckFile._allPaths.Add(DuckFile._scriptsDirectory);
      DuckFile._workshopDirectory = "Workshop/";
      DuckFile._allPaths.Add(DuckFile._workshopDirectory);
      DuckFile._customBlockDirectory = "Custom/Blocks/";
      DuckFile._allPaths.Add(DuckFile._customBlockDirectory);
      DuckFile.CreatePath(DuckFile.customBlockDirectory);
      DuckFile._downloadedBlockDirectory = "Custom/Blocks/Downloaded/";
      DuckFile._allPaths.Add(DuckFile._downloadedBlockDirectory);
      DuckFile._customBackgroundDirectory = "Custom/Background/";
      DuckFile._allPaths.Add(DuckFile._customBackgroundDirectory);
      DuckFile.CreatePath(DuckFile.customBackgroundDirectory);
      DuckFile._downloadedBackgroundDirectory = "Custom/Background/Downloaded/";
      DuckFile._allPaths.Add(DuckFile._downloadedBackgroundDirectory);
      DuckFile._customPlatformDirectory = "Custom/Platform/";
      DuckFile._allPaths.Add(DuckFile._customPlatformDirectory);
      DuckFile.CreatePath(DuckFile.customPlatformDirectory);
      DuckFile._downloadedPlatformDirectory = "Custom/Platform/Downloaded/";
      DuckFile._allPaths.Add(DuckFile._downloadedPlatformDirectory);
      DuckFile._customParallaxDirectory = "Custom/Parallax/";
      DuckFile._allPaths.Add(DuckFile._customParallaxDirectory);
      DuckFile.CreatePath(DuckFile.customParallaxDirectory);
      DuckFile._downloadedParallaxDirectory = "Custom/Parallax/Downloaded/";
      DuckFile._allPaths.Add(DuckFile._downloadedParallaxDirectory);
      DuckFile._customArcadeDirectory = "Custom/Arcade/";
      DuckFile._allPaths.Add(DuckFile.customArcadeDirectory);
      DuckFile.CreatePath(DuckFile.customArcadeDirectory);
    }

    public static void DeleteAllSaveData()
    {
      foreach (string allPath in DuckFile._allPaths)
      {
        foreach (string file in DuckFile.GetFiles(DuckFile.saveDirectory + allPath))
          DuckFile.Delete(file);
      }
    }

    public static void CreatePath(string pathString)
    {
      pathString = pathString.Replace('\\', '/');
      string[] strArray = pathString.Split('/');
      string str = "";
      for (int index = 0; index < ((IEnumerable<string>) strArray).Count<string>(); ++index)
      {
        if (!(strArray[index] == "") && !(strArray[index] == "/") && (!strArray[index].Contains<char>('.') || index != ((IEnumerable<string>) strArray).Count<string>() - 1))
        {
          string path = str + strArray[index];
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          str = path + "/";
        }
      }
    }

    public static FileStream Create(string path)
    {
      DuckFile.CreatePath(path);
      return File.Create(path);
    }

    public static string GetShortPath(string path)
    {
      path.Replace('\\', '/');
      string saveDirectory = DuckFile.saveDirectory;
      int num = path.IndexOf(saveDirectory);
      return num != -1 ? path.Substring(num + saveDirectory.Length, path.Length - num - saveDirectory.Length) : path;
    }

    public static string GetLocalSavePath(string path)
    {
      path.Replace('\\', '/');
      string saveDirectory = DuckFile.saveDirectory;
      int num = path.IndexOf(saveDirectory);
      return num != -1 ? path.Substring(num + saveDirectory.Length, path.Length - num - saveDirectory.Length) : (string) null;
    }

    public static string GetShortDirectory(string path)
    {
      path.Replace('\\', '/');
      string saveRoot = DuckFile.saveRoot;
      int num = path.IndexOf(saveRoot);
      return num != -1 ? path.Substring(num + saveRoot.Length, path.Length - num - saveRoot.Length) : path;
    }

    public static List<string> GetFilesNoCloud(string path, string filter = "*.*", SearchOption so = SearchOption.TopDirectoryOnly)
    {
      List<string> stringList = new List<string>();
      try
      {
        foreach (string file in Directory.GetFiles(path, filter, SearchOption.TopDirectoryOnly))
          stringList.Add(file);
      }
      catch (Exception ex)
      {
      }
      if (so == SearchOption.AllDirectories)
      {
        try
        {
          foreach (string directory in Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly))
          {
            List<string> filesNoCloud = DuckFile.GetFilesNoCloud(directory, filter, so);
            stringList.AddRange((IEnumerable<string>) filesNoCloud);
          }
        }
        catch (Exception ex)
        {
        }
      }
      return stringList;
    }

    public static List<string> GetDirectoriesNoCloud(string path, string filter = "*.*")
    {
      List<string> stringList = new List<string>();
      try
      {
        foreach (string directory in Directory.GetDirectories(path, filter, SearchOption.TopDirectoryOnly))
          stringList.Add(directory);
      }
      catch (Exception ex)
      {
      }
      return stringList;
    }

    public static string[] GetFiles(string path, string filter, SearchOption option = SearchOption.TopDirectoryOnly)
    {
      DuckFile._getFilesOption = option;
      string[] files = DuckFile.GetFiles(path, filter);
      DuckFile._getFilesOption = SearchOption.TopDirectoryOnly;
      return files;
    }

    public static string[] GetFiles(string path, string filter = "*.*")
    {
      List<string> stringList = new List<string>();
      if (!MonoMain.cloudOnly && Directory.Exists(path))
      {
        stringList = DuckFile.GetFilesNoCloud(path, filter, DuckFile._getFilesOption);
        for (int index = 0; index < stringList.Count; ++index)
          stringList[index] = stringList[index].Replace('\\', '/');
      }
      if (Options.Data.cloud && !MonoMain.disableCloud && Steam.IsInitialized())
      {
        string localSavePath = DuckFile.GetLocalSavePath(path);
        if (localSavePath != null)
        {
          int count = Steam.FileGetCount();
          for (int file = 0; file < count; ++file)
          {
            string name = Steam.FileGetName(file);
            int num = name.IndexOf(localSavePath);
            if (num != -1 && name.StartsWith("nq403216_") && num == "nq403216_".Length)
            {
              string source = name.Substring(num + localSavePath.Length, name.Length - (num + localSavePath.Length));
              if (source[0] == '\\' || source[0] == '/')
                source = source.Substring(1, source.Length - 1);
              if (!source.Contains<char>('/') && !source.Contains<char>('\\') || DuckFile._getFilesOption == SearchOption.AllDirectories)
              {
                string str = path;
                if (!str.EndsWith("/"))
                  str += "/";
                if (!stringList.Contains(str + source))
                  stringList.Add(path + source);
              }
            }
          }
        }
      }
      return stringList.ToArray();
    }

    public static string[] GetDirectories(string path)
    {
      path = path.Replace('\\', '/');
      List<string> stringList = new List<string>();
      if (Options.Data.cloud && !MonoMain.disableCloud && Steam.IsInitialized())
      {
        string localSavePath = DuckFile.GetLocalSavePath(path);
        if (localSavePath != null)
        {
          int count = Steam.FileGetCount();
          for (int file = 0; file < count; ++file)
          {
            string name = Steam.FileGetName(file);
            int num = name.IndexOf(localSavePath);
            if (num != -1 && name.StartsWith("nq403216_"))
            {
              if (localSavePath == "")
                num += 12;
              string str1 = name.Substring(num + localSavePath.Length, name.Length - (num + localSavePath.Length));
              int length = str1.IndexOf('/');
              switch (length)
              {
                case -1:
                case 0:
                  continue;
                default:
                  string str2 = path + str1.Substring(0, length);
                  if (!stringList.Contains(str2))
                  {
                    stringList.Add(str2);
                    continue;
                  }
                  continue;
              }
            }
          }
        }
      }
      path = path.Trim('/');
      if (!MonoMain.cloudOnly && Directory.Exists(path))
      {
        foreach (string path1 in DuckFile.GetDirectoriesNoCloud(path))
        {
          if (!Path.GetFileName(path1).Contains("._"))
          {
            string str = path1.Replace('\\', '/');
            if (!stringList.Contains(str))
              stringList.Add(str);
          }
        }
      }
      return stringList.ToArray();
    }

    public static string ReplaceInvalidCharacters(string path)
    {
      string str1 = "";
      foreach (char key in path)
      {
        string str2 = "";
        str1 = !DuckFile._invalidPathCharConversions.TryGetValue(key, out str2) ? str1 + (object) key : str1 + str2;
      }
      return str1;
    }

    public static string RestoreInvalidCharacters(string path)
    {
      foreach (KeyValuePair<char, string> pathCharConversion in DuckFile._invalidPathCharConversions)
        path = path.Replace(pathCharConversion.Value, string.Concat((object) pathCharConversion.Key));
      return path;
    }

    public static LevelData LoadLevel(string path)
    {
      DuckFile.CreatePath(Path.GetDirectoryName(path));
      DuckFile.PrepareToLoadCloudFile(path);
      return !File.Exists(path) ? (LevelData) null : DuckFile.LoadLevel(File.ReadAllBytes(path));
    }

    private static LevelData ConvertLevel(byte[] data)
    {
      LevelData levelData = (LevelData) null;
      Editor editor = new Editor();
      bool skipInitialize = Level.skipInitialize;
      Level.skipInitialize = true;
      Level currentLevel = Level.core.currentLevel;
      Level.core.currentLevel = (Level) editor;
      try
      {
        editor.minimalConversionLoad = true;
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
          using (XmlTextReader xmlTextReader = new XmlTextReader((Stream) memoryStream))
          {
            XDocument doc = XDocument.Load((XmlReader) xmlTextReader);
            editor.LegacyLoadLevelParts(doc);
            editor.things.RefreshState();
          }
        }
        levelData = editor.CreateSaveData();
        if (!editor.hadGUID)
        {
          uint key = Editor.Checksum(data);
          string str = (string) null;
          if (!DuckFile._conversionGUIDMap.TryGetValue(key, out str))
          {
            str = levelData.metaData.guid;
            DuckFile._conversionGUIDMap[key] = str;
          }
          levelData.metaData.guid = str;
        }
      }
      catch
      {
      }
      Level.core.currentLevel = currentLevel;
      Level.skipInitialize = skipInitialize;
      return levelData;
    }

    public static LevelData LoadLevel(byte[] data)
    {
      LevelData levelData = BinaryClassChunk.FromData<LevelData>(new BitBuffer(data, false));
      if (levelData != null && levelData.GetResult() != DeserializeResult.InvalidMagicNumber)
        return levelData;
      Promise<LevelData> promise = Tasker.Task<LevelData>((Func<LevelData>) (() => DuckFile.ConvertLevel(data)));
      promise.WaitForComplete();
      return promise.Result;
    }

    public static XDocument LoadXDocument(string path)
    {
      DuckFile.CreatePath(Path.GetDirectoryName(path));
      DuckFile.PrepareToLoadCloudFile(path);
      if (!File.Exists(path))
        return (XDocument) null;
      try
      {
        return XDocument.Load(path);
      }
      catch
      {
        return (XDocument) null;
      }
    }

    public static void SaveXDocument(XDocument doc, string path)
    {
      DuckFile.CreatePath(Path.GetDirectoryName(path));
      try
      {
        if (File.Exists(path))
          File.SetAttributes(path, FileAttributes.Normal);
      }
      catch (Exception ex)
      {
      }
      string contents = doc.ToString();
      if (string.IsNullOrWhiteSpace(contents))
        throw new Exception("Blank XML (" + path + ")");
      File.WriteAllText(path, contents);
      DuckFile.SaveCloudFile(path);
    }

    public static T LoadChunk<T>(string path) where T : BinaryClassChunk
    {
      DuckFile.CreatePath(Path.GetDirectoryName(path));
      DuckFile.PrepareToLoadCloudFile(path);
      return !File.Exists(path) ? default (T) : BinaryClassChunk.FromData<T>(new BitBuffer(File.ReadAllBytes(path), 0, false));
    }

    public static void SaveChunk(BinaryClassChunk doc, string path)
    {
      DuckFile.CreatePath(Path.GetDirectoryName(path));
      try
      {
        if (File.Exists(path))
          File.SetAttributes(path, FileAttributes.Normal);
      }
      catch (Exception ex)
      {
      }
      BitBuffer bitBuffer = doc.Serialize();
      FileStream fileStream = File.Create(path);
      fileStream.Write(bitBuffer.buffer, 0, bitBuffer.lengthInBytes);
      fileStream.Close();
      DuckFile.SaveCloudFile(path);
    }

    public static void UploadAllCloudData()
    {
      foreach (string path in DuckFile.GetFilesNoCloud(DuckFile.profileDirectory, so: SearchOption.AllDirectories))
        DuckFile.SaveCloudFile(path);
      foreach (string path in DuckFile.GetFilesNoCloud(DuckFile.levelDirectory, so: SearchOption.AllDirectories))
        DuckFile.SaveCloudFile(path);
      foreach (string path in DuckFile.GetFilesNoCloud(DuckFile.optionsDirectory, so: SearchOption.AllDirectories))
        DuckFile.SaveCloudFile(path);
    }

    public static void DownloadAllCloudData()
    {
      bool cloudOnly = MonoMain.cloudOnly;
      bool cloud = Options.Data.cloud;
      MonoMain.cloudOnly = true;
      Options.Data.cloud = true;
      foreach (string file in DuckFile.GetFiles(DuckFile.profileDirectory, "*.*", SearchOption.AllDirectories))
      {
        DuckFile.CreatePath(file);
        DuckFile.PrepareToLoadCloudFile(file);
      }
      foreach (string file in DuckFile.GetFiles(DuckFile.levelDirectory, "*.*", SearchOption.AllDirectories))
      {
        DuckFile.CreatePath(file);
        DuckFile.PrepareToLoadCloudFile(file);
      }
      foreach (string file in DuckFile.GetFiles(DuckFile.optionsDirectory, "*.*", SearchOption.AllDirectories))
      {
        DuckFile.CreatePath(file);
        DuckFile.PrepareToLoadCloudFile(file);
      }
      MonoMain.cloudOnly = cloudOnly;
      Options.Data.cloud = cloud;
    }

    public static void DeleteAllCloudData()
    {
      bool cloudOnly = MonoMain.cloudOnly;
      bool cloud = Options.Data.cloud;
      MonoMain.cloudOnly = true;
      Options.Data.cloud = true;
      for (int count = Steam.FileGetCount(); count > 0; count = Steam.FileGetCount())
        Steam.FileDelete(Steam.FileGetName(0));
      MonoMain.cloudOnly = cloudOnly;
      Options.Data.cloud = cloud;
    }

    public static void InitializeCloud()
    {
      if (!Steam.IsInitialized())
        return;
      try
      {
        string localSavePath = DuckFile.GetLocalSavePath(DuckFile.optionsDirectory + "options.dat");
        if (localSavePath == null)
          return;
        if (!Steam.FileExists("nq403216_" + localSavePath))
          return;
        try
        {
          XDocument xdocument = XDocument.Load((Stream) new MemoryStream(Steam.FileRead("nq403216_" + localSavePath)));
          if (xdocument != null)
          {
            Profile profile = new Profile("");
            IEnumerable<XElement> source = xdocument.Elements((XName) "Data");
            if (source != null)
            {
              foreach (XElement element in source.Elements<XElement>())
              {
                if (element.Name.LocalName == "Options")
                {
                  OptionsData optionsData = new OptionsData();
                  optionsData.Deserialize(element);
                  if (optionsData.cloud)
                    DuckFile.cloudOverload = true;
                  Options.Data.cloud = optionsData.cloud;
                  break;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
        DuckFile.needsCloudInit = false;
      }
      catch (Exception ex)
      {
      }
    }

    public static void CompleteSteamCloudInitializate()
    {
      if (!DuckFile.needsCloudInit)
        return;
      DuckFile.UploadAllCloudData();
    }

    private static void PrepareToLoadCloudFile(string path)
    {
      if (!Options.Data.cloud || !Steam.IsInitialized())
        return;
      string localSavePath = DuckFile.GetLocalSavePath(path);
      if (localSavePath == null)
        return;
      byte[] buffer = Steam.FileRead("nq403216_" + localSavePath);
      if (buffer == null)
        return;
      if (File.Exists(path))
        File.Delete(path);
      FileStream fileStream = File.Create(path);
      fileStream.Write(buffer, 0, buffer.Length);
      fileStream.Close();
    }

    private static void SaveCloudFile(string path)
    {
      if (MonoMain.cloudNoSave || !Steam.IsInitialized())
        return;
      string localSavePath = DuckFile.GetLocalSavePath(path);
      if (localSavePath == null)
        return;
      byte[] data = File.ReadAllBytes(path);
      Steam.FileWrite("nq403216_" + localSavePath, data, data.Length);
    }

    public static void DeleteFolder(string folder)
    {
      if (!Directory.Exists(folder))
        return;
      foreach (string directory in DuckFile.GetDirectories(folder))
        DuckFile.DeleteFolder(directory);
      foreach (string file in DuckFile.GetFiles(folder))
        DuckFile.Delete(file);
      Directory.Delete(folder);
    }

    public static void Delete(string file)
    {
      if (File.Exists(file))
      {
        File.SetAttributes(file, FileAttributes.Normal);
        File.Delete(file);
      }
      if (MonoMain.cloudNoSave || !Steam.IsInitialized())
        return;
      string localSavePath = DuckFile.GetLocalSavePath(file);
      if (localSavePath == null)
        return;
      Steam.FileDelete(localSavePath);
    }

    public static byte[] ReadEntireStream(Stream stream)
    {
      long num1 = 0;
      if (stream.CanSeek)
      {
        num1 = stream.Position;
        stream.Position = 0L;
      }
      try
      {
        byte[] buffer = new byte[4096];
        int length = 0;
        int num2;
        while ((num2 = stream.Read(buffer, length, buffer.Length - length)) > 0)
        {
          length += num2;
          if (length == buffer.Length)
          {
            int num3 = stream.ReadByte();
            if (num3 != -1)
            {
              byte[] numArray = new byte[buffer.Length * 2];
              Buffer.BlockCopy((Array) buffer, 0, (Array) numArray, 0, buffer.Length);
              Buffer.SetByte((Array) numArray, length, (byte) num3);
              buffer = numArray;
              ++length;
            }
          }
        }
        byte[] numArray1 = buffer;
        if (buffer.Length != length)
        {
          numArray1 = new byte[length];
          Buffer.BlockCopy((Array) buffer, 0, (Array) numArray1, 0, length);
        }
        return numArray1;
      }
      finally
      {
        if (stream.CanSeek)
          stream.Position = num1;
      }
    }
  }
}
