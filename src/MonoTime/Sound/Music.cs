// Decompiled with JetBrains decompiler
// Type: DuckGame.Music
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class Music
  {
    private static Dictionary<string, MemoryStream> _songs = new Dictionary<string, MemoryStream>();
    private static OggPlayer[] _players = new OggPlayer[2];
    private static OggPlayer _specialPlayer;
    private static int _curPlayer = 0;
    private static Dictionary<string, Queue<string>> _recentSongs = new Dictionary<string, Queue<string>>();
    private static float _fadeSpeed;
    private static float _volume = 1f;
    private static float _volumeMult = 1f;
    private static float _masterVolume = 0.65f;
    private static string _currentSong = "";
    private static string _pendingSong = "";
    private static bool _alternateLoop = false;
    private static string _alternateSong = "";
    private static string _swapSong = "";

    public static Dictionary<string, MemoryStream> songs => Music._songs;

    public static void Reset() => Music._recentSongs.Clear();

    public static bool stopped => Music._players[Music._curPlayer].state == SoundState.Stopped || Music._players[Music._curPlayer].state == SoundState.Paused;

    public static float volumeMult
    {
      get => Music._volumeMult;
      set
      {
        Music._volumeMult = value;
        Music.volume = Music._volume;
      }
    }

    public static float volume
    {
      get => Music._volume;
      set
      {
        Music._volume = value;
        Music._players[0].volume = Music._volume * Music._masterVolume * Music._volumeMult;
        Music._players[1].volume = Music._volume * Music._masterVolume * Music._volumeMult;
        Music._specialPlayer.volume = Music._volume * Music._masterVolume * Music._volumeMult;
      }
    }

    public static float masterVolume
    {
      get => Music._masterVolume;
      set
      {
        Music._masterVolume = value;
        Music.volume = Music._volume;
      }
    }

    public static string currentSong => Music._currentSong;

    public static string pendingSong => Music._pendingSong;

    public static TimeSpan position => Music._players[Music._curPlayer].position;

    public static bool finished => Music._players[Music._curPlayer] == null || Music._players[Music._curPlayer].state == SoundState.Stopped;

    public static void Initialize()
    {
      MonoMain.loadMessage = "Loading Music";
      Music._players[0] = new OggPlayer();
      Music._players[1] = new OggPlayer();
      Music._specialPlayer = new OggPlayer();
      Music.SearchDir("Content/Audio/Music");
    }

    public static void Terminate()
    {
      if (Music._players[0] != null)
        Music._players[0].Terminate();
      if (Music._players[1] != null)
        Music._players[1].Terminate();
      foreach (KeyValuePair<string, MemoryStream> song in Music._songs)
        song.Value.Close();
    }

    public static string RandomTrack(string folder, string ignore = "")
    {
      if (DevConsole.rhythmMode)
        return "InGame/comic.ogg";
      string[] files = Content.GetFiles("Content/Audio/Music/" + folder);
      if (files.Length == 0)
        return "";
      List<string> source = new List<string>();
      foreach (string path in files)
      {
        string str = folder + "/" + Path.GetFileNameWithoutExtension(path);
        if (str != ignore)
          source.Add(str);
      }
      if (source.Count == 0)
        source.Add(files[0]);
      Queue<string> stringQueue = (Queue<string>) null;
      if (!Music._recentSongs.TryGetValue(folder, out stringQueue))
      {
        stringQueue = new Queue<string>();
        Music._recentSongs[folder] = stringQueue;
      }
      if (stringQueue.Count > 0 && stringQueue.Count > source.Count - 5)
        stringQueue.Dequeue();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) source);
      string str1 = "";
      while (str1 == "")
      {
        if (source.Count == 0 && stringQueue.Count > 0)
        {
          str1 = stringQueue.Dequeue();
          if (!stringList.Contains(str1))
            str1 = "";
        }
        else if (source.Count == 0)
        {
          str1 = stringList[0];
        }
        else
        {
          str1 = source[Rando.Int(source.Count<string>() - 1)];
          if (str1 == ignore && source.Count > 1)
          {
            source.Remove(str1);
            str1 = "";
          }
          else if (stringQueue.Contains(str1))
          {
            if ((double) Rando.Float(1f) > 0.25)
            {
              source.Remove(str1);
              if (source.Count > 0)
                str1 = "";
            }
            else
              str1 = "";
          }
        }
      }
      stringQueue.Enqueue(str1);
      return str1;
    }

    public static string FindSong(string song)
    {
      foreach (string file in Content.GetFiles("Content/Audio/Music/InGame"))
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(file);
        if (withoutExtension.ToLower() == song.ToLower())
          return "InGame/" + withoutExtension;
      }
      return "Challenging";
    }

    public static void Play(string music, bool looping = true, float crossFadeTime = 0.0f)
    {
      Music._currentSong = music;
      Music._players[Music._curPlayer].Stop();
      Music._players[Music._curPlayer].SetOgg(Music._songs[music]);
      Music._players[Music._curPlayer].Play();
      Music._players[Music._curPlayer].looped = looping;
    }

    public static void Play(Song music, bool looping = true)
    {
      if (music == null)
      {
        Music.Stop();
      }
      else
      {
        Music._currentSong = music.name;
        Music._players[Music._curPlayer].Stop();
        Music._players[Music._curPlayer].SetOgg(music.data);
        Music._players[Music._curPlayer].Play();
        Music._players[Music._curPlayer].looped = looping;
      }
    }

    public static void Load(string music, bool looping = true, float crossFadeTime = 0.0f)
    {
      Music._currentSong = music;
      Music._players[Music._curPlayer].Stop();
      Music._players[Music._curPlayer].SetOgg(Music._songs[music]);
      Music._players[Music._curPlayer].looped = looping;
    }

    public static void PlayLoaded() => Music._players[Music._curPlayer].Play();

    public static void CancelLooping() => Music._players[Music._curPlayer].looped = false;

    public static void LoadAlternateSong(string music, bool looping = true, float crossFadeTime = 0.0f)
    {
      Music._alternateLoop = looping;
      Music._pendingSong = music;
      Music._alternateSong = music;
    }

    public static void QuickSwap(string song)
    {
      Music._specialPlayer.Stop();
      if (Music._swapSong != song)
        Music._specialPlayer.SetOgg(Music._songs[song]);
      Music._swapSong = song;
      Music._specialPlayer.looped = true;
      Music._players[Music._curPlayer].Pause();
      Music._specialPlayer.Play();
    }

    public static void QuickSwapBack()
    {
      Music._specialPlayer.Stop();
      Music._players[Music._curPlayer].Resume();
    }

    public static void SwitchSongs()
    {
      try
      {
        Music._currentSong = Music._pendingSong;
        Music._players[Music._curPlayer].Stop();
        Music._players[Music._curPlayer].SetOgg(Music._songs[Music._currentSong]);
        Music._players[Music._curPlayer].Play();
        Music._players[Music._curPlayer].looped = Music._alternateLoop;
      }
      catch
      {
      }
      Music._pendingSong = (string) null;
    }

    public static void Pause() => Music._players[Music._curPlayer].Pause();

    public static void Resume() => Music._players[Music._curPlayer].Resume();

    public static void Stop() => Music._players[Music._curPlayer].Stop();

    public static void FadeOut(float duration) => Music._fadeSpeed = duration / 60f;

    public static void FadeIn(float duration) => Music._fadeSpeed = (float) -((double) duration / 60.0);

    private static void SearchDir(string dir)
    {
      foreach (string file in Content.GetFiles(dir))
        Music.ProcessSong(file);
      foreach (string directory in Content.GetDirectories(dir))
        Music.SearchDir(directory);
    }

    private static void ProcessSong(string path)
    {
      path = path.Replace('\\', '/');
      MemoryStream memoryStream = OggSong.Load(path);
      path = path.Substring(0, path.Length - 4);
      string key = path.Substring(path.IndexOf("/Music/") + 7);
      Music._songs[key] = memoryStream;
      ++MonoMain.loadyBits;
    }

    public static void Update()
    {
    }
  }
}
