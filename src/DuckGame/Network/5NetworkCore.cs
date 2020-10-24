// Decompiled with JetBrains decompiler
// Type: DuckGame.NMHeartbeat
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [FixedNetworkID(10)]
  public class NMHeartbeat : NMNetworkCoreMessage
  {
    public NetIndex4 remoteSession;

    public NMHeartbeat()
    {
    }

    public NMHeartbeat(NetIndex4 s) => this.remoteSession = s;
  }
}
