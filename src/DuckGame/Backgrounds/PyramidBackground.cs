// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax")]
  public class PyramidBackground : BackgroundUpdater
  {
    public PyramidBackground(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 2
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(0.9f);
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Pyramid";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(26, 0, 0);
      Level.current.backgroundColor = this.backgroundColor;
      this._parallax = new ParallaxBackground("background/pyramid", 0.0f, 0.0f, 3);
      float speed = 0.4f;
      this._parallax.AddZone(0, 1f, 0.0f);
      this._parallax.AddZone(1, 1f, 0.0f);
      this._parallax.AddZone(2, 1f, 0.0f);
      this._parallax.AddZone(3, 1f, 0.0f);
      this._parallax.AddZone(4, 1f, 0.0f);
      this._parallax.AddZone(5, 1f, 0.0f);
      this._parallax.AddZone(6, 1f, 0.0f);
      this._parallax.AddZone(7, 1f, 0.0f);
      this._parallax.AddZone(8, 1f, 0.0f);
      this._parallax.AddZone(9, 1f, 0.0f);
      this._parallax.AddZone(10, 1f, 0.0f);
      this._parallax.AddZone(11, 1f, 0.0f);
      this._parallax.AddZone(12, 1f, 0.0f);
      this._parallax.AddZone(13, 1f, 0.0f);
      this._parallax.AddZone(14, 1f, 0.0f);
      this._parallax.AddZone(15, 1f, 0.0f);
      this._parallax.AddZone(16, 1f, 0.0f);
      this._parallax.AddZone(17, 1f, 0.0f);
      this._parallax.AddZone(18, 0.9f, speed, true);
      this._parallax.AddZone(19, 0.8f, speed, true);
      this._parallax.AddZone(20, 0.7f, speed, true);
      this._parallax.AddZone(21, 0.6f, speed, true);
      this._parallax.AddZone(22, 0.6f, speed, true);
      this._parallax.AddZone(23, 0.5f, speed, true);
      this._parallax.AddZone(24, 0.5f, speed, true);
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
