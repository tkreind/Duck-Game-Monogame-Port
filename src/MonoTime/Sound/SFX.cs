// Decompiled with JetBrains decompiler
// Type: DuckGame.SFX
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DuckGame
{
  public static class SFX
  {
    private const int kMaxSounds = 32;
    private static Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, MultiSoundUpdater> _multiSounds = new Dictionary<string, MultiSoundUpdater>();
    private static List<Sound> _soundPool = new List<Sound>();
    private static List<Sound> _playedThisFrame = new List<Sound>();
    private static float _volume = 1f;

    public static Dictionary<string, SoundEffect> sounds => SFX._sounds;

    public static bool PoolSound(Sound s)
    {
      if (SFX._soundPool.Count > 32)
      {
        bool flag = false;
        for (int index = 0; index < SFX._soundPool.Count; ++index)
        {
          if (!SFX._soundPool[index].cannotBeCancelled)
          {
            SFX.UnpoolSound(SFX._soundPool[index]);
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      SFX._soundPool.Add(s);
      return true;
    }

    public static void UnpoolSound(Sound s)
    {
      SFX._soundPool.Remove(s);
      s.Unpooled();
    }

    public static void Initialize()
    {
      MonoMain.loadMessage = "Loading SFX";
      SFX.SearchDir("Content/Audio/SFX");
    }

    public static void Update()
    {
      SFX._playedThisFrame.Clear();
      for (int index = 0; index < SFX._soundPool.Count; ++index)
      {
        if (SFX._soundPool[index].State != SoundState.Playing)
        {
          SFX._soundPool[index].Stop();
          --index;
        }
      }
      foreach (KeyValuePair<string, MultiSoundUpdater> multiSound in SFX._multiSounds)
        multiSound.Value.Update();
    }

    public static float volume
    {
      get => Math.Min(1f, Math.Max(0.0f, SFX._volume));
      set => SFX._volume = Math.Min(1f, Math.Max(0.0f, value));
    }

    public static Sound Play(string sound, float vol = 1f, float pitch = 0.0f, float pan = 0.0f, bool looped = false)
    {
      Sound sound1 = SFX._playedThisFrame.FirstOrDefault<Sound>((Func<Sound, bool>) (x => x.name == sound));
      if (sound1 == null)
      {
        try
        {
          sound1 = SFX.Get(sound, vol, pitch, pan, looped);
          if (sound1 != null)
          {
            sound1.Play();
            SFX._playedThisFrame.Add(sound1);
          }
        }
        catch (Exception ex)
        {
          return new Sound(SFX._sounds.FirstOrDefault<KeyValuePair<string, SoundEffect>>().Key, 0.0f, 0.0f, 0.0f, false);
        }
      }
      return sound1;
    }

    public static bool HasSound(string sound)
    {
      SoundEffect soundEffect = (SoundEffect) null;
      if (!SFX._sounds.TryGetValue(sound, out soundEffect))
      {
        soundEffect = Content.Load<SoundEffect>("Audio/SFX/" + sound);
        if (soundEffect == null && MonoMain.moddingEnabled && ModLoader.modsEnabled)
          soundEffect = Content.Load<SoundEffect>(sound);
        lock (SFX._sounds)
          SFX._sounds[sound] = soundEffect;
      }
      return soundEffect != null;
    }

    public static Sound Get(string sound, float vol = 1f, float pitch = 0.0f, float pan = 0.0f, bool looped = false)
    {
      try
      {
        float vol1 = Math.Min(1f, Math.Max(0.0f, vol));
        return SFX.HasSound(sound) ? new Sound(sound, vol1, pitch, pan, looped) : (Sound) new InvalidSound(sound, vol1, pitch, pan, looped);
      }
      catch (Exception ex)
      {
        return (Sound) new InvalidSound(sound, 0.0f, pitch, pan, looped);
      }
    }

    public static MultiSound GetMultiSound(string single, string multi)
    {
      if (SFX._multiSounds.ContainsKey(single + multi))
        return SFX._multiSounds[single + multi].GetInstance();
      MultiSoundUpdater multiSoundUpdater = SFX.HasSound(single) && SFX.HasSound(multi) ? new MultiSoundUpdater(single + multi, single, multi) : throw new Exception("Tried to get non existing sound effect \"" + single + "\"- \"" + multi + "\"");
      SFX._multiSounds[single + multi] = multiSoundUpdater;
      return multiSoundUpdater.GetInstance();
    }

    public static SoundEffectInstance GetInstance(
      string sound,
      float vol = 1f,
      float pitch = 0.0f,
      float pan = 0.0f,
      bool looped = false)
    {
      float num = Math.Min(1f, Math.Max(0.0f, vol));
      SoundEffectInstance instance = SFX._sounds[sound].CreateInstance();
      instance.Volume = num;
      instance.Pitch = pitch;
      instance.Pan = pan;
      instance.IsLooped = looped;
      return instance;
    }

    private static void SearchDir(string dir)
    {
      foreach (string file in Content.GetFiles(dir))
        SFX.ProcessSoundEffect(file);
      foreach (string directory in Content.GetDirectories(dir))
        SFX.SearchDir(directory);
    }

    public static void StopAllSounds()
    {
      while (SFX._soundPool.Count > 0)
        SFX._soundPool[0].Stop();
    }

    public static void KillAllSounds()
    {
      while (SFX._soundPool.Count > 0)
        SFX._soundPool[0].Stop();
    }

    private static void ProcessSoundEffect(string path)
    {
      if (MonoMain.lockLoading)
      {
        MonoMain.loadingLocked = true;
        while (MonoMain.lockLoading)
          Thread.Sleep(10);
      }
      MonoMain.loadingLocked = false;
      path = path.Replace('\\', '/');
      int num = path.IndexOf("Content/Audio/", 0);
      string str = path.Substring(num + 8);
      string name = str.Substring(0, str.Length - 4);
      SoundEffect soundEffect = Content.Load<SoundEffect>(name);
      if (soundEffect != null)
      {
        string key = name.Substring(name.IndexOf("/SFX/") + 5);
        lock (SFX._sounds)
          SFX._sounds[key] = soundEffect;
      }
      ++MonoMain.lazyLoadyBits;
    }
  }
}
