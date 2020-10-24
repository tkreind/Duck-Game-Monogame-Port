// Decompiled with JetBrains decompiler
// Type: DuckGame.Organizer`2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class Organizer<T, T2>
  {
    private MultiMap<T, T2> _items = new MultiMap<T, T2>();
    private List<T2> _globalItems = new List<T2>();

    public void Add(T2 val) => this._globalItems.Add(val);

    public void Add(T2 val, T group) => this._items.Add(group, val);

    public void Clear()
    {
      this._items = new MultiMap<T, T2>();
      this._globalItems = new List<T2>();
    }

    public bool HasGroup(T group) => this._globalItems.Count > 0 || this._items.ContainsKey(group);

    public T2 GetRandom(T group)
    {
      if (this._items.ContainsKey(group))
        return this._items[group][Rando.Int(this._items[group].Count - 1)];
      return this._globalItems.Count > 0 ? this._globalItems[Rando.Int(this._globalItems.Count - 1)] : default (T2);
    }

    public List<T2> GetList(T group)
    {
      if (this._items.ContainsKey(group))
        return this._items[group];
      return this._globalItems.Count > 0 ? this._globalItems : (List<T2>) null;
    }
  }
}
