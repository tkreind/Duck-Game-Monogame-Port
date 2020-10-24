// Decompiled with JetBrains decompiler
// Type: DuckGame.SpringDown
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|springs")]
  [BaggedProperty("isInDemo", false)]
  public class SpringDown : Spring
  {
    public SpringDown(float xpos, float ypos)
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
      this.collisionSize = new Vec2(16f, 8f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Spring Down";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.angleDegrees = 180f;
      this.hugWalls = WallHug.Ceiling;
    }

    public override void Touch(MaterialThing with)
    {
      if ((double) with.vSpeed < 12.0)
        with.vSpeed = 12f;
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
