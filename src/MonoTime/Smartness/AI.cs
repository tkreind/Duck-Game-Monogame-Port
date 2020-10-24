// Decompiled with JetBrains decompiler
// Type: DuckGame.AI
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public static class AI
  {
    public static void InitializeLevelPaths()
    {
      foreach (PathNode pathNode in Level.current.things[typeof (PathNode)])
      {
        pathNode.UninitializeLinks();
        pathNode.InitializeLinks();
      }
      foreach (PathNode pathNode in Level.current.things[typeof (PathNode)])
        pathNode.InitializePaths();
    }

    private static Thing FilterBlocker(Thing b)
    {
      switch (b)
      {
        case Door _:
        case Window _:
          return (Thing) null;
        default:
          return b;
      }
    }

    public static T Nearest<T>(Vec2 position, Thing ignore = null)
    {
      PathNode pathNode = AI.NearestNode(position);
      if (pathNode == null)
        return default (T);
      System.Type key = typeof (T);
      float num = 99999.9f;
      T obj = default (T);
      foreach (Thing thing in Level.current.things[key])
      {
        if (thing != ignore)
        {
          PathNode to = AI.NearestNode(thing.position);
          if (to != null)
          {
            AIPath path = pathNode.GetPath(to);
            if (path != null && (double) path.length < (double) num)
            {
              num = path.length;
              obj = (T) (Object)thing;
            }
          }
        }
      }
      return obj;
    }

    public static bool CanReach(PathNode from, Thing what) => PathNode.CanTraverse(from.position, what.position, what);

    public static Thing Nearest(Vec2 position, List<Thing> things)
    {
      PathNode pathNode = AI.NearestNode(position);
      if (pathNode == null)
        return (Thing) null;
      float num = 99999.9f;
      Thing thing1 = (Thing) null;
      foreach (Thing thing2 in things)
      {
        PathNode to = AI.NearestNode(thing2.position, thing2);
        if (to != null)
        {
          AIPath path = pathNode.GetPath(to);
          if (path != null && path.nodes.Count > 0 && ((double) path.length < (double) num && AI.CanReach(path.nodes.Last<PathNode>(), thing2)))
          {
            num = path.length;
            thing1 = thing2;
          }
        }
      }
      return thing1;
    }

    public static PathNode NearestNode(Vec2 pos, Thing ignore = null)
    {
      List<Thing> list = Level.current.things[typeof (PathNode)].ToList<Thing>();
      list.Sort((Comparison<Thing>) ((a, b) => (double) (a.position - pos).lengthSq >= (double) (b.position - pos).lengthSq ? 1 : -1));
      PathNode pathNode = (PathNode) null;
      foreach (Thing thing in list)
      {
        if (PathNode.LineIsClear(pos, thing.position, ignore))
        {
          pathNode = thing as PathNode;
          break;
        }
      }
      return pathNode;
    }

    private static Thing GetHighest(List<IPlatform> things)
    {
      Thing thing1 = (Thing) null;
      foreach (IPlatform thing2 in things)
      {
        if (!(thing2 is PhysicsObject))
        {
          Thing thing3 = thing2 as Thing;
          if (thing1 == null || (double) thing3.y < (double) thing1.y)
            thing1 = thing3;
        }
      }
      return thing1;
    }

    private static Thing GetHighestNotGlass(List<IPlatform> things)
    {
      Thing thing1 = (Thing) null;
      foreach (IPlatform thing2 in things)
      {
        switch (thing2)
        {
          case PhysicsObject _:
          case Window _:
            continue;
          default:
            Thing thing3 = thing2 as Thing;
            if (thing1 == null || (double) thing3.y < (double) thing1.y)
            {
              thing1 = thing3;
              continue;
            }
            continue;
        }
      }
      return thing1;
    }

    public static List<PathNode> GetPath(PathNode start, PathNode end)
    {
      List<PathNode> pathNodeList1 = new List<PathNode>();
      List<PathNode> pathNodeList2 = new List<PathNode>();
      foreach (PathNode pathNode in Level.current.things[typeof (PathNode)])
      {
        pathNode.Reset();
        pathNodeList2.Add(pathNode);
      }
      List<PathNode> pathNodeList3 = new List<PathNode>();
      List<PathNode> pathNodeList4 = new List<PathNode>();
      pathNodeList3.Add(start);
      PathNode.CalculateNode(start, start, end);
      while (pathNodeList3.Count != 0)
      {
        PathNode parent = (PathNode) null;
        foreach (PathNode pathNode in pathNodeList3)
        {
          if (parent == null)
            parent = pathNode;
          else if ((double) pathNode.cost + (double) pathNode.heuristic < (double) parent.cost + (double) parent.heuristic)
            parent = pathNode;
        }
        if (parent != null)
        {
          if (parent == end)
          {
            PathNode pathNode1 = parent;
            pathNodeList1.Clear();
            for (; pathNode1 != null; pathNode1 = pathNode1.parent)
              pathNodeList1.Add(pathNode1);
            foreach (PathNode pathNode2 in pathNodeList2)
              pathNode2.Reset();
            pathNodeList1.Reverse();
            return pathNodeList1;
          }
          pathNodeList4.Add(parent);
          foreach (PathNodeLink link1 in parent.links)
          {
            if (link1.link is PathNode link && !pathNodeList4.Contains(link))
            {
              if (!pathNodeList3.Contains(link))
              {
                link.parent = parent;
                PathNode.CalculateNode(link, parent, end);
                pathNodeList3.Add(link);
              }
              else
              {
                float cost = PathNode.CalculateCost(link, parent);
                if ((double) cost < (double) link.cost)
                {
                  link.cost = cost;
                  link.parent = parent;
                }
              }
            }
          }
          pathNodeList3.Remove(parent);
        }
      }
      return pathNodeList1;
    }
  }
}
