// Decompiled with JetBrains decompiler
// Type: DuckGame.UndergroundBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax")]
  public class UndergroundBackground : BackgroundUpdater
  {
    private float _speedMult;
    private bool _moving;
    private UndergroundRocksBackground _undergroundRocks;
    private UndergroundSkyBackground _skyline;

    public UndergroundBackground(float xpos, float ypos, bool moving = false, float speedMult = 1f)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 4
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._speedMult = speedMult;
      this._moving = moving;
      this._editorName = "Bunker";
      this._yParallax = false;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(0, 0, 0);
      Level.current.backgroundColor = this.backgroundColor;
      this._parallax = new ParallaxBackground("background/underground", 0.0f, 0.0f, 5);
      float speed = 0.9f * this._speedMult;
      this._parallax.AddZone(11, 1f, speed);
      this._parallax.AddZone(12, 1f, speed);
      this._parallax.AddZone(13, 1f, speed);
      this._parallax.AddZone(14, 0.98f, speed);
      this._parallax.AddZone(15, 0.97f, speed);
      this._parallax.AddZone(16, 0.75f, speed);
      this._parallax.AddZone(17, 0.75f, speed);
      this._parallax.AddZone(18, 0.75f, speed);
      this._parallax.AddZone(19, 0.75f, speed);
      this._parallax.AddZone(20, 0.75f, speed);
      Level.Add((Thing) this._parallax);
      this._parallax.x -= 340f;
      this._parallax.restrictBottom = false;
      this._undergroundRocks = new UndergroundRocksBackground(this.x, this.y);
      Level.Add((Thing) this._undergroundRocks);
      this._skyline = new UndergroundSkyBackground(this.x, this.y);
      Level.Add((Thing) this._skyline);
    }

    public override void Update()
    {
      Vec2 position = Vec2.Transform(new Vec2(0.0f, 10f), Level.current.camera.getMatrix());
      int num1 = (int) position.y;
      if (num1 < 0)
        num1 = 0;
      if (num1 > Graphics.height)
        num1 = Graphics.height;
      Vec2 wallScissor = BackgroundUpdater.GetWallScissor();
      this._undergroundRocks.scissor = new Rectangle((float) (int) wallScissor.x, (float) num1, (float) (int) wallScissor.y, (float) (Graphics.height - num1));
      position = new Vec2(0.0f, -10f);
      Matrix matrix = Level.current.camera.getMatrix();
      position = Vec2.Transform(position, matrix);
      int num2 = (int) position.y;
      if (num2 < 0)
        num2 = 0;
      if (num2 > Graphics.height)
        num2 = Graphics.height;
      this._skyline.scissor = new Rectangle((float) (int) wallScissor.x, 0.0f, (float) (int) wallScissor.y, (float) num2);
      base.Update();
    }

    public override void Terminate() => Level.Remove((Thing) this._parallax);
  }
}
