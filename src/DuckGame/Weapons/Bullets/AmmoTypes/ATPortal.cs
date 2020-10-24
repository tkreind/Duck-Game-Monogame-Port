// Decompiled with JetBrains decompiler
// Type: DuckGame.ATPortal
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATPortal : AmmoType
  {
    public bool angleShot = true;
    private PortalGun _ownerGun;

    public ATPortal(PortalGun OwnerGun)
    {
      this.accuracy = 0.75f;
      this.range = 250f;
      this.penetration = 1f;
      this.bulletSpeed = 20f;
      this.rebound = true;
      this.bulletThickness = 0.3f;
      this._ownerGun = OwnerGun;
    }

    public override Bullet FireBullet(
      Vec2 position,
      Thing owner = null,
      float angle = 0.0f,
      Thing firedFrom = null)
    {
      angle *= -1f;
      Bullet bullet = (Bullet) new PortalBullet(position.x, position.y, (AmmoType) this, angle, (Thing) this._ownerGun, this.rebound, thick: this.bulletThickness);
      bullet.firedFrom = firedFrom;
      Level.current.AddThing((Thing) bullet);
      return bullet;
    }
  }
}
