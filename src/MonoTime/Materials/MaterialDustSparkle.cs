// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialDustSparkle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MaterialDustSparkle : Material
  {
    private Tex2D _cone;
    public Vec2 position;
    public Vec2 size;
    public float fade;

    public MaterialDustSparkle(Vec2 pos, Vec2 s, bool wide, bool lit)
    {
      this._effect = Content.Load<MTEffect>("Shaders/dustsparkle");
      if (!lit)
      {
        this._cone = Content.Load<Tex2D>("arcade/lightSphere");
        pos.y += 10f;
      }
      else
        this._cone = !wide ? Content.Load<Tex2D>("arcade/lightCone") : Content.Load<Tex2D>("arcade/bigLightCone");
      this.position = pos;
      this.size = s;
    }

    public override void Apply()
    {
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._cone;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.PointClamp;
      this.SetValue("topLeft", this.position);
      this.SetValue("size", this.size);
      this.SetValue("fade", Layer.Game.fade * this.fade);
      this.SetValue("viewMatrix", DuckGame.Graphics.screen.viewMatrix);
      this.SetValue("projMatrix", DuckGame.Graphics.screen.projMatrix);
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }
  }
}
