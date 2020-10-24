// Decompiled with JetBrains decompiler
// Type: DuckGame.Trophy
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("stuff")]
  [BaggedProperty("canSpawn", false)]
  public class Trophy : Holdable
  {
    public StateBinding _actionBinding = new StateBinding(nameof (ownerAction));
    private bool ownerAction;
    private SpriteMap _sprite;
    private int _framesSinceThrown;
    private float _throwSpin;

    public Trophy(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("trophy", 17, 20);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 10f);
      this.collisionOffset = new Vec2(-7f, -10f);
      this.collisionSize = new Vec2(15f, 19f);
      this.depth = (Depth) -0.5f;
      this.thickness = 4f;
      this.weight = 4f;
      this.flammable = 0.3f;
      this.collideSounds.Add("rockHitGround2");
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._holdOffset = new Vec2(-2f, 0.0f);
    }

    public override void Update()
    {
      if (this.isServerForObject)
        this.ownerAction = this.action;
      if (this.duck != null && this.ownerAction)
      {
        this._holdOffset = Lerp.Vec2(this._holdOffset, new Vec2(-13f, -4f), 2f);
        this.angle = Lerp.Float(this.angle, -1f, 0.1f);
        this.handFlip = true;
        this.handOffset = Lerp.Vec2(this.handOffset, new Vec2(-3f, -4f), 1f);
        this._canRaise = false;
      }
      else
      {
        float num = 1f;
        if (this.duck != null)
          this.angle = Lerp.Float(this.angle, 0.0f, 0.1f * num);
        else
          num = 20f;
        this._holdOffset = Lerp.Vec2(this._holdOffset, new Vec2(-2f, 0.0f), num * 2f);
        this.handFlip = false;
        this.handOffset = Lerp.Vec2(this.handOffset, new Vec2(0.0f, 0.0f), 1f * num);
        this._canRaise = true;
      }
      if (this.owner == null && this.level.simulatePhysics)
      {
        if (this._framesSinceThrown == 0)
          this._throwSpin = this.angleDegrees;
        ++this._framesSinceThrown;
        if (this._framesSinceThrown > 15)
          this._framesSinceThrown = 15;
        this.angleDegrees = this._throwSpin;
        bool flag1 = false;
        bool flag2 = false;
        if (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 2.0 || !this.grounded) && ((double) this.gravMultiplier > 0.0 && !flag2) && !this._grounded)
        {
          if (this.offDir > (sbyte) 0)
            this._throwSpin += (float) (((double) Math.Abs(this.hSpeed * 2f) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
          else
            this._throwSpin -= (float) (((double) Math.Abs(this.hSpeed * 2f) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
          flag1 = true;
        }
        if (!flag1 || flag2)
        {
          this._throwSpin %= 360f;
          if ((double) this._throwSpin < 0.0)
            this._throwSpin += 360f;
          if (flag2)
            this._throwSpin = (double) Math.Abs(this._throwSpin - 90f) >= (double) Math.Abs(this._throwSpin + 90f) ? Lerp.Float(-90f, 0.0f, 16f) : Lerp.Float(this._throwSpin, 90f, 16f);
          else if ((double) this._throwSpin > 90.0 && (double) this._throwSpin < 270.0)
          {
            this._throwSpin = Lerp.Float(this._throwSpin, 180f, 14f);
          }
          else
          {
            if ((double) this._throwSpin > 180.0)
              this._throwSpin -= 360f;
            else if ((double) this._throwSpin < -180.0)
              this._throwSpin += 360f;
            this._throwSpin = Lerp.Float(this._throwSpin, 0.0f, 14f);
          }
        }
      }
      else
        this._throwSpin = 0.0f;
      base.Update();
    }
  }
}
