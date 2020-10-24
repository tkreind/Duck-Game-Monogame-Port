// Decompiled with JetBrains decompiler
// Type: DuckGame.NetMessageStatus
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NetMessageStatus
  {
    public int timesResent;
    public int timesDropped;
    public int framesSinceSent;
    public uint tickOnSend;

    public void Clear()
    {
      this.timesResent = 0;
      this.timesDropped = 0;
      this.framesSinceSent = 0;
      this.tickOnSend = 0U;
    }
  }
}
