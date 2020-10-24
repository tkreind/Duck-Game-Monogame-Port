﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Recording
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Recording
  {
    private static int kNumFrames = 300;
    protected RecorderFrame[] _frames = new RecorderFrame[Recording.kNumFrames];
    protected int _frame;
    private int _startFrame;
    private int _endFrame;
    private bool _rolledOver;
    private float _highlightScore;
    private static FrameAnalytics _analytics = new FrameAnalytics();

    public int frame
    {
      get => this._frame;
      set => this._frame = value % Recording.kNumFrames;
    }

    public int startFrame => this._startFrame;

    public int endFrame => this._endFrame;

    public bool finished => this._frame == this._endFrame;

    public float highlightScore
    {
      get => this._highlightScore;
      set => this._highlightScore = value;
    }

    public Recording() => this.Initialize();

    public void Initialize()
    {
      for (int index = 0; index < ((IEnumerable<RecorderFrame>) this._frames).Count<RecorderFrame>(); ++index)
        this._frames[index].Initialize();
    }

    public void Reset()
    {
      this._frame = 0;
      this._startFrame = 0;
      this._rolledOver = false;
      this._highlightScore = 0.0f;
      this._endFrame = 0;
    }

    public float GetFrameVelocity() => this._frames[this._frame].totalVelocity * 0.06f;

    public float GetFrameCoolness() => (float) this._frames[this._frame].coolness;

    public int GetFrame(int f)
    {
      if (f < 0)
        f += Recording.kNumFrames - 1;
      else if (f >= Recording.kNumFrames)
        f -= Recording.kNumFrames;
      return f;
    }

    public float GetFrameAction() => (float) this._frames[this._frame].actions;

    public float GetFrameBonus() => (float) this._frames[this._frame].bonus;

    public float GetFrameTotal()
    {
      FrameAnalytics analytics = this.GetAnalytics(Recording._analytics);
      return 0.0f + analytics.deaths + analytics.coolness + analytics.bonus + analytics.actions + analytics.totalVelocity;
    }

    public void Rewind() => this._frame = this._startFrame;

    public virtual void RenderFrame() => this._frames[this._frame].Render();

    public virtual void RenderFrame(float timeLag) => this._frames[this.GetFrame(this._frame - (int) ((double) timeLag / (double) Maths.IncFrameTimer()))].Render();

    public void UpdateFrame() => this._frames[this._frame].Update();

    public virtual void IncrementFrame(float speed = 1f) => this._frame = (this._frame + 1) % Recording.kNumFrames;

    public virtual void NextFrame()
    {
      ++this._frame;
      if (this._frame >= Recording.kNumFrames)
      {
        this._rolledOver = true;
        this._frame = 0;
      }
      this._frames[this._frame].Reset();
      this._frames[this._frame].actions += (byte) Math.Max((int) this._frames[this.GetFrame(this._frame - 1)].actions - 1, 0);
      this._frames[this._frame].bonus += (byte) Math.Max((int) this._frames[this.GetFrame(this._frame - 1)].bonus - 1, 0);
      this._frames[this._frame].coolness += (byte) Math.Max((int) this._frames[this.GetFrame(this._frame - 1)].coolness - 1, 0);
      this._endFrame = this._frame;
      if (!this._rolledOver)
        return;
      this._startFrame = (this._frame + 1) % Recording.kNumFrames;
    }

    public bool StepForward()
    {
      this._frame = (this._frame + 1) % Recording.kNumFrames;
      return this._frame == this._startFrame;
    }

    public void LogVelocity(float velocity) => this._frames[this._frame].totalVelocity += velocity * Highlights.highlightRatingMultiplier;

    public void LogCoolness(int val) => this._frames[this._frame].coolness += (byte) ((double) val * (double) Highlights.highlightRatingMultiplier);

    public void LogDeath() => this._frames[this._frame].deaths += (byte) (1.0 * (double) Highlights.highlightRatingMultiplier);

    public void LogAction(int num = 1) => this._frames[this._frame].actions += (byte) ((double) num * (double) Highlights.highlightRatingMultiplier);

    public void LogBonus() => this._frames[this._frame].bonus += (byte) (1.0 * (double) Highlights.highlightRatingMultiplier);

    public void LogBackgroundColor(Color c) => this._frames[this._frame].backgroundColor = c;

    public void StateChange(
      SpriteSortMode sortModeVal,
      BlendState blendStateVal,
      SamplerState samplerStateVal,
      DepthStencilState depthStencilStateVal,
      RasterizerState rasterizerStateVal,
      MTEffect effectVal,
      Matrix cameraVal,
      Rectangle scissor)
    {
      this._frames[this._frame].StateChange(sortModeVal, blendStateVal, samplerStateVal, depthStencilStateVal, rasterizerStateVal, effectVal, cameraVal, scissor);
    }

    public void LogDraw(
      short textureVal,
      Vec2 topLeftVal,
      Vec2 bottomRightVal,
      float rotationVal,
      Color colorVal,
      short texXVal,
      short texYVal,
      short texWVal,
      short texHVal,
      float depthVal,
      int spriteIndexVal,
      int thingIndexVal,
      int drawIndexVal)
    {
      this._frames[this._frame].objects[this._frames[this._frame].currentObject].SetData(textureVal, topLeftVal, bottomRightVal, rotationVal, colorVal, texXVal, texYVal, texWVal, texHVal, depthVal, (long) (spriteIndexVal + thingIndexVal * 10000 + drawIndexVal * 1000000000));
      this._frames[this._frame].IncrementObject();
    }

    public void LogSound(string soundVal, float volumeVal, float pitchVal, float panVal) => this._frames[this._frame].sounds.Add(new RecorderSoundItem()
    {
      sound = soundVal,
      volume = volumeVal,
      pitch = pitchVal,
      pan = panVal
    });

    public FrameAnalytics GetAnalytics(FrameAnalytics f, int fr = -1)
    {
      fr = fr != -1 ? this.GetFrame(fr) : this._frame;
      int kNumFrames = Recording.kNumFrames;
      int index1 = fr;
      float num1 = 0.0f;
      bool flag = false;
      for (int index2 = 0; index2 < kNumFrames; ++index2)
      {
        if (this._frames[index1].deaths > (byte) 0)
        {
          flag = true;
          break;
        }
        num1 += 0.016f;
        ++index1;
        if (index1 >= Recording.kNumFrames)
          index1 = 0;
        if (index1 == this._startFrame)
          break;
      }
      if (!flag)
        num1 = 99f;
      f.timeBeforeKill = num1;
      float num2 = (float) ((1.0 - (double) Maths.Clamp(f.timeBeforeKill, 0.0f, 3f) / 3.0) * 1.0 + 1.0);
      f.actions = (float) this._frames[fr].actions * (num2 * 0.03f);
      f.deaths = (float) this._frames[fr].deaths * num2;
      f.bonus = (float) this._frames[fr].bonus * (num2 * 0.08f);
      f.coolness = (float) this._frames[fr].coolness * (num2 * 0.1f);
      f.totalVelocity = this._frames[fr].totalVelocity * (1f / 500f) * num2;
      return f;
    }
  }
}
