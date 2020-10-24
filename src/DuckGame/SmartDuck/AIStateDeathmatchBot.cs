// Decompiled with JetBrains decompiler
// Type: DuckGame.AIStateDeathmatchBot
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class AIStateDeathmatchBot : AIState
  {
    public override AIState Update(Duck duck, DuckAI ai)
    {
      if (Level.current is TeamSelect2 && !duck.pickedHat)
      {
        duck.pickedHat = true;
        this._state.Push((AIState) new AIStatePickHat());
        return (AIState) this;
      }
      if (duck.holdObject == null || !(duck.holdObject is Gun))
      {
        this._state.Push((AIState) new AIStateFindGun());
        return (AIState) this;
      }
      this._state.Push((AIState) new AIStateFindTarget());
      return (AIState) this;
    }
  }
}
