// Decompiled with JetBrains decompiler
// Type: DuckGame.DatablockManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DatablockManager
  {
    private NetworkConnection _connection;
    private StreamManager _manager;

    public DatablockManager(NetworkConnection connection, StreamManager streamManager)
    {
      this._connection = connection;
      this._manager = streamManager;
    }

    public void OnMessage(NetMessage m)
    {
    }

    public void Update()
    {
    }

    public static void BuildLevelInitializerBlock()
    {
    }
  }
}
