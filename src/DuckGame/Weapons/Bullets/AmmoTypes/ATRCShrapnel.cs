// Decompiled with JetBrains decompiler
// Type: DuckGame.ATRCShrapnel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATRCShrapnel : AmmoType
  {
    public ATRCShrapnel()
    {
      this.accuracy = 0.75f;
      this.range = 250f;
      this.penetration = 0.4f;
      this.bulletSpeed = 18f;
      this.combustable = true;
    }

    public override void MakeNetEffect(Vec2 pos, bool fromNetwork = false)
    {
      for (int index = 0; index < 1; index = index + 1 + 1)
        Level.Add((Thing) new ExplosionPart(pos.x - 20f + Rando.Float(40f), pos.y - 20f + Rando.Float(40f)));
      SFX.Play("explode");
    }
  }
}
