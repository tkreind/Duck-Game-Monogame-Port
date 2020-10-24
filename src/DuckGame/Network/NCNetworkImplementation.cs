// Decompiled with JetBrains decompiler
// Type: DuckGame.NCNetworkImplementation
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
  public abstract class NCNetworkImplementation
  {
    private Network _core;
    public bool _connectionsDirty = true;
    private List<NetworkConnection> _connectionsInternal;
    private List<NetworkConnection> _connectionsInternalAll;
    public bool firstPrediction = true;
    private List<NetworkConnection> _connectionHistory = new List<NetworkConnection>();
    private Queue<NetworkPacket> _pendingPackets = new Queue<NetworkPacket>();
    private Thread _networkThread;
    private Thread _timeThread;
    private bool _isServer = true;
    private bool _isServerP2P = true;
    protected Queue<NCError> _pendingMessages = new Queue<NCError>();
    private float _quality = 1f;
    private float _spikeTime;
    private float _minimumLatencyInternal;
    private float _jitterInternal;
    private float _dropPercent;
    private float _failurePercent;
    private float _traffic;
    private float _spikes;
    private int _failureCounter;
    private int _timeForFailure;
    private GhostManager _ghostManager;
    private List<NCDelayPacket> _delayPackets = new List<NCDelayPacket>();
    private NetGraph _netGraph = new NetGraph();
    protected int _networkIndex;
    private readonly object _threadLock = new object();
    private volatile bool _killThread;
    private volatile Queue<NCError> _threadPendingMessages = new Queue<NCError>();
    protected ulong _networkTime;
    protected Timer _networkTimeTimer = new Timer();
    public bool networkTimeFallback = true;
    private static Timer _staticTimer;
    private static ulong _timeGotten;
    private static Socket _timeSocket;
    private bool _lastConnectionClosed;
    private DuckNetErrorInfo _lastDisconnectError;
    private int packetNum;
    private List<NetworkPacket> delayedPackets = new List<NetworkPacket>();
    public int frame;
    private bool _breakDisconnectLoop;
    private int _breakDisconnectTries = 80;

    private void RefreshConnections()
    {
      lock (this._connectionHistory)
      {
        this._connectionsInternal = new List<NetworkConnection>();
        this._connectionsInternalAll = new List<NetworkConnection>();
        foreach (NetworkConnection networkConnection in this._connectionHistory)
        {
          if (networkConnection.status != ConnectionStatus.Disconnected)
            this._connectionsInternal.Add(networkConnection);
          this._connectionsInternalAll.Add(networkConnection);
        }
      }
      this._connectionsDirty = false;
    }

    public List<NetworkConnection> connections
    {
      get
      {
        if (this._connectionsDirty)
          this.RefreshConnections();
        return this._connectionsInternal;
      }
    }

    public List<NetworkConnection> allConnections
    {
      get
      {
        if (this._connectionsDirty)
          this.RefreshConnections();
        return this._connectionsInternalAll;
      }
    }

    public List<NetworkConnection> sessionConnections
    {
      get
      {
        List<NetworkConnection> networkConnectionList = new List<NetworkConnection>((IEnumerable<NetworkConnection>) this.connections);
        if (Network.isServer && DuckNetwork.localConnection.status != ConnectionStatus.Disconnected)
          networkConnectionList.Add(DuckNetwork.localConnection);
        return networkConnectionList;
      }
    }

    public bool isServer
    {
      get => this._isServer;
      set => this._isServer = value;
    }

    public bool isServerP2P
    {
      get => this._isServerP2P;
      set => this._isServerP2P = value;
    }

    public bool isActive => this._networkThread != null;

    public float quality
    {
      get => this._quality;
      set => this._quality = value;
    }

    private float _minimumLatency
    {
      get => (double) this._spikeTime > 0.0 ? this._minimumLatencyInternal + this._minimumLatencyInternal * Rando.Float(1f, 4f) * this._spikes : this._minimumLatencyInternal;
      set => this._minimumLatencyInternal = value;
    }

    public float minimumLatency
    {
      get => this._minimumLatency;
      set => this._minimumLatency = value;
    }

    private float _jitter
    {
      get => (double) this._spikeTime > 0.0 ? this._jitterInternal + this._jitterInternal * Rando.Float(1f, 4f) * this._spikes : this._jitterInternal;
      set => this._jitterInternal = value;
    }

    public float jitter
    {
      get => this._jitter;
      set => this._jitter = value;
    }

    public float dropPercent
    {
      get => this._dropPercent;
      set => this._dropPercent = value;
    }

    public float failurePercent
    {
      get => this._failurePercent;
      set => this._failurePercent = value;
    }

    public float traffic
    {
      get => this._traffic;
      set => this._traffic = value;
    }

    public float spikes
    {
      get => this._spikes;
      set => this._spikes = value;
    }

    public GhostManager ghostManager => this._ghostManager;

    public NetGraph netGraph => this._netGraph;

    public NCNetworkImplementation(Network core, int networkIndex)
    {
      this._core = core;
      this._networkIndex = networkIndex;
      this._ghostManager = new GhostManager();
    }

    public void HostServer(
      string identifier,
      int port,
      NetworkLobbyType lobbyType,
      int maxConnections)
    {
      this._pendingPackets.Clear();
      NCError ncError = this.OnHostServer(identifier, port, lobbyType, maxConnections);
      if (ncError == null)
        return;
      DevConsole.Log(ncError.text, ncError.color, index: this._networkIndex);
    }

    public abstract NCError OnHostServer(
      string identifier,
      int port,
      NetworkLobbyType lobbyType,
      int maxConnections);

    public void JoinServer(string identifier, int port, string ip)
    {
      this._pendingPackets.Clear();
      NCError ncError = this.OnJoinServer(identifier, port, ip);
      this.AttemptConnection("host");
      if (ncError == null)
        return;
      DevConsole.Log(ncError.text, ncError.color, index: this._networkIndex);
    }

    public abstract NCError OnJoinServer(string identifier, int port, string ip);

    protected void StartServerThread()
    {
      this._isServer = true;
      this._isServerP2P = true;
      this._killThread = false;
      this._networkThread = new Thread(new ThreadStart(this.SpinThread));
      this._networkThread.CurrentCulture = CultureInfo.InvariantCulture;
      this._networkThread.Priority = ThreadPriority.Normal;
      this._networkThread.IsBackground = true;
      this._networkThread.Start();
    }

    protected void StartClientThread()
    {
      this._isServer = false;
      this._isServerP2P = false;
      this._killThread = false;
      this._networkThread = new Thread(new ThreadStart(this.SpinThread));
      this._networkThread.CurrentCulture = CultureInfo.InvariantCulture;
      this._networkThread.Priority = ThreadPriority.Normal;
      this._networkThread.IsBackground = true;
      this._networkThread.Start();
    }

    private static uint SwapEndianness(ulong x) => (uint) ((ulong) ((((long) x & (long) byte.MaxValue) << 24) + (((long) x & 65280L) << 8)) + ((x & 16711680UL) >> 8) + ((x & 4278190080UL) >> 24));

    public ulong networkTime => this._networkTime + (ulong) this._networkTimeTimer.elapsed.TotalMilliseconds;

    protected void NetworkTimeThread()
    {
      try
      {
        this.networkTimeFallback = false;
        if (NCNetworkImplementation._timeSocket != null)
        {
          while (!this._killThread)
          {
            this._networkTime = NCNetworkImplementation._timeGotten;
            this._networkTimeTimer = NCNetworkImplementation._staticTimer;
            Thread.Sleep(200);
          }
        }
        else
        {
          while (!this._killThread)
          {
            byte[] buffer = new byte[48];
            buffer[0] = (byte) 27;
            IPEndPoint ipEndPoint = new IPEndPoint(Dns.GetHostEntry("pool.ntp.org").AddressList[0], 123);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            NCNetworkImplementation._timeSocket = socket;
            NCNetworkImplementation._staticTimer = this._networkTimeTimer;
            socket.Connect((EndPoint) ipEndPoint);
            if (socket.Connected)
            {
              try
              {
                socket.Send(buffer);
                socket.ReceiveTimeout = 3000;
                socket.Receive(buffer);
                ulong num1 = (ulong) ((long) buffer[40] << 24 | (long) buffer[41] << 16 | (long) buffer[42] << 8) | (ulong) buffer[43];
                ulong num2 = (ulong) ((long) buffer[44] << 24 | (long) buffer[45] << 16 | (long) buffer[46] << 8) | (ulong) buffer[47];
                this._networkTimeTimer.Restart();
                this._networkTime = num1 * 1000UL + num2 * 1000UL / 4294967296UL;
                NCNetworkImplementation._timeGotten = this._networkTime;
              }
              catch (Exception ex)
              {
              }
              socket.Close();
              Thread.Sleep(15000);
            }
            else
              break;
          }
          this.networkTimeFallback = true;
        }
      }
      catch (Exception ex)
      {
        this.networkTimeFallback = true;
      }
    }

    protected void SpinThread()
    {
      while (!this._killThread)
      {
        Thread.Sleep(8);
        lock (this._threadLock)
        {
          NCError ncError = !this._isServer ? this.OnSpinClientThread() : this.OnSpinServerThread();
          lock (this._threadPendingMessages)
          {
            foreach (NCError pendingMessage in this._pendingMessages)
              this._threadPendingMessages.Enqueue(pendingMessage);
            this._pendingMessages.Clear();
          }
          if (ncError != null)
          {
            DevConsole.Log(ncError.text, ncError.color, index: this._networkIndex);
            if (ncError.type == NCErrorType.CriticalError)
              return;
          }
        }
      }
      this._pendingMessages.Enqueue(new NCError("|DGBLUE|NETCORE |DGRED|Network thread ended", NCErrorType.Debug));
      this._killThread = false;
    }

    protected abstract NCError OnSpinServerThread();

    protected abstract NCError OnSpinClientThread();

    public NetworkConnection AttemptConnection(object context, bool host = false)
    {
      NetworkConnection orAddConnection = this.GetOrAddConnection(context);
      if (orAddConnection.status != ConnectionStatus.Disconnected)
      {
        DevConsole.Log(DCSection.NetCore, "|DGYELLOW|" + orAddConnection.name + " already connected.", this._networkIndex);
        return orAddConnection;
      }
      lock (this._connectionHistory)
      {
        this._connectionHistory.RemoveAll((Predicate<NetworkConnection>) (x => x.identifier == "waiting"));
        this._connectionsDirty = true;
      }
      orAddConnection.Reset();
      orAddConnection.isHost = host;
      orAddConnection.AttemptConnection();
      if (host)
        DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Attempting connection with " + orAddConnection.name + " (HOST)(" + (object) orAddConnection.sessionIndex + ")", this._networkIndex);
      else
        DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Attempting connection with " + orAddConnection.name + " (" + (object) orAddConnection.sessionIndex + ")", this._networkIndex);
      return orAddConnection;
    }

    protected NetworkConnection GetWaitingConnection()
    {
      NetworkConnection networkConnection = this.allConnections.FirstOrDefault<NetworkConnection>((Func<NetworkConnection, bool>) (x => x.identifier == "waiting"));
      if (networkConnection == null)
      {
        networkConnection = new NetworkConnection((object) null, "waiting");
        lock (this._connectionHistory)
        {
          this._connectionHistory.Add(networkConnection);
          this._connectionsDirty = true;
        }
      }
      return networkConnection;
    }

    public NetworkConnection AttemptConnection(string identifier)
    {
      if (identifier == "host" && this.connections.Count == 0)
      {
        NetworkConnection waitingConnection = this.GetWaitingConnection();
        waitingConnection.WaitForInformation();
        return waitingConnection;
      }
      object connectionObject = this.GetConnectionObject(identifier);
      if (connectionObject != null)
        return this.AttemptConnection(connectionObject);
      DevConsole.Log(DCSection.NetCore, "|DGRED|Connection attempt with" + identifier + "failed (INVALID)", this._networkIndex);
      return (NetworkConnection) null;
    }

    protected virtual object GetConnectionObject(string identifier) => (object) null;

    protected void OnAttemptConnection(object context)
    {
    }

    public void SendPeerInfo(object context) => this.OnSendPeerInfo(context);

    public virtual NCError OnSendPeerInfo(object context) => (NCError) null;

    public void PushNewConnection(NetworkConnection c)
    {
      lock (this._connectionHistory)
      {
        this._connectionHistory.Add(c);
        this._connectionsDirty = true;
      }
    }

    protected NetworkConnection GetOrAddConnection(object context)
    {
      string id = this.GetConnectionIdentifier(context);
      NetworkConnection c = this.allConnections.FirstOrDefault<NetworkConnection>((Func<NetworkConnection, bool>) (x => x.identifier == id));
      if (c == null)
      {
        c = new NetworkConnection(context);
        this.PushNewConnection(c);
      }
      return c;
    }

    protected NetworkConnection GetConnection(object context)
    {
      string id = this.GetConnectionIdentifier(context);
      return this.allConnections.FirstOrDefault<NetworkConnection>((Func<NetworkConnection, bool>) (x => x.identifier == id));
    }

    public void OnDisconnect(NetworkConnection connection, DuckNetErrorInfo error, bool kicked = false)
    {
      string reason = "NO REASON GIVEN";
      if (error != null)
      {
        string[] strArray = error.message.Split('\n');
        if (strArray.Length > 0)
          reason = strArray[0];
      }
      if (connection.data == null)
        DevConsole.Log(DCSection.NetCore, "|DGRED|Host (LOCAL) disconnected.", this._networkIndex);
      else
        DevConsole.Log(DCSection.NetCore, "|DGRED|" + connection.name + " Disconnected. (" + reason + "|DGRED|)(" + (object) connection.remoteSessionIndex + ")", this._networkIndex);
      this._lastDisconnectError = error;
      this.Disconnect(connection);
      Level.current.OnDisconnect(connection);
      DuckNetwork.OnDisconnect(connection, reason, kicked);
      connection.Reset();
      if (this.sessionConnections.Count != 0)
        return;
      this._lastConnectionClosed = true;
    }

    protected virtual void Disconnect(NetworkConnection c)
    {
    }

    public void OnSessionEnded()
    {
      this._delayPackets.Clear();
      if (this._networkThread != null && this._networkThread.IsAlive)
      {
        this._killThread = true;
        for (int index = 0; this._killThread && index < 20; ++index)
          Thread.Sleep(100);
      }
      if (this._killThread)
      {
        this._networkThread.Abort();
        if (this._timeThread != null)
          this._timeThread.Abort();
      }
      this._networkThread = (Thread) null;
      this.KillConnection();
      this._pendingPackets.Clear();
      this._pendingMessages.Clear();
      this._delayPackets.Clear();
      this._ghostManager = new GhostManager();
      Network.activeNetwork.Reset();
      Network.isServer = true;
      DevConsole.Log(DCSection.NetCore, "|DGRED|Networking session has ended.", this._networkIndex);
      this._breakDisconnectLoop = true;
      DuckNetErrorInfo error = DuckNetwork.error ?? this._lastDisconnectError;
      DuckNetwork.OnSessionEnded();
      if (Level.current != null)
        Level.current.OnSessionEnded(error);
      this._lastConnectionClosed = false;
      this._lastDisconnectError = (DuckNetErrorInfo) null;
    }

    public void OnConnection(NetworkConnection connection)
    {
      DevConsole.Log(DCSection.NetCore, "|LIME|Connection with " + connection.name + " established! (" + (object) connection.remoteSessionIndex + ")", this._networkIndex);
      DuckNetwork.OnConnection(connection);
    }

    protected void OnPacket(byte[] data, object context)
    {
      NetworkConnection connection = this.GetConnection(context);
      if (connection == null)
      {
        this._pendingMessages.Enqueue(new NCError("|DGBLUE|NETCORE |DGRED|Packet received from unknown connection.", NCErrorType.Debug));
      }
      else
      {
        lock (this._pendingPackets)
          this._pendingPackets.Enqueue(new NetworkPacket(new BitBuffer(data), connection, (byte) 0));
      }
    }

    public void SendPacket(NetworkPacket packet, NetworkConnection connection)
    {
      if (connection == null)
      {
        DevConsole.Log(DCSection.NetCore, "|DGRED|Trying to send packet with no connection.", this._core.networkIndex);
      }
      else
      {
        if (NetworkDebugger.enabled)
          NetworkDebugger.LogSend(NetworkDebugger.GetID(this._networkIndex), connection.identifier);
        UIMatchmakingBox.pulseLocal = true;
        BitBuffer bitBuffer = new BitBuffer();
        bitBuffer.WriteBits((object) (int) connection.sessionIndex, 4);
        ++NCBasic.headerBytes;
        if (connection.acksToSend.Count > 0)
        {
          bitBuffer.Write(true);
          bitBuffer.Write((byte) connection.acksToSend.Count);
          ++NCBasic.ackBytes;
          foreach (byte val in connection.acksToSend)
          {
            bitBuffer.Write(val);
            ++NCBasic.ackBytes;
          }
          connection.acksToSend.Clear();
        }
        else
          bitBuffer.Write(false);
        if (packet != null)
        {
          connection.packetsSent.Add(packet);
          bitBuffer.Write(true);
          bitBuffer.Write(packet.order);
          bitBuffer.Write(packet.data, false);
        }
        else
          bitBuffer.Write(false);
        connection.sentThisFrame = true;
        int lengthInBytes = bitBuffer.lengthInBytes;
        if ((double) this._minimumLatency > 0.0 || (double) this._jitter > 0.0)
        {
          this._delayPackets.Add(new NCDelayPacket()
          {
            connection = connection,
            data = bitBuffer,
            time = this._minimumLatency - 0.016f + Rando.Float(this._jitter)
          });
          connection.PacketSent();
        }
        else if (this._timeForFailure > 0 || (double) this._dropPercent != 0.0 && (double) Rando.Float(1f) < (double) this._dropPercent)
        {
          connection.PacketSent();
        }
        else
        {
          NCError ncError = this.OnSendPacket(bitBuffer.buffer, bitBuffer.lengthInBytes, connection.data);
          connection.PacketSent();
          if (ncError == null)
            return;
          DevConsole.Log(ncError.text, ncError.color, index: this._core.networkIndex);
        }
      }
    }

    public abstract NCError OnSendPacket(byte[] data, int length, object connection);

    public abstract string GetConnectionIdentifier(object connection);

    public abstract string GetConnectionName(object connection);

    public string GetLocalName()
    {
      string str = this.OnGetLocalName();
      if (str.Length > 18)
        str = str.Substring(0, 18);
      return str;
    }

    protected virtual string OnGetLocalName() => "";

    private bool ProcessReceivedPacket(NetworkPacket packet)
    {
      if (this._timeForFailure > 0)
        return false;
      if (NetworkDebugger.enabled)
        NetworkDebugger.LogReceive(NetworkDebugger.GetID(this._networkIndex), packet.connection.identifier);
      BitBuffer data = packet.data;
      packet.sessionIndex = data.Read<NetIndex4>();
      if (data.ReadBool())
      {
        byte num = data.ReadByte();
        for (int index = 0; index < (int) num; ++index)
          packet.connection.acksReceived.Add(data.ReadByte());
      }
      if (data.ReadBool())
      {
        packet.order = data.Read<byte>();
        this._netGraph.LogPacket(packet.breakdown);
        packet.valid = true;
        return true;
      }
      if (!packet.IsValidSession())
        DevConsole.Log(DCSection.NetCore, "|DGRED|Ignored ACK from " + packet.connection.name + " (BAD SESSION " + (object) packet.sessionIndex + " VS " + (object) packet.connection.remoteSessionIndex + ")", this._core.networkIndex);
      return false;
    }

    public void UpdateDelayPackets()
    {
      if ((double) this._spikeTime > 0.0)
      {
        this._spikeTime -= Maths.IncFrameTimer();
      }
      else
      {
        this._spikeTime = 0.0f;
        if ((double) Rando.Float(2f) < (double) this._spikes)
          this._spikeTime = (float) (0.25 + (double) Rando.Float(2f) * (double) this._spikes);
      }
      lock (this._threadLock)
      {
        if ((double) this._failurePercent != 0.0)
        {
          ++this._failureCounter;
          if (this._failureCounter > 60)
          {
            this._failureCounter = 0;
            if ((double) Rando.Float(1f) < (double) this._failurePercent)
              this._timeForFailure = 240;
          }
        }
        if (this._timeForFailure > 0)
          --this._timeForFailure;
        if ((double) this._minimumLatency <= 0.0 && (double) this._jitter <= 0.0)
          return;
        List<NCDelayPacket> ncDelayPacketList = new List<NCDelayPacket>();
        foreach (NCDelayPacket delayPacket in this._delayPackets)
        {
          delayPacket.time -= Maths.IncFrameTimer();
          if ((double) delayPacket.time <= 0.0)
          {
            if (this._timeForFailure == 0 && ((double) this._dropPercent == 0.0 || (double) Rando.Float(1f) > (double) this._dropPercent))
              this.OnSendPacket(delayPacket.data.buffer, delayPacket.data.lengthInBytes, delayPacket.connection.data);
            ncDelayPacketList.Add(delayPacket);
          }
        }
        foreach (NCDelayPacket ncDelayPacket in ncDelayPacketList)
          this._delayPackets.Remove(ncDelayPacket);
      }
    }

    public virtual void Update()
    {
      if (this._lastConnectionClosed)
      {
        if (this.sessionConnections.Count > 0)
        {
          this._lastConnectionClosed = false;
        }
        else
        {
          this.OnSessionEnded();
          return;
        }
      }
      lock (this._threadPendingMessages)
      {
        foreach (NCError threadPendingMessage in this._threadPendingMessages)
          DevConsole.Log(threadPendingMessage.text, threadPendingMessage.color, index: this._networkIndex);
        this._threadPendingMessages.Clear();
      }
      if (this._networkThread == null)
      {
        this._breakDisconnectLoop = true;
      }
      else
      {
        lock (this._threadLock)
        {
          List<NetworkPacket> networkPacketList = (List<NetworkPacket>) null;
          lock (this._pendingPackets)
          {
            networkPacketList = new List<NetworkPacket>((IEnumerable<NetworkPacket>) this._pendingPackets);
            this._pendingPackets.Clear();
          }
          foreach (NetworkPacket packet in networkPacketList)
            this.ProcessReceivedPacket(packet);
          if (networkPacketList.Count > 1)
          {
            for (int index = 1; index < networkPacketList.Count; ++index)
            {
              NetworkPacket networkPacket1 = networkPacketList[index - 1];
              NetworkPacket networkPacket2 = networkPacketList[index];
              NetIndex8 order = (NetIndex8) (int) networkPacket1.order;
              if ((NetIndex8) (int) networkPacket2.order < order)
              {
                networkPacketList[index] = networkPacket1;
                networkPacketList[index - 1] = networkPacket2;
                index = 0;
              }
            }
          }
          foreach (NetworkPacket networkPacket in networkPacketList)
          {
            if (networkPacket.valid)
            {
              NetIndex8 packetOrderReceived = (NetIndex8) (int) networkPacket.connection.lastPacketOrderReceived;
              NetIndex8 order = (NetIndex8) (int) networkPacket.order;
              bool flag1 = false;
              if (order < (int) packetOrderReceived - 4 && networkPacket.connection.status == ConnectionStatus.Connected)
                flag1 = true;
              NetworkConnection.context = networkPacket.connection;
              try
              {
                networkPacket.Unpack();
              }
              catch
              {
                DevConsole.Log(DCSection.NetCore, "|DGRED|Message unpack failure, possible corruption");
                Program.LogLine("Message unpack failure, possible corruption.");
                continue;
              }
              bool flag2 = false;
              foreach (NetMessage allMessage in networkPacket.GetAllMessages())
              {
                if (allMessage is NMJoinDuckNetwork)
                  DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join messager");
                if (allMessage is NMNetworkCoreMessage)
                {
                  networkPacket.connection.OnMessage(allMessage as NMNetworkCoreMessage);
                  flag2 = true;
                }
              }
              if (flag1)
              {
                DevConsole.Log(DCSection.NetCore, "|DGRED|Ignoring out of order packet!");
              }
              else
              {
                networkPacket.connection.lastPacketOrderReceived = networkPacket.order;
                if (networkPacket.IsValidSession())
                {
                  if (!networkPacket.dropPacket)
                    networkPacket.connection.acksToSend.Add(networkPacket.order);
                  networkPacket.connection.PacketReceived(networkPacket);
                  networkPacket.connection.manager.PacketReceived(networkPacket);
                }
                else if (!flag2)
                  DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Ignored packet from " + networkPacket.connection.name + " (BAD SESSION " + (object) networkPacket.sessionIndex + " VS " + (object) networkPacket.connection.remoteSessionIndex + ")", this._networkIndex);
                NetworkConnection.context = (NetworkConnection) null;
              }
            }
          }
        }
        this._ghostManager.PreUpdate();
        foreach (NetworkConnection sessionConnection in this.sessionConnections)
        {
          NetworkConnection.context = sessionConnection;
          if ((double) this._traffic > 0.0 && sessionConnection.data != null)
            Send.Message((NetMessage) new NMNetworkTrafficGarbage((int) ((double) this._traffic * 500.0)), sessionConnection);
          sessionConnection.Update();
          NetworkConnection.context = (NetworkConnection) null;
        }
      }
    }

    public float averagePing
    {
      get
      {
        float num = 0.0f;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
          num += networkConnection.manager.ping;
        return num / (float) connections.Count;
      }
    }

    public float averagePingPeak
    {
      get
      {
        float num = 0.0f;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
          num += networkConnection.manager.pingPeak;
        return num / (float) connections.Count;
      }
    }

    public float averageJitter
    {
      get
      {
        float num = 0.0f;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
          num += networkConnection.manager.jitter;
        return num / (float) connections.Count;
      }
    }

    public float averageJitterPeak
    {
      get
      {
        float num = 0.0f;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
          num += networkConnection.manager.jitterPeak;
        return num / (float) connections.Count;
      }
    }

    public int averagePacketLoss
    {
      get
      {
        int num = 0;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
          num += networkConnection.manager.losses;
        return num / connections.Count;
      }
    }

    public int averagePacketLossPercent
    {
      get
      {
        int num1 = 0;
        int num2 = 0;
        List<NetworkConnection> connections = this.connections;
        foreach (NetworkConnection networkConnection in connections)
        {
          num1 += networkConnection.manager.losses;
          num2 += networkConnection.manager.sent;
        }
        return (int) Math.Round((double) (num1 / connections.Count) / (double) (num2 / connections.Count) * 100.0);
      }
    }

    public void PostUpdate()
    {
      List<NetworkConnection> allConnections = this.allConnections;
      this._ghostManager.UpdateInit();
      this._ghostManager.UpdateGhostLerp();
      this._ghostManager.UpdateRemoval();
      ++this.frame;
      int num = 0;
      for (int index = 0; index < allConnections.Count; ++index)
      {
        NetworkConnection.connectionLoopIndex = num;
        NetworkConnection networkConnection = allConnections[index];
        NetworkConnection.context = networkConnection;
        networkConnection.PostUpdate(this.frame);
        NetworkConnection.context = (NetworkConnection) null;
        ++num;
      }
      this._ghostManager.PostUpdate();
      this._ghostManager.UpdateRemoval();
    }

    public void PostDraw()
    {
      if (!Network.isActive)
        return;
      this._ghostManager.PostDraw();
    }

    protected virtual void KillConnection()
    {
    }

    public void ForcefulTermination()
    {
      this.OnSessionEnded();
      lock (this._connectionHistory)
      {
        this._connectionHistory.Clear();
        this._connectionsDirty = true;
      }
    }

    public virtual void Terminate()
    {
      this._breakDisconnectLoop = false;
      Network.Disconnect();
      while (!this._breakDisconnectLoop)
      {
        Network.PreUpdate();
        Network.PostUpdate();
        --this._breakDisconnectTries;
        if (this._breakDisconnectTries < 0)
        {
          this.OnSessionEnded();
          break;
        }
        Thread.Sleep(16);
      }
      if (this._networkThread != null)
        this._networkThread.Abort();
      if (this._timeThread == null)
        return;
      this._timeThread.Abort();
    }
  }
}
