﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.AIStatePickHat
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class AIStatePickHat : AIState
  {
    private Thing _target;
    private float _wait = Rando.Float(0.5f);
    private float _wait2 = Rando.Float(0.5f);
    private float _wait3 = Rando.Float(0.5f);
    private float _wait4 = Rando.Float(0.5f);
    private bool _did1;
    private bool _did2;
    private bool _did3;
    private int _moveUp = Rando.Int(0, 8);
    private int _moveLeft = Rando.Int(1, 8);

    public override AIState Update(Duck duck, DuckAI ai)
    {
      if (this._target == null)
      {
        List<Thing> list = Level.current.things[typeof (HatConsole)].ToList<Thing>();
        if (!(AI.Nearest(duck.position, list) is HatConsole hatConsole))
          return (AIState) new AIStateWait(Rando.Float(0.8f, 1f));
        this._target = (Thing) hatConsole;
        ai.SetTarget(hatConsole.position);
        return (AIState) this;
      }
      if ((double) (this._target.position - duck.position).length < 10.0 && duck.grounded)
      {
        this._wait -= 0.016f;
        if ((double) this._wait <= 0.0)
        {
          if (!this._did1 || !(this._target as HatConsole).box._hatSelector.open)
          {
            ai.Press("SHOOT");
            this._did1 = true;
          }
          this._wait2 -= 0.016f;
          if ((double) this._wait2 <= 0.0 && (this._target as HatConsole).box._hatSelector.open)
          {
            if (!this._did2)
            {
              ai.Press("JUMP");
              this._did2 = true;
            }
            this._wait3 -= 0.016f;
            if ((double) this._wait3 <= 0.0)
            {
              this._wait3 = Rando.Float(0.2f);
              if ((double) Rando.Float(1f) > 0.5)
              {
                if (this._moveLeft > 0)
                {
                  ai.Press("LEFT");
                  --this._moveLeft;
                }
                else if (this._moveUp > 0)
                {
                  ai.Press("UP");
                  --this._moveUp;
                }
              }
              else if (this._moveUp > 0)
              {
                ai.Press("UP");
                --this._moveUp;
              }
              else if (this._moveLeft > 0)
              {
                ai.Press("LEFT");
                --this._moveLeft;
              }
              if (this._moveLeft == 0 && this._moveUp == 0)
              {
                if (!this._did3)
                {
                  ai.Press("JUMP");
                  this._did3 = true;
                }
                this._wait4 -= 0.016f;
                if ((double) this._wait4 <= 0.0)
                {
                  ai.Press("QUACK");
                  return (AIState) new AIStateWait(Rando.Float(0.8f, 1f));
                }
              }
            }
          }
        }
      }
      return (AIState) this;
    }
  }
}
