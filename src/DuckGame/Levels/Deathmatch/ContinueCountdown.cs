// Decompiled with JetBrains decompiler
// Type: DuckGame.ContinueCountdown
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ContinueCountdown : Thing
  {
    public StateBinding _timerBinding = new StateBinding(nameof (timer));
    public float timer = 5f;

    public ContinueCountdown(float time = 5f)
      : base()
      => this.timer = time;

    public void UpdateTimer()
    {
      if (this.isServerForObject)
        this.timer -= Maths.IncFrameTimer();
      if ((double) this.timer >= 0.0)
        return;
      this.timer = 0.0f;
    }
  }
}
