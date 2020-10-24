﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UIGachaBoxNew
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UIGachaBoxNew : UIMenu
  {
    private Sprite _frame;
    private BitmapFont _font;
    private FancyBitmapFont _fancyFont;
    private SpriteMap _gachaEgg;
    private Sprite _furni;
    private Sprite _star;
    private Furniture _contains;
    private Sprite _whiteCircle;
    private bool _flash;
    private float yOffset = -250f;
    private UIMenu _openOnClose;
    private SpriteMap _duckCoin;
    private SpriteMap _capsule;
    private SpriteMap _capsuleBorder;
    private bool _rare;
    private bool _rareCapsule;
    private Material _flatColor = (Material) new MaterialFlatColor();
    private MaterialRainbow _rainbowMaterial = new MaterialRainbow();
    private Sprite _rainbow;
    private Sprite _gachaMachine;
    private Sprite _gachaGlass;
    private Sprite _gachaDoor;
    private Sprite _gachaTwister;
    private Sprite _gachaTwisterShadow;
    private SpriteMap _gachaBall;
    private SpriteMap _coin;
    private Sprite _coinSlot;
    private List<Furniture> prizes = new List<Furniture>();
    public int numGenerate = 3;
    public int numGenerateRare = 3;
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
    public bool down = true;
    private float _downWait = 1f;
    private float gachaY;
    private float gachaSpeed;
    private bool doubleUpdating;
    private Vec2 _eggOffset = Vec2.Zero;
    private Vec2 _toyPosition = Vec2.Zero;
    private Vec2 _toyVelocity = Vec2.Zero;
    private Vec2 _lastStick = Vec2.Zero;
    private float _toyAngle;
    private float _toyAngleLerp;
    private bool _coined;
    private float _initialWait;
    private bool didOpenToyCorner;
    private int _prizesGiven;
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
    private float rott;
    private int seed = 359392;

    public UIGachaBoxNew(
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
      this._gachaMachine = new Sprite("arcade/gotcha/machine");
      this._gachaMachine.CenterOrigin();
      this._gachaGlass = new Sprite("arcade/gotcha/glass");
      this._gachaGlass.CenterOrigin();
      this._gachaDoor = new Sprite("arcade/gotcha/door");
      this._gachaDoor.CenterOrigin();
      this._gachaTwister = new Sprite("arcade/gotcha/twister");
      this._gachaTwister.CenterOrigin();
      this._gachaBall = new SpriteMap("arcade/gotcha/balls", 40, 42);
      this._gachaBall.CenterOrigin();
      this._gachaTwisterShadow = new Sprite("arcade/gotcha/twisterShadow");
      this._gachaTwisterShadow.CenterOrigin();
      this._whiteCircle = new Sprite("furni/whiteCircle");
      this._whiteCircle.CenterOrigin();
      this._coin = new SpriteMap("arcade/gotcha/coin", 22, 22);
      this._coin.CenterOrigin();
      this._coinSlot = new Sprite("arcade/gotcha/coinSlot");
      this._coinSlot.CenterOrigin();
      this._rainbow = new Sprite("arcade/rainbow");
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
      this._capsule = new SpriteMap("arcade/egg", 40, 23);
      this._capsule.CenterOrigin();
      this._capsuleBorder = new SpriteMap("arcade/eggBorder", 66, 65);
      this._capsuleBorder.CenterOrigin();
      this._rare = false;
      this.numGenerate = MonoMain.core.gachas;
      this.numGenerateRare = MonoMain.core.rareGachas;
      for (int index = 0; index < this.numGenerate; ++index)
      {
        Furniture furniture = UIGachaBox.GetRandomFurniture(Rarity.Common, 1, gacha: true)[0];
        furniture.ballRot = Rando.Float(360f);
        furniture.rareGen = false;
        this.prizes.Add(furniture);
      }
      for (int index = 0; index < this.numGenerateRare; ++index)
      {
        Furniture furniture = UIGachaBox.GetRandomFurniture(Rarity.VeryVeryRare, 1, 0.4f, true).OrderBy<Furniture, int>((Func<Furniture, int>) (x => -x.rarity)).ElementAt<Furniture>(0);
        furniture.ballRot = Rando.Float(360f);
        furniture.rareGen = true;
        this.prizes.Add(furniture);
      }
      for (int index = 0; index < 3; ++index)
      {
        Furniture furniture = UIGachaBox.GetRandomFurniture(Rarity.Common, 1, gacha: true)[0];
        furniture.ballRot = Rando.Float(360f);
        furniture.rareGen = false;
        this.prizes.Add(furniture);
      }
      this.LoadNextPrize();
      this._gachaEgg.CenterOrigin();
    }

    public int FigureFrame(Furniture f)
    {
      if (f.rarity >= Rarity.SuperRare)
        return 2;
      return f.rarity >= Rarity.VeryVeryRare ? 0 : 1;
    }

    public void LoadNextPrize()
    {
      this._contains = this.prizes.ElementAt<Furniture>(0);
      this._capsule.frame = this.FigureFrame(this._contains);
    }

    public override void OnClose()
    {
      MonoMain.core.gachas = 0;
      MonoMain.core.rareGachas = 0;
      this.numGenerate = 0;
      this.numGenerateRare = 0;
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
      if (!this.down && (double) this.yOffset > -1.0)
      {
        if ((double) this._initialWait < 1.0)
          this._initialWait += 0.06f;
        else if ((double) this._insertCoin < 1.0)
        {
          if (!this._chinged)
          {
            SFX.Play("ching", pitch: Rando.Float(0.4f, 0.6f));
            this._chinged = true;
          }
          this._insertCoinInc += 0.008f;
          this._insertCoin += this._insertCoinInc;
        }
        else
        {
          this._insertCoin = 1f;
          if ((double) this._afterInsertWait < 1.0)
          {
            this._afterInsertWait += 0.2f;
          }
          else
          {
            if ((double) this._gachaWait >= 0.200000002980232 && !this.played)
            {
              this.played = true;
              SFX.Play("gachaSound", pitch: Rando.Float(-0.1f, 0.1f));
            }
            if (!this._coined && (double) this._gachaWait > 0.200000002980232)
            {
              SFX.Play("gachaCoin", pitch: Rando.Float(0.4f, 0.6f));
              this._coined = true;
            }
            this._gachaWait += 0.04f;
            if ((double) this._gachaWait >= 1.0)
            {
              this._gachaWait = 1f;
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
              float num = 8f;
              this._toyVelocity.y += 0.2f;
              Vec2 toyPosition1 = this._toyPosition;
              if ((double) toyPosition1.length > (double) num)
              {
                Vec2 toyPosition2 = this._toyPosition;
                this._toyPosition = toyPosition1.normalized * num;
                Vec2 vec2 = this._toyPosition - toyPosition2;
                this._toyVelocity += vec2;
                if ((double) vec2.length > 1.0)
                  SFX.Play("gachaBounce", pitch: (0.7f + Rando.Float(0.2f)));
                this._toyAngleLerp = Maths.PointDirection(Vec2.Zero, this._toyPosition);
              }
              Vec2 leftStick = InputProfile.active.leftStick;
              if (InputProfile.active.lastActiveDevice is Keyboard)
              {
                if (InputProfile.active.Down("LEFT"))
                  leftStick.x = -1f;
                if (InputProfile.active.Down("RIGHT"))
                  leftStick.x = 1f;
                if (InputProfile.active.Down("UP"))
                  leftStick.y = 1f;
                if (InputProfile.active.Down("DOWN"))
                  leftStick.y = -1f;
              }
              this._toyVelocity += (this._lastStick - leftStick) * new Vec2(2f, -2f);
              this._lastStick = leftStick;
              this._toyVelocity.x = Math.Max(Math.Min(this._toyVelocity.x, 3f), -3f);
              this._toyVelocity.y = Math.Max(Math.Min(this._toyVelocity.y, 3f), -3f);
              this._toyPosition += this._toyVelocity;
              if (!this.opened)
              {
                this._toyAngle = Lerp.FloatSmooth(this._toyAngle, this._toyAngleLerp, 0.1f);
                this._eggOffset = Lerp.Vec2Smooth(this._eggOffset, leftStick * 8f, 0.3f);
              }
              else
              {
                this._toyAngle = Lerp.FloatSmooth(this._toyAngle, -90f, 0.1f);
                this._eggOffset = Lerp.Vec2Smooth(this._eggOffset, Vec2.Zero, 0.3f);
                this._toyPosition = Lerp.Vec2Smooth(this._toyPosition, Vec2.Zero, 0.3f);
              }
              this._openWait += 0.029f;
              if ((double) this._openWait >= 1.0)
              {
                if (!this.didOpenToyCorner)
                {
                  HUD.AddCornerControl(HUDCorner.BottomRight, "@SELECT@OPEN TOY");
                  this.didOpenToyCorner = true;
                }
                if (Input.Pressed("SELECT") && !this.opened)
                {
                  this.opened = true;
                  SFX.Play("gachaOpen", pitch: Rando.Float(0.1f, 0.3f));
                  this._gachaEgg.frame += 2;
                }
                if (this.opened)
                {
                  this._swapWait += 0.06f;
                  if ((double) this._swapWait >= 1.0)
                  {
                    if (!this._swapped)
                    {
                      SFX.Play("harp");
                      HUD.CloseAllCorners();
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
      }
      this.yOffset = Lerp.FloatSmooth(this.yOffset, this.down ? -250f : 0.0f, 0.4f);
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
      if (!this.down && this._swapped && Input.Pressed("SELECT"))
      {
        this.played = false;
        this._gachaWait = 0.0f;
        this._openWait = 0.0f;
        this.finished = false;
        this.opened = false;
        this._swapWait = 0.0f;
        this._swapped = false;
        this._starGrow = 0.0f;
        this._insertCoin = 0.0f;
        this._insertCoinInc = 0.0f;
        this._afterInsertWait = 0.0f;
        this._chinged = false;
        this.gachaY = 0.0f;
        this.gachaSpeed = 0.0f;
        this.doubleUpdating = false;
        ++this._prizesGiven;
        this._eggOffset = Vec2.Zero;
        this._toyPosition = Vec2.Zero;
        this._toyVelocity = Vec2.Zero;
        this._lastStick = Vec2.Zero;
        this._toyAngle = 0.0f;
        this._toyAngleLerp = 0.0f;
        this._coined = false;
        this._initialWait = 0.0f;
        this.didOpenToyCorner = false;
        HUD.CloseAllCorners();
        SFX.Play("resume", 0.6f);
        if (this.prizes.Count > 4)
        {
          this.prizes.RemoveAt(0);
          this.LoadNextPrize();
        }
        else
        {
          this.down = true;
          this._swapped = true;
        }
      }
      base.UpdateParts();
    }

    public override void Draw()
    {
      if (this.animating)
        return;
      this.y += this.yOffset;
      Random generator = Rando.generator;
      Rando.generator = new Random(this.seed);
      this._gachaMachine.depth = (Depth) (-0.8f);
      Graphics.Draw(this._gachaMachine, this.x - 14f, this.y);
      this._coinSlot.depth = (Depth) (-0.795f);
      Graphics.Draw(this._coinSlot, this.x - 13f, this.y - 13f);
      this._coin.depth = new Depth(0.9f);
      for (int index = 0; index < this.numGenerate + this.numGenerateRare - (this._prizesGiven + 1); ++index)
      {
        this._coin.frame = this.numGenerate - (this._prizesGiven + 1) <= 0 || index >= this.numGenerate - (this._prizesGiven + 1) ? 1 : 0;
        this._coin.depth = (Depth) (float) (0.899999976158142 - (double) index * 0.00999999977648258);
        Graphics.Draw((Sprite) this._coin, (float) (16 + index * 4), 16f);
      }
      this._coin.frame = !this._contains.rareGen ? 0 : 1;
      float num1 = Math.Min(this._gachaWait * 2f, 1f);
      this._coin.depth = (Depth) (-0.798f);
      Graphics.Draw((Sprite) this._coin, (float) ((double) this.x - 15.0 + (double) num1 * 21.0), (float) ((double) this.y - 25.0 - 40.0 * (1.0 - (double) this._insertCoin) + (double) num1 * 4.0));
      this._gachaGlass.depth = (Depth) (-0.9f);
      Graphics.Draw(this._gachaGlass, this.x - 14f, this.y - 10f);
      this._gachaDoor.depth = (Depth) (-0.84f);
      Graphics.Draw(this._gachaDoor, this.x - 14f, this.y);
      Vec2 vec2_1 = Vec2.Zero;
      this._gachaBall.depth = (Depth) (-0.85f);
      this._gachaBall.alpha = 0.3f;
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num2 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num2 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num2 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x - 56f + vec2_1.x, this.y - 54f + vec2_1.y);
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num3 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num3 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num3 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x - 26f + vec2_1.x, this.y - 74f + vec2_1.y);
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num4 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num4 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num4 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x - 62f + vec2_1.x, this.y - 94f + vec2_1.y);
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num5 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num5 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num5 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x + 6f + vec2_1.x, this.y - 44f + vec2_1.y);
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num6 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num6 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num6 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x + 31f + vec2_1.x, this.y - 64f + vec2_1.y);
      this._gachaBall.angleDegrees = Rando.Float(360f);
      this._gachaBall.frame = Rando.Int(2);
      float num7 = Rando.Float(4f, 8f);
      vec2_1 = new Vec2((float) Math.Sin((double) this._gachaWait * (double) num7 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f), (float) Math.Sin((double) this._gachaWait * (double) num7 + (double) Rando.Float(4f)) * Rando.Float(1f, 2f));
      Graphics.Draw((Sprite) this._gachaBall, this.x + 8f + vec2_1.x, this.y - 92f + vec2_1.y);
      this._gachaBall.angleDegrees = this.prizes[2].ballRot;
      this._gachaBall.frame = this.FigureFrame(this.prizes[2]);
      Graphics.Draw((Sprite) this._gachaBall, (float) ((double) this.x + 31.0 - 42.0 + (double) this._gachaWait * 42.0), (float) ((double) this.y - 16.0 - 12.0 + (double) this._gachaWait * 12.0));
      this._gachaBall.angleDegrees = this.prizes[1].ballRot;
      this._gachaBall.frame = this.FigureFrame(this.prizes[1]);
      Graphics.Draw((Sprite) this._gachaBall, this.x + 31f, (float) ((double) this.y - 16.0 + (double) this._gachaWait * 42.0));
      this._gachaBall.angleDegrees = this.prizes[0].ballRot;
      this._gachaBall.frame = this.FigureFrame(this.prizes[0]);
      Graphics.Draw((Sprite) this._gachaBall, (float) ((double) this.x + 31.0 - (double) this._gachaWait * 42.0), (float) ((double) this.y - 16.0 + 42.0));
      this._gachaBall.alpha = 1f;
      this._gachaBall.angleDegrees = 0.0f;
      this._gachaBall.frame = Rando.Int(2);
      this._gachaTwister.angleDegrees = this._gachaTwisterShadow.angleDegrees = this._gachaWait * 360f;
      Vec2 vec2_2 = new Vec2(0.0f, -4f);
      this._gachaTwister.depth = (Depth) (-0.1f);
      Graphics.Draw(this._gachaTwister, this.x - 14f + vec2_2.x, this.y + vec2_2.y);
      Vec2 vec2_3 = new Vec2(2f, 2f);
      this._gachaTwisterShadow.depth = (Depth) (-0.11f);
      this._gachaTwisterShadow.alpha = 0.5f;
      Graphics.Draw(this._gachaTwisterShadow, this.x - 14f + vec2_2.x + vec2_3.x, this.y + vec2_2.y + vec2_3.y);
      Material material1 = Graphics.material;
      Graphics.material = (Material) this._rainbowMaterial;
      this._rainbowMaterial.offset += 0.05f;
      this._rainbowMaterial.offset2 += 0.02f;
      this._rainbow.alpha = 0.25f;
      this._rainbow.depth = (Depth) (-0.95f);
      Graphics.Draw(this._rainbow, 0.0f, 0.0f);
      Graphics.material = material1;
      Rando.generator = generator;
      this._frame.depth = (Depth) (-0.9f);
      if (this._swapped)
      {
        this._contains.Draw(this.position + new Vec2(0.0f, 10f), this.depth - 20);
        this._whiteCircle.color = this._contains.group.color;
        this._whiteCircle.depth = this.depth - 30;
        Graphics.Draw(this._whiteCircle, this.position.x, this.position.y + 10f);
        if ((double) this._starGrow <= 1.0)
        {
          this._star.depth = new Depth(0.9f);
          this._star.scale = new Vec2((float) (2.5 + (double) this._starGrow * 3.0));
          this._star.alpha = 1f - this._starGrow;
          Graphics.Draw(this._star, this.x, this.y + 10f);
        }
      }
      else if ((double) this.gachaY > 10.0)
      {
        Vec2 vec2_4 = new Vec2(-25f, 40f);
        float num8 = 0.0f;
        if (this.opened)
          num8 = 3f;
        this._capsule.depth = (Depth) (-0.84f);
        Graphics.Draw((Sprite) this._capsule, this.x + this._eggOffset.x + vec2_4.x, (float) ((double) this.y - 38.0 + (double) this.gachaY - (double) this._eggOffset.y - (10.0 + (double) num8)) + vec2_4.y);
        Material material2 = Graphics.material;
        Graphics.material = this._flatColor;
        this._contains.Draw(new Vec2(this.x + this._eggOffset.x + this._toyPosition.x + vec2_4.x, (float) ((double) this.y - 38.0 + (double) this.gachaY - (double) this._eggOffset.y - 10.0 + (double) this._toyPosition.y + 8.0) + vec2_4.y), /*(Depth) (-0.835f, affectScale: true, halfscale: (!this._swapped), angle: Maths.DegToRad(this._toyAngle + 90f))*/new Depth(-0.835f));
        Graphics.material = material2;
        this._capsule.depth = (Depth) (-0.83f);
        this._capsule.frame += 3;
        Graphics.Draw((Sprite) this._capsule, this.x + this._eggOffset.x + vec2_4.x, (float) ((double) this.y - 38.0 + (double) this.gachaY - (double) this._eggOffset.y + (11.0 + (double) num8)) + vec2_4.y, new Rectangle(0.0f, 2f, (float) this._capsule.width, (float) (this._capsule.height - 2)));
        this._capsule.frame -= 3;
        if ((double) this.gachaY > 30.0 && !this.opened)
        {
          this._capsuleBorder.depth = (Depth) (-0.81f);
          this._capsuleBorder.frame = 0;
          Graphics.Draw((Sprite) this._capsuleBorder, this.x + this._eggOffset.x + vec2_4.x, (float) ((double) this.y - 38.0 + (double) this.gachaY - (double) this._eggOffset.y - 2.0) + vec2_4.y);
        }
      }
      if (this._swapped)
      {
        string text1 = "@LWING@NEW TOY@RWING@";
        if (this._rare)
          text1 = "@LWING@RARE TOY@RWING@";
        Vec2 vec2_4 = new Vec2((float) -((double) this._font.GetWidth(text1) / 2.0), -42f);
        string text2 = "  ???  ";
        if (this._swapped)
          text2 = "} " + this._contains.name + " }";
        this._fancyFont.scale = new Vec2(1f, 1f);
        Vec2 vec2_5 = new Vec2((float) -((double) this._fancyFont.GetWidth(text2) / 2.0), -25f);
        this._fancyFont.DrawOutline(text2, this.position + vec2_5, this._rare || this._swapped && this._rareCapsule ? Colors.DGYellow : Color.White, Color.Black, this.depth + 2);
        Graphics.DrawRect(this.position + new Vec2((float) -((double) this._fancyFont.GetWidth(text2) / 2.0 + 4.0), -26f), this.position + new Vec2((float) ((double) this._fancyFont.GetWidth(text2) / 2.0 + 4.0), -14f), Color.Black, this.depth - 4);
        this._fancyFont.scale = new Vec2(0.5f, 0.5f);
        if ((double) this._insertCoin > 0.00999999977648258)
        {
          this._duckCoin.frame = this._rare ? 1 : 0;
          this._duckCoin.depth = (Depth) (-0.8f);
          Graphics.Draw((Sprite) this._duckCoin, this.x + 40f, (float) ((double) this.y - 100.0 + (double) this._insertCoin * 65.0));
        }
        string text3 = this._contains.description;
        int num8 = Profiles.experienceProfile.GetNumFurnitures((int) this._contains.index) - 1;
        if (num8 > 0)
          text3 = "I've already got " + (num8 - 1 >= this.numberNames.Count ? (num8 - 1).ToString() : this.numberNames[num8 - 1]) + " of these...";
        Vec2 vec2_6 = new Vec2((float) -((double) this._fancyFont.GetWidth(text3) / 2.0), 38f);
        this._fancyFont.DrawOutline(text3, this.position + vec2_6, num8 > 0 ? Colors.DGYellow : Colors.DGGreen, Color.Black, this.depth + 2, 0.5f);
        Graphics.DrawRect(this.position + new Vec2((float) -((double) this._fancyFont.GetWidth(text3) / 2.0 + 4.0), 37f), this.position + new Vec2((float) ((double) this._fancyFont.GetWidth(text3) / 2.0 + 4.0), 44f), Color.Black, this.depth - 4);
        Graphics.DrawRect(new Vec2(-100f, -100f), new Vec2(2000f, 2000f), Color.Black * 0.6f, this.depth - 100);
      }
      this.y -= this.yOffset;
    }
  }
}
