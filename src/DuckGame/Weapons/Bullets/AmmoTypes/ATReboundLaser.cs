// Decompiled with JetBrains decompiler
// Type: DuckGame.ATReboundLaser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATReboundLaser : ATLaser
  {
    public ATReboundLaser()
    {
      this.accuracy = 0.8f;
      this.range = 220f;
      this.penetration = 1f;
      this.bulletSpeed = 20f;
      this.bulletThickness = 0.3f;
      this.rebound = true;
      this.bulletType = typeof (LaserBullet);
      this.angleShot = true;
    }
  }
}
