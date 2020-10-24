// Decompiled with JetBrains decompiler
// Type: DuckGame.LayerCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class LayerCore
  {
    private const string NameParallax = "PARALLAX";
    private const string NameVirtual = "VIRTUAL";
    private const string NameBackground = "BACKGROUND";
    private const string NameGame = "GAME";
    private const string NameBlocks = "BLOCKS";
    private const string NameGlow = "GLOW";
    private const string NameLighting = "LIGHTING";
    private const string NameLighting2 = "LIGHTING2";
    private const string NameForeground = "FOREGROUND";
    private const string NameHUD = "HUD";
    private const string NameConsole = "CONSOLE";
    public bool doVirtualEffect;
    public Layer _parallax;
    public Layer _virtual;
    public Layer _background;
    public Layer _game;
    public Layer _blocks;
    public Layer _glow;
    public Layer _lighting;
    public Layer _lighting2;
    public Layer _foreground;
    public Layer _hud;
    public Layer _console;
    private List<Layer> _layers = new List<Layer>();
    private List<Layer> _extraLayers = new List<Layer>();
    private List<Layer> _hybridList = new List<Layer>();
    public MTEffect _basicEffectFadeAdd;
    public MTEffect _basicEffectAdd;
    public MTEffect _basicEffectFade;
    public MTEffect _basicEffect;
    public MTEffect _basicWireframeEffect;
    public MTEffect _basicWireframeEffectTex;
    public MTEffect _itemSpawnEffect;
    public bool basicWireframeTex;
    private int _lastDrawIndexCount;

    public bool allVisible
    {
      set
      {
        foreach (Layer layer in this._layers)
          layer.visible = value;
        foreach (Layer extraLayer in this._extraLayers)
          extraLayer.visible = value;
      }
    }

    public MTEffect basicWireframeEffect => !this.basicWireframeTex ? this._basicWireframeEffect : this._basicWireframeEffectTex;

    public MTEffect basicLayerEffect => this._basicEffectFadeAdd;

    public bool IsBasicLayerEffect(MTEffect e)
    {
      if (e == null)
        return false;
      return (int) e.effectIndex == (int) this._basicEffect.effectIndex || (int) e.effectIndex == (int) this._basicEffectAdd.effectIndex || (int) e.effectIndex == (int) this._basicEffectFade.effectIndex || (int) e.effectIndex == (int) this._basicEffectFadeAdd.effectIndex;
    }

    public void InitializeLayers()
    {
      Layer.lightingTwoPointOh = false;
      this._layers.Add(new Layer("PARALLAX", 100));
      this._parallax = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("VIRTUAL", 95));
      this._virtual = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("BACKGROUND", 90));
      this._background = this._layers[this._layers.Count - 1];
      this._background.enableCulling = true;
      this._layers.Add(new Layer("GAME"));
      this._game = this._layers[this._layers.Count - 1];
      this._game.enableCulling = false;
      this._layers.Add(new Layer("BLOCKS", -18));
      this._blocks = this._layers[this._layers.Count - 1];
      this._blocks.enableCulling = true;
      this._layers.Add(new Layer("FOREGROUND", -80));
      this._foreground = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("HUD", -90));
      this._hud = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("CONSOLE", -100, new Camera((float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height)));
      this._console = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("GLOW", -20));
      this._glow = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("LIGHTING", Layer.lightingTwoPointOh ? -19 : -10, targetLayer: true, targetSize: new Vec2((float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height)));
      this._lighting = this._layers[this._layers.Count - 1];
      this._layers.Add(new Layer("LIGHTING2", -15, targetLayer: true, targetSize: new Vec2((float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height)));
      this._lighting2 = this._layers[this._layers.Count - 1];
      BlendState blendState = new BlendState();
      blendState.ColorSourceBlend = Blend.Zero;
      blendState.ColorDestinationBlend = Blend.SourceColor;
      blendState.ColorBlendFunction = BlendFunction.Add;
      blendState.AlphaSourceBlend = Blend.Zero;
      blendState.AlphaDestinationBlend = Blend.SourceAlpha;
      blendState.AlphaBlendFunction = BlendFunction.Add;
      this._glow.blend = BlendState.Additive;
      this._lighting.targetBlend = BlendState.Additive;
      this._lighting.targetBlend = new BlendState()
      {
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.One,
        ColorBlendFunction = BlendFunction.Add,
        AlphaSourceBlend = Blend.One,
        AlphaDestinationBlend = Blend.One,
        AlphaBlendFunction = BlendFunction.Add
      };
      this._lighting.blend = blendState;
      this._lighting.targetClearColor = new Color(150, 150, 150, 0);
      this._lighting.targetDepthStencil = DepthStencilState.None;
      this._lighting.flashAddClearInfluence = 1f;
      this._lighting2.targetBlend = BlendState.Additive;
      this._lighting2.blend = BlendState.Additive;
      this._lighting2.targetClearColor = new Color(0, 0, 0);
      this._lighting2.targetDepthStencil = DepthStencilState.None;
      this._lighting2.shareDrawObjects = this._lighting;
      this._lighting2.targetFade = 0.3f;
      this._layers = this._layers.OrderBy<Layer, int>((Func<Layer, int>) (l => -l.depth)).ToList<Layer>();
      Layer.Parallax.flashAddInfluence = 1f;
      Layer.HUD.flashAddInfluence = 1f;
      if (this._basicEffect == null)
      {
        this._itemSpawnEffect = Content.Load<MTEffect>("Shaders/wireframeTex");
        this._basicWireframeEffect = Content.Load<MTEffect>("Shaders/wireframe");
        this._basicWireframeEffectTex = Content.Load<MTEffect>("Shaders/wireframeTex");
        this._basicEffect = Content.Load<MTEffect>("Shaders/basic");
        this._basicEffectFade = Content.Load<MTEffect>("Shaders/basicFade");
        this._basicEffectAdd = Content.Load<MTEffect>("Shaders/basicAdd");
        this._basicEffectFadeAdd = Content.Load<MTEffect>("Shaders/basicFadeAdd");
      }
      this.ResetLayers();
    }

    public void ClearLayers()
    {
      foreach (DrawList hybrid in this._hybridList)
        hybrid.Clear();
    }

    public void DrawTargetLayers()
    {
      IOrderedEnumerable<Layer> source = this._hybridList.OrderBy<Layer, int>((Func<Layer, int>) (l => -l.depth));
      uint num = 0;
      for (int index = 0; index < this._hybridList.Count; ++index)
      {
        Layer layer = source.ElementAt<Layer>(index);
        if (layer.visible && layer.isTargetLayer && (Layer.lighting || layer != this._lighting && layer != this._lighting2))
        {
          float x = (float) (2.0 / (double) this._hybridList.Count * (double) index / 2.0);
          float y = (float) (2.0 / (double) this._hybridList.Count * (double) (index + 1) / 2.0);
          layer.depthSpan = new Vec2(x, y);
          layer.Draw(true, true);
          ++num;
        }
      }
    }

    public void DrawLayers()
    {
      IOrderedEnumerable<Layer> source = this._hybridList.OrderBy<Layer, int>((Func<Layer, int>) (l => -l.depth));
      if (this._lastDrawIndexCount == 0)
        this._lastDrawIndexCount = this._hybridList.Count;
      int num1 = 0;
      for (int index = 0; index < this._hybridList.Count; ++index)
      {
        Layer layer = source.ElementAt<Layer>(index);
        if (layer.visible && (Layer.lighting || layer != this._lighting && layer != this._lighting2))
        {
          int num2 = 1;
          if (layer == Layer.Game)
            num2 = 3;
          float x = (float) (2.0 / (double) this._lastDrawIndexCount * (double) num1 / 2.0);
          float y = (float) (2.0 / (double) this._lastDrawIndexCount * (double) (num1 + num2) / 2.0);
          layer.depthSpan = new Vec2(x, y);
          layer.Draw(true);
          num1 += num2;
        }
      }
      this._lastDrawIndexCount = num1;
    }

    public void UpdateLayers()
    {
      foreach (Layer hybrid in this._hybridList)
        hybrid.Update();
    }

    public void ResetLayers()
    {
      Layer.lightingTwoPointOh = false;
      foreach (Layer layer in this._layers)
      {
        layer.fade = 1f;
        layer.effect = (Effect) null;
        layer.camera = (Camera) null;
        layer.perspective = false;
        layer.fadeAdd = 0.0f;
        layer.colorAdd = Vec3.Zero;
        layer.colorMul = Vec3.One;
        if (layer != this._glow && layer != this._lighting && layer != this._lighting2)
        {
          layer.blend = BlendState.AlphaBlend;
          layer.targetBlend = BlendState.AlphaBlend;
        }
        layer.ClearScissor();
        layer.Clear();
      }
      this._extraLayers.Clear();
      this._parallax.camera = new Camera(0.0f, 0.0f, 320f, 320f * DuckGame.Graphics.aspect);
      this._virtual.camera = new Camera(0.0f, 0.0f, 320f, 320f * DuckGame.Graphics.aspect);
      this._hud.camera = new Camera();
      this._console.camera = new Camera(0.0f, 0.0f, (float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height);
      this._hybridList.Clear();
      this._hybridList.AddRange((IEnumerable<Layer>) this._layers);
    }

    public Layer Get(string layer) => this._layers.FirstOrDefault<Layer>((Func<Layer, bool>) (x => x.name == layer));

    public void Add(Layer l)
    {
      if (this._extraLayers.Contains(l))
        return;
      this._extraLayers.Add(l);
      this._hybridList.Add(l);
    }

    public void Remove(Layer l)
    {
      this._extraLayers.Remove(l);
      this._hybridList.Remove(l);
    }

    public bool Contains(Layer l) => this._hybridList.Contains(l);
  }
}
