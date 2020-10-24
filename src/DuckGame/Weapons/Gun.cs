﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Gun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public abstract class Gun : Holdable
  {
    protected AmmoType _ammoType;
    public StateBinding _ammoBinding = new StateBinding(nameof (netAmmo));
    public StateBinding _waitBinding = new StateBinding(nameof (_wait));
    public StateBinding _loadedBinding = new StateBinding(nameof (loaded));
    public StateBinding _bulletFireIndexBinding = new StateBinding(nameof (bulletFireIndex));
    public StateBinding _heatBinding = new StateBinding("heat");
    public StateBinding _infiniteAmmoValBinding = new StateBinding(nameof (infiniteAmmoVal));
    public byte bulletFireIndex;
    public bool held;
    public float kick;
    protected float _kickForce = 3f;
    public int ammo;
    public bool firing;
    public bool tamping;
    public float tampPos;
    public float loseAccuracy;
    public float _accuracyLost;
    public float maxAccuracyLost;
    protected Color _bulletColor = Color.White;
    protected bool _lowerOnFire = true;
    public bool receivingPress;
    public bool hasFireEvents;
    public bool onlyFireAction;
    protected float _fireSoundPitch;
    public EditorProperty<bool> infinite;
    public bool infiniteAmmoVal;
    protected string _bio = "No Info.";
    protected Vec2 _barrelOffsetTL = new Vec2();
    protected Vec2 _laserOffsetTL = new Vec2();
    protected float _barrelAngleOffset;
    protected string _fireSound = "pistol";
    protected string _clickSound = "click";
    public float _wait;
    public float _fireWait = 1f;
    public bool loaded = true;
    private bool _laserInit;
    protected bool _fullAuto;
    protected int _numBulletsPerFire = 1;
    protected bool _manualLoad;
    public bool laserSight;
    protected SpriteMap _flare;
    protected float _flareAlpha;
    private SpriteMap _barrelSmoke;
    protected float _barrelHeat;
    protected float _smokeWait;
    protected float _smokeAngle;
    protected float _smokeFlatten;
    private SinWave _accuracyWave = (SinWave) 0.3f;
    private SpriteMap _clickPuff;
    private Tex2D _laserTex;
    protected Vec2 _wallPoint = new Vec2();
    protected Sprite _sightHit;
    private bool _doPuff;
    public byte _framesSinceThrown;
    public List<Bullet> firedBullets = new List<Bullet>();
    private Material _additiveMaterial;

    public bool CanSpawnInfinite()
    {
      switch (this)
      {
        case FlareGun _:
        case QuadLaser _:
        case RomanCandle _:
        case Matchbox _:
        case FireCrackers _:
          return false;
        default:
          return !(this is NetGun);
      }
    }

    public AmmoType ammoType => this._ammoType;

    public sbyte netAmmo
    {
      get => (sbyte) this.ammo;
      set => this.ammo = (int) value;
    }

    public bool lowerOnFire => this._lowerOnFire;

    public string bio => this._bio;

    public Vec2 barrelPosition => this.Offset(this.barrelOffset);

    public Vec2 barrelOffset => this._barrelOffsetTL - this.center + this._extraOffset;

    public Vec2 laserOffset => this._laserOffsetTL - this.center;

    public Vec2 barrelVector => this.Offset(this.barrelOffset) - this.Offset(this.barrelOffset + new Vec2(-1f, 0.0f));

    public override float angle
    {
      get => this._angle + (float) this._accuracyWave * (this._accuracyLost * 0.5f);
      set => this._angle = value;
    }

    public float barrelAngleOffset => this._barrelAngleOffset;

    public float barrelAngle => Maths.DegToRad(Maths.PointDirection(Vec2.Zero, this.barrelVector) + this._barrelAngleOffset * (float) this.offDir);

    public bool CanSpin() => (double) this.weight <= 5.0;

    public override void EditorPropertyChanged(object property)
    {
      this.infiniteAmmoVal = this.infinite.value;
      this.UpdateMaterial();
    }

    public virtual void OnNetworkBulletsFired(Vec2 pos)
    {
    }

    public void UpdateMaterial()
    {
      if (this.infinite.value)
        this.infiniteAmmoVal = true;
      if (this.infiniteAmmoVal)
      {
        if (this.material != null)
          return;
        this.material = (Material) new MaterialGold((Thing) this);
      }
      else if (this.material == null && (double) this.heat > 0.100000001490116 && this.physicsMaterial == PhysicsMaterial.Metal)
      {
        this.material = (Material) new MaterialRedHot((Thing) this);
      }
      else
      {
        if (!(this.material is MaterialRedHot))
          return;
        if ((double) this.heat < 0.100000001490116)
          this.material = (Material) null;
        else
          (this.material as MaterialRedHot).intensity = Math.Min(this.heat - 0.1f, 1f);
      }
    }

    public bool fullAuto => this._fullAuto;

    public Gun(float xval, float yval)
      : base(xval, yval)
    {
      this._flare = new SpriteMap("smallFlare", 11, 10);
      this._flare.center = new Vec2(0.0f, 5f);
      this._barrelSmoke = new SpriteMap("barrelSmoke", 8, 8);
      this._barrelSmoke.center = new Vec2(1f, 8f);
      this._barrelSmoke.ClearAnimations();
      this._barrelSmoke.AddAnimation("puff", 1f, false, 0, 1, 2);
      this._barrelSmoke.AddAnimation("loop", 1f, true, 3, 4, 5, 6, 7, 8);
      this._barrelSmoke.AddAnimation("finish", 1f, false, 9, 10, 11, 12);
      this._barrelSmoke.SetAnimation("puff");
      this._barrelSmoke.speed = 0.0f;
      this._translucent = true;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._dontCrush = true;
      this._clickPuff = new SpriteMap("clickPuff", 16, 16);
      this._clickPuff.AddAnimation("puff", 0.3f, false, 0, 1, 2, 3);
      this._clickPuff.center = new Vec2(0.0f, 12f);
      this._sightHit = new Sprite("laserSightHit");
      this._sightHit.CenterOrigin();
      this.depth = (Depth) -0.1f;
      this.infinite = new EditorProperty<bool>(false, (Thing) this);
      this.collideSounds.Add("smallMetalCollide");
      this.impactVolume = 0.3f;
    }

    public void DoAmmoClick()
    {
      this._doPuff = true;
      this._clickPuff.frame = 0;
      this._clickPuff.SetAnimation("puff");
      this._barrelHeat = 0.0f;
      this._barrelSmoke.SetAnimation("finish");
      SFX.Play(this._clickSound);
      for (int index = 0; index < 2; ++index)
      {
        SmallSmoke smallSmoke = SmallSmoke.New(this.barrelPosition.x, this.barrelPosition.y);
        smallSmoke.scale = new Vec2(0.3f, 0.3f);
        smallSmoke.hSpeed = Rando.Float(-0.1f, 0.1f);
        smallSmoke.vSpeed = -Rando.Float(0.05f, 0.2f);
        smallSmoke.alpha = 0.6f;
        Level.Add((Thing) smallSmoke);
      }
    }

    public override void HeatUp(Vec2 location)
    {
      if (this._ammoType == null || !this._ammoType.combustable || (this.ammo <= 0 || (double) this.heat <= 1.0) || (double) Rando.Float(1f) <= 0.800000011920929)
        return;
      this.heat -= 0.05f;
      this.PressAction();
      if ((double) Rando.Float(1f) <= 0.400000005960464)
        return;
      SFX.Play("bulletPop", Rando.Float(0.5f, 1f), Rando.Float(-1f, 1f));
    }

    public override void DoUpdate()
    {
      if (this.laserSight && this._laserTex == null)
      {
        this._additiveMaterial = new Material("Shaders/basicAdd");
        this._laserTex = Content.Load<Tex2D>("pointerLaser");
      }
      base.DoUpdate();
    }

    public override void Update()
    {
      if (this.infiniteAmmoVal)
        this.ammo = 99;
      if (TeamSelect2.Enabled("INFAMMO"))
      {
        this.infinite.value = true;
        this.infiniteAmmoVal = true;
      }
      this.UpdateMaterial();
      base.Update();
      if (this._clickPuff.finished)
        this._doPuff = false;
      this._accuracyLost = Maths.CountDown(this._accuracyLost, 0.015f);
      if ((double) this._flareAlpha > 0.0)
        this._flareAlpha -= 0.5f;
      else
        this._flareAlpha = 0.0f;
      if ((double) this._barrelHeat > 0.0)
        this._barrelHeat -= 0.01f;
      else
        this._barrelHeat = 0.0f;
      if ((double) this._barrelHeat > 10.0)
        this._barrelHeat = 10f;
      if ((double) this._smokeWait > 0.0)
      {
        this._smokeWait -= 0.1f;
      }
      else
      {
        if ((double) this._barrelHeat > 0.100000001490116 && (double) this._barrelSmoke.speed == 0.0)
        {
          this._barrelSmoke.SetAnimation("puff");
          this._barrelSmoke.speed = 0.1f;
        }
        if ((double) this._barrelSmoke.speed > 0.0 && this._barrelSmoke.currentAnimation == "puff" && this._barrelSmoke.finished)
          this._barrelSmoke.SetAnimation("loop");
        if ((double) this._barrelSmoke.speed > 0.0 && this._barrelSmoke.currentAnimation == "loop" && (this._barrelSmoke.frame == 5 && (double) this._barrelHeat < 0.100000001490116))
          this._barrelSmoke.SetAnimation("finish");
      }
      if ((double) this._smokeWait > 0.0 && (double) this._barrelSmoke.speed > 0.0)
        this._barrelSmoke.SetAnimation("finish");
      if (this._barrelSmoke.currentAnimation == "finish" && this._barrelSmoke.finished)
        this._barrelSmoke.speed = 0.0f;
      if (this.owner != null)
      {
        if ((double) this.owner.hSpeed > 0.100000001490116)
          this._smokeAngle -= 0.1f;
        else if ((double) this.owner.hSpeed < -0.100000001490116)
          this._smokeAngle += 0.1f;
        if ((double) this._smokeAngle > 0.400000005960464)
          this._smokeAngle = 0.4f;
        if ((double) this._smokeAngle < -0.400000005960464)
          this._smokeAngle = -0.4f;
        if ((double) this.owner.vSpeed > 0.100000001490116)
          this._smokeFlatten -= 0.1f;
        else if ((double) this.owner.vSpeed < -0.100000001490116)
          this._smokeFlatten += 0.1f;
        if ((double) this._smokeFlatten > 0.5)
          this._smokeFlatten = 0.5f;
        if ((double) this._smokeFlatten < -0.5)
          this._smokeFlatten = -0.5f;
        this._framesSinceThrown = (byte) 0;
      }
      else
      {
        ++this._framesSinceThrown;
        if (this._framesSinceThrown > (byte) 15)
          this._framesSinceThrown = (byte) 15;
      }
      if (!(this is Sword) && this.owner == null && (this.CanSpin() && Level.current.simulatePhysics))
      {
        bool flag1 = false;
        bool flag2 = false;
        if (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 2.0 || !this.grounded) && ((double) this.gravMultiplier > 0.0 && !flag2) && !this._grounded)
        {
          if (this.offDir > (sbyte) 0)
            this.angleDegrees += (float) (((double) Math.Abs(this.hSpeed * 2f) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
          else
            this.angleDegrees -= (float) (((double) Math.Abs(this.hSpeed * 2f) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
          flag1 = true;
        }
        if (!flag1 || flag2)
        {
          this.angleDegrees %= 360f;
          if ((double) this.angleDegrees < 0.0)
            this.angleDegrees += 360f;
          if (flag2)
          {
            if ((double) Math.Abs(this.angleDegrees - 90f) < (double) Math.Abs(this.angleDegrees + 90f))
              this.angleDegrees = Lerp.Float(this.angleDegrees, 90f, 16f);
            else
              this.angleDegrees = Lerp.Float(-90f, 0.0f, 16f);
          }
          else if ((double) this.angleDegrees > 90.0 && (double) this.angleDegrees < 270.0)
          {
            this.angleDegrees = Lerp.Float(this.angleDegrees, 180f, 14f);
          }
          else
          {
            if ((double) this.angleDegrees > 180.0)
              this.angleDegrees -= 360f;
            else if ((double) this.angleDegrees < -180.0)
              this.angleDegrees += 360f;
            this.angleDegrees = Lerp.Float(this.angleDegrees, 0.0f, 14f);
          }
        }
      }
      float num = (float) (1.0 - (Math.Sin((double) Maths.DegToRad(this.angleDegrees + 90f)) + 1.0) / 2.0);
      if (this._owner == null)
        this._extraOffset.y = num * (this._collisionOffset.y + this._collisionSize.y + this._collisionOffset.y);
      else
        this._extraOffset.y = 0.0f;
      if (this.owner == null || (double) this.owner.hSpeed > -0.100000001490116 && (double) this.owner.hSpeed < 0.100000001490116)
      {
        if ((double) this._smokeAngle >= 0.100000001490116)
          this._smokeAngle -= 0.1f;
        else if ((double) this._smokeAngle <= -0.100000001490116)
          this._smokeAngle += 0.1f;
        else
          this._smokeAngle = 0.0f;
      }
      if (this.owner == null || (double) this.owner.vSpeed > -0.100000001490116 && (double) this.owner.vSpeed < 0.100000001490116)
      {
        if ((double) this._smokeFlatten >= 0.100000001490116)
          this._smokeFlatten -= 0.1f;
        else if ((double) this._smokeFlatten <= -0.100000001490116)
          this._smokeFlatten += 0.1f;
        else
          this._smokeFlatten = 0.0f;
      }
      if ((double) this.kick > 0.0)
        this.kick -= 0.2f;
      else
        this.kick = 0.0f;
      if (this.owner == null)
      {
        if (this.ammo <= 0 && ((double) this.alpha < 0.990000009536743 || this.grounded && (double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) < 0.300000011920929))
        {
          this.canPickUp = false;
          this.alpha -= 10.2f;
          this.weight = 0.01f;
        }
        if ((double) this.alpha < 0.0)
          Level.Remove((Thing) this);
      }
      if (this.owner != null)
        this.graphic.flipH = this.owner.graphic.flipH;
      if ((double) this._wait > 0.0)
        this._wait -= 0.15f;
      if ((double) this._wait >= 0.0)
        return;
      this._wait = 0.0f;
    }

    public override void Terminate()
    {
      if (!(Level.current is Editor))
      {
        Level.Add((Thing) SmallSmoke.New(this.x, this.y));
        Level.Add((Thing) SmallSmoke.New(this.x + 4f, this.y));
        Level.Add((Thing) SmallSmoke.New(this.x - 4f, this.y));
        Level.Add((Thing) SmallSmoke.New(this.x, this.y + 4f));
        Level.Add((Thing) SmallSmoke.New(this.x, this.y - 4f));
      }
      base.Terminate();
    }

    public override void PressAction()
    {
      if (this.isServerForObject && TeamSelect2.Enabled("GUNEXPL") && this.ammo <= 0)
      {
        if (this.duck == null)
          return;
        this.duck.ThrowItem();
        Level.Remove((Thing) this);
        for (int index = 0; index < 1; ++index)
        {
          ExplosionPart explosionPart = new ExplosionPart(this.x - 8f + Rando.Float(16f), this.y - 8f + Rando.Float(16f));
          explosionPart.xscale *= 0.7f;
          explosionPart.yscale *= 0.7f;
          Level.Add((Thing) explosionPart);
        }
        SFX.Play("explode");
        List<Bullet> varBullets = new List<Bullet>();
        for (int index = 0; index < 12; ++index)
        {
          float num = (float) ((double) index * 30.0 - 10.0) + Rando.Float(20f);
          ATShrapnel atShrapnel = new ATShrapnel();
          atShrapnel.range = 25f + Rando.Float(10f);
          Bullet bullet = new Bullet(this.x + (float) (Math.Cos((double) Maths.DegToRad(num)) * 8.0), this.y - (float) (Math.Sin((double) Maths.DegToRad(num)) * 8.0), (AmmoType) atShrapnel, num);
          bullet.firedFrom = (Thing) this;
          varBullets.Add(bullet);
          Level.Add((Thing) bullet);
        }
        if (!Network.isActive)
          return;
        Send.Message((NetMessage) new NMExplodingProp(varBullets), NetMessagePriority.ReliableOrdered);
        varBullets.Clear();
      }
      else
        base.PressAction();
    }

    public override void OnPressAction()
    {
      if (this._fullAuto)
        return;
      this.Fire();
    }

    public override void OnHoldAction()
    {
      if (!this._fullAuto)
        return;
      this.Fire();
    }

    public void ApplyKick()
    {
      if (this.owner == null)
        return;
      if ((double) this._kickForce > 0.0)
      {
        if (this.owner is Duck && (this.owner as Duck).ragdoll != null && ((this.owner as Duck).HasEquipment(typeof (FancyShoes)) && (this.owner as Duck).ragdoll.part2 != null))
        {
          Duck owner = this.owner as Duck;
          Vec2 vec2 = -this.barrelVector * this._kickForce;
          owner.ragdoll.part2.hSpeed += vec2.x;
          owner.ragdoll.part2.vSpeed += vec2.y;
        }
        else
        {
          Vec2 vec2 = -this.barrelVector * this._kickForce;
          if (Math.Sign(this.owner.hSpeed) != Math.Sign(vec2.x) || (double) Math.Abs(vec2.x) > (double) Math.Abs(this.owner.hSpeed))
            this.owner.hSpeed = vec2.x;
          if (this.owner is Duck owner)
          {
            if (owner.crouch)
              owner.sliding = true;
            this.owner.vSpeed += vec2.y - this._kickForce * 0.333f;
          }
          else
            this.owner.vSpeed += vec2.y - this._kickForce * 0.333f;
        }
      }
      this.kick = 1f;
    }

    public virtual void Fire()
    {
      if (!this.loaded)
        return;
      this.firedBullets.Clear();
      if (this.ammo > 0 && (double) this._wait == 0.0)
      {
        this.ApplyKick();
        for (int index = 0; index < this._numBulletsPerFire; ++index)
        {
          float accuracy = this._ammoType.accuracy;
          this._ammoType.accuracy *= 1f - this._accuracyLost;
          this._ammoType.bulletColor = this._bulletColor;
          float angleDegrees = this.angleDegrees;
          float angle = this.offDir >= (sbyte) 0 ? angleDegrees + this._ammoType.barrelAngleDegrees : angleDegrees + 180f - this._ammoType.barrelAngleDegrees;
          if (!this.receivingPress)
          {
            if (this._ammoType is ATDart)
            {
              if (this.isServerForObject)
              {
                Vec2 vec2 = this.Offset(this.barrelOffset);
                Dart dart = new Dart(vec2.x, vec2.y, this.owner as Duck, -angle);
                this.Fondle((Thing) dart);
                if (this.onFire || (double) this._barrelHeat > 6.0)
                {
                  Level.Add((Thing) SmallFire.New(0.0f, 0.0f, 0.0f, 0.0f, stick: ((MaterialThing) dart), firedFrom: ((Thing) this)));
                  dart.burning = true;
                  dart.onFire = true;
                  this.Burn(this.position, (Thing) this);
                }
                Vec2 vec = Maths.AngleToVec(Maths.DegToRad(-angle));
                dart.hSpeed = vec.x * 10f;
                dart.vSpeed = vec.y * 10f;
                Level.Add((Thing) dart);
              }
            }
            else
            {
              Bullet bullet = this._ammoType.FireBullet(this.Offset(this.barrelOffset), this.owner, angle, (Thing) this);
              if (Network.isActive && this.isServerForObject)
              {
                this.firedBullets.Add(bullet);
                if (this.duck != null && this.duck.profile.connection != null)
                  bullet.connection = this.duck.profile.connection;
              }
              if (this.isServerForObject)
              {
                switch (this)
                {
                  case LaserRifle _:
                  case PewPewLaser _:
                  case Phaser _:
                    ++Global.data.laserBulletsFired.valueInt;
                    break;
                }
              }
            }
          }
          ++this.bulletFireIndex;
          this._ammoType.accuracy = accuracy;
          this._barrelHeat += 0.3f;
        }
        this._smokeWait = 3f;
        this.loaded = false;
        this._flareAlpha = 1.5f;
        if (!this._manualLoad)
          this.Reload();
        this.firing = true;
        this._wait = this._fireWait;
        this.PlayFireSound();
        if (this.owner == null)
        {
          Vec2 vec2 = this.barrelVector * Rando.Float(1f, 3f);
          vec2.y += Rando.Float(2f);
          this.hSpeed -= vec2.x;
          this.vSpeed -= vec2.y;
        }
        this._accuracyLost += this.loseAccuracy;
        if ((double) this._accuracyLost <= (double) this.maxAccuracyLost)
          return;
        this._accuracyLost = this.maxAccuracyLost;
      }
      else
      {
        if (this.ammo > 0 || (double) this._wait != 0.0)
          return;
        this.DoAmmoClick();
        this._wait = this._fireWait;
      }
    }

    protected virtual void PlayFireSound() => SFX.Play(this._fireSound, pitch: (Rando.Float(0.2f) - 0.1f + this._fireSoundPitch));

    public virtual void Reload(bool shell = true)
    {
      if (this.ammo != 0)
      {
        if (shell)
          this._ammoType.PopShell(this.x, this.y, (int) -this.offDir);
        --this.ammo;
      }
      this.loaded = true;
    }

    public override void Draw()
    {
      if (this.laserSight && this.owner != null)
      {
        ATTracer atTracer = new ATTracer();
        atTracer.range = 2000f;
        float ang = this.angleDegrees * -1f;
        if (this.offDir < (sbyte) 0)
          ang += 180f;
        Vec2 vec2 = this.Offset(this.laserOffset);
        atTracer.penetration = 0.4f;
        this._wallPoint = new Bullet(vec2.x, vec2.y, (AmmoType) atTracer, ang, this.owner, tracer: true).end;
        this._laserInit = true;
      }
      Graphics.material = (Material) null;
      if (this.owner != null)
        this.graphic.flipH = this.owner.graphic.flipH;
      else
        this.graphic.flipH = this.offDir <= (sbyte) 0;
      if (this._doPuff)
      {
        this._clickPuff.alpha = 0.6f;
        this._clickPuff.angle = this.angle + this._smokeAngle;
        this._clickPuff.flipH = this.offDir < (sbyte) 0;
        this.Draw((Sprite) this._clickPuff, this.barrelOffset);
      }
      if (!VirtualTransition.active)
        Graphics.material = this.material;
      base.Draw();
      Graphics.material = (Material) null;
      if ((double) this._flareAlpha > 0.0)
        this.Draw((Sprite) this._flare, this.barrelOffset);
      if ((double) this._barrelSmoke.speed > 0.0 && !this.raised)
      {
        this._barrelSmoke.alpha = 0.7f;
        this._barrelSmoke.angle = this._smokeAngle;
        this._barrelSmoke.flipH = this.offDir < (sbyte) 0;
        if (this.offDir > (sbyte) 0 && (double) this.angleDegrees > 90.0 && (double) this.angleDegrees < 270.0)
          this._barrelSmoke.flipH = true;
        if (this.offDir < (sbyte) 0 && (double) this.angleDegrees > 90.0 && (double) this.angleDegrees < 270.0)
          this._barrelSmoke.flipH = false;
        this._barrelSmoke.yscale = 1f - this._smokeFlatten;
        this.DrawIgnoreAngle((Sprite) this._barrelSmoke, this.barrelOffset);
      }
      if (VirtualTransition.active)
        return;
      Graphics.material = this.material;
    }

    public override void DrawGlow()
    {
      if (this.laserSight && this.owner != null && (this._laserTex != null && this._laserInit))
      {
        Vec2 p1 = this.Offset(this.laserOffset);
        float length = (p1 - this._wallPoint).length;
        float val1 = 100f;
        if (this.ammoType != null)
          val1 = this.ammoType.range;
        Vec2 normalized = (this._wallPoint - p1).normalized;
        Vec2 vec2 = p1 + normalized * Math.Min(val1, length);
        Graphics.DrawTexturedLine(this._laserTex, p1, vec2, Color.Red, 0.5f, this.depth - 1);
        if ((double) length > (double) val1)
        {
          for (int index = 1; index < 4; ++index)
          {
            Graphics.DrawTexturedLine(this._laserTex, vec2, vec2 + normalized * 2f, Color.Red * (float) (1.0 - (double) index * 0.200000002980232), 0.5f, this.depth - 1);
            vec2 += normalized * 2f;
          }
        }
        if (this._sightHit != null && (double) length < (double) val1)
        {
          this._sightHit.alpha = 1f;
          this._sightHit.color = Color.Red;
          Graphics.Draw(this._sightHit, this._wallPoint.x, this._wallPoint.y);
        }
      }
      base.DrawGlow();
    }
  }
}
