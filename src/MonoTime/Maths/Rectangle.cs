// Decompiled with JetBrains decompiler
// Type: DuckGame.Rectangle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [Serializable]
  public struct Rectangle
  {
    public float height;
    public float width;
    public float x;
    public float y;

    public float Top => this.y;

    public float Bottom => this.y + this.height;

    public float Left => this.x;

    public float Right => this.x + this.width;

    public Rectangle(float x, float y, float width, float height)
    {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
    }

    public static implicit operator Microsoft.Xna.Framework.Rectangle(Rectangle r) => new Microsoft.Xna.Framework.Rectangle((int) r.x, (int) r.y, (int) r.width, (int) r.height);

    public static implicit operator Rectangle(Microsoft.Xna.Framework.Rectangle r) => new Rectangle((float) r.X, (float) r.Y, (float) r.Width, (float) r.Height);

    public bool Contains(Vec2 position) => (double) position.x >= (double) this.x && (double) position.y >= (double) this.y && (double) position.x <= (double) this.x + (double) this.width && (double) position.y <= (double) this.y + (double) this.height;
  }
}
