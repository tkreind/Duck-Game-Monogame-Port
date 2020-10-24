// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomParallaxSegment
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax|custom")]
  public class CustomParallaxSegment : Thing
  {
    public EditorProperty<int> ystart = new EditorProperty<int>(0, max: 40f, increment: 1f);
    public EditorProperty<int> yend = new EditorProperty<int>(0, max: 40f, increment: 1f);
    public EditorProperty<float> speed = new EditorProperty<float>(0.5f, max: 2f);
    public EditorProperty<float> distance = new EditorProperty<float>(0.0f, increment: 0.05f);
    public EditorProperty<bool> moving = new EditorProperty<bool>(false);
    private bool initializedParallax;

    public CustomParallaxSegment(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 6
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Parallax Segment";
      this._canFlip = false;
      this._canHaveChance = false;
    }

    public override void Update()
    {
      if (!this.initializedParallax)
      {
        CustomParallax customParallax = Level.current.FirstOfType<CustomParallax>();
        if (customParallax != null)
        {
          if (!customParallax.didInit)
            customParallax.DoInitialize();
          if (customParallax.parallax != null)
          {
            for (int ystart = (int) this.ystart; ystart <= (int) this.yend; ++ystart)
              customParallax.parallax.AddZone(ystart, this.distance.value, this.speed.value, this.moving.value);
          }
        }
        this.initializedParallax = true;
      }
      this.Initialize();
    }
  }
}
