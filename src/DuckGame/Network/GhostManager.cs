// Decompiled with JetBrains decompiler
// Type: DuckGame.GhostManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class GhostManager
  {
    private NetParticleManager _particleManager = new NetParticleManager();
    private GhostObject _managerState;
    private Dictionary<Thing, GhostObject> _ghostsz = new Dictionary<Thing, GhostObject>();
    private Dictionary<Thing, GhostObject> _oldGhosts = new Dictionary<Thing, GhostObject>();
    private NetIndex16 ghostObjectIndex = new NetIndex16(32);
    public static bool inGhostLoop = false;
    public static bool updatingBullets = false;
    private int _freeFrames;
    public static int ghostSpread = 0;
    public static bool lazyGhostSync = true;
    private HashSet<Thing> _removeList = new HashSet<Thing>();

    public NetParticleManager particleManager => this._particleManager;

    public NetIndex16 predictionIndex
    {
      get => (this._managerState.thing as GhostManagerState).predictionIndex;
      set => (this._managerState.thing as GhostManagerState).predictionIndex = value;
    }

    private Dictionary<Thing, GhostObject> _ghosts => this._ghostsz;

    public static GhostManager context => Network.activeNetwork.core.ghostManager;

    public NetIndex16 currentGhostIndex => this.ghostObjectIndex;

    public NetIndex16 GetGhostIndex()
    {
      NetIndex16 ghostObjectIndex = this.ghostObjectIndex;
      ++this.ghostObjectIndex;
      return ghostObjectIndex;
    }

    public void SetGhostIndex(NetIndex16 idx)
    {
      this.ghostObjectIndex = idx;
      this.Clear();
    }

    public void IncrementGhostIndex(int amount) => this.ghostObjectIndex += amount;

    public void Clear()
    {
      this._ghosts.Clear();
      this._oldGhosts.Clear();
      DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Clearing all ghost data.");
    }

    public void Clear(NetworkConnection c)
    {
      bool flag = false;
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        ghost.Value.ClearConnectionData(c);
        flag = true;
      }
      List<Thing> thingList = new List<Thing>();
      foreach (KeyValuePair<Thing, GhostObject> oldGhost in this._oldGhosts)
      {
        oldGhost.Value.ClearConnectionData(c);
        flag = true;
        if (oldGhost.Key.connection == c)
          thingList.Add(oldGhost.Key);
      }
      foreach (Thing key in thingList)
        this._oldGhosts.Remove(key);
      if (Network.host != null)
      {
        foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
        {
          GhostObject ghostObject = ghost.Value;
          if (ghostObject.thing.connection == c)
          {
            Thing.SuperFondle(ghostObject.thing, Network.host);
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Clearing ghost data for " + c.identifier);
    }

    public GhostObject GetGhost(NetIndex16 id)
    {
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        if (ghost.Value.ghostObjectIndex == id)
          return ghost.Value;
      }
      foreach (KeyValuePair<Thing, GhostObject> oldGhost in this._oldGhosts)
      {
        if (oldGhost.Value.ghostObjectIndex == id)
          return oldGhost.Value;
      }
      return (GhostObject) null;
    }

    public GhostObject GetGhost(Thing thing)
    {
      GhostObject ghostObject = (GhostObject) null;
      return this._ghosts.TryGetValue(thing, out ghostObject) || this._oldGhosts.TryGetValue(thing, out ghostObject) ? ghostObject : (GhostObject) null;
    }

    public void OnMessage(NetMessage m)
    {
      try
      {
        switch (m)
        {
          case NMParticles _:
            NMParticles nmParticles = m as NMParticles;
            if ((int) nmParticles.levelIndex != (int) DuckNetwork.levelIndex)
              break;
            this.particleManager.OnMessage((NetMessage) nmParticles);
            break;
          case NMRemovedParticles _:
            this.particleManager.OnMessage(m);
            break;
          case NMFullGhostState _:
            using (List<NMGhostState>.Enumerator enumerator = (m as NMFullGhostState).states.GetEnumerator())
            {
              while (enumerator.MoveNext())
                this.OnMessage((NetMessage) enumerator.Current);
              break;
            }
          case NMGhostState _:
            NMGhostState ghostState = m as NMGhostState;
            if ((int) ghostState.levelIndex != (int) DuckNetwork.levelIndex)
              break;
            GhostObject ghost = this.GetGhost(ghostState.id);
            System.Type t = Editor.IDToType[ghostState.classID];
            long mask = ghostState.mask;
            if (mask == 0L)
              mask = GhostObject.ReadMask(t, ghostState.data);
            if (ghost != null && t != ghost.thing.GetType())
            {
              this.RemoveGhost(ghost, false);
              ghost = (GhostObject) null;
            }
            bool flag = false;
            if (ghost == null)
            {
              Thing thing = Editor.CreateThing(t);
              Level.Add(thing);
              thing.connection = m.connection;
              ghost = new GhostObject(thing, this, (int) ghostState.id);
              ghost.ClearStateMask(m.connection);
              this._ghosts[thing] = ghost;
              flag = true;
            }
            else if (ghost.isDestroyed)
            {
              DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Skipped ghost from " + m.connection.name + "(DESTROYED)");
              break;
            }
            if (!flag)
              ghost.thing.TransferControl(m.connection, ghostState.authority);
            if (ghost.thing is TrappedDuck && NetworkDebugger.networkDrawingIndex == 1)
            {
              NetworkConnection connection1 = ghost.thing.connection;
              NetworkConnection connection2 = m.connection;
            }
            if (ghost.thing.connection == m.connection)
            {
              if (ghost.thing is TrappedDuck)
              {
                int networkDrawingIndex1 = NetworkDebugger.networkDrawingIndex;
              }
              if (ghost.thing is Gun)
              {
                int networkDrawingIndex2 = NetworkDebugger.networkDrawingIndex;
              }
              if (ghost.isOldGhost)
              {
                ghost.isOldGhost = false;
                this._oldGhosts.Remove(ghost.thing);
                this._ghosts[ghost.thing] = ghost;
              }
              ghost.ReadInNetworkData(ghostState, mask, m.connection, false);
              if (!ghostState.minimalState)
                break;
              if (ghost.thing.connection == m.connection)
              {
                ghost.thing.authority = (NetIndex8) 0;
                break;
              }
              ghost.thing.authority = (NetIndex8) 2;
              break;
            }
            for (int index = 0; index < Network.connections.Count; ++index)
            {
              NetworkConnection connection = Network.connections[index];
              ghost.DirtyStateMask(mask, connection);
            }
            break;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void Notify(StreamManager manager, NetMessage m, bool dropped)
    {
      switch (m)
      {
        case NMGhostState _ when dropped:
          NMGhostState nmGhostState = m as NMGhostState;
          if (nmGhostState.mask == 0L)
          {
            nmGhostState.ghost.GetConnectionData(manager.connection).needsToSendInput = true;
            break;
          }
          long mask = ~manager.GetPendingStates(nmGhostState.ghost) & nmGhostState.mask;
          nmGhostState.ghost.DirtyStateMask(mask, manager.connection);
          break;
        case NMParticles _:
        case NMRemovedParticles _ when dropped:
          this._particleManager.Notify(m, true);
          break;
      }
    }

    public void IncrementPrediction() => ++(this._managerState.thing as GhostManagerState).predictionIndex;

    private void RemoveThing(Thing t) => this._removeList.Add(t);

    public void PostUpdate()
    {
      GhostManager.inGhostLoop = true;
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        if (ghost.Value.shouldRemove)
          this.RemoveThing(ghost.Key);
      }
      GhostManager.inGhostLoop = false;
    }

    public void UpdateGhostLerp()
    {
      bool flag = false;
      if (Network.connections != null)
      {
        for (int index = 0; index < Network.connections.Count; ++index)
        {
          if ((int) Network.connections[index].loadingStatus == (int) DuckNetwork.levelIndex)
          {
            flag = true;
            break;
          }
        }
      }
      GhostManager.inGhostLoop = true;
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        GhostObject ghostObject = ghost.Value;
        if (ghostObject.thing.connection != DuckNetwork.localConnection)
        {
          if (ghostObject.thing.owner == null)
            ghostObject.Update();
          else
            ghostObject.Update();
          ghostObject.UpdateState();
          ghostObject.UpdateRemoval();
          if (ghostObject.shouldRemove && ghostObject.thing.connection != DuckNetwork.localConnection)
            this.RemoveThing(ghost.Key);
        }
        else
        {
          ghostObject.UpdateTick();
          if (!flag)
            ghostObject.UpdateRemoval();
        }
      }
      GhostManager.inGhostLoop = false;
    }

    public void PostDraw()
    {
    }

    public void FreeOldGhosts(bool withFreeFrames = true)
    {
      if (withFreeFrames)
        this._freeFrames = 60;
      foreach (KeyValuePair<Thing, GhostObject> oldGhost in this._oldGhosts)
      {
        oldGhost.Value.permaOldGhost = false;
        oldGhost.Value.oldGhostTicks = 0;
      }
    }

    public void UpdateRemoval()
    {
      foreach (Thing remove in this._removeList)
      {
        this._oldGhosts[remove] = this._ghosts[remove];
        if (this._freeFrames == 0)
          this._oldGhosts[remove].permaOldGhost = true;
        this._ghosts[remove].isOldGhost = true;
        remove.ghostType = (ushort) 0;
        if (!remove.removeFromLevel)
          Level.Remove(remove);
        this._ghosts.Remove(remove);
      }
      if (this._freeFrames > 0)
        --this._freeFrames;
      this._removeList.Clear();
      int num = (int) Math.Round((double) Network.highestPing * 60.0);
      if (num < 60)
        num = 60;
      foreach (KeyValuePair<Thing, GhostObject> oldGhost in this._oldGhosts)
      {
        if (!oldGhost.Value.permaOldGhost)
        {
          ++oldGhost.Value.oldGhostTicks;
          if (oldGhost.Value.oldGhostTicks > num)
            this._removeList.Add(oldGhost.Key);
        }
      }
      foreach (Thing remove in this._removeList)
        this._oldGhosts.Remove(remove);
      this._removeList.Clear();
    }

    public void RemoveGhost(GhostObject ghost, bool makeOld = true)
    {
      if (!this._ghosts.ContainsKey(ghost.thing))
        return;
      DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Removing ghost (" + ghost.ghostObjectIndex.ToString() + " " + ghost.thing.GetType().ToString() + ")");
      if (makeOld)
        this._oldGhosts[ghost.thing] = this._ghosts[ghost.thing];
      this._ghosts.Remove(ghost.thing);
      Level.Remove(ghost.thing);
    }

    public GhostObject MakeGhost(Thing t, int index = -1, bool initLevel = false)
    {
      if (this._ghosts.ContainsKey(t))
        return this._ghosts[t];
      GhostObject ghostObject = new GhostObject(t, this, index, initLevel);
      this._ghosts[t] = ghostObject;
      return ghostObject;
    }

    public void RefreshGhosts(Level lev = null)
    {
      if (lev == null)
        lev = Level.current;
      if (lev.things.objectsDirty)
      {
        Thing[] array = lev.things.updateList.OrderBy<Thing, ushort>((Func<Thing, ushort>) (x => x.physicsIndex)).ToArray<Thing>();
        GhostObject ghostObject1 = (GhostObject) null;
        if (array != null)
        {
          int num = ((IEnumerable<Thing>) array).Count<Thing>();
          for (int index = 0; index < num; ++index)
          {
            Thing thing = array[index];
            if (thing.isStateObject && !thing.removeFromLevel && (!thing.ignoreGhosting && !this._ghosts.TryGetValue(thing, out ghostObject1)))
            {
              GhostObject ghostObject2 = new GhostObject(thing, this);
              this._ghosts[thing] = ghostObject2;
            }
          }
        }
        lev.things.objectsDirty = false;
      }
      int num1 = 0;
      List<NetworkConnection> connections = Network.activeNetwork.core.connections;
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        if (ghost.Value.thing.isServerForObject)
          ghost.Value.RefreshStateMask(connections);
        ++num1;
      }
    }

    public void UpdateInit()
    {
      if (this._managerState == null)
        this._managerState = new GhostObject((Thing) new GhostManagerState(), this, 0);
      this._removeList.Clear();
    }

    private void SendGhostObject(
      bool delta,
      GhostObject ghost,
      NetMessagePriority priority,
      NetworkConnection connection,
      int idx = 0)
    {
      bool flag = false;
      foreach (NetworkConnection connection1 in Network.activeNetwork.core.connections)
      {
        bool isDestroyed = ghost.isDestroyed;
        if (connection1 == connection || isDestroyed && !ghost.destroyMessageSent)
        {
          if (!delta)
            ghost.DirtyStateMask(long.MaxValue, connection1);
          if (isDestroyed && !flag)
          {
            this.RemoveThing(ghost.thing);
            ghost.thing.ghostType = (ushort) 0;
            ghost.RefreshStateMask(Network.activeNetwork.core.connections);
          }
          if (ghost.IsDirty(connection1) && !ghost.destroyMessageSent)
          {
            bool isLocalController = ghost.isLocalController;
            long connectionStateMask = ghost.GetConnectionData(connection1).connectionStateMask;
            BitBuffer networkStateData = ghost.GetNetworkStateData(connection1);
            NMGhostState nmGhostState = !isLocalController ? new NMGhostState(networkStateData) : (NMGhostState) new NMGhostInputState(networkStateData);
            nmGhostState.mask = connectionStateMask;
            nmGhostState.ghost = ghost;
            NetMessagePriority priority1 = priority;
            if (isDestroyed)
            {
              flag = true;
              NetMessagePriority priority2 = NetMessagePriority.ReliableOrdered;
              Send.Message((NetMessage) nmGhostState, priority2, connection1);
            }
            else
              Send.Message((NetMessage) nmGhostState, priority1, connection1);
          }
        }
      }
      ghost.destroyMessageSent = flag;
    }

    public void SendAllGhostData(
      bool delta,
      NetMessagePriority priority,
      NetworkConnection connection,
      bool sendPackets = true)
    {
      this._removeList.Clear();
      if (delta)
      {
        foreach (KeyValuePair<Thing, GhostObject> ghost1 in this._ghosts)
        {
          GhostObject ghost2 = ghost1.Value;
          if (sendPackets && ghost2.thing.isInitialized && (ghost2.thing.connection == DuckNetwork.localConnection || ghost2.thing.connection == null || !delta))
            this.SendGhostObject(delta, ghost2, priority, connection);
        }
      }
      else
      {
        Send.Message((NetMessage) this.GetAllGhostData(), NetMessagePriority.ReliableOrdered, connection);
        DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Sending all ghost data to " + connection.identifier + " (" + (object) DuckNetwork.levelIndex + ")");
      }
    }

    public NMFullGhostState GetAllGhostData()
    {
      NMFullGhostState nmFullGhostState = new NMFullGhostState();
      foreach (KeyValuePair<Thing, GhostObject> ghost in this._ghosts)
      {
        GhostObject g = ghost.Value;
        if (!g.thing.isInitialized)
          g.thing.DoInitialize();
        nmFullGhostState.Add(g);
      }
      return nmFullGhostState;
    }

    public void PreUpdate()
    {
    }

    public void Update(NetworkConnection connection, bool sendPackets)
    {
      this.RefreshGhosts();
      this.SendAllGhostData(true, NetMessagePriority.Volatile, connection, sendPackets);
      if (!sendPackets)
        return;
      this.particleManager.Update(connection);
    }

    public void UpdateRemovalMessages()
    {
    }
  }
}
