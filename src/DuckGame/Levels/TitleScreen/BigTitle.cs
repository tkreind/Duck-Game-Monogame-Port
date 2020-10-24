﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.BigTitle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class BigTitle : Thing
  {
    private Sprite _sprite;
    private int _wait;
    private int _count;
    private int _maxCount = 50;
    private float _alpha = 1f;
    private float _fartWait = 1f;
    private bool _showFart;
    private bool _fade;
    private int _lerpNum;
    private List<Color> _lerpColors = new List<Color>()
    {
      Color.White,
      Color.PaleVioletRed,
      Color.Red,
      Color.OrangeRed,
      Color.Orange,
      Color.Yellow,
      Color.YellowGreen,
      Color.Green,
      Color.BlueViolet,
      Color.Purple,
      Color.Pink
    };
    private Color _currentColor;
    private Sprite _demo;

    public bool fade
    {
      get => this._fade;
      set => this._fade = value;
    }

    public BigTitle()
      : base()
    {
      this._sprite = new Sprite("duckGameTitle");
      this._demo = new Sprite("demoPro");
      this.graphic = this._sprite;
      this.depth = (Depth) 0.6f;
      this.graphic.color = Color.Black;
      this.centery = (float) (this.graphic.height / 2);
      this.alpha = 0.0f;
      this.layer = Layer.HUD;
      this._currentColor = this._lerpColors[0];
    }

    public override void Initialize()
    {
    }

    public override void Draw()
    {
      Graphics.DrawRect(this.position + new Vec2(-300f, -30f), this.position + new Vec2(300f, 30f), Color.Black * 0.6f * this.alpha, this.depth - 100);
      if (this._showFart)
      {
        this._demo.alpha = this.alpha;
        this._demo.depth = (Depth) 0.7f;
        Graphics.Draw(this._demo, this.x + 28f, this.y + 32f);
      }
      base.Draw();
    }

    public override void Update()
    {
      if (Main.isDemo)
      {
        this._fartWait -= 0.008f;
        if ((double) this._fartWait < 0.0 && !this._showFart)
        {
          this._showFart = true;
          SFX.Play("fart" + (object) Rando.Int(3));
        }
      }
      ++this._wait;
      int wait = this._wait;
      if (this._fade)
      {
        this.alpha -= 0.05f;
        if ((double) this.alpha >= 0.0)
          return;
        Level.Remove((Thing) this);
      }
      else
      {
        if (this._wait <= 30 || this._count >= this._maxCount)
          return;
        this._lerpNum = (int) ((double) ((float) this._count / (float) this._maxCount) * (double) this._lerpColors.Count - 0.00999999977648258);
        int num = this._maxCount / this._lerpColors.Count;
        this._currentColor = Color.Lerp(this._currentColor, this._lerpColors[this._lerpNum], 0.1f);
        this._currentColor.a = (byte) ((double) this._alpha * (double) byte.MaxValue);
        this._alpha -= 0.02f;
        if ((double) this._alpha < 0.0)
          this._alpha = 0.0f;
        ++this._count;
      }
    }
  }
}
