// Decompiled with JetBrains decompiler
// Type: DuckGame.MTSimpleSpriteBatchItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  internal class MTSimpleSpriteBatchItem
  {
    public VertexPositionColor vertexTL;
    public VertexPositionColor vertexTR;
    public VertexPositionColor vertexBL;
    public VertexPositionColor vertexBR;

    public MTSimpleSpriteBatchItem()
    {
      this.vertexTL = new VertexPositionColor();
      this.vertexTR = new VertexPositionColor();
      this.vertexBL = new VertexPositionColor();
      this.vertexBR = new VertexPositionColor();
    }

    public void Set(float x, float y, float w, float h, Microsoft.Xna.Framework.Color color)
    {
      this.vertexTL.Position.X = x;
      this.vertexTL.Position.Y = y;
      this.vertexTL.Position.Z = 0.0f;
      this.vertexTL.Color = color;
      this.vertexTR.Position.X = x + w;
      this.vertexTR.Position.Y = y;
      this.vertexTR.Position.Z = 0.0f;
      this.vertexTR.Color = color;
      this.vertexBL.Position.X = x;
      this.vertexBL.Position.Y = y + h;
      this.vertexBL.Position.Z = 0.0f;
      this.vertexBL.Color = color;
      this.vertexBR.Position.X = x + w;
      this.vertexBR.Position.Y = y + h;
      this.vertexBR.Position.Z = 0.0f;
      this.vertexBR.Color = color;
    }

    public void Set(
      float x,
      float y,
      float dx,
      float dy,
      float w,
      float h,
      float sin,
      float cos,
      Microsoft.Xna.Framework.Color color,
      Vec2 texCoordTL,
      Vec2 texCoordBR)
    {
      this.vertexTL.Position.X = (float) ((double) x + (double) dx * (double) cos - (double) dy * (double) sin);
      this.vertexTL.Position.Y = (float) ((double) y + (double) dx * (double) sin + (double) dy * (double) cos);
      this.vertexTL.Position.Z = 0.0f;
      this.vertexTL.Color = color;
      this.vertexTR.Position.X = (float) ((double) x + ((double) dx + (double) w) * (double) cos - (double) dy * (double) sin);
      this.vertexTR.Position.Y = (float) ((double) y + ((double) dx + (double) w) * (double) sin + (double) dy * (double) cos);
      this.vertexTR.Position.Z = 0.0f;
      this.vertexTR.Color = color;
      this.vertexBL.Position.X = (float) ((double) x + (double) dx * (double) cos - ((double) dy + (double) h) * (double) sin);
      this.vertexBL.Position.Y = (float) ((double) y + (double) dx * (double) sin + ((double) dy + (double) h) * (double) cos);
      this.vertexBL.Position.Z = 0.0f;
      this.vertexBL.Color = color;
      this.vertexBR.Position.X = (float) ((double) x + ((double) dx + (double) w) * (double) cos - ((double) dy + (double) h) * (double) sin);
      this.vertexBR.Position.Y = (float) ((double) y + ((double) dx + (double) w) * (double) sin + ((double) dy + (double) h) * (double) cos);
      this.vertexBR.Position.Z = 0.0f;
      this.vertexBR.Color = color;
    }
  }
}
