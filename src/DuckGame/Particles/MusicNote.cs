﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.MusicNote
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class MusicNote : Thing
  {
    private Color _color;
    private SinWave _sin;
    private float _size;
    private float _speed;
    private SpriteMap _sprite;
    private Vec2 _dir;

    public MusicNote(float xpos, float ypos, Vec2 dir)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("notes", 8, 8);
      this._sprite.frame = Rando.Int(1);
      this._sprite.CenterOrigin();
      int num1 = Rando.ChooseInt(0, 1, 2, 3);
      if (num1 == 0)
        this._color = Color.Violet;
      if (num1 == 1)
        this._color = Color.SkyBlue;
      if (num1 == 2)
        this._color = Color.Wheat;
      if (num1 == 4)
        this._color = Color.GreenYellow;
      this._dir = dir;
      float num2 = 1f;
      if ((double) Rando.Float(1f) <= 0.5)
        num2 = -1f;
      this._sin = new SinWave(0.03f + Rando.Float(0.1f), num2 * 6.283185f);
      this._size = 3f + Rando.Float(6f);
      this._speed = 0.8f + Rando.Float(1.4f);
      this.depth = new Depth(0.95f);
      this.scale = new Vec2(0.1f, 0.1f);
    }

    public override void Update()
    {
      this.x += this._dir.x;
      Vec2 scale = this.scale;
      scale.x = scale.y = Lerp.Float(scale.x, 1f, 0.05f);
      this.scale = scale;
      if ((double) this.scale.x <= 0.899999976158142)
        return;
      this.alpha -= 0.01f;
      if ((double) this.alpha > 0.0)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      Vec2 position = this.position;
      position.y += this._sin.value * this._size;
      this._sprite.alpha = this.alpha;
      this._sprite.scale = this.scale;
      Graphics.Draw((Sprite) this._sprite, position.x, position.y);
    }
  }
}
