// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("details|arcade")]
  public class ArcadeScreen : Thing
  {
    private PointLight _light;
    private List<LightOccluder> _occluders = new List<LightOccluder>();

    public ArcadeScreen(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.center = new Vec2(5f, 4f);
      this._collisionSize = new Vec2(16f, 24f);
      this._collisionOffset = new Vec2(-8f, -22f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Ceiling;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._light = new PointLight(this.x + 1f, this.y, new Color(100, 130, 180), 60f, this._occluders);
      Level.Add((Thing) this._light);
    }

    public override void Update()
    {
      this._light.visible = this.visible;
      base.Update();
    }

    public override void Draw()
    {
      foreach (LightOccluder occluder in this._occluders)
        Graphics.DrawLine(occluder.p1, occluder.p2, Color.Red);
    }
  }
}
