// Decompiled with JetBrains decompiler
// Type: DuckGame.Command
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Command
  {
    private bool _done;
    private bool _inverse;

    public Command Inverse()
    {
      this._inverse = !this._inverse;
      return this;
    }

    public void Do()
    {
      if (this._done)
        return;
      if (this._inverse)
        this.OnUndo();
      else
        this.OnDo();
      this._done = true;
    }

    public void Undo()
    {
      if (!this._done)
        return;
      if (this._inverse)
        this.OnDo();
      else
        this.OnUndo();
      this._done = false;
    }

    public virtual void OnDo()
    {
    }

    public virtual void OnUndo()
    {
    }
  }
}
