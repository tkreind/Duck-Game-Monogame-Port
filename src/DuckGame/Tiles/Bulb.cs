// Decompiled with JetBrains decompiler
// Type: DuckGame.Bulb
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("details")]
  [BaggedProperty("isInDemo", true)]
  public class Bulb : Thing
  {
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();

    public Bulb(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("bulb");
      this.center = new Vec2(8f, 4f);
      this._collisionSize = new Vec2(4f, 6f);
      this._collisionOffset = new Vec2(-2f, -4f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Ceiling;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      Level.Add((Thing) new PointLight(this.x, this.y, new Color(155, 125, 100), 80f, this._occluders));
      this._shade = new SpriteThing(this.x, this.y, new Sprite("bulb"));
      this._shade.center = this.center;
      this._shade.layer = Layer.Foreground;
      Level.Add((Thing) this._shade);
    }
  }
}
