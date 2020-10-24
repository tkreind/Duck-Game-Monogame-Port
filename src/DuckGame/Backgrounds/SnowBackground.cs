// Decompiled with JetBrains decompiler
// Type: DuckGame.SnowBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax")]
  [BaggedProperty("isInDemo", false)]
  public class SnowBackground : BackgroundUpdater
  {
    public SnowBackground(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 0
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(0.9f);
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Snow";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(148, 178, 210);
      Level.current.backgroundColor = this.backgroundColor;
      this._parallax = new ParallaxBackground("background/snowSky", 0.0f, 0.0f, 3);
      float speed = 0.2f;
      this._parallax.AddZone(0, 0.9f, speed);
      this._parallax.AddZone(1, 0.9f, speed);
      this._parallax.AddZone(2, 0.9f, speed);
      this._parallax.AddZone(3, 0.9f, speed);
      this._parallax.AddZone(4, 0.9f, speed);
      this._parallax.AddZone(5, 0.9f, speed);
      this._parallax.AddZone(6, 0.9f, speed);
      this._parallax.AddZone(7, 0.9f, speed);
      this._parallax.AddZone(8, 0.9f, speed);
      this._parallax.AddZone(9, 0.9f, speed);
      this._parallax.AddZone(10, 0.8f, speed);
      this._parallax.AddZone(11, 0.7f, speed);
      this._parallax.AddZone(12, 0.6f, speed);
      this._parallax.AddZone(13, 0.5f, speed);
      this._parallax.AddZone(14, 0.4f, speed);
      this._parallax.AddZone(15, 0.3f, speed);
      Vec2 vec2 = new Vec2(0.0f, -12f);
      Sprite s1 = new Sprite("background/bigBerg1_reflection");
      s1.depth = new Depth(-0.9f);
      s1.position = new Vec2(-30f, 113f) + vec2;
      this._parallax.AddZoneSprite(s1, 12, 0.0f, 0.0f, true);
      Sprite s2 = new Sprite("background/bigBerg1");
      s2.depth = new Depth(-0.8f);
      s2.position = new Vec2(-31f, 50f) + vec2;
      this._parallax.AddZoneSprite(s2, 12, 0.0f, 0.0f, true);
      Sprite s3 = new Sprite("background/bigBerg2_reflection");
      s3.depth = new Depth(-0.9f);
      s3.position = new Vec2(210f, 108f) + vec2;
      this._parallax.AddZoneSprite(s3, 12, 0.0f, 0.0f, true);
      Sprite s4 = new Sprite("background/bigBerg2");
      s4.depth = new Depth(-0.8f);
      s4.position = new Vec2(211f, 52f) + vec2;
      this._parallax.AddZoneSprite(s4, 12, 0.0f, 0.0f, true);
      Sprite s5 = new Sprite("background/berg1_reflection");
      s5.depth = new Depth(-0.9f);
      s5.position = new Vec2(119f, 131f) + vec2;
      this._parallax.AddZoneSprite(s5, 13, 0.0f, 0.0f, true);
      Sprite s6 = new Sprite("background/berg1");
      s6.depth = new Depth(-0.8f);
      s6.position = new Vec2(121f, 114f) + vec2;
      this._parallax.AddZoneSprite(s6, 13, 0.0f, 0.0f, true);
      vec2 = new Vec2(-30f, -20f);
      Sprite s7 = new Sprite("background/berg2_reflection");
      s7.depth = new Depth(-0.9f);
      s7.position = new Vec2(69f, 153f) + vec2;
      this._parallax.AddZoneSprite(s7, 14, 0.0f, 0.0f, true);
      Sprite s8 = new Sprite("background/berg2");
      s8.depth = new Depth(-0.8f);
      s8.position = new Vec2(71f, 154f) + vec2;
      this._parallax.AddZoneSprite(s8, 14, 0.0f, 0.0f, true);
      vec2 = new Vec2(200f, 2f);
      Sprite s9 = new Sprite("background/berg3_reflection");
      s9.depth = new Depth(-0.9f);
      s9.position = new Vec2(70f, 153f) + vec2;
      this._parallax.AddZoneSprite(s9, 15, 0.0f, 0.0f, true);
      Sprite s10 = new Sprite("background/berg3");
      s10.depth = new Depth(-0.8f);
      s10.position = new Vec2(71f, 154f) + vec2;
      this._parallax.AddZoneSprite(s10, 15, 0.0f, 0.0f, true);
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
