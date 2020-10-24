﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UIGachaBox
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UIGachaBox : UIMenu
  {
    private Sprite _frame;
    private BitmapFont _font;
    private FancyBitmapFont _fancyFont;
    private SpriteMap _gachaEgg;
    private Sprite _furni;
    private Sprite _star;
    private Furniture _contains;
    private float gachaY;
    private float gachaSpeed;
    private bool _flash;
    private float yOffset = 150f;
    public bool down = true;
    private float _downWait = 1f;
    private UIMenu _openOnClose;
    private SpriteMap _duckCoin;
    private bool _rare;
    private bool _rareCapsule;
    private string _oldSong;
    private bool played;
    private float _gachaWait;
    private float _openWait;
    public bool finished;
    private bool opened;
    private float _swapWait;
    private bool _swapped;
    private float _starGrow;
    private float _insertCoin;
    private float _insertCoinInc;
    private float _afterInsertWait;
    private bool _chinged;
    private bool doubleUpdating;
    private List<string> numberNames = new List<string>()
    {
      "one",
      "two",
      "three",
      "four",
      "five",
      "six",
      "seven",
      "eight",
      "nine",
      "ten"
    };

    public static Furniture GetRandomFurniture(int minRarity) => UIGachaBox.GetRandomFurniture(minRarity, 1)[0];

    public static List<Furniture> GetRandomFurniture(
      int minRarity,
      int num,
      float rarityMult = 1f,
      bool gacha = false,
      int numDupes = 0)
    {
      List<Furniture> furnitureList = new List<Furniture>();
      IOrderedEnumerable<Furniture> source = RoomEditor.AllFurnis().Where<Furniture>((Func<Furniture, bool>) (x => x.rarity >= minRarity)).OrderBy<Furniture, int>((Func<Furniture, int>) (x => Rando.Int(999999)));
      for (int index = 0; index < num; ++index)
      {
        Furniture winner = (Furniture) null;
        Furniture furniture1 = (Furniture) null;
        List<int> intList = new List<int>();
        foreach (Furniture furniture2 in (IEnumerable<Furniture>) source)
        {
          if (!gacha || furniture2.canGetInGacha)
          {
            if (furniture1 == null)
              furniture1 = furniture2;
            bool flag = Profiles.experienceProfile.GetNumFurnitures((int) furniture2.index) > 0;
            int _max = 35;
            if (furniture2.rarity >= Rarity.VeryRare)
              _max = 10;
            if (furniture2.rarity >= Rarity.SuperRare)
              _max = 6;
            if ((!flag || furniture2.type == FurnitureType.Prop && (Rando.Int(_max) == 0 || numDupes > 0)) && !intList.Contains(furniture2.rarity))
            {
              if (Profiles.experienceProfile.GetNumFurnitures((int) furniture2.index) <= 0 || Rando.Int(2) == 0)
                intList.Add(furniture2.rarity);
              if (furniture1 == null || furniture2.rarity < furniture1.rarity)
                furniture1 = furniture2;
              if ((winner == null || furniture2.rarity > winner.rarity) && (furniture2.rarity == Rarity.Common || Rando.Int((int) ((double) furniture2.rarity * (double) rarityMult)) == 0))
                winner = furniture2;
            }
          }
        }
        if (winner == null)
          winner = furniture1;
        if (Profiles.experienceProfile.GetNumFurnitures((int) winner.index) > 0)
          --numDupes;
        furnitureList.Add(winner);
        if (index != num - 1)
          source = source.Where<Furniture>((Func<Furniture, bool>) (x => x != winner)).OrderBy<Furniture, int>((Func<Furniture, int>) (x => Rando.Int(999999)));
      }
      return furnitureList;
    }

    public UIGachaBox(
      float xpos,
      float ypos,
      float wide = -1f,
      float high = -1f,
      bool rare = false,
      UIMenu openOnClose = null)
      : base("", xpos, ypos, wide, high)
    {
      this._openOnClose = openOnClose;
      this._rare = rare;
      this._duckCoin = new SpriteMap("duckCoin", 18, 18);
      this._duckCoin.CenterOrigin();
      Graphics.fade = 1f;
      this._frame = new Sprite("unlockFrame");
      this._frame.CenterOrigin();
      this._furni = new Sprite("furni/tub");
      this._furni.center = new Vec2((float) (this._furni.width / 2), (float) this._furni.height);
      this._star = new Sprite("prettyStar");
      this._star.CenterOrigin();
      this._font = new BitmapFont("biosFontUI", 8, 7);
      this._fancyFont = new FancyBitmapFont("smallFontGacha");
      this._gachaEgg = new SpriteMap("gachaEgg", 44, 36);
      bool flag = false;
      if (Rando.Int(10) == 5)
        flag = true;
      this._contains = UIGachaBox.GetRandomFurniture(this._rare ? Rarity.VeryVeryRare : Rarity.Common, 1, flag ? 0.75f : (this._rare ? 0.75f : 1f), true)[0];
      this._rareCapsule = this._contains.rarity >= Rarity.VeryVeryRare;
      if (this._rareCapsule)
      {
        this._gachaEgg.frame = 36;
      }
      else
      {
        this._gachaEgg.frame = Rando.Int(2) * 12;
        if (Rando.Int(1000) == 1)
          this._gachaEgg.frame += 9;
        else if (Rando.Int(500) == 1)
          this._gachaEgg.frame += 6;
        else if (Rando.Int(100) == 1)
          this._gachaEgg.frame += 3;
      }
      this._gachaEgg.CenterOrigin();
    }

    public override void OnClose()
    {
      Profiles.Save(Profiles.experienceProfile);
      if (this._openOnClose == null)
        return;
      MonoMain.pauseMenu = (UIComponent) this._openOnClose;
    }

    public override void Open() => base.Open();

    public override void UpdateParts()
    {
      if (!this.doubleUpdating && Input.Down("JUMP"))
      {
        this.doubleUpdating = true;
        this.UpdateParts();
        this.doubleUpdating = false;
      }
      if ((double) this.yOffset < 1.0)
      {
        if ((double) this._insertCoin < 1.0)
        {
          this._insertCoinInc += 0.008f;
          this._insertCoin += this._insertCoinInc;
        }
        else
        {
          if (!this._chinged)
          {
            SFX.Play("ching", pitch: Rando.Float(0.4f, 0.6f));
            this._chinged = true;
          }
          this._insertCoin = 1f;
          if ((double) this._afterInsertWait < 1.0)
          {
            this._afterInsertWait += 0.32f;
          }
          else
          {
            if ((double) this._gachaWait >= 0.5 && !this.played)
            {
              this.played = true;
              SFX.Play("gachaSound", pitch: Rando.Float(-0.1f, 0.1f));
            }
            this._gachaWait += 0.1f;
            if ((double) this._gachaWait >= 1.0)
            {
              this.gachaSpeed += 0.25f;
              if ((double) this.gachaSpeed > 6.0)
                this.gachaSpeed = 6f;
              this.gachaY += this.gachaSpeed;
              if ((double) this.gachaY > 50.0 && (double) this.gachaSpeed > 0.0)
              {
                if ((double) this.gachaSpeed > 0.800000011920929)
                  SFX.Play("gachaBounce", pitch: 0.2f);
                this.gachaY = 50f;
                this.gachaSpeed = (float) (-(double) this.gachaSpeed * 0.400000005960464);
              }
              this._openWait += 0.019f;
              if ((double) this._openWait >= 1.0)
              {
                if (!this.opened)
                {
                  this.opened = true;
                  SFX.Play("gachaOpen", pitch: Rando.Float(0.1f, 0.3f));
                  this._gachaEgg.frame += 2;
                }
                this._swapWait += 0.06f;
                if ((double) this._swapWait >= 1.0)
                {
                  if (!this._swapped)
                  {
                    SFX.Play("harp");
                    HUD.AddCornerControl(HUDCorner.BottomRight, "@SELECT@CONTINUE");
                    Profiles.experienceProfile.SetNumFurnitures((int) this._contains.index, Profiles.experienceProfile.GetNumFurnitures((int) this._contains.index) + 1);
                  }
                  this._starGrow += 0.05f;
                  this._swapped = true;
                }
              }
            }
          }
        }
      }
      this.yOffset = Lerp.FloatSmooth(this.yOffset, this.down ? 150f : 0.0f, 0.4f, 1.1f);
      if (this.down)
      {
        if (this._swapped)
        {
          this.finished = true;
          this.Close();
        }
        else
        {
          this._downWait -= 0.06f;
          if ((double) this._downWait <= 0.0)
          {
            this._downWait = 1f;
            this.down = false;
            SFX.Play("gachaGet", pitch: -0.4f);
          }
        }
      }
      if (this._swapped && Input.Pressed("SELECT"))
      {
        HUD.CloseAllCorners();
        SFX.Play("resume", 0.6f);
        this.down = true;
      }
      base.UpdateParts();
    }

    public override void Draw()
    {
      this.y += this.yOffset;
      this._frame.depth = (Depth) -0.9f;
      Graphics.Draw(this._frame, this.x, this.y);
      this._frame.depth = (Depth) -0.7f;
      Graphics.Draw(this._frame, this.x, this.y, new Rectangle(0.0f, 0.0f, 125f, 36f));
      if (this._swapped)
      {
        this._contains.Draw(this.position + new Vec2(0.0f, 10f), (Depth) -0.8f);
        if ((double) this._starGrow <= 1.0)
        {
          this._star.depth = (Depth) 0.9f;
          this._star.scale = new Vec2((float) (2.5 + (double) this._starGrow * 3.0));
          this._star.alpha = 1f - this._starGrow;
          Graphics.Draw(this._star, this.x, this.y + 10f);
        }
      }
      else if ((double) this.gachaY > 10.0)
      {
        this._gachaEgg.depth = (Depth) -0.8f;
        Graphics.Draw((Sprite) this._gachaEgg, this.x, this.y - 38f + this.gachaY);
      }
      string text1 = "@LWING@NEW TOY@RWING@";
      if (this._rare)
        text1 = "@LWING@RARE TOY@RWING@";
      Vec2 vec2_1 = new Vec2((float) -((double) this._font.GetWidth(text1) / 2.0), -42f);
      this._font.DrawOutline(text1, this.position + vec2_1, this._rare ? Colors.DGYellow : Color.White, Color.Black, this.depth + 2);
      string text2 = "  ???  ";
      if (this._swapped)
        text2 = "} " + this._contains.name + " }";
      this._fancyFont.scale = new Vec2(1f, 1f);
      Vec2 vec2_2 = new Vec2((float) -((double) this._fancyFont.GetWidth(text2) / 2.0), -25f);
      this._fancyFont.DrawOutline(text2, this.position + vec2_2, this._rare || this._swapped && this._rareCapsule ? Colors.DGYellow : Color.White, Color.Black, this.depth + 2);
      this._fancyFont.scale = new Vec2(0.5f, 0.5f);
      if ((double) this._insertCoin > 0.00999999977648258)
      {
        this._duckCoin.frame = this._rare ? 1 : 0;
        this._duckCoin.depth = (Depth) -0.8f;
        Graphics.Draw((Sprite) this._duckCoin, this.x + 40f, (float) ((double) this.y - 100.0 + (double) this._insertCoin * 65.0));
      }
      if (this._swapped)
      {
        string text3 = this._contains.description;
        int num = Profiles.experienceProfile.GetNumFurnitures((int) this._contains.index) - 1;
        if (num > 0)
          text3 = "I've already got " + (num - 1 >= this.numberNames.Count ? num.ToString() : this.numberNames[num - 1]) + " of these...";
        Vec2 vec2_3 = new Vec2((float) -((double) this._fancyFont.GetWidth(text3) / 2.0), 38f);
        this._fancyFont.DrawOutline(text3, this.position + vec2_3, num > 0 ? Colors.DGYellow : Colors.DGGreen, Color.Black, this.depth + 2, 0.5f);
      }
      this.y -= this.yOffset;
    }
  }
}
