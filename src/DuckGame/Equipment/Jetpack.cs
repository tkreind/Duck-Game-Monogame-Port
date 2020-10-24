﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Jetpack
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("equipment")]
  public class Jetpack : Equipment
  {
    public StateBinding _onBinding = new StateBinding(nameof (_on));
    public StateBinding _heatBinding = new StateBinding(nameof (_heat));
    protected SpriteMap _sprite;
    public bool _on;
    public float _heat;

    public Jetpack(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("jetpack", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-5f, -5f);
      this.collisionSize = new Vec2(11f, 12f);
      this._offset = new Vec2(-3f, 3f);
      this._equippedDepth = -15;
      this._jumpMod = true;
      this.thickness = 0.1f;
      this._wearOffset = new Vec2(-2f, 0.0f);
    }

    public override void OnPressAction() => this._on = true;

    public override void OnReleaseAction() => this._on = false;

    public override void Update()
    {
      this._sprite.frame = (int) ((double) this._heat * 7.0);
      if (this._equippedDuck != null)
      {
        float num1 = 0.0f;
        this._offset = new Vec2(-3f, 3f);
        this.angle = 0.0f;
        if (this._equippedDuck.sliding && this._equippedDuck._trapped == null)
        {
          if (this._equippedDuck.offDir > (sbyte) 0)
            this.angle = -1.570796f;
          else
            this.angle = 1.570796f;
          this._offset.y += 12f;
          num1 -= 6f;
        }
        if (this._equippedDuck.crouch && !this._equippedDuck.sliding)
          this._offset.y += 4f;
        this.collisionOffset = new Vec2(0.0f, -9999f);
        this.collisionSize = new Vec2(0.0f, 0.0f);
        this.solid = false;
        PhysicsObject physicsObject1 = (PhysicsObject) this._equippedDuck;
        if (this._equippedDuck._trapped != null)
          physicsObject1 = (PhysicsObject) this._equippedDuck._trapped;
        else if (this._equippedDuck.ragdoll != null && this._equippedDuck.ragdoll.part1 != null)
          physicsObject1 = (PhysicsObject) this._equippedDuck.ragdoll.part1;
        this._sprite.flipH = this._equippedDuck._sprite.flipH;
        if (this._on && (double) this._heat < 1.0)
        {
          if (this._equippedDuck._trapped == null && this._equippedDuck.crouch)
            this._equippedDuck.sliding = true;
          if (this.isServerForObject)
            Global.data.jetFuelUsed.valueFloat += Maths.IncFrameTimer();
          this._heat += 11f / 1000f;
          if (physicsObject1 is RagdollPart)
          {
            float angle = this.angle;
            this.angle = physicsObject1.angle;
            Vec2 vec2_1 = this.Offset(new Vec2(0.0f, 8f));
            Level.Add((Thing) new JetpackSmoke(vec2_1.x, vec2_1.y));
            this.angle = angle;
            if ((double) physicsObject1.velocity.length < 7.0)
            {
              RagdollPart ragdollPart = physicsObject1 as RagdollPart;
              ragdollPart.addWeight = 0.2f;
              this._equippedDuck.ragdoll.jetting = true;
              float num2 = (float) -((double) physicsObject1.angle - 1.57079637050629);
              Vec2 vec2_2 = Vec2.Zero;
              if ((double) this._equippedDuck.inputProfile.leftStick.length > 0.100000001490116)
              {
                vec2_2 = new Vec2(this._equippedDuck.inputProfile.leftStick.x, -this._equippedDuck.inputProfile.leftStick.y);
              }
              else
              {
                vec2_2 = new Vec2(0.0f, 0.0f);
                if (this._equippedDuck.inputProfile.Down("LEFT"))
                  --vec2_2.x;
                if (this._equippedDuck.inputProfile.Down("RIGHT"))
                  ++vec2_2.x;
                if (this._equippedDuck.inputProfile.Down("UP"))
                  --vec2_2.y;
                if (this._equippedDuck.inputProfile.Down("DOWN"))
                  ++vec2_2.y;
              }
              if ((double) vec2_2.length < 0.100000001490116)
                vec2_2 = new Vec2((float) Math.Cos((double) num2), (float) -Math.Sin((double) num2));
              PhysicsObject physicsObject2 = physicsObject1;
              physicsObject2.velocity = physicsObject2.velocity + vec2_2 * 1.5f;
              if (ragdollPart.doll != null && ragdollPart.doll.part1 != null && (ragdollPart.doll.part2 != null && ragdollPart.doll.part3 != null))
              {
                ragdollPart.doll.part1.extraGravMultiplier = 0.4f;
                ragdollPart.doll.part2.extraGravMultiplier = 0.4f;
                ragdollPart.doll.part3.extraGravMultiplier = 0.7f;
              }
            }
          }
          else
          {
            Level.Add((Thing) new JetpackSmoke(this.x, this.y + 8f + num1));
            if ((double) this.angle > 0.0)
            {
              if ((double) physicsObject1.hSpeed < 6.0)
                physicsObject1.hSpeed += 0.9f;
            }
            else if ((double) this.angle < 0.0)
            {
              if ((double) physicsObject1.hSpeed > -6.0)
                physicsObject1.hSpeed -= 0.9f;
            }
            else if ((double) physicsObject1.vSpeed > -4.5)
              physicsObject1.vSpeed -= 0.38f;
          }
        }
        if ((double) this._heat >= 1.0)
          this._on = false;
        if (physicsObject1.grounded)
        {
          if ((double) this._heat > 0.0)
            this._heat -= 0.25f;
          else
            this._heat = 0.0f;
        }
      }
      else
      {
        this._sprite.flipH = false;
        this.collisionOffset = new Vec2(-5f, -5f);
        this.collisionSize = new Vec2(11f, 12f);
        this.solid = true;
      }
      base.Update();
    }
  }
}
