// Decompiled with JetBrains decompiler
// Type: DuckGame.EscapingGhost
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EscapingGhost : Thing
  {
    private SpriteMap _sprite;

    public EscapingGhost(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("ghost", 32, 32);
      this._sprite.AddAnimation("wither", 0.5f, false, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
      this._sprite.SetAnimation("wither");
      this.center = new Vec2(16f, 32f);
      this.alpha = 0.6f;
      this.depth = (Depth) 0.9f;
      this.graphic = (Sprite) this._sprite;
    }

    public override void Update()
    {
      if (this._sprite.finished)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
