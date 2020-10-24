// Decompiled with JetBrains decompiler
// Type: DuckGame.GlassParticle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GlassParticle : PhysicsParticle
  {
    private int _tint;

    public GlassParticle(float xpos, float ypos, Vec2 hitAngle, int tint = -1)
      : base(xpos, ypos)
    {
      this.hSpeed = (float) (-(double) hitAngle.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
      this.vSpeed = (float) (-(double) hitAngle.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
      this._bounceEfficiency = 0.6f;
      this._tint = tint;
    }

    public override void Update()
    {
      this.alpha -= 0.01f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Draw() => Graphics.DrawRect(this.position, this.position + new Vec2(1f, 1f), (this._tint > 0 ? Window.windowColors[this._tint] : Color.LightBlue) * this.alpha, this.depth);
  }
}
