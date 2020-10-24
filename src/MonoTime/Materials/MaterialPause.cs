// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialPause
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialPause : Material
  {
    private Tex2D _watermark;
    private float _fade;
    private float _scrollX;
    private float _scrollY;
    private float _rot;
    private float _rot2;
    public float dim = 0.6f;

    public float fade
    {
      get => this._fade;
      set => this._fade = value;
    }

    public MaterialPause()
    {
      this._effect = Content.Load<MTEffect>("Shaders/pause");
      this._watermark = Content.Load<Tex2D>("looptex");
    }

    public override void Apply()
    {
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._watermark;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.PointClamp;
      this.SetValue("fade", this._fade);
      this.SetValue("dim", this.dim);
      this.SetValue("scrollX", this._scrollX);
      this.SetValue("scrollY", this._scrollY);
      float num = 0.0002f;
      this._rot += num;
      this._rot2 += num * 1.777778f;
      this._scrollX = this._rot % 0.1421875f;
      this._scrollY = (float) (-(double) this._rot2 % 0.252777010202408);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
