// Decompiled with JetBrains decompiler
// Type: DuckGame.HeartPuff
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class HeartPuff : Thing
  {
    private SpriteMap _sprite;

    public HeartPuff(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("heartpuff", 16, 16);
      this._sprite.AddAnimation("wither", 0.35f, false, 0, 1, 2, 3, 4);
      this._sprite.SetAnimation("wither");
      this.center = new Vec2(5f, 16f);
      this.alpha = 0.6f;
      this.depth = new Depth(0.9f);
      this.graphic = (Sprite) this._sprite;
      this._sprite.color = Color.Green;
    }

    public override void Update()
    {
      if (this.anchor != (Thing) null && this.anchor.thing != null)
      {
        this.flipHorizontal = this.anchor.thing.offDir < (sbyte) 0;
        if (this.flipHorizontal)
          this.center = new Vec2(10f, 16f);
        else
          this.center = new Vec2(5f, 16f);
        this.angle = this.anchor.thing.angle;
      }
      if (this._sprite.finished)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
