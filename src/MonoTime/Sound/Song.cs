// Decompiled with JetBrains decompiler
// Type: DuckGame.Song
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.IO;

namespace DuckGame
{
  public class Song
  {
    private MemoryStream _data;
    private string _name;

    public MemoryStream data => this._data;

    public string name => this._name;

    public Song(MemoryStream dat, string nam)
    {
      this._data = dat;
      this._name = nam;
    }

    public void Play(bool loop = true) => Music.Play(this, loop);

    public void Stop()
    {
      if (!(Music.currentSong == this._name))
        return;
      Music.Stop();
    }
  }
}
