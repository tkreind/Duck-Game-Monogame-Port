﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PlasmaLayer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class PlasmaLayer : Layer
  {
    private Sprite _plasma;
    private MaterialPlasma _plasmaMaterial;
    private RenderTarget2D _target;
    public float alpha;

    public PlasmaLayer(string nameval, int depthval = 0)
      : base(nameval, depthval)
    {
      this._plasma = new Sprite("arcade/plasma");
      this.camera = new Camera();
      this._plasmaMaterial = new MaterialPlasma();
      this._target = new RenderTarget2D(320, 180);
      this.visible = false;
    }

    public override void Update()
    {
      this.visible = (double) this.alpha > 0.00999999977648258;
      if (!this.visible)
        return;
      this._plasmaMaterial.offset = 0.5f;
      this._plasmaMaterial.offset2 = 0.3f;
      this._plasmaMaterial.scroll += 0.0009f;
      this._plasmaMaterial.scroll2 += 0.0006f;
      this._plasmaMaterial.gradientOffset -= 0.0009f;
      this._plasmaMaterial.gradientOffset2 += 0.0007f;
      this._plasmaMaterial.color1 = Color.DeepPink;
      this._plasmaMaterial.color2 = Color.HotPink;
      this._plasmaMaterial.Apply();
      DuckGame.Graphics.currentLayer = (Layer) this;
      DuckGame.Graphics.SetRenderTarget(this._target);
      Viewport viewport = DuckGame.Graphics.viewport;
      DuckGame.Graphics.viewport = new Viewport(0, 0, 320, 180);
      this.Begin(false, false);
      this._plasma.depth = (Depth) -0.9f;
      this._plasma.alpha = 1f;
      DuckGame.Graphics.device.SamplerStates[0] = SamplerState.PointWrap;
      DuckGame.Graphics.Draw(this._plasma, -30f, -30f);
      this._batch.End();
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      DuckGame.Graphics.viewport = viewport;
      DuckGame.Graphics.screen = (MTSpriteBatch) null;
      DuckGame.Graphics.currentLayer = (Layer) null;
    }

    public override void Begin(bool transparent, bool isTargetDraw = false)
    {
      DuckGame.Graphics.ResetSpanAdjust();
      DuckGame.Graphics.ResetDepthBias();
      DuckGame.Graphics.screen = this._batch;
      this._batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.DepthRead, this._state, (MTEffect) (Material) this._plasmaMaterial, this.camera.getMatrix());
    }

    public override void Draw(bool transparent, bool isTargetDraw = false)
    {
      if (!this.visible)
        return;
      DuckGame.Graphics.ResetSpanAdjust();
      DuckGame.Graphics.ResetDepthBias();
      DuckGame.Graphics.screen = this._batch;
      this._batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, this._state, (MTEffect) null, this.camera.getMatrix());
      DuckGame.Graphics.Draw((Tex2D) this._target, Vec2.Zero, new Rectangle?(), Color.White * this.alpha, 0.0f, Vec2.Zero, new Vec2(1f, 1f), SpriteEffects.None, (Depth) 1f);
      this._batch.End();
      DuckGame.Graphics.screen = (MTSpriteBatch) null;
      DuckGame.Graphics.currentLayer = (Layer) null;
    }
  }
}
