// Decompiled with JetBrains decompiler
// Type: DuckGame.RecorderFrameStateChange
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public struct RecorderFrameStateChange
  {
    public SpriteSortMode sortMode;
    public BlendState blendState;
    public SamplerState samplerState;
    public DepthStencilState depthStencilState;
    public RasterizerState rasterizerState;
    public short effectIndex;
    public Matrix camera;
    public int stateIndex;
    public Rectangle scissor;
  }
}
