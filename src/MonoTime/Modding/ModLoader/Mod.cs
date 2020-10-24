// Decompiled with JetBrains decompiler
// Type: DuckGame.Mod
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Diagnostics;
using System.IO;

namespace DuckGame
{
  /// <summary>
  /// The base class for mod information. Each mod has a custom instance of this class.
  /// </summary>
  public abstract class Mod
  {
    /// <summary>
    /// The property bag for this mod. Other mods may view and read from this collection.
    /// You must not edit this bag while the game is running, only during mod initialization.
    /// </summary>
    protected readonly PropertyBag _properties = new PropertyBag();
    private Tex2D _previewTexture;
    private Tex2D _screenshot;

    /// <summary>
    /// Returns a formatted path that leads to the "asset" parameter in a given mod.
    /// </summary>
    public static string GetPath<T>(string asset) where T : Mod => ModLoader.GetMod<T>().configuration.contentDirectory + asset.Replace('\\', '/');

    /// <summary>
    /// Returns a formatted path that leads to the "asset" parameter in this mod.
    /// </summary>
    public string GetPath(string asset) => this.configuration.contentDirectory + asset.Replace('\\', '/');

    /// <summary>
    /// The read-only property bag that this mod was initialized with.
    /// </summary>
    public IReadOnlyPropertyBag properties => (IReadOnlyPropertyBag) this._properties;

    /// <summary>The configuration class for this mod</summary>
    public ModConfiguration configuration { get; internal set; }

    /// <summary>The priority of this mod as compared to other mods.</summary>
    /// <value>The priority.</value>
    public virtual Priority priority => Priority.Normal;

    /// <summary>Gets the preview texture for this mod.</summary>
    /// <value>The preview texture.</value>
    public virtual Tex2D previewTexture
    {
      get
      {
        if (this._previewTexture == null)
        {
          if (this.configuration.loaded)
          {
            if (this.configuration.contentDirectory != null)
              this._previewTexture = Content.Load<Tex2D>(this.GetPath("preview"));
            if (this._previewTexture == null)
              this._previewTexture = Content.Load<Tex2D>("notexture");
          }
          else
            this._previewTexture = Content.Load<Tex2D>("none");
        }
        return this._previewTexture;
      }
      protected set => this._previewTexture = value;
    }

    /// <summary>Gets path for screenshot.png from Content folder.</summary>
    /// <value>Path for mod screenshot.png from Content folder.</value>
    public virtual Tex2D screenshot
    {
      get
      {
        if (this._screenshot == null)
        {
          if (this.configuration.loaded)
          {
            if (this.configuration.contentDirectory != null)
            {
              string str = this.GetPath(nameof (screenshot)) + ".png";
              if (File.Exists(str))
                this._screenshot = Content.Load<Tex2D>(str);
            }
            if (this._screenshot == null)
              this._screenshot = Content.Load<Tex2D>("defaultMod");
          }
          else
            this._screenshot = (Tex2D) null;
        }
        return this._screenshot;
      }
    }

    /// <summary>
    /// Called on a mod when all mods and the core are finished being created
    /// and are ready to be initialized. You may use game functions and Reflection
    /// in here safely. Note that during this method, not all mods may have ran
    /// their pre-initialization routines and may not have sent their content to
    /// the core. Ideally, you will want to set up your properties here.
    /// </summary>
    protected virtual void OnPreInitialize()
    {
    }

    /// <summary>
    /// Called on a mod after all mods have finished their pre-initialization
    /// and have sent their content to the core.
    /// </summary>
    protected virtual void OnPostInitialize()
    {
    }

    internal void InvokeOnPreInitialize() => this.OnPreInitialize();

    internal void InvokeOnPostInitialize() => this.OnPostInitialize();

    /// <summary>Provides some mod debugging logic</summary>
    public static class Debug
    {
      /// <summary>
      /// Logs the specified line to any attached debuggers.
      /// If "-moddebug" is specified this will also output
      /// to the dev console in ~
      /// </summary>
      /// <param name="format">The format.</param>
      /// <param name="objs">The format parameters.</param>
      public static void Log(string format, params object[] objs)
      {
        if (!MonoMain.modDebugging)
          return;
        string text = string.Format(format, objs);
        if (Debugger.IsAttached)
          Debugger.IsLogging();
        DevConsole.Log(DCSection.Mod, text);
      }
    }
  }
}
