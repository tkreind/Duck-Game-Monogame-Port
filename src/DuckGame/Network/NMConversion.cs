// Decompiled with JetBrains decompiler
// Type: DuckGame.NMConversion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMConversion : NMEvent
  {
    public byte who;
    public byte to;

    public NMConversion()
    {
    }

    public NMConversion(byte _who, byte _to)
    {
      this.who = _who;
      this.to = _to;
    }

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
      Duck duck1 = this.GetDuck((int) this.who);
      Duck duck2 = this.GetDuck((int) this.to);
      duck1.isConversionMessage = true;
      duck1.ConvertDuck(duck2);
      duck1.isConversionMessage = false;
      base.Activate();
    }
  }
}
