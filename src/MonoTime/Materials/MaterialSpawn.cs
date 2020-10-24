// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialSpawn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialSpawn : Material
  {
    public MaterialSpawn() => this._effect = Content.Load<MTEffect>("Shaders/wireframeTex");

    public override void Apply()
    {
      if (DuckGame.Graphics.device.Textures[0] != null)
      {
        Tex2D texture = (Tex2D) (DuckGame.Graphics.device.Textures[0] as Texture2D);
      }
      this.effect.effect.Parameters["screenCross"].SetValue(0.5f);
      this.effect.effect.Parameters["scanMul"].SetValue(1f);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
