﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ConsoleScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class ConsoleScreen : Thing
  {
    private Material _lcdMaterial;
    private RenderTarget2D _realScreenTarget;
    private RenderTarget2D _bloomTarget;
    private RenderTarget2D _finalTarget;
    private MTSpriteBatch _batch;
    private Material _blurMaterial;
    public float _darken = 1f;
    private HatSelector _selector;
    public bool _flashTransition;
    private Viewport _oldViewport;

    private RenderTarget2D _screenTarget
    {
      get => (double) this._selector._roomEditor.fade > 0.0 ? this._finalTarget : this._realScreenTarget;
      set => this._realScreenTarget = value;
    }

    public RenderTarget2D target => this._finalTarget;

    public float darken => this._darken;

    public ConsoleScreen(float xpos, float ypos, HatSelector s)
      : base(xpos, ypos)
    {
      this._lcdMaterial = new Material("Shaders/lcd");
      this._blurMaterial = new Material("Shaders/lcdBlur");
      this._screenTarget = new RenderTarget2D(134, 86);
      this._bloomTarget = new RenderTarget2D(134, 86);
      this._finalTarget = new RenderTarget2D(536, 344);
      this._batch = new MTSpriteBatch(DuckGame.Graphics.device);
      this._selector = s;
    }

    public bool transitioning => this._flashTransition;

    public void DoFlashTransition() => this._flashTransition = true;

    public void BeginDraw()
    {
      this._oldViewport = DuckGame.Graphics.viewport;
      DuckGame.Graphics.SetRenderTarget(this._screenTarget);
      DuckGame.Graphics.viewport = new Viewport(0, 0, this._screenTarget.width, this._screenTarget.height);
      DuckGame.Graphics.Clear(Color.Black);
      DuckGame.Graphics.ResetSpanAdjust();
      DuckGame.Graphics.ResetDepthBias();
      DuckGame.Graphics.screen = this._batch;
      Camera camera = new Camera(3f, 4f, (float) this._screenTarget.width, (float) this._screenTarget.height);
      if ((double) this._selector._roomEditor.fade > 0.0)
        camera = new Camera(3f, 4f, (float) (this._screenTarget.width / 4), (float) (this._screenTarget.height / 4));
      this._batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, (MTEffect) null, camera.getMatrix());
    }

    public void EndDraw()
    {
      this._batch.End();
      if (!this._flashTransition)
      {
        Camera camera1 = new Camera(0.0f, 0.0f, (float) this._screenTarget.width, (float) this._screenTarget.height);
        if ((double) this._selector._roomEditor.fade <= 0.0)
        {
          DuckGame.Graphics.SetRenderTarget(this._bloomTarget);
          DuckGame.Graphics.viewport = new Viewport(0, 0, this._bloomTarget.width, this._bloomTarget.height);
          DuckGame.Graphics.ResetSpanAdjust();
          DuckGame.Graphics.ResetDepthBias();
          DuckGame.Graphics.screen = this._batch;
          this._batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, (MTEffect) this._blurMaterial, camera1.getMatrix());
          DuckGame.Graphics.Draw((Tex2D) this._screenTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(1f, 1f), SpriteEffects.None, new Depth(1f));
          this._batch.End();
          DuckGame.Graphics.SetRenderTarget(this._finalTarget);
          DuckGame.Graphics.viewport = new Viewport(0, 0, this._finalTarget.width, this._finalTarget.height);
          Camera camera2 = new Camera(0.0f, 0.0f, (float) this._screenTarget.width, (float) this._screenTarget.height);
          this._batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, (MTEffect) this._lcdMaterial, camera2.getMatrix());
          DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) (Tex2D) this._bloomTarget;
          DuckGame.Graphics.device.SamplerStates[1] = SamplerState.LinearClamp;
          DuckGame.Graphics.Draw((Tex2D) this._screenTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(1f, 1f), SpriteEffects.None, new Depth(0.82f));
          DuckGame.Graphics.material = (Material) null;
          this._batch.End();
        }
      }
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      DuckGame.Graphics.viewport = this._oldViewport;
      DuckGame.Graphics.screen = (MTSpriteBatch) null;
      DuckGame.Graphics.currentLayer = (Layer) null;
    }

    public override void Update()
    {
      if (this._flashTransition)
      {
        this._darken -= 0.2f;
        if ((double) this._darken < 0.200000002980232)
          this._flashTransition = false;
      }
      if (this._flashTransition)
        return;
      if ((double) this._darken < 1.0)
        this._darken += 0.2f;
      else
        this._darken = 1f;
    }
  }
}
