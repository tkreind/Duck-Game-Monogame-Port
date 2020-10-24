// Decompiled with JetBrains decompiler
// Type: DuckGame.OpenPresent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class OpenPresent : PhysicsParticle
  {
    private SpriteMap _sprite;

    public OpenPresent(float xpos, float ypos, int frame)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("presents", 16, 16);
      this._sprite.frame = frame + 8;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 13f);
      this.hSpeed = 0.0f;
      this.vSpeed = 0.0f;
      this._bounceEfficiency = 0.0f;
      this.depth = new Depth(0.9f);
      this._life = 5f;
    }

    public override void Update() => base.Update();
  }
}
