// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialGold
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialGold : Material
  {
    private Tex2D _goldTexture;
    private Thing _thing;

    public MaterialGold(Thing t)
    {
      this._effect = Content.Load<MTEffect>("Shaders/gold");
      this._goldTexture = Content.Load<Tex2D>("bigGold");
      this._thing = t;
    }

    public override void Apply()
    {
      if (DuckGame.Graphics.device.Textures[0] != null)
      {
        Tex2D texture = (Tex2D) (DuckGame.Graphics.device.Textures[0] as Texture2D);
        this.SetValue("width", texture.frameWidth / (float) texture.width);
        this.SetValue("height", texture.frameHeight / (float) texture.height);
        this.SetValue("xpos", this._thing.x);
        this.SetValue("ypos", this._thing.y);
      }
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._goldTexture;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.PointWrap;
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
