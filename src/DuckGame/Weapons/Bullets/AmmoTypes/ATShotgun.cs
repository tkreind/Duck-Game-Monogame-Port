// Decompiled with JetBrains decompiler
// Type: DuckGame.ATShotgun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATShotgun : AmmoType
  {
    public ATShotgun()
    {
      this.accuracy = 0.6f;
      this.range = 115f;
      this.penetration = 1f;
      this.rangeVariation = 10f;
      this.combustable = true;
    }

    public override void PopShell(float x, float y, int dir)
    {
      ShotgunShell shotgunShell = new ShotgunShell(x, y);
      shotgunShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) shotgunShell);
    }
  }
}
