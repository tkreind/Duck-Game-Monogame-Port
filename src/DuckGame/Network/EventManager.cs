// Decompiled with JetBrains decompiler
// Type: DuckGame.EventManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class EventManager
  {
    private NetworkConnection _connection;
    private StreamManager _manager;
    private Dictionary<Thing, GhostObject> _ghosts = new Dictionary<Thing, GhostObject>();

    public EventManager(NetworkConnection connection, StreamManager streamManager)
    {
      this._connection = connection;
      this._manager = streamManager;
    }

    public void OnMessage(NetMessage m)
    {
      if (DuckNetwork.levelIndex == (byte) 0 && (int) DuckNetwork.localConnection.loadingStatus != (int) DuckNetwork.levelIndex && !(m is NMAllClientsReady))
        return;
      switch (m)
      {
        case NMEvent _:
          (m as NMEvent).Activate();
          break;
        case NMSynchronizedEvent _:
          (m as NMSynchronizedEvent).Activate();
          break;
      }
      Level.current.OnMessage(m);
    }

    public void Update()
    {
    }
  }
}
