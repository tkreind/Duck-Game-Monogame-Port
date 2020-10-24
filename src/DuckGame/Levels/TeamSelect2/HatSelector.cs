// Decompiled with JetBrains decompiler
// Type: DuckGame.HatSelector
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class HatSelector : Thing, ITakeInput
  {
    public StateBinding _profileBoxNumberBinding = new StateBinding(nameof (profileBoxNumber));
    private sbyte _profileBoxNumber;
    public StateBinding _positionBinding = new StateBinding(nameof (netPosition));
    public StateBinding _fadeBinding = new StateBinding(nameof (_fade));
    public StateBinding _openBinding = new StateBinding(nameof (_open));
    public StateBinding _selectorPositionBinding = new StateBinding(nameof (_selectorPosition));
    public StateBinding _teamSelectionBinding = new StateBinding(nameof (_teamSelection));
    public StateBinding _desiredTeamSelectionBinding = new StateBinding(nameof (_desiredTeamSelection));
    public StateBinding _mainSelectionBinding = new StateBinding(nameof (_mainSelection));
    public StateBinding _selectionBinding = new StateBinding(nameof (selectionInt));
    public StateBinding _lcdFlashBinding = new StateBinding(nameof (_lcdFlash));
    public StateBinding _lcdFlashIncBinding = new StateBinding(nameof (_lcdFlashInc));
    public StateBinding _editingRoomBinding = new StateBinding(nameof (_editingRoom));
    public StateBinding _gettingXPBinding = new StateBinding(nameof (_gettingXP));
    public StateBinding _gettingXPCompletionBinding = new StateBinding(nameof (_gettingXPCompletion));
    public StateBinding _flashTransitionBinding = new StateBinding(nameof (flashTransition));
    public StateBinding _darkenBinding = new StateBinding(nameof (darken));
    public float _fade;
    private float _blackFade;
    public bool _open;
    private bool _closing;
    public bool _gettingXP;
    public float _gettingXPCompletion;
    public bool _editingRoom;
    public short _selectorPosition;
    public short _teamSelection;
    public short _desiredTeamSelection;
    public short _mainSelection;
    public float _slide;
    public float _slideTo;
    public float _upSlide;
    public float _upSlideTo;
    private bool fakefade;
    private string _firstWord = "";
    private string _secondWord = "";
    private InputProfile _blankProfile = new InputProfile();
    private InputProfile _inputProfile;
    private Profile _profile;
    private BitmapFont _font;
    public float _lcdFlash;
    public float _lcdFlashInc;
    private HSSelection _selection = HSSelection.ChooseProfile;
    private ConsoleScreen _screen;
    private ProfileSelector _profileSelector;
    public RoomEditor _roomEditor;
    private Sprite _oButton;
    private ProfileBox2 _box;
    private SpriteMap _demoBox;
    private Sprite _selectBorder;
    private Sprite _consoleText;
    private Sprite _contextArrow;
    private SpriteMap _clueHat;
    private SpriteMap _boardLoader;
    private SpriteMap _lock;
    private SpriteMap _goldLock;
    private SpriteMap _gettingXPBoard;
    private SpriteMap _editingRoomBoard;
    private Sprite _blind;
    public Hat hat;
    public bool isArcadeHatSelector;
    private bool _editRoomDisabled;
    private Team _startingTeam;
    private float _blindLerp;

    public sbyte profileBoxNumber
    {
      get => this._profileBoxNumber;
      set
      {
        if ((int) this._profileBoxNumber != (int) value)
          DevConsole.Log(DCSection.General, "Assigning profile box number (" + (object) value + ")(" + (object) this._profileBoxNumber + ")");
        this._profileBoxNumber = value;
        if (Network.isClient)
        {
          this._profile = DuckNetwork.profiles[(int) this._profileBoxNumber];
          this._inputProfile = this._profile.inputProfile;
          if (Level.current is TeamSelect2 current)
          {
            this._box = current.GetBox((byte) this._profileBoxNumber);
            this._box.SetHatSelector(this);
          }
        }
        if (this._profile == null || this._profile.connection != DuckNetwork.localConnection)
          return;
        this.connection = DuckNetwork.localConnection;
      }
    }

    public new virtual Vec2 netPosition
    {
      get => this.position;
      set => this.position = value;
    }

    public bool flashTransition
    {
      get => this._screen._flashTransition;
      set => this._screen._flashTransition = value;
    }

    public float darken
    {
      get => this._screen._darken;
      set => this._screen._darken = value;
    }

    public float fade
    {
      get => this._fade;
      set => this._fade = value;
    }

    public bool open => this._open;

    public float fadeVal
    {
      get
      {
        if (this.fakefade)
          return 1f;
        float num = this._fade;
        if ((double) this._profileSelector.fade > 0.0)
          num = 1f;
        if ((double) this._roomEditor.fade > 0.0)
          num = 1f;
        return num;
      }
    }

    public string firstWord
    {
      get => this._firstWord;
      set => this._firstWord = value;
    }

    public string secondWord
    {
      get => this._secondWord;
      set => this._secondWord = value;
    }

    public InputProfile profileInput => this._profile != null ? this._profile.inputProfile : this.inputProfile;

    public InputProfile inputProfile
    {
      get
      {
        if (Network.isActive && this.connection != DuckNetwork.localConnection)
          return this._blankProfile;
        return this._profile != null ? this._profile.inputProfile : this._inputProfile;
      }
    }

    public Profile profile => this._profile;

    public float lcdFlash => this._lcdFlash;

    public byte selectionInt
    {
      get => (byte) this._selection;
      set => this._selection = (HSSelection) value;
    }

    public ConsoleScreen screen => this._screen;

    public ProfileBox2 box => this._box;

    public HatSelector(float xpos, float ypos, Profile profile, ProfileBox2 box)
      : base(xpos, ypos)
    {
      this._profile = profile;
      this._inputProfile = this._profile.inputProfile;
      this._box = box;
      if (Network.isServer)
        this._profileBoxNumber = (sbyte) box.controllerIndex;
      this.Construct();
    }

    public HatSelector()
      : base()
      => this.Construct();

    public void Construct()
    {
      this._font = new BitmapFont("biosFontUI", 8, 7);
      this._font.scale = new Vec2(0.5f, 0.5f);
      this._collisionSize = new Vec2(141f, 89f);
      this._oButton = new Sprite("oButton");
      this._demoBox = new SpriteMap("demoCrate", 20, 20);
      this._demoBox.CenterOrigin();
      this._clueHat = new SpriteMap("hats/cluehat", 32, 32);
      this._clueHat.CenterOrigin();
      this._blind = new Sprite("blind");
      this._gettingXPBoard = new SpriteMap("gettingXP", 63, 30);
      this._gettingXPBoard.CenterOrigin();
      this._editingRoomBoard = new SpriteMap("editingRoom", 63, 30);
      this._editingRoomBoard.CenterOrigin();
      this._boardLoader = new SpriteMap("boardLoader", 7, 7);
      this._boardLoader.AddAnimation("idle", 0.2f, true, 0, 1, 2, 3, 4, 5, 6, 7);
      this._boardLoader.CenterOrigin();
      this._boardLoader.SetAnimation("idle");
      this._selectBorder = new Sprite("selectBorder2");
      this._consoleText = new Sprite("corptronConsoleText");
      this._contextArrow = new Sprite("contextArrowRight");
      this._lock = new SpriteMap("arcade/unlockLock", 15, 18);
      this._goldLock = new SpriteMap("arcade/goldUnlockLock", 15, 18);
    }

    public void SetProfile(Profile newProfile) => this._profile = newProfile;

    public void ConfirmProfile() => this._selection = HSSelection.Main;

    public override void Initialize()
    {
      this._profileSelector = new ProfileSelector(this.x, this.y, this._box, this);
      Level.Add((Thing) this._profileSelector);
      this._roomEditor = new RoomEditor(this.x, this.y, this._box, this);
      Level.Add((Thing) this._roomEditor);
      this._screen = new ConsoleScreen(this.x, this.y, this);
    }

    private int ControllerNumber()
    {
      if (Network.isActive)
        return Maths.Clamp((int) this._profileBoxNumber, 0, 3);
      if (this.inputProfile.name == "MPPlayer1")
        return 0;
      if (this.inputProfile.name == "MPPlayer2")
        return 1;
      if (this.inputProfile.name == "MPPlayer3")
        return 2;
      return this.inputProfile.name == "MPPlayer4" ? 3 : 0;
    }

    private void SelectTeam()
    {
      if ((int) this._desiredTeamSelection >= this.AllTeams().Count)
        return;
      this.FilterTeam().Join(this._profile);
    }

    private Team FilterTeam(bool hardFilter = false)
    {
      if (!Network.isActive)
        return this.AllTeams()[(int) this._desiredTeamSelection];
      int index1 = (int) this._desiredTeamSelection;
      if (index1 >= this.AllTeams().Count)
        index1 = this.ControllerNumber();
      if (this._profile != null && this._profile.connection == DuckNetwork.localConnection && !hardFilter)
      {
        if (index1 >= Teams.core.teams.Count)
        {
          Team allTeam = this.AllTeams()[index1];
          index1 = this.ControllerNumber();
        }
        return this.AllTeams()[index1];
      }
      Team allTeam1;
      if (this._profile.connection == DuckNetwork.localConnection)
      {
        if (index1 >= Teams.core.teams.Count)
        {
          Team allTeam2 = this.AllTeams()[index1];
          int index2 = this.ControllerNumber();
          allTeam1 = this.AllTeams()[index2];
          Team.MapFacade(DG.localID, allTeam2);
          Send.Message((NetMessage) new NMSpecialHat(allTeam2, DG.localID));
        }
        else
        {
          allTeam1 = this.AllTeams()[index1];
          Team.ClearFacade(this._profile.steamID);
          Send.Message((NetMessage) new NMSpecialHat((Team) null, DG.localID));
        }
      }
      else
        allTeam1 = this.AllTeams()[index1];
      return allTeam1;
    }

    public override void Terminate() => base.Terminate();

    public void ConfirmTeamSelection()
    {
      Team t = this.FilterTeam(true);
      if (Network.isActive && this._box.duck != null)
        Send.Message((NetMessage) new NMSetTeam(this._box.duck.profile.networkIndex, (byte) Teams.IndexOf(t)));
      if (t.hasHat)
      {
        if (this._box.duck != null)
        {
          Hat equipment = this._box.duck.GetEquipment(typeof (Hat)) as Hat;
          Hat hat = (Hat) new TeamHat(0.0f, 0.0f, t);
          Level.Add((Thing) hat);
          this._box.duck.Equip((Equipment) hat, false);
          this._box.duck.Fondle((Thing) hat);
          if (this.hat != null)
            Level.Remove((Thing) this.hat);
          this.hat = hat;
          if (equipment != null)
            Level.Remove((Thing) equipment);
        }
        else if (this.hat != null)
          Level.Remove((Thing) this.hat);
      }
      else
      {
        if (this.hat != null)
          Level.Remove((Thing) this.hat);
        this.hat = (Hat) null;
        if (this._box.duck != null && this._box.duck.GetEquipment(typeof (Hat)) is Hat equipment)
        {
          this._box.duck.Unequip((Equipment) equipment);
          Level.Remove((Thing) equipment);
        }
      }
      if (this._desiredTeamSelection <= (short) 3)
        return;
      DuckNetwork.OnTeamSwitch(this._box.duck.profile);
    }

    private int TeamIndexAdd(int index, int plus, bool alwaysThree = true)
    {
      if (alwaysThree && index < 4 && index >= 0)
        index = 3;
      int num = index + plus;
      if (num >= this.AllTeams().Count)
        return num - this.AllTeams().Count + 3;
      return num < 3 ? this.AllTeams().Count + (num - 3) : num;
    }

    private int GetTeamIndex(Team tm)
    {
      int num = 0;
      foreach (Team allTeam in this.AllTeams())
      {
        if (allTeam != tm)
          ++num;
        else
          break;
      }
      return num;
    }

    public void Reset()
    {
      this._open = false;
      this._closing = true;
      this._selection = HSSelection.Main;
      this._mainSelection = (short) 0;
      this._editingRoom = false;
      this._gettingXP = false;
      this._profileSelector.Reset();
      this._roomEditor.Reset();
    }

    public List<Team> AllTeams()
    {
      if (!Network.isActive)
        return Teams.all;
      if (this._profile == null)
        return Teams.core.teams;
      if (this._profile.connection == DuckNetwork.localConnection)
        return Teams.all;
      List<Team> teamList = new List<Team>((IEnumerable<Team>) Teams.core.teams);
      if (this._profile.hasCustomHats)
        teamList.Add(new Team("CUSTOM", "hats/cluehat"));
      return teamList;
    }

    public override void Update()
    {
      bool flag1 = true;
      if (this._profileBoxNumber < (sbyte) 0 || this.inputProfile == null)
        return;
      if (this.connection == DuckNetwork.localConnection && this.inputProfile.Pressed("ANY"))
      {
        HatSelector hatSelector = this;
        hatSelector.authority = ++(hatSelector.authority);
      }
      if (Network.isActive && this.connection == DuckNetwork.localConnection && (Profiles.experienceProfile != null && this.profile.linkedProfile == Profiles.experienceProfile))
      {
        if (MonoMain.pauseMenu != null)
        {
          HatSelector hatSelector = this;
          hatSelector.authority = ++(hatSelector.authority);
          if (MonoMain.pauseMenu is UILevelBox)
          {
            this._gettingXP = true;
            UILevelBox pauseMenu = MonoMain.pauseMenu as UILevelBox;
            this._gettingXPCompletion = (float) (((double) pauseMenu._dayProgress + (double) pauseMenu._xpProgress) / 2.0 * 0.699999988079071);
          }
          else
          {
            this._gettingXPCompletion = 0.7f;
            if (MonoMain.pauseMenu is UIFuneral)
              this._gettingXPCompletion = 0.8f;
            else if (MonoMain.pauseMenu is UIGachaBox)
              this._gettingXPCompletion = 0.9f;
          }
        }
        else
        {
          this._gettingXP = false;
          this._gettingXPCompletion = 0.0f;
        }
      }
      if (Network.isActive && this.connection != null && this.connection.status == ConnectionStatus.Disconnected)
        this._gettingXP = false;
      this._fade = Lerp.Float(this._fade, !this._open || this._profileSelector.open || this._roomEditor.open ? 0.0f : 1f, 0.1f);
      this._blackFade = Lerp.Float(this._blackFade, this._open ? 1f : 0.0f, 0.1f);
      this._screen.Update();
      if (this._screen.transitioning || this._profileSelector.open || this._roomEditor.open)
        return;
      if (Profiles.IsDefault(this._profile))
        flag1 = false;
      if (Profiles.experienceProfile == null || Profiles.experienceProfile.GetTotalFurnitures() <= 0)
        flag1 = false;
      this._editRoomDisabled = false;
      if (NetworkDebugger.enabled)
      {
        flag1 = true;
        this._editRoomDisabled = false;
      }
      else if (!flag1 && Network.isActive)
      {
        flag1 = true;
        this._editRoomDisabled = true;
      }
      if (this.isArcadeHatSelector)
        flag1 = false;
      if (!this._open)
      {
        if ((double) this._fade >= 0.00999999977648258 || !this._closing)
          return;
        this._closing = false;
        this._box.ReturnControl();
      }
      else
      {
        if (this._profile.team == null)
          return;
        this._lcdFlashInc += Rando.Float(0.3f, 0.6f);
        this._lcdFlash = (float) (0.899999976158142 + (Math.Sin((double) this._lcdFlashInc) + 1.0) / 2.0 * 0.100000001490116);
        if ((double) this._slideTo != 0.0 && (double) this._slide != (double) this._slideTo)
          this._slide = Lerp.Float(this._slide, this._slideTo, 0.1f);
        else if ((double) this._slideTo != 0.0 && (double) this._slide == (double) this._slideTo)
        {
          this._slide = 0.0f;
          this._slideTo = 0.0f;
          this._teamSelection = this._desiredTeamSelection;
          Team allTeam = this.AllTeams()[(int) this._desiredTeamSelection];
          if (!Main.isDemo || allTeam.inDemo)
            this.SelectTeam();
        }
        if ((double) this._upSlideTo != 0.0 && (double) this._upSlide != (double) this._upSlideTo)
          this._upSlide = Lerp.Float(this._upSlide, this._upSlideTo, 0.1f);
        else if ((double) this._upSlideTo != 0.0 && (double) this._upSlide == (double) this._upSlideTo)
        {
          this._upSlide = 0.0f;
          this._upSlideTo = 0.0f;
          this._teamSelection = this._desiredTeamSelection;
          Team allTeam = this.AllTeams()[(int) this._desiredTeamSelection];
          if (!Main.isDemo || allTeam.inDemo)
            this.SelectTeam();
        }
        if (this._selection == HSSelection.ChooseTeam)
        {
          if ((int) this._desiredTeamSelection == (int) this._teamSelection)
          {
            bool flag2 = false;
            if (this.inputProfile.Down("LEFT"))
            {
              if (this._desiredTeamSelection < (short) 4)
                this._desiredTeamSelection = (short) (this.AllTeams().Count - 1);
              else if (this._desiredTeamSelection == (short) 4)
                this._desiredTeamSelection = (short) this.ControllerNumber();
              else
                --this._desiredTeamSelection;
              this._slideTo = -1f;
              flag2 = true;
              SFX.Play("consoleTick");
            }
            if (this.inputProfile.Down("RIGHT"))
            {
              if ((int) this._desiredTeamSelection >= this.AllTeams().Count - 1)
                this._desiredTeamSelection = (short) this.ControllerNumber();
              else if (this._desiredTeamSelection < (short) 4)
                this._desiredTeamSelection = (short) 4;
              else
                ++this._desiredTeamSelection;
              this._slideTo = 1f;
              flag2 = true;
              SFX.Play("consoleTick");
            }
            if (this.inputProfile.Down("UP"))
            {
              if (this._desiredTeamSelection < (short) 4)
                this._desiredTeamSelection = (short) 0;
              else
                this._desiredTeamSelection -= (short) 3;
              this._desiredTeamSelection -= (short) 5;
              if (this._desiredTeamSelection < (short) 0)
                this._desiredTeamSelection += (short) (this.AllTeams().Count - 3);
              if (this._desiredTeamSelection == (short) 0)
                this._desiredTeamSelection = (short) this.ControllerNumber();
              else
                this._desiredTeamSelection += (short) 3;
              this._upSlideTo = -1f;
              flag2 = true;
              SFX.Play("consoleTick");
            }
            if (this.inputProfile.Down("DOWN"))
            {
              if (this._desiredTeamSelection < (short) 4)
                this._desiredTeamSelection = (short) 0;
              else
                this._desiredTeamSelection -= (short) 3;
              this._desiredTeamSelection += (short) 5;
              if ((int) this._desiredTeamSelection >= this.AllTeams().Count - 3)
                this._desiredTeamSelection -= (short) (this.AllTeams().Count - 3);
              if (this._desiredTeamSelection == (short) 0)
                this._desiredTeamSelection = (short) this.ControllerNumber();
              else
                this._desiredTeamSelection += (short) 3;
              this._upSlideTo = 1f;
              flag2 = true;
              SFX.Play("consoleTick");
            }
            if (this.inputProfile.Pressed("SELECT") && !flag2)
            {
              if (this._profile.team.locked)
              {
                SFX.Play("consoleError");
              }
              else
              {
                SFX.Play("consoleSelect", 0.4f);
                this._selection = HSSelection.Main;
                this._screen.DoFlashTransition();
                this.ConfirmTeamSelection();
              }
            }
            if (this.inputProfile.Pressed("QUACK"))
            {
              this._desiredTeamSelection = (short) this.GetTeamIndex(this._startingTeam);
              this._teamSelection = this._desiredTeamSelection;
              this.SelectTeam();
              this.ConfirmTeamSelection();
              SFX.Play("consoleCancel", 0.4f);
              this._selection = HSSelection.Main;
              this._screen.DoFlashTransition();
            }
          }
          Vec2 position = this.position;
          this.position = Vec2.Zero;
          this._screen.BeginDraw();
          float num1 = -18f;
          this._profile.persona.sprite.alpha = this._fade;
          this._profile.persona.sprite.color = Color.White;
          this._profile.persona.sprite.color = new Color(this._profile.persona.sprite.color.r, this._profile.persona.sprite.color.g, this._profile.persona.sprite.color.b);
          this._profile.persona.sprite.depth = new Depth(0.9f);
          this._profile.persona.sprite.scale = new Vec2(1f, 1f);
          DuckGame.Graphics.Draw((Sprite) this._profile.persona.sprite, this.x + 70f, this.y + 60f + num1, new Depth(0.9f));
          short num2 = 0;
          bool flag3 = false;
          if ((int) this._teamSelection >= this.AllTeams().Count)
          {
            num2 = this._teamSelection;
            this._teamSelection = (short) (this.AllTeams().Count - 1);
            flag3 = true;
          }
          int count = this.AllTeams().Count;
          for (int index1 = 0; index1 < 5; ++index1)
          {
            for (int index2 = 0; index2 < 7; ++index2)
            {
              int plus = index2 - 3 + (index1 - 2) * 5;
              float x = (float) ((double) this.x + 2.0 + (double) (index2 * 22) + -(double) this._slide * 20.0);
              float num3 = (float) ((double) this.y + 37.0 + -(double) this._upSlide * 20.0);
              int index3 = this.TeamIndexAdd((int) this._teamSelection, plus);
              if (index3 == 3)
                index3 = this.ControllerNumber();
              Team allTeam = this.AllTeams()[index3];
              float num4 = (float) ((double) this.x + ((double) this.x + 2.0 + 154.0 - (double) (this.x + 2f)) / 2.0 - 9.0);
              float num5 = Maths.Clamp((float) ((50.0 - (double) Math.Abs(x - num4)) / 50.0), 0.0f, 1f);
              float num6 = (float) ((double) Maths.NormalizeSection(num5, 0.9f, 1f) * 0.800000011920929 + 0.200000002980232);
              if ((double) num5 < 0.5)
                num6 = Maths.NormalizeSection(num5, 0.1f, 0.2f) * 0.3f;
              num6 = 0.3f;
              float num7 = Maths.NormalizeSection(num5, 0.0f, 0.1f) * 0.3f;
              switch (index1)
              {
                case 0:
                  num3 -= num5 * 3f;
                  num7 = (double) this._upSlide >= 0.0 ? 0.0f : Math.Abs(this._upSlide) * num7;
                  break;
                case 1:
                  num3 -= num5 * 3f;
                  if ((double) this._upSlide > 0.0)
                  {
                    num7 = (1f - Math.Abs(this._upSlide)) * num7;
                    break;
                  }
                  break;
                case 2:
                  float num8 = num3 - (float) ((double) num5 * 4.0 * (1.0 - (double) Math.Abs(this._upSlide)));
                  num3 = (double) this._upSlide <= 0.0 ? num8 + num5 * 4f * Math.Abs(this._upSlide) : num8 - num5 * 3f * Math.Abs(this._upSlide);
                  num7 = Maths.NormalizeSection(num5, 0.9f, 1f) * 0.7f + num7;
                  break;
                case 3:
                  float num9 = Math.Max(0.0f, this._upSlide);
                  num3 += (float) ((double) num5 * 4.0 * (1.0 - (double) num9) + -(double) num5 * 4.0 * (double) num9);
                  if ((double) this._upSlide < 0.0)
                  {
                    num7 = (1f - Math.Abs(this._upSlide)) * num7;
                    break;
                  }
                  break;
                case 4:
                  num3 += num5 * 4f;
                  num7 = (double) this._upSlide <= 0.0 ? 0.0f : Math.Abs(this._upSlide) * num7;
                  break;
              }
              if ((double) num7 >= 0.00999999977648258)
              {
                Main.SpecialCode = "DRAW DUCK SET PROPS";
                this._profile.persona.sprite.alpha = this._fade;
                this._profile.persona.sprite.color = Color.White;
                this._profile.persona.sprite.color = new Color(this._profile.persona.sprite.color.r, this._profile.persona.sprite.color.g, this._profile.persona.sprite.color.b);
                this._profile.persona.sprite.depth = new Depth(0.9f);
                this._profile.persona.sprite.scale = new Vec2(1f, 1f);
                DuckRig.GetHatPoint(this._profile.persona.sprite.imageIndex);
                SpriteMap spriteMap = allTeam.hat;
                Vec2 vec2 = allTeam.hatOffset;
                if (allTeam.locked)
                {
                  spriteMap = this._lock;
                  if (allTeam.name == "Chancy")
                    spriteMap = this._goldLock;
                  vec2 = new Vec2(-10f, -10f);
                }
                bool flag2 = Main.isDemo && !allTeam.inDemo;
                if (flag2)
                  spriteMap = this._demoBox;
                spriteMap.depth = new Depth(0.95f);
                spriteMap.alpha = this._profile.persona.sprite.alpha;
                spriteMap.color = Color.White * num7;
                spriteMap.scale = new Vec2(1f, 1f);
                if (!flag2)
                  spriteMap.center = new Vec2(16f, 16f) + vec2;
                if (index3 > 3 && (double) this._fade > 0.00999999977648258)
                {
                  Vec2 pos = Vec2.Zero;
                  pos = !flag2 ? new Vec2(x, (float) ((double) num3 + (double) num1 + (double) (index1 * 20) - 20.0)) : new Vec2(x + 2f, (float) ((double) num3 + (double) num1 + (double) (index1 * 20) - 20.0 + 1.0));
                  Vec2 pixel = Maths.RoundToPixel(pos);
                  DuckGame.Graphics.Draw((Sprite) spriteMap, pixel.x, pixel.y);
                }
                this._profile.persona.sprite.color = Color.White;
                spriteMap.color = Color.White;
                this._profile.persona.sprite.scale = new Vec2(1f, 1f);
                spriteMap.scale = new Vec2(1f, 1f);
              }
            }
          }
          this._font.alpha = this._fade;
          this._font.depth = new Depth(0.96f);
          Main.SpecialCode = "AFTER DRAW HAT";
          string str1 = "NO PROFILE";
          if (!Profiles.IsDefault(this._profile))
            str1 = this._profile.name;
          if (this._selection == HSSelection.ChooseProfile)
          {
            string str2 = "> " + str1 + " <";
          }
          if (this._selection == HSSelection.ChooseTeam)
          {
            string text = "<              >";
            Vec2 pixel = Maths.RoundToPixel(new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) this._font.GetWidth(text) / 2.0), this.y + 60f + num1));
            this._font.Draw(text, pixel.x, pixel.y, Color.White, new Depth(0.95f));
          }
          Main.SpecialCode = "EXTRA AFTER DRAW HAT";
          string text1 = this._profile.team.name;
          if (text1 == Teams.Player1.name || text1 == Teams.Player2.name || (text1 == Teams.Player3.name || text1 == Teams.Player4.name))
            text1 = "No Team";
          if (this._profile.team.locked)
            text1 = "LOCKED";
          this._font.scale = new Vec2(1f, 1f);
          float width = this._font.GetWidth(text1);
          Vec2 pos1 = new Vec2((float) ((double) this.x + (double) this.width / 2.0 - (double) width / 2.0), this.y + 25f + num1);
          pos1 = Maths.RoundToPixel(pos1);
          this._font.Draw(text1, pos1.x, pos1.y, Color.LimeGreen * (this._selection == HSSelection.ChooseTeam ? 1f : 0.6f), new Depth(0.95f));
          DuckGame.Graphics.DrawLine(pos1 + new Vec2(-10f, 4f), pos1 + new Vec2(width + 10f, 4f), Color.White * 0.1f, 2f, new Depth(0.93f));
          Main.SpecialCode = "WAY AFTER DRAW HAT";
          this._font.Draw("@SELECT@", this.x + 4f, this.y + 79f, new Color(180, 180, 180), new Depth(0.95f), this.profileInput);
          this._font.Draw("@QUACK@", this.x + 122f, this.y + 79f, new Color(180, 180, 180), new Depth(0.95f), this.profileInput);
          this._screen.EndDraw();
          this.position = position;
          if (!flag3)
            return;
          this._teamSelection = num2;
        }
        else
        {
          if (this._selection != HSSelection.Main)
            return;
          if (this.inputProfile.Pressed("UP"))
          {
            if (this._mainSelection > (short) 0)
            {
              --this._mainSelection;
              SFX.Play("consoleTick");
              if (this._editRoomDisabled && this._mainSelection == (short) 2)
                this._mainSelection = (short) 1;
            }
          }
          else if (this.inputProfile.Pressed("DOWN"))
          {
            if ((int) this._mainSelection < (flag1 ? 3 : 2))
            {
              ++this._mainSelection;
              SFX.Play("consoleTick");
            }
            if (this._editRoomDisabled && this._mainSelection == (short) 2)
              this._mainSelection = (short) 3;
          }
          else if (this.inputProfile.Pressed("SELECT"))
          {
            if (this._mainSelection == (short) 1 && !Network.isActive)
            {
              this._profileSelector.Open(this._profile);
              SFX.Play("consoleSelect", 0.4f);
              this._fade = 0.0f;
              this._screen.DoFlashTransition();
            }
            else if (this._mainSelection == (short) 0)
            {
              this._selection = HSSelection.ChooseTeam;
              SFX.Play("consoleSelect", 0.4f);
              this._screen.DoFlashTransition();
            }
            else if ((int) this._mainSelection == (flag1 ? 3 : 2))
            {
              this._open = false;
              this._closing = true;
              SFX.Play("consoleCancel", 0.4f);
              this._selection = HSSelection.Main;
            }
            else if (flag1 && this._mainSelection == (short) 2)
            {
              this._editingRoom = true;
              this._roomEditor.Open(this._profile);
              SFX.Play("consoleSelect", 0.4f);
              this._fade = 0.0f;
              this._screen.DoFlashTransition();
            }
          }
          else if (this.inputProfile.Pressed("QUACK"))
          {
            this._open = false;
            this._closing = true;
            SFX.Play("consoleCancel", 0.4f);
            this._selection = HSSelection.Main;
          }
          else if (this._mainSelection == (short) 1 && this.inputProfile.Pressed("SHOOT") && !Profiles.IsDefault(this._profile))
          {
            this._profileSelector.EditProfile(this._profile);
            SFX.Play("consoleSelect", 0.4f);
            this._fade = 0.0f;
            this._screen.DoFlashTransition();
          }
          this._screen.BeginDraw();
          this._font.scale = new Vec2(1f, 1f);
          string text1 = "@LWING@CUSTOM DUCK@RWING@";
          this._font.Draw(text1, Maths.RoundToPixel(new Vec2((float) ((double) this.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), 10f)), Color.White, new Depth(0.95f));
          string text2 = !Profiles.IsDefault(this._profile) ? this._profile.name : "Pick Profile";
          Vec2 pixel1 = Maths.RoundToPixel(new Vec2((float) ((double) this.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), 39f));
          this._font.Draw(text2, pixel1, Colors.MenuOption * (this._mainSelection == (short) 1 ? 1f : 0.6f), new Depth(0.95f));
          if (this._mainSelection == (short) 1)
            DuckGame.Graphics.Draw(this._contextArrow, pixel1.x - 8f, pixel1.y);
          if (flag1)
          {
            string text3 = "@RAINBOWICON@Edit Room";
            Vec2 pixel2 = Maths.RoundToPixel(new Vec2((float) ((double) this.width / 2.0 - (double) this._font.GetWidth(text3) / 2.0), 48f));
            this._font.Draw(text3, pixel2, this._editRoomDisabled ? Colors.SuperDarkBlueGray : Colors.MenuOption * (this._mainSelection == (short) 2 ? 1f : 0.6f), new Depth(0.95f), colorSymbols: true);
            if (this._mainSelection == (short) 2)
              DuckGame.Graphics.Draw(this._contextArrow, pixel2.x - 8f, pixel2.y);
          }
          string text4 = this._profile.team.hasHat ? "|LIME|" + this._profile.team.name + "|MENUORANGE| HAT" : "|MENUORANGE|Choose Hat";
          Vec2 pixel3 = Maths.RoundToPixel(new Vec2((float) ((double) this.width / 2.0 - (double) this._font.GetWidth(text4) / 2.0), 30f));
          this._font.Draw(text4, pixel3, Color.White * (this._mainSelection == (short) 0 ? 1f : 0.6f), new Depth(0.95f));
          if (this._mainSelection == (short) 0)
            DuckGame.Graphics.Draw(this._contextArrow, pixel3.x - 8f, pixel3.y);
          string text5 = "EXIT";
          Vec2 pixel4 = Maths.RoundToPixel(new Vec2((float) ((double) this.width / 2.0 - (double) this._font.GetWidth(text5) / 2.0), (float) (50 + (flag1 ? 12 : 9))));
          this._font.Draw(text5, pixel4, Colors.MenuOption * ((int) this._mainSelection == (flag1 ? 3 : 2) ? 1f : 0.6f), new Depth(0.95f));
          if ((int) this._mainSelection == (flag1 ? 3 : 2))
            DuckGame.Graphics.Draw(this._contextArrow, pixel4.x - 8f, pixel4.y);
          this._font.Draw("@SELECT@", 4f, 79f, new Color(180, 180, 180), new Depth(0.95f), this.profileInput);
          this._font.Draw(this._mainSelection != (short) 1 || Profiles.IsDefault(this._profile) ? "@QUACK@" : "@SHOOT@", 122f, 79f, new Color(180, 180, 180), new Depth(0.95f), this.profileInput);
          this._consoleText.color = new Color(140, 140, 140);
          DuckGame.Graphics.Draw(this._consoleText, 30f, 18f);
          this._screen.EndDraw();
        }
      }
    }

    public void Open(Profile p)
    {
      this._profile = p;
      this._startingTeam = this._profile.team;
      this._open = true;
      this._mainSelection = (short) 0;
      this._editingRoom = false;
      this._gettingXP = false;
      this._selection = HSSelection.Main;
      this._teamSelection = this._desiredTeamSelection = (short) this.GetTeamIndex(this._profile.team);
    }

    public override void Draw()
    {
      if (this._profileBoxNumber < (sbyte) 0)
        return;
      this.fakefade = false;
      if (Network.isActive && this._box.profile != null && this._box.profile.connection != DuckNetwork.localConnection)
      {
        this._blindLerp = Lerp.Float(this._blindLerp, this._editingRoom || this._gettingXP ? 1f : 0.0f, 0.05f);
        if ((double) this._blindLerp > 0.00999999977648258)
        {
          for (int index = 0; index < 8; ++index)
          {
            this._blind.yscale = Math.Max(0.0f, Math.Min((float) ((double) this._blindLerp * 3.0 - (double) index * 0.0500000007450581), 1f));
            this._blind.depth = (Depth) (float) (0.910000026226044 + (double) index * 0.00800000037997961);
            this._blind.flipH = false;
            DuckGame.Graphics.Draw(this._blind, (float) ((double) this.x - 3.0 + (double) index * (9.0 * (double) this._blindLerp)), this.y + 1f);
            this._blind.flipH = true;
            DuckGame.Graphics.Draw(this._blind, (float) ((double) this.x + 4.0 + 140.0 - (double) index * (9.0 * (double) this._blindLerp)), this.y + 1f);
          }
          float num = Math.Max((float) (((double) this._blindLerp - 0.5) * 2.0), 0.0f);
          if ((double) num > 0.00999999977648258)
          {
            if (this._gettingXP)
            {
              this._gettingXPBoard.depth = new Depth(0.99f);
              this._gettingXPBoard.frame = (int) Math.Round((double) this._gettingXPCompletion * 9.0);
              DuckGame.Graphics.Draw((Sprite) this._gettingXPBoard, this.x + 71f, this.y + 43f * num);
              this._boardLoader.depth = new Depth(0.995f);
              DuckGame.Graphics.Draw((Sprite) this._boardLoader, this.x + 94f, this.y + 52f * num);
            }
            else if (this._editingRoom)
            {
              this._editingRoomBoard.depth = new Depth(0.99f);
              DuckGame.Graphics.Draw((Sprite) this._editingRoomBoard, this.x + 71f, this.y + 43f * num);
              this._boardLoader.depth = new Depth(0.995f);
              DuckGame.Graphics.Draw((Sprite) this._boardLoader, this.x + 94f, this.y + 52f * num);
            }
          }
        }
        if (this._editingRoom)
          this.fakefade = true;
      }
      if ((double) this.fadeVal < 0.00999999977648258 || this._roomEditor._mode == REMode.Place)
        return;
      DuckGame.Graphics.Draw((Tex2D) this._screen.target, this.position + new Vec2(3f, 3f), new Rectangle?(), new Color(this._screen.darken, this._screen.darken, this._screen.darken) * this.fadeVal, 0.0f, Vec2.Zero, new Vec2(0.25f, 0.25f), SpriteEffects.None, new Depth(0.82f));
      this._selectBorder.alpha = this.fadeVal;
      this._selectBorder.depth = new Depth(0.85f);
      DuckGame.Graphics.Draw(this._selectBorder, this.x - 1f, this.y, new Rectangle(0.0f, 0.0f, 4f, (float) this._selectBorder.height));
      DuckGame.Graphics.Draw(this._selectBorder, (float) ((double) this.x - 1.0 + (double) this._selectBorder.width - 4.0), this.y, new Rectangle((float) (this._selectBorder.width - 4), 0.0f, 4f, (float) this._selectBorder.height));
      DuckGame.Graphics.Draw(this._selectBorder, (float) ((double) this.x - 1.0 + 4.0), this.y, new Rectangle(4f, 0.0f, (float) (this._selectBorder.width - 8), 4f));
      DuckGame.Graphics.Draw(this._selectBorder, (float) ((double) this.x - 1.0 + 4.0), this.y + (float) (this._selectBorder.height - 25), new Rectangle(4f, (float) (this._selectBorder.height - 25), (float) (this._selectBorder.width - 8), 25f));
      string firstWord = this._firstWord;
      this._font.scale = new Vec2(1f, 1f);
      this._font.Draw(firstWord, this.x + 25f, this.y + 79f, new Color(163, 206, 39) * this.fadeVal * this._lcdFlash, new Depth(0.9f));
      string secondWord = this._secondWord;
      this._font.scale = new Vec2(1f, 1f);
      this._font.Draw(secondWord, this.x + 116f - this._font.GetWidth(secondWord), this.y + 79f, new Color(163, 206, 39) * this.fadeVal * this._lcdFlash, new Depth(0.9f));
      if (this._selection == HSSelection.ChooseTeam)
      {
        this._firstWord = "OK";
        this._secondWord = "BACK";
      }
      else
      {
        if (this._selection != HSSelection.Main)
          return;
        this._firstWord = "PICK";
        if (this._mainSelection == (short) 1 && !Profiles.IsDefault(this._profile))
          this._secondWord = "EDIT";
        else
          this._secondWord = "EXIT";
      }
    }
  }
}
