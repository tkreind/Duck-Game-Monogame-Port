// Decompiled with JetBrains decompiler
// Type: DuckGame.Promise
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Threading;

namespace DuckGame
{
  public class Promise
  {
    protected bool _finished;
    protected Delegate _delegate;

    public bool Finished
    {
      get
      {
        lock (this)
          return this._finished;
      }
      protected set
      {
        lock (this)
          this._finished = value;
      }
    }

    protected Promise(Delegate d) => this._delegate = d;

    public Promise(Action action)
      : this((Delegate) action)
    {
    }

    public virtual void Execute()
    {
      this._delegate.Method.Invoke(this._delegate.Target, (object[]) null);
      this.Finished = true;
    }

    public void WaitForComplete(uint waitMs = 13, uint maxAttempts = 0)
    {
      while (!this.Finished)
      {
        Thread.Sleep((int) waitMs);
        if (maxAttempts != 0U && --maxAttempts == 0U)
          break;
      }
    }
  }
}
