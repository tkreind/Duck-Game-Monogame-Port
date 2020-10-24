// Decompiled with JetBrains decompiler
// Type: DuckGame.CompareHoldablePriorities
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CompareHoldablePriorities : IComparer<Holdable>
  {
    private Duck _duck;

    public CompareHoldablePriorities(Duck d) => this._duck = d;

    public int Compare(Holdable h1, Holdable h2)
    {
      if (h1 == h2)
        return 0;
      if (h1 is CTFPresent)
        return -1;
      if (h2 is CTFPresent)
        return 1;
      if (h1 is TrappedDuck)
        return -1;
      if (h2 is TrappedDuck || h1 is Equipment && this._duck.HasEquipment(h1 as Equipment))
        return 1;
      if (h2 is Equipment && this._duck.HasEquipment(h2 as Equipment))
        return -1;
      return h1.PickupPriority() == h2.PickupPriority() ? ((double) (h1.position - this._duck.position).length - (double) (h2.position - this._duck.position).length < -2.0 ? -1 : 1) : (h1.PickupPriority() < h2.PickupPriority() ? -1 : 1);
    }
  }
}
