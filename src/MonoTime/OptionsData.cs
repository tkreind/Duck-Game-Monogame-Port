// Decompiled with JetBrains decompiler
// Type: DuckGame.OptionsData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class OptionsData : DataClass
  {
    public float musicVolume { get; set; }

    public float sfxVolume { get; set; }

    public bool shennanigans { get; set; }

    public bool fullscreen { get; set; }

    public int resolution { get; set; }

    public bool fireGlow { get; set; }

    public bool lighting { get; set; }

    public bool cloud { get; set; }

    public int keyboard1PlayerIndex { get; set; }

    public int keyboard2PlayerIndex { get; set; }

    public OptionsData()
    {
      this._nodeName = "Options";
      this.sfxVolume = 0.8f;
      this.musicVolume = 0.7f;
      this.shennanigans = true;
      this.fireGlow = true;
      this.lighting = true;
      this.keyboard1PlayerIndex = 0;
      this.keyboard2PlayerIndex = 1;
    }
  }
}
