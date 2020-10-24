﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.QuadTreeObjectList
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class QuadTreeObjectList : IEnumerable<Thing>, IEnumerable
  {
    private HashSet<Thing> _bigList = new HashSet<Thing>();
    private HashSet<Thing> _addThings = new HashSet<Thing>();
    private HashSet<Thing> _removeThings = new HashSet<Thing>();
    private MultiMap<System.Type, Thing, HashSet<Thing>> _objectsByType = new MultiMap<System.Type, Thing, HashSet<Thing>>();
    private MultiMap<System.Type, Thing, HashSet<Thing>> _staticObjectsByType = new MultiMap<System.Type, Thing, HashSet<Thing>>();
    private Block[,] _solidBlocks = new Block[256, 256];
    private QuadTree _quadTree = new QuadTree(4, new Vec2(-1656f, -1800f), 4096f, 8);
    private List<CollisionIsland> _islands = new List<CollisionIsland>();
    private bool _autoRefresh;
    private bool _useTree;
    public bool objectsDirty;

    public HashSet<Thing> updateList => this._bigList;

    public QuadTree quadTree => this._quadTree;

    public List<CollisionIsland> GetIslands(Vec2 point)
    {
      List<CollisionIsland> collisionIslandList = new List<CollisionIsland>();
      foreach (CollisionIsland island in this._islands)
      {
        if (!island.willDie && (double) (point - island.owner.position).lengthSq < (double) island.radiusSquared)
          collisionIslandList.Add(island);
      }
      return collisionIslandList;
    }

    public List<CollisionIsland> GetIslandsForCollisionCheck(Vec2 point)
    {
      List<CollisionIsland> collisionIslandList = new List<CollisionIsland>();
      foreach (CollisionIsland island in this._islands)
      {
        if (!island.willDie && (double) (point - island.owner.position).lengthSq < (double) island.radiusCheckSquared)
          collisionIslandList.Add(island);
      }
      return collisionIslandList;
    }

    public CollisionIsland GetIsland(Vec2 point, CollisionIsland ignore = null)
    {
      foreach (CollisionIsland island in this._islands)
      {
        if (!island.willDie && island != ignore && (double) (point - island.owner.position).lengthSq < (double) island.radiusSquared)
          return island;
      }
      return (CollisionIsland) null;
    }

    public void AddIsland(MaterialThing t) => this._islands.Add(new CollisionIsland(t, this));

    public void RemoveIsland(CollisionIsland i)
    {
      if (i.things.Count != 0)
      {
        i.owner = i.things.First<MaterialThing>();
        for (int index = 0; index < i.things.Count; ++index)
        {
          MaterialThing materialThing = i.things.ElementAt<MaterialThing>(index);
          if (materialThing != i.owner)
          {
            int count = i.things.Count;
            materialThing.UpdateIsland();
            if (i.things.Count != count)
              --index;
          }
        }
      }
      else
        i.willDie = true;
    }

    public void UpdateIslands()
    {
    }

    public bool useTree
    {
      get => this._useTree;
      set => this._useTree = value;
    }

    public QuadTreeObjectList(bool auto = false, bool tree = true)
    {
      this._autoRefresh = auto;
      this._useTree = tree;
    }

    public List<Thing> ToList()
    {
      List<Thing> thingList = new List<Thing>();
      thingList.AddRange((IEnumerable<Thing>) this._bigList);
      return thingList;
    }

    public IEnumerable<Thing> this[System.Type key]
    {
      get
      {
        if (key == typeof (Thing))
          return (IEnumerable<Thing>) this._bigList;
        HashSet<Thing> list1 = (HashSet<Thing>) null;
        if (this._objectsByType.TryGetValue(key, out list1))
        {
          HashSet<Thing> list2 = (HashSet<Thing>) null;
          return this._staticObjectsByType.TryGetValue(key, out list2) ? list1.Concat<Thing>((IEnumerable<Thing>) list2) : (IEnumerable<Thing>) list1;
        }
        return this._staticObjectsByType.ContainsKey(key) ? (IEnumerable<Thing>) this._staticObjectsByType[key] : Enumerable.Empty<Thing>();
      }
    }

    public IEnumerable<Thing> GetDynamicObjects(System.Type key)
    {
      if (key == typeof (Thing))
        return (IEnumerable<Thing>) this._bigList;
      HashSet<Thing> list = (HashSet<Thing>) null;
      return this._objectsByType.TryGetValue(key, out list) ? (IEnumerable<Thing>) list : Enumerable.Empty<Thing>();
    }

    public IEnumerable<Thing> GetStaticObjects(System.Type key)
    {
      if (key == typeof (Thing))
        return (IEnumerable<Thing>) this._bigList;
      HashSet<Thing> list = (HashSet<Thing>) null;
      return this._staticObjectsByType.TryGetValue(key, out list) ? (IEnumerable<Thing>) list : Enumerable.Empty<Thing>();
    }

    private IEnumerable<Thing> GetIslandObjects(System.Type t, Vec2 pos, float radiusSq)
    {
      IEnumerable<Thing> first = (IEnumerable<Thing>) new List<Thing>();
      foreach (CollisionIsland island in this._islands)
      {
        if ((double) (island.owner.position - pos).lengthSq - (double) radiusSq < (double) island.radiusCheckSquared)
          first = first.Concat<Thing>((IEnumerable<Thing>) island.things);
      }
      return first;
    }

    public bool HasStaticObjects(System.Type key) => key == typeof (Thing) || this._staticObjectsByType.ContainsKey(key);

    public int Count => this._bigList.Count;

    public void Add(Thing obj)
    {
      this._addThings.Add(obj);
      if (!this._autoRefresh)
        return;
      this.RefreshState();
    }

    public void AddRange(ObjectList list)
    {
      foreach (Thing thing in list)
        this.Add(thing);
    }

    public void Remove(Thing obj)
    {
      this._removeThings.Add(obj);
      if (!this._autoRefresh)
        return;
      this.RefreshState();
    }

    public void Clear()
    {
      this._bigList.Clear();
      this._addThings.Clear();
      this._objectsByType.Clear();
      this._staticObjectsByType.Clear();
      this._quadTree.Clear();
    }

    public bool Contains(Thing obj) => this._bigList.Contains(obj);

    public void RefreshState()
    {
      this._bigList.RemoveWhere((Predicate<Thing>) (x => x.removeFromLevel));
      foreach (Thing removeThing in this._removeThings)
      {
        removeThing.level = (Level) null;
        if (removeThing is IDontMove && this._useTree)
        {
          this.removeItem(this._staticObjectsByType, removeThing);
          this._quadTree.Remove(removeThing);
        }
        else
          this.removeItem(this._objectsByType, removeThing);
        this.objectsDirty = true;
      }
      this._removeThings.Clear();
      foreach (Thing addThing in this._addThings)
      {
        this._bigList.Add(addThing);
        addThing.level = Level.current;
        if (addThing is IDontMove && this._useTree)
        {
          this.addItem(this._staticObjectsByType, addThing);
          this._quadTree.Add(addThing);
        }
        else
          this.addItem(this._objectsByType, addThing);
        this.objectsDirty = true;
      }
      this._addThings.Clear();
    }

    private void addItem(MultiMap<System.Type, Thing, HashSet<Thing>> list, Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
      {
        if (list.Contains(key, obj))
          break;
        list.Add(key, obj);
      }
    }

    private void removeItem(MultiMap<System.Type, Thing, HashSet<Thing>> list, Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
        list.Remove(key, obj);
    }

    public IEnumerator<Thing> GetEnumerator() => (IEnumerator<Thing>) this._bigList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._bigList.GetEnumerator();

    public void Draw()
    {
      if (!DevConsole.showIslands)
        return;
      int num = 0;
      foreach (CollisionIsland island in this._islands)
      {
        Graphics.DrawCircle(island.owner.position, island.radiusCheck, Color.Red * 0.7f, depth: ((Depth) 0.9f), iterations: 64);
        Graphics.DrawCircle(island.owner.position, island.radius, Color.Blue * 0.3f, depth: ((Depth) 0.9f), iterations: 64);
        Graphics.DrawString(Convert.ToString(num), island.owner.position, Color.Red, (Depth) 1f);
        foreach (Thing thing in island.things)
        {
          if (thing != island.owner)
            Graphics.DrawString(Convert.ToString(num), thing.position, Color.White, (Depth) 1f);
        }
        ++num;
      }
    }
  }
}
