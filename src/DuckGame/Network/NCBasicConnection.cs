// Decompiled with JetBrains decompiler
// Type: DuckGame.NCBasicConnection
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Net;

namespace DuckGame
{
  public class NCBasicConnection
  {
    public IPEndPoint connection;
    public string address;
    public int port;
    public NCBasicStatus status;
    public Timer timeout = new Timer();
    public Timer heartbeat = new Timer();
    public int attempts;
    public int beatsReceived;
  }
}
