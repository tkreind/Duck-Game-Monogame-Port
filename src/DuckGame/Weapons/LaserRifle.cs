// Decompiled with JetBrains decompiler
// Type: DuckGame.LaserRifle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|laser")]
  public class LaserRifle : Gun
  {
    public LaserRifle(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 30;
      this._ammoType = (AmmoType) new ATReboundLaser();
      this._ammoType.barrelAngleDegrees = 45f;
      this._type = "gun";
      this.graphic = new Sprite("laserRifle");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(16f, 10f);
      this._barrelOffsetTL = new Vec2(26f, 14f);
      this._fireSound = "laserRifle";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this._holdOffset = new Vec2(-2f, 0.0f);
    }
  }
}
