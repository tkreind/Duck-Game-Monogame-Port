// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialFlatColor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialFlatColor : Material
  {
    public MaterialFlatColor() => this._effect = Content.Load<MTEffect>("Shaders/flatColor");

    public override void Apply()
    {
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
