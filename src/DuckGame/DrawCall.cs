// Decompiled with JetBrains decompiler
// Type: DuckGame.DrawCall
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public struct DrawCall
  {
    public Tex2D texture;
    public Vec2 position;
    public Rectangle? sourceRect;
    public Color color;
    public float rotation;
    public Vec2 origin;
    public Vec2 scale;
    public SpriteEffects effects;
    public float depth;
    public Material material;
  }
}
