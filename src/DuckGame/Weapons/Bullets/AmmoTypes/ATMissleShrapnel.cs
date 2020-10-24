// Decompiled with JetBrains decompiler
// Type: DuckGame.ATMissileShrapnel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATMissileShrapnel : AmmoType
  {
    public ATMissileShrapnel()
    {
      this.accuracy = 0.75f;
      this.range = 250f;
      this.penetration = 0.4f;
      this.bulletSpeed = 18f;
      this.combustable = true;
    }

    public override void MakeNetEffect(Vec2 pos, bool fromNetwork = false)
    {
      Level.Add((Thing) new ExplosionPart(pos.x + Rando.Float(-2f, 2f), pos.y + Rando.Float(-2f, 2f), false));
      for (int index = 0; index < 4; ++index)
        Level.Add((Thing) new ExplosionPart(pos.x + Rando.Float(-11f, 11f), pos.y + Rando.Float(-11f, 11f), false));
      if (fromNetwork)
      {
        foreach (PhysicsObject physicsObject in Level.CheckCircleAll<PhysicsObject>(pos, 70f))
        {
          if (physicsObject.isServerForObject)
          {
            physicsObject.sleeping = false;
            physicsObject.vSpeed = -2f;
          }
        }
      }
      SFX.Play("explode");
    }
  }
}
