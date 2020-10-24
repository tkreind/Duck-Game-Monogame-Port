// Decompiled with JetBrains decompiler
// Type: DuckGame.Dart
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Dart : PhysicsObject, IPlatform
  {
    public StateBinding _stickTimeBinding = new StateBinding(nameof (_stickTime));
    public StateBinding _stuckBinding = new StateBinding(nameof (_stuck));
    private SpriteMap _sprite;
    public bool _stuck;
    public float _stickTime = 1f;
    private Duck _owner;
    public bool burning;

    public Dart(float xpos, float ypos, Duck owner, float fireAngle)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("dart", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-4f, -2f);
      this.collisionSize = new Vec2(9f, 4f);
      this.depth = new Depth(-0.5f);
      this.thickness = 1f;
      this.weight = 3f;
      this._owner = owner;
      this.breakForce = 1f;
      this._stickTime = 2f + Rando.Float(0.8f);
      if ((double) Rando.Float(1f) > 0.949999988079071)
        this._stickTime += Rando.Float(15f);
      this.angle = fireAngle;
      if (owner == null)
        return;
      owner.clip.Add((MaterialThing) this);
      this.clip.Add((MaterialThing) owner);
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (this._stuck && (double) this._stickTime > 0.980000019073486)
        return false;
      if (type is DTFade)
      {
        DartShell dartShell = new DartShell(this.x, this.y, Rando.Float(0.1f) * -this._sprite.flipMultH, this._sprite.flipH);
        dartShell.angle = this.angle;
        Level.Add((Thing) dartShell);
        dartShell.hSpeed = (float) ((0.5 + (double) Rando.Float(0.3f)) * -(double) this._sprite.flipMultH);
        Level.Remove((Thing) this);
        return true;
      }
      if (this._stuck && (double) this._stickTime > 0.100000001490116)
        this._stickTime = 0.1f;
      return false;
    }

    public override void OnImpact(MaterialThing with, ImpactedFrom from)
    {
      if (this._stuck || with is Gun || (double) with.weight < 5.0 && !(with is Dart) || (with is FeatherVolume || with is Teleporter || this.removeFromLevel))
        return;
      switch (with)
      {
        case Spring _:
          break;
        case SpringUpLeft _:
          break;
        case SpringUpRight _:
          break;
        default:
          if (this.destroyed || this._stuck)
            break;
          if (with is PhysicsObject)
          {
            if (with is Duck duck)
            {
              duck.hSpeed += this.hSpeed * 0.25f;
              duck.vSpeed -= 0.3f;
              Event.Log((Event) new DartHitEvent(this.responsibleProfile, duck.profile));
              if (duck.holdObject is Grenade)
                duck.forceFire = true;
              if ((double) Rando.Float(1f) > 0.600000023841858)
                duck.Swear();
              duck.ThrowItem();
            }
            // TODO
            //if (with is IPlatform || duck != null)
            //{
            //  DartShell dartShell = new DartShell(this.x, this.y, -this._sprite.flipMultH * Rando.Float(0.6f), this._sprite.flipH);
            //  Level.Add((Thing) dartShell);
            //  dartShell.hSpeed = (float) (-(double) this.hSpeed / 3.0 * (0.300000011920929 + (double) Rando.Float(0.8f)));
            //  dartShell.vSpeed = Rando.Float(4f) - 2f;
            //  Level.Remove((Thing) this);
            //  if (!this.burning)
            //    break;
            //  with.Burn(this.position, (Thing) this);
            //  break;
            //}
          }
          float num = (float) (-(double) this.angleDegrees % 360.0);
          if ((double) num < 0.0)
            num += 360f;
          bool flag = false;
          if (from == ImpactedFrom.Right && ((double) num < 45.0 || (double) num > 315.0))
          {
            flag = true;
            this.angleDegrees = 0.0f;
          }
          else if (from == ImpactedFrom.Top && (double) num > 45.0 && (double) num < 135.0)
          {
            flag = true;
            this.angleDegrees = 270f;
          }
          else if (from == ImpactedFrom.Left && (double) num > 135.0 && (double) num < 225.0)
          {
            flag = true;
            this.angleDegrees = 180f;
          }
          else if (from == ImpactedFrom.Bottom && (double) num > 225.0 && (double) num < 315.0)
          {
            flag = true;
            this.angleDegrees = 90f;
          }
          if (!flag)
            break;
          this._stuck = true;
          SFX.Play("dartStick", 0.8f, Rando.Float(0.2f) - 0.1f);
          this.vSpeed = 0.0f;
          this.gravMultiplier = 0.0f;
          this.grounded = true;
          this._sprite.frame = 1;
          this._stickTime = 1f;
          break;
      }
    }

    public override void Update()
    {
      base.Update();
      if (!this.destroyed && !this._stuck)
      {
        if (!this.burning && Level.CheckCircle<SmallFire>(this.position, 8f) != null)
        {
          this.burning = true;
          this.onFire = true;
          Level.Add((Thing) SmallFire.New(0.0f, 0.0f, 0.0f, 0.0f, stick: ((MaterialThing) this), firedFrom: ((Thing) this)));
          SFX.Play("ignite", Rando.Float(0.9f, 1f), Rando.Float(-0.2f, 0.2f));
        }
        this._sprite.frame = 0;
        this.angleDegrees = -Maths.PointDirection(Vec2.Zero, new Vec2(this.hSpeed, this.vSpeed));
      }
      if (this._stuck)
      {
        this.vSpeed = 0.0f;
        this.hSpeed = 0.0f;
        this.grounded = true;
        this._sprite.frame = 1;
        this._stickTime -= 0.01f;
        this.gravMultiplier = 0.0f;
      }
      if ((double) this._stickTime > 0.0 || this.destroyed)
        return;
      this.Destroy((DestroyType) new DTFade());
    }
  }
}
