// Decompiled with JetBrains decompiler
// Type: DuckGame.AIState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class AIState
  {
    protected Stack<AIState> _state = new Stack<AIState>();

    public virtual AIState DoUpdate(Duck duck, DuckAI ai)
    {
      if (this._state.Count <= 0)
        return this.Update(duck, ai);
      AIState aiState = this._state.Peek().DoUpdate(duck, ai);
      if (aiState == null)
        this._state.Pop();
      else if (aiState != this._state.Peek())
      {
        this._state.Pop();
        this._state.Push(aiState);
      }
      return this;
    }

    public virtual AIState Update(Duck duck, DuckAI ai) => this;
  }
}
