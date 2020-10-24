// Decompiled with JetBrains decompiler
// Type: DuckGame.StandingFluid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class StandingFluid : Thing
  {
    public EditorProperty<int> deep = new EditorProperty<int>(1, min: 1f, max: 256f, increment: 1f);
    public EditorProperty<int> fluidType = new EditorProperty<int>(0, max: 2f, increment: 1f);
    private Vec2 _prevPos = Vec2.Zero;
    private Vec2 _leftSide;
    private Vec2 _rightSide;
    private float _floor;
    private bool _isValid;
    private bool _filled;
    private int w8;

    public StandingFluid(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.hugWalls = WallHug.Floor;
    }

    public override void Initialize() => base.Initialize();

    private FluidData GetFluidType()
    {
      switch ((int) this.fluidType)
      {
        case 0:
          return Fluid.Water;
        case 1:
          return Fluid.Gas;
        case 2:
          return Fluid.Lava;
        default:
          return Fluid.Water;
      }
    }

    public override void Update()
    {
      if (!this._filled)
      {
        this._filled = true;
        Block b = Level.CheckRay<Block>(new Vec2(this.x, this.y), new Vec2(this.x, this.y + 64f));
        if (b != null)
        {
          FluidPuddle fluidPuddle = new FluidPuddle(this.x, b.top, b);
          Level.Add((Thing) fluidPuddle);
          float num = 0.0f;
          while ((double) fluidPuddle.CalculateDepth() < (double) ((int) this.deep * 8))
          {
            FluidData fluidType = this.GetFluidType();
            fluidType.amount = 0.5f;
            fluidPuddle.Feed(fluidType);
            float depth = fluidPuddle.CalculateDepth();
            if ((double) Math.Abs(num - depth) < 1.0 / 1000.0)
            {
              Level.Remove((Thing) this);
              break;
            }
            num = depth;
          }
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      if (Level.current is Editor)
      {
        if (this._prevPos != this.position)
        {
          this._isValid = false;
          this._prevPos = this.position;
          Vec2 hitPos1;
          Vec2 hitPos2;
          Vec2 hitPos3;
          if (Level.CheckRay<Block>(this.position, this.position - new Vec2(1000f, 0.0f), out hitPos1) != null && Level.CheckRay<Block>(this.position, this.position + new Vec2(1000f, 0.0f), out hitPos2) != null && Level.CheckRay<Block>(this.position, this.position + new Vec2(0.0f, 64f), out hitPos3) != null)
          {
            this._floor = hitPos3.y;
            this._leftSide = hitPos1;
            this._rightSide = hitPos2;
            this._isValid = true;
          }
        }
        if (this._isValid)
          Graphics.DrawRect(new Vec2(this._leftSide.x, this._floor - (float) ((int) this.deep * 8)), new Vec2(this._rightSide.x, this._floor), new Color(this.GetFluidType().color) * 0.5f, new Depth(0.9f));
      }
      base.Draw();
    }
  }
}
