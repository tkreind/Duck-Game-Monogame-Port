// Decompiled with JetBrains decompiler
// Type: DuckGame.GeometryItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckGame
{
  public class GeometryItem
  {
    public bool temporary;
    public float depth;
    public VertexPositionColor[] vertices;
    public int length;
    public int size = 512;
    public Material material;

    public GeometryItem() => this.vertices = new VertexPositionColor[this.size];

    public void Clear() => this.length = 0;

    public void AddTriangle(Vec2 p1, Vec2 p2, Vec2 p3, Color c)
    {
      if (this.length + 3 >= this.size)
      {
        VertexPositionColor[] vertexPositionColorArray = new VertexPositionColor[this.size * 2];
        this.vertices.CopyTo((Array) vertexPositionColorArray, 0);
        this.vertices = vertexPositionColorArray;
        this.size *= 2;
      }
      this.vertices[this.length].Position.X = p1.x;
      this.vertices[this.length].Position.Y = p1.y;
      this.vertices[this.length].Position.Z = this.depth;
      this.vertices[this.length].Color = (Microsoft.Xna.Framework.Color) c;
      this.vertices[this.length + 1].Position.X = p2.x;
      this.vertices[this.length + 1].Position.Y = p2.y;
      this.vertices[this.length + 1].Position.Z = this.depth;
      this.vertices[this.length + 1].Color = (Microsoft.Xna.Framework.Color) c;
      this.vertices[this.length + 2].Position.X = p3.x;
      this.vertices[this.length + 2].Position.Y = p3.y;
      this.vertices[this.length + 2].Position.Z = this.depth;
      this.vertices[this.length + 2].Color = (Microsoft.Xna.Framework.Color) c;
      this.length += 3;
    }

    public void AddTriangle(Vec2 p1, Vec2 p2, Vec2 p3, Color c, Color c2, Color c3)
    {
      if (this.length + 3 >= this.size)
      {
        VertexPositionColor[] vertexPositionColorArray = new VertexPositionColor[this.size * 2];
        this.vertices.CopyTo((Array) vertexPositionColorArray, 0);
        this.vertices = vertexPositionColorArray;
        this.size *= 2;
      }
      this.vertices[this.length].Position.X = p1.x;
      this.vertices[this.length].Position.Y = p1.y;
      this.vertices[this.length].Position.Z = this.depth;
      this.vertices[this.length].Color = (Microsoft.Xna.Framework.Color) c;
      this.vertices[this.length + 1].Position.X = p2.x;
      this.vertices[this.length + 1].Position.Y = p2.y;
      this.vertices[this.length + 1].Position.Z = this.depth;
      this.vertices[this.length + 1].Color = (Microsoft.Xna.Framework.Color) c2;
      this.vertices[this.length + 2].Position.X = p3.x;
      this.vertices[this.length + 2].Position.Y = p3.y;
      this.vertices[this.length + 2].Position.Z = this.depth;
      this.vertices[this.length + 2].Color = (Microsoft.Xna.Framework.Color) c3;
      this.length += 3;
    }
  }
}
