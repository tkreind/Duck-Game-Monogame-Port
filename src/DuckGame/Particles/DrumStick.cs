// Decompiled with JetBrains decompiler
// Type: DuckGame.DrumStick
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DrumStick : Thing
  {
    private float _startY;

    public DrumStick(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("drumset/drumStick");
      this.center = new Vec2((float) (this.graphic.w / 2), (float) (this.graphic.h / 2));
      this._startY = ypos;
      this.vSpeed = -3f;
    }

    public override void Update()
    {
      this.angle += 0.6f;
      this.y += this.vSpeed;
      this.vSpeed += 0.2f;
      if ((double) this.y <= (double) this._startY)
        return;
      Level.Remove((Thing) this);
    }
  }
}
