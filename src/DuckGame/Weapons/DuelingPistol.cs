// Decompiled with JetBrains decompiler
// Type: DuckGame.DuelingPistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  [BaggedProperty("isFatal", false)]
  [BaggedProperty("isInDemo", true)]
  public class DuelingPistol : Gun
  {
    public DuelingPistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 1;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._ammoType.range = 70f;
      this._ammoType.accuracy = 0.4f;
      this._ammoType.penetration = 0.4f;
      this._type = "gun";
      this.graphic = new Sprite("tinyGun");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(12f, 8f);
      this._barrelOffsetTL = new Vec2(20f, 15f);
      this._fireSound = "littleGun";
      this._kickForce = 0.0f;
    }
  }
}
