// Decompiled with JetBrains decompiler
// Type: DuckGame.EjectedShell
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public abstract class EjectedShell : PhysicsParticle
  {
    private SpriteMap _sprite;

    protected EjectedShell(float xpos, float ypos, string shellSprite, string bounceSound = "metalBounce")
      : base(xpos, ypos)
    {
      this.hSpeed = -4f - Rando.Float(3f);
      this.vSpeed = (float) -((double) Rando.Float(1.5f) + 1.0);
      this._sprite = new SpriteMap(shellSprite, 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this._bounceSound = bounceSound;
      this.depth = (Depth) (0.3f + Rando.Float(0.0f, 0.1f));
    }

    public override void Update()
    {
      base.Update();
      this._angle = Maths.DegToRad(-this._spinAngle);
    }
  }
}
