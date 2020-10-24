// Decompiled with JetBrains decompiler
// Type: DuckGame.Holdable
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public abstract class Holdable : PhysicsObject
  {
    public StateBinding _triggerHeldBinding = new StateBinding(nameof (_triggerHeld));
    public StateBinding _canPickUpBinding = new StateBinding(nameof (canPickUp));
    public float raiseSpeed = 0.2f;
    protected Vec2 _prevHoverPos;
    private ItemSpawner _hoverSpawner;
    protected Vec2 _lastReceivedPosition = Vec2.Zero;
    public bool _keepRaised;
    protected bool _canRaise = true;
    public bool hoverRaise = true;
    public bool ignoreHands;
    protected bool _hasTrigger = true;
    public bool canPickUp = true;
    public Vec2 handOffset = new Vec2();
    protected Vec2 _holdOffset = new Vec2();
    public Vec2 _extraOffset = Vec2.Zero;
    public float handAngle;
    public bool handFlip;
    public Duck _equippedDuck;
    protected bool _raised;
    public bool _triggerHeld;
    private Duck _disarmedFrom;
    private DateTime _disarmTime = DateTime.MinValue;
    private Depth _oldDepth;
    protected bool _hasOldDepth;
    protected int _equippedDepth = 2;

    public override Thing netOwner
    {
      get => this.owner;
      set
      {
        this._prevOwner = this.owner;
        this._lastThrownBy = this._owner;
        this.owner = value;
      }
    }

    public ItemSpawner hoverSpawner
    {
      get => this._hoverSpawner;
      set
      {
        if (this._hoverSpawner != null && value == null)
          this.gravMultiplier = 1f;
        else if (this._hoverSpawner == null && value != null)
          this.gravMultiplier = 0.0f;
        if (value != null && this._hoverSpawner != value)
          this._prevHoverPos = this.position;
        this._hoverSpawner = value;
      }
    }

    public override Vec2 netPosition
    {
      get
      {
        if (this.owner != null)
          return new Vec2(-16000f, -8999f);
        return this.hoverSpawner == null ? this.position : this._prevHoverPos;
      }
      set
      {
        if ((double) value.x <= -15000.0)
          return;
        if (this.hoverSpawner == null || this._lastReceivedPosition != value || (double) (this._lastReceivedPosition - this.position).length > 16.0)
          this.position = value;
        this._lastReceivedPosition = value;
      }
    }

    public Duck duck => this._owner as Duck;

    public bool keepRaised => this._keepRaised;

    public bool canRaise => this._canRaise;

    public bool hasTrigger => this._hasTrigger;

    public Vec2 holdOffset => new Vec2(this._holdOffset.x * (float) this.offDir, this._holdOffset.y);

    public override Vec2 center
    {
      get => this._center + this._extraOffset;
      set => this._center = value;
    }

    public override Vec2 OffsetLocal(Vec2 pos)
    {
      Vec2 vec2 = pos * this.scale - this._extraOffset;
      if (this.offDir < (sbyte) 0)
        vec2.x *= -1f;
      vec2 = vec2.Rotate(this.angle, new Vec2(0.0f, 0.0f));
      return vec2;
    }

    public override Vec2 ReverseOffsetLocal(Vec2 pos)
    {
      Vec2 vec2 = pos * this.scale - this._extraOffset;
      vec2 = vec2.Rotate(-this.angle, new Vec2(0.0f, 0.0f));
      return vec2;
    }

    public override bool action => this._owner != null && this._owner.action;

    public Duck equippedDuck => this._equippedDuck;

    public bool raised
    {
      get => this._raised;
      set => this._raised = value;
    }

    public Duck disarmedFrom
    {
      get => this._disarmedFrom;
      set => this._disarmedFrom = value;
    }

    public DateTime disarmTime
    {
      get => this._disarmTime;
      set => this._disarmTime = value;
    }

    public int equippedDepth => this._equippedDepth;

    public Holdable()
    {
    }

    public Holdable(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public virtual void Thrown() => this.angle = 0.0f;

    public virtual void CheckIfHoldObstructed()
    {
      if (!(this.owner is Duck owner))
        return;
      if (this.offDir > (sbyte) 0)
      {
        Block block = Level.CheckLine<Block>(new Vec2(owner.x, this.y), new Vec2(this.right, this.y));
        if (block is Door && ((double) (block as Door)._jam == 1.0 || (double) (block as Door)._jam == -1.0))
          block = (Block) null;
        owner.holdObstructed = block != null;
      }
      else
      {
        Block block = Level.CheckLine<Block>(new Vec2(this.left, this.y), new Vec2(owner.x, this.y));
        if (block is Door && ((double) (block as Door)._jam == 1.0 || (double) (block as Door)._jam == -1.0))
          block = (Block) null;
        owner.holdObstructed = block != null;
      }
    }

    public virtual void ReturnToWorld()
    {
    }

    public int PickupPriority() => !(this is CTFPresent) ? (!(this is Gun) ? (!(this is Hat) || this is TinfoilHat ? (!(this is Equipment) ? 3 : 2) : 3) : 1) : 0;

    public void UpdateAction()
    {
      if (!this.isServerForObject)
        return;
      bool flag1 = false;
      bool flag2 = false;
      if (this.action)
      {
        if (!this._triggerHeld)
        {
          this.PressAction();
          flag1 = true;
        }
        else
          this.HoldAction();
      }
      else if (this._triggerHeld)
      {
        this.ReleaseAction();
        flag2 = true;
      }
      if (this is Gun && flag1)
        ++(this as Gun).bulletFireIndex;
      if (!Network.isActive || !this.isServerForObject || !(this is Gun))
        return;
      if (flag1 || (this as Gun).firedBullets.Count > 0)
        Send.Message((NetMessage) new NMFireGun(this as Gun, (this as Gun).firedBullets, (this as Gun).bulletFireIndex, !flag1 && flag2, this.duck != null ? this.duck.netProfileIndex : (byte) 4, !flag1 && !flag2), NetMessagePriority.Urgent);
      (this as Gun).firedBullets.Clear();
    }

    public override void Update()
    {
      if (this.owner != null)
      {
        if (!this._hasOldDepth)
        {
          this._oldDepth = this.depth;
          this._hasOldDepth = true;
        }
        Thing thing = this.owner;
        if (this.owner is Duck && (this.owner as Duck)._trapped != null)
          thing = (Thing) (this.owner as Duck)._trapped;
        this.depth = thing.depth + (this._equippedDuck != null ? this._equippedDepth : 8);
        this.offDir = thing.offDir;
        if (this.owner is Duck owner)
        {
          this._responsibleProfile = owner.profile;
          owner.UpdateHoldPosition(false);
        }
        this._sleeping = false;
        this.grounded = false;
        if (this.duck != null)
          return;
        this.UpdateAction();
      }
      else
      {
        if (this._hasOldDepth)
        {
          this.depth = this._oldDepth;
          this._hasOldDepth = false;
        }
        if (this.owner == null)
          this.UpdateAction();
        base.Update();
      }
    }

    public void UpdateHoldPositioning()
    {
    }

    public virtual void PressAction()
    {
      this._triggerHeld = true;
      this.OnPressAction();
      this.HoldAction();
    }

    public virtual void OnPressAction()
    {
    }

    public void HoldAction()
    {
      this._triggerHeld = true;
      this.OnHoldAction();
    }

    public virtual void OnHoldAction()
    {
    }

    public void ReleaseAction()
    {
      this._triggerHeld = false;
      this.OnReleaseAction();
    }

    public virtual void OnReleaseAction()
    {
    }
  }
}
