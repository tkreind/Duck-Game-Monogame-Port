// Decompiled with JetBrains decompiler
// Type: DuckGame.NatureBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("background|parallax")]
  public class NatureBackground : BackgroundUpdater
  {
    public NatureBackground(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 0
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Nature";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(110, 176, 222);
      Level.current.backgroundColor = this.backgroundColor;
      this._parallax = new ParallaxBackground("background/forest4", 0.0f, 0.0f, 3);
      float speed = 0.4f;
      Sprite s1 = new Sprite("background/cloud1");
      s1.depth = (Depth) -0.9f;
      s1.position = new Vec2(50f, 50f);
      this._parallax.AddZoneSprite(s1, 6, 0.72f, speed, true);
      Sprite s2 = new Sprite("background/cloud4");
      s2.depth = (Depth) -0.9f;
      s2.position = new Vec2(200f, 55f);
      this._parallax.AddZoneSprite(s2, 5, 0.72f, speed, true);
      Sprite s3 = new Sprite("background/cloud2");
      s3.depth = (Depth) -0.9f;
      s3.position = new Vec2(170f, 40f);
      this._parallax.AddZoneSprite(s3, 8, 0.82f, speed, true);
      Sprite s4 = new Sprite("background/cloud5");
      s4.depth = (Depth) -0.9f;
      s4.position = new Vec2(30f, 40f);
      this._parallax.AddZoneSprite(s4, 4, 0.82f, speed, true);
      Sprite s5 = new Sprite("background/cloud3");
      s5.depth = (Depth) -0.9f;
      s5.position = new Vec2(150f, 30f);
      this._parallax.AddZoneSprite(s5, 7, 0.91f, speed, true);
      this._parallax.AddZone(10, 0.68f, speed);
      this._parallax.AddZone(11, 0.65f, speed);
      this._parallax.AddZone(12, 0.65f, speed);
      this._parallax.AddZone(13, 0.65f, speed);
      this._parallax.AddZone(14, 0.6f, speed);
      this._parallax.AddZone(15, 0.6f, speed);
      this._parallax.AddZone(16, 0.6f, speed);
      this._parallax.AddZone(17, 0.6f, speed);
      this._parallax.AddZone(18, 0.6f, speed);
      this._parallax.AddZone(19, 0.6f, speed);
      this._parallax.AddZone(20, 0.6f, speed);
      this._parallax.AddZone(21, 0.6f, speed);
      this._parallax.AddZone(22, 0.6f, speed);
      this._parallax.AddZone(23, 0.55f, speed);
      this._parallax.AddZone(24, 0.5f, speed);
      this._parallax.AddZone(25, 0.45f, speed);
      this._parallax.AddZone(26, 0.4f, speed);
      this._parallax.AddZone(27, 0.35f, speed);
      this._parallax.AddZone(28, 0.3f, speed);
      this._parallax.AddZone(29, 0.25f, speed);
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
