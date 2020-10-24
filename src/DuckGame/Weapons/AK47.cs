// Decompiled with JetBrains decompiler
// Type: DuckGame.AK47
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|machine guns")]
  [BaggedProperty("isSuperWeapon", true)]
  public class AK47 : Gun
  {
    public AK47(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 30;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.range = 200f;
      this._ammoType.accuracy = 0.85f;
      this._ammoType.penetration = 2f;
      this._type = "gun";
      this.graphic = new Sprite("ak47");
      this.center = new Vec2(16f, 15f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(18f, 10f);
      this._barrelOffsetTL = new Vec2(32f, 14f);
      this._fireSound = "deepMachineGun2";
      this._fullAuto = true;
      this._fireWait = 1.2f;
      this._kickForce = 3.5f;
      this.loseAccuracy = 0.2f;
      this.maxAccuracyLost = 0.8f;
    }
  }
}
