// Decompiled with JetBrains decompiler
// Type: DuckGame.ObjectListImmediate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ObjectListImmediate : IEnumerable<Thing>, IEnumerable
  {
    private HashSet<Thing> _bigList = new HashSet<Thing>();
    private MultiMap<System.Type, Thing, HashSet<Thing>> _objectsByType = new MultiMap<System.Type, Thing, HashSet<Thing>>();

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
        return this._objectsByType.ContainsKey(key) ? (IEnumerable<Thing>) this._objectsByType[key] : Enumerable.Empty<Thing>();
      }
    }

    public int Count => this._bigList.Count;

    public void Add(Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
        this._objectsByType.Add(key, obj);
      this._bigList.Add(obj);
    }

    public void AddRange(ObjectListImmediate list)
    {
      foreach (Thing thing in list)
        this.Add(thing);
    }

    public void Remove(Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
        this._objectsByType.Remove(key, obj);
      this._bigList.Remove(obj);
    }

    public void Clear()
    {
      this._bigList.Clear();
      this._objectsByType.Clear();
    }

    public bool Contains(Thing obj) => this._bigList.Contains(obj);

    public IEnumerator<Thing> GetEnumerator() => (IEnumerator<Thing>) this._bigList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
