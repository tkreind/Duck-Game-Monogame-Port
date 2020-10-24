// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelUpArrowAnimation
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class LevelUpArrowAnimation : EffectAnimation
  {
    private float _startWait;
    private float _alph = 2f;
    private float _vel;

    public LevelUpArrowAnimation(Vec2 pos)
      : base(pos, new SpriteMap("levelUpArrow", 16, 16), 0.9f)
    {
      this.layer = Layer.HUD;
      this.alpha = 0.0f;
      this._startWait = Rando.Float(2.5f);
      this._sprite.depth = new Depth(1f);
    }

    public override void Update()
    {
      if ((double) this._startWait > 0.0)
      {
        this._startWait -= 0.1f;
      }
      else
      {
        this._vel -= 0.1f;
        this.y += this._vel;
        this._alph -= 0.1f;
        this.alpha = Math.Min(this._alph, 1f);
        if ((double) this.alpha <= 0.0)
          Level.Remove((Thing) this);
      }
      base.Update();
    }
  }
}
