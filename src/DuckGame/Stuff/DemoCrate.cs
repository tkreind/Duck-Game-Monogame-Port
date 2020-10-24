// Decompiled with JetBrains decompiler
// Type: DuckGame.DemoCrate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [BaggedProperty("onlySpawnInDemo", true)]
  [EditorGroup("stuff|props")]
  public class DemoCrate : Holdable, IPlatform
  {
    private float damageMultiplier = 1f;
    private SpriteMap _sprite;

    public DemoCrate(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._maxHealth = 15f;
      this._hitPoints = 15f;
      this._sprite = new SpriteMap("demoCrate", 20, 20);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(10f, 10f);
      this.collisionOffset = new Vec2(-10f, -10f);
      this.collisionSize = new Vec2(20f, 19f);
      this.depth = new Depth(-0.5f);
      this._editorName = "Demo Crate";
      this.thickness = 2f;
      this.weight = 10f;
      this._holdOffset = new Vec2(2f, 0.0f);
      this.flammable = 0.3f;
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      this._hitPoints = 0.0f;
      SFX.Play("crateDestroy");
      Level.Remove((Thing) this);
      Vec2 vec2 = Vec2.Zero;
      if (type is DTShot)
        vec2 = (type as DTShot).bullet.travelDirNormalized;
      for (int index = 0; index < 6; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(this.x - 8f + Rando.Float(16f), this.y - 8f + Rando.Float(16f));
        thing.hSpeed = (float) (((double) Rando.Float(1f) > 0.5 ? 1.0 : -1.0) * (double) Rando.Float(3f) + (double) Math.Sign(vec2.x) * 0.5);
        thing.vSpeed = -Rando.Float(1f);
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

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if ((double) this._hitPoints <= 0.0)
        return base.Hit(bullet, hitPos);
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(hitPos.x, hitPos.y);
        thing.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) (-(double) bullet.travelDirNormalized.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
        Level.Add(thing);
      }
      SFX.Play("woodHit");
      if (bullet.isLocal)
      {
        this._hitPoints -= this.damageMultiplier;
        this.damageMultiplier += 2f;
        if ((double) this._hitPoints <= 0.0)
          this.Destroy((DestroyType) new DTShot(bullet));
      }
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(exitPos.x, exitPos.y);
        thing.hSpeed = (float) ((double) bullet.travelDirNormalized.x * 3.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) ((double) bullet.travelDirNormalized.y * 3.0 * ((double) Rando.Float(1f) + 0.300000011920929) - ((double) Rando.Float(2f) - 1.0));
        Level.Add(thing);
      }
    }

    public override void Update()
    {
      base.Update();
      if ((double) this.damageMultiplier > 1.0)
        this.damageMultiplier -= 0.2f;
      else
        this.damageMultiplier = 1f;
      if ((double) this._hitPoints <= 0.0 && !this._destroyed)
        this.Destroy((DestroyType) new DTImpact((Thing) this));
      if (this._onFire && (double) this.burnt < 0.899999976158142)
      {
        float num = 1f - this.burnt;
        if ((double) this._hitPoints > (double) num * (double) this._maxHealth)
          this._hitPoints = num * this._maxHealth;
        this._sprite.color = new Color(num, num, num);
      }
      if (Level.CheckPoint<FluidPuddle>(this.position) != null)
      {
        this.gravMultiplier = -0.2f;
        this.vSpeed *= 0.95f;
        this.grounded = true;
      }
      else
        this.gravMultiplier = 1f;
    }
  }
}
