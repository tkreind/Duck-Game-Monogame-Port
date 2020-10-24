// Decompiled with JetBrains decompiler
// Type: DuckGame.Sun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("details")]
  public class Sun : Thing
  {
    public Sun(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("officeLight");
      this.center = new Vec2(16f, 3f);
      this._collisionSize = new Vec2(30f, 6f);
      this._collisionOffset = new Vec2(-15f, -3f);
      this.depth = new Depth(0.9f);
      this.hugWalls = WallHug.Ceiling;
      this.layer = Layer.Game;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      Level.Add((Thing) new SunLight(this.x, this.y - 1f, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 100f));
      Level.Add((Thing) new SunLight(this.x, this.y - 1f, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 100f, vertical: true));
    }
  }
}
