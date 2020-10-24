﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidLightRoof
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("details|pyramid")]
  public class PyramidLightRoof : Thing
  {
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();
    private Block myBlock;
    private PointLight light;
    private bool did;

    public PyramidLightRoof(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("pyramidRoofLight");
      this.center = new Vec2(7f, 5f);
      this._collisionSize = new Vec2(14f, 6f);
      this._collisionOffset = new Vec2(-7f, -3f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Ceiling;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._occluders.Add(new LightOccluder(this.position + new Vec2(-15f, -3f), this.position + new Vec2(-15f, 4f), new Color(1f, 1f, 1f)));
      this._occluders.Add(new LightOccluder(this.position + new Vec2(15f, -3f), this.position + new Vec2(15f, 4f), new Color(1f, 1f, 1f)));
      this._occluders.Add(new LightOccluder(this.position + new Vec2(-15f, -2f), this.position + new Vec2(15f, -2f), new Color(1f, 1f, 1f)));
      this.light = new PointLight(this.x, this.y - 1f, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 100f, this._occluders, true);
      Level.Add((Thing) this.light);
      this._shade = new SpriteThing(this.x, this.y, new Sprite("pyramidRoofLightShade"));
      this._shade.center = this.center;
      this._shade.layer = Layer.Foreground;
      Level.Add((Thing) this._shade);
    }

    public override void Update()
    {
      if (!this.did)
      {
        this.myBlock = Level.CheckPoint<Block>(new Vec2(this.x, this.y - 8f));
        this.did = true;
      }
      if (this.myBlock != null && this.myBlock.removeFromLevel)
      {
        Level.Remove((Thing) this);
        Level.Remove((Thing) this.light);
        Level.Remove((Thing) this._shade);
      }
      base.Update();
    }
  }
}
