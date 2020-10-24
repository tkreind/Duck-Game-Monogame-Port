// Decompiled with JetBrains decompiler
// Type: DuckGame.SinWave
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class SinWave : IAutoUpdate
  {
    private float _increment;
    private float _wave;
    private float _value;

    public float value
    {
      get => this._value;
      set => this._value = value;
    }

    public float normalized => (float) (((double) this._value + 1.0) / 2.0);

    public SinWave(float inc, float start = 0.0f)
    {
      this._increment = inc;
      this._wave = start;
      AutoUpdatables.Add((IAutoUpdate) this);
    }

    public SinWave()
    {
      this._increment = 0.1f;
      this._wave = 0.0f;
    }

    public void Update()
    {
      this._wave += this._increment;
      this._value = (float) Math.Sin((double) this._wave);
    }

    public static implicit operator float(SinWave val) => val.value;

    public static implicit operator SinWave(float val) => new SinWave(val);
  }
}
