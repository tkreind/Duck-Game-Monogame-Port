// Decompiled with JetBrains decompiler
// Type: DuckGame.CollisionTree
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame
{
  public class CollisionTree
  {
    private Vec2 _position;
    private float _width;
    private CollisionTree[] _children;
    private SpriteFont _font;
    private List<Thing> _objects = new List<Thing>();
    private int _depth;

    public Vec2 position => this._position;

    public CollisionTree(float xpos, float ypos, float wval, int divisions)
    {
      this._depth = divisions;
      this._position = new Vec2(xpos, ypos);
      this._width = wval;
      this._font = Content.Load<SpriteFont>("font_SuperNew");
      if (this._depth <= 0)
        return;
      this._children = new CollisionTree[4];
      this._children[0] = new CollisionTree(this._position.x, this._position.y, this._width * 0.5f, this._depth - 1);
      this._children[1] = new CollisionTree(this._position.x + this._width * 0.5f, this._position.y, this._width * 0.5f, this._depth - 1);
      this._children[2] = new CollisionTree(this._position.x, this._position.y + this._width * 0.5f, this._width * 0.5f, this._depth - 1);
      this._children[3] = new CollisionTree(this._position.x + this._width * 0.5f, this._position.y + this._width * 0.5f, this._width * 0.5f, this._depth - 1);
    }

    public void Add(Thing t)
    {
    }

    public void Draw()
    {
    }
  }
}
