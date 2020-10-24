// Decompiled with JetBrains decompiler
// Type: DuckGame.DartShell
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DartShell : PhysicsParticle
  {
    private SpriteMap _sprite;
    private float _rotSpeed;
    private bool _die;

    public DartShell(float xpos, float ypos, float rotSpeed, bool flip)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("dart", 16, 16);
      this._sprite.flipH = flip;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this._bounceSound = "";
      this._rotSpeed = rotSpeed;
      this.depth = new Depth(0.3f);
    }

    public override void Update()
    {
      base.Update();
      this.angle += this._rotSpeed;
      if ((double) this.vSpeed < 0.0 || this._grounded)
        this._die = true;
      if (this._die)
        this.alpha -= 0.05f;
      if ((double) this.alpha > 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
