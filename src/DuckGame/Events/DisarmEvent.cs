// Decompiled with JetBrains decompiler
// Type: DuckGame.DisarmEvent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DisarmEvent : Event
  {
    public DisarmEvent(Profile dealerVal, Profile victimVal)
      : base(dealerVal, victimVal)
    {
      if (dealerVal != null)
        ++this.dealer.stats.disarms;
      if (victimVal == null)
        return;
      ++victimVal.stats.timesDisarmed;
    }
  }
}
