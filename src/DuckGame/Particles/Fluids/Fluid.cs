﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Fluid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Fluid : PhysicsParticle
  {
    public static FluidData Lava = new FluidData(0.0f, new Color((int) byte.MaxValue, 89, 5).ToVector4(), 0.0f, "lava", 1f, 0.8f);
    public static FluidData Gas = new FluidData(0.0f, new Color(246, 198, 55).ToVector4(), 1f, "gas");
    public static FluidData Water = new FluidData(0.0f, new Color(0, 137, 209).ToVector4(), 0.0f, "water");
    public static FluidData Ketchup = new FluidData(0.0f, Color.Red.ToVector4() * 0.8f, 0.4f, "water");
    public static FluidData Poo = new FluidData(0.0f, Color.SaddleBrown.ToVector4() * 0.8f, 0.5f, "water");
    private Fluid _stream;
    private Fluid _child;
    private bool _firstHit;
    private float _thickness;
    public FluidData data;
    private float _thickMult;
    private SmallFire _fire;
    public SpriteMap _glob;
    private float startThick;
    private float live = 1f;

    public Fluid child
    {
      get => this._child;
      set => this._child = value;
    }

    public SmallFire fire
    {
      get => this._fire;
      set => this._fire = value;
    }

    public Fluid(
      float xpos,
      float ypos,
      Vec2 hitAngle,
      FluidData dat,
      Fluid stream = null,
      float thickMult = 1f)
      : base(xpos, ypos)
    {
      this.hSpeed = (float) (-(double) hitAngle.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
      this.vSpeed = (float) (-(double) hitAngle.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
      this.hSpeed = hitAngle.x;
      this.vSpeed = hitAngle.y;
      this._bounceEfficiency = 0.6f;
      this._stream = stream;
      if (stream != null)
        stream.child = this;
      this.alpha = 1f;
      this._gravMult = 2f;
      this.depth = new Depth(-0.5f);
      this.data = dat;
      this._thickMult = thickMult;
      this._thickness = Maths.Clamp(this.data.amount * 600f, 0.2f, 8f) * this._thickMult;
      this.startThick = this._thickness;
      this._glob = new SpriteMap("bigGlob", 8, 8);
    }

    public override void Update()
    {
      if (this._fire != null)
        this._fire.position = this.position;
      this._life = 1f;
      if ((double) this._thickness < 4.0 || (double) Math.Abs(this.vSpeed) < 1.5)
        this.live -= 0.01f;
      this._thickness = Lerp.FloatSmooth(this.startThick, 0.1f, 1f - this.live);
      if ((double) this.live < 0.0 || this._grounded && (double) Math.Abs(this.vSpeed) < 0.100000001490116)
      {
        Level.Remove((Thing) this);
        this.active = false;
        FluidPuddle fluidPuddle1 = (FluidPuddle) null;
        foreach (FluidPuddle fluidPuddle2 in Level.current.things[typeof (FluidPuddle)])
        {
          if ((double) this.x > (double) fluidPuddle2.left && (double) this.x < (double) fluidPuddle2.right && (double) Math.Abs(fluidPuddle2.y - this.y) < 10.0)
          {
            fluidPuddle1 = fluidPuddle2;
            break;
          }
        }
        if (fluidPuddle1 == null)
        {
          Vec2 position;
          Block b = (Block) Level.CheckLine<AutoBlock>(this.position + new Vec2(0.0f, -8f), this.position + new Vec2(0.0f, 16f), out position);
          if (b != null && (double) position.y == (double) b.top)
          {
            fluidPuddle1 = new FluidPuddle(position.x, position.y, b);
            Level.Add((Thing) fluidPuddle1);
          }
        }
        fluidPuddle1?.Feed(this.data);
      }
      else
      {
        base.Update();
        if (this._touchedFloor && !this._firstHit)
        {
          this._firstHit = true;
          this.hSpeed += Rando.Float(-1f, 1f);
          this.hSpeed *= Rando.Float(-1f, 1.5f);
          this.vSpeed *= Rando.Float(0.3f, 1f);
        }
        if (this._stream == null)
          return;
        float num = Math.Abs(this.hSpeed - this._stream.hSpeed);
        if ((double) Math.Abs(this.x - this._stream.x) * (double) num <= 40.0 && (double) Math.Abs(this.vSpeed - this._stream.vSpeed) <= 1.89999997615814 && (double) num <= 1.89999997615814)
          return;
        this.BreakStream();
      }
    }

    public void BreakStream()
    {
      if (this._child != null)
        this._child._stream = (Fluid) null;
      this._child = (Fluid) null;
      if (this._stream != null)
        this._stream._child = (Fluid) null;
      this._stream = (Fluid) null;
    }

    public override void Draw()
    {
      if (this._stream != null)
      {
        ++Graphics.currentDrawIndex;
        Graphics.DrawLine(this.position, this._stream.position, new Color(this.data.color) * this.alpha, this._thickness, this.depth);
      }
      else
      {
        if (this._child != null)
          return;
        if ((double) this._thickness > 4.0)
        {
          this._glob.depth = this.depth;
          this._glob.frame = 2;
          this._glob.color = new Color(this.data.color) * this.alpha;
          this._glob.CenterOrigin();
          this._glob.angle = Maths.DegToRad((float) (-(double) Maths.PointDirection(this.position, this.position + this.velocity) + 90.0));
          Graphics.Draw((Sprite) this._glob, this.x, this.y);
        }
        else
          Graphics.DrawRect(this.position - new Vec2(this._thickness / 2f, this._thickness / 2f), this.position + new Vec2(this._thickness / 2f, this._thickness / 2f), new Color(this.data.color) * this.alpha, this.depth);
      }
    }
  }
}
