// Decompiled with JetBrains decompiler
// Type: DuckGame.BassDrum
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BassDrum : Drum
  {
    public BassDrum(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("drumset/bassDrum");
      this.center = new Vec2((float) (this.graphic.w / 2), (float) (this.graphic.h / 2));
      this._sound = "kick";
    }
  }
}
