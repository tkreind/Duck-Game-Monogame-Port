// Decompiled with JetBrains decompiler
// Type: DuckGame.AIStateWait
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class AIStateWait : AIState
  {
    private float _wait;

    public AIStateWait(float wait) => this._wait = wait;

    public override AIState Update(Duck duck, DuckAI ai)
    {
      this._wait -= 0.016f;
      return (double) this._wait <= 0.0 ? (AIState) null : (AIState) this;
    }
  }
}
