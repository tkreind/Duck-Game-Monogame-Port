// Decompiled with JetBrains decompiler
// Type: DuckGame.BackgroundPyramid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background")]
  public class BackgroundPyramid : BackgroundTile
  {
    private bool inited;

    public BackgroundPyramid(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("pyramidBackground", 16, 16, true);
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._editorName = "Pyramid";
    }

    public override void Initialize() => base.Initialize();

    public override void Update()
    {
      if (!this.inited)
      {
        this.inited = true;
        SpriteMap graphic = this.graphic as SpriteMap;
        if (!this.flipHorizontal && graphic.frame % 8 == 0)
        {
          if (Level.CheckPoint<BackgroundPyramid>(this.position + new Vec2(-16f, 0.0f)) != null)
          {
            ++graphic.frame;
            graphic.UpdateFrame();
          }
        }
        else if (!this.flipHorizontal && graphic.frame % 8 == 7)
        {
          if (Level.CheckPoint<BackgroundPyramid>(this.position + new Vec2(16f, 0.0f)) != null)
          {
            --graphic.frame;
            graphic.UpdateFrame();
          }
        }
        else if (this.flipHorizontal && graphic.frame % 8 == 0)
        {
          if (Level.CheckPoint<BackgroundPyramid>(this.position + new Vec2(16f, 0.0f)) != null)
          {
            ++graphic.frame;
            graphic.UpdateFrame();
          }
        }
        else if (this.flipHorizontal && graphic.frame % 8 == 7 && Level.CheckPoint<BackgroundPyramid>(this.position + new Vec2(-16f, 0.0f)) != null)
        {
          --graphic.frame;
          graphic.UpdateFrame();
        }
      }
      base.Update();
    }
  }
}
