﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.TrailPiece
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class TrailPiece
  {
    internal Vec2 position;
    internal Vec2 p1;
    internal Vec2 p2;
    internal Vec2 scale = new Vec2(1f, 1f);
    internal float wide = 1f;

    internal TrailPiece(float _x, float _y, float _width, Vec2 _p1, Vec2 _p2)
    {
      this.position.x = _x;
      this.position.y = _y;
      this.wide = _width;
      this.p1 = _p1;
      this.p2 = _p2;
    }
  }
}
