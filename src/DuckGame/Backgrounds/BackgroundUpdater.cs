// Decompiled with JetBrains decompiler
// Type: DuckGame.BackgroundUpdater
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BackgroundUpdater : Thing
  {
    protected ParallaxBackground _parallax;
    protected float _lastCameraX;
    protected bool _update = true;
    protected bool _yParallax = true;
    protected float _yOffset;
    public Rectangle scissor = new Rectangle(0.0f, 0.0f, 0.0f, 0.0f);
    public float _extraYOffset;
    public Color backgroundColor;
    protected bool _skipMovement;

    public ParallaxBackground parallax => this._parallax;

    public bool update
    {
      get => this._update;
      set => this._update = value;
    }

    public void SetVisible(bool vis)
    {
      this._parallax.scissor = this.scissor;
      this._parallax.visible = vis;
      if ((double) this.scissor.width == 0.0)
        return;
      this._parallax.layer.scissor = this.scissor;
    }

    public BackgroundUpdater(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._isStatic = true;
      this._opaque = true;
    }

    public static Vec2 GetWallScissor()
    {
      Matrix matrix = Level.current.camera.getMatrix();
      int num1 = 0;
      int num2 = 0;
      foreach (RockWall rockWall in Level.current.things[typeof (RockWall)])
      {
        if (num2 == 0)
          num2 = Graphics.width;
        Vec2 vec2 = Vec2.Transform(rockWall.position, matrix);
        if (!rockWall.flipHorizontal && (double) vec2.x > (double) num1)
          num1 = (int) vec2.x;
        else if (rockWall.flipHorizontal && (double) vec2.x < (double) num2)
          num2 = (int) vec2.x;
      }
      if (num2 != 0)
        num2 -= num1;
      if (num2 == 0)
        num2 = Graphics.width;
      return new Vec2((float) num1, (float) num2);
    }

    public override void Update()
    {
      if (!this._update)
        return;
      if (!this._skipMovement)
      {
        float num = Level.current.camera.width * 4f / (float) Graphics.width;
        if (this._yParallax)
        {
          this._parallax.y = (float) (-((double) Level.current.camera.centerY / 12.0) - 5.0) + this._yOffset;
        }
        else
        {
          Layer.Parallax.camera = Level.current.camera;
          this._parallax.y = this._extraYOffset - 108f;
        }
        this._parallax.xmove = (this._lastCameraX - Level.current.camera.centerX) / num;
      }
      this._lastCameraX = Level.current.camera.centerX;
      if ((double) this.scissor.width != 0.0)
        this._parallax.scissor = this.scissor;
      base.Update();
    }

    public override ContextMenu GetContextMenu() => (ContextMenu) null;
  }
}
