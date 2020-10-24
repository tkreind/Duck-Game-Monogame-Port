// Decompiled with JetBrains decompiler
// Type: DuckGame.ATMagnum
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATMagnum : AmmoType
  {
    public float angle;

    public ATMagnum()
    {
      this.accuracy = 1f;
      this.range = 300f;
      this.penetration = 2f;
      this.bulletSpeed = 36f;
      this.combustable = true;
    }

    public override void PopShell(float x, float y, int dir)
    {
      MagnumShell magnumShell = new MagnumShell(x, y);
      magnumShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) magnumShell);
    }
  }
}
