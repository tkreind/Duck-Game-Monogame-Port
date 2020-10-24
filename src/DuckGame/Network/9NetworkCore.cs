// Decompiled with JetBrains decompiler
// Type: DuckGame.NetworkConnection
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NetworkConnection
  {
    public byte lastPacketOrderReceived;
    private static NetworkConnection _context;
    private bool _hadConnection;
    private Profile _profile;
    private NetIndex4 _sessionIndex = new NetIndex4(0);
    public NetIndex4 remoteSessionIndex = new NetIndex4(0, true);
    private object _data;
    private string _identifier;
    private StreamManager _manager;
    private byte _loadingStatus = byte.MaxValue;
    protected ConnectionStatus _internalStatus;
    private bool _isHost;
    private bool _sentThisFrame;
    private uint _lastReceivedTime;
    private uint _lastSentTime;
    private byte _connectsReceived;
    private uint _personalTick;
    private int _disconnectsSent;
    private string _theirVersion = "";
    private bool _theirModsIncompatible;
    public int dataTransferSize;
    public int dataTransferProgress;
    public int wantsGhostData = -1;
    public static int connectionLoopIndex;
    private uint _ticksSinceLastMessage;
    private int _ticksSinceConnectionAttempt;
    private uint _lastTickReceived;
    private uint _estimatedClientTick;
    public List<byte> acksToSend = new List<byte>();
    public List<byte> acksReceived = new List<byte>();
    public List<NetworkPacket> packetsSent = new List<NetworkPacket>();
    public bool restartPingTimer = true;
    public double averageHeartbeatTime;
    private Timer _frameTimer = new Timer();
    private int pingWait;
    public bool sendPacketsNow;
    public static int packetsEvery = 1;

    public static NetworkConnection context
    {
      get => NetworkConnection._context;
      set => NetworkConnection._context = value;
    }

    public bool hadConnection => this._hadConnection;

    public Profile profile
    {
      get => this._profile;
      set => this._profile = value;
    }

    public NetIndex4 sessionIndex => this._sessionIndex;

    public object data => this._data;

    public string identifier => this._identifier;

    public string name
    {
      get
      {
        string str = (string) null;
        if (this._data != null)
          str = Network.activeNetwork.core.GetConnectionName(this._data);
        return str == null || str == "" ? this._identifier : str;
      }
    }

    public StreamManager manager => this._manager;

    public byte loadingStatus
    {
      get => this._loadingStatus;
      set => this._loadingStatus = value;
    }

    protected ConnectionStatus _status
    {
      get => this._internalStatus;
      set
      {
        if (this._internalStatus != value && Network.activeNetwork != null && Network.activeNetwork.core != null)
          Network.activeNetwork.core._connectionsDirty = true;
        this._internalStatus = value;
      }
    }

    public ConnectionStatus status => this._status;

    public void AttemptConnection() => this._status = this._data == null ? ConnectionStatus.Connected : ConnectionStatus.Connecting;

    public void WaitForInformation() => this._status = ConnectionStatus.WaitingForInformation;

    private void ChangeStatus(ConnectionStatus s) => this._status = s;

    public bool isHost
    {
      get => this._isHost;
      set => this._isHost = value;
    }

    public bool sentThisFrame
    {
      get => this._sentThisFrame;
      set => this._sentThisFrame = value;
    }

    public uint ticksSinceLastMessage => this._ticksSinceLastMessage;

    public uint lastTickReceived
    {
      get => this._lastTickReceived;
      set
      {
        this._lastTickReceived = value;
        this._estimatedClientTick = this._lastTickReceived;
      }
    }

    public uint estimatedClientTick
    {
      get => this._estimatedClientTick;
      set => this._estimatedClientTick = value;
    }

    public void SetData(object d)
    {
      this._data = d;
      if (this._data != null)
        this._identifier = Network.activeNetwork.core.GetConnectionIdentifier(this._data);
      this.Reset();
    }

    public NetworkConnection(object dat, string id = null)
    {
      this._data = dat;
      this._identifier = this._data == null ? "local" : Network.activeNetwork.core.GetConnectionIdentifier(this._data);
      if (id != null)
        this._identifier = id;
      this.Reset();
    }

    public void Reset()
    {
      this._manager = new StreamManager(this);
      this.acksReceived = new List<byte>();
      this.acksToSend = new List<byte>();
      this.packetsSent = new List<NetworkPacket>();
      this._isHost = false;
      this.ChangeStatus(ConnectionStatus.Disconnected);
      this.loadingStatus = byte.MaxValue;
      this._sentThisFrame = false;
      this._lastReceivedTime = 0U;
      this._lastSentTime = 0U;
      this._personalTick = 0U;
      this._connectsReceived = (byte) 0;
      this._ticksSinceLastMessage = 0U;
      this._lastTickReceived = 0U;
      this._estimatedClientTick = 0U;
      this._ticksSinceConnectionAttempt = 0;
      this.remoteSessionIndex = (NetIndex4) 0;
      this._disconnectsSent = 0;
      this.wantsGhostData = -1;
      this._theirVersion = "";
      this._theirModsIncompatible = false;
      if (this._data != null && GhostManager.context != null)
        GhostManager.context.Clear(this);
      ++this._sessionIndex;
      DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Reset called on " + this.identifier);
    }

    public void Disconnect()
    {
      if (this.status != ConnectionStatus.Disconnected && this._status != ConnectionStatus.Disconnecting)
      {
        this._ticksSinceConnectionAttempt = 0;
        this.remoteSessionIndex = (NetIndex4) 0;
        this._status = ConnectionStatus.Disconnecting;
      }
      if (this._data != null)
        return;
      this.ChangeStatus(ConnectionStatus.Disconnected);
      Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ControlledDisconnect, "Controlled disconnect."));
    }

    public void PacketReceived(NetworkPacket packet) => this._lastReceivedTime = this._personalTick;

    public void PacketSent() => this._lastSentTime = this._personalTick;

    public void OnMessage(NMNetworkCoreMessage message)
    {
      if (this._data == null)
        return;
      switch (message)
      {
        case NMPing _:
          Send.Message((NetMessage) new NMPong(), NetMessagePriority.UnreliableUnordered, this);
          this.sendPacketsNow = true;
          break;
        case NMPong _:
          this.manager.LogPing((float) this._frameTimer.elapsed.TotalSeconds);
          this.pingWait = 30;
          break;
        case NMWrongVersion _:
          this._theirVersion = (message as NMWrongVersion).version;
          this.ChangeStatus(ConnectionStatus.Disconnecting);
          break;
        case NMModIncompatibility _:
          this._theirModsIncompatible = true;
          this.ChangeStatus(ConnectionStatus.Disconnecting);
          break;
        case NMConnect _:
          if (this.status == ConnectionStatus.Disconnecting)
            break;
          NMConnect nmConnect = message as NMConnect;
          if (DG.version != nmConnect.version)
          {
            this._theirVersion = nmConnect.version;
            Send.Message((NetMessage) new NMWrongVersion(DG.version), NetMessagePriority.UnreliableUnordered, this);
            this.ChangeStatus(ConnectionStatus.Disconnecting);
            break;
          }
          if (ModLoader.modHash != nmConnect.modHash)
          {
            this._theirModsIncompatible = true;
            Send.Message((NetMessage) new NMModIncompatibility(), NetMessagePriority.UnreliableUnordered, this);
            this.ChangeStatus(ConnectionStatus.Disconnecting);
            break;
          }
          if (this.remoteSessionIndex == 0 || this.remoteSessionIndex < message.session)
          {
            this.remoteSessionIndex = message.session;
            if (this.status != ConnectionStatus.Disconnected && this.status != ConnectionStatus.Connecting)
            {
              this.ChangeStatus(ConnectionStatus.Disconnected);
              Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.NewSessionRequested, "Client requested a new session."));
              Network.activeNetwork.core.AttemptConnection(this._data, this.isHost);
            }
            this._ticksSinceConnectionAttempt = 0;
            this._connectsReceived = (byte) 0;
          }
          DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Connect message (" + (object) nmConnect.remoteSession + " " + (object) this.remoteSessionIndex + " " + (object) this.sessionIndex + ")");
          if (nmConnect.remoteSession == this.sessionIndex)
          {
            ++this._connectsReceived;
            if (this.status == ConnectionStatus.Connecting)
            {
              this._hadConnection = true;
              this.ChangeStatus(ConnectionStatus.Connected);
              Network.activeNetwork.core.OnConnection(this);
            }
          }
          if (this.status != ConnectionStatus.Connected || !(nmConnect.connectsReceived == 0))
            break;
          Send.Message((NetMessage) new NMConnect(this._connectsReceived, this.remoteSessionIndex, DG.version, ModLoader.modHash), NetMessagePriority.UnreliableUnordered, this);
          break;
        case NMDisconnect _:
          if (this.status == ConnectionStatus.Disconnected || !(message.session == this.remoteSessionIndex))
            break;
          this.ChangeStatus(ConnectionStatus.Disconnected);
          Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ControlledDisconnect, "CONTROLLED DISCONNECT"));
          break;
      }
    }

    public void RestartPingTimer()
    {
      this._frameTimer.Restart();
      this.restartPingTimer = false;
    }

    public void Update()
    {
      if (this._status == ConnectionStatus.Connected)
        this._hadConnection = true;
      if (this._status == ConnectionStatus.Disconnected)
        return;
      if (this._data == null && this._status == ConnectionStatus.WaitingForInformation)
      {
        ++this._ticksSinceConnectionAttempt;
        if (this._ticksSinceConnectionAttempt <= Maths.SecondsToTicks(6f))
          return;
        this.ChangeStatus(ConnectionStatus.Disconnected);
        Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ConnectionTimeout, "Lobby join timeout."));
      }
      else if (this._data == null || this._disconnectsSent == 3)
      {
        if (this._status != ConnectionStatus.Disconnecting)
          return;
        this.ChangeStatus(ConnectionStatus.Disconnected);
        if (this._theirVersion != "")
        {
          int num = (int) DuckNetwork.CheckVersion(this._theirVersion);
          Network.activeNetwork.core.OnDisconnect(this, DuckNetwork.AssembleMismatchError(this._theirVersion));
        }
        else if (this._theirModsIncompatible)
        {
          Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ModsIncompatible, "INCOMPATIBLE MOD SETUP!"));
          ConnectionError.joinLobby = Steam.lobby;
        }
        else
          Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ControlledDisconnect, "CONTROLLED DISCONNECT!"));
      }
      else
      {
        this.UpdatePacketAcking();
        this._manager.Update();
        int num1 = 30;
        if (this._status == ConnectionStatus.Disconnecting)
          num1 = 2;
        else if (this._status == ConnectionStatus.Connecting)
          num1 = 20;
        if (this._ticksSinceConnectionAttempt % num1 == 0)
        {
          if (this._status == ConnectionStatus.Disconnecting)
          {
            Send.Message((NetMessage) new NMDisconnect(), NetMessagePriority.UnreliableUnordered, this);
            ++this._disconnectsSent;
          }
          else if (this.status == ConnectionStatus.Connecting)
          {
            Send.Message((NetMessage) new NMConnect(this._connectsReceived, this.remoteSessionIndex, DG.version, ModLoader.modHash), NetMessagePriority.UnreliableUnordered, this);
            DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Connect send    (  " + (object) this.remoteSessionIndex + " " + (object) this.sessionIndex + ")");
          }
          else if (this.pingWait > 60)
          {
            this.restartPingTimer = true;
            Send.Message((NetMessage) new NMPing(), NetMessagePriority.UnreliableUnordered, this);
            this.pingWait = 0;
            this.sendPacketsNow = true;
          }
        }
        ++this.pingWait;
        ++this._ticksSinceConnectionAttempt;
        if (this._status == ConnectionStatus.Connecting)
        {
          if ((double) Maths.TicksToSeconds(this._ticksSinceConnectionAttempt) <= 10.0)
            return;
          this.ChangeStatus(ConnectionStatus.Disconnected);
          Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ConnectionTimeout, "Could not establish connection."));
        }
        else if (this._status == ConnectionStatus.Disconnecting)
        {
          if ((double) Maths.TicksToSeconds(this._ticksSinceConnectionAttempt) <= 6.0)
            return;
          this.ChangeStatus(ConnectionStatus.Disconnected);
          if (this._theirVersion != "")
          {
            int num2 = (int) DuckNetwork.CheckVersion(this._theirVersion);
            Network.activeNetwork.core.OnDisconnect(this, DuckNetwork.AssembleMismatchError(this._theirVersion));
          }
          else if (this._theirModsIncompatible)
            Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ModsIncompatible, "Mods incompatible."));
          else
            Network.activeNetwork.core.OnDisconnect(this, new DuckNetErrorInfo(DuckNetError.ControlledDisconnect, "Disconnect attempt timed out."));
        }
        else
        {
          ++this._personalTick;
          ++this._estimatedClientTick;
          if (this._status == ConnectionStatus.Connecting || this._status == ConnectionStatus.Disconnecting)
            return;
          uint num2 = this._personalTick - this._lastReceivedTime;
          if (num2 > 580U)
            this.Disconnect();
          this._ticksSinceLastMessage = num2;
        }
      }
    }

    public void UpdatePacketAcking()
    {
      this.sentThisFrame = false;
      List<NetworkPacket> networkPacketList = new List<NetworkPacket>();
      foreach (NetworkPacket networkPacket in this.packetsSent)
      {
        networkPacket.timeout += Maths.IncFrameTimer();
        if (this.acksReceived.Contains(networkPacket.order))
        {
          foreach (NetMessage message in networkPacket.messages)
          {
            if (message.priority == NetMessagePriority.ReliableOrdered)
            {
              DevConsole.Log(DCSection.NetCore, "|DGYELLOW|was received " + message.ToString());
              this.manager.NotifyReliableMessageStatus(message, false);
            }
            else if (message.priority == NetMessagePriority.Volatile)
              this.manager.NotifyVolatileMessageStatus(message, false);
          }
        }
        else if ((double) networkPacket.timeout > (double) networkPacket.maxTimeout)
        {
          foreach (NetMessage message in networkPacket.messages)
          {
            if (message.priority == NetMessagePriority.ReliableOrdered)
            {
              DevConsole.Log(DCSection.NetCore, "|DGYELLOW|timeout " + message.ToString());
              this.manager.NotifyReliableMessageStatus(message, true);
            }
            else if (message.priority == NetMessagePriority.Volatile)
              this.manager.NotifyVolatileMessageStatus(message, true);
          }
        }
        else
          networkPacketList.Add(networkPacket);
      }
      this.acksReceived.Clear();
      this.packetsSent = networkPacketList;
    }

    public void PostUpdate(int frameCounter)
    {
      if (this._status == ConnectionStatus.Disconnected)
        return;
      bool sendPackets = (frameCounter + NetworkConnection.connectionLoopIndex) % NetworkConnection.packetsEvery == 0;
      bool flag = true;
      if ((int) this.loadingStatus == (int) DuckNetwork.levelIndex && flag)
      {
        GhostManager.context.Update(this, sendPackets);
        this._manager._ghostSyncInc = 0;
      }
      ++this._manager._ghostSyncInc;
      this._manager.PostUpdate();
      if (this.sendPacketsNow || sendPackets)
      {
        this.sendPacketsNow = false;
        this._manager.Flush(false);
      }
      this._manager.Flush(true);
      if (this._sentThisFrame || this.acksToSend.Count <= 0)
        return;
      Network.activeNetwork.core.SendPacket((NetworkPacket) null, this);
    }
  }
}
