// Decompiled with JetBrains decompiler
// Type: DuckGame.UndergroundSkyBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UndergroundSkyBackground : BackgroundUpdater
  {
    private float _speedMult;
    private bool _moving;

    public UndergroundSkyBackground(float xpos, float ypos, bool moving = false, float speedMult = 1f)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 1
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._speedMult = speedMult;
      this._moving = moving;
      this.visible = false;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      Level.current.backgroundColor = new Color(0, 0, 0);
      this._parallax = new ParallaxBackground("background/underground", 0.0f, 0.0f, 5);
      float speed = 0.9f * this._speedMult;
      float distance = 0.99f;
      this._parallax.AddZone(0, distance, speed);
      this._parallax.AddZone(1, distance, speed);
      this._parallax.AddZone(2, distance, speed);
      this._parallax.AddZone(3, distance, speed);
      this._parallax.AddZone(4, distance, speed);
      this._parallax.AddZone(5, distance, speed);
      this._parallax.AddZone(6, distance, speed);
      this._parallax.AddZone(7, distance, speed);
      this._parallax.AddZone(8, distance, speed);
      this._parallax.AddZone(9, distance, speed);
      this._parallax.AddZone(10, distance, speed);
      Level.Add((Thing) this._parallax);
      this._parallax.x -= 340f;
      this._parallax.restrictBottom = false;
      this._parallax.depth = (Depth) -0.9f;
      this._parallax.layer = new Layer("PARALLAX3", 115, new Camera(0.0f, 0.0f, 320f, 200f));
      Layer.Add(this._parallax.layer);
      Level.Add((Thing) this._parallax);
    }

    public override void Terminate() => Level.Remove((Thing) this._parallax);
  }
}
