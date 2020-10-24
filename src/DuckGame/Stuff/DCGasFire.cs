// Decompiled with JetBrains decompiler
// Type: DuckGame.DCGasFire
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DCGasFire : DeathCrateSetting
  {
    public override void Activate(DeathCrate c, bool server = true)
    {
      Level.Add((Thing) new ExplosionPart(c.x, c.y - 2f));
      if (server)
      {
        YellowBarrel yellowBarrel = new YellowBarrel(c.x, c.y);
        yellowBarrel.vSpeed = -3f;
        Level.Add((Thing) yellowBarrel);
        Grenade grenade1 = new Grenade(c.x, c.y);
        grenade1.PressAction();
        grenade1.hSpeed = -1f;
        grenade1.vSpeed = -2f;
        Level.Add((Thing) grenade1);
        Grenade grenade2 = new Grenade(c.x, c.y);
        grenade2.PressAction();
        grenade2.hSpeed = 1f;
        grenade2.vSpeed = -2f;
        Level.Add((Thing) grenade2);
        Level.Remove((Thing) c);
      }
      Level.Add((Thing) new MusketSmoke(c.x, c.y));
    }
  }
}
