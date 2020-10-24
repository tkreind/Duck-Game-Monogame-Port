// Decompiled with JetBrains decompiler
// Type: DuckGame.CoreMod
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Reflection;

namespace DuckGame
{
  /// <summary>The core "mod", for consistency sake.</summary>
  public sealed class CoreMod : Mod
  {
    /// <summary>The core mod instance, for quick comparisons.</summary>
    public static CoreMod coreMod { get; internal set; }

    internal CoreMod()
    {
      this.configuration = new ModConfiguration();
      this.configuration.assembly = Assembly.GetExecutingAssembly();
      this.configuration.contentManager = ContentManagers.GetContentManager(typeof (DefaultContentManager));
      this.configuration.name = "DuckGame";
      this.configuration.displayName = "Core";
      this.configuration.description = "The core mod for Duck Game content. This is no touchy. Bad. BAD duck. You will break your game, and I swear if you do I'm not picking that up. I just don't have the time these days to pick up after all of your goddamn mistakes. Seriously, just leave it alone. (it okay just do your best)";
      this.configuration.version = new Version(DG.version);
      this.configuration.author = "CORPTRON";
    }

    public override Priority priority => Priority.Inconsequential;
  }
}
