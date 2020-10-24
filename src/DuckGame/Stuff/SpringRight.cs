// Decompiled with JetBrains decompiler
// Type: DuckGame.SpringRight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|springs")]
  [BaggedProperty("isInDemo", false)]
  public class SpringRight : Spring
  {
    public override bool flipHorizontal
    {
      get => this._flipHorizontal;
      set
      {
        this._flipHorizontal = value;
        this.offDir = this._flipHorizontal ? (sbyte) -1 : (sbyte) 1;
        if (!this._flipHorizontal)
        {
          this.center = new Vec2(8f, 7f);
          this.collisionOffset = new Vec2(-8f, -8f);
          this.collisionSize = new Vec2(8f, 16f);
          this.angleDegrees = 90f;
          this.hugWalls = WallHug.Left;
        }
        else
        {
          this.center = new Vec2(8f, 7f);
          this.collisionOffset = new Vec2(0.0f, -8f);
          this.collisionSize = new Vec2(8f, 16f);
          this.angleDegrees = -90f;
          this.hugWalls = WallHug.Right;
        }
      }
    }

    public SpringRight(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("spring", 16, 15);
      this._sprite.ClearAnimations();
      this._sprite.AddAnimation("idle", 1f, false, new int[1]);
      this._sprite.AddAnimation("spring", 4f, false, 1, 2, 1, 0);
      this._sprite.SetAnimation("idle");
      this._sprite.speed = 0.1f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 7f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(8f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Spring Right";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.angleDegrees = 90f;
      this.hugWalls = WallHug.Left;
    }

    public override void Touch(MaterialThing with)
    {
      if (!this._flipHorizontal)
      {
        if ((double) with.hSpeed < 12.0)
          with.hSpeed = 12f;
      }
      else if ((double) with.hSpeed > -12.0)
        with.hSpeed = -12f;
      if (with is Gun)
        (with as Gun).PressAction();
      if (with is Duck)
        (with as Duck).jumping = false;
      with.lastHSpeed = with._hSpeed;
      with.lastVSpeed = with._vSpeed;
      this.SpringUp();
    }

    public override void Draw() => base.Draw();
  }
}
