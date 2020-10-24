// Decompiled with JetBrains decompiler
// Type: DuckGame.ATPhaser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATPhaser : ATLaser
  {
    public ATPhaser()
    {
      this.accuracy = 0.8f;
      this.range = 600f;
      this.penetration = 1f;
      this.bulletSpeed = 10f;
      this.bulletThickness = 0.3f;
      this.bulletLength = 40f;
      this.rangeVariation = 50f;
      this.bulletType = typeof (LaserBullet);
      this.angleShot = false;
    }

    public override void WriteAdditionalData(BitBuffer b)
    {
      base.WriteAdditionalData(b);
      b.Write(this.bulletThickness);
      b.Write(this.penetration);
    }

    public override void ReadAdditionalData(BitBuffer b)
    {
      base.ReadAdditionalData(b);
      this.bulletThickness = b.ReadFloat();
      this.penetration = b.ReadFloat();
    }
  }
}
