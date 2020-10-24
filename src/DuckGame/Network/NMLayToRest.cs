// Decompiled with JetBrains decompiler
// Type: DuckGame.NMLayToRest
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMLayToRest : NMEvent
  {
    public byte who;

    public NMLayToRest()
    {
    }

    public NMLayToRest(byte _who) => this.who = _who;

    public Duck GetDuck(int index)
    {
      foreach (Duck duck in Level.current.things[typeof (Duck)])
      {
        if (duck.profile != null && (int) duck.profile.networkIndex == index)
          return duck;
      }
      return (Duck) null;
    }

    public override void Activate()
    {
      Duck duck = this.GetDuck((int) this.who);
      if (duck != null)
      {
        duck.isConversionMessage = true;
        duck.LayToRest((Profile) null);
        duck.isConversionMessage = false;
      }
      base.Activate();
    }
  }
}
