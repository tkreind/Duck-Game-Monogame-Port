// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialSunshine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialSunshine : Material
  {
    private RenderTarget2D _colorMap;

    public MaterialSunshine(RenderTarget2D col)
    {
      this._effect = Content.Load<MTEffect>("Shaders/sunshine");
      this._colorMap = col;
    }

    public override void Apply()
    {
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) (Tex2D) this._colorMap;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.PointClamp;
      DuckGame.Graphics.device.SamplerStates[0] = SamplerState.PointClamp;
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
