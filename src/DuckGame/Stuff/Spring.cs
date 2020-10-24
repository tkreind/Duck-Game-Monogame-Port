// Decompiled with JetBrains decompiler
// Type: DuckGame.Spring
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff|springs")]
  [BaggedProperty("isInDemo", true)]
  public class Spring : MaterialThing, IDontMove
  {
    protected SpriteMap _sprite;
    protected float _soundWait;
    protected float _mult;

    public Spring(float xpos, float ypos, float mult = 1f)
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
      this.collisionOffset = new Vec2(-8f, 0.0f);
      this.collisionSize = new Vec2(16f, 8f);
      this.depth = (Depth) -0.5f;
      this._editorName = nameof (Spring);
      this.thickness = 0.1f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._impactThreshold = 0.0f;
      this._mult = mult;
    }

    public override void Update()
    {
      if ((double) this._soundWait > 0.0)
        this._soundWait -= 0.1f;
      base.Update();
    }

    public void SpringUp()
    {
      this._sprite.currentAnimation = "spring";
      this._sprite.frame = 0;
      if ((double) this._soundWait > 0.0)
        return;
      SFX.Play("spring", 0.25f, Rando.Float(0.2f) - 0.1f);
      this._soundWait = 1f;
    }

    public override void Touch(MaterialThing with)
    {
      if (with.isServerForObject)
      {
        if ((double) with.vSpeed > -22.0 * (double) this._mult)
          with.vSpeed = -22f * this._mult;
        if (with is RagdollPart)
        {
          if ((double) Math.Abs(with.hSpeed) < 0.100000001490116)
            with.hSpeed = (double) Rando.Float(1f) >= 0.5 ? 1.3f : -1.3f;
          else
            with.hSpeed *= Rando.Float(1.1f, 1.4f);
        }
        if (with is Mine)
        {
          if ((double) Math.Abs(with.hSpeed) < 0.100000001490116)
            with.hSpeed = (double) Rando.Float(1f) >= 0.5 ? 1.2f : -1.2f;
          else
            with.hSpeed *= Rando.Float(1.1f, 1.2f);
        }
        with.lastHSpeed = with._hSpeed;
        with.lastVSpeed = with._vSpeed;
        if (with is Duck)
          (with as Duck).jumping = false;
        if (with is Gun)
          (with as Gun).PressAction();
      }
      this.SpringUp();
    }

    public override void Draw() => base.Draw();
  }
}
