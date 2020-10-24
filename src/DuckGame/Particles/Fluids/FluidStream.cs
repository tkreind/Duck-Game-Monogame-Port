﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.FluidStream
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class FluidStream : Thing
  {
    private Vec2 _sprayAngle;
    private Vec2 _startSprayAngle;
    private float _holeThickness = 1f;
    private float _sprayVelocity;
    private Vec2 _endPoint;
    private Vec2 _offset;
    private bool _onFire;
    private int _framesSinceFire;
    private Fluid _lastFluid;
    private int _framesSinceFluid;
    private float _fluctuate;
    private float _speedMul = 0.1f;
    private float _maxSpeedMul = 0.1f;
    public float streamSpeedMultiplier = 1f;

    public Vec2 sprayAngle
    {
      get => this._sprayAngle;
      set => this._sprayAngle = value;
    }

    public Vec2 startSprayAngle => this._startSprayAngle;

    public float holeThickness
    {
      get => this._holeThickness;
      set => this._holeThickness = value;
    }

    public Vec2 offset
    {
      get => this._offset;
      set => this._offset = value;
    }

    public bool onFire
    {
      get => this._onFire;
      set => this._onFire = value;
    }

    public FluidStream(float xpos, float ypos, Vec2 sprayAngleVal, float sprayVelocity, Vec2 off = default (Vec2))
      : base(xpos, ypos)
    {
      this._endPoint = new Vec2(xpos, ypos);
      this._sprayAngle = sprayAngleVal;
      this._startSprayAngle = sprayAngleVal;
      this._sprayVelocity = sprayVelocity;
      this._offset = off;
    }

    public void Feed(FluidData dat)
    {
      float to = Maths.Clamp(dat.amount * 200f, 0.1f, 2f);
      if ((double) to > (double) this._maxSpeedMul)
        this._maxSpeedMul = Lerp.Float(this._maxSpeedMul, to, 0.1f);
      this._lastFluid = new Fluid(this.x, this.y, (this._sprayAngle * ((float) (2.0 + Math.Sin((double) this._fluctuate) * 0.5) * this._speedMul) + new Vec2(this.hSpeed * 0.0f, this.vSpeed * 0.0f)) * this.streamSpeedMultiplier, dat, this._lastFluid);
      Level.Add((Thing) this._lastFluid);
      this._framesSinceFluid = 0;
      if ((double) dat.flammable > 0.5 && this.onFire && (this._framesSinceFire > 12 && (double) Rando.Float(1f) < 0.119999997317791 * (double) dat.flammable))
      {
        SmallFire smallFire = SmallFire.New(this._lastFluid.x, this._lastFluid.y, 0.0f, 0.0f);
        this._lastFluid.fire = smallFire;
        Level.Add((Thing) smallFire);
        this._framesSinceFire = 0;
      }
      this._fluctuate += 0.2f;
    }

    public override void Update()
    {
      ++this._framesSinceFire;
      this._maxSpeedMul = Lerp.Float(this._maxSpeedMul, 0.1f, 1f / 1000f);
      this._speedMul = Lerp.Float(this._speedMul, this._maxSpeedMul, 0.04f);
      if (this._lastFluid != null)
        ++this._framesSinceFluid;
      if (this._framesSinceFluid <= 12)
        return;
      this._framesSinceFluid = 0;
      this._lastFluid = (Fluid) null;
    }

    public override void Draw() => base.Draw();
  }
}
