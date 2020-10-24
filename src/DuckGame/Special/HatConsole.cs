﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeHatConsole
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("special")]
  [BaggedProperty("isOnlineCapable", false)]
  public class ArcadeHatConsole : Thing
  {
    private ProfileBox2 _profileBox;
    private Sprite _consoleHighlight;
    private Sprite _consoleFlash;
    private SpriteMap _selectConsole;
    private Sprite _base;
    private PointLight _light;
    private float _consoleFade;
    private Profile _profile;
    private Duck _duck;
    public bool hover;

    public ArcadeHatConsole(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._consoleHighlight = new Sprite("consoleHighlight");
      this._base = new Sprite("hatConsoleBase");
      this._consoleFlash = new Sprite("consoleFlash");
      this._consoleFlash.CenterOrigin();
      this._selectConsole = new SpriteMap("selectConsole", 20, 19);
      this._selectConsole.AddAnimation("idle", 1f, true, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
      this._selectConsole.SetAnimation("idle");
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(0.0f, 0.0f);
      this.depth = new Depth(-0.5f);
      this.graphic = (Sprite) this._selectConsole;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._light = new PointLight(this.x + 9f, this.y + 7f, new Color(160, (int) byte.MaxValue, 160), 70f, new List<LightOccluder>());
      Level.Add((Thing) this._light);
    }

    public void MakeHatSelector(Duck d)
    {
      if (this._profileBox != null || d == null)
        return;
      this._profileBox = new ProfileBox2(9999f, 9999f, d.inputProfile, d.profile, (TeamSelect2) null, 0);
      this._profileBox.duck = d;
      this._profileBox._hatSelector.layer = Layer.HUD;
      this._profileBox._hatSelector.isArcadeHatSelector = true;
      this._profile = d.profile;
      this._duck = d;
      Level.Add((Thing) this._profileBox);
    }

    public bool IsOpen() => this._profileBox._hatSelector.open;

    public void Open()
    {
      if (this._duck == null)
        return;
      this._profileBox._hatSelector.position = new Vec2(85f, 45f);
      this._profileBox._hatSelector.Open(this._duck.profile);
      this._profileBox.OpenCorners();
      SFX.Play("consoleOpen", 0.5f);
    }

    public override void Update()
    {
      bool hover = this.hover;
      Duck duck = Level.Nearest<Duck>(this.x, this.y);
      this.hover = duck != null && (double) (duck.position - (this.position + new Vec2(8f, 0.0f))).length < 16.0;
      if (!hover && this.hover)
        HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@PROFILE");
      else if (hover && !this.hover)
        HUD.CloseAllCorners();
      this._consoleFade = Lerp.Float(this._consoleFade, this.hover ? 1f : 0.0f, 0.1f);
      base.Update();
    }

    public override void Draw()
    {
      if (this._light != null)
      {
        this._consoleFlash.scale = new Vec2(0.75f, 0.75f);
        if (this._selectConsole.imageIndex == 0)
        {
          this._light.visible = true;
          this._consoleFlash.alpha = 0.3f;
        }
        else if (this._selectConsole.imageIndex == 1)
        {
          this._light.visible = true;
          this._consoleFlash.alpha = 0.1f;
        }
        else if (this._selectConsole.imageIndex == 2)
        {
          this._light.visible = false;
          this._consoleFlash.alpha = 0.0f;
        }
      }
      this._consoleFlash.depth = this.depth + 10;
      Graphics.Draw(this._consoleFlash, this.x + 9f, this.y + 7f);
      this._base.depth = this.depth - 10;
      Graphics.Draw(this._base, this.x + 3f, this.y + 13f);
      if ((double) this._consoleFade > 0.00999999977648258)
      {
        this._consoleHighlight.depth = this.depth + 5;
        this._consoleHighlight.alpha = this._consoleFade;
        Graphics.Draw(this._consoleHighlight, this.x, this.y);
      }
      base.Draw();
    }
  }
}
