// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialBurn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialBurn : Material
  {
    private Tex2D _burnTexture;
    private float _burnVal;

    public float burnVal
    {
      get => this._burnVal;
      set => this._burnVal = value;
    }

    public MaterialBurn(float burnVal = 0.0f)
    {
      this._effect = Content.Load<MTEffect>("Shaders/burn");
      this._burnTexture = Content.Load<Tex2D>("burn");
      this._burnVal = burnVal;
    }

    public override void Apply()
    {
      Tex2D texture = (Tex2D) (DuckGame.Graphics.device.Textures[0] as Texture2D);
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._burnTexture;
      this.SetValue("width", texture.frameWidth / (float) texture.width);
      this.SetValue("height", texture.frameHeight / (float) texture.height);
      this.SetValue("burn", this._burnVal);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
