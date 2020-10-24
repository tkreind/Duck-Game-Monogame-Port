// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public abstract class PhysicsObject : MaterialThing, ITeleport
  {
    private const short positionMax = 8191;
    public StateBinding _positionBinding = (StateBinding) new InterpolatedVec2Binding("netPosition");
    public StateBinding _velocityBinding = (StateBinding) new CompressedVec2Binding(GhostPriority.High, nameof (netVelocity), 20, true);
    public StateBinding _angleBinding = (StateBinding) new CompressedFloatBinding(GhostPriority.High, "_angle", 0.0f, 8, true, true);
    public StateBinding _offDirBinding = new StateBinding(GhostPriority.High, "_offDir");
    public StateBinding _ownerBinding = new StateBinding(GhostPriority.High, nameof (netOwner));
    public StateBinding _physicsStateBinding = (StateBinding) new StateFlagBinding(GhostPriority.High, new string[8]
    {
      "solid",
      "grounded",
      "onFire",
      "enablePhysics",
      "active",
      "visible",
      "_destroyed",
      nameof (isSpawned)
    });
    public StateBinding _burntBinding = (StateBinding) new CompressedFloatBinding("burnt", bits: 8);
    public StateBinding _collideSoundBinding = (StateBinding) new NetSoundBinding("_netCollideSound");
    public bool isSpawned;
    public float vMax = 8f;
    public float hMax = 12f;
    private Holdable _holdObject;
    public bool sliding;
    public bool crouch;
    public float friction = 0.1f;
    public float frictionMod;
    public float frictionMult = 1f;
    public float airFrictionMult = 1f;
    public float throwSpeedMultiplier = 1f;
    public static float gravity = 0.2f;
    protected bool _skipPlatforms;
    public bool wasHeldByPlayer;
    private PhysicsSnapshotBuffer _networkFrames = new PhysicsSnapshotBuffer();
    private PhysicsSnapshotBuffer _localFrames = new PhysicsSnapshotBuffer();
    protected bool duck;
    public float gravMultiplier = 1f;
    public float floatMultiplier = 1f;
    private MaterialThing _collideLeft;
    private MaterialThing _collideRight;
    private MaterialThing _collideTop;
    private MaterialThing _collideBottom;
    private MaterialThing _wallCollideLeft;
    private MaterialThing _wallCollideRight;
    protected bool _inPhysicsLoop;
    protected Vec2 _lastPosition = Vec2.Zero;
    protected Vec2 _lastVelocity = Vec2.Zero;
    private PhysicsSnapshotObject _lastSnapshot;
    public bool inRewindLoop;
    public bool predict;
    public Vec2 velocityBeforeFriction = Vec2.Zero;
    private bool _initedNetSounds;
    public bool skipClip;
    private FluidData _curFluid;
    protected FluidPuddle _curPuddle;
    public bool removedFromFall;
    public DateTime lastGrounded;
    public byte framesSinceGrounded = 99;
    public bool _sleeping;
    public bool doFloat;
    public bool platformSkip;
    public float specialFrictionMod = 1f;
    public bool modFric;
    public bool updatePhysics = true;
    public bool didSpawn;
    public bool spawnAnimation;
    private MaterialGrid _gridMaterial;
    private Material _oldMaterial;
    private bool _oldMaterialSet;

    public short netPositionX
    {
      get => (short) Maths.Clamp((int) Math.Round((double) this.position.x * 4.0), (int) short.MinValue, (int) short.MaxValue);
      set => this.position.x = (float) value / 4f;
    }

    public short netPositionY
    {
      get => (short) Maths.Clamp((int) Math.Round((double) this.position.y * 4.0), (int) short.MinValue, (int) short.MaxValue);
      set => this.position.y = (float) value / 4f;
    }

    public short netVelocityX
    {
      get => (short) Math.Round((double) this.hSpeed * 1000.0);
      set => this.hSpeed = (float) value / 1000f;
    }

    public short netVelocityY
    {
      get => (short) Math.Round((double) this.vSpeed * 1000.0);
      set => this.vSpeed = (float) value / 1000f;
    }

    public byte netAngle
    {
      get
      {
        float num = this.angleDegrees;
        if ((double) num < 0.0)
          num = Math.Abs(num) + 180f;
        return (byte) Math.Round((double) num % 360.0 / 2.0);
      }
      set => this.angleDegrees = (float) value * 2f;
    }

    public virtual Vec2 netVelocity
    {
      get => this.velocity;
      set => this.velocity = value;
    }

    public virtual Thing netOwner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public Thing clipThing
    {
      get => this.clip.Count == 0 ? (Thing) null : (Thing) this.clip.ElementAt<MaterialThing>(0);
      set
      {
        if (value != null && value != this)
        {
          this.clip.Clear();
          this.clip.Add(value as MaterialThing);
        }
        else
          this.clip.Clear();
      }
    }

    public Thing impactingThing
    {
      get => this.impacting.Count == 0 ? (Thing) null : (Thing) this.impacting.ElementAt<MaterialThing>(0);
      set
      {
        if (value != null && value != this)
        {
          this.impacting.Clear();
          this.impacting.Add(value as MaterialThing);
        }
        else
          this.impacting.Clear();
      }
    }

    public virtual Holdable holdObject
    {
      get => this._holdObject;
      set => this._holdObject = value;
    }

    public Gun gun => this.holdObject as Gun;

    public float currentFriction => (this.friction + this.frictionMod) * this.frictionMult;

    public virtual float currentGravity => PhysicsObject.gravity * this.gravMultiplier * this.floatMultiplier;

    public PhysicsSnapshotBuffer networkFrames => this._networkFrames;

    public PhysicsSnapshotBuffer localFrames => this._localFrames;

    public MaterialThing collideLeft => this._collideLeft;

    public MaterialThing collideRight => this._collideRight;

    public MaterialThing collideTop => this._collideTop;

    public MaterialThing collideBottom => this._collideBottom;

    public MaterialThing wallCollideLeft => this._wallCollideLeft;

    public MaterialThing wallCollideRight => this._wallCollideRight;

    public override float impactPowerV => base.impactPowerV - ((double) this.vSpeed > 0.0 ? this.currentGravity * this.weightMultiplierInvTotal : 0.0f);

    public override float hSpeed
    {
      get => !this._inPhysicsLoop ? this._hSpeed : this.lastHSpeed;
      set => this._hSpeed = value;
    }

    public override float vSpeed
    {
      get => !this._inPhysicsLoop ? this._vSpeed : this.lastVSpeed;
      set => this._vSpeed = value;
    }

    public Vec2 lastPosition => this._lastPosition;

    public Vec2 lastVelocity => this._lastVelocity;

    public PhysicsSnapshotObject lastSnapshot
    {
      get => this._lastSnapshot;
      set => this._lastSnapshot = value;
    }

    public bool ownerIsLocalController => this.owner != null && this.owner.responsibleProfile != null && this.owner.responsibleProfile.localPlayer;

    public float holdWeightMultiplier => this.holdObject != null ? this.holdObject.weightMultiplier : 1f;

    public float holdWeightMultiplierSmall => this.holdObject != null ? this.holdObject.weightMultiplierSmall : 1f;

    public PhysicsObject()
      : base(0.0f, 0.0f)
    {
      this.syncPriority = GhostPriority.Normal;
      PhysicsObject.gravity = !TeamSelect2.Enabled("MOOGRAV") ? 0.2f : 0.1f;
      this._physicsIndex = Thing.GetGlobalIndex();
      this._ghostType = Editor.IDToType[this.GetType()];
    }

    public PhysicsObject(float xval, float yval)
      : base(xval, yval)
    {
      this.syncPriority = GhostPriority.Normal;
      PhysicsObject.gravity = !TeamSelect2.Enabled("MOOGRAV") ? 0.2f : 0.1f;
      this._physicsIndex = Thing.GetGlobalIndex();
      this._ghostType = Editor.IDToType[this.GetType()];
    }

    public override void DoInitialize() => base.DoInitialize();

    public override void Initialize() => this._grounded = true;

    public override bool ShouldUpdate() => false;

    public bool sleeping
    {
      get => this._sleeping;
      set
      {
        if (this._sleeping && !value)
        {
          foreach (PhysicsObject physicsObject in Level.CheckLineAll<PhysicsObject>(this.topLeft + new Vec2(0.0f, -4f), this.topRight + new Vec2(0.0f, -4f)))
            physicsObject.sleeping = false;
        }
        this._sleeping = value;
      }
    }

    public virtual void UpdatePhysics()
    {
      if (this.framesSinceGrounded > (byte) 10)
        this.framesSinceGrounded = (byte) 10;
      this._lastPosition = this.position;
      this._lastVelocity = this.velocity;
      base.Update();
      if (!this.solid || !this.enablePhysics || this.level != null && !this.level.simulatePhysics)
      {
        this.lastGrounded = DateTime.Now;
      }
      else
      {
        this._collideLeft = (MaterialThing) null;
        this._collideRight = (MaterialThing) null;
        this._collideTop = (MaterialThing) null;
        this._collideBottom = (MaterialThing) null;
        this._wallCollideLeft = (MaterialThing) null;
        this._wallCollideRight = (MaterialThing) null;
        if (!this.skipClip)
        {
          this.clip.RemoveWhere((Predicate<MaterialThing>) (thing => thing == null || !Collision.Rect(this.topLeft, this.bottomRight, (Thing) thing)));
          this.impacting.RemoveWhere((Predicate<MaterialThing>) (thing => thing == null || !Collision.Rect(this.topLeft, this.bottomRight, (Thing) thing)));
        }
        if (this._sleeping)
        {
          if ((double) this.hSpeed == 0.0 && (double) this.vSpeed == 0.0)
            return;
          this._sleeping = false;
        }
        if (!this.skipClip)
          this.solidImpacting.RemoveWhere((Predicate<MaterialThing>) (thing => thing == null || !Collision.Rect(this.topLeft, this.bottomRight, (Thing) thing)));
        float currentFriction = this.currentFriction;
        if (this.sliding || this.crouch)
          currentFriction *= 0.28f;
        float num1 = currentFriction * this.specialFrictionMod;
        if (this.owner is Duck)
          this.gravMultiplier = 1f;
        if ((double) this.hSpeed > -(double) num1 && (double) this.hSpeed < (double) num1)
          this.hSpeed = 0.0f;
        if (this.duck)
        {
          if ((double) this.hSpeed > 0.0)
            this.hSpeed -= num1;
          if ((double) this.hSpeed < 0.0)
            this.hSpeed += num1;
        }
        else if (this.grounded)
        {
          if ((double) this.hSpeed > 0.0)
            this.hSpeed -= num1;
          if ((double) this.hSpeed < 0.0)
            this.hSpeed += num1;
        }
        else
        {
          if (this.isServerForObject && (double) this.y > (double) Level.current.lowestPoint + 500.0)
          {
            this.removedFromFall = true;
            switch (this)
            {
              case Duck _:
                return;
              case RagdollPart _:
                return;
              case TrappedDuck _:
                return;
              default:
                Level.Remove((Thing) this);
                break;
            }
          }
          if ((double) this.hSpeed > 0.0)
            this.hSpeed -= num1 * 0.7f * this.airFrictionMult;
          if ((double) this.hSpeed < 0.0)
            this.hSpeed += num1 * 0.7f * this.airFrictionMult;
        }
        if ((double) this.hSpeed > (double) this.hMax)
          this.hSpeed = this.hMax;
        if ((double) this.hSpeed < -(double) this.hMax)
          this.hSpeed = -this.hMax;
        Vec2 p1_1 = this.topLeft + new Vec2(0.0f, 0.5f);
        Vec2 p2_1 = this.bottomRight + new Vec2(0.0f, -0.5f);
        this.lastHSpeed = this.hSpeed;
        float num2 = 0.0f;
        bool flag1 = false;
        if ((double) this.hSpeed != 0.0)
        {
          int num3 = (int) Math.Ceiling((double) Math.Abs(this.hSpeed) / 4.0);
          float hSpeed = this.hSpeed;
          for (int index = 0; index < num3; ++index)
          {
            float num4 = this.hSpeed / (float) num3;
            if ((double) num4 != 0.0 && Math.Sign(num4) == Math.Sign(hSpeed))
            {
              this.x += num4;
              if ((double) this.hSpeed < 0.0)
              {
                p1_1.x += num4;
                p2_1.x -= 2f;
              }
              else
              {
                p2_1.x += num4;
                p1_1.x += 2f;
              }
              IEnumerable<MaterialThing> source = Level.CheckRectAll<MaterialThing>(p1_1, p2_1);
              if (Network.isActive && !this.isServerForObject && (double) Math.Abs(this.hSpeed) > 0.5)
              {
                foreach (Duck duck in Level.CheckRectAll<Duck>(p1_1 + new Vec2(this.hSpeed * 2f, 0.0f), p2_1 + new Vec2(this.hSpeed * 2f, 0.0f)))
                {
                  if ((double) this.hSpeed > 0.0)
                    duck.Impact((MaterialThing) this, ImpactedFrom.Left, true);
                  else if ((double) this.hSpeed < 0.0)
                    duck.Impact((MaterialThing) this, ImpactedFrom.Right, true);
                }
              }
              IEnumerable<MaterialThing> materialThings = (double) this.hSpeed <= 0.0 ? (IEnumerable<MaterialThing>) source.OrderBy<MaterialThing, float>((Func<MaterialThing, float>) (l => (float) (-(double) l.x + (l is Block ? 10000.0 : 0.0)))) : (IEnumerable<MaterialThing>) source.OrderBy<MaterialThing, float>((Func<MaterialThing, float>) (l => l.x + (l is Block ? -10000f : 0.0f)));
              this._inPhysicsLoop = true;
              foreach (MaterialThing with in materialThings)
              {
                if (with != this && !this.clip.Contains(with) && (!with.clip.Contains((MaterialThing) this) && with.solid) && ((!Network.isClient || with.ghostType != (ushort) 0) && (this.planeOfExistence == (byte) 4 || (int) with.planeOfExistence == (int) this.planeOfExistence)))
                {
                  bool flag2 = false;
                  if ((double) with.left <= (double) this.right && (double) with.left > (double) this.left)
                  {
                    flag2 = true;
                    if ((double) this.hSpeed > 0.0)
                    {
                      this._collideRight = with;
                      if (with is Block)
                        this._wallCollideRight = with;
                      with.Impact((MaterialThing) this, ImpactedFrom.Left, true);
                      this.Impact(with, ImpactedFrom.Right, true);
                    }
                  }
                  if ((double) with.right >= (double) this.left && (double) with.right < (double) this.right)
                  {
                    flag2 = true;
                    if ((double) this.hSpeed < 0.0)
                    {
                      this._collideLeft = with;
                      if (with is Block)
                        this._wallCollideLeft = with;
                      with.Impact((MaterialThing) this, ImpactedFrom.Right, true);
                      this.Impact(with, ImpactedFrom.Left, true);
                    }
                  }
                  if (flag2)
                  {
                    with.Touch((MaterialThing) this);
                    this.Touch(with);
                  }
                }
              }
              this._inPhysicsLoop = false;
            }
            else
              break;
          }
        }
        if (flag1)
          this.x = num2;
        if ((double) this.vSpeed > (double) this.vMax)
          this.vSpeed = this.vMax;
        if ((double) this.vSpeed < -(double) this.vMax)
          this.vSpeed = -this.vMax;
        this.vSpeed += this.currentGravity;
        if ((double) this.vSpeed < 0.0)
          this.grounded = false;
        this.grounded = false;
        ++this.framesSinceGrounded;
        if ((double) this.vSpeed <= 0.0)
          Math.Floor((double) this.vSpeed);
        else
          Math.Ceiling((double) this.vSpeed);
        Vec2 p1_2 = this.topLeft + new Vec2(0.5f, 0.0f);
        Vec2 p2_2 = this.bottomRight + new Vec2(-0.5f, 0.0f);
        float num5 = -9999f;
        bool flag3 = false;
        int num6 = (int) Math.Ceiling((double) Math.Abs(this.vSpeed) / 4.0);
        float vSpeed = this.vSpeed;
        this.lastVSpeed = this.vSpeed;
        for (int index = 0; index < num6; ++index)
        {
          float num3 = this.vSpeed / (float) num6;
          if ((double) num3 != 0.0 && Math.Sign(num3) == Math.Sign(vSpeed))
          {
            if ((double) this.vSpeed < 0.0)
            {
              p1_2.y += num3;
              p2_2.y -= 2f;
            }
            else
            {
              p2_2.y += num3;
              p1_2.y += 2f;
            }
            IEnumerable<MaterialThing> source = Level.CheckRectAll<MaterialThing>(p1_2, p2_2);
            this.y += num3;
            IEnumerable<MaterialThing> materialThings = (double) this.vSpeed <= 0.0 ? (IEnumerable<MaterialThing>) source.OrderBy<MaterialThing, float>((Func<MaterialThing, float>) (l => (float) (-(double) l.y + (l is Block ? -10000.0 : 0.0)))) : (IEnumerable<MaterialThing>) source.OrderBy<MaterialThing, float>((Func<MaterialThing, float>) (l => l.y + (l is Block ? 10000f : 0.0f)));
            double top = (double) this.top;
            double bottom = (double) this.bottom;
            if (this is Duck duck && duck.inputProfile.Down("DOWN"))
            {
              int jumpValid = duck._jumpValid;
            }
            this._inPhysicsLoop = true;
            foreach (MaterialThing with in materialThings)
            {
              if (with is FluidPuddle)
              {
                flag3 = true;
                this._curFluid = (with as FluidPuddle).data;
                this._curPuddle = with as FluidPuddle;
                if ((double) with.top < (double) this.bottom - 2.0 && (double) with.collisionSize.y > 2.0)
                  num5 = with.top;
              }
              if (with != this && !this.clip.Contains(with) && (!with.clip.Contains((MaterialThing) this) && with.solid) && ((!Network.isClient || with.ghostType != (ushort) 0) && (this.planeOfExistence == (byte) 4 || (int) with.planeOfExistence == (int) this.planeOfExistence)))
              {
                bool flag2 = false;
                if ((double) with.bottom >= (double) this.top && (double) with.top < (double) this.top)
                {
                  flag2 = true;
                  if ((double) this.vSpeed < 0.0)
                  {
                    this._collideTop = with;
                    with.Impact((MaterialThing) this, ImpactedFrom.Bottom, true);
                    this.Impact(with, ImpactedFrom.Top, true);
                  }
                }
                if ((double) with.top <= (double) this.bottom && (double) with.bottom > (double) this.bottom)
                {
                  flag2 = true;
                  if ((double) this.vSpeed > 0.0)
                  {
                    this._collideBottom = with;
                    with.Impact((MaterialThing) this, ImpactedFrom.Top, true);
                    this.Impact(with, ImpactedFrom.Bottom, true);
                  }
                }
                if (flag2)
                {
                  with.Touch((MaterialThing) this);
                  this.Touch(with);
                }
              }
            }
            this._inPhysicsLoop = false;
          }
          else
            break;
        }
        if (this.grounded)
        {
          this.lastGrounded = DateTime.Now;
          this.framesSinceGrounded = (byte) 0;
          if (!this.doFloat && (double) this.hSpeed == 0.0 && ((double) this.vSpeed == 0.0 && !(this._collideBottom is PhysicsObject)) && ((this._collideBottom is Block || this._collideBottom is IPlatform) && (!(this._collideBottom is ItemBox) || (this._collideBottom as ItemBox).canBounce)))
          {
            if (this is Crate)
            {
              AutoBlock collideBottom = this._collideBottom as AutoBlock;
            }
            this._sleeping = true;
          }
        }
        if ((double) num5 > -999.0)
        {
          if (!this.doFloat && (double) this.vSpeed > 1.0)
          {
            Level.Add((Thing) new WaterSplash(this.x, num5 - 3f, this._curFluid));
            SFX.Play("largeSplash", Rando.Float(0.6f, 0.7f), Rando.Float(-0.7f, -0.2f));
          }
          this.doFloat = true;
        }
        else
          this.doFloat = false;
        if (this.doFloat)
        {
          if ((double) this.floatMultiplier > 0.980000019073486)
            this.vSpeed *= 0.4f;
          this.vSpeed *= 0.95f;
          this.floatMultiplier = 0.4f;
          if (this.onFire && (double) this._curFluid.flammable <= 0.5 && (double) this._curFluid.heat <= 0.5)
            this.onFire = false;
          else if ((double) this._curFluid.heat > 0.5)
          {
            if ((double) this.flammable > 0.0)
            {
              bool onFire = this.onFire;
              this.Burn(this.position, (Thing) this);
              if (this is Duck && (this as Duck).onFire && !onFire)
                (this as Duck).ThrowItem();
            }
            this.DoHeatUp(0.015f, this.position);
          }
          else
            this.DoHeatUp(-0.05f, this.position);
        }
        else
        {
          if (flag3 && (double) vSpeed > 1.0 && (double) Math.Abs(this.vSpeed) < 0.00999999977648258)
          {
            Level.Add((Thing) new WaterSplash(this.x, this.bottom, this._curFluid));
            SFX.Play("littleSplash", Rando.Float(0.8f, 0.9f), Rando.Float(-0.2f, 0.2f));
          }
          this.floatMultiplier = 1f;
        }
        Recorder.LogVelocity(Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed));
        if (this._sleeping)
          return;
        if (this.modFric)
          this.modFric = false;
        else
          this.specialFrictionMod = 1f;
      }
    }

    public override void Impact(MaterialThing with, ImpactedFrom from, bool solidImpact)
    {
      bool flag1 = true;
      bool flag2 = false;
      switch (with)
      {
        case Block _:
          flag2 = true;
          with.SolidImpact((MaterialThing) this, from);
          if (with.destroyed)
            return;
          if (from == ImpactedFrom.Right)
          {
            this.x = with.left + (this.x - this.right);
            this.SolidImpact(with, from);
            if ((double) this.hSpeed > -(double) this.hSpeed * (double) this.bouncy)
            {
              this.hSpeed = -this.hSpeed * this.bouncy;
              if ((double) Math.Abs(this.hSpeed) < 0.100000001490116)
                this.hSpeed = 0.0f;
            }
          }
          if (from == ImpactedFrom.Left)
          {
            this.x = with.right + (this.x - this.left);
            this.SolidImpact(with, from);
            if ((double) this.hSpeed < -(double) this.hSpeed * (double) this.bouncy)
            {
              this.hSpeed = -this.hSpeed * this.bouncy;
              if ((double) Math.Abs(this.hSpeed) < 0.100000001490116)
                this.hSpeed = 0.0f;
            }
          }
          if (from == ImpactedFrom.Top)
          {
            this.y = (float) ((double) with.bottom + ((double) this.y - (double) this.top) + 1.0);
            this.SolidImpact(with, from);
            if ((double) this.vSpeed < -(double) this.vSpeed * (double) this.bouncy)
            {
              this.vSpeed = -this.vSpeed * this.bouncy;
              if ((double) Math.Abs(this.vSpeed) < 0.100000001490116)
                this.vSpeed = 0.0f;
            }
          }
          if (from == ImpactedFrom.Bottom)
          {
            this.y = with.top + (this.y - this.bottom);
            this.SolidImpact(with, from);
            if ((double) this.vSpeed > -(double) this.vSpeed * (double) this.bouncy)
            {
              this.vSpeed = -this.vSpeed * this.bouncy;
              if ((double) Math.Abs(this.vSpeed) < 0.100000001490116)
                this.vSpeed = 0.0f;
            }
            this.grounded = true;
            break;
          }
          break;
        case IPlatform _:
          flag2 = false;
          if (from == ImpactedFrom.Bottom)
          {
            if (with is PhysicsObject && (!with.grounded || (double) Math.Abs(with.vSpeed) >= 0.300000011920929) || ((double) with.top + ((double) this.vSpeed + 2.0) <= (double) this.bottom || this._skipPlatforms))
              return;
            with.SolidImpact((MaterialThing) this, ImpactedFrom.Top);
            if (with.destroyed)
              return;
            this.y = with.top + (this.y - this.bottom);
            this.SolidImpact(with, from);
            if ((double) this.vSpeed > -(double) this.vSpeed * (double) this.bouncy)
            {
              this.vSpeed = -this.vSpeed * this.bouncy;
              if ((double) Math.Abs(this.vSpeed) < 0.100000001490116)
                this.vSpeed = 0.0f;
            }
            this.grounded = true;
            break;
          }
          break;
        default:
          flag1 = false;
          break;
      }
      if (flag1)
      {
        if (!flag2 && !this.impacting.Contains(with))
        {
          base.Impact(with, from, solidImpact);
          this.impacting.Add(with);
        }
        else
        {
          if (!flag2 || this.solidImpacting.Contains(with))
            return;
          base.Impact(with, from, solidImpact);
          this.solidImpacting.Add(with);
        }
      }
      else
        base.Impact(with, from, solidImpact);
    }

    public override void Update()
    {
      if (Network.isActive && !this._initedNetSounds)
      {
        this._initedNetSounds = true;
        List<string> list = this.collideSounds.GetList(ImpactedFrom.Bottom);
        if (list != null)
        {
          this._netCollideSound = new NetSoundEffect(list.ToArray());
          this._netCollideSound.volume = this._impactVolume;
        }
      }
      if (!this.updatePhysics)
        return;
      this.UpdatePhysics();
    }

    public override void DoDraw()
    {
      if (!Content.renderingPreview && this.spawnAnimation)
      {
        if (this._gridMaterial == null)
          this._gridMaterial = new MaterialGrid((Thing) this);
        if (!this._gridMaterial.finished)
        {
          if (!this._oldMaterialSet)
          {
            this._oldMaterial = this.material;
            this._oldMaterialSet = true;
          }
          this.material = (Material) this._gridMaterial;
        }
        else
        {
          if (this._oldMaterialSet)
            this.material = this._oldMaterial;
          this.spawnAnimation = false;
        }
      }
      base.DoDraw();
    }

    public override void Draw()
    {
      if (this.graphic != null)
        this.graphic.flipH = this.offDir <= (sbyte) 0;
      base.Draw();
    }

    public override void NetworkUpdate()
    {
    }
  }
}
