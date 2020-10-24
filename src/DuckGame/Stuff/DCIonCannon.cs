// Decompiled with JetBrains decompiler
// Type: DuckGame.DCIonCannon
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DCIonCannon : DeathCrateSetting
  {
    public override void Activate(DeathCrate c, bool server = true)
    {
      Level.Add((Thing) new ExplosionPart(c.x, c.y - 2f));
      Level.Add((Thing) new IonCannon(new Vec2(c.x, c.y + 3000f), new Vec2(c.x, c.y - 3000f))
      {
        serverVersion = server
      });
      Graphics.FlashScreen();
      SFX.Play("laserBlast");
      if (!server)
        return;
      Level.Remove((Thing) c);
    }
  }
}
