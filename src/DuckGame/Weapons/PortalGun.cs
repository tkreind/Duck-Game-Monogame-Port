// Decompiled with JetBrains decompiler
// Type: DuckGame.PortalGun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [BaggedProperty("canSpawn", false)]
  public class PortalGun : Gun
  {
    private bool _curPortal;

    public bool curPortal
    {
      get => this._curPortal;
      set => this._curPortal = value;
    }

    public PortalGun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this._ammoType = (AmmoType) new ATPortal(this);
      this._ammoType.range = 600f;
      this._ammoType.accuracy = 1f;
      this._ammoType.rebound = false;
      this._ammoType.bulletSpeed = 10f;
      this._ammoType.bulletLength = 40f;
      this._ammoType.rangeVariation = 50f;
      (this._ammoType as ATPortal).angleShot = false;
      this._type = "gun";
      this.graphic = new Sprite("phaser");
      this.center = new Vec2(7f, 4f);
      this.collisionOffset = new Vec2(-7f, -4f);
      this.collisionSize = new Vec2(15f, 9f);
      this._barrelOffsetTL = new Vec2(14f, 3f);
      this._fireSound = "laserRifle";
      this._fullAuto = false;
      this._fireWait = 0.0f;
      this._kickForce = 0.5f;
      this._holdOffset = new Vec2(0.0f, 0.0f);
      this._flare = new SpriteMap("laserFlare", 16, 16);
      this._flare.center = new Vec2(0.0f, 8f);
    }
  }
}
