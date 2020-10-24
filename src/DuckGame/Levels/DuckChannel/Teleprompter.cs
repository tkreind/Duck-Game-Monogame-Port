﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Teleprompter
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class Teleprompter : Thing
  {
    private BitmapFont _font;
    private List<DuckStory> _lines = new List<DuckStory>();
    private string _currentLine = "";
    private List<TextLine> _lineProgress = new List<TextLine>();
    private float _waitLetter = 1f;
    private float _waitAfterLine = 1f;
    private SpriteMap _newsCaster;
    private CasterMood _mood;
    private float _talkMove;
    private bool _paused;
    private SinWave _pitchSin = (SinWave) 0.15f;
    private bool _demoWait;
    private bool _tried;
    private Sound s;

    public bool finished => this._lines.Count == 0 && this._currentLine == "";

    public Teleprompter(float xpos, float ypos, SpriteMap newsCaster)
      : base(xpos, ypos)
      => this._newsCaster = newsCaster;

    public void Pause() => this._paused = true;

    public void Resume() => this._paused = false;

    public void ReadLine(DuckStory line) => this._lines.Add(line);

    public void InsertLine(string line, int index) => this._lines.Insert(index, new DuckStory()
    {
      text = line
    });

    public void ReadStory(DuckStory line) => this._lines.Add(line);

    public void SkipToClose()
    {
      this._lines.RemoveAll((Predicate<DuckStory>) (x => x.section < NewsSection.BeforeClosing));
      this._lines.Insert(0, new DuckStory()
      {
        section = NewsSection.DemoClosing,
        text = "|EXCITED|OK! We'll cut it short, But I'd like to talk |RED|Demo|WHITE|!*"
      });
    }

    public void ClearLines()
    {
      this._currentLine = "";
      this._lines.Clear();
    }

    public override void Initialize()
    {
      this._font = new BitmapFont("biosFont", 8);
      this.layer = Layer.HUD;
      base.Initialize();
    }

    public void SetCasterFrame(int frame)
    {
      if (this._mood == CasterMood.Excited)
        this._newsCaster.frame = frame + 3;
      else if (this._mood == CasterMood.Suave)
        this._newsCaster.frame = frame + 6;
      else
        this._newsCaster.frame = frame;
    }

    public int GetCasterFrame()
    {
      if (this._mood == CasterMood.Excited)
        return this._newsCaster.frame - 3;
      return this._mood == CasterMood.Suave ? this._newsCaster.frame - 6 : this._newsCaster.frame;
    }

    public override void Update()
    {
      if (this._demoWait)
      {
        bool flag1 = true;
        bool flag2 = false;
        if (!this._tried)
        {
          if (Input.Pressed("GRAB"))
          {
            HUD.CloseAllCorners();
            SFX.Play("rockHitGround", 0.9f);
            flag1 = false;
            this._tried = true;
          }
          if (Input.Pressed("SELECT"))
          {
            HUD.CloseAllCorners();
            flag2 = true;
            this._tried = true;
          }
        }
        if (!flag1)
        {
          Main.isDemo = false;
          this._lines.Clear();
          this._lines.Add(new DuckStory()
          {
            section = NewsSection.Closing,
            text = "|EXCITED|THANK YOU! You now have |GREEN|FULL Duck Game|WHITE|! You're the best!!!*"
          });
          this._currentLine = "";
          this._demoWait = false;
          HighlightLevel._cancelSkip = true;
        }
        else
        {
          if (!flag2)
            return;
          this._lines.Insert(0, new DuckStory()
          {
            section = NewsSection.Closing,
            text = "Ah, alright then!"
          });
          this._demoWait = false;
        }
      }
      else
      {
        if (this._paused)
          return;
        if (this._lines.Count > 0 && this._currentLine == "")
        {
          this._waitAfterLine -= 0.03f;
          this._talkMove += 0.75f;
          if ((double) this._talkMove > 1.0)
          {
            this.SetCasterFrame(0);
            this._talkMove = 0.0f;
          }
          if ((double) this._waitAfterLine <= 0.0)
          {
            this._lineProgress.Clear();
            if (!this._lines[0].text.StartsWith("CUE%"))
            {
              this._currentLine = this._lines[0].text;
              this._waitAfterLine = 1.2f;
            }
            this._lines[0].DoCallback();
            this._lines.RemoveAt(0);
            this._mood = CasterMood.Normal;
          }
        }
        if (this._currentLine != "")
        {
          this._waitLetter -= 0.5f;
          if ((double) this._waitLetter >= 0.0)
            return;
          this._talkMove += 0.75f;
          if ((double) this._talkMove > 1.0)
          {
            if (this._currentLine[0] != ' ' && this.GetCasterFrame() == 0)
              this.SetCasterFrame(Rando.Int(1) + 1);
            else
              this.SetCasterFrame(0);
            this._talkMove = 0.0f;
          }
          this._waitLetter = 1f;
          while (this._currentLine[0] == '|')
          {
            this._currentLine = this._currentLine.Remove(0, 1);
            string str = "";
            for (; this._currentLine[0] != '|' && this._currentLine.Length > 0; this._currentLine = this._currentLine.Remove(0, 1))
              str += (string) (object) this._currentLine[0];
            if (this._currentLine.Length <= 1)
            {
              this._currentLine = "";
              return;
            }
            this._currentLine = this._currentLine.Remove(0, 1);
            Color c = Color.White;
            bool flag = false;
            if (str == "RED")
            {
              flag = true;
              c = Color.Red;
            }
            else if (str == "WHITE")
            {
              flag = true;
              c = Color.White;
            }
            else if (str == "BLUE")
            {
              flag = true;
              c = Color.Blue;
            }
            else if (str == "GREEN")
            {
              flag = true;
              c = Color.LimeGreen;
            }
            else if (str == "EXCITED")
              this._mood = CasterMood.Excited;
            else if (str == "SUAVE")
              this._mood = CasterMood.Suave;
            else if (str == "CALM")
              this._mood = CasterMood.Normal;
            else if (str == "DEMOWAIT")
            {
              HUD.CloseAllCorners();
              HUD.AddCornerControl(HUDCorner.BottomLeft, "PAY THE MAN@GRAB@");
              HUD.AddCornerControl(HUDCorner.BottomRight, "@SELECT@NO!");
              this._demoWait = true;
              return;
            }
            if (flag)
            {
              if (this._lineProgress.Count == 0)
                this._lineProgress.Insert(0, new TextLine()
                {
                  lineColor = c
                });
              else
                this._lineProgress[0].SwitchColor(c);
            }
          }
          string str1 = "";
          int index1 = 1;
          if (this._currentLine[0] == ' ')
          {
            while (index1 < this._currentLine.Length && this._currentLine[index1] != ' ')
            {
              if (this._currentLine[index1] == '|')
              {
                int index2 = index1 + 1;
                while (index2 < this._currentLine.Length && this._currentLine[index2] != '|')
                  ++index2;
                index1 = index2 + 1;
              }
              else
              {
                str1 += (string) (object) this._currentLine[index1];
                ++index1;
              }
            }
          }
          if (this._lineProgress.Count == 0 || this._currentLine[0] == ' ' && this._lineProgress[0].Length() + str1.Length > 21)
          {
            Color color = Color.White;
            if (this._lineProgress.Count > 0)
              color = this._lineProgress[0].lineColor;
            this._lineProgress.Insert(0, new TextLine()
            {
              lineColor = color
            });
            if (this._currentLine[0] != ' ')
              return;
            this._currentLine = this._currentLine.Remove(0, 1);
          }
          else
          {
            if (this._currentLine[0] == '!' || this._currentLine[0] == '?' || this._currentLine[0] == '.')
              this._waitLetter = 5f;
            else if (this._currentLine[0] == ',')
              this._waitLetter = 3f;
            if (this._currentLine[0] == '*')
            {
              this._waitLetter = 5f;
            }
            else
            {
              this._lineProgress[0].Add(this._currentLine[0]);
              char ch = this._currentLine[0].ToString().ToLowerInvariant()[0];
              if (ch >= 'a' && ch <= 'z' || ch >= '0' && ch <= '9')
                this.s = SFX.Play("tinyNoise" + Convert.ToString(Rando.Int(1, 5)), pitch: Rando.Float(-0.8f, -0.4f));
            }
            this._currentLine = this._currentLine.Remove(0, 1);
          }
        }
        else
        {
          this._talkMove += 0.75f;
          if ((double) this._talkMove <= 1.0)
            return;
          this.SetCasterFrame(0);
          this._talkMove = 0.0f;
        }
      }
    }

    public override void Draw()
    {
      int num = 0;
      for (int index1 = this._lineProgress.Count - 1; index1 >= 0; --index1)
      {
        float width = this._font.GetWidth(this._lineProgress[index1].text);
        float ypos = (float) (140 - (this._lineProgress.Count - 1) * 9 + num * 9);
        Graphics.DrawRect(new Vec2((float) (132.0 - (double) width / 2.0 - 1.0), ypos - 1f), new Vec2((float) (132.0 + (double) width / 2.0), ypos + 9f), Color.Black, (Depth) 0.84f);
        float xpos = (float) (132.0 - (double) width / 2.0);
        for (int index2 = this._lineProgress[index1].segments.Count - 1; index2 >= 0; --index2)
        {
          this._font.Draw(this._lineProgress[index1].segments[index2].text, xpos, ypos, this._lineProgress[index1].segments[index2].color, (Depth) 0.85f);
          xpos += (float) (this._lineProgress[index1].segments[index2].text.Length * 8);
        }
        ++num;
      }
    }
  }
}
