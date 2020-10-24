// Decompiled with JetBrains decompiler
// Type: DuckGame.NMConnect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [FixedNetworkID(43)]
  public class NMConnect : NMNetworkCoreMessage
  {
    public string version;
    public NetIndex4 connectsReceived;
    public NetIndex4 remoteSession;
    public string modHash;

    public NMConnect()
    {
    }

    public NMConnect(byte received, NetIndex4 s, string v, string mH)
    {
      this.version = v;
      this.connectsReceived = (NetIndex4) (int) received;
      this.remoteSession = s;
      this.modHash = mH;
    }
  }
}
