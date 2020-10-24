// Decompiled with JetBrains decompiler
// Type: DuckGame.ObjectList
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ObjectList : IEnumerable<Thing>, IEnumerable
  {
    private List<Thing> _bigList = new List<Thing>();
    private MultiMap<string, Thing> _objects = new MultiMap<string, Thing>();
    private MultiMap<string, Thing> _addThings = new MultiMap<string, Thing>();
    private MultiMap<string, Thing> _removeThings = new MultiMap<string, Thing>();
    private MultiMap<System.Type, Thing> _objectsByType = new MultiMap<System.Type, Thing>();
    private bool _autoRefresh;

    public ObjectList(bool auto = false) => this._autoRefresh = auto;

    public List<Thing> ToList()
    {
      List<Thing> thingList1 = new List<Thing>();
      foreach (List<Thing> thingList2 in this._objects.Values)
        thingList1.AddRange((IEnumerable<Thing>) thingList2);
      return thingList1;
    }

    public IEnumerable<Thing> this[System.Type key] => this._objectsByType.ContainsKey(key) ? (IEnumerable<Thing>) this._objectsByType[key] : Enumerable.Empty<Thing>();

    public IEnumerable<Thing> this[string key] => this._objects.ContainsKey(key) ? (IEnumerable<Thing>) this._objects[key] : Enumerable.Empty<Thing>();

    public Thing this[int key] => key < this._bigList.Count ? this._bigList[key] : (Thing) null;

    public int Count => this._bigList.Count;

    public void Add(Thing obj)
    {
      this.removeItem(this._removeThings, obj);
      this.addItem(this._addThings, obj);
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
      this.addItem(this._removeThings, obj);
      this.removeItem(this._addThings, obj);
      if (!this._autoRefresh)
        return;
      this.RefreshState();
    }

    public void Clear()
    {
      for (int key = 0; key < this.Count; ++key)
        this.Remove(this[key]);
    }

    public bool Contains(Thing obj) => this._bigList.Contains(obj);

    public void RefreshState()
    {
      foreach (KeyValuePair<string, List<Thing>> removeThing in (MultiMap<string, Thing, List<Thing>>) this._removeThings)
      {
        foreach (Thing thing in removeThing.Value)
        {
          this._bigList.Remove(thing);
          this.removeItem(this._objects, thing);
          this.removeItem(this._objectsByType, thing);
        }
      }
      this._removeThings.Clear();
      foreach (KeyValuePair<string, List<Thing>> addThing in (MultiMap<string, Thing, List<Thing>>) this._addThings)
      {
        foreach (Thing thing in addThing.Value)
        {
          this._bigList.Add(thing);
          this.addItem(this._objects, thing);
          this.addItem(this._objectsByType, thing);
        }
      }
      this._addThings.Clear();
    }

    private void addItem(MultiMap<string, Thing> list, Thing obj)
    {
      if (list.Contains(obj.type, obj))
        return;
      list.Add(obj.type, obj);
    }

    private void removeItem(MultiMap<string, Thing> list, Thing obj) => list.Remove(obj.type, obj);

    private void addItem(MultiMap<System.Type, Thing> list, Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
      {
        if (list.Contains(key, obj))
          break;
        list.Add(key, obj);
      }
    }

    private void removeItem(MultiMap<System.Type, Thing> list, Thing obj)
    {
      foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
        list.Remove(key, obj);
    }

    public IEnumerator<Thing> GetEnumerator() => (IEnumerator<Thing>) this._bigList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
