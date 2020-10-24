// Decompiled with JetBrains decompiler
// Type: DuckGame.RenderTarget2D
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class RenderTarget2D : Tex2D
  {
    public RenderTarget2D(int width, int height, bool depth, RenderTargetUsage usage)
      : base((Texture2D) new Microsoft.Xna.Framework.Graphics.RenderTarget2D(DuckGame.Graphics.device, width, height, false, SurfaceFormat.Color, depth ? DepthFormat.Depth24Stencil8 : DepthFormat.None, 0, usage), "__renderTarget")
    {
    }

    public RenderTarget2D(int width, int height, bool depth = false)
      : base((Texture2D) new Microsoft.Xna.Framework.Graphics.RenderTarget2D(DuckGame.Graphics.device, width, height, false, SurfaceFormat.Color, depth ? DepthFormat.Depth24Stencil8 : DepthFormat.None, 0, RenderTargetUsage.DiscardContents), "__renderTarget")
    {
    }
  }
}
