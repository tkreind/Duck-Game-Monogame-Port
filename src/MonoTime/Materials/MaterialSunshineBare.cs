// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialSunshineBare
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialSunshineBare : Material
  {
    public MaterialSunshineBare() => this._effect = Content.Load<MTEffect>("Shaders/baresunshine");

    public override void Apply()
    {
      DuckGame.Graphics.device.SamplerStates[0] = SamplerState.LinearClamp;
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
