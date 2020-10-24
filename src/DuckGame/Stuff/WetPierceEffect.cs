// Decompiled with JetBrains decompiler
// Type: DuckGame.WetPierceEffect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WetPierceEffect : Thing
  {
    private SpriteMap _sprite;

    public WetPierceEffect(float xpos, float ypos, Vec2 dir, Thing attach)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("wetParticle", 16, 16);
      this._sprite.AddAnimation("splash", 0.45f, false, 0, 1, 2, 3);
      this._sprite.SetAnimation("splash");
      this.center = new Vec2(0.0f, 7f);
      this.graphic = (Sprite) this._sprite;
      this.depth = new Depth(0.7f);
      this.alpha = 0.6f;
      this.angle = Maths.DegToRad(-Maths.PointDirection(Vec2.Zero, dir));
      this.anchor = new Anchor(attach);
      this.anchor.offset = new Vec2(xpos, ypos) - attach.position;
    }

    public override void Update()
    {
      if (!this._sprite.finished)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw() => base.Draw();
  }
}
