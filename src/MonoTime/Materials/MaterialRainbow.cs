// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialRainbow
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialRainbow : Material
  {
    public float offset;
    public float offset2;

    public MaterialRainbow() => this._effect = Content.Load<MTEffect>("Shaders/rainbow");

    public override void Apply()
    {
      this._effect.effect.Parameters["offset"].SetValue(this.offset);
      this._effect.effect.Parameters["offset2"].SetValue(this.offset2);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
