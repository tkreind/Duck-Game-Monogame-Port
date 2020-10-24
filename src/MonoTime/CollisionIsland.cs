// Decompiled with JetBrains decompiler
// Type: DuckGame.CollisionIsland
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CollisionIsland
  {
    private HashSet<MaterialThing> _things = new HashSet<MaterialThing>();
    public MaterialThing owner;
    public float radius = 200f;
    public float radiusCheck = 220f;
    public float radiusSquared;
    public float radiusCheckSquared;
    public QuadTreeObjectList level;
    public bool willDie;

    public HashSet<MaterialThing> things => this._things;

    public CollisionIsland(MaterialThing own, QuadTreeObjectList lev)
    {
      this.radiusSquared = this.radius * this.radius;
      this.radiusCheckSquared = this.radiusCheck * this.radiusCheck;
      this.owner = own;
      this.level = lev;
      this.AddThing(own);
    }

    public void KillIsland() => this.level.RemoveIsland(this);

    public void AddThing(MaterialThing thing)
    {
      if (thing.island != null && thing.island != this)
        thing.island.RemoveThing(thing);
      thing.island = this;
      this._things.Add(thing);
    }

    public void RemoveThing(MaterialThing thing)
    {
      this._things.Remove(thing);
      thing.island = (CollisionIsland) null;
      if (thing != this.owner)
        return;
      this.KillIsland();
    }
  }
}
