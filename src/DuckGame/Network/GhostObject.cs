// Decompiled with JetBrains decompiler
// Type: DuckGame.GhostObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DuckGame
{
  [DebuggerDisplay("Thing = {_thing}")]
  public class GhostObject
  {
    public GhostCost cost;
    public bool hasPrev;
    public Vec2 prevPosition;
    public float prevRotation;
    private Dictionary<NetworkConnection, GhostConnectionData> _connectionData = new Dictionary<NetworkConnection, GhostConnectionData>();
    private ushort prevInputState;
    public int oldGhostTicks;
    private BitBuffer _stateData;
    public bool destroyMessageSent;
    public bool permaOldGhost;
    private NetIndex16 _ghostObjectIndex;
    private BufferedGhostState _previousObjectState;
    public bool didDestroyRefresh;
    private ushort _lastInputState;
    public int cooldown;
    private bool _dataInitialized;
    private List<BufferedGhostState> _bufferedStates = new List<BufferedGhostState>();
    private BufferedGhostState _currentNetworkState;
    private BufferedGhostState _latestNetworkState;
    private bool _firstState = true;
    private bool _connectionSwitch;
    private int _framesSinceData;
    private NetIndex16 _lastTickReceived = (NetIndex16) 0;
    private NetIndex16 _chasingLastTickReceived = (NetIndex16) 0;
    public List<BufferedGhostState> _stateTimeline = new List<BufferedGhostState>();
    private int _applyFrames;
    private bool _applyNow;
    public bool isOldGhost;
    private bool _shouldRemove;
    private bool inLoop;
    private byte _storedInputStates;
    private ushort[] _inputStates = new ushort[NetworkConnection.packetsEvery];
    private bool becomeVisible;
    private float angleLerp;
    private bool interpolating;
    private bool emptied = true;
    private byte followingTick;
    private int inc;
    private ushort lastInputState;
    public NetIndex16 _chasingTick = (NetIndex16) 0;
    public float _tickIncrement;
    public float _tickIncrementAmount;
    private bool usedLastIndex;
    private BufferedGhostState _lastAppliedState;
    private BufferedGhostState _lastPreviousState;
    private Thing _thing;
    private bool initializedCached;
    private List<StateBinding> _fields = new List<StateBinding>();
    private Dictionary<GhostPriority, List<StateBinding>> _sortedFields = new Dictionary<GhostPriority, List<StateBinding>>()
    {
      {
        GhostPriority.Low,
        new List<StateBinding>()
      },
      {
        GhostPriority.Normal,
        new List<StateBinding>()
      },
      {
        GhostPriority.High,
        new List<StateBinding>()
      }
    };
    public List<StateBinding> _lerpedFields = new List<StateBinding>();
    private GhostManager _manager;
    public ITakeInput _inputObject;

    public GhostConnectionData GetConnectionData(NetworkConnection c)
    {
      if (c == null)
        return (GhostConnectionData) null;
      GhostConnectionData ghostConnectionData = (GhostConnectionData) null;
      if (!this._connectionData.TryGetValue(c, out ghostConnectionData))
      {
        ghostConnectionData = new GhostConnectionData();
        this._connectionData[c] = ghostConnectionData;
      }
      return ghostConnectionData;
    }

    public void ClearConnectionData(NetworkConnection c)
    {
      if (!this._connectionData.ContainsKey(c))
        return;
      this._connectionData.Remove(c);
    }

    private void WriteMinimalStateMask(NetworkConnection c, BitBuffer b)
    {
      long connectionStateMask = this.GetConnectionData(c).connectionStateMask;
      b.WriteBits((object) connectionStateMask, this._fields.Count);
    }

    public static long ReadMinimalStateMask(System.Type t, BitBuffer b) => b.ReadBits<long>(Editor.AllStateFields[t].Length);

    private object GetMinimalStateMask(NetworkConnection c)
    {
      long connectionStateMask = this.GetConnectionData(c).connectionStateMask;
      int count = this._fields.Count;
      if (count <= 8)
        return (object) (byte) connectionStateMask;
      if (count <= 16)
        return (object) (short) connectionStateMask;
      return count <= 32 ? (object) (int) connectionStateMask : (object) connectionStateMask;
    }

    public static long ReadMask(System.Type t, BitBuffer b)
    {
      int length = Editor.AllStateFields[t].Length;
      if (length <= 8)
        return (long) b.ReadByte();
      if (length <= 16)
        return (long) b.ReadShort();
      return length <= 32 ? (long) b.ReadInt() : b.ReadLong();
    }

    public static bool MaskIsMaxValue(System.Type t, long mask)
    {
      int length = Editor.AllStateFields[t].Length;
      if (length <= 8)
        return mask == (long) byte.MaxValue;
      if (length <= 16)
        return mask == (long) short.MaxValue;
      return length <= 32 ? mask == (long) int.MaxValue : mask == long.MaxValue;
    }

    public void ClearStateMask(NetworkConnection c) => this.GetConnectionData(c).connectionStateMask = 0L;

    public void DirtyStateMask(long mask, NetworkConnection c) => this.GetConnectionData(c).connectionStateMask |= mask;

    public void SuperDirtyStateMask()
    {
      List<NetworkConnection> connections = Network.activeNetwork.core.connections;
      for (int index = 0; index < connections.Count; ++index)
        this.GetConnectionData(connections[index]).connectionStateMask = long.MaxValue;
    }

    public bool HasInput()
    {
      if ((int) this._inputStates[0] == (int) this.prevInputState)
        return false;
      this.prevInputState = this._inputStates[0];
      return true;
    }

    public bool IsDirty(NetworkConnection c)
    {
      GhostConnectionData connectionData = this.GetConnectionData(c);
      bool flag = this.HasInput();
      return connectionData.connectionStateMask != 0L || connectionData.authority < this.thing.authority || flag || connectionData.needsToSendInput;
    }

    public bool isDestroyed => this._thing.removeFromLevel;

    public NetIndex16 ghostObjectIndex
    {
      get => this._ghostObjectIndex;
      set => this._ghostObjectIndex = value;
    }

    public BufferedGhostState previousObjectState
    {
      get => this._previousObjectState;
      set
      {
        if (this._previousObjectState != null)
          this._previousObjectState = (BufferedGhostState) null;
        this._previousObjectState = value;
      }
    }

    public void RefreshStateMask(List<NetworkConnection> nclist)
    {
      long num1 = 0;
      int num2 = 0;
      if (this.previousObjectState != null && this.previousObjectState.properties != null)
      {
        List<BufferedGhostProperty> properties = this.previousObjectState.properties;
        for (int index = 0; index < properties.Count; ++index)
        {
          BufferedGhostProperty bufferedGhostProperty = properties[index];
          if (this.thing is TrappedDuck && (this.thing as TrappedDuck)._duckOwner != null && (this.thing as TrappedDuck)._duckOwner.netProfileIndex == (byte) 1)
            ;
          if (bufferedGhostProperty.binding.trueOnly)
          {
            if (!(bool) bufferedGhostProperty.value && (bool) bufferedGhostProperty.binding.value)
              num1 |= 1L << num2;
          }
          else if (!StateBinding.Compare(bufferedGhostProperty.value, bufferedGhostProperty.binding.value))
            num1 |= 1L << num2;
          ++num2;
        }
      }
      if (num1 == 0L)
        --this.cooldown;
      for (int index = 0; index < nclist.Count; ++index)
        this.GetConnectionData(nclist[index]).connectionStateMask |= num1;
      this.previousObjectState = this.GetCurrentState();
    }

    public void ForceInitialize()
    {
      this.initializedCached = true;
      foreach (StateBinding field in this._fields)
        field.initialized = true;
    }

    public BitBuffer GetNetworkStateData(NetworkConnection c, bool minimum = false)
    {
      if (this.thing is TrappedDuck && (this.thing as TrappedDuck)._duckOwner != null && ((this.thing as TrappedDuck)._duckOwner.netProfileIndex == (byte) 1 && c != null))
      {
        int num1 = c.isHost ? 1 : 0;
      }
      GhostConnectionData connectionData = this.GetConnectionData(c);
      this._stateData = new BitBuffer();
      if (!minimum && this.isLocalController)
      {
        for (int index = 0; index < NetworkConnection.packetsEvery; ++index)
          this._stateData.Write(this._inputStates[((int) this._storedInputStates + index) % NetworkConnection.packetsEvery]);
      }
      this._stateData.Write((ushort) (int) this.ghostObjectIndex);
      this._dataInitialized = true;
      this.cooldown = NetworkConnection.packetsEvery * 2;
      if (!minimum)
      {
        connectionData.needsToSendInput = false;
        this._stateData.Write(Editor.IDToType[this._thing.GetType()]);
        this._stateData.Write(DuckNetwork.levelIndex);
        this._stateData.Write((object) this.thing.authority);
        this._stateData.Write((ushort) (int) Network.TickSync);
      }
      BitBuffer val = new BitBuffer();
      short num2 = 0;
      if (!minimum)
        val.Write(this.GetMinimalStateMask(c));
      foreach (StateBinding field in this._fields)
      {
        if (minimum || (connectionData.connectionStateMask & 1L << (int) num2) != 0L)
        {
          if (field is DataBinding)
            val.Write(field.GetNetValue() as BitBuffer, true);
          else
            val.WriteBits(field.GetNetValue(), field.bits);
          field.Clean();
        }
        ++num2;
      }
      this._stateData.Write(val, true);
      if (!minimum)
      {
        connectionData.connectionStateMask = 0L;
        connectionData.authority = this.thing.authority;
      }
      return this._stateData;
    }

    public void ReadInNetworkData(
      NMGhostState ghostState,
      long mask,
      NetworkConnection c,
      bool constructed)
    {
      ghostState.mask = mask;
      if (this._lastTickReceived < ghostState.tick || Math.Abs(NetIndex16.Difference(ghostState.tick, this._lastTickReceived)) > 10)
        this._lastTickReceived = ghostState.tick;
      this._chasingLastTickReceived = this._lastTickReceived;
      BufferedGhostState bufferedGhostState1 = new BufferedGhostState();
      bufferedGhostState1.tick = ghostState.tick;
      if (ghostState is NMGhostInputState)
        bufferedGhostState1.inputStates = (ghostState as NMGhostInputState).inputStates;
      short num1 = 0;
      foreach (StateBinding field in this._fields)
      {
        long num2 = 1L << (int) num1;
        if ((ghostState.mask & num2) != 0L)
        {
          if (field is DataBinding)
            bufferedGhostState1.properties.Add(new BufferedGhostProperty()
            {
              binding = field,
              value = (object) ghostState.data.ReadBitBuffer(),
              isNetValue = true
            });
          else
            bufferedGhostState1.properties.Add(new BufferedGhostProperty()
            {
              binding = field,
              value = field.ReadNetValue(ghostState.data.ReadBits(field.type, field.bits)),
              isNetValue = true
            });
          field.initialized = true;
        }
        else
          bufferedGhostState1.properties.Add(new BufferedGhostProperty()
          {
            binding = field,
            valid = false
          });
        ++num1;
      }
      if (ghostState.minimalState || this._firstState)
      {
        bufferedGhostState1.Apply((BufferedGhostState) null);
        this._previousObjectState = bufferedGhostState1;
        if (!ghostState.minimalState)
          this._firstState = false;
        if (this.thing is Holdable)
        {
          Holdable thing = this.thing as Holdable;
          if (thing.isSpawned && !thing.didSpawn)
          {
            thing.spawnAnimation = true;
            thing.didSpawn = true;
          }
        }
      }
      else
      {
        if (this._stateTimeline.Count == 1 && this._stateTimeline[this._stateTimeline.Count - 1].life <= 0)
          this._stateTimeline.Clear();
        if (this._stateTimeline.Count == 0)
          this._chasingTick = this._chasingLastTickReceived;
        bool flag = false;
        for (int index = 0; index < this._stateTimeline.Count; ++index)
        {
          BufferedGhostState bufferedGhostState2 = this._stateTimeline[index];
          if (bufferedGhostState1.tick < bufferedGhostState2.tick)
          {
            this._stateTimeline.Insert(index, bufferedGhostState1);
            flag = true;
            break;
          }
        }
        if (!flag)
          this._stateTimeline.Add(bufferedGhostState1);
        for (int index = 1; index < this._stateTimeline.Count; ++index)
          this._stateTimeline[index].Trickle(this._stateTimeline[index - 1]);
      }
      if (!(this._thing is PhysicsObject))
        return;
      (this._thing as PhysicsObject).sleeping = false;
    }

    public BufferedGhostState GetCurrentState()
    {
      BufferedGhostState bufferedGhostState = new BufferedGhostState();
      int count = this._fields.Count;
      for (int index = 0; index < count; ++index)
        bufferedGhostState.properties.Add(new BufferedGhostProperty()
        {
          binding = this._fields[index],
          value = this._fields[index].value
        });
      return bufferedGhostState;
    }

    public void ApplyBufferedState(
      BufferedGhostState ghostState,
      bool lerp = false,
      BufferedGhostState lerpFrom = null,
      float lerpProgress = 0.0f)
    {
      if (ghostState.properties == null)
        return;
      for (int index = 0; index < ghostState.properties.Count; ++index)
      {
        BufferedGhostProperty property = ghostState.properties[index];
        if (property.valid)
        {
          if (!lerp)
            property.binding.value = property.value;
          else if (property.value is Vec2)
          {
            Vec2 current = (Vec2) lerpFrom.properties[index].value;
            Vec2 to = (Vec2) property.value;
            property.binding.value = (object) Lerp.Vec2Smooth(current, to, lerpProgress);
          }
          else if (property.binding.isRotation)
          {
            if (property.binding._previousValue == null)
              property.binding._previousValue = property.binding.value;
            Vec2 p2 = this.Slerp(Maths.AngleToVec((float) lerpFrom.properties[index].value), Maths.AngleToVec((float) property.value), lerpProgress);
            property.binding.value = (object) Maths.DegToRad(Maths.PointDirection(Vec2.Zero, p2));
          }
        }
      }
    }

    private Vec2 Slerp(Vec2 from, Vec2 to, float step)
    {
      if ((double) step == 0.0)
        return from;
      if (from == to || (double) step == 1.0)
        return to;
      double a = Math.Acos((double) Vec2.Dot(from, to));
      if (a == 0.0)
        return to;
      double num = Math.Sin(a);
      return (float) (Math.Sin((1.0 - (double) step) * a) / num) * from + (float) (Math.Sin((double) step * a) / num) * to;
    }

    public bool isLocalController => this._inputObject != null && this._inputObject.inputProfile != null && this._inputObject.inputProfile.virtualDevice == null;

    public bool shouldRemove
    {
      get => this._shouldRemove;
      set => this._shouldRemove = value;
    }

    public void KillNetworkData()
    {
      if (this.thing is TrappedDuck && (this.thing as TrappedDuck)._duckOwner != null)
      {
        int netProfileIndex = (int) (this.thing as TrappedDuck)._duckOwner.netProfileIndex;
      }
      this._bufferedStates.Clear();
      this.hasPrev = false;
      this._firstState = true;
      this._lastAppliedState = (BufferedGhostState) null;
      this._lastPreviousState = (BufferedGhostState) null;
      this._stateTimeline.Clear();
    }

    public bool IsInitialized()
    {
      if (!this.initializedCached)
      {
        this.initializedCached = true;
        foreach (StateBinding field in this._fields)
        {
          if (!field.initialized)
          {
            this.initializedCached = false;
            break;
          }
        }
      }
      return this.initializedCached;
    }

    public void UpdateTick()
    {
      if (this.isLocalController)
      {
        this._inputStates[(int) this._storedInputStates] = this._inputObject.inputProfile.state;
        this._storedInputStates = (byte) (((int) this._storedInputStates + 1) % NetworkConnection.packetsEvery);
      }
      this._bufferedStates.Clear();
    }

    public void Update()
    {
      ++this.inc;
      if (this.inc > 4)
        this.inc = 0;
      if (this._thing == null)
        return;
      this._thing.isLocal = false;
      if (!this.IsInitialized())
      {
        DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Skipping ghost update (NOT INITIALIZED)");
      }
      else
      {
        Vec2 position = this._thing.position;
        float angle = this._thing.angle;
        if (this._thing.active)
          this._thing.DoUpdate();
        if (this._thing.owner != null)
          return;
        this._thing.position = position;
        this._thing.angle = angle;
      }
    }

    public void UpdateRemoval()
    {
      if (this._thing.ghostType != (ushort) 0 && this._thing.level == Level.current)
        return;
      this._shouldRemove = true;
    }

    public BufferedGhostState GetStateForTick(NetIndex16 t, float lerp)
    {
      for (int index = 0; index < this._stateTimeline.Count; ++index)
      {
        BufferedGhostState bufferedGhostState1 = this._stateTimeline[index];
        if (bufferedGhostState1.tick > t && index > 0)
        {
          if (index > 1)
          {
            BufferedGhostState bufferedGhostState2 = this._stateTimeline[index - 2];
            this._stateTimeline[index - 1].prev = bufferedGhostState2;
          }
          return this._stateTimeline[index - 1];
        }
        if (bufferedGhostState1.tick == t)
        {
          if (index > 0)
            bufferedGhostState1.prev = this._stateTimeline[index - 1];
          return bufferedGhostState1;
        }
      }
      return this._stateTimeline.Count > 0 ? this._stateTimeline[this._stateTimeline.Count - 1] : (BufferedGhostState) null;
    }

    public void UpdateState()
    {
      if (this._stateTimeline.Count > 8)
        this._stateTimeline.RemoveRange(0, 2);
      if (this._stateTimeline.Count > 0)
      {
        --this._stateTimeline[this._stateTimeline.Count - 1].life;
        if (this._stateTimeline[this._stateTimeline.Count - 1].life <= 0 && this._stateTimeline.Count > 1)
          this._stateTimeline.RemoveAt(0);
      }
      if (NetIndex16.Difference(this._chasingLastTickReceived, this._chasingTick) > 6 || this._chasingTick > this._chasingLastTickReceived)
        this._chasingTick = this._chasingLastTickReceived;
      float num = (float) NetIndex16.Difference(this._chasingTick, this._chasingLastTickReceived) * 0.2f;
      if ((double) num < -1.0)
        num = -1f;
      if ((double) num > 1.0)
        num = 1f;
      this._tickIncrementAmount = 1f - num;
      if ((double) this._tickIncrementAmount < 0.0)
        this._tickIncrementAmount = Math.Min(this._tickIncrementAmount + 0.1f, 1f);
      if ((double) this._tickIncrementAmount > 1.0)
        this._tickIncrementAmount = Math.Max(this._tickIncrementAmount - 0.1f, 1f);
      if ((double) this._tickIncrementAmount < 1.0)
      {
        double tickIncrementAmount = (double) this._tickIncrementAmount;
      }
      this._tickIncrement += this._tickIncrementAmount;
      while ((double) this._tickIncrement >= 1.0)
      {
        --this._tickIncrement;
        this._chasingTick += 1;
      }
      ++this._chasingLastTickReceived;
      this.usedLastIndex = false;
      BufferedGhostState stateForTick = this.GetStateForTick((NetIndex16) ((int) this._chasingTick - 4), this._tickIncrementAmount);
      if (stateForTick != null)
      {
        if (stateForTick == this._stateTimeline[this._stateTimeline.Count - 1])
          this.usedLastIndex = true;
        stateForTick.Apply((BufferedGhostState) null, this._tickIncrementAmount);
        this.previousObjectState = stateForTick;
        if (stateForTick.inputStates != null && stateForTick.inputStates.Count > 0 && (this._inputObject != null && this._inputObject.inputProfile != null) && this._inputObject.inputProfile.virtualDevice != null)
          this._inputObject.inputProfile.virtualDevice.SetState(stateForTick.inputStates[0]);
      }
      if (this._thing.ghostType != (ushort) 0)
        return;
      this._shouldRemove = true;
    }

    public Thing thing => this._thing;

    public GhostManager manager => this._manager;

    public GhostObject(Thing thing, GhostManager manager, int ghostIndex = -1, bool levelInit = false)
    {
      this._thing = thing;
      this._thing.ghostObject = this;
      this._inputObject = this._thing as ITakeInput;
      this.initializedCached = false;
      foreach (FieldInfo fieldInfo in Editor.AllStateFields[this._thing.GetType()])
      {
        StateBinding stateBinding = fieldInfo.GetValue((object) this._thing) as StateBinding;
        stateBinding.Connect(this._thing);
        this._fields.Add(stateBinding);
        if (ghostIndex != -1)
          stateBinding.initialized = false;
      }
      this.previousObjectState = this.GetCurrentState();
      this.cost = GhostCost.Cheap;
      if (this._fields.Count > 20)
        this.cost = GhostCost.Expensive;
      else if (this._fields.Count > 10)
        this.cost = GhostCost.Normal;
      this._manager = manager;
      if (ghostIndex != -1)
      {
        this._ghostObjectIndex = new NetIndex16(ghostIndex);
        this._thing.ghostType = Editor.IDToType[this._thing.GetType()];
      }
      else
      {
        this._ghostObjectIndex = this._manager.GetGhostIndex();
        int num = DuckNetwork.localDuckIndex;
        if (levelInit)
          num = DuckNetwork.hostDuckIndex;
        this._ghostObjectIndex += num * 10000;
        if (levelInit && !Network.isServer)
          return;
        this._thing.connection = DuckNetwork.localConnection;
      }
    }
  }
}
