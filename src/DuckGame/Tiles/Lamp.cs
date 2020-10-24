// Decompiled with JetBrains decompiler
// Type: DuckGame.Lamp
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("details")]
  [BaggedProperty("isInDemo", true)]
  public class Lamp : Thing
  {
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();

    public Lamp(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("lamp");
      this.center = new Vec2(7f, 28f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -15f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Floor;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._occluders.Add(new LightOccluder(this.position + new Vec2(-7f, -16f), this.position + new Vec2(-3f, -28f), new Color(1f, 0.7f, 0.7f)));
      this._occluders.Add(new LightOccluder(this.position + new Vec2(7f, -16f), this.position + new Vec2(3f, -28f), new Color(1f, 0.7f, 0.7f)));
      Level.Add((Thing) new PointLight(this.x, this.y - 24f, new Color((int) byte.MaxValue, (int) byte.MaxValue, 180), 100f, this._occluders));
      this._shade = new SpriteThing(this.x, this.y, new Sprite("lampShade"));
      this._shade.center = this.center;
      this._shade.layer = Layer.Foreground;
      Level.Add((Thing) this._shade);
    }
  }
}
