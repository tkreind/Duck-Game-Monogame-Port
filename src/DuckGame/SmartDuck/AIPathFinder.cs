﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.AIPathFinder
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class AIPathFinder
  {
    private Thing _followObject;
    private PathNodeLink _revert;
    private List<PathNodeLink> _path;

    public Thing followObject
    {
      get => this._followObject;
      set => this._followObject = value;
    }

    public AIPathFinder(Thing t = null) => this._followObject = t;

    public List<PathNodeLink> path
    {
      get
      {
        if (this._path == null)
          return (List<PathNodeLink>) null;
        return this._path.Count <= 0 ? (List<PathNodeLink>) null : this._path;
      }
    }

    public PathNodeLink target => this._path == null || this._path.Count == 0 ? (PathNodeLink) null : this._path[0];

    public PathNodeLink peek
    {
      get
      {
        if (this._path == null || this._path.Count == 0)
          return (PathNodeLink) null;
        return this._path.Count > 1 ? this._path[1] : this._path[0];
      }
    }

    public bool finished => this._path == null || this._path.Count == 0;

    public void AtTarget()
    {
      if (this._path == null || this._path.Count <= 0)
        return;
      this._revert = this._path[0];
      this._path.RemoveAt(0);
    }

    public void Revert()
    {
      if (this._path == null || this._revert == null)
        return;
      this._path.Insert(0, this._revert);
    }

    public void Refresh()
    {
      if (this._path == null)
        return;
      this.SetTarget(this._path.Last<PathNodeLink>());
    }

    public void SetTarget(Vec2 target)
    {
      if (this._followObject != null)
        this.SetTarget(this._followObject.position, target);
      else
        this.SetTarget(target, target);
    }

    public void SetTarget(PathNodeLink target)
    {
      if (this._followObject == null)
        return;
      this.SetTarget(this._followObject.position, target.owner.position);
    }

    public void SetTarget(Vec2 position, Vec2 target)
    {
      this._revert = (PathNodeLink) null;
      this._path = (List<PathNodeLink>) null;
      List<Thing> list = Level.current.things[typeof (PathNode)].ToList<Thing>();
      list.Sort((Comparison<Thing>) ((a, b) => (double) (a.position - position).lengthSq >= (double) (b.position - position).lengthSq ? 1 : -1));
      PathNode pathNode1 = (PathNode) null;
      foreach (Thing thing in list)
      {
        if (PathNode.LineIsClear(position, thing.position))
        {
          pathNode1 = thing as PathNode;
          break;
        }
      }
      if (pathNode1 == null)
        return;
      list.Sort((Comparison<Thing>) ((a, b) => (double) (a.position - target).lengthSq >= (double) (b.position - target).lengthSq ? 1 : -1));
      PathNode to = (PathNode) null;
      foreach (Thing thing in list)
      {
        if (PathNode.LineIsClear(target, thing.position))
        {
          to = thing as PathNode;
          break;
        }
      }
      if (to == null)
        return;
      AIPath path = pathNode1.GetPath(to);
      if (path == null || path.nodes.Count <= 0)
        return;
      bool flag = false;
      if (path.nodes.Count > 1 && PathNode.LineIsClear(position, path.nodes[1].position))
        flag = true;
      this._path = new List<PathNodeLink>();
      PathNode pathNode2 = (PathNode) null;
      foreach (PathNode node in path.nodes)
      {
        if (!flag)
        {
          Thing thing = (Thing) null;
          PathNodeLink pathNodeLink = new PathNodeLink();
          pathNodeLink.owner = thing;
          pathNodeLink.link = (Thing) node;
          pathNode2 = node;
          flag = true;
          this._path.Add(pathNodeLink);
        }
        else
        {
          if (pathNode2 != null)
            this._path.Add(pathNode2.GetLink(node));
          pathNode2 = node;
        }
      }
      Thing thing1 = (Thing) null;
      this._path.Add(new PathNodeLink()
      {
        owner = this._path.Count <= 0 ? thing1 : this._path.Last<PathNodeLink>().link,
        link = thing1
      });
    }
  }
}
