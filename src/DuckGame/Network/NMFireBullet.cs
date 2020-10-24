// Decompiled with JetBrains decompiler
// Type: DuckGame.NMFireBullet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMFireBullet : NMEvent
  {
    public float range;
    public float speed;
    public float angle;
    public AmmoType typeInstance;

    public NMFireBullet()
    {
    }

    public NMFireBullet(float varRange, float varSpeed, float varAngle)
    {
      this.range = varRange;
      this.speed = varSpeed;
      this.angle = varAngle;
    }

    public void DoActivate(Vec2 position, Profile owner)
    {
      this.typeInstance.rangeVariation = 0.0f;
      this.typeInstance.accuracy = 1f;
      this.typeInstance.bulletSpeed = this.speed;
      this.typeInstance.speedVariation = 0.0f;
      Bullet bullet = this.typeInstance.GetBullet(position.x, position.y, (Thing) owner?.duck, -this.angle, distance: this.range, network: false);
      bullet.isLocal = false;
      Level.current.AddThing((Thing) bullet);
    }
  }
}
