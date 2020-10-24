// Decompiled with JetBrains decompiler
// Type: DuckGame.Sword
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("guns|melee")]
  public class Sword : Gun
  {
    public StateBinding _swingBinding = new StateBinding(true, nameof (_swing));
    public StateBinding _holdBinding = new StateBinding(true, nameof (_hold));
    public StateBinding _jabStanceBinding = new StateBinding(nameof (_jabStance));
    public StateBinding _crouchStanceBinding = new StateBinding(nameof (_crouchStance));
    public StateBinding _slamStanceBinding = new StateBinding(nameof (_slamStance));
    public StateBinding _pullBackBinding = new StateBinding(true, nameof (_pullBack));
    public StateBinding _swingingBinding = new StateBinding(nameof (_swinging));
    public StateBinding _throwSpinBinding = new StateBinding(true, nameof (_throwSpin));
    public StateBinding _volatileBinding = new StateBinding(nameof (_volatile));
    public StateBinding _addOffsetXBinding = new StateBinding(nameof (_addOffsetX));
    public StateBinding _addOffsetYBinding = new StateBinding(nameof (_addOffsetY));
    public float _swing;
    public float _hold;
    private bool _drawing;
    public bool _pullBack;
    public bool _jabStance;
    public bool _crouchStance;
    public bool _slamStance;
    public bool _swinging;
    public float _addOffsetX;
    public float _addOffsetY;
    public bool _swingPress;
    public bool _shing;
    public static bool _playedShing;
    public bool _atRest = true;
    public bool _swung;
    public bool _wasLifted;
    public float _throwSpin;
    public int _framesExisting;
    public int _hitWait;
    private SpriteMap _swordSwing;
    private int _unslam;
    private byte blocked;
    public bool _volatile;
    private List<float> _lastAngles = new List<float>();
    private List<Vec2> _lastPositions = new List<Vec2>();

    public override float angle
    {
      get => this._drawing ? this._angle : base.angle + (this._swing + this._hold) * (float) this.offDir;
      set => this._angle = value;
    }

    public bool jabStance => this._jabStance;

    public bool crouchStance => this._crouchStance;

    public Vec2 barrelStartPos
    {
      get
      {
        if (this.owner == null)
          return this.position - (this.Offset(this.barrelOffset) - this.position).normalized * 6f;
        return this._slamStance ? this.position + (this.Offset(this.barrelOffset) - this.position).normalized * 12f : this.position + (this.Offset(this.barrelOffset) - this.position).normalized * 2f;
      }
    }

    public Sword(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this.graphic = new Sprite("sword");
      this.center = new Vec2(4f, 21f);
      this.collisionOffset = new Vec2(-2f, -16f);
      this.collisionSize = new Vec2(4f, 18f);
      this._barrelOffsetTL = new Vec2(4f, 1f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-4f, 4f);
      this.weight = 0.9f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._swordSwing = new SpriteMap("swordSwipe", 32, 32);
      this._swordSwing.AddAnimation("swing", 0.6f, false, 0, 1, 1, 2);
      this._swordSwing.currentAnimation = "swing";
      this._swordSwing.speed = 0.0f;
      this._swordSwing.center = new Vec2(9f, 25f);
      this._bouncy = 0.5f;
      this._impactThreshold = 0.3f;
    }

    public override void Initialize() => base.Initialize();

    public override void CheckIfHoldObstructed()
    {
      if (!(this.owner is Duck owner))
        return;
      owner.holdObstructed = false;
    }

    public override void Thrown()
    {
    }

    public void Shing()
    {
      if (this._shing)
        return;
      this._pullBack = false;
      this._swinging = false;
      this._shing = true;
      this._swingPress = false;
      if (!Sword._playedShing)
      {
        Sword._playedShing = true;
        SFX.Play("swordClash", Rando.Float(0.6f, 0.7f), Rando.Float(-0.1f, 0.1f), Rando.Float(-0.1f, 0.1f));
      }
      Vec2 normalized = (this.position - this.barrelPosition).normalized;
      Vec2 barrelPosition = this.barrelPosition;
      for (int index = 0; index < 6; ++index)
      {
        Level.Add((Thing) Spark.New(barrelPosition.x, barrelPosition.y, new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
        barrelPosition += normalized * 4f;
      }
      this._swung = false;
      this._swordSwing.speed = 0.0f;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (this.duck == null)
        return false;
      if (this.blocked == (byte) 0)
      {
        this.duck.AddCoolness(1);
      }
      else
      {
        ++this.blocked;
        if (this.blocked > (byte) 4)
        {
          this.blocked = (byte) 1;
          this.duck.AddCoolness(1);
        }
      }
      SFX.Play("ting");
      return base.Hit(bullet, hitPos);
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (!this._wasLifted || this.owner != null || !(with is Block))
        return;
      this.Shing();
      this._framesSinceThrown = (byte) 15;
    }

    public override void ReturnToWorld()
    {
      this._throwSpin = 90f;
      this.collisionOffset = new Vec2(-2f, -16f);
      this.collisionSize = new Vec2(4f, 18f);
      if (!this._wasLifted)
        return;
      this.collisionOffset = new Vec2(-4f, -2f);
      this.collisionSize = new Vec2(8f, 4f);
    }

    public override void Update()
    {
      base.Update();
      if (this._swordSwing.finished)
        this._swordSwing.speed = 0.0f;
      if (this._hitWait > 0)
        --this._hitWait;
      ++this._framesExisting;
      if (this._framesExisting > 100)
        this._framesExisting = 100;
      if ((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 4.0 && this._framesExisting > 10)
        this._wasLifted = true;
      if (this.owner != null)
      {
        this._hold = -0.4f;
        this._wasLifted = true;
        this.center = new Vec2(4f, 21f);
        this._framesSinceThrown = (byte) 0;
      }
      else
      {
        if (this._framesSinceThrown == (byte) 1)
        {
          this._throwSpin = Maths.RadToDeg(this.angle) - 90f;
          this._hold = 0.0f;
          this._swing = 0.0f;
        }
        if (this._wasLifted)
        {
          this.angleDegrees = 90f + this._throwSpin;
          this.center = new Vec2(4f, 11f);
        }
        this._volatile = false;
        bool flag1 = false;
        bool flag2 = false;
        if ((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 2.0 || !this.grounded)
        {
          if (!this.grounded && Level.CheckRect<Block>(this.position + new Vec2(-6f, -6f), this.position + new Vec2(6f, -2f)) != null)
          {
            flag2 = true;
            if ((double) this.vSpeed > 4.0)
              this._volatile = true;
          }
          if (!flag2 && !this._grounded && Level.CheckPoint<IPlatform>(this.position + new Vec2(0.0f, 8f)) == null)
          {
            if ((double) this.hSpeed > 0.0)
              this._throwSpin += (float) (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed)) * 2.0 + 4.0);
            else
              this._throwSpin -= (float) (((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed)) * 2.0 + 4.0);
            flag1 = true;
          }
        }
        if (this._framesExisting > 15 && (double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 3.0)
          this._volatile = true;
        if (!flag1 || flag2)
        {
          this._throwSpin %= 360f;
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
        if (this._volatile && this._hitWait == 0)
        {
          (this.Offset(this.barrelOffset) - this.position).Normalize();
          this.Offset(this.barrelOffset);
          bool flag3 = false;
          foreach (Sword sword in Level.current.things[typeof (Sword)])
          {
            if (sword != this && sword.owner != null && (sword._crouchStance && !sword._jabStance) && !sword._jabStance && (((double) this.hSpeed > 0.0 && (double) sword.x > (double) this.x - 4.0 || (double) this.hSpeed < 0.0 && (double) sword.x < (double) this.x + 4.0) && Collision.LineIntersect(this.barrelStartPos, this.barrelPosition, sword.barrelStartPos, sword.barrelPosition)))
            {
              this.Shing();
              sword.Shing();
              sword.owner.hSpeed += (float) this.offDir * 1f;
              --sword.owner.vSpeed;
              flag3 = true;
              this._hitWait = 4;
              this.hSpeed = (float) (-(double) this.hSpeed * 0.600000023841858);
            }
          }
          int num = 12;
          if (!flag3)
          {
            foreach (Chainsaw chainsaw in Level.current.things[typeof (Chainsaw)])
            {
              if (chainsaw.owner != null && chainsaw.throttle && Collision.LineIntersect(this.barrelStartPos, this.barrelPosition, chainsaw.barrelStartPos, chainsaw.barrelPosition))
              {
                this.Shing();
                chainsaw.Shing((Thing) this);
                chainsaw.owner.hSpeed += (float) this.offDir * 1f;
                --chainsaw.owner.vSpeed;
                flag3 = true;
                this.hSpeed = (float) (-(double) this.hSpeed * 0.600000023841858);
                this._hitWait = 4;
                if (Recorder.currentRecording != null)
                  Recorder.currentRecording.LogBonus();
              }
            }
            if (!flag3)
            {
              Helmet helmet = Level.CheckLine<Helmet>(this.barrelStartPos, this.barrelPosition);
              if (helmet != null && helmet.equippedDuck != null && (helmet.owner != this.prevOwner || (int) this._framesSinceThrown > num))
              {
                this.hSpeed = (float) (-(double) this.hSpeed * 0.600000023841858);
                this.Shing();
                flag3 = true;
                this._hitWait = 4;
              }
              else
              {
                ChestPlate chestPlate = Level.CheckLine<ChestPlate>(this.barrelStartPos, this.barrelPosition);
                if (chestPlate != null && chestPlate.equippedDuck != null && (chestPlate.owner != this.prevOwner || (int) this._framesSinceThrown > num))
                {
                  this.hSpeed = (float) (-(double) this.hSpeed * 0.600000023841858);
                  this.Shing();
                  flag3 = true;
                  this._hitWait = 4;
                }
              }
            }
          }
          if (!flag3 && this.isServerForObject)
          {
            foreach (IAmADuck amAduck in Level.CheckLineAll<IAmADuck>(this.barrelStartPos, this.barrelPosition))
            {
              if (amAduck != this.duck && amAduck is MaterialThing materialThing && (materialThing != this.prevOwner || (int) this._framesSinceThrown > num))
              {
                materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
                if (Recorder.currentRecording != null)
                  Recorder.currentRecording.LogBonus();
              }
            }
          }
        }
      }
      if (this.owner == null)
      {
        this._swinging = false;
        this._jabStance = false;
        this._crouchStance = false;
        this._pullBack = false;
        this._swung = false;
        this._shing = false;
        this._swing = 0.0f;
        this._swingPress = false;
        this._slamStance = false;
        this._unslam = 0;
      }
      if (this.isServerForObject)
      {
        if (this._unslam > 1)
        {
          --this._unslam;
          this._slamStance = true;
        }
        else if (this._unslam == 1)
        {
          this._unslam = 0;
          this._slamStance = false;
        }
        if (this._pullBack)
        {
          if (this.duck != null)
          {
            if (this._jabStance)
            {
              this._pullBack = false;
              this._swinging = true;
            }
            else
            {
              this._swinging = true;
              this._pullBack = false;
            }
          }
        }
        else if (this._swinging)
        {
          if (this._jabStance)
          {
            this._addOffsetX = MathHelper.Lerp(this._addOffsetX, 3f, 0.4f);
            if ((double) this._addOffsetX > 2.0 && !this.action)
              this._swinging = false;
          }
          else if (this.raised)
          {
            this._swing = MathHelper.Lerp(this._swing, -2.8f, 0.2f);
            if ((double) this._swing < -2.40000009536743 && !this.action)
            {
              this._swinging = false;
              this._swing = 1.8f;
            }
          }
          else
          {
            this._swing = MathHelper.Lerp(this._swing, 2.1f, 0.4f);
            if ((double) this._swing > 1.79999995231628 && !this.action)
            {
              this._swinging = false;
              this._swing = 1.8f;
            }
          }
        }
        else
        {
          if (!this._swinging && (!this._swingPress || this._shing || this._jabStance && (double) this._addOffsetX < 1.0 || !this._jabStance && (double) this._swing < 1.60000002384186))
          {
            if (this._jabStance)
            {
              this._swing = MathHelper.Lerp(this._swing, 1.75f, 0.4f);
              if ((double) this._swing > 1.54999995231628)
              {
                this._swing = 1.55f;
                this._shing = false;
                this._swung = false;
              }
              this._addOffsetX = MathHelper.Lerp(this._addOffsetX, -12f, 0.45f);
              if ((double) this._addOffsetX < -12.0)
                this._addOffsetX = -12f;
              this._addOffsetY = MathHelper.Lerp(this._addOffsetY, -4f, 0.35f);
              if ((double) this._addOffsetX < -3.0)
                this._addOffsetY = -3f;
            }
            else if (this._slamStance)
            {
              this._swing = MathHelper.Lerp(this._swing, 3.14f, 0.8f);
              if ((double) this._swing > 3.09999990463257 && this._unslam == 0)
              {
                this._swing = 3.14f;
                this._shing = false;
                this._swung = true;
              }
              this._addOffsetX = MathHelper.Lerp(this._addOffsetX, -5f, 0.45f);
              if ((double) this._addOffsetX < -4.59999990463257)
                this._addOffsetX = -5f;
              this._addOffsetY = MathHelper.Lerp(this._addOffsetY, -6f, 0.35f);
              if ((double) this._addOffsetX < -5.5)
                this._addOffsetY = -6f;
            }
            else
            {
              this._swing = MathHelper.Lerp(this._swing, -0.22f, 0.36f);
              this._addOffsetX = MathHelper.Lerp(this._addOffsetX, 1f, 0.2f);
              if ((double) this._addOffsetX > 0.0)
                this._addOffsetX = 0.0f;
              this._addOffsetY = MathHelper.Lerp(this._addOffsetY, 1f, 0.2f);
              if ((double) this._addOffsetY > 0.0)
                this._addOffsetY = 0.0f;
            }
          }
          if (((double) this._swing < 0.0 || this._jabStance) && (double) this._swing < 0.0)
          {
            this._swing = 0.0f;
            this._shing = false;
            this._swung = false;
          }
        }
      }
      if (this.duck != null)
      {
        this.collisionOffset = new Vec2(-4f, 0.0f);
        this.collisionSize = new Vec2(4f, 4f);
        if (this._crouchStance && !this._jabStance)
        {
          this.collisionOffset = new Vec2(-2f, -19f);
          this.collisionSize = new Vec2(4f, 16f);
          this.thickness = 3f;
        }
        this._swingPress = false;
        if (!this._pullBack && !this._swinging)
        {
          this._crouchStance = false;
          this._jabStance = false;
          if (this.duck.crouch)
          {
            if (!this._pullBack && !this._swinging && this.duck.inputProfile.Down(this.offDir > (sbyte) 0 ? "LEFT" : "RIGHT"))
              this._jabStance = true;
            this._crouchStance = true;
          }
          if (!this._crouchStance || this._jabStance)
            this._slamStance = false;
        }
        if (!this._crouchStance)
        {
          this._hold = -0.4f;
          this.handOffset = new Vec2(this._addOffsetX, this._addOffsetY);
          this._holdOffset = new Vec2(this._addOffsetX - 4f, 4f + this._addOffsetY);
        }
        else
        {
          this._hold = 0.0f;
          this._holdOffset = new Vec2(0.0f + this._addOffsetX, 4f + this._addOffsetY);
          this.handOffset = new Vec2(3f + this._addOffsetX, this._addOffsetY);
        }
      }
      else
      {
        this.collisionOffset = new Vec2(-2f, -16f);
        this.collisionSize = new Vec2(4f, 18f);
        if (this._wasLifted)
        {
          this.collisionOffset = new Vec2(-4f, -2f);
          this.collisionSize = new Vec2(8f, 4f);
        }
        this.thickness = 0.0f;
      }
      if ((this._swung || this._swinging) && !this._shing)
      {
        (this.Offset(this.barrelOffset) - this.position).Normalize();
        this.Offset(this.barrelOffset);
        IEnumerable<IAmADuck> amAducks = Level.CheckLineAll<IAmADuck>(this.barrelStartPos, this.barrelPosition);
        Block block = Level.CheckLine<Block>(this.barrelStartPos, this.barrelPosition);
        if (block != null && !this._slamStance)
        {
          if (this.offDir < (sbyte) 0 && (double) block.x > (double) this.x)
            block = (Block) null;
          else if (this.offDir > (sbyte) 0 && (double) block.x < (double) this.x)
            block = (Block) null;
        }
        bool flag = false;
        if (block != null)
        {
          this.Shing();
          if (this._slamStance)
          {
            this._swung = false;
            this._unslam = 20;
            this.owner.vSpeed = -5f;
          }
          if (block is Window)
            block.Destroy((DestroyType) new DTImpact((Thing) this));
        }
        else if (!this._jabStance && !this._slamStance)
        {
          Thing ignore = (Thing) null;
          if (this.duck != null)
            ignore = (Thing) this.duck.GetEquipment(typeof (Helmet));
          Vec2 vec2_1 = this.barrelPosition + this.barrelVector * 3f;
          QuadLaserBullet quadLaserBullet = Level.CheckRect<QuadLaserBullet>(new Vec2((double) this.position.x < (double) vec2_1.x ? this.position.x : vec2_1.x, (double) this.position.y < (double) vec2_1.y ? this.position.y : vec2_1.y), new Vec2((double) this.position.x > (double) vec2_1.x ? this.position.x : vec2_1.x, (double) this.position.y > (double) vec2_1.y ? this.position.y : vec2_1.y));
          if (quadLaserBullet != null)
          {
            this.Shing();
            this.Fondle((Thing) quadLaserBullet);
            quadLaserBullet.safeFrames = 8;
            quadLaserBullet.safeDuck = this.duck;
            Vec2 vec2_2 = quadLaserBullet.travel;
            float length = vec2_2.length;
            float num = 1f;
            if (this.offDir > (sbyte) 0 && (double) vec2_2.x < 0.0)
              num = 1.5f;
            else if (this.offDir < (sbyte) 0 && (double) vec2_2.x > 0.0)
              num = 1.5f;
            vec2_2 = this.offDir <= (sbyte) 0 ? new Vec2(-length * num, 0.0f) : new Vec2(length * num, 0.0f);
            quadLaserBullet.travel = vec2_2;
          }
          else
          {
            Helmet helmet = Level.CheckLine<Helmet>(this.barrelStartPos, this.barrelPosition, ignore);
            if (helmet != null && helmet.equippedDuck != null)
            {
              this.Shing();
              helmet.owner.hSpeed += (float) this.offDir * 3f;
              helmet.owner.vSpeed -= 2f;
              helmet.duck.crippleTimer = 1f;
              helmet.Hurt(0.53f);
              flag = true;
            }
            else
            {
              if (this.duck != null)
                ignore = (Thing) this.duck.GetEquipment(typeof (ChestPlate));
              ChestPlate chestPlate = Level.CheckLine<ChestPlate>(this.barrelStartPos, this.barrelPosition, ignore);
              if (chestPlate != null && chestPlate.equippedDuck != null)
              {
                this.Shing();
                chestPlate.owner.hSpeed += (float) this.offDir * 3f;
                chestPlate.owner.vSpeed -= 2f;
                chestPlate.duck.crippleTimer = 1f;
                chestPlate.Hurt(0.53f);
                flag = true;
              }
            }
          }
        }
        if (!flag)
        {
          foreach (Sword sword in Level.current.things[typeof (Sword)])
          {
            if (sword != this && sword.duck != null && (!this._jabStance && !sword._jabStance) && (this.duck != null && Collision.LineIntersect(this.barrelStartPos, this.barrelPosition, sword.barrelStartPos, sword.barrelPosition)))
            {
              this.Shing();
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
        }
        if (flag)
          return;
        foreach (IAmADuck amAduck in amAducks)
        {
          if (amAduck != this.duck && amAduck is MaterialThing materialThing)
            materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
        }
      }
      else
      {
        if (!this._crouchStance || this.duck == null)
          return;
        foreach (IAmADuck amAduck in Level.CheckLineAll<IAmADuck>(this.barrelStartPos, this.barrelPosition))
        {
          if (amAduck != this.duck && amAduck is MaterialThing materialThing)
          {
            if ((double) materialThing.vSpeed > 0.5 && (double) materialThing.bottom < (double) this.position.y - 8.0 && ((double) materialThing.left < (double) this.barrelPosition.x && (double) materialThing.right > (double) this.barrelPosition.x))
              materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
            else if (!this._jabStance && !materialThing.destroyed && (this.offDir > (sbyte) 0 && (double) materialThing.x > (double) this.duck.x || this.offDir < (sbyte) 0 && (double) materialThing.x < (double) this.duck.x))
            {
              if (materialThing is Duck)
                (materialThing as Duck).crippleTimer = 1f;
              else if ((double) this.duck.x > (double) materialThing.x && (double) materialThing.hSpeed > 1.5 || (double) this.duck.x < (double) materialThing.x && (double) materialThing.hSpeed < -1.5)
                materialThing.Destroy((DestroyType) new DTImpale((Thing) this));
              this.Fondle((Thing) materialThing);
              materialThing.hSpeed = (float) this.offDir * 3f;
              materialThing.vSpeed = -2f;
            }
          }
        }
      }
    }

    public override void Draw()
    {
      Sword._playedShing = false;
      if ((double) this._swordSwing.speed > 0.0)
      {
        if (this.duck != null)
          this._swordSwing.flipH = this.duck.offDir <= (sbyte) 0;
        this._swordSwing.alpha = 0.4f;
        this._swordSwing.position = this.position;
        this._swordSwing.depth = this.depth + 1;
        this._swordSwing.Draw();
      }
      this.alpha = 1f;
      Vec2 position = this.position;
      Depth depth = this.depth;
      this.graphic.color = Color.White;
      if (this.owner == null && (double) this.velocity.length > 1.0 || (double) this._swing != 0.0)
      {
        float angle1 = this.angle;
        this._drawing = true;
        float angle2 = this._angle;
        this.angle = angle1;
        for (int index = 0; index < 7; ++index)
        {
          base.Draw();
          if (this._lastAngles.Count > index)
            this._angle = this._lastAngles[index];
          if (this._lastPositions.Count > index)
          {
            this.position = this._lastPositions[index];
            if (this.owner != null)
            {
              Sword sword = this;
              sword.position = sword.position + this.owner.velocity;
            }
            Sword sword1 = this;
            sword1.depth = sword1.depth - 2;
            this.alpha -= 0.15f;
            this.graphic.color = Color.Red;
          }
          else
            break;
        }
        this.position = position;
        this.depth = depth;
        this.alpha = 1f;
        this._angle = angle2;
        this.xscale = 1f;
        this._drawing = false;
      }
      else
        base.Draw();
      this._lastAngles.Insert(0, this.angle);
      this._lastPositions.Insert(0, this.position);
      if (this._lastAngles.Count > 2)
      {
        this._lastAngles.Insert(0, (float) (((double) this._lastAngles[0] + (double) this._lastAngles[2]) / 2.0));
        this._lastPositions.Insert(0, (this._lastPositions[0] + this._lastPositions[2]) / 2f);
      }
      if (this._lastAngles.Count > 8)
        this._lastAngles.RemoveAt(this._lastAngles.Count - 1);
      if (this._lastPositions.Count <= 8)
        return;
      this._lastPositions.RemoveAt(this._lastPositions.Count - 1);
    }

    public override void OnPressAction()
    {
      if (this._crouchStance && this._jabStance && !this._swinging || !this._crouchStance && !this._swinging && (double) this._swing < 0.100000001490116)
      {
        this._pullBack = true;
        this._swung = true;
        this._shing = false;
        SFX.Play("swipe", Rando.Float(0.8f, 1f), Rando.Float(-0.1f, 0.1f));
        if (this._jabStance)
          return;
        this._swordSwing.speed = 1f;
        this._swordSwing.frame = 0;
      }
      else
      {
        if (!this._crouchStance || this._jabStance || (this.duck == null || this.duck.grounded))
          return;
        this._slamStance = true;
      }
    }

    public override void Fire()
    {
    }
  }
}
