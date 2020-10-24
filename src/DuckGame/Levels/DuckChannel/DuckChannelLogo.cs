﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckChannelLogo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class DuckChannelLogo : Thing
  {
    private Sprite _duck;
    private Sprite _channel;
    private Sprite _five;
    private float _duckLerp;
    private float _channelLerp;
    private float _fiveLerp;
    private List<float> _swipeLines = new List<float>();
    private List<float> _swipeSpeeds = new List<float>();
    private float _slideOutWait;
    private float _transitionWait;
    private bool _playSwipe;
    private bool _doTransition;

    public void PlaySwipe()
    {
      if (this._playSwipe)
        return;
      this._playSwipe = true;
      this._duckLerp = 0.0f;
      this._channelLerp = 0.0f;
      this._slideOutWait = 0.0f;
      this._fiveLerp = 0.0f;
      this._doTransition = false;
      this._transitionWait = 0.0f;
      for (int index = 0; index < 10; ++index)
      {
        this._swipeLines[index] = Rando.Float(0.1f);
        this._swipeSpeeds[index] = Rando.Float(0.01f, 0.012f);
      }
    }

    public bool doTransition => this._doTransition;

    public DuckChannelLogo()
      : base()
    {
      this._duck = new Sprite("newsTitleDuck");
      this._channel = new Sprite("newsTitleChannel");
      this._five = new Sprite("newsTitle5");
      this.layer = Layer.HUD;
      for (int index = 0; index < 10; ++index)
      {
        this._swipeLines.Add(Rando.Float(0.1f));
        this._swipeSpeeds.Add(Rando.Float(0.01f, 0.012f));
      }
    }

    public override void Update()
    {
      if (this._playSwipe)
      {
        this._transitionWait += 0.02f;
        if ((double) this._transitionWait > 1.0)
          this._doTransition = true;
        if ((double) this._slideOutWait < 1.0)
        {
          this._duckLerp = Lerp.FloatSmooth(this._duckLerp, 1f, 0.1f, 1.1f);
          this._channelLerp = Lerp.FloatSmooth(this._channelLerp, 1f, 0.1f, 1.1f);
          this._fiveLerp = Lerp.FloatSmooth(this._fiveLerp, 1f, 0.1f, 1.1f);
          this._slideOutWait += 0.012f;
        }
        else
        {
          this._duckLerp = Lerp.FloatSmooth(this._duckLerp, 0.0f, 0.1f, 1.1f);
          this._channelLerp = Lerp.FloatSmooth(this._channelLerp, 0.0f, 0.1f, 1.1f);
          this._fiveLerp = Lerp.FloatSmooth(this._fiveLerp, 0.0f, 0.1f, 1.1f);
          if ((double) this._duckLerp < 0.00999999977648258)
            this._playSwipe = false;
        }
        for (int index = 0; index < this._swipeLines.Count; ++index)
          this._swipeLines[index] = Lerp.Float(this._swipeLines[index], 1f, this._swipeSpeeds[index]);
      }
      else
        this._doTransition = false;
    }

    public override void Draw()
    {
      Vec2 vec2_1 = new Vec2(10f, 12f);
      Vec2 vec2_2 = new Vec2((float) (-200.0 * (1.0 - (double) this._duckLerp)), 0.0f);
      Vec2 vec2_3 = new Vec2((float) (200.0 * (1.0 - (double) this._channelLerp)), 0.0f);
      Vec2 vec2_4 = new Vec2((float) (300.0 * (1.0 - (double) this._channelLerp)), 0.0f);
      this._duck.depth = new Depth(0.85f);
      Graphics.Draw(this._duck, vec2_1.x + 80f + vec2_2.x, vec2_1.y + 60f + vec2_2.y);
      this._channel.depth = new Depth(0.86f);
      Graphics.Draw(this._channel, vec2_1.x + 64f + vec2_3.x, vec2_1.y + 74f + vec2_3.y);
      this._five.depth = new Depth(0.85f);
      Graphics.Draw(this._five, vec2_1.x + 144f + vec2_4.x, vec2_1.y + 64f + vec2_4.y);
      Vec2 vec2_5 = new Vec2(30f, 20f);
      float num1 = 500f;
      float num2 = 16f;
      float num3 = 600f;
      for (int index = 0; index < this._swipeLines.Count; ++index)
      {
        float num4 = this._swipeLines[index] * -1200f;
        Graphics.DrawRect(new Vec2(vec2_5.x + num3 + num4, vec2_5.y + (float) index * num2), new Vec2(vec2_5.x + num1 + num3 + num4, vec2_5.y + (float) index * num2 + num2), Color.Black, new Depth(0.83f));
      }
    }
  }
}
