// Decompiled with JetBrains decompiler
// Type: DuckGame.ATPropExplosion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATPropExplosion : ATShrapnel
  {
    public override void MakeNetEffect(Vec2 pos, bool fromNetwork = false)
    {
      for (int index = 0; index < 1; ++index)
      {
        ExplosionPart explosionPart = new ExplosionPart(pos.x - 8f + Rando.Float(16f), pos.y - 8f + Rando.Float(16f));
        explosionPart.xscale *= 0.7f;
        explosionPart.yscale *= 0.7f;
        Level.Add((Thing) explosionPart);
      }
      SFX.Play("explode");
      Graphics.FlashScreen();
    }
  }
}
