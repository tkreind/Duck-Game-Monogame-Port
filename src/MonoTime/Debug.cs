// Decompiled with JetBrains decompiler
// Type: DuckGame.Debug
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace DuckGame
{
  public static class Debug
  {
    private static Texture2D _blank;

    [Conditional("DEBUG")]
    public static void Initialize()
    {
      MonoMain.loadMessage = "Loading Debug Data";
      Debug._blank = new Texture2D(DuckGame.Graphics.device, 1, 1, false, SurfaceFormat.Color);
      Debug._blank.SetData<Color>(new Color[1]
      {
        Color.White
      });
    }

    [Conditional("DEBUG")]
    public static void DrawLine(Vec2 p1, Vec2 p2, Color col, float width = 1f)
    {
      p1 = new Vec2((float) (int) p1.x, (float) (int) p1.y);
      p2 = new Vec2((float) (int) p2.x, (float) (int) p2.y);
      float rotation = (float) Math.Atan2((double) p2.y - (double) p1.y, (double) p2.x - (double) p1.x);
      float length = (p1 - p2).length;
      DuckGame.Graphics.Draw((Tex2D) Debug._blank, p1, new Rectangle?(), col, rotation, Vec2.Zero, new Vec2(length, width), SpriteEffects.None, (Depth) 1f);
    }

    [Conditional("DEBUG")]
    public static void DrawRect(Vec2 p1, Vec2 p2, Color col) => DuckGame.Graphics.Draw((Tex2D) Debug._blank, p1, new Rectangle?(), col, 0.0f, Vec2.Zero, new Vec2((float) -((double) p1.x - (double) p2.x), (float) -((double) p1.y - (double) p2.y)), SpriteEffects.None, (Depth) 1f);
  }
}
