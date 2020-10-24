// Decompiled with JetBrains decompiler
// Type: DuckGame.NCBasic
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DuckGame
{
  public class NCBasic : NCNetworkImplementation
  {
    private const long _specialNumber = 2449832521355936907;
    private UdpClient _socket;
    private byte[] _receiveBuffer = new byte[4096];
    private string _serverIdentifier = "";
    private IPEndPoint _serverEndPoint;
    private int _port;
    private bool _isAServer;
    private Queue<NCBasicPacket> _threadPackets = new Queue<NCBasicPacket>();
    private List<NCBasicConnection> _basicConnections = new List<NCBasicConnection>();
    private Thread _packetThread;
    public static int bytesThisFrame = 0;
    public static int headerBytes;
    public static int ghostBytes;
    public static int ackBytes;
    public static int localPort = 1338;
    private volatile bool _quit;
    private string _localName = "";

    public NCBasic(Network c, int networkIndex)
      : base(c, networkIndex)
    {
    }

    public override NCError OnSendPacket(byte[] data, int length, object connection)
    {
      NCBasicConnection ncBasicConnection = connection as NCBasicConnection;
      this._socket.Send(data, length, ncBasicConnection.connection);
      NCBasic.bytesThisFrame += length;
      return (NCError) null;
    }

    protected override object GetConnectionObject(string identifier)
    {
      string[] strArray = identifier.Split(':');
      if (strArray.Length != 2)
        return (object) null;
      string ip = strArray[0];
      return this.MakeConnection(Convert.ToInt32(strArray[1]), ip);
    }

    public override NCError OnHostServer(
      string identifier,
      int port,
      NetworkLobbyType lobbyType,
      int maxConnections)
    {
      if (this._socket != null)
        return new NCError("server is already started...", NCErrorType.Error);
      this._serverIdentifier = identifier;
      this._socket = new UdpClient();
      this._socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
      this._socket.Client.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
      this._socket.AllowNatTraversal(true);
      this._port = port;
      this._isAServer = true;
      this.StartServerThread();
      this.StartPacketThread();
      return new NCError("server started on port " + (object) port + ".", NCErrorType.Success);
    }

    public override NCError OnJoinServer(string identifier, int port, string ip)
    {
      if (this._socket != null)
        return new NCError("client is already started...", NCErrorType.Error);
      this._serverIdentifier = identifier;
      if (this._networkIndex == 0)
        NCBasic.localPort = 1337;
      else if (this._networkIndex == 1)
        NCBasic.localPort = 1338;
      else if (this._networkIndex == 2)
        NCBasic.localPort = 1339;
      else if (this._networkIndex == 3)
        NCBasic.localPort = 1340;
      this._socket = new UdpClient();
      this._socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
      this._socket.Client.Bind((EndPoint) new IPEndPoint(IPAddress.Any, NCBasic.localPort));
      this._socket.AllowNatTraversal(true);
      this._port = port;
      this.MakeConnection(port, ip);
      this._isAServer = false;
      this.StartClientThread();
      this.StartPacketThread();
      return (NCError) null;
    }

    public object MakeConnection(int port, string ip)
    {
      lock (this._basicConnections)
      {
        NCBasicConnection ncBasicConnection1 = this._basicConnections.FirstOrDefault<NCBasicConnection>((Func<NCBasicConnection, bool>) (x => x.address == ip + ":" + (object) port));
        if (ncBasicConnection1 != null)
          return (object) ncBasicConnection1;
        IPEndPoint ipEndPoint = !(ip == "localhost") ? new IPEndPoint(IPAddress.Parse(ip), port) : new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        NCBasicConnection ncBasicConnection2 = new NCBasicConnection()
        {
          connection = ipEndPoint,
          address = ipEndPoint.ToString(),
          status = NCBasicStatus.TryingToConnect,
          port = port
        };
        this._basicConnections.Add(ncBasicConnection2);
        this._pendingMessages.Enqueue(new NCError("client connecting to " + ip + ":" + (object) port + ".", NCErrorType.Message));
        return (object) ncBasicConnection2;
      }
    }

    public void StartPacketThread()
    {
      this._quit = false;
      this._packetThread = new Thread(new ThreadStart(this.SpinPacketThread));
      this._packetThread.CurrentCulture = CultureInfo.InvariantCulture;
      this._packetThread.Priority = ThreadPriority.Normal;
      this._packetThread.IsBackground = true;
      this._packetThread.Start();
    }

    public void SpinPacketThread()
    {
      while (!this._quit)
      {
        try
        {
          IPEndPoint remoteEP = this._serverEndPoint ?? new IPEndPoint(IPAddress.Any, this._port);
          byte[] numArray = this._socket.Receive(ref remoteEP);
          if (numArray != null)
          {
            lock (this._threadPackets)
              this._threadPackets.Enqueue(new NCBasicPacket()
              {
                data = numArray,
                sender = remoteEP
              });
          }
          Thread.Sleep(2);
        }
        catch
        {
        }
      }
    }

    public override string GetConnectionIdentifier(object connection) => (connection as NCBasicConnection).connection.ToString();

    public override string GetConnectionName(object connection) => (connection as NCBasicConnection).connection.ToString();

    protected override string OnGetLocalName()
    {
      if (this._localName == "")
        this._localName = "test " + Rando.Int(235645).ToString();
      return this._localName;
    }

    protected override NCError OnSpinServerThread()
    {
      if (this._socket == null)
        return new NCError("connection was lost.", NCErrorType.CriticalError);
      Queue<NCBasicPacket> ncBasicPacketQueue = (Queue<NCBasicPacket>) null;
      lock (this._threadPackets)
      {
        ncBasicPacketQueue = new Queue<NCBasicPacket>((IEnumerable<NCBasicPacket>) this._threadPackets);
        this._threadPackets.Clear();
      }
      foreach (NCBasicPacket ncBasicPacket in ncBasicPacketQueue)
      {
        IPEndPoint sender = ncBasicPacket.sender;
        byte[] data = ncBasicPacket.data;
        string address = sender.ToString();
        lock (this._basicConnections)
        {
          NCBasicConnection ncBasicConnection1 = this._basicConnections.FirstOrDefault<NCBasicConnection>((Func<NCBasicConnection, bool>) (x => x.address == address));
          if (ncBasicConnection1 == null || ncBasicConnection1.status == NCBasicStatus.Disconnected)
          {
            if (data.Length > 8)
            {
              BitBuffer bitBuffer1 = new BitBuffer(data);
              if (bitBuffer1.ReadLong() == 2449832521355936907L)
              {
                if (bitBuffer1.ReadString() == this._serverIdentifier)
                {
                  NCBasicConnection ncBasicConnection2 = new NCBasicConnection()
                  {
                    connection = sender,
                    address = address,
                    status = NCBasicStatus.Connecting
                  };
                  ncBasicConnection2.port = sender.Port;
                  this._basicConnections.Add(ncBasicConnection2);
                  this._pendingMessages.Enqueue(new NCError("connection attempt from " + ncBasicConnection2.address, NCErrorType.Message));
                  ncBasicConnection2.heartbeat.Restart();
                  ncBasicConnection2.timeout.Restart();
                  BitBuffer bitBuffer2 = new BitBuffer();
                  bitBuffer2.Write(2449832521355936907L);
                  this._socket.Send(bitBuffer2.buffer, bitBuffer2.lengthInBytes, ncBasicConnection2.connection);
                }
              }
            }
          }
          else if (ncBasicConnection1.status == NCBasicStatus.WaitingForAck || ncBasicConnection1.status == NCBasicStatus.TryingToConnect)
          {
            if (data.Length >= 8)
            {
              if (new BitBuffer(data).ReadLong() == 2449832521355936907L)
              {
                ncBasicConnection1.status = NCBasicStatus.Connecting;
                ncBasicConnection1.timeout.Restart();
                ncBasicConnection1.heartbeat.Restart();
                this._pendingMessages.Enqueue(new NCError("received first acknowledgement...", NCErrorType.Success));
                BitBuffer bitBuffer = new BitBuffer();
                bitBuffer.Write(2449832521355936907L);
                this._socket.Send(bitBuffer.buffer, bitBuffer.lengthInBytes, ncBasicConnection1.connection);
              }
            }
          }
          else
          {
            BitBuffer bitBuffer = new BitBuffer(data);
            if (ncBasicConnection1.status != NCBasicStatus.Connected)
            {
              if (ncBasicConnection1.status != NCBasicStatus.Connecting)
                continue;
            }
            bool flag = false;
            if (data.Length >= 8)
            {
              if (bitBuffer.ReadLong() == 2449832521355936907L)
              {
                ncBasicConnection1.timeout.Restart();
                ++ncBasicConnection1.beatsReceived;
                flag = true;
              }
              else
                bitBuffer.SeekToStart();
            }
            if (!flag)
              this.OnPacket(data, (object) ncBasicConnection1);
          }
        }
      }
      List<NCBasicConnection> ncBasicConnectionList = new List<NCBasicConnection>();
      for (int index = 0; index < this._basicConnections.Count; ++index)
      {
        NCBasicConnection basicConnection = this._basicConnections[index];
        if (basicConnection.status == NCBasicStatus.TryingToConnect)
        {
          if (basicConnection.attempts > 5)
          {
            ncBasicConnectionList.Add(basicConnection);
            this._pendingMessages.Enqueue(new NCError("could not connect to server.", NCErrorType.CriticalError));
          }
          BitBuffer bitBuffer = new BitBuffer();
          bitBuffer.Write(2449832521355936907L);
          bitBuffer.Write(this._serverIdentifier);
          this._socket.Send(bitBuffer.buffer, bitBuffer.lengthInBytes, basicConnection.connection);
          ++basicConnection.attempts;
          basicConnection.timeout.Restart();
          basicConnection.status = NCBasicStatus.WaitingForAck;
        }
        else if (basicConnection.status == NCBasicStatus.WaitingForAck && basicConnection.timeout.elapsed.TotalSeconds > 2.0)
        {
          basicConnection.status = NCBasicStatus.TryingToConnect;
          this._pendingMessages.Enqueue(new NCError("resending connection message to server.", NCErrorType.Message));
        }
        float num = 1f;
        if (basicConnection.status == NCBasicStatus.Connecting)
          num = 0.05f;
        if (basicConnection.status == NCBasicStatus.Connecting || basicConnection.status == NCBasicStatus.Connected)
        {
          if (basicConnection.heartbeat.elapsed.TotalSeconds >= (double) num)
          {
            BitBuffer bitBuffer = new BitBuffer();
            bitBuffer.Write(2449832521355936907L);
            this._socket.Send(bitBuffer.buffer, bitBuffer.lengthInBytes, basicConnection.connection);
            basicConnection.heartbeat.Restart();
          }
          if (basicConnection.status == NCBasicStatus.Connecting && basicConnection.beatsReceived > 0)
          {
            this._pendingMessages.Enqueue(new NCError("connection to " + basicConnection.address + " succeeded!", NCErrorType.Success));
            basicConnection.status = NCBasicStatus.Connected;
            bool host = false;
            if (!this._isAServer && this.connections.Count == 1 && this.connections[0].identifier == "waiting")
              host = true;
            this.AttemptConnection((object) basicConnection, host);
          }
        }
      }
      foreach (NCBasicConnection ncBasicConnection in ncBasicConnectionList)
        this._basicConnections.Remove(ncBasicConnection);
      return (NCError) null;
    }

    protected override NCError OnSpinClientThread() => this.OnSpinServerThread();

    protected override void KillConnection()
    {
      this._packetThread = (Thread) null;
      this._quit = true;
      this._basicConnections.Clear();
      if (this._socket != null)
        this._socket.Close();
      base.KillConnection();
    }

    protected override void Disconnect(NetworkConnection c)
    {
      if (c != null)
      {
        NCBasicConnection ncBasicConnection = this._basicConnections.FirstOrDefault<NCBasicConnection>((Func<NCBasicConnection, bool>) (x => x.address == c.identifier));
        if (ncBasicConnection != null)
          this._basicConnections.Remove(ncBasicConnection);
      }
      base.Disconnect(c);
    }

    public override void Terminate()
    {
      this._packetThread = (Thread) null;
      this._quit = true;
      base.Terminate();
    }
  }
}
