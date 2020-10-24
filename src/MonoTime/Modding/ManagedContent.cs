// Decompiled with JetBrains decompiler
// Type: DuckGame.ManagedContent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DuckGame
{
  public static class ManagedContent
  {
    public static ManagedContentList<Thing> Things = new ManagedContentList<Thing>();
    public static ManagedContentList<AmmoType> AmmoTypes = new ManagedContentList<AmmoType>();
    public static ManagedContentList<DeathCrateSetting> DeathCrateSettings = new ManagedContentList<DeathCrateSetting>();
    public static ManagedContentList<DestroyType> DestroyTypes = new ManagedContentList<DestroyType>();

    private static void InitializeContentSet<T>(ManagedContentList<T> list)
    {
      foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
      {
        foreach (System.Type type in accessibleMod.configuration.contentManager.Compile<T>(accessibleMod))
          list.Add(type);
      }
    }

    public static void InitializeMods()
    {
      MonoMain.loadMessage = "Loading Mods";
      Mod mod;
      AppDomain.CurrentDomain.AssemblyResolve += (ResolveEventHandler) ((s, resolveArgs) => ModLoader._modAssemblyNames.TryGetValue(resolveArgs.Name, out mod) ? mod.configuration.assembly : (Assembly) null);
      ModLoader.AddMod((Mod) (CoreMod.coreMod = new CoreMod()));
      DuckFile.CreatePath(DuckFile.modsDirectory);
      ModLoader.LoadMods(DuckFile.modsDirectory);
      ManagedContent.InitializeContentSet<Thing>(ManagedContent.Things);
      ManagedContent.InitializeContentSet<AmmoType>(ManagedContent.AmmoTypes);
      ManagedContent.InitializeContentSet<DeathCrateSetting>(ManagedContent.DeathCrateSettings);
      ManagedContent.InitializeContentSet<DestroyType>(ManagedContent.DestroyTypes);
      ContentProperties.InitializeBags(ManagedContent.Things.Types);
      ModLoader.PostLoadMods();
    }
  }
}
