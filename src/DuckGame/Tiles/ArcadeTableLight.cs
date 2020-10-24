// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeTableLight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("details|arcade")]
  [BaggedProperty("isInDemo", true)]
  public class ArcadeTableLight : Thing
  {
    private PointLight _light;
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();

    public ArcadeTableLight(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("arcade/bigFixture");
      this.center = new Vec2(35f, 24f);
      this._collisionSize = new Vec2(16f, 24f);
      this._collisionOffset = new Vec2(-8f, -22f);
      this.depth = new Depth(0.9f);
      this.hugWalls = WallHug.Ceiling;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._occluders.Add(new LightOccluder(this.position + new Vec2(-26f, 2f), this.position + new Vec2(-26f, -20f), new Color(1f, 0.7f, 0.7f)));
      this._occluders.Add(new LightOccluder(this.position + new Vec2(28f, 2f), this.position + new Vec2(28f, -20f), new Color(1f, 0.7f, 0.7f)));
      this._occluders.Add(new LightOccluder(this.position + new Vec2(-26f, -18f), this.position + new Vec2(28f, -18f), new Color(1f, 0.7f, 0.7f)));
      this._light = new PointLight(this.x + 1f, this.y - 16f, new Color((int) byte.MaxValue, (int) byte.MaxValue, 190), 130f, this._occluders);
      Level.Add((Thing) this._light);
      this._shade = new SpriteThing(this.x, this.y, new Sprite("arcade/bigFixture"));
      this._shade.center = this.center;
      this._shade.layer = Layer.Foreground;
      Level.Add((Thing) this._shade);
    }

    public override void Update()
    {
      this._light.visible = this.visible;
      this._shade.visible = this.visible;
      base.Update();
    }
  }
}
