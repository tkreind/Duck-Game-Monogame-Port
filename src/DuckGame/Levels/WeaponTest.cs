﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.WeaponTest
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WeaponTest : DuckGameTestArea
  {
    private System.Type[] _types;

    public WeaponTest(Editor e, params System.Type[] types)
      : base(e, "Content\\levels\\weaponTest.lev")
      => this._types = types;

    public override void Initialize()
    {
      base.Initialize();
      int num1 = 0;
      int num2 = 240;
      foreach (System.Type type in this._types)
      {
        Thing thing1 = Thing.Instantiate(type);
        thing1.x = (float) (num2 + num1 * 22);
        thing1.y = 200f;
        Level.Add(thing1);
        Thing thing2 = Thing.Instantiate(type);
        thing2.x = (float) (num2 + num1 * 22 + 8);
        thing2.y = 200f;
        Level.Add(thing2);
        ++num1;
      }
      Duck duck1 = new Duck(210f, 200f, Profiles.DefaultPlayer1);
      Level.Add((Thing) duck1);
      (Level.current as DeathmatchLevel).followCam.Add((Thing) duck1);
      Duck duck2 = new Duck(400f, 200f, Profiles.DefaultPlayer2);
      Level.Add((Thing) duck2);
      (Level.current as DeathmatchLevel).followCam.Add((Thing) duck2);
      Level.Add((Thing) new PhysicsChain(300f, 100f));
      Level.Add((Thing) new PhysicsRope(350f, 100f));
    }
  }
}
