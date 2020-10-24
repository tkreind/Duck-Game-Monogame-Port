// Decompiled with JetBrains decompiler
// Type: DuckGame.ProfileSelector
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class ProfileSelector : Thing
  {
    private float _fade;
    private bool _open;
    private bool _closing;
    private float _takenFlash;
    private string _name = "";
    private string _maskName = "aaaaaaaaa";
    private List<char> _characters = new List<char>()
    {
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      '{',
      '}',
      ' ',
      '-',
      '!'
    };
    private float _slide;
    private float _slideTo;
    private bool _changeName;
    private int _currentLetter;
    private int _controlPosition = 1;
    private bool _editControl;
    private PSMode _mode;
    private PSMode _desiredMode;
    private PSCreateSelection _createSelection;
    private InputProfile _inputProfile;
    private int _selectorPosition = -1;
    private int _desiredSelectorPosition = -1;
    private Profile _profile;
    private BitmapFont _font;
    private BitmapFont _smallFont;
    private List<Profile> _profiles;
    private ProfileBox2 _box;
    private SpriteMap _happyIcons;
    private SpriteMap _angryIcons;
    private SpriteMap _spinnerArrows;
    private Profile _starterProfile;
    private HatSelector _selector;
    private DeviceInputMapping _configInputMapping;
    private int _configDevice;
    private List<string> _remapTriggers = new List<string>()
    {
      "LEFT",
      "RIGHT",
      "UP",
      "DOWN",
      "START",
      "QUACK",
      "SELECT",
      "STRAFE",
      "JUMP",
      "GRAB",
      "RAGDOLL",
      "SHOOT",
      "LSTICK",
      "RSTICK",
      "LTRIGGER",
      "RTRIGGER"
    };
    private UIMenu _confirmMenu;
    private MenuBoolean _deleteProfile = new MenuBoolean();
    private bool _wasDown;
    private bool _autoSelect;
    private List<DeviceInputMapping> _pendingMaps = new List<DeviceInputMapping>();
    private bool isEditing;
    private float _moodVal = 0.5f;

    public float fade => this._fade;

    public bool open => this._open;

    public ProfileSelector(float xpos, float ypos, ProfileBox2 box, HatSelector sel)
      : base(xpos, ypos)
    {
      this._font = new BitmapFont("biosFontUI", 8, 7);
      this._font.scale = new Vec2(0.5f, 0.5f);
      this._collisionSize = new Vec2(141f, 89f);
      this._spinnerArrows = new SpriteMap("spinnerArrows", 8, 4);
      this._box = box;
      this._selector = sel;
      this._happyIcons = new SpriteMap("happyFace", 16, 16);
      this._happyIcons.CenterOrigin();
      this._angryIcons = new SpriteMap("angryFace", 16, 16);
      this._angryIcons.CenterOrigin();
      this._smallFont = new BitmapFont("smallBiosFont", 7, 6);
      this._smallFont.scale = new Vec2(0.5f, 0.5f);
    }

    public override void Initialize()
    {
      this._confirmMenu = new UIMenu("DELETE PROFILE!?", 320f / 2f, 180f / 2f, 160f, conString: "@SELECT@SELECT @QUACK@OH NO!");
      this._confirmMenu.Add((UIComponent) new UIMenuItem("WHAT? NO!", (UIMenuAction) new UIMenuActionCloseMenu((UIComponent) this._confirmMenu), backButton: true), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("YEAH!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean((UIComponent) this._confirmMenu, this._deleteProfile)), true);
      this._confirmMenu.Close();
      Level.Add((Thing) this._confirmMenu);
      base.Initialize();
    }

    public void Reset()
    {
      this._open = false;
      this._selector.fade = 1f;
      this._fade = 0.0f;
      this._selector.screen.DoFlashTransition();
    }

    public string GetMaskName(int length)
    {
      string str = "";
      for (int index = 0; index < 9; ++index)
        str = index >= length ? str + " " : str + (object) this._maskName[index];
      return str;
    }

    public void SelectDown()
    {
      if (this._desiredSelectorPosition >= this._profiles.Count - 1)
        this._desiredSelectorPosition = -1;
      else
        ++this._desiredSelectorPosition;
      this._slideTo = 1f;
    }

    public void SelectUp()
    {
      if (this._desiredSelectorPosition <= -1)
        this._desiredSelectorPosition = this._profiles.Count - 1;
      else
        --this._desiredSelectorPosition;
      this._slideTo = -1f;
    }

    public int GetCharIndex(char c)
    {
      for (int index = 0; index < this._characters.Count; ++index)
      {
        if ((int) this._characters[index] == (int) c)
          return index;
      }
      return -1;
    }

    public override void Update()
    {
      if (this._selector.screen.transitioning)
        return;
      this._takenFlash = Lerp.Float(this._takenFlash, 0.0f, 0.02f);
      if (!this._open)
      {
        if ((double) this._fade >= 0.00999999977648258 || !this._closing)
          return;
        this._closing = false;
      }
      else
      {
        if (this._configInputMapping != null && this._inputProfile != null && this._configInputMapping.device.productName + this._configInputMapping.device.productGUID != this._inputProfile.lastActiveDevice.productName + this._inputProfile.lastActiveDevice.productGUID)
          this._configInputMapping = Input.GetDefaultMapping(this._inputProfile.lastActiveDevice.productName, this._inputProfile.lastActiveDevice.productGUID, p: (this.isEditing ? this._profile : (Profile) null)).Clone();
        if (this._mode != this._desiredMode)
        {
          this._selector.screen.DoFlashTransition();
          this._mode = this._desiredMode;
        }
        if ((double) this._fade > 0.899999976158142 && this._mode != PSMode.CreateProfile && (this._mode != PSMode.EditProfile && this._mode != PSMode.EditControls) && this._desiredSelectorPosition == this._selectorPosition)
        {
          if (this._profile.inputProfile.Down("UP"))
          {
            this.SelectUp();
            this._wasDown = false;
            if (this._profiles.Count > 0)
              SFX.Play("consoleTick");
          }
          if (this._profile.inputProfile.Down("DOWN"))
          {
            this.SelectDown();
            this._wasDown = true;
            if (this._profiles.Count > 0)
              SFX.Play("consoleTick");
          }
          if (!Profiles.IsDefault(this._profile) && this._selectorPosition != -1 && (this._profile.steamID == 0UL && MonoMain.pauseMenu == null) && this._profile.inputProfile.Pressed("GRAB"))
          {
            MonoMain.pauseMenu = (UIComponent) this._confirmMenu;
            this._confirmMenu.Open();
            SFX.Play("pause", 0.6f);
          }
          if (this._deleteProfile.value)
          {
            this._deleteProfile.value = false;
            Profiles.Delete(this._profile);
            this.SelectUp();
            this._profiles = Profiles.allCustomProfiles;
            this._profiles.Add(Profiles.defaultProfiles[this._box.controllerIndex]);
            this._slide = this._slideTo;
          }
          if (this._profile.inputProfile.Pressed("QUACK"))
          {
            if (Profiles.IsDefault(this._starterProfile))
              this._box.ChangeProfile(this._starterProfile);
            this._open = false;
            this._selector.fade = 1f;
            this._fade = 0.0f;
            this._selector.screen.DoFlashTransition();
            SFX.Play("consoleCancel", 0.4f);
            return;
          }
          if (this._profile.inputProfile.Pressed("SELECT") || this._autoSelect)
          {
            this._autoSelect = false;
            if (this._selectorPosition == -1)
            {
              this._desiredMode = PSMode.CreateProfile;
              this._changeName = true;
              this._currentLetter = 0;
              this._createSelection = PSCreateSelection.ChangeName;
              this._maskName = "aaaaaaaaa";
              this._name = this.GetMaskName(1);
              SFX.Play("consoleSelect", 0.4f);
            }
            else
            {
              if (this._selectorPosition != -1)
              {
                this._box.ChangeProfile(this._profiles[this._selectorPosition]);
                this._profile = this._profiles[this._selectorPosition];
                this._profile.inputProfile = (InputProfile) null;
                this._profile.inputProfile = this._inputProfile;
                Input.ApplyDefaultMapping(this._inputProfile, this._profile);
              }
              this._selector.ConfirmProfile();
              this._open = false;
              this._selector.fade = 1f;
              this._fade = 0.0f;
              this._selector.screen.DoFlashTransition();
              SFX.Play("consoleSelect", 0.4f);
            }
          }
        }
        else if (this._mode == PSMode.EditControls)
        {
          if (!this._editControl)
          {
            if (this._profile.inputProfile.Pressed("UP"))
            {
              if (this._controlPosition > 16)
              {
                this._controlPosition = 8;
              }
              else
              {
                --this._controlPosition;
                if (this._controlPosition == 8)
                  this._controlPosition = 9;
                else if (this._controlPosition < 1)
                  this._controlPosition = 1;
              }
              SFX.Play("consoleTick");
            }
            else if (this._profile.inputProfile.Pressed("DOWN"))
            {
              if (this._controlPosition == 8)
                this._controlPosition = 17;
              else if (this._controlPosition == 16)
                this._controlPosition = 17;
              else if (this._controlPosition < 16)
              {
                ++this._controlPosition;
                if (this._controlPosition > 17)
                  this._controlPosition = 17;
              }
              SFX.Play("consoleTick");
            }
            else if (this._profile.inputProfile.Pressed("LEFT"))
            {
              if (this._controlPosition == 0)
              {
                --this._configDevice;
                if (this._configDevice < 0)
                  this._configDevice = 1;
              }
              else if (this._controlPosition >= 17)
              {
                if (this._controlPosition == 18)
                  this._controlPosition = 17;
              }
              else if (this._controlPosition > 8)
                this._controlPosition -= 8;
              SFX.Play("consoleTick");
            }
            else if (this._profile.inputProfile.Pressed("RIGHT"))
            {
              if (this._controlPosition > 16)
              {
                if (this._controlPosition == 17)
                  this._controlPosition = 18;
              }
              else if (this._controlPosition <= 8)
                this._controlPosition += 8;
              SFX.Play("consoleTick");
            }
            else if (this._profile.inputProfile.Pressed("SELECT"))
            {
              if (this._controlPosition == 17)
              {
                this._pendingMaps.Add(this._configInputMapping);
                this._desiredMode = PSMode.CreateProfile;
                SFX.Play("consoleSelect");
                return;
              }
              if (this._controlPosition == 18)
              {
                this._desiredMode = PSMode.CreateProfile;
                SFX.Play("consoleError");
                return;
              }
              this._editControl = true;
              SFX.Play("consoleTick");
            }
            else if (this._profile.inputProfile.Pressed("QUACK"))
            {
              this._desiredMode = PSMode.CreateProfile;
              SFX.Play("consoleError");
              return;
            }
          }
          else if (Keyboard.Pressed(Keys.Escape))
          {
            this._editControl = false;
            SFX.Play("consoleError");
          }
          else if (this._configInputMapping.RunMappingUpdate(this._remapTriggers[this._controlPosition - 1]))
          {
            this._editControl = false;
            SFX.Play("consoleSelect");
          }
        }
        else if (this._mode == PSMode.CreateProfile)
        {
          if (!this._changeName)
          {
            if (this._createSelection == PSCreateSelection.Controls && this._profile.inputProfile.Pressed("SELECT"))
            {
              this._desiredMode = PSMode.EditControls;
              this._configInputMapping = Input.GetDefaultMapping(this._inputProfile.lastActiveDevice.productName, this._inputProfile.lastActiveDevice.productGUID, p: (this.isEditing ? this._profile : (Profile) null)).Clone();
              SFX.Play("consoleTick");
            }
            if (this._createSelection == PSCreateSelection.Mood)
            {
              if (this._profile.inputProfile.Pressed("LEFT"))
              {
                this._moodVal = Maths.Clamp(this._moodVal - 0.25f, 0.0f, 1f);
                SFX.Play("consoleTick");
              }
              if (this._profile.inputProfile.Pressed("RIGHT"))
              {
                this._moodVal = Maths.Clamp(this._moodVal + 0.25f, 0.0f, 1f);
                SFX.Play("consoleTick");
              }
            }
            if (this._profile.inputProfile.Pressed("DOWN") && this._name != "" && this._createSelection < PSCreateSelection.Accept)
            {
              ++this._createSelection;
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("UP") && this._name != "" && this._createSelection > PSCreateSelection.ChangeName)
            {
              --this._createSelection;
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("SELECT"))
            {
              if (this._createSelection == PSCreateSelection.ChangeName)
              {
                if (!this.isEditing)
                {
                  this._changeName = true;
                  if (this._name == "")
                    this._name = this.GetMaskName(1);
                  SFX.Play("consoleSelect", 0.4f);
                }
                else
                  SFX.Play("consoleError", 0.8f);
              }
              else if (this._createSelection == PSCreateSelection.Accept)
              {
                string varName = this._name.Replace(" ", "");
                Profile profile = this._profile;
                if (!this.isEditing)
                  profile = new Profile(varName);
                profile.funslider = this._moodVal;
                profile.inputMappingOverrides.Clear();
                foreach (DeviceInputMapping pendingMap in this._pendingMaps)
                  Input.SetDefaultMapping(pendingMap, profile);
                profile.inputProfile = this._inputProfile;
                this._pendingMaps.Clear();
                if (!this.isEditing)
                  Profiles.Add(profile);
                Input.ApplyDefaultMapping(this._inputProfile, this._profile);
                if (!this.isEditing)
                {
                  this._profiles = Profiles.allCustomProfiles;
                  this._profiles.Add(Profiles.defaultProfiles[this._box.controllerIndex]);
                  for (int index = 0; index < this._profiles.Count; ++index)
                  {
                    if (this._profiles[index].name == varName)
                    {
                      this._selectorPosition = index;
                      this._desiredSelectorPosition = this._selectorPosition;
                      break;
                    }
                  }
                  this._desiredMode = PSMode.SelectProfile;
                  this._autoSelect = true;
                }
                else
                {
                  Profiles.Save(this._profile);
                  this._desiredMode = PSMode.SelectProfile;
                  this._mode = PSMode.SelectProfile;
                  this._open = false;
                  this._selector.fade = 1f;
                  this._fade = 0.0f;
                  this._selector.screen.DoFlashTransition();
                }
                SFX.Play("consoleSelect", 0.4f);
              }
            }
            if (this._profile.inputProfile.Pressed("QUACK"))
            {
              if (!this.isEditing)
              {
                this._desiredMode = PSMode.SelectProfile;
              }
              else
              {
                this._desiredMode = PSMode.SelectProfile;
                this._mode = PSMode.SelectProfile;
                this._open = false;
                this._selector.fade = 1f;
                this._fade = 0.0f;
                this._selector.screen.DoFlashTransition();
              }
              SFX.Play("consoleCancel", 0.4f);
            }
          }
          else
          {
            AnalogGamePad.repeat = true;
            Keyboard.repeat = true;
            if (this._profile.inputProfile.Pressed("SELECT"))
            {
              string str = this._name.Replace(" ", "");
              List<Profile> allCustomProfiles = Profiles.allCustomProfiles;
              bool flag = false;
              foreach (Profile profile in allCustomProfiles)
              {
                if (profile.name == str)
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
                this._takenFlash = 1f;
              else
                this._changeName = false;
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("LEFT"))
            {
              --this._currentLetter;
              if (this._currentLetter < 0)
              {
                this._currentLetter = 0;
              }
              else
              {
                this._name = this._name.Remove(this._currentLetter + 1, 1);
                this._name = this._name.Insert(this._currentLetter + 1, " ");
              }
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("RIGHT"))
            {
              ++this._currentLetter;
              if (this._currentLetter > 8)
              {
                this._currentLetter = 8;
              }
              else
              {
                this._name = this._name.Remove(this._currentLetter, 1);
                if (this._currentLetter > 0)
                  this._name = this._name.Insert(this._currentLetter, string.Concat((object) this._name[this._currentLetter - 1]));
              }
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("UP"))
            {
              int index = this.GetCharIndex(this._name[this._currentLetter]) + 1;
              if (index >= this._characters.Count)
                index = 0;
              char character = this._characters[index];
              this._name = this._name.Remove(this._currentLetter, 1);
              this._name = this._name.Insert(this._currentLetter, string.Concat((object) character));
              this._maskName = this._maskName.Remove(this._currentLetter, 1);
              this._maskName = this._maskName.Insert(this._currentLetter, string.Concat((object) character));
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("DOWN"))
            {
              int index = this.GetCharIndex(this._name[this._currentLetter]) - 1;
              if (index < 0)
                index = this._characters.Count - 1;
              char character = this._characters[index];
              this._name = this._name.Remove(this._currentLetter, 1);
              this._name = this._name.Insert(this._currentLetter, string.Concat((object) character));
              this._maskName = this._maskName.Remove(this._currentLetter, 1);
              this._maskName = this._maskName.Insert(this._currentLetter, string.Concat((object) character));
              SFX.Play("consoleTick");
            }
            if (this._profile.inputProfile.Pressed("QUACK"))
            {
              this._desiredMode = PSMode.SelectProfile;
              SFX.Play("consoleCancel", 0.4f);
            }
          }
        }
        if ((double) this._slideTo != 0.0 && (double) this._slide != (double) this._slideTo)
          this._slide = Lerp.Float(this._slide, this._slideTo, 0.1f);
        else if ((double) this._slideTo != 0.0 && (double) this._slide == (double) this._slideTo)
        {
          this._slide = 0.0f;
          this._slideTo = 0.0f;
          if (this._desiredSelectorPosition != -1 && this._profiles[this._desiredSelectorPosition].inputProfile != null && this._profiles[this._desiredSelectorPosition].inputProfile != this._inputProfile)
          {
            this._selectorPosition = this._desiredSelectorPosition;
            if (this._wasDown)
              this.SelectDown();
            else
              this.SelectUp();
          }
          else
          {
            this._selectorPosition = this._desiredSelectorPosition;
            if (this._selectorPosition != -1)
            {
              this._box.ChangeProfile(this._profiles[this._selectorPosition]);
              this._profile = this._profiles[this._selectorPosition];
            }
            else
            {
              this._box.ChangeProfile((Profile) null);
              this._profile = this._box.profile;
            }
          }
        }
        this._font.alpha = this._fade;
        this._font.depth = (Depth) 0.96f;
        this._font.scale = new Vec2(1f, 1f);
        if (this._mode == PSMode.EditControls)
        {
          Vec2 position = this.position;
          this.position = Vec2.Zero;
          this._selector.screen.BeginDraw();
          InputProfile inputProfile = this._inputProfile;
          this._smallFont.scale = new Vec2(1f, 1f);
          float num1 = 6f;
          string text = inputProfile.lastActiveDevice.productName;
          if (text.Length > 15)
            text = text.Substring(0, 15) + "...";
          if (this._controlPosition == 0)
            text = "< " + text + " >";
          this._smallFont.Draw(text, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._smallFont.GetWidth(text) / 2.0), this.y + num1)), Colors.MenuOption * (this._controlPosition == 0 ? 1f : 0.6f), (Depth) 0.95f);
          float num2 = 62f;
          string str1 = "{:|DGBLUE|";
          string str2 = "_";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 1 ? str1 + this._configInputMapping.GetMappingString("LEFT") : str1 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0))), Colors.MenuOption * (this._controlPosition == 1 ? 1f : 0.6f), (Depth) 0.95f);
          string str3 = "/:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 2 ? str3 + this._configInputMapping.GetMappingString("RIGHT") : str3 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 2 ? 1f : 0.6f), (Depth) 0.95f);
          string str4 = "}:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 3 ? str4 + this._configInputMapping.GetMappingString("UP") : str4 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0))), Colors.MenuOption * (this._controlPosition == 3 ? 1f : 0.6f), (Depth) 0.95f);
          string str5 = "~:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 4 ? str5 + this._configInputMapping.GetMappingString("DOWN") : str5 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 4 ? 1f : 0.6f), (Depth) 0.95f);
          string str6 = "START:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 5 ? str6 + this._configInputMapping.GetMappingString("START") : str6 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 5 ? 1f : 0.6f), (Depth) 0.95f);
          string str7 = "QUACK:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 6 ? str7 + this._configInputMapping.GetMappingString("QUACK") : str7 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 6 ? 1f : 0.6f), (Depth) 0.95f);
          string str8 = "ACCEPT:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 7 ? str8 + this._configInputMapping.GetMappingString("SELECT") : str8 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0 + 6.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 7 ? 1f : 0.6f), (Depth) 0.95f);
          string str9 = "STRAFE:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 8 ? str9 + this._configInputMapping.GetMappingString("STRAFE") : str9 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num2, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0 + 6.0 + 6.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 8 ? 1f : 0.6f), (Depth) 0.95f);
          float num3 = 4f;
          string str10 = "JUMP:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 9 ? str10 + this._configInputMapping.GetMappingString("JUMP") : str10 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num3, (float) ((double) this.y + (double) num1 + 8.0))), Colors.MenuOption * (this._controlPosition == 9 ? 1f : 0.6f), (Depth) 0.95f);
          string str11 = "GRAB:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 10 ? str11 + this._configInputMapping.GetMappingString("GRAB") : str11 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num3, (float) ((double) this.y + (double) num1 + 8.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 10 ? 1f : 0.6f), (Depth) 0.95f);
          string str12 = "TRIP:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 11 ? str12 + this._configInputMapping.GetMappingString("RAGDOLL") : str12 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num3, (float) ((double) this.y + (double) num1 + 8.0 + 12.0))), Colors.MenuOption * (this._controlPosition == 11 ? 1f : 0.6f), (Depth) 0.95f);
          string str13 = "FIRE:|DGBLUE|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 12 ? str13 + this._configInputMapping.GetMappingString("SHOOT") : str13 + str2, Maths.RoundToPixel(new Vec2(this.x + this.width / 2f - num3, (float) ((double) this.y + (double) num1 + 8.0 + 12.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 12 ? 1f : 0.6f), (Depth) 0.95f);
          string str14 = "|DGGREEN|LS:|DGYELLOW|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 13 ? str14 + this._configInputMapping.GetMappingString("LSTICK") : str14 + str2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num3 + 34.0), (float) ((double) this.y + (double) num1 + 8.0 + 6.0 + 12.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 13 ? 1f : 0.6f), (Depth) 0.95f);
          string str15 = "|DGGREEN|RS:|DGYELLOW|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 14 ? str15 + this._configInputMapping.GetMappingString("RSTICK") : str15 + str2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num3 + 34.0), (float) ((double) this.y + (double) num1 + 8.0 + 6.0 + 12.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 14 ? 1f : 0.6f), (Depth) 0.95f);
          string str16 = "|DGGREEN|LT:|DGYELLOW|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 15 ? str16 + this._configInputMapping.GetMappingString("LTRIGGER") : str16 + str2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num3 + 34.0), (float) ((double) this.y + (double) num1 + 8.0 + 6.0 + 12.0 + 6.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 15 ? 1f : 0.6f), (Depth) 0.95f);
          string str17 = "|DGGREEN|RT:|DGYELLOW|";
          this._smallFont.Draw(!this._editControl || this._controlPosition != 16 ? str17 + this._configInputMapping.GetMappingString("RTRIGGER") : str17 + str2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num3 + 34.0), (float) ((double) this.y + (double) num1 + 8.0 + 6.0 + 12.0 + 6.0 + 6.0 + 6.0 + 6.0))), Colors.MenuOption * (this._controlPosition == 16 ? 1f : 0.6f), (Depth) 0.95f);
          this._smallFont.Draw("ACCEPT", Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - 48.0), (float) ((double) this.y + (double) num1 + 9.0 + 22.0 + 6.0 + 6.0 + 6.0 + 8.0))), Colors.MenuOption * (this._controlPosition == 17 ? 1f : 0.6f), (Depth) 0.95f);
          this._smallFont.Draw("CANCEL", Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 + 10.0), (float) ((double) this.y + (double) num1 + 9.0 + 22.0 + 6.0 + 6.0 + 6.0 + 8.0))), Colors.MenuOption * (this._controlPosition == 18 ? 1f : 0.6f), (Depth) 0.95f);
          this._font.Draw("@SELECT@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          this._font.Draw("@QUACK@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          this.position = position;
          this._selector.screen.EndDraw();
        }
        else if (this._mode == PSMode.SelectProfile)
        {
          this._pendingMaps.Clear();
          Vec2 position = this.position;
          this.position = Vec2.Zero;
          this._selector.screen.BeginDraw();
          string text1 = "@LWING@PICK PROFILE@RWING@";
          this._font.Draw(text1, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), this.y + 8f)), Color.White, (Depth) 0.95f);
          float num1 = 8f;
          for (int index1 = 0; index1 < 7; ++index1)
          {
            int index2 = this.ProfileIndexAdd(this._selectorPosition, index1 - 3);
            string text2 = "New Profile";
            bool flag1 = true;
            bool flag2 = false;
            if (index2 != -1)
            {
              if (Profiles.IsDefault(this._profiles[index2]))
              {
                text2 = "DEFAULT";
                flag2 = true;
              }
              else
                text2 = this._profiles[index2].name;
              flag1 = false;
              if (this._profiles[index2] == Profiles.experienceProfile)
                text2 = "@RAINBOWICON@|DGBLUE|" + text2 + "|WHITE|";
              else if (this._profiles[index2].steamID != 0UL)
                text2 = "@STEAMICON@|DGBLUE|" + text2 + "|WHITE|";
            }
            string text3 = (string) null;
            if (this._desiredSelectorPosition == index2 && (index1 == 3 || (double) this._slideTo > 0.0 && index1 == 4 || (double) this._slideTo < 0.0 && index1 == 2))
              text3 = "> " + text2 + " <";
            float num2 = (float) ((double) this.y + (double) num1 + 33.0);
            float y = (float) ((double) this.y + (double) num1 + (double) (index1 * 11) + -(double) this._slide * 11.0);
            float num3 = Maths.Clamp((float) ((33.0 - (double) Math.Abs(y - num2)) / 33.0), 0.0f, 1f);
            float num4 = num3 * Maths.NormalizeSection(num3, 0.0f, 0.9f);
            float num5 = 0.2f;
            float num6 = Maths.Clamp((double) num3 >= 0.300000011920929 ? ((double) num3 >= 0.800000011920929 ? Maths.NormalizeSection(num3, 0.8f, 1f) + num5 : num5) : Maths.NormalizeSection(num3, 0.0f, 0.3f) * num5, 0.0f, 1f);
            bool flag3 = false;
            if (index2 != -1 && this._profiles[index2].inputProfile != null && this._profiles[index2].inputProfile != this._inputProfile)
              flag3 = true;
            if (flag3)
              text2 = text2.Replace("|DGBLUE|", "");
            this._font.Draw(text2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), y)), (flag3 ? Color.Red : (flag1 ? Color.Lime : (flag2 ? Colors.DGYellow : Colors.MenuOption))) * num6, (Depth) 0.95f);
            if (text3 != null)
              this._font.Draw(text3, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text3) / 2.0), y)), Color.White, (Depth) 0.92f);
          }
          float y1 = num1 + 32f;
          Graphics.DrawRect(this.position + new Vec2(2f, y1), this.position + new Vec2(138f, y1 + 9f), new Color(30, 30, 30) * this._fade, (Depth) 0.8f);
          this._font.Draw("@SELECT@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          this._font.Draw(Profiles.IsDefault(this._profile) || this._selectorPosition == -1 || this._profile.steamID != 0UL ? "@QUACK@" : "@GRAB@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          this.position = position;
          this._selector.screen.EndDraw();
        }
        else
        {
          if (this._mode != PSMode.CreateProfile)
            return;
          Vec2 position = this.position;
          this.position = Vec2.Zero;
          this._selector.screen.BeginDraw();
          string text1 = "@LWING@New Profile@RWING@";
          this._font.Draw(text1, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), this.y + 8f)), Color.White * (1f - Math.Min(1f, this._takenFlash * 2f)), (Depth) 0.95f);
          string text2 = "Name Taken";
          this._font.Draw(text2, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), this.y + 8f)), Color.Red * this._takenFlash, (Depth) 0.97f);
          string text3 = "NONAME";
          if (this._name != "")
            text3 = this._name;
          Vec2 pos = new Vec2(this.x + 36f, (float) ((double) this.y + 38.0 - 16.0));
          if (this._changeName)
          {
            for (int index = 0; index < 9; ++index)
            {
              Graphics.DrawRect(pos + new Vec2((float) (index * 8), 0.0f), pos + new Vec2((float) (index * 8 + 7), 7f), new Color(60, 60, 60), (Depth) 0.8f);
              if (index == this._currentLetter)
              {
                this._spinnerArrows.frame = 0;
                Vec2 vec2_1 = pos + new Vec2((float) (index * 8), -6f);
                Graphics.Draw((Sprite) this._spinnerArrows, vec2_1.x, vec2_1.y, (Depth) 0.95f);
                this._spinnerArrows.frame = 1;
                Vec2 vec2_2 = pos + new Vec2((float) (index * 8), 9f);
                Graphics.Draw((Sprite) this._spinnerArrows, vec2_2.x, vec2_2.y, (Depth) 0.95f);
                Graphics.DrawRect(pos + new Vec2((float) (index * 8 - 2), -2f), pos + new Vec2((float) (index * 8 + 9), 9f), Color.White * 0.8f, (Depth) 0.97f, false);
              }
            }
            this._font.Draw(text3, Maths.RoundToPixel(pos), Color.Lime * (this._createSelection == PSCreateSelection.ChangeName ? 1f : 0.6f), (Depth) 0.95f);
            string text4 = ">              <";
            this._font.Draw(text4, Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text4) / 2.0), pos.y)), Color.White * (this._createSelection == PSCreateSelection.ChangeName ? 1f : 0.6f), (Depth) 0.95f);
          }
          else
          {
            string text4 = text3.Replace(" ", "");
            if (this._createSelection == PSCreateSelection.ChangeName)
              text4 = "> " + text4 + " <";
            this._font.Draw(text4, Maths.RoundToPixel(new Vec2((float) ((double) this.x + 2.0 + (double) this.width / 2.0 - (double) this._font.GetWidth(text4) / 2.0), pos.y)), Colors.MenuOption * (this._createSelection == PSCreateSelection.ChangeName ? 1f : 0.6f), (Depth) 0.95f);
          }
          float ypos = this.y + 34f;
          string text5 = "            ";
          if (this._createSelection == PSCreateSelection.Mood)
            text5 = "< " + text5 + " >";
          this._font.Draw(text5, (float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text5) / 2.0), ypos, Color.White * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f), (Depth) 0.95f);
          Graphics.DrawLine(new Vec2((float) ((double) this.x + (double) this.width / 4.0 + 4.0), ypos + 5f), new Vec2(this.x + (float) ((double) this.width / 4.0 * 3.0), ypos + 5f), Colors.MenuOption * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f), 2f, (Depth) 0.95f);
          float num = 60f;
          Graphics.DrawLine(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num / 2.0 + (double) num * (double) this._moodVal + 2.0), ypos + 1f), new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num / 2.0 + (double) num * (double) this._moodVal + 2.0), ypos + 4f), Colors.MenuOption * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f), 3f, (Depth) 0.95f);
          Graphics.DrawLine(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num / 2.0 + (double) num * (double) this._moodVal + 2.0), ypos + 6f), new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) num / 2.0 + (double) num * (double) this._moodVal + 2.0), ypos + 9f), Colors.MenuOption * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f), 3f, (Depth) 0.95f);
          this._happyIcons.color = Color.White * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f);
          this._happyIcons.alpha = this._fade;
          this._happyIcons.frame = (int) Math.Round((double) this._moodVal * 4.0);
          this._happyIcons.depth = (Depth) 0.95f;
          Graphics.Draw((Sprite) this._happyIcons, (float) ((double) this.x + (double) this.width / 6.0 + 2.0), ypos + 4f);
          this._angryIcons.color = Color.White * (this._createSelection == PSCreateSelection.Mood ? 1f : 0.6f);
          this._angryIcons.alpha = this._fade;
          this._angryIcons.frame = (int) Math.Round((1.0 - (double) this._moodVal) * 4.0);
          this._angryIcons.depth = (Depth) 0.95f;
          Graphics.Draw((Sprite) this._angryIcons, this.x + (float) ((double) this.width / 6.0 * 5.0), ypos + 4f);
          string text6 = "CONTROLS";
          if (this._createSelection == PSCreateSelection.Controls)
            text6 = "> " + text6 + " <";
          this._font.Draw(text6, Maths.RoundToPixel(new Vec2((float) ((double) this.x + 2.0 + (double) this.width / 2.0 - (double) this._font.GetWidth(text6) / 2.0), this.y + 48f)), Colors.MenuOption * (this._createSelection == PSCreateSelection.Controls ? 1f : 0.6f), (Depth) 0.95f);
          string text7 = "OK";
          if (this._createSelection == PSCreateSelection.Accept)
            text7 = "> " + text7 + " <";
          this._font.Draw(text7, Maths.RoundToPixel(new Vec2((float) ((double) this.x + 2.0 + (double) this.width / 2.0 - (double) this._font.GetWidth(text7) / 2.0), (float) ((double) this.y + 48.0 + 12.0))), Colors.MenuOption * (this._createSelection == PSCreateSelection.Accept ? 1f : 0.6f), (Depth) 0.95f);
          if (this._changeName)
          {
            this._font.Draw("@DPAD@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
            this._font.Draw("@SELECT@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          }
          else if (this._createSelection == PSCreateSelection.ChangeName)
          {
            this._font.Draw("@SELECT@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
            this._font.Draw("@QUACK@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          }
          else if (this._createSelection == PSCreateSelection.Mood)
          {
            this._font.Draw("@DPAD@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
            this._font.Draw("@QUACK@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          }
          else
          {
            this._font.Draw("@SELECT@", 4f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
            this._font.Draw("@QUACK@", 122f, 79f, new Color(180, 180, 180), (Depth) 0.95f, this._profile.inputProfile);
          }
          this.position = position;
          this._selector.screen.EndDraw();
        }
      }
    }

    public void Open(Profile p)
    {
      if (this._box == null && Level.current is TeamSelect2)
        this._box = (Level.current as TeamSelect2).GetBox(p.networkIndex);
      if (this._box == null)
        return;
      this.isEditing = false;
      this._inputProfile = p.inputProfile;
      this._profile = this._starterProfile = p;
      this._profiles = Profiles.allCustomProfiles;
      this._profiles.Add(Profiles.defaultProfiles[this._box.controllerIndex]);
      for (int index = 0; index < this._profiles.Count; ++index)
      {
        if (this._profiles[index] == this._profile)
        {
          this._selectorPosition = index;
          break;
        }
      }
      this._desiredSelectorPosition = this._selectorPosition;
      this._open = true;
      this._fade = 1f;
    }

    public void EditProfile(Profile p)
    {
      this.Open(p);
      this.isEditing = true;
      this._mode = PSMode.EditProfile;
      this._desiredMode = PSMode.EditProfile;
      this._name = p.name;
      this._desiredMode = PSMode.CreateProfile;
      this._changeName = false;
      this._currentLetter = 0;
      this._moodVal = p.funslider;
      this._createSelection = PSCreateSelection.Accept;
    }

    private int ProfileIndexAdd(int index, int plus)
    {
      if (this._profiles.Count == 0)
        return -1;
      int num = index + plus;
      while (num >= this._profiles.Count)
        num -= this._profiles.Count + 1;
      while (num < -1)
        num += this._profiles.Count + 1;
      return num;
    }

    public override void Draw()
    {
      if ((double) this._fade < 0.00999999977648258)
        return;
      if (this._mode == PSMode.CreateProfile)
      {
        if (this._changeName)
        {
          this._selector.firstWord = "MOVE";
          this._selector.secondWord = "OK";
        }
        else if (this._createSelection == PSCreateSelection.ChangeName)
        {
          this._selector.firstWord = "ALTER";
          this._selector.secondWord = "BACK";
        }
        else if (this._createSelection == PSCreateSelection.Mood)
        {
          this._selector.firstWord = "MOVE";
          this._selector.secondWord = "BACK";
        }
        else
        {
          this._selector.firstWord = "OK";
          this._selector.secondWord = "BACK";
        }
      }
      else if (Profiles.IsDefault(this._profile) || this._selectorPosition == -1 || this._profile.steamID != 0UL)
      {
        this._selector.firstWord = "PICK";
        this._selector.secondWord = "BACK";
      }
      else
      {
        this._selector.firstWord = "PICK";
        this._selector.secondWord = "KILL";
      }
    }
  }
}
