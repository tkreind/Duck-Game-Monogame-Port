// Decompiled with JetBrains decompiler
// Type: DuckGame.IceBlock
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  [BaggedProperty("isInDemo", false)]
  public class IceBlock : Holdable, IPlatform
  {
    public StateBinding _hitPointsBinding = new StateBinding("_hitPoints");
    private SpriteMap _sprite;
    private float breakPoints = 15f;
    private float damageMultiplier;

    public IceBlock(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("iceBlock", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Ice Block";
      this.thickness = 2f;
      this.weight = 5f;
      this.buoyancy = 1f;
      this._hitPoints = 1f;
      this.impactThreshold = -1f;
      this._holdOffset = new Vec2(2f, 0.0f);
      this.flammable = 0.0f;
      this.collideSounds.Add("glassHit");
      this.superNonFlammable = true;
    }

    protected override float CalculatePersonalImpactPower(MaterialThing with, ImpactedFrom from) => base.CalculatePersonalImpactPower(with, from) - 1.5f;

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      if (with is PhysicsObject)
      {
        (with as PhysicsObject).specialFrictionMod = 0.16f;
        (with as PhysicsObject).modFric = true;
      }
      base.OnSolidImpact(with, from);
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal && this.owner == null)
        Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      for (int index = 0; index < 4; ++index)
      {
        Thing thing = (Thing) new GlassParticle(hitPos.x, hitPos.y, bullet.travelDirNormalized);
        Level.Add(thing);
        thing.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) (-(double) bullet.travelDirNormalized.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
        Level.Add(thing);
      }
      SFX.Play("glassHit", 0.6f);
      if (bullet.isLocal && TeamSelect2.Enabled("EXPLODEYCRATES"))
      {
        Thing.Fondle((Thing) this, DuckNetwork.localConnection);
        if (this.duck != null)
          this.duck.ThrowItem();
        this.Destroy((DestroyType) new DTShot(bullet));
        Level.Add((Thing) new GrenadeExplosion(this.x, this.y));
      }
      if (bullet.isLocal)
      {
        this.breakPoints -= this.damageMultiplier;
        this.damageMultiplier += 2f;
        if ((double) this.breakPoints <= 0.0)
          this.Destroy((DestroyType) new DTShot(bullet));
        --this.vSpeed;
        this.hSpeed += bullet.travelDirNormalized.x;
        this.vSpeed += bullet.travelDirNormalized.y;
      }
      return base.Hit(bullet, hitPos);
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      this._hitPoints = 0.0f;
      Level.Remove((Thing) this);
      SFX.Play("glassHit");
      Vec2 hitAngle = Vec2.Zero;
      if (type is DTShot)
        hitAngle = (type as DTShot).bullet.travelDirNormalized;
      for (int index = 0; index < 8; ++index)
      {
        Thing thing = (Thing) new GlassParticle(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f), hitAngle);
        Level.Add(thing);
        thing.hSpeed = (float) ((double) hitAngle.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) ((double) hitAngle.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
        Level.Add(thing);
      }
      for (int index = 0; index < 5; ++index)
      {
        SmallSmoke smallSmoke = SmallSmoke.New(this.x + Rando.Float(-6f, 6f), this.y + Rando.Float(-6f, 6f));
        smallSmoke.hSpeed += Rando.Float(-0.3f, 0.3f);
        smallSmoke.vSpeed -= Rando.Float(0.1f, 0.2f);
        Level.Add((Thing) smallSmoke);
      }
      return true;
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      for (int index = 0; index < 4; ++index)
      {
        Thing thing = (Thing) new GlassParticle(exitPos.x, exitPos.y, bullet.travelDirNormalized);
        Level.Add(thing);
        thing.hSpeed = (float) ((double) bullet.travelDirNormalized.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) ((double) bullet.travelDirNormalized.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
        Level.Add(thing);
      }
    }

    public override void HeatUp(Vec2 location)
    {
      this._hitPoints -= 0.016f;
      if ((double) this._hitPoints < 0.0500000007450581)
      {
        Level.Remove((Thing) this);
        for (int index = 0; index < 16; ++index)
        {
          FluidData water = Fluid.Water;
          water.amount = 3f / 1000f;
          Fluid fluid = new Fluid(this.x, this.y, Vec2.Zero, water);
          fluid.hSpeed = (float) ((double) index / 16.0 - 0.5) * Rando.Float(0.3f, 0.4f);
          fluid.vSpeed = Rando.Float(-1.5f, 0.5f);
          Level.Add((Thing) fluid);
        }
      }
      FluidData water1 = Fluid.Water;
      water1.amount = 0.004f;
      Fluid fluid1 = new Fluid(this.x, this.y, Vec2.Zero, water1);
      fluid1.hSpeed = Rando.Float(-0.1f, 0.1f);
      fluid1.vSpeed = Rando.Float(-0.3f, 0.3f);
      Level.Add((Thing) fluid1);
      base.HeatUp(location);
    }

    public override void Update()
    {
      base.Update();
      if ((double) this.damageMultiplier > 1.0)
      {
        this.damageMultiplier -= 0.2f;
      }
      else
      {
        this.damageMultiplier = 1f;
        this.breakPoints = 15f;
      }
      this._sprite.frame = (int) Math.Floor((1.0 - (double) this._hitPoints / 1.0) * 4.0);
      if (this._sprite.frame == 0)
      {
        this.collisionOffset = new Vec2(-8f, -8f);
        this.collisionSize = new Vec2(16f, 16f);
      }
      else if (this._sprite.frame == 1)
      {
        this.collisionOffset = new Vec2(-8f, -7f);
        this.collisionSize = new Vec2(16f, 15f);
      }
      else if (this._sprite.frame == 2)
      {
        this.collisionOffset = new Vec2(-8f, -4f);
        this.collisionSize = new Vec2(16f, 12f);
      }
      else if (this._sprite.frame == 3)
      {
        this.collisionOffset = new Vec2(-8f, -1f);
        this.collisionSize = new Vec2(16f, 9f);
      }
      FluidPuddle fluidPuddle = Level.CheckPoint<FluidPuddle>(this.position + new Vec2(0.0f, 4f));
      if (fluidPuddle != null)
      {
        if ((double) this.y + 4.0 - (double) fluidPuddle.top > 8.0)
        {
          this.gravMultiplier = -0.5f;
          this.grounded = false;
        }
        else
        {
          if ((double) this.y + 4.0 - (double) fluidPuddle.top < 3.0)
          {
            this.gravMultiplier = 0.2f;
            this.grounded = true;
          }
          else if ((double) this.y + 4.0 - (double) fluidPuddle.top > 4.0)
          {
            this.gravMultiplier = -0.2f;
            this.grounded = true;
          }
          this.grounded = true;
        }
      }
      else
        this.gravMultiplier = 1f;
    }
  }
}
