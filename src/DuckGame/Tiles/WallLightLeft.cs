// Decompiled with JetBrains decompiler
// Type: DuckGame.WallLightLeft
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("details|arcade")]
  [BaggedProperty("isInDemo", true)]
  public class WallLightLeft : Thing
  {
    private PointLight _light;
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();

    public WallLightLeft(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("wallLight");
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(5f, 16f);
      this._collisionOffset = new Vec2(-7f, -8f);
      this.depth = new Depth(0.9f);
      this.hugWalls = WallHug.Left;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._occluders.Add(new LightOccluder(this.topLeft + new Vec2(-2f, 0.0f), this.topRight, new Color(1f, 0.8f, 0.8f)));
      this._occluders.Add(new LightOccluder(this.bottomLeft + new Vec2(-2f, 0.0f), this.bottomRight, new Color(1f, 0.8f, 0.8f)));
      this._light = new PointLight(this.x - 5f, this.y, new Color((int) byte.MaxValue, (int) byte.MaxValue, 190), 100f, this._occluders);
      Level.Add((Thing) this._light);
      this._shade = new SpriteThing(this.x, this.y, new Sprite("wallLight"));
      this._shade.center = this.center;
      this._shade.layer = Layer.Foreground;
      Level.Add((Thing) this._shade);
    }

    public override void Update()
    {
      this._light.visible = this.visible;
      base.Update();
    }

    public override void Draw() => base.Draw();
  }
}
