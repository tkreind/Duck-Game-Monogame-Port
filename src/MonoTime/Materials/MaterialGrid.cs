﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialGrid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckGame
{
  public class MaterialGrid : Material
  {
    private Thing _thing;
    private float transWave = 0.2f;
    private bool secondScan;
    private bool scanSwitch;
    public bool finished;

    public MaterialGrid(Thing t)
    {
      this._effect = Content.Load<MTEffect>("Shaders/wireframeTexOuya");
      this._thing = t;
    }

    public override void Update()
    {
      this.transWave -= 0.12f;
      if (Math.Sin((double) this.transWave) < -0.699999988079071 && !this.secondScan)
      {
        this.secondScan = true;
        this.transWave -= 2f;
      }
      else if (Math.Sin((double) this.transWave) > 0.9 && this.secondScan)
        this.finished = true;
      base.Update();
    }

    public override void Apply()
    {
      Matrix fullMatrix = Layer.Game.fullMatrix;
      Vec3 vec3_1 = Vec3.Transform(new Vec3(this._thing.x - 28f, this._thing.y, 0.0f), fullMatrix);
      Vec3 vec3_2 = Vec3.Transform(new Vec3(this._thing.x + 28f, this._thing.y, 0.0f), fullMatrix);
      this.SetValue("scan", vec3_1.x + (float) ((Math.Sin((double) this.transWave) + 1.0) / 2.0 * ((double) vec3_2.x - (double) vec3_1.x)));
      this.SetValue("secondScan", this.secondScan ? 1f : 0.0f);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
