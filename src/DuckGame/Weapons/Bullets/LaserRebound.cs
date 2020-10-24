// Decompiled with JetBrains decompiler
// Type: DuckGame.LaserRebound
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class LaserRebound : Thing
  {
    private Tex2D _rebound = Content.Load<Tex2D>("laserRebound");

    public LaserRebound(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite(this._rebound);
      this.depth = (Depth) 0.9f;
      this.center = new Vec2(4f, 4f);
    }

    public override void Update()
    {
      this.alpha -= 0.07f;
      if ((double) this.alpha > 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
