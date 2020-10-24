// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialAlbum
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialAlbum : Material
  {
    private Tex2D _albumTexture;

    public MaterialAlbum()
    {
      this._effect = Content.Load<MTEffect>("Shaders/album");
      this._albumTexture = Content.Load<Tex2D>("playBookPageOffset");
    }

    public override void Apply()
    {
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._albumTexture;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.PointClamp;
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
