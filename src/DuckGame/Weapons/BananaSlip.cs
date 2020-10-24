// Decompiled with JetBrains decompiler
// Type: DuckGame.BananaSlip
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BananaSlip : Thing
  {
    private SpriteMap _sprite;

    public BananaSlip(float xpos, float ypos, bool flip)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("slip", 32, 32);
      this._sprite.AddAnimation("slip", 0.45f, false, 0, 1, 2, 3);
      this._sprite.SetAnimation("slip");
      this.center = new Vec2(19f, 31f);
      this.graphic = (Sprite) this._sprite;
      this._sprite.flipH = flip;
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
