// Decompiled with JetBrains decompiler
// Type: DuckGame.DizzyStar
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DizzyStar : PhysicsParticle, IFactory
  {
    private float maxSize;

    public DizzyStar(float xpos, float ypos, Vec2 dir)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("dizzyStar");
      this.graphic.CenterOrigin();
      this.xscale = this.yscale = Rando.Float(0.3f, 0.8f);
      this.hSpeed = dir.x;
      this.vSpeed = dir.y;
      this.maxSize = Rando.Float(0.4f, 1f);
    }

    public override void Update()
    {
      this.xscale = Lerp.Float(this.xscale, this.maxSize, 0.05f);
      this.yscale = this.xscale;
      this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
