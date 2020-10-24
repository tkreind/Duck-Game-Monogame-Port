// Decompiled with JetBrains decompiler
// Type: DuckGame.ProfileBox2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ProfileBox2 : Thing
  {
    private BitmapFont _font;
    private BitmapFont _fontSmall;
    private SinWave _pulse = new SinWave(0.05f);
    private bool _playerActive;
    private int _teamSelection;
    private Sprite _plaque;
    private bool _ready;
    private InputProfile _inputProfile;
    private Profile _playerProfile;
    private Sprite _doorLeft;
    private Sprite _doorRight;
    private Sprite _doorLeftBlank;
    private Sprite _doorRightBlank;
    private SpriteMap _doorSpinner;
    private SpriteMap _doorIcon;
    private Sprite _roomLeftBackground;
    private Sprite _roomLeftForeground;
    private SpriteMap _tutorialMessages;
    private Sprite _tutorialTV;
    private SpriteMap _selectConsole;
    private Sprite _consoleHighlight;
    private Sprite _aButton;
    private Sprite _readySign;
    public float _doorX;
    private int _currentMessage;
    private float _screenFade;
    private float _consoleFade;
    private Vec2 _consolePos = Vec2.Zero;
    private TeamProjector _projector;
    private TeamSelect2 _teamSelect;
    private Profile _defaultProfile;
    private Sprite _hostCrown;
    private Sprite _consoleFlash;
    private SpriteMap _lightBar;
    private SpriteMap _roomSwitch;
    private int _controllerIndex;
    private Duck _duck;
    private VirtualShotgun _gun;
    private Window _window;
    private Vec2 _gunSpawnPoint = Vec2.Zero;
    public HatSelector _hatSelector;
    private DuckNetStatus _prevStatus = DuckNetStatus.EstablishingCommunication;
    public float _tooManyPulse;
    public float _noMorePulse;
    private float _prevDoorX;

    public bool playerActive => this._playerActive;

    public bool ready => this._ready;

    public Profile profile => this._playerProfile;

    public void SetProfile(Profile p)
    {
      this._playerProfile = p;
      if (this._duck == null)
        return;
      this._duck.profile = p;
    }

    public void SetHatSelector(HatSelector s) => this._hatSelector = s;

    public int controllerIndex => this._controllerIndex;

    public ProfileBox2(
      float xpos,
      float ypos,
      InputProfile profile,
      Profile defaultProfile,
      TeamSelect2 teamSelect,
      int index)
      : base(xpos, ypos)
    {
      this._hostCrown = new Sprite("hostCrown");
      this._hostCrown.CenterOrigin();
      this._lightBar = new SpriteMap("lightBar", 2, 1);
      this._lightBar.frame = 0;
      this._roomSwitch = new SpriteMap("roomSwitch", 7, 5);
      this._roomSwitch.frame = 0;
      this._controllerIndex = index;
      this._font = new BitmapFont("biosFont", 8);
      this._fontSmall = new BitmapFont("smallBiosFont", 7, 6);
      this.layer = Layer.Game;
      this._collisionSize = new Vec2(150f, 87f);
      this._plaque = new Sprite("plaque");
      this._plaque.center = new Vec2(16f, 16f);
      this._inputProfile = profile;
      this._playerProfile = defaultProfile;
      this._teamSelection = this.ControllerNumber();
      this._doorLeft = new Sprite("selectDoorLeftPC");
      this._doorLeft.depth = (Depth) 0.905f;
      this._doorRight = new Sprite("selectDoorRight");
      this._doorRight.depth = (Depth) 0.9f;
      this._doorLeftBlank = new Sprite("selectDoorLeftBlank");
      this._doorLeftBlank.depth = (Depth) 0.905f;
      this._doorRightBlank = new Sprite("selectDoorRightBlank");
      this._doorRightBlank.depth = (Depth) 0.9f;
      this._doorSpinner = new SpriteMap("doorSpinner", 25, 25);
      this._doorSpinner.AddAnimation("spin", 0.2f, true, 0, 1, 2, 3, 4, 5, 6, 7);
      this._doorSpinner.SetAnimation("spin");
      this._doorIcon = new SpriteMap("doorSpinner", 25, 25);
      this._teamSelect = teamSelect;
      this._defaultProfile = defaultProfile;
      if (this.rightRoom)
      {
        this._roomSwitch = new SpriteMap("roomSwitchRight", 7, 5);
        this._roomSwitch.frame = 0;
        this._roomLeftBackground = new Sprite("rightRoomBackground");
        this._roomLeftForeground = new Sprite("rightRoomForeground");
        Level.Add((Thing) new InvisibleBlock((float) ((double) this.x - 2.0 + 142.0 - 138.0), this.y + 69f, 138f, 16f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock((float) ((double) this.x - 2.0 + 142.0 - 138.0), this.y - 11f, 138f, 12f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock((float) ((double) this.x + 142.0 - 98.0 - 46.0), this.y + 56f, 50f, 16f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock((float) ((double) this.x + 142.0 + 2.0 - 8.0), this.y, 8f, 100f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock((float) ((double) this.x + 142.0 - 136.0 - 9.0), this.y, 8f, 25f, PhysicsMaterial.Metal));
        ScaffoldingTileset scaffoldingTileset = new ScaffoldingTileset(this.x + 126f, this.y + 63f);
        scaffoldingTileset.neverCheap = true;
        Level.Add((Thing) scaffoldingTileset);
        scaffoldingTileset.depth = (Depth) -0.5f;
        scaffoldingTileset.PlaceBlock();
        scaffoldingTileset.UpdateNubbers();
        Level.Add((Thing) new Platform(this.x + 49f, this.y + 56f, 3f, 5f));
        this._readySign = new Sprite("readyLeft");
      }
      else
      {
        this._roomLeftBackground = new Sprite("leftRoomBackground");
        this._roomLeftForeground = new Sprite("leftRoomForeground");
        Level.Add((Thing) new InvisibleBlock(this.x + 2f, this.y + 69f, 138f, 16f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock(this.x + 2f, this.y - 11f, 138f, 12f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock(this.x + 92f, this.y + 56f, 50f, 16f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock(this.x - 4f, this.y, 8f, 100f, PhysicsMaterial.Metal));
        Level.Add((Thing) new InvisibleBlock(this.x + 135f, this.y, 8f, 25f, PhysicsMaterial.Metal));
        ScaffoldingTileset scaffoldingTileset = new ScaffoldingTileset(this.x + 14f, this.y + 63f);
        scaffoldingTileset.neverCheap = true;
        Level.Add((Thing) scaffoldingTileset);
        scaffoldingTileset.depth = (Depth) -0.5f;
        scaffoldingTileset.PlaceBlock();
        scaffoldingTileset.UpdateNubbers();
        Level.Add((Thing) new Platform(this.x + 89f, this.y + 56f, 3f, 5f));
        this._readySign = new Sprite("readyRight");
      }
      this._gunSpawnPoint = !this.rightRoom ? new Vec2(this.x + 113f, this.y + 42f) : new Vec2((float) ((double) this.x + 142.0 - 118.0), this.y + 42f);
      this._readySign.depth = (Depth) 0.2f;
      this._roomLeftBackground.depth = (Depth) -0.6f;
      this._roomLeftForeground.depth = (Depth) 0.1f;
      this._tutorialMessages = new SpriteMap("tutorialScreensPC", 53, 30);
      this._aButton = new Sprite("aButton");
      this._tutorialTV = new Sprite("tutorialTV");
      this._consoleHighlight = new Sprite("consoleHighlight");
      this._consoleFlash = new Sprite("consoleFlash");
      this._consoleFlash.CenterOrigin();
      this._selectConsole = new SpriteMap("selectConsole", 20, 19);
      this._selectConsole.AddAnimation("idle", 1f, true, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
      this._selectConsole.SetAnimation("idle");
      if (Network.isServer)
      {
        this._hatSelector = new HatSelector(this.x, this.y, this._playerProfile, this);
        this._hatSelector.profileBoxNumber = (sbyte) index;
        Level.Add((Thing) this._hatSelector);
      }
      if (this.rightRoom)
      {
        this._projector = new TeamProjector(this.x + 80f, this.y + 68f, this._playerProfile);
        if (Network.isServer)
          Level.Add((Thing) new ItemSpawner(this.x + 26f, this.y + 54f));
      }
      else
      {
        this._projector = new TeamProjector(this.x + 59f, this.y + 68f, this._playerProfile);
        if (Network.isServer)
          Level.Add((Thing) new ItemSpawner(this.x + 112f, this.y + 54f));
      }
      Level.Add((Thing) this._projector);
    }

    public override void Initialize()
    {
      if (Network.isServer && this._playerProfile != null && (this._playerProfile.connection != null && Network.isActive))
        this.Spawn();
      base.Initialize();
    }

    public void ReturnControl()
    {
      if (this._duck == null)
        return;
      this._duck.immobilized = false;
    }

    public bool rightRoom => this.ControllerNumber() == 1 || this.ControllerNumber() == 3;

    private int ControllerNumber() => this._controllerIndex;

    private void SelectTeam() => Teams.all[this._teamSelection].Join(this._playerProfile);

    public void ChangeProfile(Profile p)
    {
      if (p == null)
        p = this._defaultProfile;
      if (p != this._playerProfile)
      {
        Team team1 = this._playerProfile.team;
        if (team1 != null)
        {
          this._playerProfile.team.Leave(this._playerProfile);
          team1.Join(p);
        }
        if (!Network.isActive)
          p.inputProfile = this._playerProfile.inputProfile;
        p.persona = this._playerProfile.persona;
        if (!Network.isActive)
          this._playerProfile.inputProfile = (InputProfile) null;
        this._playerProfile = p;
        if (this._duck != null)
        {
          if (this._duck.profile.team != null)
          {
            Team team2 = this._duck.profile.team;
            team2.Leave(this._duck.profile);
            team2.Join(this._playerProfile);
          }
          this._duck.profile = this._playerProfile;
          if (Network.isActive)
            this._duck.netProfileIndex = (byte) DuckNetwork.IndexOf(this._playerProfile);
        }
        this._projector.SetProfile(p);
        this._hatSelector.SetProfile(p);
      }
      this.OpenCorners();
    }

    public void OpenCorners()
    {
      if (!(Level.current is ArcadeLevel))
        return;
      HUD.CloseAllCorners();
      HUD.AddCornerCounter(HUDCorner.TopLeft, "@TICKET@ ", new FieldBinding((object) this._playerProfile, "ticketCount"), animateCount: true);
      List<ChallengeSaveData> allSaveData = Challenges.GetAllSaveData(this._playerProfile);
      Dictionary<TrophyType, int> dictionary1 = new Dictionary<TrophyType, int>()
      {
        {
          TrophyType.Bronze,
          0
        },
        {
          TrophyType.Silver,
          0
        },
        {
          TrophyType.Gold,
          0
        },
        {
          TrophyType.Platinum,
          0
        },
        {
          TrophyType.Developer,
          0
        }
      };
      foreach (ChallengeSaveData challengeSaveData in allSaveData)
      {
        if (challengeSaveData.trophy != TrophyType.Baseline)
        {
          Dictionary<TrophyType, int> dictionary2;
          TrophyType trophy;
          (dictionary2 = dictionary1)[trophy = challengeSaveData.trophy] = dictionary2[trophy] + 1;
        }
      }
      string text = "";
      if (dictionary1[TrophyType.Bronze] > 0 || dictionary1[TrophyType.Silver] == 0 && dictionary1[TrophyType.Gold] == 0 && dictionary1[TrophyType.Platinum] == 0)
        text = text + "@BRONZE@" + (object) dictionary1[TrophyType.Bronze];
      if (dictionary1[TrophyType.Silver] > 0)
        text = text + " @SILVER@" + (object) dictionary1[TrophyType.Silver];
      if (dictionary1[TrophyType.Gold] > 0)
        text = text + " @GOLD@" + (object) dictionary1[TrophyType.Gold];
      if (dictionary1[TrophyType.Platinum] > 0)
        text = text + " @PLATINUM@" + (object) dictionary1[TrophyType.Platinum];
      if (dictionary1[TrophyType.Developer] > 0)
        text = text + " @DEVELOPER@" + (object) dictionary1[TrophyType.Developer];
      HUD.AddCornerControl(HUDCorner.TopRight, text);
    }

    public Duck duck
    {
      get => this._duck;
      set => this._duck = value;
    }

    public VirtualShotgun gun
    {
      get => this._gun;
      set => this._gun = value;
    }

    public void CloseDoor()
    {
      this._duck.immobilized = true;
      this._playerActive = false;
      if ((double) this._doorX != 0.0)
        return;
      this.OnDoorClosed();
    }

    public void OnDoorClosed()
    {
      if (!Network.isServer)
        return;
      if (this._playerProfile.team != null)
        this._playerProfile.team.Leave(this._playerProfile);
      if (!Network.isActive)
        this._playerProfile.inputProfile = (InputProfile) null;
      this._playerProfile = Profiles.all.ElementAt<Profile>(this.ControllerNumber());
      this._teamSelection = this.ControllerNumber();
      this.SelectTeam();
      this._playerProfile.team.Leave(this._playerProfile);
      if (!Network.isActive)
        this._playerProfile.inputProfile = this._inputProfile;
      if (this._duck != null)
      {
        this._duck.profile = this._playerProfile;
        if (this._duck.GetEquipment(typeof (Hat)) is Hat equipment)
        {
          this._duck.Unequip((Equipment) equipment);
          Level.Remove((Thing) equipment);
        }
      }
      this.Despawn();
    }

    public void Spawn()
    {
      if (this._duck != null)
      {
        this._teamSelection = this.ControllerNumber();
        this.SelectTeam();
        this.ReturnControl();
      }
      else
      {
        this._gun = new VirtualShotgun(this._gunSpawnPoint.x, this._gunSpawnPoint.y);
        this._gun.roomIndex = (byte) this._controllerIndex;
        Level.Add((Thing) this._gun);
        if (this.rightRoom)
        {
          this._duck = new Duck((float) ((double) this.x + 142.0 - 48.0), this.y + 40f, this._playerProfile);
          this._window = new Window((float) ((double) this.x + 142.0 - 141.0), this.y + 49f);
          this._window.noframe = true;
          Level.Add((Thing) this._window);
        }
        else
        {
          this._duck = new Duck(this.x + 48f, this.y + 40f, this._playerProfile);
          this._window = new Window(this.x + 139f, this.y + 49f);
          this._window.noframe = true;
          Level.Add((Thing) this._window);
        }
        Level.Add((Thing) this._duck);
        if (this._duck == null || !this._duck.HasEquipment(typeof (TeamHat)))
          return;
        this._hatSelector.hat = (Hat) (this._duck.GetEquipment(typeof (TeamHat)) as TeamHat);
      }
    }

    public void Despawn()
    {
      if (Network.isServer)
      {
        if (this._duck != null)
        {
          Thing.Fondle((Thing) this._duck, DuckNetwork.localConnection);
          Level.Remove((Thing) this._duck);
          if (!Network.isActive)
          {
            if (this._duck.ragdoll != null)
              Level.Remove((Thing) this._duck.ragdoll);
          }
          else
            GhostManager.context.IncrementGhostIndex(150);
        }
        if (this._gun != null)
        {
          Thing.Fondle((Thing) this._gun, DuckNetwork.localConnection);
          Level.Remove((Thing) this._gun);
        }
        if (this._window != null)
        {
          this._window.lobbyRemoving = true;
          Thing.Fondle((Thing) this._window, DuckNetwork.localConnection);
          Level.Remove((Thing) this._window);
        }
      }
      this._duck = (Duck) null;
      this._gun = (VirtualShotgun) null;
    }

    public void OpenDoor()
    {
      this._playerActive = true;
      this.SelectTeam();
    }

    public void PrepareDoor()
    {
      if (!Network.isServer || this._duck != null)
        return;
      this.Spawn();
    }

    public void OpenDoor(Duck d) => this._duck = d;

    public override void Update()
    {
      if (Network.isActive)
        this._playerActive = this.profile.networkStatus == DuckNetStatus.Connected;
      if (this._duck != null && this._duck.inputProfile != null)
        this._inputProfile = this._duck.inputProfile;
      if (this._hatSelector == null)
        return;
      if (this._hatSelector.open && this.profile.team == null)
        this._hatSelector.Reset();
      foreach (VirtualShotgun virtualShotgun in Level.current.things[typeof (VirtualShotgun)])
      {
        if ((int) virtualShotgun.roomIndex == this._controllerIndex && virtualShotgun.isServerForObject && (double) virtualShotgun.alpha <= 0.0)
        {
          virtualShotgun.position = this._gunSpawnPoint;
          virtualShotgun.alpha = 1f;
          virtualShotgun.vSpeed = -1f;
        }
      }
      if (this._teamSelect != null && (!Network.isActive || this._hatSelector.connection == DuckNetwork.localConnection) && (!Network.isActive && this._inputProfile.Pressed("START") && !this._hatSelector.open) && (!Network.isActive || !NetworkDebugger.enabled || NetworkDebugger.interfaces[NetworkDebugger.networkDrawingIndex].visible))
      {
        if (!this._playerActive)
        {
          if (!Network.isActive)
            this.OpenDoor();
        }
        else if (!this.ready && !Network.isActive)
          this._teamSelect.OpenPauseMenu(this);
      }
      if (Network.isServer && this._duck == null && this._playerProfile.team != null)
      {
        int num = 0;
        foreach (Team team in Teams.all)
        {
          if (!(team.name == this._playerProfile.team.name))
            ++num;
          else
            break;
        }
        this._teamSelection = num;
        this._playerActive = true;
        this.SelectTeam();
        this.Spawn();
      }
      this._ready = (double) this._doorX > 82.0 && this._duck != null && (this._duck.dead || this._duck.ragdoll == null) && (this._duck.dead || this._duck.immobilized || ((double) this._duck.y < -100.0 || (double) this._duck.y > 300.0) || ((double) this._duck.x < -50.0 || (double) this._duck.x > 400.0)) && (double) this._hatSelector.fade == 0.0 && !this._hatSelector.open;
      if (this._duck != null)
      {
        this._currentMessage = 0;
        bool flag = (double) (this._duck.position - this._consolePos).length < 20.0;
        this._consoleFade = Lerp.Float(this._consoleFade, flag ? 1f : 0.0f, 0.1f);
        if (this._teamSelect != null && flag)
        {
          this._currentMessage = 4;
          this._duck.canFire = false;
          if (this._duck.isServerForObject && this._inputProfile.Pressed("SHOOT") && (!this._hatSelector.open && (double) this._hatSelector.fade < 0.00999999977648258))
          {
            this._duck.immobilized = true;
            this._hatSelector.Open(this._playerProfile);
            this._duck.Fondle((Thing) this._hatSelector);
            SFX.Play("consoleOpen", 0.5f);
          }
        }
        else
          this._duck.canFire = true;
        if (this._hatSelector.hat != null && (double) this._hatSelector.hat.alpha < 0.00999999977648258 && !this._duck.HasEquipment((Equipment) this._hatSelector.hat))
        {
          this._hatSelector.hat.alpha = 1f;
          this._duck.Equip((Equipment) this._hatSelector.hat, false);
        }
        if (this._duck.immobilized && (double) this._hatSelector.fade < 0.00999999977648258 && this._duck.ragdoll == null)
        {
          this._currentMessage = 3;
          this._readySign.color = Lerp.Color(this._readySign.color, Color.LimeGreen, 0.1f);
          if (this._hatSelector.hat != null && !this._duck.HasEquipment((Equipment) this._hatSelector.hat))
          {
            this._hatSelector.hat.alpha = 1f;
            this._duck.Equip((Equipment) this._hatSelector.hat, false);
          }
        }
        else
        {
          this._readySign.color = Lerp.Color(this._readySign.color, Color.Red, 0.1f);
          if (this._gun != null && (double) (this._gun.position - this._duck.position).length < 30.0)
          {
            if (this._duck.holdObject != null)
            {
              this._currentMessage = 2;
              if (flag)
                this._currentMessage = 5;
            }
            else
              this._currentMessage = 1;
          }
        }
      }
      this._prevDoorX = this._doorX;
      this._doorX = Maths.LerpTowards(this._doorX, !this._playerActive || this._playerProfile.team == null ? 0.0f : 83f, 4f);
      if (Network.isActive && this.profile.networkStatus == DuckNetStatus.Disconnected && this._prevStatus != DuckNetStatus.Disconnected || (double) this._doorX == 0.0 && (double) this._prevDoorX != 0.0)
        this.OnDoorClosed();
      if (this._currentMessage != this._tutorialMessages.frame)
      {
        this._screenFade = Maths.LerpTowards(this._screenFade, 0.0f, 0.15f);
        if ((double) this._screenFade < 0.00999999977648258)
          this._tutorialMessages.frame = this._currentMessage;
      }
      else
        this._screenFade = Maths.LerpTowards(this._screenFade, 1f, 0.15f);
      this._prevStatus = this.profile.networkStatus;
    }

    public override void Draw()
    {
      if (this._hatSelector != null && (double) this._hatSelector.fadeVal > 0.899999976158142 && this._hatSelector._roomEditor._mode != REMode.Place)
      {
        this._projector.visible = false;
        if (this._duck == null)
          return;
        this._duck.mindControl = new InputProfile();
      }
      else
      {
        if (this._duck != null)
          this._duck.mindControl = (InputProfile) null;
        this._projector.visible = true;
        if ((double) this._tooManyPulse > 0.00999999977648258)
          Graphics.DrawStringOutline("ROOM FULL", this.position + new Vec2(0.0f, 36f), Color.Red * this._tooManyPulse, Color.Black * this._tooManyPulse, (Depth) 0.95f, scale: 2f);
        if ((double) this._noMorePulse > 0.00999999977648258)
          Graphics.DrawStringOutline(" NO MORE ", this.position + new Vec2(0.0f, 36f), Color.Red * this._noMorePulse, Color.Black * this._noMorePulse, (Depth) 0.95f, scale: 2f);
        this._tooManyPulse = Lerp.Float(this._tooManyPulse, 0.0f, 0.05f);
        this._noMorePulse = Lerp.Float(this._noMorePulse, 0.0f, 0.05f);
        if ((double) this._doorX < 82.0)
        {
          Sprite sprite1 = this._doorLeft;
          Sprite sprite2 = this._doorRight;
          bool flag1 = this.profile.slotType == SlotType.Closed;
          bool flag2 = this.profile.slotType == SlotType.Friend;
          bool flag3 = this.profile.slotType == SlotType.Invite;
          bool flag4 = this.profile.slotType == SlotType.Reserved;
          bool flag5 = this.profile.slotType == SlotType.Local;
          bool flag6 = this.profile.networkStatus != DuckNetStatus.Disconnected;
          if (Network.isActive)
          {
            sprite1 = this._doorLeftBlank;
            sprite2 = this._doorRightBlank;
          }
          else
          {
            flag1 = false;
            flag2 = false;
            flag3 = false;
            flag4 = false;
            flag6 = false;
          }
          Sprite doorLeftBlank = this._doorLeftBlank;
          Sprite doorRightBlank = this._doorRightBlank;
          if (this.rightRoom)
          {
            Rectangle sourceRectangle1 = new Rectangle((float) (int) this._doorX, 0.0f, (float) doorLeftBlank.width, (float) this._doorLeft.height);
            Graphics.Draw(doorLeftBlank, this.x - 1f, this.y, sourceRectangle1);
            Rectangle sourceRectangle2 = new Rectangle((float) (int) -(double) this._doorX, 0.0f, (float) this._doorRight.width, (float) this._doorRight.height);
            Graphics.Draw(doorRightBlank, (float) ((double) this.x - 1.0 + 68.0), this.y, sourceRectangle2);
            if ((double) this._doorX == 0.0)
            {
              this._fontSmall.depth = doorLeftBlank.depth + 10;
              if (!Network.isActive || flag5 && Network.isServer)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 10;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
                this._fontSmall.DrawOutline("PRESS", new Vec2(this.x + 19f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("START", new Vec2(this.x + 86f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag1)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 8;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
              }
              else if (flag6)
              {
                this._doorSpinner.depth = doorLeftBlank.depth + 10;
                Graphics.Draw((Sprite) this._doorSpinner, (float) ((int) this.x + 57), this.y + 31f);
              }
              else if (flag2)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 11;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
                this._fontSmall.DrawOutline("PALS", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("ONLY", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag3)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 12;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
                this._fontSmall.DrawOutline("VIPS", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("ONLY", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag4 && this.profile.reservedUser != null)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 12;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                float num = 120f;
                float x = this.x + 10f;
                Graphics.DrawRect(new Vec2(x, this.y + 35f), new Vec2(x + num, this.y + 52f), Color.Black, doorLeftBlank.depth + 20);
                string text1 = "WAITING FOR";
                this._fontSmall.Draw(text1, new Vec2((float) ((double) x + (double) num / 2.0 - (double) this._fontSmall.GetWidth(text1) / 2.0), this.y + 36f), Color.White, doorLeftBlank.depth + 30);
                string text2 = this.profile.name;
                if (text2.Length > 16)
                  text2 = text2.Substring(0, 16);
                this._fontSmall.Draw(text2, new Vec2((float) ((double) x + (double) num / 2.0 - (double) this._fontSmall.GetWidth(text2) / 2.0), this.y + 44f), Color.White, doorLeftBlank.depth + 30);
              }
              else if (flag5)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 13;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
                this._fontSmall.DrawOutline("HOST", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("SLOT", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 9;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 57), this.y + 31f);
                this._fontSmall.DrawOutline("OPEN", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("SLOT", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
            }
          }
          else
          {
            Rectangle sourceRectangle1 = new Rectangle((float) (int) this._doorX, 0.0f, (float) this._doorLeft.width, (float) this._doorLeft.height);
            Graphics.Draw(doorLeftBlank, this.x, this.y, sourceRectangle1);
            Rectangle sourceRectangle2 = new Rectangle((float) (int) -(double) this._doorX, 0.0f, (float) this._doorRight.width, (float) this._doorRight.height);
            Graphics.Draw(doorRightBlank, this.x + 68f, this.y, sourceRectangle2);
            if ((double) this._doorX == 0.0)
            {
              this._fontSmall.depth = doorLeftBlank.depth + 10;
              if (!Network.isActive || flag5 && Network.isServer)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 10;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                this._fontSmall.DrawOutline("PRESS", new Vec2(this.x + 19f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("START", new Vec2(this.x + 86f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag1)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 8;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
              }
              else if (flag6)
              {
                this._doorSpinner.depth = doorLeftBlank.depth + 10;
                Graphics.Draw((Sprite) this._doorSpinner, (float) ((int) this.x + 58), this.y + 31f);
              }
              else if (flag2)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 11;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                this._fontSmall.DrawOutline("PALS", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("ONLY", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag3)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 12;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                this._fontSmall.DrawOutline("VIPS", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("ONLY", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else if (flag4 && this.profile.reservedUser != null)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 12;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                float num = 120f;
                float x = this.x + 10f;
                Graphics.DrawRect(new Vec2(x, this.y + 35f), new Vec2(x + num, this.y + 52f), Color.Black, doorLeftBlank.depth + 20);
                string text1 = "WAITING FOR";
                this._fontSmall.Draw(text1, new Vec2((float) ((double) x + (double) num / 2.0 - (double) this._fontSmall.GetWidth(text1) / 2.0), this.y + 36f), Color.White, doorLeftBlank.depth + 30);
                string text2 = this.profile.name;
                if (text2.Length > 16)
                  text2 = text2.Substring(0, 16);
                this._fontSmall.Draw(text2, new Vec2((float) ((double) x + (double) num / 2.0 - (double) this._fontSmall.GetWidth(text2) / 2.0), this.y + 44f), Color.White, doorLeftBlank.depth + 30);
              }
              else if (flag5)
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 13;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                this._fontSmall.DrawOutline("HOST", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("SLOT", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
              else
              {
                this._doorIcon.depth = doorLeftBlank.depth + 10;
                this._doorIcon.frame = 9;
                Graphics.Draw((Sprite) this._doorIcon, (float) ((int) this.x + 58), this.y + 31f);
                this._fontSmall.DrawOutline("OPEN", new Vec2(this.x + 22f, this.y + 40f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                this._fontSmall.DrawOutline("SLOT", new Vec2(this.x + 90f, this.y + 40f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
              }
            }
          }
        }
        if (this._playerProfile.team == null || (double) this._doorX <= 0.0)
          return;
        Furniture furniture1 = (Furniture) null;
        if (Profiles.experienceProfile != null)
        {
          List<FurniturePosition> source1 = new List<FurniturePosition>();
          List<FurniturePosition> source2 = new List<FurniturePosition>();
          foreach (FurniturePosition furniturePosition in this.profile.furniturePositions)
          {
            Furniture furniture2 = RoomEditor.GetFurniture((int) furniturePosition.id);
            if (furniture2 != null)
            {
              if (furniture2.group == Furniture.Characters)
              {
                if (!furniturePosition.flip && this.rightRoom)
                {
                  furniturePosition.furniMapping = furniture2;
                  source2.Add(furniturePosition);
                  continue;
                }
                if (furniturePosition.flip && !this.rightRoom)
                {
                  furniturePosition.furniMapping = furniture2;
                  source1.Add(furniturePosition);
                  continue;
                }
              }
              if (furniture2.type == FurnitureType.Theme)
                furniture1 = furniture2;
              else if (furniture2.type != FurnitureType.Font)
              {
                furniture2.sprite.depth = (Depth) (float) ((double) furniture2.deep * (1.0 / 1000.0) - 0.560000002384186);
                furniture2.sprite.frame = (int) furniturePosition.variation;
                Vec2 pos = new Vec2((float) furniturePosition.x, (float) furniturePosition.y);
                furniture2.sprite.flipH = furniturePosition.flip;
                if (this.rightRoom)
                {
                  pos.x = (float) RoomEditor.roomSize - pos.x;
                  furniture2.sprite.flipH = !furniture2.sprite.flipH;
                  --pos.x;
                }
                pos += this.position;
                furniture2.Draw(pos, furniture2.sprite.depth, (int) furniturePosition.variation, this.profile.steamID);
                furniture2.sprite.frame = 0;
                furniture2.sprite.flipH = false;
              }
            }
          }
          if (source2.Count > 0)
          {
            IOrderedEnumerable<FurniturePosition> source3 = source2.OrderBy<FurniturePosition, byte>((Func<FurniturePosition, byte>) (furni => furni.x));
            IEnumerable<FurniturePosition> source4 = source3.Reverse<FurniturePosition>();
            int index1 = 0;
            for (int index2 = 0; index2 < source3.Count<FurniturePosition>(); ++index2)
            {
              FurniturePosition furniturePosition = source3.ElementAt<FurniturePosition>(index2);
              Furniture furniMapping = furniturePosition.furniMapping;
              furniMapping.sprite.depth = (Depth) (float) ((double) furniMapping.deep * (1.0 / 1000.0) - 0.560000002384186);
              furniMapping.sprite.frame = (int) source4.ElementAt<FurniturePosition>(index1).variation;
              Vec2 pos = new Vec2((float) furniturePosition.x, (float) furniturePosition.y);
              furniMapping.sprite.flipH = furniturePosition.flip;
              if (this.rightRoom)
              {
                pos.x = (float) RoomEditor.roomSize - pos.x;
                furniMapping.sprite.flipH = !furniMapping.sprite.flipH;
                --pos.x;
              }
              pos += this.position;
              furniMapping.Draw(pos, furniMapping.sprite.depth, furniMapping.sprite.frame, this.profile.steamID);
              furniMapping.sprite.frame = 0;
              furniMapping.sprite.flipH = false;
              ++index1;
            }
          }
          if (source1.Count > 0)
          {
            IOrderedEnumerable<FurniturePosition> source3 = source1.OrderBy<FurniturePosition, int>((Func<FurniturePosition, int>) (furni => (int) -furni.x));
            IEnumerable<FurniturePosition> source4 = source3.Reverse<FurniturePosition>();
            int index1 = 0;
            for (int index2 = 0; index2 < source3.Count<FurniturePosition>(); ++index2)
            {
              FurniturePosition furniturePosition = source3.ElementAt<FurniturePosition>(index2);
              Furniture furniMapping = furniturePosition.furniMapping;
              furniMapping.sprite.depth = (Depth) (float) ((double) furniMapping.deep * (1.0 / 1000.0) - 0.560000002384186);
              furniMapping.sprite.frame = (int) source4.ElementAt<FurniturePosition>(index1).variation;
              Vec2 pos = new Vec2((float) furniturePosition.x, (float) furniturePosition.y);
              furniMapping.sprite.flipH = furniturePosition.flip;
              if (this.rightRoom)
              {
                pos.x = (float) RoomEditor.roomSize - pos.x;
                furniMapping.sprite.flipH = !furniMapping.sprite.flipH;
                --pos.x;
              }
              pos += this.position;
              furniMapping.Draw(pos, furniMapping.sprite.depth, furniMapping.sprite.frame, this.profile.steamID);
              furniMapping.sprite.frame = 0;
              furniMapping.sprite.flipH = false;
              ++index1;
            }
          }
          if (this._hatSelector._roomEditor._mode == REMode.Place && this._hatSelector._roomEditor.CurFurni().type == FurnitureType.Theme)
            furniture1 = this._hatSelector._roomEditor.CurFurni();
        }
        if (this.rightRoom)
        {
          if (furniture1 == null)
          {
            for (int index = 0; index < 4; ++index)
            {
              if (this.profile.GetLightStatus(index))
              {
                this._lightBar.depth = this._tutorialTV.depth;
                this._lightBar.frame = index;
                Graphics.Draw((Sprite) this._lightBar, this.x + 38f + (float) (index * 3), this.y + 49f);
              }
            }
            this._roomSwitch.depth = this._tutorialTV.depth;
            this._roomSwitch.frame = this.profile.switchStatus ? 1 : 0;
            Graphics.Draw((Sprite) this._roomSwitch, this.x + 52f, this.y + 47f);
          }
          if (furniture1 != null)
          {
            Furniture furniture2 = furniture1;
            furniture2.sprite.flipH = true;
            furniture2.sprite.depth = this._roomLeftForeground.depth;
            furniture2.background.depth = this._roomLeftBackground.depth;
            furniture2.sprite.scale = new Vec2(1f);
            furniture2.background.scale = new Vec2(1f);
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, this.y + 44f, new Rectangle(0.0f, 0.0f, 4f, 87f));
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, (float) ((double) this.y + 44.0 + 68.0), new Rectangle(0.0f, 68f, 141f, 19f));
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, this.y + 44f, new Rectangle(0.0f, 0.0f, 141f, 16f));
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 21f, this.y + 44f, new Rectangle(49f, 0.0f, 92f, 68f));
            furniture2.sprite.depth = this._selectConsole.depth - 20;
            Graphics.Draw((Sprite) furniture2.sprite, (float) ((double) this.x + 70.0 - 4.0), this.y + 44f, new Rectangle(4f, 0.0f, 44f, 54f));
            furniture2.sprite.depth = (Depth) 0.31f;
            Graphics.Draw((Sprite) furniture2.sprite, (float) ((double) this.x + 70.0 - 4.0), (float) ((double) this.y + 44.0 + 54.0), new Rectangle(4f, 54f, 44f, 14f));
            furniture2.sprite.flipH = false;
            furniture2.background.flipH = true;
            Graphics.Draw((Sprite) furniture2.background, this.x + 70f, this.y + 45f);
            furniture2.background.flipH = false;
          }
          else
          {
            Graphics.Draw(this._roomLeftBackground, this.x - 1f, this.y + 1f);
            Graphics.Draw(this._roomLeftForeground, this.x - 1f, this.y + 1f, new Rectangle(0.0f, 0.0f, 49f, 16f));
            Graphics.Draw(this._roomLeftForeground, this.x - 1f, (float) ((double) this.y + 1.0 + 16.0), new Rectangle(0.0f, 16f, 6f, 8f));
            Graphics.Draw(this._roomLeftForeground, this.x - 1f, (float) ((double) this.y + 1.0 + 55.0), new Rectangle(0.0f, 55f, 53f, 13f));
            Graphics.Draw(this._roomLeftForeground, this.x - 1f, (float) ((double) this.y + 1.0 + 68.0), new Rectangle(0.0f, 68f, 141f, 19f));
            Graphics.Draw(this._roomLeftForeground, (float) ((double) this.x - 1.0 + 137.0), this.y + 1f, new Rectangle(137f, 0.0f, 4f, 87f));
          }
          if (Network.isActive && (Network.isServer && this.profile.connection == DuckNetwork.localConnection || this.profile.connection == Network.host))
          {
            this._hostCrown.depth = (Depth) -0.5f;
            Graphics.Draw(this._hostCrown, this.x + 126f, this.y + 23f);
          }
        }
        else
        {
          if (furniture1 == null)
          {
            for (int index = 0; index < 4; ++index)
            {
              if (this.profile.GetLightStatus(index))
              {
                this._lightBar.depth = this._tutorialTV.depth;
                this._lightBar.frame = index;
                Graphics.Draw((Sprite) this._lightBar, this.x + 91f + (float) (index * 3), this.y + 49f);
              }
            }
            this._roomSwitch.depth = this._tutorialTV.depth;
            this._roomSwitch.frame = this.profile.switchStatus ? 1 : 0;
            Graphics.Draw((Sprite) this._roomSwitch, this.x + 81f, this.y + 47f);
          }
          if (furniture1 != null)
          {
            Furniture furniture2 = furniture1;
            furniture2.sprite.depth = this._roomLeftForeground.depth;
            furniture2.background.depth = this._roomLeftBackground.depth;
            furniture2.sprite.scale = new Vec2(1f);
            furniture2.background.scale = new Vec2(1f);
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, this.y + 44f, new Rectangle(0.0f, 0.0f, 4f, 87f));
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, (float) ((double) this.y + 44.0 + 68.0), new Rectangle(0.0f, 68f, 141f, 19f));
            Graphics.Draw((Sprite) furniture2.sprite, this.x + 70f, this.y + 44f, new Rectangle(0.0f, 0.0f, 141f, 16f));
            Graphics.Draw((Sprite) furniture2.sprite, (float) ((double) this.x + 70.0 + 49.0), this.y + 44f, new Rectangle(49f, 0.0f, 92f, 68f));
            furniture2.sprite.depth = this._selectConsole.depth - 20;
            Graphics.Draw((Sprite) furniture2.sprite, (float) ((double) this.x + 70.0 + 4.0), this.y + 44f, new Rectangle(4f, 0.0f, 44f, 54f));
            furniture2.sprite.depth = (Depth) 0.31f;
            Graphics.Draw((Sprite) furniture2.sprite, (float) ((double) this.x + 70.0 + 4.0), (float) ((double) this.y + 44.0 + 54.0), new Rectangle(4f, 54f, 44f, 14f));
            Graphics.Draw((Sprite) furniture2.background, this.x + 70f, this.y + 45f);
          }
          else
          {
            Graphics.Draw(this._roomLeftBackground, this.x + 4f, this.y + 1f);
            Graphics.Draw(this._roomLeftForeground, this.x, this.y + 1f, new Rectangle(0.0f, 0.0f, 4f, 87f));
            Graphics.Draw(this._roomLeftForeground, this.x + 4f, (float) ((double) this.y + 1.0 + 68.0), new Rectangle(4f, 68f, 137f, 19f));
            Graphics.Draw(this._roomLeftForeground, this.x + 92f, this.y + 1f, new Rectangle(92f, 0.0f, 49f, 16f));
            Graphics.Draw(this._roomLeftForeground, this.x + 135f, (float) ((double) this.y + 1.0 + 16.0), new Rectangle(135f, 16f, 6f, 8f));
            Graphics.Draw(this._roomLeftForeground, this.x + 89f, (float) ((double) this.y + 1.0 + 55.0), new Rectangle(89f, 55f, 52f, 13f));
          }
          if (Network.isActive && (Network.isServer && this.profile.connection == DuckNetwork.localConnection || this.profile.connection == Network.host))
          {
            this._hostCrown.depth = (Depth) -0.5f;
            Graphics.Draw(this._hostCrown, this.x + 14f, this.y + 23f);
          }
        }
        this._tutorialTV.depth = (Depth) -0.58f;
        this._tutorialMessages.depth = (Depth) -0.5f;
        this._tutorialMessages.alpha = this._screenFade;
        this._font.alpha = 1f;
        this._font.depth = (Depth) 0.6f;
        if (furniture1 != null)
        {
          this._tutorialTV.depth = (Depth) -0.8f;
          this._tutorialMessages.depth = (Depth) -0.8f;
        }
        string currentDisplayName = this._playerProfile.team.currentDisplayName;
        this._selectConsole.depth = (Depth) -0.5f;
        this._consoleHighlight.depth = (Depth) -0.49f;
        float num1 = 8f;
        if (this.rightRoom)
        {
          this._consolePos = new Vec2(this.x + 116f, this.y + 30f);
          this._consoleFlash.scale = new Vec2(0.75f, 0.75f);
          if (this._selectConsole.imageIndex == 0)
            this._consoleFlash.alpha = 0.3f;
          else if (this._selectConsole.imageIndex == 1)
            this._consoleFlash.alpha = 0.1f;
          else if (this._selectConsole.imageIndex == 2)
            this._consoleFlash.alpha = 0.0f;
          Graphics.Draw(this._consoleFlash, this._consolePos.x + 9f, this._consolePos.y + 7f);
          if (furniture1 == null)
            Graphics.Draw((Sprite) this._selectConsole, this._consolePos.x, this._consolePos.y);
          if ((double) this._consoleFade > 0.00999999977648258)
          {
            this._consoleHighlight.alpha = this._consoleFade;
            Graphics.Draw(this._consoleHighlight, this._consolePos.x, this._consolePos.y);
          }
          Graphics.Draw(this._readySign, this.x + 1f, this.y + 3f);
          Graphics.Draw(this._tutorialTV, this.x + 57f - num1, this.y + 8f);
          float num2 = -0.57f;
          if (furniture1 != null)
            num2 = -0.8f;
          float num3 = 27f;
          if (this._tutorialMessages.frame == 0)
          {
            this._font.Draw("@DPAD@MOVE", new Vec2(this.x + 28f + num3, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@JUMP@JUMP", new Vec2(this.x + 28f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 1)
          {
            this._font.Draw("@GRAB@", new Vec2(this.x + 45f + num3, this.y + 17f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("PICKUP", new Vec2(this.x + 29f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 2)
          {
            this._font.Draw("@GRAB@TOSS", new Vec2(this.x + 28f + num3, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@FIRE", new Vec2(this.x + 28f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 3)
          {
            this._font.Draw("@QUACK@", new Vec2(this.x + 45f + num3, this.y + 17f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("CANCEL", new Vec2(this.x + 29f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 4)
          {
            this._font.Draw("@DPAD@MOVE", new Vec2(this.x + 28f + num3, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@TEAM", new Vec2(this.x + 28f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 5)
          {
            this._font.Draw("@GRAB@TOSS", new Vec2(this.x + 28f + num3, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@TEAM", new Vec2(this.x + 28f + num3, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          this._font.depth = (Depth) 0.6f;
          float num4 = 0.0f;
          float num5 = 0.0f;
          Vec2 vec2 = new Vec2(1f, 1f);
          if (currentDisplayName.Length > 9)
          {
            vec2 = new Vec2(0.75f, 0.75f);
            num4 = 1f;
            num5 = 1f;
          }
          if (currentDisplayName.Length > 12)
          {
            vec2 = new Vec2(0.5f, 0.5f);
            num4 = 2f;
            num5 = 1f;
          }
          this._font.scale = vec2;
          if (this._hatSelector._roomEditor._mode == REMode.Place)
          {
            float num6 = 0.0f;
            float num7 = 0.0f;
            string text = "PLAYER 1";
            float num8 = 47f;
            this.x += num8;
            Furniture furniture2 = this._hatSelector._roomEditor.CurFurni();
            if (furniture2.type == FurnitureType.Font)
            {
              furniture2.font.scale = new Vec2(0.5f, 0.5f);
              furniture2.font.spriteScale = new Vec2(0.5f, 0.5f);
              furniture2.font.Draw("@SELECT@ACCEPT @QUACK@CANCEL", (float) ((double) this.x + 24.0 - (double) furniture2.font.GetWidth(text) / 2.0) - num6, this.y + 75f + num7, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              furniture2.font.scale = new Vec2(1f, 1f);
            }
            else if (furniture2.type == FurnitureType.Theme)
            {
              this.profile.font.scale = new Vec2(0.5f, 0.5f);
              this.profile.font.spriteScale = new Vec2(0.5f, 0.5f);
              this.profile.font.Draw("@SELECT@ACCEPT @QUACK@CANCEL", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num6, this.y + 75f + num7, Color.White, (Depth) 0.7f, this.profile.inputProfile);
            }
            else
            {
              this.profile.font.scale = new Vec2(0.5f, 0.5f);
              this.profile.font.spriteScale = new Vec2(0.5f, 0.5f);
              if (this._hatSelector._roomEditor._hover != null)
                this.profile.font.Draw("@SELECT@DEL @GRAB@GRAB @QUACK@DONE", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num6, this.y + 75f + num7, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              else
                this.profile.font.Draw("@SELECT@ADD @GRAB@MOD @QUACK@DONE", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num6, this.y + 75f + num7, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              this.profile.font.scale = new Vec2(0.25f, 0.25f);
              int num9 = Profiles.experienceProfile.GetNumFurnitures((int) furniture2.index) - this.profile.GetNumFurnituresPlaced((int) furniture2.index);
              this.profile.font.Draw(furniture2.name + (num9 > 0 ? (object) " |DGGREEN|" : (object) " |DGRED|") + "x" + (object) num9, (float) ((double) this.x + 17.0 - (double) this.profile.font.GetWidth(text) / 2.0 - (double) num6), (float) ((double) this.y + 75.0 + 6.5 + (double) num7), Color.White, (Depth) 0.7f);
              int furnituresPlaced = this.profile.GetTotalFurnituresPlaced();
              int num10 = 16;
              float num11 = (float) furnituresPlaced / (float) num10;
              this.profile.font.Draw(furnituresPlaced.ToString() + "/" + num10.ToString(), (float) ((double) this.x + 68.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num6, (float) ((double) this.y + 75.0 + 6.5) + num7, Color.Black, (Depth) 0.7f);
              Vec2 p1 = new Vec2((float) ((double) this.x + 56.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num6, (float) ((double) this.y + 75.0 + 6.0) + num7);
              Graphics.DrawRect(p1, p1 + new Vec2(37f, 3f), Colors.BlueGray, (Depth) 0.66f, borderWidth: 0.5f);
              Graphics.DrawRect(p1, p1 + new Vec2(37f * num11, 3f), (double) num11 < 0.400000005960464 ? Colors.DGGreen : ((double) num11 < 0.800000011920929 ? Colors.DGYellow : Colors.DGRed), (Depth) 0.68f, borderWidth: 0.5f);
            }
            this.profile.font.spriteScale = new Vec2(1f, 1f);
            this.profile.font.scale = new Vec2(1f, 1f);
            this.x -= num8;
          }
          else
          {
            this._playerProfile.font.scale = vec2;
            this._playerProfile.font.Draw(currentDisplayName, (float) ((double) this.x + 94.0 - (double) this._playerProfile.font.GetWidth(currentDisplayName) / 2.0) - num5, this.y + 75f + num4, Color.White, (Depth) 0.7f);
            this._font.scale = new Vec2(1f, 1f);
          }
        }
        else
        {
          this._consolePos = new Vec2(this.x + 4f, this.y + 30f);
          this._consoleFlash.scale = new Vec2(0.75f, 0.75f);
          if (this._selectConsole.imageIndex == 0)
            this._consoleFlash.alpha = 0.3f;
          else if (this._selectConsole.imageIndex == 1)
            this._consoleFlash.alpha = 0.1f;
          else if (this._selectConsole.imageIndex == 2)
            this._consoleFlash.alpha = 0.0f;
          Graphics.Draw(this._consoleFlash, this._consolePos.x + 9f, this._consolePos.y + 7f);
          if (furniture1 != null)
            this._selectConsole.depth = (Depth) -0.8f;
          Graphics.Draw((Sprite) this._selectConsole, this._consolePos.x, this._consolePos.y);
          this._selectConsole.depth = (Depth) -0.5f;
          if ((double) this._consoleFade > 0.00999999977648258)
          {
            this._consoleHighlight.alpha = this._consoleFade;
            Graphics.Draw(this._consoleHighlight, this._consolePos.x, this._consolePos.y);
          }
          Graphics.Draw(this._readySign, this.x + 96f, this.y + 3f);
          Graphics.Draw(this._tutorialTV, this.x + 22f + num1, this.y + 8f);
          float num2 = -0.57f;
          if (furniture1 != null)
            num2 = -0.8f;
          if (this._tutorialMessages.frame == 0)
          {
            this._font.Draw("@DPAD@MOVE", new Vec2(this.x + 28f + num1, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@JUMP@JUMP", new Vec2(this.x + 28f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 1)
          {
            this._font.Draw("@GRAB@", new Vec2(this.x + 45f + num1, this.y + 17f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("PICKUP", new Vec2(this.x + 29f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 2)
          {
            this._font.Draw("@GRAB@TOSS", new Vec2(this.x + 28f + num1, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@FIRE", new Vec2(this.x + 28f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 3)
          {
            this._font.Draw("@QUACK@", new Vec2(this.x + 45f + num1, this.y + 17f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("CANCEL", new Vec2(this.x + 29f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 4)
          {
            this._font.Draw("@DPAD@MOVE", new Vec2(this.x + 28f + num1, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@TEAM", new Vec2(this.x + 28f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          else if (this._tutorialMessages.frame == 5)
          {
            this._font.Draw("@GRAB@TOSS", new Vec2(this.x + 28f + num1, this.y + 16f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
            this._font.Draw("@SHOOT@TEAM", new Vec2(this.x + 28f + num1, this.y + 30f), Color.White * this._screenFade, (Depth) num2, this._inputProfile);
          }
          this._font.depth = (Depth) 0.6f;
          this._aButton.position = new Vec2(this.x + 39f, this.y + 71f);
          float num3 = 0.0f;
          float num4 = 0.0f;
          Vec2 vec2 = new Vec2(1f, 1f);
          if (currentDisplayName.Length > 9)
          {
            vec2 = new Vec2(0.75f, 0.75f);
            num3 = 1f;
            num4 = 1f;
          }
          if (currentDisplayName.Length > 12)
          {
            vec2 = new Vec2(0.5f, 0.5f);
            num3 = 2f;
            num4 = 1f;
          }
          if (this._hatSelector._roomEditor._mode == REMode.Place && Profiles.experienceProfile != null)
          {
            string text = "PLAYER 1";
            float num5 = 0.0f;
            float num6 = 0.0f;
            Furniture furniture2 = this._hatSelector._roomEditor.CurFurni();
            if (furniture2.type == FurnitureType.Font)
            {
              furniture2.font.scale = new Vec2(0.5f, 0.5f);
              furniture2.font.spriteScale = new Vec2(0.5f, 0.5f);
              furniture2.font.Draw("@SELECT@ACCEPT @QUACK@CANCEL", (float) ((double) this.x + 24.0 - (double) furniture2.font.GetWidth(text) / 2.0) - num5, this.y + 75f + num6, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              furniture2.font.scale = new Vec2(1f, 1f);
            }
            else if (furniture2.type == FurnitureType.Theme)
            {
              this.profile.font.scale = new Vec2(0.5f, 0.5f);
              this.profile.font.spriteScale = new Vec2(0.5f, 0.5f);
              this.profile.font.Draw("@SELECT@ACCEPT @QUACK@CANCEL", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num5, this.y + 75f + num6, Color.White, (Depth) 0.7f, this.profile.inputProfile);
            }
            else
            {
              this.profile.font.scale = new Vec2(0.5f, 0.5f);
              this.profile.font.spriteScale = new Vec2(0.5f, 0.5f);
              if (this._hatSelector._roomEditor._hover != null)
                this.profile.font.Draw("@SELECT@DEL @GRAB@GRAB @QUACK@DONE", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num5, this.y + 75f + num6, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              else
                this.profile.font.Draw("@SELECT@ADD @GRAB@MOD @QUACK@DONE", (float) ((double) this.x + 24.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num5, this.y + 75f + num6, Color.White, (Depth) 0.7f, this.profile.inputProfile);
              this.profile.font.scale = new Vec2(0.25f, 0.25f);
              int num7 = Profiles.experienceProfile.GetNumFurnitures((int) furniture2.index) - this.profile.GetNumFurnituresPlaced((int) furniture2.index);
              this.profile.font.Draw(furniture2.name + (num7 > 0 ? (object) " |DGGREEN|" : (object) " |DGRED|") + "x" + (object) num7, (float) ((double) this.x + 17.0 - (double) this.profile.font.GetWidth(text) / 2.0 - (double) num5), (float) ((double) this.y + 75.0 + 6.5 + (double) num6), Color.White, (Depth) 0.7f);
              int furnituresPlaced = this.profile.GetTotalFurnituresPlaced();
              int num8 = 16;
              float num9 = (float) furnituresPlaced / (float) num8;
              this.profile.font.Draw(furnituresPlaced.ToString() + "/" + num8.ToString(), (float) ((double) this.x + 68.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num5, (float) ((double) this.y + 75.0 + 6.5) + num6, Color.Black, (Depth) 0.7f);
              Vec2 p1 = new Vec2((float) ((double) this.x + 56.0 - (double) this.profile.font.GetWidth(text) / 2.0) - num5, (float) ((double) this.y + 75.0 + 6.0) + num6);
              Graphics.DrawRect(p1, p1 + new Vec2(37f, 3f), Colors.BlueGray, (Depth) 0.66f, borderWidth: 0.5f);
              Graphics.DrawRect(p1, p1 + new Vec2(37f * num9, 3f), (double) num9 < 0.400000005960464 ? Colors.DGGreen : ((double) num9 < 0.800000011920929 ? Colors.DGYellow : Colors.DGRed), (Depth) 0.68f, borderWidth: 0.5f);
            }
            this.profile.font.spriteScale = new Vec2(1f, 1f);
            this.profile.font.scale = new Vec2(1f, 1f);
          }
          else
          {
            this._playerProfile.font.scale = vec2;
            this._playerProfile.font.Draw(currentDisplayName, (float) ((double) this.x + 48.0 - (double) this._playerProfile.font.GetWidth(currentDisplayName) / 2.0) - num4, this.y + 75f + num3, Color.White, (Depth) 0.7f);
            this._font.scale = new Vec2(1f, 1f);
          }
        }
      }
    }
  }
}
