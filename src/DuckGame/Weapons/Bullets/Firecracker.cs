// Decompiled with JetBrains decompiler
// Type: DuckGame.Firecracker
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Firecracker : PhysicsParticle, ITeleport
  {
    private ActionTimer _sparkTimer = (ActionTimer) 0.2f;
    private ActionTimer _explodeTimer = (ActionTimer) Rando.Float(0.01f, 0.012f);

    public Firecracker(float xpos, float ypos, float ang = 0.0f)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("fireCracker");
      this.center = new Vec2(4f, 4f);
      this._bounceSound = "plasticBounce";
      this._airFriction = 0.02f;
      this._bounceEfficiency = 0.65f;
      this._spinAngle = ang;
    }

    public override void Update()
    {
      base.Update();
      this._life = 1f;
      this.angleDegrees = this._spinAngle;
      if ((bool) this._sparkTimer)
        Level.Add((Thing) Spark.New(this.x, this.y - 2f, new Vec2(Rando.Float(-1f, 1f), -0.5f), 0.1f));
      if (!(bool) this._explodeTimer)
        return;
      SFX.Play("littleGun", Rando.Float(0.8f, 1f), Rando.Float(-0.5f, 0.5f));
      for (int index = 0; index < 8; ++index)
      {
        float num = (float) ((double) index * 45.0 - 5.0) + Rando.Float(10f);
        ATShrapnel atShrapnel = new ATShrapnel();
        atShrapnel.range = 8f + Rando.Float(3f);
        Level.Add((Thing) new Bullet(this.x + (float) (Math.Cos((double) Maths.DegToRad(num)) * 6.0), this.y - (float) (Math.Sin((double) Maths.DegToRad(num)) * 6.0), (AmmoType) atShrapnel, num)
        {
          firedFrom = (Thing) this
        });
      }
      Level.Add((Thing) SmallSmoke.New(this.x, this.y));
      if ((double) Rando.Float(1f) < 0.100000001490116)
        Level.Add((Thing) SmallFire.New(this.x, this.y, 0.0f, 0.0f, firedFrom: ((Thing) this)));
      Level.Remove((Thing) this);
    }
  }
}
