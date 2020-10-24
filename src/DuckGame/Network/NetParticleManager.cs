// Decompiled with JetBrains decompiler
// Type: DuckGame.NetParticleManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class NetParticleManager
  {
    private Dictionary<ushort, PhysicsParticle> _particles = new Dictionary<ushort, PhysicsParticle>();
    private ushort _nextParticleIndex = 1;
    public HashSet<ushort> removedParticleIndexes = new HashSet<ushort>();
    private bool syncNow;
    private List<PhysicsParticle> removedParticles = new List<PhysicsParticle>();
    private Queue<List<PhysicsParticle>> _pendingParticles = new Queue<List<PhysicsParticle>>();
    private Dictionary<System.Type, List<PhysicsParticle>> _inProgressParticleLists = new Dictionary<System.Type, List<PhysicsParticle>>();
    public Dictionary<NetworkConnection, Dictionary<System.Type, List<NMParticles>>> _pendingParticleSends = new Dictionary<NetworkConnection, Dictionary<System.Type, List<NMParticles>>>();
    public Dictionary<NetworkConnection, List<NMRemovedParticles>> _pendingParticleRemoveSends = new Dictionary<NetworkConnection, List<NMRemovedParticles>>();
    public static int _particleSyncSpread = 2;
    public static int _syncWait = 4;

    public ushort GetParticleIndex()
    {
      ++this._nextParticleIndex;
      return this._nextParticleIndex;
    }

    public void ResetParticleIndex() => this._nextParticleIndex = (ushort) 1;

    public void OnMessage(NetMessage m)
    {
      switch (m)
      {
        case NMRemovedParticles _:
          NMRemovedParticles removedParticles = m as NMRemovedParticles;
          for (int index = 0; index < removedParticles.count; ++index)
          {
            ushort key = removedParticles.data.ReadUShort();
            PhysicsParticle physicsParticle = (PhysicsParticle) null;
            if (this._particles.TryGetValue(key, out physicsParticle))
            {
              this.removedParticles.Add(physicsParticle);
              this.removedParticleIndexes.Add(physicsParticle.netIndex);
            }
          }
          break;
        case NMParticles _:
          NMParticles nmParticles = m as NMParticles;
          if ((int) nmParticles.levelIndex != (int) DuckNetwork.levelIndex)
            break;
          for (int index = 0; index < nmParticles.count; ++index)
          {
            System.Type type = nmParticles.type;
            ushort key = nmParticles.data.ReadUShort();
            PhysicsParticle physicsParticle = (PhysicsParticle) null;
            if (!this._particles.TryGetValue(key, out physicsParticle))
            {
              if (type == typeof (SmallFire))
                physicsParticle = (PhysicsParticle) SmallFire.New(-999f, -999f, 0.0f, 0.0f, canMultiply: false, network: true);
              else if (type == typeof (ExtinguisherSmoke))
                physicsParticle = (PhysicsParticle) new ExtinguisherSmoke(-999f, -999f, true);
              physicsParticle.netIndex = key;
              physicsParticle.isLocal = false;
              if (!this.removedParticleIndexes.Contains(key))
              {
                this._particles[key] = physicsParticle;
                Level.Add((Thing) physicsParticle);
              }
            }
            physicsParticle.NetDeserialize(nmParticles.data);
          }
          break;
      }
    }

    public void AddLocalParticle(PhysicsParticle p)
    {
      p.connection = DuckNetwork.localConnection;
      p.netIndex = (ushort) ((uint) this.GetParticleIndex() + (uint) (DuckNetwork.localDuckIndex * 5000));
      this._particles[p.netIndex] = p;
    }

    public void ChangeLevels()
    {
      this._particles.Clear();
      this._inProgressParticleLists.Clear();
      this._pendingParticles.Clear();
      this.removedParticleIndexes.Clear();
      this.ResetParticleIndex();
    }

    public List<PhysicsParticle> GetParticleList(System.Type t)
    {
      List<PhysicsParticle> physicsParticleList = (List<PhysicsParticle>) null;
      if (!this._inProgressParticleLists.TryGetValue(t, out physicsParticleList) || physicsParticleList.Count >= 20)
      {
        physicsParticleList = new List<PhysicsParticle>();
        this._inProgressParticleLists[t] = physicsParticleList;
        this._pendingParticles.Enqueue(physicsParticleList);
      }
      return physicsParticleList;
    }

    public void RemoveParticle(PhysicsParticle p)
    {
      if (!this._particles.ContainsKey(p.netIndex))
        return;
      this.removedParticles.Add(p);
    }

    public List<PhysicsParticle> GetSendList(NetworkConnection c, System.Type t)
    {
      Dictionary<System.Type, List<NMParticles>> particlePairs = this.GetParticlePairs(c);
      List<NMParticles> nmParticlesList = (List<NMParticles>) null;
      if (!particlePairs.TryGetValue(t, out nmParticlesList))
      {
        nmParticlesList = new List<NMParticles>();
        particlePairs[t] = nmParticlesList;
      }
      if (nmParticlesList.Count == 0 || nmParticlesList[nmParticlesList.Count - 1].GetParticles().Count > 30)
      {
        NMParticles nmParticles = new NMParticles();
        nmParticlesList.Add(nmParticles);
      }
      return nmParticlesList[nmParticlesList.Count - 1].GetParticles();
    }

    public Dictionary<System.Type, List<NMParticles>> GetParticlePairs(
      NetworkConnection c)
    {
      Dictionary<System.Type, List<NMParticles>> dictionary = (Dictionary<System.Type, List<NMParticles>>) null;
      if (!this._pendingParticleSends.TryGetValue(c, out dictionary))
      {
        dictionary = new Dictionary<System.Type, List<NMParticles>>();
        this._pendingParticleSends[c] = dictionary;
      }
      return dictionary;
    }

    public List<ushort> GetSendRemoveList(NetworkConnection c)
    {
      List<NMRemovedParticles> removedParticlesList = (List<NMRemovedParticles>) null;
      if (!this._pendingParticleRemoveSends.TryGetValue(c, out removedParticlesList))
      {
        removedParticlesList = new List<NMRemovedParticles>();
        this._pendingParticleRemoveSends[c] = removedParticlesList;
      }
      if (removedParticlesList.Count == 0 || removedParticlesList[removedParticlesList.Count - 1].GetParticles().Count > 30)
      {
        NMRemovedParticles removedParticles = new NMRemovedParticles();
        removedParticlesList.Add(removedParticles);
      }
      return removedParticlesList[removedParticlesList.Count - 1].GetParticles();
    }

    public List<NMRemovedParticles> GetParticleRemoveList(NetworkConnection c)
    {
      List<NMRemovedParticles> removedParticlesList = (List<NMRemovedParticles>) null;
      if (!this._pendingParticleRemoveSends.TryGetValue(c, out removedParticlesList))
      {
        removedParticlesList = new List<NMRemovedParticles>();
        this._pendingParticleRemoveSends[c] = removedParticlesList;
      }
      return removedParticlesList;
    }

    public void Notify(NetMessage m, bool dropped)
    {
      if (!(m is NMRemovedParticles))
        return;
      foreach (ushort particle in (m as NMRemovedParticles).GetParticles())
        this.GetSendRemoveList(m.connection).Add(particle);
    }

    public void Update(NetworkConnection c)
    {
      foreach (KeyValuePair<ushort, PhysicsParticle> particle in this._particles)
      {
        PhysicsParticle physicsParticle = particle.Value;
        if (physicsParticle.isLocal)
          this.GetSendList(c, physicsParticle.GetType()).Add(physicsParticle);
      }
      if (this.removedParticles.Count > 0)
      {
        for (int index = 0; index < this.removedParticles.Count; ++index)
        {
          foreach (NetworkConnection connection in Network.connections)
          {
            if (this.removedParticles[index].isLocal)
              this.GetSendRemoveList(connection).Add(this.removedParticles[index].netIndex);
          }
          this._particles.Remove(this.removedParticles[index].netIndex);
          Level.Remove((Thing) this.removedParticles[index]);
        }
        this.removedParticles.Clear();
      }
      using (Dictionary<System.Type, List<NMParticles>>.Enumerator enumerator = this.GetParticlePairs(c).GetEnumerator())
      {
label_22:
        while (enumerator.MoveNext())
        {
          KeyValuePair<System.Type, List<NMParticles>> current = enumerator.Current;
          int num = 0;
          while (true)
          {
            if (num < 3 && current.Value.Count > 0)
            {
              Send.Message((NetMessage) current.Value[0], NetMessagePriority.UnreliableUnordered, c);
              current.Value.RemoveAt(0);
              ++num;
            }
            else
              goto label_22;
          }
        }
      }
      List<NMRemovedParticles> particleRemoveList = this.GetParticleRemoveList(c);
      for (int index = 0; index < Math.Min(particleRemoveList.Count, 3); index = index - 1 + 1)
      {
        Send.Message((NetMessage) particleRemoveList[index], NetMessagePriority.Volatile, c);
        particleRemoveList.RemoveAt(index);
      }
    }
  }
}
