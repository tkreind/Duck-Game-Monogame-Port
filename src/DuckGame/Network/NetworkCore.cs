// Decompiled with JetBrains decompiler
// Type: DuckGame.NetworkPacket
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class NetworkPacket
  {
    public bool valid;
    public List<NetMessage> messages = new List<NetMessage>();
    public byte order;
    public float timeout;
    public float maxTimeout;
    public int extraWait = -1;
    private NCPacketBreakdown _breakdown;
    public NetIndex4 sessionIndex = new NetIndex4();
    private bool _received;
    private bool _sent;
    private BitBuffer _data;
    private NetworkConnection _receivedFrom;
    private float _timeSinceReceived;
    public bool dropPacket;

    public NetworkPacket(BitBuffer dat, NetworkConnection from, byte orderVal)
    {
      if (from == null)
        throw new Exception("Network packet connection information cannot be null.");
      this._data = dat;
      this._receivedFrom = from;
      this._breakdown = new NCPacketBreakdown();
      this.order = orderVal;
    }

    public bool IsValidSession() => this.sessionIndex == this._receivedFrom.remoteSessionIndex;

    public NCPacketBreakdown breakdown => this._breakdown;

    public bool received => this._received;

    public bool sent => this._sent;

    public BitBuffer data => this._data;

    public NetworkConnection connection => this._receivedFrom;

    public float timeSinceReceived => this._timeSinceReceived;

    public void Tick() => this._timeSinceReceived += Maths.IncFrameTimer();

    public MultiMap<NetMessagePriority, NetMessage> unpackedMessages { get; private set; }

    public void Unpack()
    {
      if (this.unpackedMessages != null)
        return;
      this.breakdown.Add(NCPacketDataType.ExtraData, 64);
      this.unpackedMessages = new MultiMap<NetMessagePriority, NetMessage>();
      foreach (NetMessagePriority key1 in Enum.GetValues(typeof (NetMessagePriority)).Cast<NetMessagePriority>())
      {
        this.breakdown.Add(NCPacketDataType.ExtraData, 1);
        if (this._data.ReadBool())
        {
          do
          {
            this.breakdown.Add(NCPacketDataType.ExtraData, 16);
            ushort key2 = this._data.ReadUShort();
            NetMessage instance = Activator.CreateInstance(Network.typeToMessageID[key2], (object[]) null) as NetMessage;
            instance.connection = this._receivedFrom;
            instance.priority = key1;
            instance.session = this.sessionIndex;
            instance.typeIndex = key2;
            int num = this._data.position + this._data.bitOffset;
            instance.Deserialize(this._data);
            int bits = this._data.position + this._data.bitOffset - num;
            switch (instance)
            {
              case NMEvent _:
                this.breakdown.Add(NCPacketDataType.Event, bits);
                break;
              case NMGhostState _:
                this.breakdown.Add(NCPacketDataType.Ghost, bits);
                break;
              default:
                this.breakdown.Add(NCPacketDataType.Other, bits);
                break;
            }
            if (!this.connection.hadConnection && !(instance is NMNetworkCoreMessage))
            {
              DevConsole.Log(DCSection.NetCore, "|DGYELLOW|Received non-connection message during connection.");
              this.dropPacket = true;
            }
            else
              this.unpackedMessages.Add(key1, instance);
            this.breakdown.Add(NCPacketDataType.ExtraData, 1);
          }
          while (this._data.ReadBool());
        }
      }
    }

    public List<NetMessage> FlushUrgentMessages()
    {
      List<NetMessage> netMessageList = new List<NetMessage>();
      List<NetMessage> list = (List<NetMessage>) null;
      if (this.unpackedMessages.TryGetValue(NetMessagePriority.Urgent, out list))
      {
        netMessageList = new List<NetMessage>((IEnumerable<NetMessage>) list);
        list.Clear();
      }
      return netMessageList;
    }

    public List<NetMessage> GetAllMessages()
    {
      List<NetMessage> netMessageList = new List<NetMessage>();
      foreach (NetMessagePriority key in Enum.GetValues(typeof (NetMessagePriority)).Cast<NetMessagePriority>())
      {
        List<NetMessage> list = (List<NetMessage>) null;
        if (this.unpackedMessages.TryGetValue(key, out list))
          netMessageList.AddRange((IEnumerable<NetMessage>) list);
      }
      return netMessageList;
    }
  }
}
