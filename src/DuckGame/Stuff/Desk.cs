// Decompiled with JetBrains decompiler
// Type: DuckGame.Desk
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  public class Desk : Holdable, IPlatform
  {
    public StateBinding _flippedBinding = new StateBinding(nameof (flipped));
    public StateBinding _flipBinding = new StateBinding(nameof (_flip));
    private float damageMultiplier = 1f;
    private SpriteMap _sprite;
    public int flipped;
    public bool landed = true;
    public float _flip;
    private bool firstFrame = true;

    public Desk(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._maxHealth = 15f;
      this._hitPoints = 15f;
      this._sprite = new SpriteMap("desk", 19, 12);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(9f, 6f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(17f, 6f);
      this.depth = new Depth(-0.5f);
      this._editorName = nameof (Desk);
      this.thickness = 8f;
      this.weight = 8f;
      this._holdOffset = new Vec2(2f, 2f);
      this.collideSounds.Add("thud");
      this.physicsMaterial = PhysicsMaterial.Metal;
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
      if ((double) this._flip < 0.0500000007450581 && (double) hitPos.y > (double) this.top + 4.0)
        return false;
      if ((double) this._hitPoints <= 0.0)
        return base.Hit(bullet, hitPos);
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(this.x - 8f + Rando.Float(16f), this.y - 8f + Rando.Float(16f));
        thing.hSpeed = (float) ((double) Math.Sign(bullet.travel.x) * (double) Rando.Float(2f) + (double) Math.Sign(bullet.travel.x) * 0.5);
        thing.vSpeed = -Rando.Float(1f);
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

    public override void Update()
    {
      base.Update();
      this.offDir = (sbyte) 1;
      if ((double) this.damageMultiplier > 1.0)
        this.damageMultiplier -= 0.2f;
      else
        this.damageMultiplier = 1f;
      this._sprite.frame = (int) Math.Floor((1.0 - (double) this._hitPoints / (double) this._maxHealth) * 4.0);
      if ((double) this._hitPoints <= 0.0 && !this._destroyed)
        this.Destroy((DestroyType) new DTImpact((Thing) this));
      this._flip = MathHelper.Lerp(this._flip, this.flipped != 0 ? 1.1f : -0.1f, 0.2f);
      if ((double) this._flip > 1.0)
        this._flip = 1f;
      if ((double) this._flip < 0.0)
        this._flip = 0.0f;
      if (this.owner != null && this.flipped != 0)
        this.flipped = 0;
      Vec2 collisionSize = this.collisionSize;
      Vec2 collisionOffset = this.collisionOffset;
      if ((double) this._flip == 0.0)
      {
        if (!this.landed)
          this.Land();
        this.collisionOffset = new Vec2(-8f, -6f);
        this.collisionSize = new Vec2(17f, 11f);
      }
      else if ((double) this._flip == 1.0)
      {
        if (!this.landed)
          this.Land();
        if (this.flipped > 0)
        {
          this.collisionOffset = new Vec2(0.0f, -12f);
          this.collisionSize = new Vec2(8f, 17f);
        }
        else
        {
          this.collisionOffset = new Vec2(-10f, -13f);
          this.collisionSize = new Vec2(8f, 17f);
        }
      }
      else
      {
        this.landed = false;
        this.collisionOffset = new Vec2(-2f, 4f);
        this.collisionSize = new Vec2(4f, 1f);
      }
      if (!this.firstFrame && (collisionOffset != this.collisionOffset || collisionSize != this.collisionSize))
        this.ReturnItemToWorld((Thing) this);
      if (this.flipped != 0)
      {
        this.centerx = (float) (9.0 + 4.0 * (double) this._flip * (this.flipped > 0 ? 1.0 : -1.0));
        this.centery = (float) (6.0 + 4.0 * (double) this._flip);
        this.angle = this._flip * (float) (1.5 * (this.flipped > 0 ? 1.0 : -1.0));
      }
      else
      {
        this.centerx = (float) (9.0 + 4.0 * (double) this._flip * ((double) this.angle > 0.0 ? 1.0 : -1.0));
        this.centery = (float) (6.0 + 4.0 * (double) this._flip);
        this.angle = this._flip * (float) (1.5 * ((double) this.angle > 0.0 ? 1.0 : -1.0));
      }
      this.firstFrame = false;
    }

    public void Flip(bool left)
    {
      if (this.owner != null || !this.isServerForObject)
        return;
      SFX.Play("swipe", 0.5f);
      if (this.grounded)
      {
        if (this.flipped == 0)
          this.vSpeed -= 1.4f;
        else
          --this.vSpeed;
      }
      if (this.flipped == 0)
        this.flipped = left ? -1 : 1;
      else
        this.flipped = 0;
    }

    public void Land()
    {
      this.landed = true;
      if (this.owner == null)
        SFX.Play("rockHitGround2", 0.7f);
      if (this.flipped > 0)
      {
        for (int index = 0; index < 2; ++index)
          Level.Add((Thing) SmallSmoke.New(this.bottomRight.x, this.bottomRight.y));
      }
      else
      {
        if (this.flipped >= 0)
          return;
        for (int index = 0; index < 2; ++index)
          Level.Add((Thing) SmallSmoke.New(this.bottomLeft.x, this.bottomLeft.y));
      }
    }
  }
}
