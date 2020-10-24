// Decompiled with JetBrains decompiler
// Type: DuckGame.IndustrialBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax")]
  public class IndustrialBackground : BackgroundUpdater
  {
    public IndustrialBackground(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 2
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Industrial";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(26, 0, 0);
      Level.current.backgroundColor = this.backgroundColor;
      this._parallax = new ParallaxBackground("background/industrial", 0.0f, 0.0f, 3);
      float speed = 0.4f;
      this._parallax.AddZone(0, 0.0f, -speed, true);
      this._parallax.AddZone(1, 0.0f, -speed, true);
      this._parallax.AddZone(2, 0.0f, -speed, true);
      this._parallax.AddZone(3, 0.2f, -speed, true);
      this._parallax.AddZone(4, 0.2f, -speed, true);
      this._parallax.AddZone(5, 0.4f, -speed, true);
      this._parallax.AddZone(6, 0.6f, -speed, true);
      float distance = 0.8f;
      this._parallax.AddZone(16, distance, speed);
      this._parallax.AddZone(17, distance, speed);
      this._parallax.AddZone(18, distance, speed);
      this._parallax.AddZone(19, distance, speed);
      this._parallax.AddZone(20, distance, speed);
      this._parallax.AddZone(21, distance, speed);
      this._parallax.AddZone(22, 0.3f, speed);
      this._parallax.AddZone(23, 0.3f, speed);
      this._parallax.AddZone(24, 0.2f, speed);
      this._parallax.AddZone(25, 0.2f, speed);
      this._parallax.AddZone(26, 0.1f, speed);
      this._parallax.AddZone(27, 0.1f, speed);
      this._parallax.AddZone(28, 0.1f, speed);
      this._parallax.AddZone(29, 0.0f, speed);
      Level.Add((Thing) this._parallax);
    }

    public override void Update()
    {
      Vec2 wallScissor = BackgroundUpdater.GetWallScissor();
      if (wallScissor != Vec2.Zero)
        this.scissor = new Rectangle((float) (int) wallScissor.x, 0.0f, (float) (int) wallScissor.y, (float) Graphics.height);
      base.Update();
    }

    public override void Terminate() => Level.Remove((Thing) this._parallax);
  }
}
