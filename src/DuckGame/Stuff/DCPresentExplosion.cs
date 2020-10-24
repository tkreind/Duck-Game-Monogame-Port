// Decompiled with JetBrains decompiler
// Type: DuckGame.DCPresentExplosion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class DCPresentExplosion : DeathCrateSetting
  {
    public override void Activate(DeathCrate c, bool server = true)
    {
      float x = c.x;
      float ypos = c.y - 2f;
      Level.Add((Thing) new ExplosionPart(x, ypos));
      int num1 = 6;
      if (Graphics.effectsLevel < 2)
        num1 = 3;
      for (int index = 0; index < num1; ++index)
      {
        float deg = (float) index * 60f + Rando.Float(-10f, 10f);
        float num2 = Rando.Float(12f, 20f);
        Level.Add((Thing) new ExplosionPart(x + (float) Math.Cos((double) Maths.DegToRad(deg)) * num2, ypos - (float) Math.Sin((double) Maths.DegToRad(deg)) * num2));
      }
      if (server)
      {
        for (int index = 0; index < 8; ++index)
        {
          Present present = new Present(c.x, c.y);
          float num2 = (float) index / 7f;
          present.hSpeed = (float) ((double) num2 * 30.0 - 15.0) * Rando.Float(0.5f, 1f);
          present.vSpeed = Rando.Float(-3f, -11f);
          Level.Add((Thing) present);
        }
        for (int index = 0; index < 4; ++index)
        {
          Flower flower = new Flower(c.x, c.y);
          float num2 = (float) index / 3f;
          flower.hSpeed = (float) ((double) num2 * 20.0 - 10.0) * Rando.Float(0.5f, 1f);
          flower.vSpeed = Rando.Float(-3f, -11f);
          Level.Add((Thing) flower);
        }
        Level.Remove((Thing) c);
      }
      Graphics.FlashScreen();
      SFX.Play("harp");
    }
  }
}
