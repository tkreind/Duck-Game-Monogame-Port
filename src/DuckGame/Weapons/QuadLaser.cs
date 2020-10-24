// Decompiled with JetBrains decompiler
// Type: DuckGame.QuadLaser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|laser")]
  public class QuadLaser : Gun
  {
    public QuadLaser(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 3;
      this._ammoType = (AmmoType) new AT9mm();
      this._type = "gun";
      this.graphic = new Sprite("quadLaser");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(20f, 8f);
      this._fireSound = "pistolFire";
      this._kickForce = 3f;
      this.loseAccuracy = 0.1f;
      this.maxAccuracyLost = 0.6f;
      this._holdOffset = new Vec2(2f, -2f);
      this._bio = "Stop moving...";
      this._editorName = "Quad Laser";
    }

    public override void OnPressAction()
    {
      if (this.ammo <= 0)
        return;
      Vec2 vec2 = this.Offset(this.barrelOffset);
      if (this.isServerForObject)
      {
        QuadLaserBullet quadLaserBullet = new QuadLaserBullet(vec2.x, vec2.y, this.barrelVector);
        quadLaserBullet.killThingType = this.GetType();
        Level.Add((Thing) quadLaserBullet);
        if (this.duck != null)
        {
          this.duck.hSpeed = (float) (-(double) this.barrelVector.x * 8.0);
          this.duck.vSpeed = (float) (-(double) this.barrelVector.y * 4.0 - 2.0);
          quadLaserBullet.responsibleProfile = this.duck.profile;
        }
      }
      --this.ammo;
      SFX.Play("laserBlast");
    }
  }
}
