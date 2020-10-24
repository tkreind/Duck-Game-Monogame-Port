// Decompiled with JetBrains decompiler
// Type: DuckGame.HotnessAnimation
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class HotnessAnimation
  {
    private Sprite _redBar;
    private Sprite _blueBar;
    private SpriteMap _icon;
    private BitmapFont _font;
    private List<int> _sampleCool = new List<int>();
    private List<int> _cool = new List<int>();
    private List<int> _lastFrame = new List<int>();
    private List<float> _upScale = new List<float>();
    private List<string> _hotnessStrings = new List<string>()
    {
      "Absolute Zero",
      "Icy Moon",
      "Antarctica",
      "Ice Cube",
      "Ice Cream",
      "Coffee",
      "Fire",
      "A Volcanic Eruption",
      "The Sun"
    };
    private List<int> _tempMap = new List<int>()
    {
      -250,
      -200,
      -100,
      -40,
      -20,
      0,
      100,
      1200,
      4000,
      4500,
      5000
    };
    private bool _readyToTalk;
    private float _wait;

    public bool ready => this._readyToTalk;

    public HotnessAnimation()
    {
      this._redBar = new Sprite("newscast/redBar");
      this._blueBar = new Sprite("newscast/blueBar");
      this._icon = new SpriteMap("newscast/hotness", 18, 18);
      this._icon.CenterOrigin();
      this._font = new BitmapFont("biosFontDegree", 8);
      for (int index = 0; index < 4; ++index)
      {
        if (Profiles.active.Count > index)
        {
          int num = Profiles.active[index].endOfRoundStats.GetProfileScore();
          if (num < 0)
            num = 0;
          this._sampleCool.Add(num);
        }
        else
          this._sampleCool.Add(0);
        this._cool.Add(-50);
        this._lastFrame.Add(0);
        this._upScale.Add(0.0f);
      }
    }

    public void Draw()
    {
      if ((double) this._wait > 1.0)
      {
        bool flag = true;
        for (int index1 = 0; index1 < this._cool.Count; ++index1)
        {
          if (this._sampleCool[index1] < this._cool[index1])
          {
            List<int> cool;
            int index2;
            (cool = this._cool)[index2 = index1] = cool[index2] - 1;
            flag = false;
          }
          else if (this._sampleCool[index1] > this._cool[index1])
          {
            List<int> cool;
            int index2;
            (cool = this._cool)[index2 = index1] = cool[index2] + 1;
            flag = false;
          }
          if ((double) this._upScale[index1] > 0.0)
          {
            List<float> upScale;
            int index2;
            (upScale = this._upScale)[index2 = index1] = upScale[index2] - 0.05f;
          }
        }
        if (flag)
        {
          this._wait += 0.015f;
          if ((double) this._wait > 2.0)
            this._readyToTalk = true;
        }
      }
      else
        this._wait += 0.01f;
      this._redBar.depth = new Depth(0.2f);
      Graphics.Draw(this._redBar, 30f, 25f);
      this._font.depth = new Depth(0.25f);
      if (DG.isHalloween)
        this._font.Draw("SPOOKY  REPORT", 44f, 28f, Color.White, new Depth(0.25f));
      else
        this._font.Draw("HOTNESS REPORT", 44f, 28f, Color.White, new Depth(0.25f));
      this._blueBar.depth = new Depth(0.1f);
      Graphics.Draw(this._blueBar, 30f, 18f);
      Graphics.DrawRect(new Vec2(20f, 135f), new Vec2(260f, 160f), new Color(12, 90, 182), new Depth(0.1f));
      Vec2 vec2_1 = new Vec2(60f, 50f);
      Vec2 vec2_2 = new Vec2(200f, 150f);
      Vec2 vec2_3 = new Vec2(vec2_2.x - vec2_1.x, vec2_2.y - vec2_1.y);
      List<Profile> active = Profiles.active;
      int index3 = 0;
      foreach (Profile profile in active)
      {
        float num1 = active.Count != 1 ? (active.Count != 2 ? (float) index3 * (vec2_3.x / (float) (active.Count - 1)) : (float) ((double) vec2_3.x / 2.0 - (double) vec2_3.x / 4.0 + (double) index3 * ((double) vec2_3.x / 2.0))) : vec2_3.x / 2f;
        float num2 = (float) (this._cool[index3] + 50) / 250f;
        float num3 = 1f / (float) (this._tempMap.Count - 2);
        int index1 = (int) ((double) num2 * (double) (this._tempMap.Count - 2));
        if (index1 < 0)
          index1 = 0;
        int temp = this._tempMap[index1];
        float num4 = Maths.NormalizeSection(num2, num3 * (float) index1, num3 * (float) (index1 + 1));
        int num5 = (int) ((double) this._tempMap[index1] + (double) (this._tempMap[index1 + 1] - this._tempMap[index1]) * (double) num4);
        float num6 = 50f;
        float num7 = num2 + 0.28f;
        float x = vec2_1.x + num1;
        float y = (float) ((double) vec2_2.y - 32.0 - (double) num7 * (double) num6);
        profile.persona.sprite.depth = new Depth(0.3f);
        profile.persona.sprite.color = Color.White;
        Graphics.Draw(profile.persona.sprite, 0, x, y);
        Vec2 hatPoint = DuckRig.GetHatPoint(profile.persona.sprite.imageIndex);
        profile.team.hat.depth = new Depth(0.31f);
        profile.team.hat.center = new Vec2(16f, 16f) + profile.team.hatOffset;
        Graphics.Draw(profile.team.hat, profile.team.hat.frame, x + hatPoint.x, y + hatPoint.y);
        Graphics.DrawRect(new Vec2(x - 17f, y + 16f), new Vec2(x + 16f, 160f), profile.persona.colorUsable, new Depth(0.05f));
        string text = num5.ToString() + "=";
        this._font.depth = new Depth(0.25f);
        this._font.Draw(text, new Vec2((float) ((double) x - (double) this._font.GetWidth(text) / 2.0 + 3.0), 140f), Color.White, new Depth(0.25f));
        this._icon.depth = new Depth(0.3f);
        this._icon.frame = (int) Math.Floor((double) num2 * 8.98999977111816);
        if (this._icon.frame != this._lastFrame[index3])
        {
          this._lastFrame[index3] = this._icon.frame;
          this._upScale[index3] = 0.5f;
        }
        this._icon.scale = new Vec2(1f + this._upScale[index3]);
        Graphics.Draw((Sprite) this._icon, x, y + 28f);
        ++index3;
      }
    }
  }
}
