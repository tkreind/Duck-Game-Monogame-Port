// Decompiled with JetBrains decompiler
// Type: DuckGame.ActionTimer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ActionTimer : IAutoUpdate
  {
    private float _max;
    private float _inc;
    private float _val;
    private bool _hit;
    private bool _reset = true;

    public bool hit => this._hit;

    public ActionTimer(float inc, float max = 1f, bool reset = true)
    {
      this._inc = inc;
      this._max = max;
      this._reset = reset;
      AutoUpdatables.Add((IAutoUpdate) this);
    }

    public void Update()
    {
      if (this._reset)
        this._hit = false;
      this._val += this._inc;
      if ((double) this._val < (double) this._max)
        return;
      this._val = 0.0f;
      this._hit = true;
    }

    public void Reset() => this._val = 0.0f;

    public void SetToEnd()
    {
      this._val = 0.0f;
      this._hit = true;
    }

    public static implicit operator bool(ActionTimer val) => val.hit;

    public static implicit operator ActionTimer(float val) => new ActionTimer(val);
  }
}
