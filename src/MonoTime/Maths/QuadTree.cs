// Decompiled with JetBrains decompiler
// Type: DuckGame.QuadTree
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class QuadTree
  {
    private QuadTree _parent;
    private int _depth;
    private List<QuadTree> _children = new List<QuadTree>();
    private ObjectListImmediate _objects = new ObjectListImmediate();
    private List<Vec2> _corners = new List<Vec2>();
    private Vec2 _position;
    private Vec2 _center;
    private float _width;
    private float _halfWidth;
    private int _max;
    private Rectangle _rectangle;
    private bool _split;

    public Rectangle rectangle => this._rectangle;

    public QuadTree(int depth, Vec2 position, float width, int max = 4, QuadTree parent = null)
    {
      this._depth = depth;
      this._position = position;
      this._width = width;
      this._halfWidth = this._width / 2f;
      this._max = max;
      this._parent = parent;
      this._center = this._position + new Vec2(this._halfWidth, this._halfWidth);
      this._rectangle = new Rectangle((float) (int) position.x, (float) (int) position.y, (float) (int) width, (float) (int) width);
      if (this._depth == 0)
        return;
      for (int index = 0; index < 4; ++index)
      {
        this._corners.Add(new Vec2());
        Vec2 position1 = new Vec2(this._position);
        if (index == 1 || index == 3)
          position1.x += this._halfWidth;
        if (index == 2 || index == 3)
          position1.y += this._halfWidth;
        this._children.Add(new QuadTree(this._depth - 1, position1, this._halfWidth, this._max, this));
      }
    }

    public T CheckPoint<T>(Vec2 pos, Thing ignore = null, Layer layer = null)
    {
      QuadTree quadTree = this;
      System.Type key = typeof (T);
      int index;
      for (; quadTree != null; quadTree = quadTree._children[index])
      {
        if (!quadTree._split)
        {
          foreach (Thing t in quadTree._objects[key])
          {
            if (t != ignore && (layer == null || layer == t.layer) && (t.ghostType != (ushort) 0 && Collision.Point(pos, t)))
              return (T) t;
          }
          return default (T);
        }
        if ((double) pos.x > (double) quadTree._position.x + (double) quadTree._width)
          return default (T);
        if ((double) pos.y > (double) quadTree._position.y + (double) quadTree._width)
          return default (T);
        index = 0;
        if ((double) pos.x > (double) quadTree._position.x + (double) quadTree._halfWidth)
          index = 1;
        if ((double) pos.y > (double) quadTree._position.y + (double) quadTree._halfWidth)
          index += 2;
      }
      return default (T);
    }

    public T CheckPointPlacementLayer<T>(Vec2 pos, Thing ignore = null, Layer layer = null)
    {
      QuadTree quadTree = this;
      System.Type key = typeof (T);
      int index;
      for (; quadTree != null; quadTree = quadTree._children[index])
      {
        if (!quadTree._split)
        {
          foreach (Thing t in quadTree._objects[key])
          {
            if (t != ignore && (layer == null || layer == t.placementLayer) && (t.ghostType != (ushort) 0 && Collision.Point(pos, t)))
              return (T) t;
          }
          return default (T);
        }
        if ((double) pos.x > (double) quadTree._position.x + (double) quadTree._width)
          return default (T);
        if ((double) pos.y > (double) quadTree._position.y + (double) quadTree._width)
          return default (T);
        index = 0;
        if ((double) pos.x > (double) quadTree._position.x + (double) quadTree._halfWidth)
          index = 1;
        if ((double) pos.y > (double) quadTree._position.y + (double) quadTree._halfWidth)
          index += 2;
      }
      return default (T);
    }

    public T CheckLine<T>(Vec2 p1, Vec2 p2, Thing ignore = null, Layer layer = null)
    {
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (t != ignore && (layer == null || layer == t.layer) && (t.ghostType != (ushort) 0 && Collision.Line(p1, p2, t)))
            return (T) t;
        }
        return default (T);
      }
      T obj = default (T);
      for (int index = 0; index < 4; ++index)
      {
        if (Collision.Line(p1, p2, this._children[index].rectangle))
          obj = this._children[index].CheckLine<T>(p1, p2, ignore, layer);
        if ((object) obj != null)
          return obj;
      }
      return default (T);
    }

    public List<T> CheckLineAll<T>(Vec2 p1, Vec2 p2, Thing ignore = null, Layer layer = null)
    {
      List<T> objList1 = new List<T>();
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (t != ignore && (layer == null || layer == t.layer) && (t.ghostType != (ushort) 0 && Collision.Line(p1, p2, t)))
            objList1.Add((T) t);
        }
        return objList1;
      }
      for (int index = 0; index < 4; ++index)
      {
        if (Collision.Line(p1, p2, this._children[index].rectangle))
        {
          List<T> objList2 = this._children[index].CheckLineAll<T>(p1, p2, ignore, layer);
          objList1.AddRange((IEnumerable<T>) objList2);
        }
      }
      return objList1;
    }

    public T CheckLinePoint<T>(Vec2 p1, Vec2 p2, out Vec2 hit, Thing ignore = null, Layer layer = null)
    {
      hit = new Vec2();
      if (!this._split)
      {
        foreach (Thing thing in this._objects[typeof (T)])
        {
          if (thing != ignore && (layer == null || layer == thing.layer) && thing.ghostType != (ushort) 0)
          {
            Vec2 vec2 = Collision.LinePoint(p1, p2, thing);
            if (vec2 != Vec2.Zero)
            {
              hit = vec2;
              return (T) thing;
            }
          }
        }
        return default (T);
      }
      T obj = default (T);
      for (int index = 0; index < 4; ++index)
      {
        if (Collision.Line(p1, p2, this._children[index].rectangle))
          obj = this._children[index].CheckLinePoint<T>(p1, p2, out hit, ignore, layer);
        if ((object) obj != null)
          return obj;
      }
      return default (T);
    }

    public T CheckRectangle<T>(Vec2 p1, Vec2 p2, Thing ignore = null, Layer layer = null)
    {
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (t != ignore && (layer == null || layer == t.layer) && (t.ghostType != (ushort) 0 && Collision.Rect(p1, p2, t)))
            return (T) t;
        }
        return default (T);
      }
      T obj = default (T);
      for (int index = 0; index < 4; ++index)
      {
        if (Collision.Rect(p1, p2, this._children[index].rectangle))
          obj = this._children[index].CheckRectangle<T>(p1, p2, ignore, layer);
        if ((object) obj != null)
          return obj;
      }
      return default (T);
    }

    public void CheckRectangleAll<T>(Vec2 p1, Vec2 p2, List<object> outList)
    {
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (t.ghostType != (ushort) 0 && Collision.Rect(p1, p2, t))
            outList.Add((object) (T) t);
        }
      }
      else
      {
        for (int index = 0; index < 4; ++index)
        {
          QuadTree child = this._children[index];
          if (Collision.Rect(p1, p2, this._children[index].rectangle))
            this._children[index].CheckRectangleAll<T>(p1, p2, outList);
        }
      }
    }

    public T CheckCircle<T>(Vec2 p1, float radius, Thing ignore = null, Layer layer = null)
    {
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (t != ignore && (layer == null || layer == t.layer) && (t.ghostType != (ushort) 0 && Collision.Circle(p1, radius, t)))
            return (T) t;
        }
        return default (T);
      }
      T obj = default (T);
      for (int index = 0; index < 4; ++index)
      {
        if (Collision.Circle(p1, radius, this._children[index].rectangle))
          obj = this._children[index].CheckCircle<T>(p1, radius, ignore, layer);
        if ((object) obj != null)
          return obj;
      }
      return default (T);
    }

    public void CheckCircleAll<T>(Vec2 p1, float radius, List<object> outList)
    {
      if (!this._split)
      {
        foreach (Thing t in this._objects[typeof (T)])
        {
          if (Collision.Circle(p1, radius, t))
            outList.Add((object) (T) t);
        }
      }
      else
      {
        for (int index = 0; index < 4; ++index)
        {
          if (Collision.Circle(p1, radius, this._children[index].rectangle))
            this._children[index].CheckCircleAll<T>(p1, radius, outList);
        }
      }
    }

    private void GetUniqueChildren(List<Thing> things)
    {
      foreach (Thing thing in this._objects)
      {
        if (!things.Contains(thing))
          things.Add(thing);
      }
      if (!this._split)
        return;
      foreach (QuadTree child in this._children)
        child.GetUniqueChildren(things);
    }

    private int Count() => this._objects.Count;

    private void Divide()
    {
      if (this._split || this._depth == 0)
        return;
      this._split = true;
      foreach (Thing t in this._objects)
        this.Add(t);
    }

    private void Combine()
    {
      if (!this._split)
        return;
      foreach (QuadTree child in this._children)
      {
        child.Combine();
        this._objects.AddRange(child._objects);
        child._objects.Clear();
      }
      this._split = false;
    }

    public void Add(Thing t)
    {
      this._objects.Add(t);
      if (!this._split)
      {
        if (this._objects.Count <= this._max || this._depth <= 0)
          return;
        this.Divide();
      }
      else
      {
        Rectangle rectangle = t.rectangle;
        foreach (QuadTree child in this._children)
        {
          if (Collision.Rect(child.rectangle, rectangle))
            child.Add(t);
        }
      }
    }

    public void Remove(Thing t)
    {
      this._objects.Remove(t);
      if (!this._split)
        return;
      Rectangle rectangle = t.rectangle;
      foreach (QuadTree child in this._children)
      {
        if (Collision.Rect(child.rectangle, rectangle))
          child.Remove(t);
      }
      if (this._objects.Count > this._max)
        return;
      this.Combine();
    }

    public void Draw()
    {
      Graphics.DrawRect(this._position, this._position + new Vec2(this._width, this._width), Color.Red, (Depth) 1f, false);
      if (!this._split)
        Graphics.DrawString(Change.ToString((object) this._objects.Count), this._position + new Vec2(2f, 2f), Color.White, (Depth) 0.9f);
      if (this._depth == 0 || !this._split)
        return;
      foreach (QuadTree child in this._children)
        child.Draw();
    }

    public void Clear()
    {
      foreach (QuadTree child in this._children)
        child.Clear();
      this._objects.Clear();
    }
  }
}
