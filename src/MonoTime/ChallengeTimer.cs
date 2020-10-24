// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeTimer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class ChallengeTimer : Timer
  {
    private float _time;
    private bool _active;

    public override TimeSpan elapsed
    {
      get
      {
        TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int) ((double) this._time * 1000.0));
        return this._maxTime.TotalSeconds == 0.0 || timeSpan < this._maxTime ? timeSpan : this._maxTime;
      }
    }

    public ChallengeTimer(TimeSpan max = default (TimeSpan))
      : base()
      => this._maxTime = max;

    public void Update()
    {
      if (!this._active)
        return;
      this._time += Maths.IncFrameTimer();
    }

    public override void Start() => this._active = true;

    public override void Stop() => this._active = false;

    public override void Reset()
    {
      this._time = 0.0f;
      this._active = false;
    }

    public override void Restart() => this._time = 0.0f;
  }
}
