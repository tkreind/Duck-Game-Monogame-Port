// Decompiled with JetBrains decompiler
// Type: DuckGame.StreamManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class StreamManager
  {
    private float _ping;
    private float _pingPeak;
    private int _pingPeakReset;
    private int _losses;
    private int _lossAccumulator;
    private int _lossAccumulatorInc;
    public bool lossThisFrame;
    private int _sent;
    private float _jitter;
    private float _jitterPeak;
    private int _jitterPeakReset;
    private EventManager _eventManager;
    private List<SynchronizedNetMessage> _synchronizedEvents = new List<SynchronizedNetMessage>();
    private MultiMap<NetMessagePriority, NetMessage> _messages = new MultiMap<NetMessagePriority, NetMessage>();
    private MultiMap<NetMessagePriority, NetMessage> _largeMessages = new MultiMap<NetMessagePriority, NetMessage>();
    private List<NetMessage> _urgentResendList = new List<NetMessage>();
    private List<NetMessage> _reliableSendQueue = new List<NetMessage>();
    private List<NetMessage> _volatileSendList = new List<NetMessage>();
    private List<NetMessage> _orderedPackets = new List<NetMessage>();
    private BitBuffer _currentBuffer;
    private NetworkPacket _currentPacket;
    private int _maxBufferSize = 1024;
    private NetworkConnection _connection;
    private ushort _expectedReliableOrder;
    private int _lastReceivedAck = -1;
    private Queue<ushort> _receivedVolatilePackets = new Queue<ushort>();
    private Queue<ushort> _receivedUrgentPackets = new Queue<ushort>();
    public int _ghostSyncInc;
    public int syncWaitFrames = 2;
    private byte _packetOrder;
    private ushort _reliableOrder;
    private ushort _volatileID;
    private ushort _urgentID;
    private List<NetMessage> _resendUrgentList = new List<NetMessage>();

    public void Reset()
    {
      this._urgentResendList.Clear();
      this._volatileSendList.Clear();
      this._resendUrgentList.Clear();
      if (this._messages.ContainsKey(NetMessagePriority.UnreliableUnordered))
        this._messages[NetMessagePriority.UnreliableUnordered].Clear();
      if (this._messages.ContainsKey(NetMessagePriority.Urgent))
        this._messages[NetMessagePriority.Urgent].Clear();
      if (this._messages.ContainsKey(NetMessagePriority.Volatile))
        this._messages[NetMessagePriority.Volatile].Clear();
      if (this._largeMessages.ContainsKey(NetMessagePriority.UnreliableUnordered))
        this._largeMessages[NetMessagePriority.UnreliableUnordered].Clear();
      if (this._largeMessages.ContainsKey(NetMessagePriority.Urgent))
        this._largeMessages[NetMessagePriority.Urgent].Clear();
      if (!this._largeMessages.ContainsKey(NetMessagePriority.Volatile))
        return;
      this._largeMessages[NetMessagePriority.Volatile].Clear();
    }

    public float ping => this._ping;

    public float pingPeak => this._pingPeak;

    public int losses => this._losses;

    public int accumulatedLoss => this._lossAccumulator;

    public void RecordLoss()
    {
      this.lossThisFrame = true;
      ++this._lossAccumulator;
      ++this._losses;
    }

    public int sent => this._sent;

    public float jitter => this._jitter;

    public float jitterPeak => this._jitterPeak;

    public static StreamManager context => NetworkConnection.context != null ? NetworkConnection.context.manager : (StreamManager) null;

    public void LogPing(float pingVal)
    {
      this._ping = (float) (((double) pingVal + (double) this._ping) / 2.0);
      if ((double) this._ping <= 1.0)
        return;
      this._ping = 1f;
    }

    public EventManager eventManager => this._eventManager;

    public void UpdateSynchronizedEvents()
    {
      for (int index = 0; index < this._synchronizedEvents.Count; ++index)
      {
        if (this._synchronizedEvents[index].Update())
        {
          this.ProcessReceivedMessage((NetMessage) this._synchronizedEvents[index]);
          this._synchronizedEvents.RemoveAt(index);
          --index;
        }
      }
    }

    public NetMessage GetEarliestReliableSend() => this._reliableSendQueue.Count > 0 ? this._reliableSendQueue.First<NetMessage>() : (NetMessage) null;

    public List<NetMessage> GetVolatileSendList() => new List<NetMessage>((IEnumerable<NetMessage>) this._volatileSendList);

    public long GetPendingStates(GhostObject obj)
    {
      long num = 0;
      foreach (NetMessage volatileSend in this._volatileSendList)
      {
        if (volatileSend is NMGhostState)
        {
          NMGhostState nmGhostState = volatileSend as NMGhostState;
          if (nmGhostState.ghost == obj)
            num |= nmGhostState.mask;
        }
      }
      return num;
    }

    public int maxBufferSize
    {
      get => this._maxBufferSize;
      set => this._maxBufferSize = value;
    }

    public NetworkConnection connection => this._connection;

    public ushort expectedReliableOrder => this._expectedReliableOrder;

    private void IncrementExpectedOrder() => this._expectedReliableOrder = (ushort) (((int) this._expectedReliableOrder + 1) % (int) ushort.MaxValue);

    public int lastReceivedAck => this._lastReceivedAck;

    public StreamManager(NetworkConnection connection)
    {
      this._connection = connection;
      this._eventManager = new EventManager(connection, this);
    }

    public void NotifyReliableMessageStatus(NetMessage m, bool dropped)
    {
      if (dropped)
        this.RecordLoss();
      if (dropped)
      {
        this.QueueMessageTop(m);
        DevConsole.Log(DCSection.DuckNet, "|DGRED|re-sending " + m.ToString() + " (" + (object) m.order + ")(" + m.connection.identifier + ")");
      }
      else
      {
        m.DoMessageWasReceived();
        this._reliableSendQueue.Remove(m);
      }
    }

    public void NotifyVolatileMessageStatus(NetMessage m, bool dropped)
    {
      if (dropped)
        this.RecordLoss();
      this._volatileSendList.Remove(m);
      if (m.manager != BelongsToManager.GhostManager)
        return;
      GhostManager.context.Notify(this, m, dropped);
    }

    public void PacketReceived(NetworkPacket p)
    {
      using (List<NetMessage>.Enumerator enumerator = p.GetAllMessages().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetMessage m = enumerator.Current;
          if (m.priority == NetMessagePriority.ReliableOrdered)
          {
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|arrival " + m.ToString() + " (" + (object) m.order + "->" + (object) this._expectedReliableOrder + ")(" + this.connection.identifier + ")");
            if ((int) m.order >= (int) this._expectedReliableOrder)
            {
              if (this._orderedPackets.FirstOrDefault<NetMessage>((Func<NetMessage, bool>) (x => (int) x.order == (int) m.order)) == null)
              {
                this._orderedPackets.Add(m);
                if (m is NMJoinDuckNetwork)
                  DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|queuing join message");
              }
              else if (m is NMJoinDuckNetwork)
                DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|already have join message");
            }
            else if (m is NMJoinDuckNetwork)
              DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|trashing " + m.ToString() + " (" + (object) m.order + " VS " + (object) this._expectedReliableOrder + ")(" + this.connection.identifier + ")");
          }
          else if (m.priority == NetMessagePriority.Volatile)
          {
            if (!this._receivedVolatilePackets.Contains(m.order))
            {
              this.NetMessageReceived(m);
              this._receivedVolatilePackets.Enqueue(m.order);
            }
            if (this._receivedVolatilePackets.Count > 128)
            {
              int num = (int) this._receivedVolatilePackets.Dequeue();
            }
          }
          else if (m.priority == NetMessagePriority.Urgent)
          {
            if (!this._receivedUrgentPackets.Contains(m.order))
            {
              this.NetMessageReceived(m);
              this._receivedUrgentPackets.Enqueue(m.order);
            }
            if (this._receivedUrgentPackets.Count > 128)
            {
              int num = (int) this._receivedUrgentPackets.Dequeue();
            }
          }
          else
            this.NetMessageReceived(m);
        }
      }
    }

    public void NetMessageReceived(NetMessage m)
    {
      if (m.priority == NetMessagePriority.ReliableOrdered)
        return;
      this.ProcessReceivedMessage(m);
    }

    public void ProcessReceivedMessage(NetMessage m)
    {
      NetworkConnection.context = this._connection;
      Main.codeNumber = (int) m.typeIndex;
      if (m.manager == BelongsToManager.GhostManager)
        GhostManager.context.OnMessage(m);
      else if (m.manager == BelongsToManager.EventManager)
        this._eventManager.OnMessage(m);
      else if (m.manager == BelongsToManager.DuckNetwork)
        DuckNetwork.OnMessage(m);
      else if (m.manager == BelongsToManager.None)
        Network.OnMessageStatic(m);
      Main.codeNumber = 12345;
      NetworkConnection.context = (NetworkConnection) null;
    }

    public void QueueMessage(NetMessage msg)
    {
      if (!this.connection.hadConnection && !(msg is NMNetworkCoreMessage))
        return;
      if (msg.queued)
      {
        NetMessage instance = Activator.CreateInstance(msg.GetType(), (object[]) null) as NetMessage;
        Editor.CopyClass((object) msg, (object) instance);
        instance.ClearSerializedData();
        msg = instance;
      }
      msg.queued = true;
      msg.connection = this.connection;
      this._messages.Add(msg.priority, msg);
    }

    public void QueueMessageTop(NetMessage msg) => this._messages.Insert(msg.priority, 0, msg);

    public void Update()
    {
      if (this._lossAccumulator > 0)
      {
        ++this._lossAccumulatorInc;
        if (this._lossAccumulatorInc > 8)
        {
          this._lossAccumulatorInc = 0;
          --this._lossAccumulator;
        }
        if (this._lossAccumulator > 30)
          this._lossAccumulator = 30;
      }
      this._eventManager.Update();
      bool flag;
      do
      {
        flag = false;
        Queue<NetMessage> netMessageQueue = new Queue<NetMessage>();
        for (int index = 0; index < this._orderedPackets.Count; ++index)
        {
          NetMessage orderedPacket = this._orderedPackets[index];
          if ((int) orderedPacket.order <= (int) this._expectedReliableOrder)
          {
            if ((int) orderedPacket.order == (int) this._expectedReliableOrder)
            {
              DevConsole.Log(DCSection.DuckNet, "|DGGREEN|receiving " + orderedPacket.ToString() + " (" + (object) orderedPacket.order + "->" + (object) this._expectedReliableOrder + ")(" + this.connection.identifier + ")");
              this.IncrementExpectedOrder();
            }
            if (!(orderedPacket is SynchronizedNetMessage) || (orderedPacket as SynchronizedNetMessage).Update())
            {
              if (!orderedPacket.activated)
              {
                this.NetMessageReceived(orderedPacket);
                this.ProcessReceivedMessage(orderedPacket);
                orderedPacket.activated = true;
              }
              if (orderedPacket.MessageIsCompleted())
              {
                this._orderedPackets.RemoveAt(index);
                --index;
                flag = true;
              }
              else
                break;
            }
          }
        }
      }
      while (flag);
    }

    public void PostUpdate()
    {
    }

    private void SendCurrentPacket()
    {
      if (this._currentBuffer != null && this._currentBuffer.lengthInBytes > 0)
      {
        Network.activeNetwork.core.SendPacket(this._currentPacket, this._connection);
        this._currentPacket.maxTimeout = this.CalculateCurrentTimeout();
        this._currentBuffer = (BitBuffer) null;
        this._currentPacket = (NetworkPacket) null;
      }
      if (this._currentBuffer != null)
        return;
      this._currentBuffer = new BitBuffer();
      this._currentPacket = new NetworkPacket(this._currentBuffer, this._connection, this.GetPacketOrder());
    }

    private byte GetPacketOrder()
    {
      byte packetOrder = this._packetOrder;
      this._packetOrder = (byte) (((int) this._packetOrder + 1) % (int) byte.MaxValue);
      return packetOrder;
    }

    private ushort GetReliableOrder()
    {
      ushort reliableOrder = this._reliableOrder;
      this._reliableOrder = (ushort) (((int) this._reliableOrder + 1) % (int) ushort.MaxValue);
      return reliableOrder;
    }

    private ushort GetVolatileID()
    {
      ushort volatileId = this._volatileID;
      this._volatileID = (ushort) (((int) this._volatileID + 1) % (int) ushort.MaxValue);
      return volatileId;
    }

    private ushort GetUrgentID()
    {
      ushort urgentId = this._urgentID;
      this._urgentID = (ushort) (((int) this._urgentID + 1) % (int) ushort.MaxValue);
      return urgentId;
    }

    public void ClearAllMessages()
    {
    }

    public float CalculateCurrentTimeout()
    {
      float num1 = this.ping;
      if ((double) num1 > 0.5)
        num1 = 0.5f;
      float num2 = num1 * 2f;
      if (this._currentBuffer.lengthInBytes > 1024)
        num2 += Math.Min((float) this._currentBuffer.lengthInBytes / 8192f, 1f) * 12f;
      if ((double) num2 > 3.0)
        num2 = 3f;
      if ((double) num2 < 0.300000011920929)
        num2 = 0.3f;
      return num2;
    }

    public void Flush(bool large)
    {
      this.SendCurrentPacket();
      if (large && this._largeMessages.CountValues == 0 || !large && this._messages.CountValues == 0)
        return;
      bool flag = false;
      foreach (NetMessagePriority key in Enum.GetValues(typeof (NetMessagePriority)).Cast<NetMessagePriority>())
      {
        if (!flag || !large)
        {
          List<NetMessage> list = (List<NetMessage>) null;
          if (large)
            this._largeMessages.TryGetValue(key, out list);
          else
            this._messages.TryGetValue(key, out list);
          if (!large && key == NetMessagePriority.Urgent)
          {
            if (list == null)
            {
              this._resendUrgentList.Clear();
              list = this._resendUrgentList;
            }
            for (int index = this._urgentResendList.Count<NetMessage>() - 1; index >= 0; --index)
            {
              NetMessage urgentResend = this._urgentResendList[index];
              list.Insert(0, urgentResend);
            }
          }
          int num;
          if (list != null && list.Count > 0)
          {
            for (int index = 0; index < list.Count; index = num + 1)
            {
              NetMessage m = list[index];
              if (m.serializedData == null)
              {
                if (m.priority == NetMessagePriority.ReliableOrdered)
                {
                  m.order = this.GetReliableOrder();
                  DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|sending " + m.ToString() + " (" + (object) m.order + ")(" + this.connection.identifier + ")");
                }
                else if (m.priority == NetMessagePriority.Volatile)
                  m.order = this.GetVolatileID();
                else if (m.priority == NetMessagePriority.Urgent)
                  m.order = this.GetUrgentID();
                m.Serialize();
                if (m is NMFullGhostState)
                  DevConsole.Log("BIGPAK " + (object) m.serializedData.lengthInBytes, Color.Red);
              }
              if (this._maxBufferSize < 0 || this._maxBufferSize - this._currentBuffer.lengthInBytes >= m.serializedData.lengthInBytes || large)
              {
                if (m.priority == NetMessagePriority.ReliableOrdered)
                {
                  SynchronizedNetMessage synchronizedNetMessage = m as SynchronizedNetMessage;
                  if (this._reliableSendQueue.FirstOrDefault<NetMessage>((Func<NetMessage, bool>) (x => (int) x.order == (int) m.order)) == null)
                  {
                    NetMessage netMessage = m;
                    this._reliableSendQueue.Add(m);
                    ++this._sent;
                    if (synchronizedNetMessage != null)
                      this._synchronizedEvents.Add(synchronizedNetMessage);
                  }
                }
                else if (m.priority == NetMessagePriority.Volatile)
                {
                  NetMessage netMessage1 = this._volatileSendList.FirstOrDefault<NetMessage>((Func<NetMessage, bool>) (x => (int) x.order == (int) m.order));
                  if (netMessage1 == null)
                  {
                    NetMessage netMessage2 = m;
                    this._volatileSendList.Add(m);
                    ++this._sent;
                  }
                  else
                    netMessage1.status.tickOnSend = Network.Tick;
                }
                else if (m.priority == NetMessagePriority.Urgent)
                {
                  NetMessage netMessage = this._urgentResendList.FirstOrDefault<NetMessage>((Func<NetMessage, bool>) (x => x == m));
                  if (netMessage == null)
                  {
                    this._urgentResendList.Add(m);
                    ++this._sent;
                  }
                  else
                  {
                    ++netMessage.status.timesResent;
                    if (netMessage.status.timesResent >= 2)
                      this._urgentResendList.Remove(netMessage);
                  }
                }
                flag = true;
                this._currentBuffer.Write(true);
                this._currentBuffer.WriteBufferData(m.serializedData);
                this._currentPacket.messages.Add(m);
                list.RemoveAt(index);
                num = index - 1;
                if (large)
                  break;
              }
              else if (m.serializedData.lengthInBytes > 300)
              {
                this._largeMessages.Add(m.priority, m);
                list.RemoveAt(index);
                num = index - 1;
              }
              else
                break;
            }
          }
        }
        this._currentBuffer.Write(false);
      }
      ++NCBasic.headerBytes;
      if (!flag)
      {
        this._currentBuffer = (BitBuffer) null;
        this._currentPacket = (NetworkPacket) null;
      }
      if (this.connection.restartPingTimer)
        this.connection.RestartPingTimer();
      this.SendCurrentPacket();
    }
  }
}
