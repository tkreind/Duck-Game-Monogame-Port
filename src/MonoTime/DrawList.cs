// Decompiled with JetBrains decompiler
// Type: DuckGame.DrawList
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class DrawList
  {
    protected HashSet<Thing> _transparent = new HashSet<Thing>();
    protected HashSet<Thing> _opaque = new HashSet<Thing>();

    public void Add(Thing obj)
    {
      if (obj.opaque)
        this._opaque.Add(obj);
      else
        this._transparent.Add(obj);
    }

    public void Remove(Thing obj)
    {
      if (obj.opaque)
        this._opaque.Remove(obj);
      else
        this._transparent.Remove(obj);
    }

    public void Clear()
    {
      this._transparent.Clear();
      this._opaque.Clear();
    }
  }
}
