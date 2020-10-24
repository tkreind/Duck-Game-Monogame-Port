// Decompiled with JetBrains decompiler
// Type: DuckGame.EffectAnimation
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EffectAnimation : Thing
  {
    protected SpriteMap _sprite;
    public Color color = Color.White;

    public EffectAnimation(Vec2 pos, SpriteMap spr, float deep)
      : base(pos.x, pos.y)
    {
      this.depth = (Depth) deep;
      this._sprite = spr;
      this._sprite.CenterOrigin();
      this.layer = Layer.Foreground;
    }

    public override void Update()
    {
      if (this._sprite.finished)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Draw()
    {
      this._sprite.scale = this.scale;
      this._sprite.alpha = this.alpha;
      this._sprite.color = this.color;
      this._sprite.depth = this.depth;
      this._sprite.flipH = this.flipHorizontal;
      Graphics.Draw((Sprite) this._sprite, this.x, this.y);
      base.Draw();
    }
  }
}
