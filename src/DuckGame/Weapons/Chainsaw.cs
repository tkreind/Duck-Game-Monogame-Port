// Decompiled with JetBrains decompiler
// Type: DuckGame.Chainsaw
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isSuperWeapon", true)]
  [EditorGroup("guns|melee")]
  public class Chainsaw : Gun
  {
    public StateBinding _angleOffsetBinding = new StateBinding(nameof (_hold));
    public StateBinding _throwSpinBinding = new StateBinding(nameof (_throwSpin));
    public StateBinding _gasBinding = new StateBinding(nameof (_gas));
    public StateBinding _floodBinding = new StateBinding(nameof (_flood));
    public StateBinding _chainsawStateBinding = (StateBinding) new StateFlagBinding(new string[3]
    {
      nameof (_flooded),
      nameof (_started),
      nameof (_throttle)
    });
    public EditorProperty<bool> souped = new EditorProperty<bool>(false);
    private float _hold;
    private bool _shing;
    private static bool _playedShing;
    public float _throwSpin;
    private int _framesExisting;
    private int _hitWait;
    private SpriteMap _swordSwing;
    private SpriteMap _sprite;
    private float _rotSway;
    public bool _started;
    private int _pullState = -1;
    private float _animRot;
    private float _upWait;
    private float _engineSpin;
    private float _bladeSpin;
    private float _engineResistance = 1f;
    private SinWave _idleWave = (SinWave) 0.6f;
    private SinWave _spinWave = (SinWave) 1f;
    private bool _puffClick;
    private float _warmUp;
    public bool _flooded;
    private int _gasDripFrames;
    public float _flood;
    private bool _releasePull;
    public float _gas = 1f;
    private bool _struggling;
    private bool _throttle;
    private float _throttleWait;
    private bool _releasedSincePull;
    private int _skipDebris;
    private bool _resetDuck;
    private int _skipSpark;
    private ConstantSound _sound = new ConstantSound("chainsawIdle", multiSound: "chainsawIdleMulti");
    private ConstantSound _bladeSound = new ConstantSound("chainsawBladeLoop", multiSound: "chainsawBladeLoopMulti");
    private ConstantSound _bladeSoundLow = new ConstantSound("chainsawBladeLoopLow", multiSound: "chainsawBladeLoopLowMulti");
    private bool _smokeFlipper;
    private float _fireTrailWait;
    private bool _skipSmoke;

    public override float angle
    {
      get => (float) ((double) base.angle + (double) this._hold * (double) this.offDir + (double) this._animRot * (double) this.offDir + (double) this._rotSway * (double) this.offDir);
      set => this._angle = value;
    }

    public Vec2 barrelStartPos => this.position + (this.Offset(this.barrelOffset) - this.position).normalized * 2f;

    public bool throttle => this._throttle;

    public Chainsaw(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this._sprite = new SpriteMap("chainsaw", 29, 13);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 7f);
      this.collisionOffset = new Vec2(-8f, -6f);
      this.collisionSize = new Vec2(20f, 11f);
      this._barrelOffsetTL = new Vec2(27f, 8f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-4f, 4f);
      this.weight = 5f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._swordSwing = new SpriteMap("swordSwipe", 32, 32);
      this._swordSwing.AddAnimation("swing", 0.6f, false, 0, 1, 1, 2);
      this._swordSwing.currentAnimation = "swing";
      this._swordSwing.speed = 0.0f;
      this._swordSwing.center = new Vec2(9f, 25f);
      this.throwSpeedMultiplier = 0.5f;
      this._bouncy = 0.5f;
      this._impactThreshold = 0.3f;
      this.collideSounds.Add("landTV");
    }

    public override void Initialize()
    {
      this._sprite = new SpriteMap("chainsaw", 29, 13);
      if ((bool) this.souped)
        this._sprite = new SpriteMap("turbochainsaw", 29, 13);
      this.graphic = (Sprite) this._sprite;
      base.Initialize();
    }

    public override void Terminate()
    {
      this._sound.Kill();
      this._bladeSound.Kill();
      this._bladeSoundLow.Kill();
    }

    public void Shing(Thing wall)
    {
      if (this._shing)
        return;
      this._struggling = true;
      this._shing = true;
      if (!Chainsaw._playedShing)
      {
        Chainsaw._playedShing = true;
        SFX.Play("chainsawClash", Rando.Float(0.4f, 0.55f), Rando.Float(-0.2f, 0.2f), Rando.Float(-0.1f, 0.1f));
      }
      Vec2 normalized = (this.position - this.barrelPosition).normalized;
      Vec2 barrelPosition = this.barrelPosition;
      for (int index = 0; index < 6; ++index)
      {
        Level.Add((Thing) Spark.New(barrelPosition.x, barrelPosition.y, new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
        barrelPosition += normalized * 4f;
      }
      this._swordSwing.speed = 0.0f;
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogAction(7);
      if (this.duck == null)
        return;
      Duck duck = this.duck;
      if ((double) wall.bottom < (double) duck.top)
      {
        duck.vSpeed += 2f;
      }
      else
      {
        if (duck.sliding)
          duck.sliding = false;
        if ((double) wall.x > (double) duck.x)
          duck.hSpeed -= 5f;
        else
          duck.hSpeed += 5f;
        duck.vSpeed -= 2f;
      }
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      SFX.Play("ting");
      return base.Hit(bullet, hitPos);
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (this.owner != null || !(with is Block))
        return;
      this.Shing((Thing) with);
      if ((double) this.totalImpactPower <= 3.0)
        return;
      this._started = false;
    }

    public override void ReturnToWorld() => this._throwSpin = 90f;

    public void PullEngine()
    {
      float pitch = (bool) this.souped ? 0.3f : 0.0f;
      if (!this._flooded && (double) this._gas > 0.0 && ((double) this._warmUp > 0.5 || (double) this._engineResistance < 1.0))
      {
        SFX.Play("chainsawFire");
        this._started = true;
        this._engineSpin = 1.5f;
        for (int index = 0; index < 2; ++index)
          Level.Add((Thing) SmallSmoke.New(this.x + (float) ((int) this.offDir * 4), this.y + 5f));
        this._flooded = false;
        this._flood = 0.0f;
      }
      else
      {
        if (this._flooded && (double) this._gas > 0.0)
        {
          SFX.Play("chainsawFlooded", 0.9f, Rando.Float(-0.2f, 0.2f));
          this._engineSpin = 1.6f;
        }
        else
        {
          if ((double) this._gas == 0.0 || (double) Rando.Float(1f) > 0.300000011920929)
            SFX.Play("chainsawPull", pitch: pitch);
          else
            SFX.Play("chainsawFire", pitch: pitch);
          this._engineSpin = 0.8f;
        }
        if ((double) Rando.Float(1f) > 0.800000011920929)
        {
          this._flooded = false;
          this._flood = 0.0f;
        }
      }
      this._engineResistance -= 0.5f;
      if ((double) this._gas <= 0.0)
        return;
      int num = this._flooded ? 4 : 2;
      for (int index = 0; index < num; ++index)
        Level.Add((Thing) SmallSmoke.New(this.x + (float) ((int) this.offDir * 4), this.y + 5f));
    }

    public override void Update()
    {
      base.Update();
      float num1 = 1f;
      if ((bool) this.souped)
        num1 = 1.3f;
      if (this._swordSwing.finished)
        this._swordSwing.speed = 0.0f;
      if (this._hitWait > 0)
        --this._hitWait;
      if ((double) this._gas < 0.00999999977648258)
        this.ammo = 0;
      ++this._framesExisting;
      if (this._framesExisting > 100)
        this._framesExisting = 100;
      float pitch = (bool) this.souped ? 0.3f : 0.0f;
      this._sound.lerpVolume = !this._started || this._throttle ? 0.0f : 0.6f;
      this._sound.pitch = pitch;
      if (this._started)
      {
        this._warmUp += 1f / 1000f;
        if ((double) this._warmUp > 1.0)
          this._warmUp = 1f;
        if (!this._puffClick && (double) (float) this._idleWave > 0.899999976158142)
        {
          this._skipSmoke = !this._skipSmoke;
          if (this._throttle || !this._skipSmoke)
          {
            Level.Add((Thing) SmallSmoke.New(this.x + (float) ((int) this.offDir * 4), this.y + 5f, this._smokeFlipper ? -0.1f : 0.8f, 0.7f));
            this._smokeFlipper = !this._smokeFlipper;
            this._puffClick = true;
          }
        }
        else if (this._puffClick && (double) (float) this._idleWave < 0.0)
          this._puffClick = false;
        if (this._pullState < 0)
        {
          float num2 = (float) (1.0 + (double) Maths.NormalizeSection(this._engineSpin, 1f, 2f) * 2.0);
          float num3 = (float) this._idleWave;
          if ((double) num2 > 1.0)
            num3 = (float) this._spinWave;
          this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0.0f, (float) (2.0 + (double) num3 * (double) num2)), 0.23f);
          this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, (float) (2.0 + (double) num3 * (double) num2)), 0.23f);
          this._rotSway = (float) ((double) this._idleWave.normalized * (double) (Maths.NormalizeSection(this._engineSpin, 1f, 2f) * 3f) * 0.0299999993294477);
        }
        else
          this._rotSway = 0.0f;
        this._gas -= 3E-05f;
        if (this._throttle)
          this._gas -= 0.0002f;
        if ((double) this._gas < 0.0)
        {
          this._gas = 0.0f;
          this._started = false;
          this._throttle = false;
        }
        if (this._triggerHeld)
        {
          if (this._releasedSincePull)
          {
            if (!this._throttle)
            {
              this._throttle = true;
              SFX.Play("chainsawBladeRevUp", 0.5f, pitch);
            }
            this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 4f, 0.1f);
          }
        }
        else
        {
          if (this._throttle)
          {
            this._throttle = false;
            if ((double) this._engineSpin > 1.70000004768372)
              SFX.Play("chainsawBladeRevDown", 0.5f, pitch);
          }
          this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 0.0f, 0.1f);
          this._releasedSincePull = true;
        }
      }
      else
      {
        this._warmUp -= 1f / 1000f;
        if ((double) this._warmUp < 0.0)
          this._warmUp = 0.0f;
        this._releasedSincePull = false;
        this._throttle = false;
      }
      this._bladeSound.lerpSpeed = 0.1f;
      this._throttleWait = Lerp.Float(this._throttleWait, this._throttle ? 1f : 0.0f, 0.07f);
      this._bladeSound.lerpVolume = (double) this._throttleWait > 0.959999978542328 ? 0.6f : 0.0f;
      if (this._struggling)
        this._bladeSound.lerpVolume = 0.0f;
      this._bladeSoundLow.lerpVolume = (double) this._throttleWait <= 0.959999978542328 || !this._struggling ? 0.0f : 0.6f;
      this._bladeSound.pitch = pitch;
      this._bladeSoundLow.pitch = pitch;
      if (this.owner == null)
      {
        this.collisionOffset = new Vec2(-8f, -6f);
        this.collisionSize = new Vec2(13f, 11f);
      }
      else if (this.duck != null && (this.duck.sliding || this.duck.crouch))
      {
        this.collisionOffset = new Vec2(-8f, -6f);
        this.collisionSize = new Vec2(6f, 11f);
      }
      else
      {
        this.collisionOffset = new Vec2(-8f, -6f);
        this.collisionSize = new Vec2(10f, 11f);
      }
      if (this.owner != null)
      {
        this._resetDuck = false;
        if (this._pullState == -1)
        {
          if (!this._started)
          {
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0.0f, 2f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
          }
          this._upWait = 0.0f;
        }
        else if (this._pullState == 0)
        {
          this._animRot = Lerp.FloatSmooth(this._animRot, -0.4f, 0.15f);
          this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, -2f), 0.25f);
          this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-4f, 4f), 0.23f);
          if ((double) this._animRot <= -0.349999994039536)
          {
            this._animRot = -0.4f;
            this._pullState = 1;
            this.PullEngine();
          }
          this._upWait = 0.0f;
        }
        else if (this._pullState == 1)
        {
          this._releasePull = false;
          this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(2f, 3f), 0.23f);
          this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-4f, -2f), 0.23f);
          this._animRot = Lerp.FloatSmooth(this._animRot, -0.5f, 0.07f);
          if ((double) this._animRot < -0.449999988079071)
          {
            this._animRot = -0.5f;
            this._pullState = 2;
          }
          this._upWait = 0.0f;
        }
        else if (this._pullState == 2)
        {
          if (this._releasePull || !this._triggerHeld)
          {
            this._releasePull = true;
            if (this._started)
            {
              this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(0.0f, 2f + this._idleWave.normalized), 0.23f);
              this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f + this._idleWave.normalized), 0.23f);
              this._animRot = Lerp.FloatSmooth(this._animRot, 0.0f, 0.1f);
              if ((double) this._animRot > -0.0700000002980232)
              {
                this._animRot = 0.0f;
                this._pullState = -1;
              }
            }
            else
            {
              this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-4f, 4f), 0.24f);
              this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, -2f), 0.24f);
              this._animRot = Lerp.FloatSmooth(this._animRot, -0.4f, 0.12f);
              if ((double) this._animRot > -0.439999997615814)
              {
                this._releasePull = false;
                this._animRot = -0.4f;
                this._pullState = 3;
                this._holdOffset = new Vec2(-4f, 4f);
                this.handOffset = new Vec2(-2f, -2f);
              }
            }
          }
          this._upWait = 0.0f;
        }
        else if (this._pullState == 3)
        {
          this._releasePull = false;
          this._upWait += 0.1f;
          if ((double) this._upWait > 6.0)
            this._pullState = -1;
        }
        this._bladeSpin += this._engineSpin;
        while ((double) this._bladeSpin >= 1.0)
        {
          --this._bladeSpin;
          int num2 = this._sprite.frame + 1;
          if (num2 > 15)
            num2 = 0;
          this._sprite.frame = num2;
        }
        this._engineSpin = Lerp.FloatSmooth(this._engineSpin, 0.0f, 0.1f);
        this._engineResistance = Lerp.FloatSmooth(this._engineResistance, 1f, 0.01f);
        this._hold = -0.4f;
        this.center = new Vec2(8f, 7f);
        this._framesSinceThrown = (byte) 0;
      }
      else
      {
        this._rotSway = 0.0f;
        this._shing = false;
        this._animRot = Lerp.FloatSmooth(this._animRot, 0.0f, 0.18f);
        if (this._framesSinceThrown == (byte) 1)
          this._throwSpin = this.angleDegrees;
        this._hold = 0.0f;
        this.angleDegrees = this._throwSpin;
        this.center = new Vec2(8f, 7f);
        bool flag1 = false;
        bool flag2 = false;
        if (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 2.0 || !this.grounded) && (double) this.gravMultiplier > 0.0)
        {
          if (!this.grounded && Level.CheckRect<Block>(this.position + new Vec2(-8f, -6f), this.position + new Vec2(8f, -2f)) != null)
            flag2 = true;
          if (!flag2 && !this._grounded && Level.CheckPoint<IPlatform>(this.position + new Vec2(0.0f, 8f)) == null)
          {
            if (this.offDir > (sbyte) 0)
              this._throwSpin += (float) (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
            else
              this._throwSpin -= (float) (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed)) * 1.0 + 5.0);
            flag1 = true;
          }
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
      if ((double) Math.Abs(this._angle) > 1.0)
      {
        this._flood += 0.005f;
        if ((double) this._flood > 1.0)
        {
          this._flooded = true;
          this._started = false;
        }
        ++this._gasDripFrames;
        if ((double) this._gas > 0.0 && this._flooded && this._gasDripFrames > 2)
        {
          FluidData gas = Fluid.Gas;
          gas.amount = 3f / 1000f;
          this._gas -= 0.005f;
          if ((double) this._gas < 0.0)
            this._gas = 0.0f;
          Level.Add((Thing) new Fluid(this.x, this.y, Vec2.Zero, gas));
          this._gasDripFrames = 0;
        }
        if ((double) this._gas <= 0.0)
          this._started = false;
      }
      else
      {
        this._flood -= 0.008f;
        if ((double) this._flood < 0.0)
          this._flood = 0.0f;
      }
      if (this.duck != null)
      {
        this.duck.frictionMult = 1f;
        if (this._skipSpark > 0)
        {
          ++this._skipSpark;
          if (this._skipSpark > 2)
            this._skipSpark = 0;
        }
        if (this.duck.sliding && this._throttle && this._skipSpark == 0)
        {
          if (Level.CheckLine<Block>(this.barrelStartPos + new Vec2(0.0f, 8f), this.barrelPosition + new Vec2(0.0f, 8f)) != null)
          {
            this._skipSpark = 1;
            Vec2 vec2 = this.position + this.barrelVector * 5f;
            for (int index = 0; index < 2; ++index)
            {
              Level.Add((Thing) Spark.New(vec2.x, vec2.y, new Vec2((float) this.offDir * Rando.Float(0.0f, 2f), Rando.Float(0.5f, 1.5f))));
              vec2 += this.barrelVector * 2f;
              this._fireTrailWait -= 0.5f;
              if ((bool) this.souped && (double) this._fireTrailWait <= 0.0)
              {
                this._fireTrailWait = 1f;
                SmallFire smallFire = SmallFire.New(vec2.x, vec2.y, (float) this.offDir * Rando.Float(0.0f, 2f), Rando.Float(0.5f, 1.5f));
                smallFire.waitToHurt = Rando.Float(1f, 2f);
                smallFire.whoWait = this.owner as Duck;
                Level.Add((Thing) smallFire);
              }
            }
            if (this.offDir > (sbyte) 0 && (double) this.owner.hSpeed < (double) ((int) this.offDir * 6) * (double) num1)
              this.owner.hSpeed = (float) ((int) this.offDir * 6) * num1;
            else if (this.offDir < (sbyte) 0 && (double) this.owner.hSpeed > (double) ((int) this.offDir * 6) * (double) num1)
              this.owner.hSpeed = (float) ((int) this.offDir * 6) * num1;
          }
          else if (this.offDir > (sbyte) 0 && (double) this.owner.hSpeed < (double) ((int) this.offDir * 3) * (double) num1)
            this.owner.hSpeed = (float) ((int) this.offDir * 3) * num1;
          else if (this.offDir < (sbyte) 0 && (double) this.owner.hSpeed > (double) ((int) this.offDir * 3) * (double) num1)
            this.owner.hSpeed = (float) ((int) this.offDir * 3) * num1;
        }
        if (this._pullState == -1)
        {
          if (!this._throttle)
          {
            this._animRot = MathHelper.Lerp(this._animRot, 0.3f, 0.2f);
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(-2f, 2f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(-3f, 4f), 0.23f);
          }
          else if (this._shing)
          {
            this._animRot = MathHelper.Lerp(this._animRot, -1.8f, 0.4f);
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0.0f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
            if ((double) this._animRot < -1.5)
              this._shing = false;
          }
          else if (this.duck.crouch)
          {
            this._animRot = MathHelper.Lerp(this._animRot, 0.4f, 0.2f);
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0.0f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
          }
          else if (this.duck.inputProfile.Down("UP"))
          {
            this._animRot = MathHelper.Lerp(this._animRot, -0.9f, 0.2f);
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0.0f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
          }
          else
          {
            this._animRot = MathHelper.Lerp(this._animRot, 0.0f, 0.2f);
            this.handOffset = Lerp.Vec2Smooth(this.handOffset, new Vec2(1f, 0.0f), 0.25f);
            this._holdOffset = Lerp.Vec2Smooth(this._holdOffset, new Vec2(1f, 2f), 0.23f);
          }
        }
      }
      else if (!this._resetDuck && this.prevOwner != null)
      {
        if (this.prevOwner is PhysicsObject prevOwner)
          prevOwner.frictionMult = 1f;
        this._resetDuck = true;
      }
      if (this._skipDebris > 0)
        ++this._skipDebris;
      if (this._skipDebris > 3)
        this._skipDebris = 0;
      this._struggling = false;
      if (this.owner == null || !this._started || (!this._throttle || this._shing))
        return;
      (this.Offset(this.barrelOffset) - this.position).Normalize();
      this.Offset(this.barrelOffset);
      IEnumerable<IAmADuck> amAducks = Level.CheckLineAll<IAmADuck>(this.barrelStartPos, this.barrelPosition);
      Block block = Level.CheckLine<Block>(this.barrelStartPos, this.barrelPosition);
      if (this.owner != null)
      {
        foreach (MaterialThing materialThing in Level.CheckLineAll<MaterialThing>(this.barrelStartPos, this.barrelPosition))
        {
          if (materialThing.Hurt(materialThing is Door ? 1.8f : 0.5f))
          {
            if (this.duck != null && this.duck.sliding && (materialThing is Door && (materialThing as Door)._jammed))
            {
              materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
            }
            else
            {
              this._struggling = true;
              if (this.duck != null)
                this.duck.frictionMult = 4f;
              if (this._skipDebris == 0)
              {
                this._skipDebris = 1;
                Vec2 vec2_1 = Collision.LinePoint(this.barrelStartPos, this.barrelPosition, materialThing.rectangle);
                if (vec2_1 != Vec2.Zero)
                {
                  Vec2 vec2_2 = vec2_1 + this.barrelVector * Rando.Float(0.0f, 3f);
                  Vec2 vec2_3 = -this.barrelVector.Rotate(Rando.Float(-0.2f, 0.2f), Vec2.Zero);
                  if (materialThing.physicsMaterial == PhysicsMaterial.Wood)
                  {
                    Thing thing = (Thing) WoodDebris.New(vec2_2.x, vec2_2.y);
                    thing.hSpeed = vec2_3.x * 3f;
                    thing.vSpeed = vec2_3.y * 3f;
                    Level.Add(thing);
                  }
                  else if (materialThing.physicsMaterial == PhysicsMaterial.Metal)
                  {
                    Thing thing = (Thing) Spark.New(vec2_2.x, vec2_2.y, Vec2.Zero);
                    thing.hSpeed = vec2_3.x * 3f;
                    thing.vSpeed = vec2_3.y * 3f;
                    Level.Add(thing);
                  }
                }
              }
            }
          }
        }
      }
      bool flag = false;
      switch (block)
      {
        case null:
        case Door _:
          foreach (Sword sword in Level.current.things[typeof (Sword)])
          {
            if (sword.owner != null && sword.crouchStance && (!sword.jabStance && Collision.LineIntersect(this.barrelStartPos, this.barrelPosition, sword.barrelStartPos, sword.barrelPosition)))
            {
              this.Shing((Thing) sword);
              sword.Shing();
              sword.owner.hSpeed += (float) this.offDir * 3f;
              sword.owner.vSpeed -= 2f;
              this.duck.hSpeed += (float) -this.offDir * 3f;
              this.duck.vSpeed -= 2f;
              sword.duck.crippleTimer = 1f;
              this.duck.crippleTimer = 1f;
              flag = true;
            }
          }
          if (!flag)
          {
            Thing ignore = (Thing) null;
            if (this.duck != null)
              ignore = (Thing) this.duck.GetEquipment(typeof (Helmet));
            QuadLaserBullet quadLaserBullet = Level.CheckLine<QuadLaserBullet>(this.position, this.barrelPosition);
            if (quadLaserBullet != null)
            {
              this.Shing((Thing) quadLaserBullet);
              Vec2 vec2 = quadLaserBullet.travel;
              float length = vec2.length;
              float num2 = 1f;
              if (this.offDir > (sbyte) 0 && (double) vec2.x < 0.0)
                num2 = 1.5f;
              else if (this.offDir < (sbyte) 0 && (double) vec2.x > 0.0)
                num2 = 1.5f;
              vec2 = this.offDir <= (sbyte) 0 ? new Vec2(-length * num2, 0.0f) : new Vec2(length * num2, 0.0f);
              quadLaserBullet.travel = vec2;
              break;
            }
            Helmet helmet = Level.CheckLine<Helmet>(this.barrelStartPos, this.barrelPosition, ignore);
            if (helmet != null && helmet.equippedDuck != null)
            {
              this.Shing((Thing) helmet);
              helmet.owner.hSpeed += (float) this.offDir * 3f;
              helmet.owner.vSpeed -= 2f;
              helmet.duck.crippleTimer = 1f;
              helmet.Hurt(0.53f);
              flag = true;
              break;
            }
            if (this.duck != null)
              ignore = (Thing) this.duck.GetEquipment(typeof (ChestPlate));
            ChestPlate chestPlate = Level.CheckLine<ChestPlate>(this.barrelStartPos, this.barrelPosition, ignore);
            if (chestPlate != null && chestPlate.equippedDuck != null)
            {
              this.Shing((Thing) chestPlate);
              chestPlate.owner.hSpeed += (float) this.offDir * 3f;
              chestPlate.owner.vSpeed -= 2f;
              chestPlate.duck.crippleTimer = 1f;
              chestPlate.Hurt(0.53f);
              flag = true;
              break;
            }
            break;
          }
          break;
        default:
          this.Shing((Thing) block);
          if (block is Window)
          {
            block.Destroy((DestroyType) new DTImpact((Thing) this));
            break;
          }
          break;
      }
      if (!flag)
      {
        foreach (Chainsaw chainsaw in Level.current.things[typeof (Chainsaw)])
        {
          if (chainsaw != this && chainsaw.owner != null && Collision.LineIntersect(this.barrelStartPos, this.barrelPosition, chainsaw.barrelStartPos, chainsaw.barrelPosition))
          {
            this.Shing((Thing) chainsaw);
            chainsaw.Shing((Thing) this);
            chainsaw.owner.hSpeed += (float) this.offDir * 2f;
            chainsaw.owner.vSpeed -= 1.5f;
            this.duck.hSpeed += (float) -this.offDir * 2f;
            this.duck.vSpeed -= 1.5f;
            chainsaw.duck.crippleTimer = 1f;
            this.duck.crippleTimer = 1f;
            flag = true;
            if (Recorder.currentRecording != null)
              Recorder.currentRecording.LogBonus();
          }
        }
      }
      if (flag)
        return;
      foreach (IAmADuck amAduck in amAducks)
      {
        if (amAduck != this.duck && amAduck is MaterialThing materialThing)
        {
          materialThing.velocity = materialThing.velocity + new Vec2((float) this.offDir * 0.8f, -0.8f);
          materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
          if (this.duck != null)
            this.duck._timeSinceChainKill = 0;
        }
      }
    }

    public override void Draw()
    {
      Chainsaw._playedShing = false;
      if ((double) this._swordSwing.speed > 0.0)
      {
        if (this.duck != null)
          this._swordSwing.flipH = this.duck.offDir <= (sbyte) 0;
        this._swordSwing.alpha = 0.4f;
        this._swordSwing.position = this.position;
        this._swordSwing.depth = this.depth + 1;
        this._swordSwing.Draw();
      }
      if (this.duck != null && (this._pullState == 1 || this._pullState == 2))
        Graphics.DrawLine(this.Offset(new Vec2(-2f, -2f)), this.duck.armPosition + new Vec2(this.handOffset.x * (float) this.offDir, this.handOffset.y), Color.White, depth: (this.duck.depth + 10 - 1));
      base.Draw();
    }

    public override void OnPressAction()
    {
      if (this._started)
        return;
      if (this._pullState == -1)
      {
        this._pullState = 0;
      }
      else
      {
        if (this._pullState != 3)
          return;
        this._pullState = 1;
        this.PullEngine();
      }
    }

    public override void Fire()
    {
    }
  }
}
