// Decompiled with JetBrains decompiler
// Type: DuckGame.WaterParticle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WaterParticle : Thing
  {
    public WaterParticle(float xpos, float ypos, Vec2 hitAngle)
      : base(xpos, ypos)
      => this.hSpeed = (float) (-(double) hitAngle.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));

    public override void Update()
    {
      this.vSpeed += 0.1f;
      this.hSpeed *= 0.9f;
      this.x += this.hSpeed;
      this.y += this.vSpeed;
      this.alpha -= 0.06f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Draw() => Graphics.DrawRect(this.position, this.position + new Vec2(1f, 1f), Color.LightBlue * this.alpha, this.depth);
  }
}
