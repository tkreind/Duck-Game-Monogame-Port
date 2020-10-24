// Decompiled with JetBrains decompiler
// Type: DuckGame.ContentPack
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DuckGame
{
  public class ContentPack
  {
    private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
    private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
    private ModConfiguration _modConfig;

    public ContentPack(ModConfiguration modConfiguration) => this._modConfig = modConfiguration;

    public void ImportAsset(string path, byte[] data)
    {
      try
      {
        string key = path.Substring(0, path.Length - 4);
        if (path.EndsWith(".png"))
        {
          Texture2D texture2D = TextureConverter.LoadPNGWithPinkAwesomeness(DuckGame.Graphics.device, new Bitmap((Stream) new MemoryStream(data)), true);
          this._textures[key] = texture2D;
          Content.textures[key] = (Tex2D) texture2D;
        }
        else
        {
          if (!path.EndsWith(".wav"))
            return;
          SoundEffect soundEffect = SoundEffect.FromStream((Stream) new MemoryStream(data));
          this._sounds[key] = soundEffect;
          SFX.sounds[key] = soundEffect;
        }
      }
      catch (Exception ex)
      {
      }
    }

    /// <summary>
    /// Called when the mod is loaded to preload content. This is only called if preload is set to true.
    /// </summary>
    public virtual void PreloadContent()
    {
      List<string> files = Content.GetFiles<Texture2D>(this._modConfig.contentDirectory);
      int length = this._modConfig.contentDirectory.Length;
      foreach (string file in files)
      {
        Texture2D texture2D = ContentPack.LoadTexture2DInternal(file);
        string key = file.Substring(0, file.Length - 4);
        this._textures[key] = texture2D;
        Content.textures[key] = (Tex2D) texture2D;
      }
      foreach (string file in Content.GetFiles<SoundEffect>(this._modConfig.contentDirectory))
      {
        SoundEffect soundEffect = this.LoadSoundInternal(file);
        string key = file.Substring(0, file.Length - 4);
        this._sounds[key] = soundEffect;
        SFX.sounds[key] = soundEffect;
      }
      foreach (string file in Content.GetFiles<Song>(this._modConfig.contentDirectory))
      {
        Song song = this.LoadSongInternal(file);
        this._songs[file.Substring(0, file.Length - 4)] = song;
      }
    }

    /// <summary>
    /// Called when the mod is loaded to preload the paths to all content. Does not actually load content, and is only called if PreloadContent is disabled.
    /// </summary>
    public virtual void PreloadContentPaths()
    {
      List<string> files = Content.GetFiles<Texture2D>(this._modConfig.contentDirectory);
      int length = this._modConfig.contentDirectory.Length;
      foreach (string file in files)
      {
        Texture2D texture2D = ContentPack.LoadTexture2DInternal(file);
        string key = file.Substring(0, file.Length - 4);
        this._textures[key] = texture2D;
        Content.textures[key] = (Tex2D) texture2D;
      }
      foreach (string file in Content.GetFiles<SoundEffect>(this._modConfig.contentDirectory))
      {
        SoundEffect soundEffect = this.LoadSoundInternal(file);
        string key = file.Substring(0, file.Length - 4);
        this._sounds[key] = soundEffect;
        SFX.sounds[key] = soundEffect;
      }
      foreach (string file in Content.GetFiles<Song>(this._modConfig.contentDirectory))
      {
        Song song = this.LoadSongInternal(file);
        this._songs[file.Substring(0, file.Length - 4)] = song;
      }
    }

    private static Texture2D LoadTexture2DInternal(string file, bool processPink = true)
    {
      Texture2D texture2D = (Texture2D) null;
      try
      {
        texture2D = TextureConverter.LoadPNGWithPinkAwesomeness(DuckGame.Graphics.device, file, processPink);
      }
      catch
      {
      }
      return texture2D;
    }

    public static Texture2D LoadTexture2D(string name, bool processPink = true)
    {
      Texture2D texture2D = (Texture2D) null;
      if (!name.EndsWith(".png"))
        name += ".png";
      if (File.Exists(name))
        texture2D = ContentPack.LoadTexture2DInternal(name);
      return texture2D;
    }

    internal SoundEffect LoadSoundInternal(string file)
    {
      SoundEffect soundEffect = (SoundEffect) null;
      try
      {
        soundEffect = SoundEffect.FromStream((Stream) new MemoryStream(File.ReadAllBytes(file)));
      }
      catch
      {
      }
      return soundEffect;
    }

    internal SoundEffect LoadSoundEffect(string name)
    {
      SoundEffect soundEffect = (SoundEffect) null;
      if (!name.EndsWith(".wav"))
        name += ".wav";
      if (File.Exists(name))
        soundEffect = this.LoadSoundInternal(name);
      return soundEffect;
    }

    internal Song LoadSongInternal(string file)
    {
      Song song = (Song) null;
      try
      {
        MemoryStream dat = OggSong.Load(file, false);
        if (dat != null)
          song = new Song(dat, file);
      }
      catch
      {
      }
      return song;
    }

    internal Song LoadSong(string name)
    {
      Song song = (Song) null;
      if (!name.EndsWith(".ogg"))
        name += ".ogg";
      if (File.Exists(name))
        song = this.LoadSongInternal(name);
      return song;
    }

    /// <summary>
    /// Loads content from the content pack. Currently supports Texture2D(png) and SoundEffect(wav) in
    /// "mySound" "customSounds/mySound" path format. You should usually use Content.Load&lt;&gt;().
    /// </summary>
    public T Load<T>(string name)
    {
      if (typeof (T) == typeof (Texture2D))
      {
        Texture2D texture2D1 = (Texture2D) null;
        if (this._textures.TryGetValue(name, out texture2D1))
          return (T)(Object)texture2D1;
        Texture2D texture2D2 = ContentPack.LoadTexture2D(name, this._modConfig.processPinkTransparency);
        this._textures[name] = texture2D2;
        return (T)(Object)texture2D2;
      }
      if (typeof (T) == typeof (SoundEffect))
      {
        SoundEffect soundEffect1 = (SoundEffect) null;
        if (this._sounds.TryGetValue(name, out soundEffect1))
          return (T)(Object)soundEffect1;
        SoundEffect soundEffect2 = this.LoadSoundEffect(name);
        this._sounds[name] = soundEffect2;
        return (T)(Object)soundEffect2;
      }
      if (!(typeof (T) == typeof (Song)))
        return default (T);
      Song song1 = (Song) null;
      if (this._songs.TryGetValue(name, out song1))
        return (T)(Object)song1;
      Song song2 = this.LoadSong(name);
      this._songs[name] = song2;
      return (T)(Object)song2;
    }
  }
}
