﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SpaceDonut
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class SpaceDonut : Thing
  {
    private float sinInc;
    private SpriteMap _donuroid;
    private List<Donuroid> _roids = new List<Donuroid>();

    public SpaceDonut(float xpos, float ypos)
      : base(xpos, ypos)
    {
      Sprite sprite = new Sprite("background/donut");
      sprite.depth = new Depth(-0.9f);
      this.graphic = sprite;
      this._donuroid = new SpriteMap("background/donuroids", 32, 32);
      this._donuroid.CenterOrigin();
      Random random = new Random(4562280);
      Random generator = Rando.generator;
      Rando.generator = random;
      Vec2 vec2_1 = new Vec2(-22f, -14f);
      Vec2 vec2_2 = new Vec2(130f, 120f);
      for (int index = 0; index < 20; ++index)
      {
        this._roids.Add(new Donuroid(vec2_2.x + Rando.Float(-6f, 6f), vec2_2.y + Rando.Float(-18f, 18f), this._donuroid, Rando.Int(0, 7), new Depth(1f), 1f));
        this._roids.Add(new Donuroid(vec2_2.x + Rando.Float(-6f, -1f), (float) ((double) vec2_2.y + (double) Rando.Float(-10f, 0.0f) - 10.0), this._donuroid, Rando.Int(0, 7), this.depth - 20, 0.5f));
        this._roids.Add(new Donuroid(vec2_2.x + Rando.Float(6f, 1f), (float) ((double) vec2_2.y + (double) Rando.Float(10f, 0.0f) - 10.0), this._donuroid, Rando.Int(0, 7), this.depth - 20, 0.5f));
        this._roids.Add(new Donuroid(vec2_2.x + Rando.Float(-6f, -1f), (float) ((double) vec2_2.y + (double) Rando.Float(-10f, 0.0f) - 20.0), this._donuroid, Rando.Int(0, 7), this.depth - 30, 0.25f));
        this._roids.Add(new Donuroid(vec2_2.x + Rando.Float(6f, 1f), (float) ((double) vec2_2.y + (double) Rando.Float(10f, 0.0f) - 20.0), this._donuroid, Rando.Int(0, 7), this.depth - 30, 0.25f));
        vec2_2 += vec2_1;
        vec2_1.y += 1.4f;
      }
      Rando.generator = generator;
    }

    public override void Draw()
    {
      this.sinInc += 0.02f;
      Graphics.Draw(this.graphic, this.x, this.y + (float) Math.Sin((double) this.sinInc) * 2f, new Depth(0.9f));
      foreach (Donuroid roid in this._roids)
        roid.Draw(this.position);
    }
  }
}
