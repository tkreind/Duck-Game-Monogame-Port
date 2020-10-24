﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.HighlightPlayback
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame
{
  public class HighlightPlayback : Level
  {
    private BitmapFont _font;
    private bool _fadeOut;
    private float _waitToShow = 1f;
    private int _currentHighlight;
    private float _endWait = 1f;
    private bool _showHighlight = true;
    private float _keepPaused = 1f;
    private List<Recording> _highlights;
    private Sprite _tv;
    private SpriteMap _numbers;
    private bool _skip;

    public HighlightPlayback(int highlight)
    {
      this._font = new BitmapFont("biosFont", 8);
      this._currentHighlight = highlight;
      this._highlights = Highlights.GetHighlights();
      while (this._currentHighlight >= this._highlights.Count)
        --this._currentHighlight;
      this._numbers = new SpriteMap("newscast/numberfont", 25, 22);
    }

    public override void Initialize()
    {
      this._tv = new Sprite("bigTV");
      Vote.OpenVoting("SKIP", "START");
    }

    public override void Update()
    {
      if (!this._skip && Vote.Passed(VoteType.Skip))
        this._skip = true;
      if (this._skip)
        this._fadeOut = true;
      DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, this._fadeOut ? 0.0f : 1f, 0.02f);
      if ((double) DuckGame.Graphics.fade < 0.00999999977648258 && this._skip)
      {
        HighlightLevel.didSkip = true;
        Vote.CloseVoting();
        Level.current = !Main.isDemo ? (Level) new RockScoreboard(RockScoreboard.returnLevel, ScoreBoardMode.ShowWinner, true) : (Level) new HighlightLevel(true);
      }
      if (!this._showHighlight && (double) DuckGame.Graphics.fade > 0.949999988079071)
      {
        this._waitToShow -= 0.02f;
        if ((double) this._waitToShow <= 0.0)
        {
          this._waitToShow = 0.0f;
          this._fadeOut = true;
        }
      }
      if ((double) DuckGame.Graphics.fade < 0.00999999977648258 && !this._showHighlight && this._fadeOut)
      {
        this._fadeOut = false;
        this._showHighlight = true;
      }
      if (this._showHighlight && (double) DuckGame.Graphics.fade > 0.949999988079071)
        this._keepPaused -= 0.03f;
      if (!this._highlights[this._currentHighlight].finished)
        return;
      this._endWait -= 0.03f;
      if ((double) this._endWait > 0.0)
        return;
      this._fadeOut = true;
      if ((double) DuckGame.Graphics.fade >= 0.00999999977648258)
        return;
      int highlight = this._currentHighlight - 1;
      if (this._currentHighlight == 0)
        Level.current = (Level) new HighlightLevel(true);
      else
        Level.current = (Level) new HighlightPlayback(highlight);
    }

    public override void Draw()
    {
    }

    public override void BeforeDraw()
    {
      if (!this._showHighlight)
        return;
      if ((double) this._keepPaused > 0.0)
        this._highlights[this._currentHighlight].frame = this._highlights[this._currentHighlight].startFrame + 5;
      this._highlights[this._currentHighlight].RenderFrame();
      if ((double) this._keepPaused > 0.0 || this._highlights[this._currentHighlight].finished)
        return;
      this._highlights[this._currentHighlight].UpdateFrame();
      this._highlights[this._currentHighlight].IncrementFrame();
    }

    public override void AfterDrawLayers()
    {
      if ((double) this._keepPaused <= 0.0)
        return;
      DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
      this._font.scale = new Vec2(8f, 8f);
      double width = (double) this._font.GetWidth(Change.ToString((object) (this._currentHighlight + 1)));
      double height = (double) this._font.height;
      this._numbers.frame = 4 - this._currentHighlight;
      this._numbers.depth = (Depth) 1f;
      this._numbers.scale = new Vec2(4f, 4f);
      DuckGame.Graphics.Draw((Sprite) this._numbers, 32f, 32f);
      DuckGame.Graphics.screen.End();
    }
  }
}
