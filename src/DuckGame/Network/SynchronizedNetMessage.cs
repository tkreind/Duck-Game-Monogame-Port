// Decompiled with JetBrains decompiler
// Type: DuckGame.SynchronizedNetMessage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class SynchronizedNetMessage : NetMessage
  {
    public double waitTime;

    public bool Update()
    {
      if (Network.activeNetwork.core.networkTimeFallback)
      {
        this.waitTime -= (double) Maths.IncFrameTimer();
        if (this.waitTime <= 0.0)
          return true;
      }
      else
      {
        if (Math.Abs(this.waitTime - (double) Network.activeNetwork.core.networkTime) > 3000.0)
          this.waitTime = (double) (Network.activeNetwork.core.networkTime + 1000UL);
        if ((double) Network.activeNetwork.core.networkTime >= this.waitTime)
        {
          this.waitTime = -1.0;
          return true;
        }
      }
      return false;
    }

    protected override void OnSerialize()
    {
      if (Network.activeNetwork.core.networkTimeFallback)
      {
        this.waitTime = (double) Network.highestPing / 2.0 + 0.100000001490116;
        if (this.waitTime > 2.0)
          this.waitTime = 2.0;
      }
      else
      {
        ulong num = (ulong) ((double) Network.highestPing * 2.0 * 1000.0);
        if (num > 2000UL)
          num = 2000UL;
        this.waitTime = (double) (Network.activeNetwork.core.networkTime + num);
      }
      base.OnSerialize();
    }
  }
}
