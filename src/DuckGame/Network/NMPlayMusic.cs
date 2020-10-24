// Decompiled with JetBrains decompiler
// Type: DuckGame.NMPlayMusic
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMPlayMusic : NMEvent
  {
    public string song;

    public NMPlayMusic(string s) => this.song = s;

    public NMPlayMusic()
    {
    }

    public override void Activate() => Music.Play(this.song);
  }
}
