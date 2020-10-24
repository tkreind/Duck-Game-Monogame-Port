// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialThing
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public abstract class MaterialThing : Thing
  {
    protected NetSoundEffect _netCollideSound = new NetSoundEffect();
    public float buoyancy;
    protected bool ignoreCollisions;
    public float thickness = -1f;
    public PhysicsMaterial physicsMaterial = PhysicsMaterial.Wood;
    private bool _destroyedReal;
    protected bool _translucent;
    protected float _maxHealth;
    public float _hitPoints;
    public bool destructive;
    public bool onlyCrush;
    protected bool _dontCrush;
    private HashSet<MaterialThing> _clip = new HashSet<MaterialThing>();
    private HashSet<MaterialThing> _impacting = new HashSet<MaterialThing>();
    private HashSet<MaterialThing> _solidImpacting = new HashSet<MaterialThing>();
    private byte _planeOfExistence = 4;
    protected bool _didImpactSound;
    protected bool _grounded;
    protected float _bouncy;
    protected float _breakForce = 999f;
    protected float _impactThreshold = 0.5f;
    protected float _weight = 5f;
    public float spreadExtinguisherSmoke;
    private bool _islandDirty;
    private CollisionIsland _island;
    protected Thing _zapper;
    public bool willHeat;
    private Organizer<ImpactedFrom, string> _collideSounds = new Organizer<ImpactedFrom, string>();
    protected float _impactVolume = 0.5f;
    public float lastHSpeed;
    public float lastVSpeed;
    private Thing _lastBurnedBy;
    private float _flameWait;
    public float burnSpeed = 1f / 500f;
    private int extWait;
    public float flammable;
    public float heat;
    public float burnt;
    protected bool _onFire;
    public ActionTimer _burnWaitTimer;
    public bool isBurnMessage;
    public bool superNonFlammable;

    public static Vec2 ImpactVector(ImpactedFrom from)
    {
      switch (from)
      {
        case ImpactedFrom.Left:
          return new Vec2(-1f, 0.0f);
        case ImpactedFrom.Right:
          return new Vec2(1f, 0.0f);
        case ImpactedFrom.Top:
          return new Vec2(0.0f, -1f);
        default:
          return new Vec2(0.0f, 1f);
      }
    }

    public bool _destroyed
    {
      get => this._destroyedReal;
      set => this._destroyedReal = value;
    }

    public bool translucent => this._translucent;

    public virtual bool Hurt(float points)
    {
      if ((double) this._maxHealth == 0.0)
        return false;
      this._hitPoints -= points;
      return true;
    }

    public bool dontCrush
    {
      get => this._dontCrush;
      set => this._dontCrush = value;
    }

    public HashSet<MaterialThing> clip
    {
      get => this._clip;
      set => this._clip = value;
    }

    public HashSet<MaterialThing> impacting
    {
      get => this._impacting;
      set => this._impacting = value;
    }

    public HashSet<MaterialThing> solidImpacting
    {
      get => this._solidImpacting;
      set => this._solidImpacting = value;
    }

    public byte planeOfExistence
    {
      get => this._planeOfExistence;
      set => this._planeOfExistence = value;
    }

    public bool grounded
    {
      get => this._grounded;
      set => this._grounded = value;
    }

    public float bouncy
    {
      get => this._bouncy;
      set => this._bouncy = value;
    }

    public float breakForce
    {
      get => this._breakForce;
      set => this._breakForce = value;
    }

    public virtual bool destroyed => this._destroyed;

    public float impactThreshold
    {
      get => this._impactThreshold;
      set => this._impactThreshold = value;
    }

    public virtual float weight
    {
      get => this._weight;
      set => this._weight = value;
    }

    public float weightMultiplier
    {
      get
      {
        float num = this.weight;
        if ((double) num < 5.0)
          num = 5f;
        return 5f / num;
      }
    }

    public float weightMultiplierSmall
    {
      get
      {
        float num = this.weight * 0.75f;
        if ((double) num < 5.0)
          num = 5f;
        return 5f / num;
      }
    }

    public float weightMultiplierInv
    {
      get
      {
        float num = this.weight;
        if ((double) num < 5.0)
          num = 5f;
        return num / 5f;
      }
    }

    public float weightMultiplierInvTotal => this.weight / 5f;

    public bool islandDirty
    {
      get => this._islandDirty;
      set => this._islandDirty = value;
    }

    public CollisionIsland island
    {
      get => this._island;
      set => this._island = value;
    }

    public virtual void Zap(Thing zapper) => this._zapper = zapper;

    public void CheckIsland()
    {
      if (this.island == null || this.island.owner == this || (this.level == null || !this.level.simulatePhysics) || (double) (this.position - this.island.owner.position).lengthSq <= (double) this.island.radiusSquared)
        return;
      this.island.RemoveThing(this);
      this.UpdateIsland();
    }

    public void UpdateIsland()
    {
      CollisionIsland island = Level.current.things.GetIsland(this.position);
      if (this.island != null && this.island != island)
        this.island.RemoveThing(this);
      if (island != null)
      {
        if (this.island != island)
          island.AddThing(this);
      }
      else
        Level.current.things.AddIsland(this);
      this.islandDirty = false;
    }

    public override void DoInitialize() => base.DoInitialize();

    public override void DoTerminate()
    {
      if (this.island != null)
        this.island.RemoveThing(this);
      base.DoTerminate();
    }

    public virtual float impactPowerH => Math.Abs(this.hSpeed) * this.weightMultiplierInvTotal;

    public virtual float impactPowerV => Math.Abs(this.vSpeed) * this.weightMultiplierInvTotal;

    public float impactDirectionH => this.hSpeed * this.weightMultiplierInvTotal;

    public float impactDirectionV => this.vSpeed * this.weightMultiplierInvTotal;

    public float totalImpactPower => this.impactPowerH + this.impactPowerV;

    public Vec2 impactDirection => new Vec2(this.impactDirectionH, this.impactDirectionV);

    public Organizer<ImpactedFrom, string> collideSounds
    {
      get => this._collideSounds;
      set => this._collideSounds = value;
    }

    public float impactVolume
    {
      get => this._impactVolume;
      set => this._impactVolume = value;
    }

    public MaterialThing(float x, float y)
      : base(x, y)
    {
    }

    public void Bounce(ImpactedFrom direction)
    {
      if (direction == ImpactedFrom.Left || direction == ImpactedFrom.Right)
        this.BounceH();
      else
        this.BounceV();
    }

    public void BounceH() => this.hSpeed = -this.hSpeed * this.bouncy;

    public void BounceV() => this.vSpeed = -this.vSpeed * this.bouncy;

    public override void Update() => this._didImpactSound = false;

    public Thing lastBurnedBy => this._lastBurnedBy;

    public virtual void UpdateOnFire()
    {
      if ((double) this.burnt < 1.0)
      {
        if ((double) this._flameWait > 1.0)
        {
          this.AddFire();
          this._flameWait = 0.0f;
        }
        this._flameWait += 0.03f;
        this.burnt += this.burnSpeed;
      }
      else
        this.Destroy((DestroyType) new DTIncinerate(this._lastBurnedBy));
    }

    public override void DoUpdate()
    {
      if ((double) this.spreadExtinguisherSmoke > 0.0)
      {
        this.spreadExtinguisherSmoke -= 0.15f;
        if ((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 2.0)
        {
          ++this.extWait;
          if (this.extWait >= 3)
          {
            JetpackSmoke jetpackSmoke = new JetpackSmoke(this.x + Rando.Float(-1f, 1f), this.bottom + Rando.Float(-4f, 1f));
            jetpackSmoke.depth = new Depth(0.9f);
            Level.current.AddThing((Thing) jetpackSmoke);
            jetpackSmoke.hSpeed += this.hSpeed * Rando.Float(0.2f, 0.3f);
            jetpackSmoke.vSpeed = Rando.Float(-0.1f, 0.0f);
            jetpackSmoke.vSpeed -= Math.Abs(this.vSpeed) * Rando.Float(0.05f, 0.1f);
            this.extWait = 0;
          }
        }
      }
      if (this.isServerForObject)
      {
        if ((double) this.heat > 0.0)
          this.heat -= 1f / 500f;
        else
          this.heat = 0.0f;
        if (this._onFire)
          this.UpdateOnFire();
      }
      base.DoUpdate();
    }

    public void NetworkDestroy() => this.OnDestroy((DestroyType) new DTImpact((Thing) this));

    public virtual bool Destroy(DestroyType type = null)
    {
      if (!this._destroyed)
      {
        this._destroyed = this.OnDestroy(type);
        if (this.isServerForObject && this._destroyed && this.isStateObject)
        {
          foreach (NetworkConnection connection in Network.connections)
            Send.Message((NetMessage) new NMDestroyProp((Thing) this));
        }
      }
      return this._destroyed;
    }

    protected virtual bool OnDestroy(DestroyType type = null) => false;

    public virtual void Regenerate() => this._destroyed = false;

    public virtual bool DoHit(Bullet bullet, Vec2 hitPos) => this.Hit(bullet, hitPos);

    public virtual bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (this.physicsMaterial == PhysicsMaterial.Metal)
      {
        Level.Add((Thing) MetalRebound.New(hitPos.x, hitPos.y, (double) bullet.travelDirNormalized.x > 0.0 ? 1 : -1));
        hitPos -= bullet.travelDirNormalized;
        for (int index = 0; index < 3; ++index)
          Level.Add((Thing) Spark.New(hitPos.x, hitPos.y, bullet.travelDirNormalized));
      }
      return (double) this.thickness > (double) bullet.ammo.penetration;
    }

    public virtual void DoExitHit(Bullet bullet, Vec2 exitPos) => this.ExitHit(bullet, exitPos);

    public virtual void ExitHit(Bullet bullet, Vec2 exitPos)
    {
    }

    public bool onFire
    {
      get => this._onFire;
      set => this._onFire = value;
    }

    public void Burn(Vec2 firePosition, Thing litBy)
    {
      if (Network.isActive && !this.isServerForObject && (!this.isBurnMessage && this is Duck) && (this as Duck).profile != null)
        Send.Message((NetMessage) new NMLightDuck((this as Duck).profile.networkIndex));
      if (!this.isServerForObject && !this.isBurnMessage || this._onFire || this._burnWaitTimer != null && !(bool) this._burnWaitTimer)
        return;
      int num = this.onFire ? 1 : 0;
      this._onFire = this.OnBurn(firePosition, litBy);
      this._lastBurnedBy = litBy;
    }

    public virtual void UpdateFirePosition(SmallFire f)
    {
    }

    public virtual void AddFire() => Level.Add((Thing) SmallFire.New(Rando.Float((float) (((double) this.left - (double) this.x) * 0.699999988079071), (float) (((double) this.right - (double) this.x) * 0.699999988079071)), Rando.Float((float) (((double) this.top - (double) this.y) * 0.699999988079071), (float) (((double) this.bottom - (double) this.y) * 0.699999988079071)), 0.0f, 0.0f, stick: this));

    protected virtual bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      SFX.Play("ignite", pitch: (Rando.Float(0.3f) - 0.3f));
      this.AddFire();
      this.AddFire();
      return true;
    }

    public void DoHeatUp(float val, Vec2 location)
    {
      this.heat += val;
      if ((double) this.heat > 1.5)
        this.heat = 1.5f;
      if ((double) this.heat < 0.0)
        this.heat = 0.0f;
      if ((double) val <= 0.0)
        return;
      this.HeatUp(location);
    }

    public void DoHeatUp(float val)
    {
      this.heat += val;
      if ((double) this.heat > 1.5)
        this.heat = 1.5f;
      if ((double) this.heat < 0.0)
        this.heat = 0.0f;
      if ((double) val <= 0.0)
        return;
      this.HeatUp(this.position);
    }

    public virtual void HeatUp(Vec2 location)
    {
    }

    public virtual void Extinquish()
    {
      this._onFire = false;
      this._burnWaitTimer = new ActionTimer(0.05f, reset: false);
    }

    protected float CalculateImpactPower(MaterialThing with, ImpactedFrom from) => from == ImpactedFrom.Left || from == ImpactedFrom.Right ? this.impactPowerH + with.impactPowerH : this.impactPowerV + with.impactPowerV;

    protected virtual float CalculatePersonalImpactPower(MaterialThing with, ImpactedFrom from) => Math.Abs(from == ImpactedFrom.Left || from == ImpactedFrom.Right ? this.impactPowerH : this.impactPowerV);

    public virtual void Impact(MaterialThing with, ImpactedFrom from, bool solidImpact)
    {
      if (with.ignoreCollisions || this.ignoreCollisions || (double) this.CalculateImpactPower(with, from) <= (double) this._impactThreshold)
        return;
      if (!with.onlyCrush || from == ImpactedFrom.Top)
        this.OnSoftImpact(with, from);
      this.OnImpact(with, from);
    }

    public virtual void Touch(MaterialThing with)
    {
    }

    public virtual void SolidImpact(MaterialThing with, ImpactedFrom from)
    {
      if (with.ignoreCollisions || this.ignoreCollisions)
        return;
      float impactPower = this.CalculateImpactPower(with, from);
      if ((double) impactPower > (double) this._breakForce)
        this.Destroy((DestroyType) new DTImpact((Thing) with));
      if (this.collideSounds.HasGroup(from) && !this._didImpactSound && (double) this.CalculatePersonalImpactPower(with, from) > (double) this._impactThreshold)
      {
        if (Network.isActive)
        {
          if (this.isServerForObject)
            this._netCollideSound.Play();
        }
        else
          SFX.Play(this.collideSounds.GetRandom(from), this._impactVolume);
        this._didImpactSound = true;
      }
      if ((double) impactPower <= (double) this._impactThreshold)
        return;
      this.OnSolidImpact(with, from);
      this.OnImpact(with, from);
    }

    public virtual void OnImpact(MaterialThing with, ImpactedFrom from)
    {
    }

    public virtual void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
    }

    public virtual void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
    }

    public virtual void DrawGlow()
    {
    }
  }
}
