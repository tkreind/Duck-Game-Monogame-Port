﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ChaingunBullet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class ChaingunBullet : Thing
  {
    public Thing parentThing;
    public Thing childThing;
    public Vec2 chainOffset = new Vec2();
    public float sway;
    public float desiredSway;
    public float lastDesiredSway;
    public float wave;
    public float shake;
    public float waveSpeed;
    public float waveAdd = 0.07f;

    public ChaingunBullet(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("chainBullet");
      this.center = new Vec2(4f, 3f);
      this.depth = new Depth(0.8f);
    }

    public ChaingunBullet(float xpos, float ypos, bool dart)
      : base(xpos, ypos)
    {
      if (dart)
      {
        this.graphic = (Sprite) new SpriteMap(nameof (dart), 16, 16);
        this.center = new Vec2(7f, 7f);
      }
      else
      {
        this.graphic = new Sprite("chainBullet");
        this.center = new Vec2(4f, 3f);
      }
      this.depth = new Depth(0.8f);
    }

    public override void Update()
    {
      this.wave += 0.1f + this.waveSpeed;
      if (this.childThing == null)
        return;
      this.childThing.Update();
    }

    public override void Draw()
    {
      if (this.parentThing != null)
      {
        this.position = this.parentThing.position + this.chainOffset + new Vec2(0.0f, 2f);
        this.graphic.flipH = this.parentThing.graphic.flipH;
        this.desiredSway = 0.0f;
        this.desiredSway = !(this.parentThing is Gun parentThing) || parentThing.owner == null ? -this.parentThing.hSpeed : -parentThing.owner.hSpeed;
        this.shake += Math.Abs(this.lastDesiredSway - this.desiredSway) * 0.3f;
        if ((double) this.shake > 0.0)
          this.shake -= 0.01f;
        else
          this.shake = 0.0f;
        if ((double) this.shake > 1.5)
        {
          this.shake = 1.5f;
          this.waveSpeed += 0.02f;
        }
        if ((double) this.waveSpeed > 0.100000001490116)
          this.waveSpeed = 0.1f;
        if ((double) this.waveSpeed > 0.0)
          this.waveSpeed -= 0.01f;
        else
          this.waveSpeed = 0.0f;
        this.lastDesiredSway = this.desiredSway;
        if (this.parentThing is ChaingunBullet parentThing2)
          this.desiredSway += parentThing2.sway * 0.7f;
        this.desiredSway += (float) Math.Sin((double) this.wave + (double) this.waveAdd) * this.shake;
        this.sway = MathHelper.Lerp(this.sway, this.desiredSway, 1f);
        this.position.x += this.sway;
      }
      base.Draw();
      if (this.childThing == null)
        return;
      this.childThing.depth = this.depth - 1;
      this.childThing.Draw();
    }
  }
}
