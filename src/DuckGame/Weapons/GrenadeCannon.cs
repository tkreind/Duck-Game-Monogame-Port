﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.GrenadeCannon
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("guns|explosives")]
  public class GrenadeCannon : Gun
  {
    public StateBinding _fireAngleState = new StateBinding(nameof (_fireAngle));
    public StateBinding _aimAngleState = new StateBinding(nameof (_aimAngle));
    public StateBinding _aimWaitState = new StateBinding(nameof (_aimWait));
    public StateBinding _aimingState = new StateBinding(nameof (_aiming));
    public StateBinding _cooldownState = new StateBinding(nameof (_cooldown));
    public StateBinding _timerBinding = new StateBinding(nameof (_timer));
    public bool _doLoad;
    public bool _doneLoad;
    public float _timer = 1.2f;
    public float _fireAngle;
    public float _aimAngle;
    public float _aimWait;
    public bool _aiming;
    public float _cooldown;
    private SpriteMap _sprite;

    public override float angle
    {
      get => base.angle + this._aimAngle;
      set => this._angle = value;
    }

    public GrenadeCannon(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._type = "gun";
      this._sprite = new SpriteMap("grenadecannon", 26, 12);
      this._sprite.AddAnimation("idle4", 0.4f, false, new int[1]);
      this._sprite.AddAnimation("load4", 0.4f, false, 1, 2, 3, 4);
      this._sprite.AddAnimation("idle3", 0.4f, false, 5);
      this._sprite.AddAnimation("load3", 0.4f, false, 6, 7, 8, 9);
      this._sprite.AddAnimation("idle2", 0.4f, false, 10);
      this._sprite.AddAnimation("load2", 0.4f, false, 11, 12, 13, 14);
      this._sprite.AddAnimation("idle1", 0.4f, false, 15);
      this._sprite.AddAnimation("load1", 0.4f, false, 16, 17, 18, 19);
      this._sprite.AddAnimation("idle0", 0.4f, false, 20);
      this._sprite.SetAnimation("idle4");
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(6f, 7f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(22f, 6f);
      this._laserOffsetTL = new Vec2(22f, 6f);
      this._fireSound = "pistol";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-3f, 0.0f);
      this._ammoType = (AmmoType) new ATGrenade();
      this._fireSound = "deepMachineGun";
      this._bulletColor = Color.White;
    }

    public override void Update()
    {
      base.Update();
      if (this._doLoad && this._sprite.finished)
      {
        GrenadePin grenadePin = new GrenadePin(this.x, this.y);
        grenadePin.hSpeed = (float) -this.offDir * (1.5f + Rando.Float(0.5f));
        grenadePin.vSpeed = -2f;
        Level.Add((Thing) grenadePin);
        SFX.Play("pullPin");
        this._doneLoad = true;
        this._doLoad = false;
      }
      if (this._doneLoad)
        this._timer -= 0.01f;
      if ((double) this._timer <= 0.0)
      {
        this._timer = 1.2f;
        this._doneLoad = false;
        this._doLoad = false;
        Vec2 vec2 = this.Offset(this.barrelOffset);
        --this.ammo;
        Vec2 vec = Maths.AngleToVec(this.barrelAngle + Rando.Float(-0.1f, 0.1f));
        for (int index = 0; index < 12; ++index)
          Level.Add((Thing) SmallFire.New(vec2.x, vec2.y, vec.x * Rando.Float(3.5f, 5f) + Rando.Float(-2f, 2f), vec.y * Rando.Float(3.5f, 5f) + Rando.Float(-2f, 2f)));
        for (int index = 0; index < 6; ++index)
          Level.Add((Thing) SmallSmoke.New(vec2.x + Rando.Float(-2f, 2f), vec2.y + Rando.Float(-2f, 2f)));
        this._sprite.SetAnimation("idle" + Math.Min(this.ammo, 4).ToString());
        this.kick = 1f;
        this._aiming = false;
        this._cooldown = 1f;
        this._fireAngle = 0.0f;
        if (this.owner != null)
        {
          this.owner.hSpeed -= vec.x * 4f;
          this.owner.vSpeed -= vec.y * 4f;
          if (this.owner is Duck owner && owner.crouch)
            owner.sliding = true;
        }
        else
        {
          this.hSpeed -= vec.x * 4f;
          this.vSpeed -= vec.y * 4f;
        }
      }
      if (this._doneLoad && this._aiming)
        this.laserSight = true;
      if (this._aiming && (double) this._aimWait <= 0.0 && (double) this._fireAngle < 90.0)
        this._fireAngle += 3f;
      if ((double) this._aimWait > 0.0)
        this._aimWait -= 0.9f;
      if ((double) this._cooldown > 0.0)
        this._cooldown -= 0.1f;
      else
        this._cooldown = 0.0f;
      if (this.owner != null)
      {
        this._aimAngle = -Maths.DegToRad(this._fireAngle);
        if (this.offDir < (sbyte) 0)
          this._aimAngle = -this._aimAngle;
      }
      else
      {
        this._aimWait = 0.0f;
        this._aiming = false;
        this._aimAngle = 0.0f;
        this._fireAngle = 0.0f;
      }
      if (!this._raised)
        return;
      this._aimAngle = 0.0f;
    }

    public override void OnPressAction()
    {
      if (!this._doneLoad && !this._doLoad)
      {
        this._sprite.SetAnimation("load" + Math.Min(this.ammo, 4).ToString());
        this._doLoad = true;
      }
      if (!this._doneLoad || (double) this._cooldown != 0.0)
        return;
      if (this.ammo > 0)
      {
        this._aiming = true;
        this._aimWait = 1f;
      }
      else
        SFX.Play("click");
    }

    public override void OnReleaseAction()
    {
      if (!this._doneLoad || (double) this._cooldown != 0.0 || (!this._aiming || this.ammo <= 0))
        return;
      this._aiming = false;
      --this.ammo;
      this.kick = 1f;
      if (!this.receivingPress && this.isServerForObject)
      {
        Vec2 vec2 = this.Offset(this.barrelOffset);
        float radians = this.barrelAngle + Rando.Float(-0.1f, 0.1f);
        CannonGrenade cannonGrenade = new CannonGrenade(vec2.x, vec2.y);
        cannonGrenade._pin = false;
        cannonGrenade._timer = this._timer;
        this.Fondle((Thing) cannonGrenade);
        Vec2 vec = Maths.AngleToVec(radians);
        cannonGrenade.hSpeed = vec.x * 10f;
        cannonGrenade.vSpeed = vec.y * 10f;
        Level.Add((Thing) cannonGrenade);
        this._timer = 1.2f;
        this._doneLoad = false;
        this._doLoad = false;
        this._sprite.SetAnimation("idle" + Math.Min(this.ammo, 4).ToString());
      }
      this._cooldown = 1f;
      this.angle = 0.0f;
      this._fireAngle = 0.0f;
    }

    public override void Fire()
    {
    }
  }
}
