// Decompiled with JetBrains decompiler
// Type: DuckGame.NetMessage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Reflection;

namespace DuckGame
{
  public class NetMessage
  {
    public bool failedOnce;
    private NetworkConnection _connection;
    public NetMessageStatus status = new NetMessageStatus();
    public ushort order;
    public bool activated;
    public bool queued;
    public ushort typeIndex;
    public NetIndex4 session;
    public NetMessagePriority priority;
    public BelongsToManager manager;
    protected BitBuffer _serializedData;
    private bool _wasReceived;

    public NetworkConnection connection
    {
      get => this._connection;
      set => this._connection = value;
    }

    public virtual bool MessageIsCompleted() => true;

    public BitBuffer serializedData => this._serializedData;

    public void ClearSerializedData() => this._serializedData = (BitBuffer) null;

    public void Deserialize(BitBuffer msg)
    {
      this.order = msg.ReadUShort();
      this.OnDeserialize(msg);
    }

    public virtual void OnDeserialize(BitBuffer msg)
    {
      foreach (FieldInfo field in this.GetType().GetFields())
      {
        if (field.FieldType == typeof (string))
          field.SetValue((object) this, (object) msg.ReadString());
        else if (field.FieldType == typeof (float))
          field.SetValue((object) this, (object) msg.ReadFloat());
        else if (field.FieldType == typeof (bool) && field.Name != "activated" && field.Name != "queued")
          field.SetValue((object) this, (object) msg.ReadBool());
        else if (field.FieldType == typeof (byte))
          field.SetValue((object) this, (object) msg.ReadByte());
        else if (field.FieldType == typeof (sbyte))
          field.SetValue((object) this, (object) msg.ReadSByte());
        else if (field.FieldType == typeof (double))
          field.SetValue((object) this, (object) msg.ReadDouble());
        else if (field.FieldType == typeof (int))
          field.SetValue((object) this, (object) msg.ReadInt());
        else if (field.FieldType == typeof (ulong))
          field.SetValue((object) this, (object) msg.ReadULong());
        else if (field.FieldType == typeof (uint))
          field.SetValue((object) this, (object) msg.ReadUInt());
        else if (field.FieldType == typeof (ushort) && field.Name != "order" && field.Name != "typeIndex")
          field.SetValue((object) this, (object) msg.ReadUShort());
        else if (field.FieldType == typeof (short))
          field.SetValue((object) this, (object) msg.ReadShort());
        else if (field.FieldType == typeof (NetIndex4) && field.Name != "session")
          field.SetValue((object) this, (object) msg.Read<NetIndex4>());
        else if (field.FieldType == typeof (Vec2))
          field.SetValue((object) this, (object) new Vec2()
          {
            x = msg.ReadFloat(),
            y = msg.ReadFloat()
          });
        else if (typeof (Thing).IsAssignableFrom(field.FieldType))
          field.SetValue((object) this, msg.Read(typeof (Thing)));
      }
    }

    public BitBuffer Serialize()
    {
      if (this._serializedData != null)
        return this._serializedData;
      this._serializedData = new BitBuffer();
      this._serializedData.Write(Network.typeToMessageID[this.GetType()]);
      this._serializedData.Write(this.order);
      this.OnSerialize();
      return this._serializedData;
    }

    public void SerializePacketData()
    {
      this._serializedData = (BitBuffer) null;
      this.OnSerialize();
    }

    protected virtual void OnSerialize()
    {
      if (this._serializedData == null)
        this._serializedData = new BitBuffer();
      foreach (FieldInfo field in this.GetType().GetFields())
      {
        if (field.FieldType == typeof (string))
          this._serializedData.Write(field.GetValue((object) this) as string);
        else if (field.FieldType == typeof (float))
          this._serializedData.Write((float) field.GetValue((object) this));
        else if (field.FieldType == typeof (bool) && field.Name != "activated" && field.Name != "queued")
          this._serializedData.Write((bool) field.GetValue((object) this));
        else if (field.FieldType == typeof (byte))
          this._serializedData.Write((byte) field.GetValue((object) this));
        else if (field.FieldType == typeof (sbyte))
          this._serializedData.Write((sbyte) field.GetValue((object) this));
        else if (field.FieldType == typeof (double))
          this._serializedData.Write((double) field.GetValue((object) this));
        else if (field.FieldType == typeof (int))
          this._serializedData.Write((int) field.GetValue((object) this));
        else if (field.FieldType == typeof (ulong))
          this._serializedData.Write((ulong) field.GetValue((object) this));
        else if (field.FieldType == typeof (uint))
          this._serializedData.Write((uint) field.GetValue((object) this));
        else if (field.FieldType == typeof (ushort) && field.Name != "order" && field.Name != "typeIndex")
          this._serializedData.Write((ushort) field.GetValue((object) this));
        else if (field.FieldType == typeof (short))
          this._serializedData.Write((short) field.GetValue((object) this));
        else if (field.FieldType == typeof (NetIndex4) && field.Name != "session")
          this._serializedData.WriteBits((object) (int) (NetIndex4) field.GetValue((object) this), 4);
        else if (field.FieldType == typeof (Vec2))
        {
          Vec2 vec2 = (Vec2) field.GetValue((object) this);
          this._serializedData.Write(vec2.x);
          this._serializedData.Write(vec2.y);
        }
        else if (typeof (Thing).IsAssignableFrom(field.FieldType))
          this._serializedData.Write((object) (field.GetValue((object) this) as Thing));
      }
    }

    public virtual void DoMessageWasReceived()
    {
      if (!this._wasReceived)
        this.MessageWasReceived();
      this._wasReceived = true;
    }

    public virtual void MessageWasReceived()
    {
    }
  }
}
