// Decompiled with JetBrains decompiler
// Type: DuckGame.ATSniper
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATSniper : AmmoType
  {
    public ATSniper()
    {
      this.accuracy = 0.75f;
      this.range = 250f;
      this.penetration = 1f;
      this.combustable = true;
      this.range = 1000f;
      this.accuracy = 1f;
      this.penetration = 2f;
      this.bulletSpeed = 48f;
    }

    public override void PopShell(float x, float y, int dir)
    {
      SniperShell sniperShell = new SniperShell(x, y);
      sniperShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) sniperShell);
    }
  }
}
