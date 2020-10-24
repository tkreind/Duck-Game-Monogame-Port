// Decompiled with JetBrains decompiler
// Type: DuckGame.NetStepActivator
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NetStepActivator
  {
    private NetStepActivator.Function _function;
    private int _index;

    public NetStepActivator(NetStepActivator.Function del) => this._function = del;

    public int index
    {
      get => this._index;
      set
      {
        this._index = value;
        if (this._index <= 3)
          return;
        this._index = 0;
      }
    }

    public void Activate()
    {
      if (this._function != null)
        this._function();
      this.Step();
    }

    public void Step() => ++this.index;

    public delegate void Function();
  }
}
