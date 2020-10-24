﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DustSparkleEffect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class DustSparkleEffect : Thing
  {
    private int _sparkleWait;
    private SpriteMap _light;
    private List<DustSparkle> _sparkles = new List<DustSparkle>();
    private bool _wide;
    private bool _lit;

    public DustSparkleEffect(float xpos, float ypos, bool wide, bool lit)
      : base(xpos, ypos)
    {
      this._light = !wide ? new SpriteMap("arcade/lights", 56, 57) : new SpriteMap("arcade/prizeLights", 107, 55);
      this._wide = wide;
      this._lit = lit;
    }

    public override void Initialize()
    {
      this.material = (Material) new MaterialDustSparkle(this.position, new Vec2((float) this._light.width, (float) this._light.height), this._wide, this._lit);
      base.Initialize();
    }

    public float fade
    {
      get => (this.material as MaterialDustSparkle).fade;
      set => (this.material as MaterialDustSparkle).fade = value;
    }

    public override void Update()
    {
      for (int index = 0; index < this._sparkles.Count; ++index)
      {
        DustSparkle sparkle = this._sparkles[index];
        sparkle.position += sparkle.velocity;
        sparkle.position.y += (float) Math.Sin((double) sparkle.sin) * 0.01f;
        sparkle.sin += 0.01f;
        if ((double) sparkle.alpha < 1.0)
          sparkle.alpha += 0.01f;
        bool flag = false;
        if ((double) sparkle.position.x > (double) this.x + (double) this._light.width + 2.0 || (double) sparkle.position.x < (double) this.x - 2.0 || ((double) sparkle.position.y < (double) this.y + 1.0 || (double) sparkle.position.y > (double) this.y + (double) this._light.height))
          flag = true;
        if (flag)
        {
          this._sparkles.RemoveAt(index);
          --index;
        }
      }
      ++this._sparkleWait;
      if (this._sparkleWait <= 10)
        return;
      this._sparkleWait = 0;
      int num = 1;
      if ((double) Rando.Float(1f) > 0.5)
        num = -1;
      this._sparkles.Add(new DustSparkle(new Vec2(this.x + Rando.Float((float) this._light.width), this.y + Rando.Float((float) this._light.height)), new Vec2(Rando.Float(0.15f, 0.25f) * (float) num, Rando.Float(-0.05f, 0.05f))));
    }

    public override void Draw()
    {
      this._light.depth = this.depth - 2;
      this._light.frame = 1;
      this._light.alpha = 0.7f;
      Graphics.Draw((Sprite) this._light, this.x, this.y);
      foreach (DustSparkle sparkle in this._sparkles)
        Graphics.DrawRect(sparkle.position + new Vec2(-0.5f, -0.5f), sparkle.position + new Vec2(0.5f, 0.5f), Color.White * sparkle.alpha, this.depth + 10);
    }
  }
}
