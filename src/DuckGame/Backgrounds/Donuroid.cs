﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Donuroid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Donuroid
  {
    private SpriteMap _image;
    private int _frame;
    public Depth _depth;
    private Vec2 _position;
    private float _scale = 1f;
    private float _sin;

    public Donuroid(
      float xpos,
      float ypos,
      SpriteMap image,
      int frame,
      Depth depth,
      float scale)
    {
      this._image = image;
      this._frame = frame;
      this._depth = depth;
      this._scale = scale;
      this._position = new Vec2(xpos, ypos);
      this._sin = Rando.Float(8f);
    }

    public void Draw(Vec2 pos)
    {
      this._image.frame = this._frame;
      this._image.depth = this._depth;
      this._image.xscale = this._image.yscale = this._scale;
      if ((double) this._scale == 1.0)
        this._image.color = new Color(0.8f, 0.8f, 0.8f, 1f);
      else
        this._image.color = Color.White * this._scale;
      Graphics.Draw((Sprite) this._image, pos.x + this._position.x, (float) ((double) pos.y + (double) this._position.y + Math.Sin((double) this._sin) * ((double) this._scale * 2.0)));
      this._sin += 0.01f;
    }
  }
}
