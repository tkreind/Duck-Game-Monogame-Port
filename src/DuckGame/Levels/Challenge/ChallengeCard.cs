// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeCard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class ChallengeCard : Thing
  {
    private ChallengeData _challenge;
    private SpriteMap _thumb;
    private SpriteMap _preview;
    private SpriteMap _medalNoRibbon;
    private SpriteMap _medalRibbon;
    private BitmapFont _font;
    public bool hover;
    private bool _unlocked;
    public bool expand;
    public bool contract;
    private float _size = 42f;
    public float _alphaMul = 1f;
    public float _dataAlpha;
    private ChallengeSaveData _save;
    private ChallengeSaveData _realSave;
    private FancyBitmapFont _fancyFont;

    public ChallengeData challenge => this._challenge;

    public bool unlocked
    {
      get => this._unlocked;
      set => this._unlocked = value;
    }

    public ChallengeCard(float xpos, float ypos, ChallengeData c)
      : base(xpos, ypos)
    {
      this._challenge = c;
      this._thumb = new SpriteMap("arcade/challengeThumbnails", 38, 38);
      this._thumb.frame = 1;
      this._font = new BitmapFont("biosFont", 8);
      this._medalNoRibbon = new SpriteMap("arcade/medalNoRibbon", 18, 18);
      this._realSave = Challenges.GetSaveData(this._challenge.levelID, Profiles.active[0]);
      this._save = this._realSave.Clone();
      this._medalRibbon = new SpriteMap("arcade/medalRibbon", 18, 27);
      this._medalRibbon.center = new Vec2(6f, 3f);
      this._fancyFont = new FancyBitmapFont("smallFont");
    }

    public override void Update()
    {
      if (this._preview == null && this._challenge.preview != null)
      {
        Texture2D texture2D = Texture2D.FromStream(DuckGame.Graphics.device, (Stream) new MemoryStream(Convert.FromBase64String(this._challenge.preview)));
        this._preview = new SpriteMap((Tex2D) texture2D, texture2D.Width, texture2D.Height);
        this._preview.scale = new Vec2(0.25f);
      }
      this._size = Lerp.Float(this._size, this.contract ? 1f : (this.expand ? 130f : 42f), 8f);
      this._alphaMul = Lerp.Float(this._alphaMul, this.contract ? 0.0f : 1f, 0.1f);
      this._dataAlpha = Lerp.Float(this._dataAlpha, (double) this._size <= 126.0 || !this.expand ? 0.0f : 1f, !this.expand ? 1f : 0.2f);
    }

    public string MakeQuestionMarks(string val)
    {
      for (int index = 0; index < val.Length; ++index)
      {
        if (val[index] != ' ')
        {
          val = val.Remove(index, 1);
          val = val.Insert(index, "?");
        }
      }
      return val;
    }

    public bool HasNewTrophy() => this._realSave.trophy != this._save.trophy;

    public bool HasNewTime() => this._realSave.bestTime != this._save.bestTime;

    public int GiveTrophy()
    {
      int num = 0;
      if (this._save.trophy != this._realSave.trophy)
      {
        for (int index = (int) (this._save.trophy + 1); (TrophyType) index <= this._realSave.trophy; ++index)
        {
          switch (index)
          {
            case 1:
              num += Challenges.valueBronze;
              break;
            case 2:
              num += Challenges.valueSilver;
              break;
            case 3:
              num += Challenges.valueGold;
              break;
            case 4:
              num += Challenges.valuePlatinum;
              break;
          }
        }
        this._save.trophy = this._realSave.trophy;
      }
      return num;
    }

    public void GiveTime()
    {
      if (this._save.bestTime != this._realSave.bestTime)
        this._save.bestTime = this._realSave.bestTime;
      if (this._save.goodies != this._realSave.goodies)
        this._save.goodies = this._realSave.goodies;
      if (this._save.targets == this._realSave.targets)
        return;
      this._save.targets = this._realSave.targets;
    }

    public void UnlockAnimation()
    {
      SFX.Play("landTV", pitch: -0.3f);
      SmallSmoke smallSmoke = SmallSmoke.New(this.x + 2f, this.y + 2f);
      smallSmoke.layer = Layer.HUD;
      Level.Add((Thing) smallSmoke);
    }

    public override void Draw()
    {
      float num1 = this.alpha * (this.hover ? 1f : 0.6f) * this._alphaMul;
      this._font.alpha = num1;
      DuckGame.Graphics.DrawRect(this.position, this.position + new Vec2(258f, this._size), Color.White * num1, (Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303), false);
      if (this._save.trophy != TrophyType.Baseline)
      {
        this._medalRibbon.depth = (Depth) (float) (0.810000002384186 + (double) num1 * 0.0399999991059303);
        this._medalRibbon.color = new Color(num1, num1, num1);
        this._medalRibbon.alpha = ArcadeHUD.alphaVal;
        if (this._save.trophy == TrophyType.Bronze)
          this._medalRibbon.frame = 0;
        else if (this._save.trophy == TrophyType.Silver)
          this._medalRibbon.frame = 1;
        else if (this._save.trophy == TrophyType.Gold)
          this._medalRibbon.frame = 2;
        else if (this._save.trophy == TrophyType.Platinum)
          this._medalRibbon.frame = 3;
        else if (this._save.trophy == TrophyType.Developer)
          this._medalRibbon.frame = 4;
        DuckGame.Graphics.Draw((Sprite) this._medalRibbon, this.position.x, this.position.y);
      }
      else if (!this._unlocked)
      {
        this._medalRibbon.depth = (Depth) (float) (0.810000002384186 + (double) num1 * 0.0399999991059303);
        this._medalRibbon.color = new Color(num1, num1, num1);
        this._medalRibbon.frame = 5;
        DuckGame.Graphics.Draw((Sprite) this._medalRibbon, this.position.x, this.position.y);
      }
      this._thumb.alpha = num1;
      this._thumb.depth = (Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303);
      this._thumb.frame = this._unlocked ? 1 : 0;
      if (this._unlocked && this._preview != null)
      {
        this._preview.alpha = num1;
        this._preview.depth = (Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303);
        DuckGame.Graphics.Draw((Sprite) this._preview, this.x + 2f, this.y + 2f);
      }
      else
        DuckGame.Graphics.Draw((Sprite) this._thumb, this.x + 2f, this.y + 2f);
      this._font.maxWidth = 200;
      string str1 = this._challenge.name;
      if (!this._unlocked)
        str1 = this.MakeQuestionMarks(str1);
      this._font.Draw(str1, this.x + 41f, this.y + 2f, Color.White * num1, new Depth(1f));
      Color c1 = new Color(247, 224, 89);
      string str2 = this._challenge.description;
      if (!this._unlocked)
        str2 = this.MakeQuestionMarks(str2);
      this._fancyFont.maxWidth = 200;
      this._fancyFont.alpha = num1;
      this._fancyFont.xscale = this._fancyFont.yscale = 0.75f;
      this._fancyFont.Draw(str2, this.x + 41f, this.y + 12f, c1, new Depth(1f));
      if ((double) this._dataAlpha <= 0.00999999977648258)
        return;
      float num2 = this._dataAlpha * num1;
      DuckGame.Graphics.DrawLine(this.position + new Vec2(0.0f, 42f), this.position + new Vec2(258f, 42f), Color.White * num2, depth: ((Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303)));
      DuckGame.Graphics.DrawLine(this.position + new Vec2(0.0f, 64f), this.position + new Vec2(258f, 64f), Color.White * num2, depth: ((Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303)));
      this._font.alpha = num2;
      Color color = new Color(245, 165, 36);
      Color c2 = Color.Red;
      if (this._save.trophy == TrophyType.Bronze)
        c2 = Colors.Bronze;
      else if (this._save.trophy == TrophyType.Silver)
        c2 = Colors.Silver;
      else if (this._save.trophy == TrophyType.Gold)
        c2 = Colors.Gold;
      else if (this._save.trophy == TrophyType.Platinum)
        c2 = Colors.Platinum;
      this._fancyFont.Draw("|DGBLUE|" + this._challenge.goal, this.x + 6f, this.y + 45f, Color.White, new Depth(1f));
      this._font.Draw(Chancy.GetChallengeBestString(this._save, this._challenge), this.x + 6f, (float) ((double) this.y + 45.0 + 9.0), c2, new Depth(1f));
      bool flag = false;
      this._medalNoRibbon.depth = (Depth) (float) (0.800000011920929 + (double) num1 * 0.0399999991059303);
      this._medalNoRibbon.alpha = num2;
      this._medalNoRibbon.frame = 0;
      float x = this.x + 6f;
      float num3 = this.y + 68f;
      DuckGame.Graphics.Draw((Sprite) this._medalNoRibbon, x, num3);
      Color c3 = new Color(245, 165, 36);
      this._font.Draw("GOLD", x + 22f, num3, c3, new Depth(1f));
      ChallengeTrophy challengeTrophy1 = this._challenge.trophies.FirstOrDefault<ChallengeTrophy>((Func<ChallengeTrophy, bool>) (val => val.type == TrophyType.Gold));
      string text1 = "";
      if (challengeTrophy1.timeRequirement > 0)
      {
        TimeSpan span = TimeSpan.FromSeconds((double) challengeTrophy1.timeRequirement);
        text1 = text1 + "TIME " + MonoMain.TimeString(span, 2) + " ";
        flag = true;
      }
      if (challengeTrophy1.targets > 0)
      {
        if (text1 != "")
          text1 += ", ";
        text1 = text1 + "|LIME|" + challengeTrophy1.targets.ToString() + " TARGETS";
      }
      if (challengeTrophy1.goodies > 0)
      {
        if (text1 != "")
          text1 += ", ";
        string str3 = "GOODIES";
        if (this._challenge.prefix != "")
          str3 = this._challenge.prefix;
        text1 = text1 + "|ORANGE|" + challengeTrophy1.goodies.ToString() + " " + str3;
      }
      this._font.Draw(text1, x + 22f, num3 + 9f, Color.White, new Depth(1f));
      float num4 = (float) ((double) this.y + 68.0 + 20.0);
      this._medalNoRibbon.alpha = num2;
      this._medalNoRibbon.frame = 1;
      DuckGame.Graphics.Draw((Sprite) this._medalNoRibbon, x, num4);
      c3 = new Color(173, 173, 173);
      this._font.Draw("SILVER", x + 22f, num4, c3, new Depth(1f));
      ChallengeTrophy challengeTrophy2 = this._challenge.trophies.FirstOrDefault<ChallengeTrophy>((Func<ChallengeTrophy, bool>) (val => val.type == TrophyType.Silver));
      string text2 = "";
      if (challengeTrophy2.timeRequirement > 0)
      {
        TimeSpan span = TimeSpan.FromSeconds((double) challengeTrophy2.timeRequirement);
        text2 = text2 + "TIME " + MonoMain.TimeString(span, 2) + " ";
        flag = true;
      }
      else if (flag)
        text2 = "ANY TIME ";
      if (challengeTrophy2.targets > 0)
      {
        if (text2 != "")
          text2 += ", ";
        text2 = text2 + "|LIME|" + challengeTrophy2.targets.ToString() + " TARGETS";
      }
      if (challengeTrophy2.goodies > 0)
      {
        if (text2 != "")
          text2 += ", ";
        string str3 = "GOODIES";
        if (this._challenge.prefix != "")
          str3 = this._challenge.prefix;
        text2 = text2 + "|ORANGE|" + challengeTrophy2.goodies.ToString() + " " + str3;
      }
      this._font.Draw(text2, x + 22f, num4 + 9f, Color.White, new Depth(1f));
      float num5 = (float) ((double) this.y + 68.0 + 40.0);
      this._medalNoRibbon.alpha = num2;
      this._medalNoRibbon.frame = 2;
      DuckGame.Graphics.Draw((Sprite) this._medalNoRibbon, x, num5);
      c3 = new Color(181, 86, 3);
      this._font.Draw("BRONZE", x + 22f, num5, c3, new Depth(1f));
      ChallengeTrophy challengeTrophy3 = this._challenge.trophies.FirstOrDefault<ChallengeTrophy>((Func<ChallengeTrophy, bool>) (val => val.type == TrophyType.Bronze));
      string text3 = "";
      if (challengeTrophy3.timeRequirement > 0)
      {
        TimeSpan span = TimeSpan.FromSeconds((double) challengeTrophy3.timeRequirement);
        text3 = text3 + "TIME " + MonoMain.TimeString(span, 2) + " ";
      }
      else if (flag)
        text3 = "ANY TIME ";
      if (challengeTrophy3.targets > 0)
      {
        if (text3 != "")
          text3 += ", ";
        text3 = text3 + "|LIME|" + challengeTrophy3.targets.ToString() + " TARGETS";
      }
      if (challengeTrophy3.goodies > 0)
      {
        if (text3 != "")
          text3 += ", ";
        string str3 = "GOODIES";
        if (this._challenge.prefix != "")
          str3 = this._challenge.prefix;
        text3 = text3 + "|ORANGE|" + challengeTrophy3.goodies.ToString() + " " + str3;
      }
      this._font.Draw(text3, x + 22f, num5 + 9f, Color.White, new Depth(1f));
    }
  }
}
