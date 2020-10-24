// Decompiled with JetBrains decompiler
// Type: DuckGame.ATDefault
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATDefault : AmmoType
  {
    public ATDefault()
    {
      this.accuracy = 0.75f;
      this.range = 250f;
      this.penetration = 1f;
      this.combustable = true;
    }

    public override void PopShell(float x, float y, int dir)
    {
      PistolShell pistolShell = new PistolShell(x, y);
      pistolShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) pistolShell);
    }
  }
}
