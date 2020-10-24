// Decompiled with JetBrains decompiler
// Type: DuckGame.RockScoreboard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class RockScoreboard : Level
  {
    private static Level _returnLevel;
    private bool _afterHighlights;
    private ScoreBoardMode _mode;
    private ScoreBoardState _state = ScoreBoardState.Intro;
    private float _throwWait = 1f;
    private float _afterThrowWait = 1f;
    private Sprite _bleachers;
    private Sprite _bleacherSeats;
    private Sprite _intermissionText;
    private Sprite _winnerPost;
    private Sprite _winnerBanner;
    private BitmapFont _font;
    private FieldBackground _field;
    private WallLayer _wall;
    private FieldBackground _fieldForeground;
    private FieldBackground _fieldForeground2;
    private ContinueCountdown _netCountdown;
    private GinormoBoard _scoreBoard;
    private bool _shiftCamera;
    private bool _finished;
    private bool _getScreenshot;
    private Sprite _finalSprite;
    private float _intermissionSlide = 1f;
    private float _controlSlide;
    private int _controlMessage = -1;
    private CornerDisplay _continueHUD;
    private float _desiredScroll;
    private float _animWait = 1f;
    private float _backWait = 1f;
    private float _showScoreWait = 1f;
    private float _fieldWidth = 680f;
    private bool _skipFade;
    private float _winnerWait = 1f;
    private bool _matchOver;
    private bool _tie;
    private bool _viewBoard;
    private bool _quit;
    private bool _misfire;
    private float _cameraWait = 1f;
    private bool _takePicture;
    private int _playedBeeps;
    private bool _playedFlash;
    private float _cameraFadeVel;
    private int _flashSkipFrames;
    private float _fieldScroll;
    private SinWave _sin = (SinWave) 0.01f;
    private Crowd _crowd;
    public List<Slot3D> _slots = new List<Slot3D>();
    private Slot3D _highestSlot;
    private Team _winningTeam;
    public static RenderTarget2D finalImage;
    public static RenderTarget2D finalImage2;
    private float _backgroundFade;
    private Layer _sunLayer;
    private RenderTarget2D _sunshineTarget;
    private RenderTarget2D _screenTarget;
    private RenderTarget2D _pixelTarget;
    public static bool _sunEnabled = true;
    private RockWeather _weather;
    private List<InputObject> _inputs = new List<InputObject>();
    public static bool initializingDucks = false;
    public static bool wallMode = false;
    private Thing sunThing;
    private Thing rainbowThing;
    private Thing rainbowThing2;
    private static bool _drawingSunTarget = false;
    private static bool _drawingLighting = false;
    private static bool _drawingNormalTarget = false;
    private bool _hatSelect;
    private bool _focusRock;
    private bool _droppedConfetti;
    private float _confettiDrop;
    private Material _sunshineMaterial;
    private Material _sunshineMaterialBare;

    public static Level returnLevel => RockScoreboard._returnLevel;

    public bool afterHighlights
    {
      get => this._afterHighlights;
      set => this._afterHighlights = value;
    }

    public RockScoreboard(Level r = null, ScoreBoardMode mode = ScoreBoardMode.ShowScores, bool afterHighlights = false)
    {
      this._afterHighlights = afterHighlights;
      if (Network.isServer)
        RockScoreboard._returnLevel = r;
      this._mode = mode;
      if (mode != ScoreBoardMode.ShowWinner)
        return;
      this._state = ScoreBoardState.None;
    }

    public ScoreBoardMode mode => this._mode;

    public Vec3 fieldAddColor
    {
      set
      {
        if (this._field == null)
          return;
        this._field.colorAdd = value;
        this._fieldForeground.colorAdd = value;
        this._fieldForeground2.colorAdd = value;
        this._wall.colorAdd = value;
      }
    }

    public Vec3 fieldMulColor
    {
      set
      {
        if (this._field == null)
          return;
        this._field.colorMul = value;
        this._fieldForeground.colorMul = value;
        this._fieldForeground2.colorMul = value;
        this._wall.colorMul = value;
      }
    }

    public float cameraY
    {
      get => this.camera.y;
      set
      {
        this.camera.y = value;
        this._field.ypos = this.camera.y * 1.4f;
      }
    }

    public int controlMessage
    {
      get => this._controlMessage;
      set
      {
        if (this._controlMessage != value)
        {
          HUD.CloseAllCorners();
          if (value == 0)
            HUD.AddCornerControl(HUDCorner.BottomRight, "@START@SKIP");
          else if (value > 0)
          {
            if (!Network.isServer)
            {
              this._continueHUD = HUD.AddCornerMessage(HUDCorner.BottomRight, "WAITING");
            }
            else
            {
              this._continueHUD = HUD.AddCornerControl(HUDCorner.BottomRight, "@START@CONTINUE");
              if (value > 1)
              {
                HUD.AddCornerControl(HUDCorner.BottomLeft, "QUIT@QUACK@");
                HUD.AddCornerControl(HUDCorner.TopRight, "@GRAB@CHANGE HAT");
              }
            }
          }
        }
        this._controlMessage = value;
      }
    }

    public void SetWeather(Weather w)
    {
      if (this._weather == null)
        return;
      this._weather.SetWeather(w);
    }

    public override void SendLevelData(NetworkConnection c)
    {
      if (!Network.isServer)
        return;
      Send.Message((NetMessage) new NMCrowdData(this._crowd.NetSerialize()));
      Send.Message((NetMessage) new NMWeatherData(this._weather.NetSerialize()));
    }

    public override void OnMessage(NetMessage message)
    {
      if (message is NMCrowdData && Network.isClient)
        this._crowd.NetDeserialize((message as NMCrowdData).data);
      if (!(message is NMWeatherData) || !Network.isClient)
        return;
      this._weather.NetDeserialize((message as NMWeatherData).data);
    }

    public InputProfile GetNetInput(sbyte index) => (int) index >= this._inputs.Count || this._inputs[(int) index].duckProfile == null || this._inputs[(int) index].duckProfile.inputProfile == null ? new InputProfile() : this._inputs[(int) index].duckProfile.inputProfile;

    public override void Initialize()
    {
      if (Network.isActive && Network.isServer && this._mode == ScoreBoardMode.ShowScores)
      {
        int num = 0;
        foreach (Profile profile in DuckNetwork.profiles)
        {
          if (profile.connection != null)
          {
            InputObject inputObject = new InputObject();
            inputObject.profileNumber = (sbyte) num;
            Level.Add((Thing) inputObject);
            this._inputs.Add(inputObject);
            ++num;
          }
        }
      }
      HighlightLevel.didSkip = false;
      if (this._afterHighlights)
        this._skipFade = true;
      this._weather = new RockWeather(this);
      this._weather.Start();
      Level.Add((Thing) this._weather);
      for (int index = 0; index < 350; ++index)
        this._weather.Update();
      if (RockScoreboard._sunEnabled)
      {
        float num = 9f / 16f;
        this._sunshineTarget = new RenderTarget2D(DuckGame.Graphics.width / 12, (int) ((double) DuckGame.Graphics.width * (double) num) / 12);
        this._screenTarget = new RenderTarget2D(DuckGame.Graphics.width, (int) ((double) DuckGame.Graphics.width * (double) num));
        this._pixelTarget = new RenderTarget2D(160, (int) (320.0 * (double) num / 2.0));
        this._sunLayer = new Layer("SUN LAYER", 99999);
        Layer.Add(this._sunLayer);
        Thing thing = (Thing) new SpriteThing(150f, 120f, new Sprite("sun"));
        thing.z = -9999f;
        thing.depth = (Depth) -0.99f;
        thing.layer = this._sunLayer;
        thing.xscale = 1f;
        thing.yscale = 1f;
        thing.collisionSize = new Vec2(1f, 1f);
        thing.collisionOffset = new Vec2(0.0f, 0.0f);
        Level.Add(thing);
        this.sunThing = thing;
        SpriteThing spriteThing1 = new SpriteThing(150f, 80f, new Sprite("rainbow"));
        spriteThing1.alpha = 0.15f;
        spriteThing1.z = -9999f;
        spriteThing1.depth = (Depth) -0.99f;
        spriteThing1.layer = this._sunLayer;
        spriteThing1.xscale = 1f;
        spriteThing1.yscale = 1f;
        spriteThing1.color = new Color(100, 100, 100);
        spriteThing1.collisionSize = new Vec2(1f, 1f);
        spriteThing1.collisionOffset = new Vec2(0.0f, 0.0f);
        Level.Add((Thing) spriteThing1);
        this.rainbowThing = (Thing) spriteThing1;
        this.rainbowThing.visible = false;
        SpriteThing spriteThing2 = new SpriteThing(150f, 80f, new Sprite("rainbow"));
        spriteThing2.z = -9999f;
        spriteThing2.depth = (Depth) -0.99f;
        spriteThing2.layer = this._sunLayer;
        spriteThing2.xscale = 1f;
        spriteThing2.yscale = 1f;
        spriteThing2.color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 90);
        spriteThing2.collisionSize = new Vec2(1f, 1f);
        spriteThing2.collisionOffset = new Vec2(0.0f, 0.0f);
        Level.Add((Thing) spriteThing2);
        this.rainbowThing2 = (Thing) spriteThing2;
        this.rainbowThing2.visible = false;
      }
      List<Team> allRandomized = Teams.allRandomized;
      if (RockScoreboard.returnLevel == null && !Network.isActive)
      {
        allRandomized[0].Join(Profiles.DefaultPlayer1);
        allRandomized[1].Join(Profiles.DefaultPlayer2);
        allRandomized[0].score = 10;
        allRandomized[1].score = 2;
        Teams.Player3.score = 3;
        Teams.Player4.score = 4;
      }
      this._crowd = new Crowd();
      Level.Add((Thing) this._crowd);
      Crowd.mood = Mood.Calm;
      this._field = new FieldBackground("FIELD", 9999);
      Layer.Add((Layer) this._field);
      this._bleacherSeats = new Sprite("bleacherSeats");
      this._bleachers = RockWeather.weather != Weather.Snowing ? new Sprite("bleacherBack") : new Sprite("bleacherBackSnow");
      this._bleachers.center = new Vec2((float) (this._bleachers.w / 2), (float) (this._bleachers.height - 3));
      this._intermissionText = new Sprite("rockThrow/intermission");
      this._winnerPost = new Sprite("rockThrow/winnerPost");
      this._winnerBanner = new Sprite("rockThrow/winnerBanner");
      this._font = new BitmapFont("biosFont", 8);
      List<Team> teamList1 = new List<Team>();
      foreach (Team team in Teams.all)
      {
        if (team.activeProfiles.Count > 0)
          teamList1.Add(team);
      }
      foreach (Team team in teamList1)
      {
        team.rockScore = team.score;
        if (RockScoreboard.wallMode && this._mode == ScoreBoardMode.ShowScores)
          team.score = Math.Min(team.score, GameMode.winsPerSet);
      }
      if (this._mode == ScoreBoardMode.ShowScores)
      {
        this._intermissionSlide = 1f;
        DuckGame.Graphics.fade = 1f;
        Layer.Game.fade = 0.0f;
        Layer.Background.fade = 0.0f;
        Crowd.UpdateFans();
        int num1 = 0;
        Stack<Depth> depthStack = new Stack<Depth>();
        for (int index = 0; index < 4; ++index)
          depthStack.Push(new Depth((float) index * 0.02f));
        int num2 = 0;
        foreach (Team team in teamList1)
        {
          Depth depth = depthStack.Pop();
          float num3 = 223f;
          float ypos = 0.0f;
          float num4 = 26f;
          switch (num1)
          {
            case 1:
              num4 = 24f;
              break;
            case 2:
              num4 = 27f;
              break;
            case 3:
              num4 = 32f;
              break;
          }
          float num5 = (float) (158.0 - (double) num1 * (double) num4);
          int prevScoreboardScore = team.prevScoreboardScore;
          int num6 = GameMode.winsPerSet * 2;
          int num7 = team.score;
          if (RockScoreboard.wallMode && num7 > GameMode.winsPerSet)
            num7 = GameMode.winsPerSet;
          this._slots.Add(new Slot3D());
          if (num7 >= GameMode.winsPerSet && num7 == num2)
            this._tie = true;
          else if (num7 >= GameMode.winsPerSet && num7 > num2)
          {
            this._tie = false;
            num2 = num7;
            this._highestSlot = this._slots[this._slots.Count - 1];
          }
          List<Profile> profileList = new List<Profile>();
          Profile profile1 = (Profile) null;
          bool flag = false;
          foreach (Profile activeProfile in team.activeProfiles)
          {
            if (flag)
            {
              profile1 = activeProfile;
              flag = false;
            }
            if (activeProfile.wasRockThrower)
            {
              activeProfile.wasRockThrower = false;
              flag = true;
            }
            profileList.Add(activeProfile);
          }
          if (profile1 == null)
            profile1 = team.activeProfiles[0];
          profileList.Remove(profile1);
          profileList.Insert(0, profile1);
          profile1.wasRockThrower = true;
          byte num8 = (byte) (this._slots.Count - 1);
          int num9 = 0;
          foreach (Profile profile2 in profileList)
          {
            if (profile2 == profile1)
            {
              RockScoreboard.initializingDucks = true;
              this._slots[(int) num8].duck = (Duck) new RockThrowDuck(num3 - (float) (num9 * 10), ypos - 16f, profile2);
              this._slots[(int) num8].duck.planeOfExistence = num8;
              this._slots[(int) num8].duck.ignoreGhosting = true;
              this._slots[(int) num8].duck.forceMindControl = true;
              Level.Add((Thing) this._slots[(int) num8].duck);
              this._slots[(int) num8].duck.connection = DuckNetwork.localConnection;
              RockScoreboard.initializingDucks = false;
              if (this._slots[this._slots.Count - 1].duck.GetEquipment(typeof (TeamHat)) is TeamHat equipment)
                equipment.ignoreGhosting = true;
              this._slots[this._slots.Count - 1].duck.z = num5;
              this._slots[this._slots.Count - 1].duck.depth = depth;
              this._slots[this._slots.Count - 1].ai = new DuckAI(profile2.inputProfile);
              if (Network.isActive && profile2.connection != DuckNetwork.localConnection)
                this._slots[this._slots.Count - 1].ai._manualQuack = this.GetNetInput((sbyte) profile2.networkIndex);
              this._slots[this._slots.Count - 1].duck.derpMindControl = false;
              this._slots[this._slots.Count - 1].duck.mindControl = (InputProfile) this._slots[this._slots.Count - 1].ai;
              this._slots[this._slots.Count - 1].rock = new ScoreRock((float) ((double) num3 + 18.0 + (double) prevScoreboardScore / (double) num6 * (double) this._fieldWidth), ypos, profile2);
              this._slots[this._slots.Count - 1].rock.planeOfExistence = num8;
              this._slots[this._slots.Count - 1].rock.ignoreGhosting = true;
              Level.Add((Thing) this._slots[this._slots.Count - 1].rock);
              this._slots[this._slots.Count - 1].rock.z = num5;
              this._slots[this._slots.Count - 1].rock.depth = this._slots[this._slots.Count - 1].duck.depth + 1;
              this._slots[this._slots.Count - 1].rock.grounded = true;
              this._slots[this._slots.Count - 1].duck.isRockThrowDuck = true;
            }
            else
            {
              RockScoreboard.initializingDucks = true;
              Duck duck = (Duck) new RockThrowDuck(num3 - (float) (num9 * 12), ypos - 16f, profile2);
              duck.forceMindControl = true;
              duck.planeOfExistence = num8;
              duck.ignoreGhosting = true;
              Level.Add((Thing) duck);
              RockScoreboard.initializingDucks = false;
              duck.depth = depth;
              duck.z = num5;
              duck.derpMindControl = false;
              DuckAI duckAi = new DuckAI(profile2.inputProfile);
              if (Network.isActive && profile2.connection != DuckNetwork.localConnection)
                duckAi._manualQuack = this.GetNetInput((sbyte) profile2.networkIndex);
              duck.mindControl = (InputProfile) duckAi;
              duck.isRockThrowDuck = true;
              duck.connection = DuckNetwork.localConnection;
              this._slots[this._slots.Count - 1].subDucks.Add(duck);
              this._slots[this._slots.Count - 1].subAIs.Add(duckAi);
            }
            ++num9;
          }
          this._slots[this._slots.Count - 1].slotIndex = num1;
          this._slots[this._slots.Count - 1].startX = num3;
          ++num1;
        }
        for (int index = 0; index < 4; ++index)
        {
          Block block = new Block(-50f, 0.0f, 1200f, 32f);
          block.planeOfExistence = (byte) index;
          Level.Add((Thing) block);
        }
        if (!this._tie && num2 > 0)
          this._matchOver = true;
        if (this._tie)
          GameMode.showdown = true;
      }
      else
      {
        PurpleBlock.Reset();
        if (Teams.active.Count > 1 && !this._afterHighlights)
        {
          Global.data.matchesPlayed += 1;
          Global.WinMatch(Teams.winning[0]);
          if (Network.isActive)
          {
            foreach (Profile activeProfile in Teams.winning[0].activeProfiles)
            {
              if (activeProfile.connection == DuckNetwork.localConnection)
              {
                DuckNetwork.GiveXP("Won Match", 0, 100);
                break;
              }
            }
          }
          if (GameMode.winsPerSet > (int) Global.data.longestMatchPlayed)
            Global.data.longestMatchPlayed.valueInt = GameMode.winsPerSet;
        }
        this._intermissionSlide = 0.0f;
        teamList1.Sort((Comparison<Team>) ((a, b) =>
        {
          if (a.score == b.score)
            return 0;
          return a.score >= b.score ? -1 : 1;
        }));
        float num1 = (float) (160.0 - (double) (teamList1.Count * 42 / 2) + 21.0);
        foreach (Team team in Teams.all)
          team.prevScoreboardScore = 0;
        List<List<Team>> source = new List<List<Team>>();
        foreach (Team team in teamList1)
        {
          int score = team.score;
          bool flag = false;
          for (int index = 0; index < source.Count; ++index)
          {
            if (source[index][0].score < score)
            {
              source.Insert(index, new List<Team>());
              source[index].Add(team);
              flag = true;
              break;
            }
            if (source[index][0].score == score)
            {
              source[index].Add(team);
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            source.Add(new List<Team>());
            source.Last<List<Team>>().Add(team);
          }
        }
        this._winningTeam = teamList1[0];
        this.controlMessage = 1;
        this._state = ScoreBoardState.None;
        Crowd.mood = Mood.Dead;
        bool flag1 = false;
        if (!this._afterHighlights)
        {
          int place = 0;
          int num2 = 0;
          foreach (List<Team> teamList2 in source)
          {
            foreach (Team team in teamList2)
            {
              Level.Add((Thing) new Pedestal(num1 + (float) (num2 * 42), 150f, team, place));
              ++num2;
            }
            ++place;
          }
          if (this._winningTeam.activeProfiles.Count > 1)
            ++this._winningTeam.wins;
          else
            ++this._winningTeam.activeProfiles[0].wins;
          foreach (Profile activeProfile in this._winningTeam.activeProfiles)
          {
            ++activeProfile.stats.trophiesWon;
            activeProfile.stats.trophiesSinceLastWin = activeProfile.stats.trophiesSinceLastWinCounter;
            activeProfile.stats.trophiesSinceLastWinCounter = 0;
            if (Network.isActive && activeProfile.connection == DuckNetwork.localConnection && !flag1)
            {
              flag1 = true;
              ++Global.data.onlineWins.valueInt;
              if (activeProfile.team.name == "SWACK")
                ++Global.data.winsAsSwack.valueInt;
            }
            if (!Network.isActive && activeProfile.team.name == "SWACK")
              ++Global.data.winsAsSwack.valueInt;
          }
          MonoMain.LogPlay();
          foreach (Team team in teamList1)
          {
            foreach (Profile activeProfile in team.activeProfiles)
            {
              ++activeProfile.stats.trophiesSinceLastWinCounter;
              ++activeProfile.stats.gamesPlayed;
            }
          }
          Main.lastLevel = "";
        }
      }
      this._bottomRight = new Vec2(1000f, 1000f);
      this.lowestPoint = 1000f;
      this._scoreBoard = new GinormoBoard(300f, -320f, this._mode == ScoreBoardMode.ShowScores ? BoardMode.Points : BoardMode.Wins);
      this._scoreBoard.z = -130f;
      Level.Add((Thing) this._scoreBoard);
      this.backgroundColor = new Color(0, 0, 0);
      Music.volume = Options.Data.musicVolume;
      if (this._mode != ScoreBoardMode.ShowWinner && !this._afterHighlights)
        Music.Play("SportsTime");
      this.cameraY = 0.0f;
      Sprite s1;
      switch (RockWeather.weather)
      {
        case Weather.Snowing:
          s1 = new Sprite("fieldNoiseSnow");
          break;
        case Weather.Raining:
          s1 = new Sprite("fieldNoiseRain");
          break;
        default:
          s1 = new Sprite("fieldNoise");
          break;
      }
      s1.scale = new Vec2(4f, 4f);
      s1.depth = (Depth) 0.5f;
      s1.y -= 16f;
      this._field.AddSprite(s1);
      Sprite s2 = new Sprite("fieldWall");
      s2.scale = new Vec2(4f, 4f);
      s2.depth = (Depth) 0.5f;
      s2.y -= 16f;
      this._wall = new WallLayer("FIELDWALL", 80);
      if (RockScoreboard.wallMode)
        this._wall.AddWallSprite(s2);
      Layer.Add((Layer) this._wall);
      this._fieldForeground = new FieldBackground("FIELDFOREGROUND", 80);
      this._fieldForeground.fieldHeight = -13f;
      Layer.Add((Layer) this._fieldForeground);
      this._fieldForeground2 = new FieldBackground("FIELDFOREGROUND2", 70);
      this._fieldForeground2.fieldHeight = -15f;
      Layer.Add((Layer) this._fieldForeground2);
      if (this._mode != ScoreBoardMode.ShowWinner)
      {
        Sprite s3 = new Sprite("rockThrow/chairSeat");
        s3.CenterOrigin();
        s3.x = 300f;
        s3.y = 20f;
        s3.scale = new Vec2(1.2f, 1.2f);
        this._fieldForeground.AddSprite(s3);
        Sprite s4 = new Sprite("rockThrow/tableTop");
        s4.CenterOrigin();
        s4.x = 450f;
        s4.y = 14f;
        s4.scale = new Vec2(1.2f, 1.4f);
        this._fieldForeground2.AddSprite(s4);
        int num = -95;
        Sprite spr1 = new Sprite("rockThrow/chairBottomBack");
        Thing thing1 = (Thing) new SpriteThing(300f, -10f, spr1);
        thing1.center = new Vec2((float) (spr1.w / 2), (float) (spr1.h / 2));
        thing1.z = (float) (106 + num);
        thing1.depth = (Depth) 0.5f;
        thing1.layer = Layer.Background;
        Level.Add(thing1);
        Sprite spr2 = new Sprite("rockThrow/chairBottom");
        Thing thing2 = (Thing) new SpriteThing(300f, -6f, spr2);
        thing2.center = new Vec2((float) (spr2.w / 2), (float) (spr2.h / 2));
        thing2.z = (float) (120 + num);
        thing2.depth = (Depth) 0.8f;
        thing2.layer = Layer.Background;
        Level.Add(thing2);
        Sprite spr3 = new Sprite("rockThrow/chairFront");
        Thing thing3 = (Thing) new SpriteThing(300f, -9f, spr3);
        thing3.center = new Vec2((float) (spr3.w / 2), (float) (spr3.h / 2));
        thing3.z = (float) (122 + num);
        thing3.depth = (Depth) 0.9f;
        thing3.layer = Layer.Background;
        Level.Add(thing3);
        Sprite spr4 = new Sprite("rockThrow/tableBottomBack");
        Thing thing4 = (Thing) new SpriteThing(450f, -7f, spr4);
        thing4.center = new Vec2((float) (spr4.w / 2), (float) (spr4.h / 2));
        thing4.z = (float) (106 + num);
        thing4.depth = (Depth) 0.5f;
        thing4.layer = Layer.Background;
        Level.Add(thing4);
        Sprite spr5 = new Sprite("rockThrow/tableBottom");
        Thing thing5 = (Thing) new SpriteThing(450f, -7f, spr5);
        thing5.center = new Vec2((float) (spr5.w / 2), (float) (spr5.h / 2));
        thing5.z = (float) (120 + num);
        thing5.depth = (Depth) 0.8f;
        thing5.layer = Layer.Background;
        Level.Add(thing5);
        Sprite spr6 = new Sprite("rockThrow/keg");
        Thing thing6 = (Thing) new SpriteThing(460f, -24f, spr6);
        thing6.center = new Vec2((float) (spr6.w / 2), (float) (spr6.h / 2));
        thing6.z = (float) (120 + num - 4);
        thing6.depth = (Depth) -0.4f;
        thing6.layer = Layer.Game;
        Level.Add(thing6);
        Sprite spr7 = new Sprite("rockThrow/cup");
        Thing thing7 = (Thing) new SpriteThing(445f, -21f, spr7);
        thing7.center = new Vec2((float) (spr7.w / 2), (float) (spr7.h / 2));
        thing7.z = (float) (120 + num - 6);
        thing7.depth = (Depth) -0.5f;
        thing7.layer = Layer.Game;
        Level.Add(thing7);
        Sprite spr8 = new Sprite("rockThrow/cup");
        Thing thing8 = (Thing) new SpriteThing(437f, -20f, spr8);
        thing8.center = new Vec2((float) (spr8.w / 2), (float) (spr8.h / 2));
        thing8.z = (float) (120 + num);
        thing8.depth = (Depth) -0.3f;
        thing8.layer = Layer.Game;
        Level.Add(thing8);
        Sprite spr9 = new Sprite("rockThrow/cup");
        Thing thing9 = (Thing) new SpriteThing(472f, -20f, spr9);
        thing9.center = new Vec2((float) (spr9.w / 2), (float) (spr9.h / 2));
        thing9.z = (float) (120 + num - 7);
        thing9.depth = (Depth) -0.5f;
        thing9.layer = Layer.Game;
        thing9.angleDegrees = 80f;
        Level.Add(thing9);
      }
      for (int index = 0; index < 3; ++index)
      {
        DistanceMarker distanceMarker = new DistanceMarker((float) (230 + index * 175), -25f, (int) Math.Round((double) (index * GameMode.winsPerSet) / 2.0));
        distanceMarker.z = 0.0f;
        distanceMarker.depth = (Depth) 0.34f;
        distanceMarker.layer = Layer.Background;
        Level.Add((Thing) distanceMarker);
      }
      Sprite spr = RockWeather.weather != Weather.Snowing ? new Sprite("bleacherBack") : new Sprite("bleacherBackSnow");
      for (int index = 0; index < 24; ++index)
      {
        SpriteThing spriteThing = new SpriteThing((float) (100 + index * (spr.w + 13)), (float) (spr.h + 15), spr);
        spriteThing.center = new Vec2((float) (spr.w / 2), (float) (spr.h - 1));
        spriteThing.collisionOffset = new Vec2(spriteThing.collisionOffset.x, (float) -spr.h);
        spriteThing.z = 0.0f;
        spriteThing.depth = (Depth) 0.33f;
        spriteThing.layer = Layer.Background;
        Level.Add((Thing) spriteThing);
      }
      SpriteThing spriteThing3 = new SpriteThing(600f, 0.0f, new Sprite("blackSquare"));
      spriteThing3.z = -90f;
      spriteThing3.centery = 7f;
      spriteThing3.depth = (Depth) 0.1f;
      spriteThing3.layer = Layer.Background;
      spriteThing3.xscale = 100f;
      spriteThing3.yscale = 7f;
      Level.Add((Thing) spriteThing3);
      this._weather.Update();
    }

    public Vec2 sunPos => this.sunThing.position;

    public Layer sunLayer => this._sunLayer;

    public static bool drawingSunTarget => RockScoreboard._drawingSunTarget;

    public static bool drawingLighting => RockScoreboard._drawingLighting;

    public static bool drawingNormalTarget => RockScoreboard._drawingNormalTarget;

    public void DoRender()
    {
      Color backgroundColor = this.backgroundColor;
      if (NetworkDebugger.enabled)
      {
        RockScoreboard._drawingSunTarget = true;
        Layer.Game.camera.width = 320f;
        Layer.Game.camera.height = 180f;
        this._field.fade = Layer.Game.fade;
        this._fieldForeground.fade = Layer.Game.fade;
        this._wall.fade = Layer.Game.fade;
        this._fieldForeground2.fade = Layer.Game.fade;
        this.backgroundColor = backgroundColor;
        MonoMain.RenderGame(this._screenTarget);
        RockScoreboard._drawingSunTarget = false;
      }
      else
      {
        this.backgroundColor = Color.Black;
        RockScoreboard._drawingSunTarget = true;
        float fade1 = Layer.HUD.fade;
        float fade2 = Layer.Console.fade;
        float fade3 = Layer.Game.fade;
        float fade4 = Layer.Background.fade;
        float fade5 = this._field.fade;
        Layer.Game.fade = 0.0f;
        Layer.Background.fade = 0.0f;
        Layer.Foreground.fade = 0.0f;
        this._field.fade = 0.0f;
        this._fieldForeground.fade = 0.0f;
        this._wall.fade = 0.0f;
        this._fieldForeground2.fade = 0.0f;
        Vec3 colorMul1 = Layer.Game.colorMul;
        Vec3 colorMul2 = Layer.Background.colorMul;
        Layer.Game.colorMul = Vec3.One;
        Layer.Background.colorMul = Vec3.One;
        Layer.HUD.fade = 0.0f;
        Layer.Console.fade = 0.0f;
        this.fieldMulColor = Vec3.One;
        Vec3 colorAdd = Layer.Game.colorAdd;
        Layer.Game.colorAdd = Vec3.Zero;
        Layer.Background.colorAdd = Vec3.Zero;
        this.fieldAddColor = Vec3.Zero;
        Layer.blurry = true;
        this.sunThing.alpha = RockWeather.sunOpacity;
        this.rainbowThing2.alpha = 0.0f;
        RockScoreboard._drawingLighting = true;
        MonoMain.RenderGame(this._sunshineTarget);
        RockScoreboard._drawingLighting = false;
        if (this._sunshineMaterialBare == null)
          this._sunshineMaterialBare = (Material) new MaterialSunshineBare();
        Vec2 sunPos = this.sunPos;
        Vec3 vec3_1 = new Vec3(sunPos.x, -9999f, sunPos.y);
        Viewport viewport1 = new Viewport(0, 0, (int) Layer.HUD.width, (int) Layer.HUD.height);
        Vec3 vec3_2 = (Vec3) viewport1.Project((Vector3) vec3_1, (Microsoft.Xna.Framework.Matrix) this.sunLayer.projection, (Microsoft.Xna.Framework.Matrix) this.sunLayer.view, (Microsoft.Xna.Framework.Matrix) Matrix.Identity);
        vec3_2.y -= 256f;
        vec3_2.x /= (float) viewport1.Width;
        vec3_2.y /= (float) viewport1.Height;
        this._sunshineMaterialBare.effect.effect.Parameters["lightPos"].SetValue((Vector2) new Vec2(vec3_2.x, vec3_2.y));
        this._sunshineMaterialBare.effect.effect.Parameters["weight"].SetValue(1f);
        this._sunshineMaterialBare.effect.effect.Parameters["density"].SetValue(0.4f);
        this._sunshineMaterialBare.effect.effect.Parameters["decay"].SetValue(0.68f + RockWeather.sunGlow);
        this._sunshineMaterialBare.effect.effect.Parameters["exposure"].SetValue(1f);
        Viewport viewport2 = DuckGame.Graphics.viewport;
        DuckGame.Graphics.SetRenderTarget(this._pixelTarget);
        DuckGame.Graphics.viewport = new Viewport(0, 0, this._pixelTarget.width, this._pixelTarget.height);
        DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, this.camera.getMatrix());
        DuckGame.Graphics.material = this._sunshineMaterialBare;
        float num = (float) (this._pixelTarget.width * 2) / (float) this._sunshineTarget.width;
        DuckGame.Graphics.Draw((Tex2D) this._sunshineTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(num), SpriteEffects.None);
        DuckGame.Graphics.material = (Material) null;
        DuckGame.Graphics.screen.End();
        DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
        DuckGame.Graphics.viewport = viewport2;
        Layer.blurry = false;
        Layer.HUD.fade = fade1;
        Layer.Console.fade = fade2;
        Layer.Game.fade = fade3;
        Layer.Foreground.fade = fade3;
        Layer.Background.fade = fade4;
        this._field.fade = fade5;
        this._fieldForeground.fade = fade5;
        this._fieldForeground2.fade = fade5;
        this._wall.fade = fade5;
        Layer.Game.colorMul = colorMul1;
        Layer.Background.colorMul = colorMul2;
        this.fieldMulColor = colorMul2;
        Layer.Game.colorAdd = colorAdd;
        Layer.Background.colorAdd = colorAdd;
        this.fieldAddColor = colorAdd;
        RockScoreboard._drawingSunTarget = false;
        this.sunThing.x = (float) (290.0 + (double) RockWeather.sunPos.x * 8000.0);
        this.sunThing.y = (float) (10000.0 - (double) RockWeather.sunPos.y * 8000.0);
        this.rainbowThing.y = this.rainbowThing2.y = 2000f;
        this.rainbowThing.x = this.rainbowThing2.x = (float) (-(double) this._field.scroll * 15.0 + 6800.0);
        this.rainbowThing.alpha = this._weather.rainbowLight;
        this.rainbowThing2.alpha = this._weather.rainbowLight2;
        this.rainbowThing.visible = (double) this.rainbowThing.alpha > 0.00999999977648258;
        this.rainbowThing2.visible = (double) this.rainbowThing2.alpha > 0.00999999977648258;
        RockScoreboard._drawingSunTarget = true;
        Layer.Game.camera.width = 320f;
        Layer.Game.camera.height = 180f;
        this._field.fade = Layer.Game.fade;
        this._fieldForeground.fade = Layer.Game.fade;
        this._fieldForeground2.fade = Layer.Game.fade;
        this._wall.fade = Layer.Game.fade;
        this.backgroundColor = backgroundColor;
        RockScoreboard._drawingNormalTarget = true;
        MonoMain.RenderGame(this._screenTarget);
        RockScoreboard._drawingNormalTarget = false;
        RockScoreboard._drawingSunTarget = false;
      }
    }

    public override void Update()
    {
      if (Network.isActive && (this._mode == ScoreBoardMode.ShowScores || this._mode == ScoreBoardMode.ShowWinner))
      {
        if (this._netCountdown == null)
        {
          if (Network.isServer)
          {
            this._netCountdown = new ContinueCountdown(this._mode == ScoreBoardMode.ShowScores ? 5f : 15f);
            Level.Add((Thing) this._netCountdown);
          }
          else
          {
            IEnumerable<Thing> thing = Level.current.things[typeof (ContinueCountdown)];
            if (thing.Count<Thing>() > 0)
              this._netCountdown = thing.ElementAt<Thing>(0) as ContinueCountdown;
          }
        }
        else if (this._continueHUD != null)
        {
          if (Network.isServer)
          {
            this._continueHUD.text = "@START@CONTINUE(" + (object) (int) Math.Ceiling((double) this._netCountdown.timer) + ")";
            this._netCountdown.UpdateTimer();
          }
          else
            this._continueHUD.text = "WAITING(" + (object) (int) Math.Ceiling((double) this._netCountdown.timer) + ")";
        }
      }
      bool isServer = Network.isServer;
      Network.isServer = true;
      this.backgroundColor = new Color(139, 204, 248) * this._backgroundFade;
      Layer.Game.fade = this._backgroundFade;
      Layer.Background.fade = this._backgroundFade;
      this._backgroundFade = Lerp.Float(this._backgroundFade, 1f, 0.02f);
      this._field.rise = this._fieldScroll;
      this._fieldForeground.rise = this._fieldScroll;
      this._fieldForeground2.rise = this._fieldScroll;
      this._wall.rise = this._fieldScroll;
      this._bottomRight = new Vec2(1000f, 1000f);
      this.lowestPoint = 1000f;
      bool flag1 = false;
      this._field.scroll = Lerp.Float(this._field.scroll, this._desiredScroll, 6f);
      if ((double) this._field.scroll < 297.0)
      {
        this._field.scroll = 0.0f;
        flag1 = true;
      }
      if ((double) this._field.scroll < 302.0)
        this._field.scroll = 302f;
      this._fieldForeground.scroll = this._field.scroll;
      this._fieldForeground2.scroll = this._field.scroll;
      this._wall.scroll = this._field.scroll;
      if (this._state != ScoreBoardState.Transition)
      {
        if (this._state == ScoreBoardState.Intro)
        {
          if ((double) this._animWait > 0.0)
          {
            this._animWait -= 0.021f;
          }
          else
          {
            Crowd.mood = Mood.Silent;
            this._intermissionSlide = Lerp.FloatSmooth(this._intermissionSlide, 2.1f, 0.1f, 1.05f);
            if ((double) this._intermissionSlide > 2.08999991416931)
            {
              this.controlMessage = 0;
              Vote.OpenVoting("", "", false);
              this._state = ScoreBoardState.ThrowRocks;
            }
          }
        }
        else if (this._state == ScoreBoardState.MatchOver)
        {
          if ((double) this._highestSlot.duck.position.x < (double) this._highestSlot.rock.x - 16.0)
          {
            this._highestSlot.ai.Release("LEFT");
            this._highestSlot.ai.Press("RIGHT");
          }
          if ((double) this._highestSlot.duck.position.x > (double) this._highestSlot.rock.x + 16.0)
          {
            this._highestSlot.ai.Release("RIGHT");
            this._highestSlot.ai.Press("LEFT");
          }
          if ((double) this._highestSlot.duck.position.x > (double) this._highestSlot.rock.position.x - 16.0)
            this._focusRock = true;
          for (int index = 0; index < this._highestSlot.subAIs.Count; ++index)
          {
            DuckAI subAi = this._highestSlot.subAIs[index];
            Duck subDuck = this._highestSlot.subDucks[index];
            if ((double) subDuck.position.x < (double) this._highestSlot.rock.x - 16.0)
            {
              subAi.Release("LEFT");
              subAi.Press("RIGHT");
            }
            if ((double) subDuck.position.x > (double) this._highestSlot.rock.x + 16.0)
            {
              subAi.Release("RIGHT");
              subAi.Press("LEFT");
            }
          }
          if (this._focusRock)
          {
            this._highestSlot.ai.Release("JUMP");
            if ((double) Rando.Float(1f) > 0.980000019073486)
              this._highestSlot.ai.Press("JUMP");
            for (int index = 0; index < this._highestSlot.subAIs.Count; ++index)
            {
              DuckAI subAi = this._highestSlot.subAIs[index];
              Duck subDuck = this._highestSlot.subDucks[index];
              subAi.Release("JUMP");
              if ((double) Rando.Float(1f) > 0.980000019073486)
                subAi.Press("JUMP");
            }
            if (!this._droppedConfetti)
            {
              this._desiredScroll = this._highestSlot.duck.position.x;
              if ((double) this._desiredScroll >= (double) this._highestSlot.rock.position.x)
              {
                this._desiredScroll = this._highestSlot.rock.position.x;
                Crowd.mood = Mood.Extatic;
                this._droppedConfetti = true;
                for (int index = 0; index < 64; ++index)
                  Level.Add((Thing) new Confetti(this._confettiDrop + Rando.Float(-32f, 32f), this._highestSlot.rock.y - 220f - Rando.Float(50f)));
              }
            }
            if (Network.isServer && (Input.Pressed("START") || this._netCountdown != null && (double) this._netCountdown.timer <= 0.0))
              this._finished = true;
            this._winnerWait -= 0.007f;
            if ((double) this._winnerWait < 0.0)
              this._finished = true;
          }
          else
          {
            this._desiredScroll = this._highestSlot.duck.position.x;
            Crowd.mood = Mood.Excited;
          }
        }
      }
      if (this._state == ScoreBoardState.ThrowRocks)
      {
        if (!this._shiftCamera)
          this._controlSlide = Lerp.FloatSmooth(this._controlSlide, 1f, 0.1f, 1.05f);
        bool flag2 = true;
        foreach (Slot3D slot in this._slots)
        {
          slot.follow = false;
          if (flag2)
          {
            if (slot.state != RockThrow.Finished)
            {
              flag2 = false;
              slot.follow = true;
            }
            else if (slot == this._slots[this._slots.Count - 1])
            {
              if (this._matchOver)
                this._skipFade = true;
              else
                this._state = ScoreBoardState.ShowBoard;
            }
            if (slot.state == RockThrow.Idle)
              slot.state = RockThrow.PickUpRock;
            if (slot.state == RockThrow.PickUpRock)
            {
              if ((double) slot.duck.position.x < (double) slot.rock.position.x)
              {
                slot.ai.Press("RIGHT");
              }
              else
              {
                slot.state = RockThrow.ThrowRock;
                slot.duck.position.x = slot.rock.position.x;
                slot.duck.hSpeed = 0.0f;
                this._throwWait = 1.1f;
              }
            }
            if (slot.state == RockThrow.ThrowRock)
            {
              if ((double) this._throwWait > 0.0)
              {
                this._throwWait -= 0.08f;
                slot.ai.Release("RIGHT");
                slot.duck.GiveHoldable((Holdable) slot.rock);
                this._afterThrowWait = 0.8f;
              }
              else
              {
                if (slot.duck.holdObject != null)
                {
                  this._misfire = false;
                  slot.duck.ThrowItem();
                  float num1 = (float) slot.duck.profile.team.rockScore;
                  int num2 = GameMode.winsPerSet * 2;
                  if ((double) num1 > (double) (num2 - 2))
                    num1 = (float) (num2 - 2) + Math.Min((float) (slot.duck.profile.team.rockScore - GameMode.winsPerSet * 2) / 16f, 1f);
                  float num3 = (float) ((double) slot.startX + 30.0 + (double) num1 / (double) num2 * (double) this._fieldWidth) - slot.rock.x;
                  slot.rock.vSpeed = (float) (-2.0 - (double) Maths.Clamp(num3 / 300f, 0.0f, 1f) * 4.0);
                  float num4 = Math.Abs(2f * slot.rock.vSpeed) / slot.rock.currentGravity;
                  double currentFriction = (double) slot.rock.currentFriction;
                  float num5 = num3 / num4;
                  slot.rock.frictionMult = 0.0f;
                  slot.rock.grounded = false;
                  slot.rock.hMax = 100f;
                  slot.rock.vMax = 100f;
                  if (slot.duck.profile.team.rockScore == slot.duck.profile.team.prevScoreboardScore)
                  {
                    num5 = 0.3f;
                    slot.rock.vSpeed = -0.6f;
                    this._misfire = true;
                  }
                  slot.rock.hSpeed = num5 * 0.88f;
                  if (RockScoreboard.wallMode && slot.duck.profile.team.rockScore > GameMode.winsPerSet)
                    ++slot.rock.hSpeed;
                }
                if (slot.rock.grounded)
                {
                  float num1 = 0.015f;
                  int num2 = slot.duck.profile.team.rockScore - slot.duck.profile.team.prevScoreboardScore;
                  if (num2 == 0)
                    Crowd.mood = Mood.Dead;
                  else if (num2 < 2)
                    Crowd.mood = Mood.Calm;
                  else if (num2 < 5)
                  {
                    Crowd.mood = Mood.Excited;
                    num1 = 0.013f;
                  }
                  else if (num2 < 99)
                  {
                    Crowd.mood = Mood.Extatic;
                    num1 = 0.01f;
                  }
                  int rockScore = slot.duck.profile.team.rockScore;
                  int winsPerSet = GameMode.winsPerSet;
                  if ((double) slot.rock.frictionMult == 0.0)
                  {
                    Sprite s;
                    switch (RockWeather.weather)
                    {
                      case Weather.Snowing:
                        s = new Sprite("rockThrow/rockSmudgeSnow");
                        break;
                      case Weather.Raining:
                        s = new Sprite("rockThrow/rockSmudgeMud");
                        break;
                      default:
                        s = new Sprite("rockThrow/rockSmudge");
                        break;
                    }
                    s.position = new Vec2(slot.rock.x - 12f, slot.rock.z - 10f);
                    s.depth = (Depth) 0.9f;
                    s.xscale = 0.8f;
                    s.yscale = 1.4f;
                    s.alpha = 0.9f;
                    this._field.AddSprite(s);
                  }
                  ++slot.slideWait;
                  if (slot.slideWait > 3 && (double) slot.rock.hSpeed > 0.0)
                  {
                    Sprite s;
                    switch (RockWeather.weather)
                    {
                      case Weather.Snowing:
                        s = new Sprite("rockThrow/rockSmearSnow");
                        break;
                      case Weather.Raining:
                        s = new Sprite("rockThrow/rockSmearMud");
                        break;
                      default:
                        s = new Sprite("rockThrow/rockSmear");
                        break;
                    }
                    s.position = new Vec2(slot.rock.x - 5f, slot.rock.z - 10f);
                    s.depth = (Depth) 0.9f;
                    s.xscale = 0.6f;
                    s.yscale = 1.4f;
                    s.alpha = 0.9f;
                    slot.slideWait = 0;
                    this._field.AddSprite(s);
                  }
                  slot.rock.frictionMult = 4f;
                  this._afterThrowWait -= num1;
                  if ((double) this._afterThrowWait < 0.400000005960464)
                  {
                    slot.state = RockThrow.ShowScore;
                    SFX.Play("scoreDing");
                    if (RockScoreboard.wallMode && slot.duck.profile.team.rockScore > GameMode.winsPerSet)
                      slot.duck.profile.team.rockScore = GameMode.winsPerSet;
                    this._showScoreWait = 0.9f;
                    Crowd.ThrowHats(slot.duck.profile);
                    if (!slot.showScore)
                    {
                      slot.showScore = true;
                      PointBoard pointBoard = new PointBoard((Thing) slot.rock, slot.duck.profile.team);
                      pointBoard.depth = slot.rock.depth + 1;
                      pointBoard.z = slot.rock.z;
                      Level.Add((Thing) pointBoard);
                    }
                  }
                }
                else
                {
                  int rockScore = slot.duck.profile.team.rockScore;
                  int num = GameMode.winsPerSet * 2;
                  if (!this._misfire && (double) slot.rock.x > (double) slot.startX + 30.0 + (double) rockScore / (double) num * (double) this._fieldWidth)
                    slot.rock.x = (float) ((double) slot.startX + 30.0 + (double) rockScore / (double) num * (double) this._fieldWidth);
                }
              }
            }
            if (slot.state == RockThrow.ShowScore)
            {
              this._showScoreWait -= 0.016f;
              if ((double) this._showScoreWait < 0.0)
                slot.state = RockThrow.RunBack;
            }
            if (slot.state == RockThrow.RunBack)
            {
              if (slot == this._slots[this._slots.Count - 1])
                slot.follow = false;
              if ((double) slot.duck.position.x > (double) slot.startX)
              {
                slot.ai.Press("LEFT");
              }
              else
              {
                slot.duck.position.x = slot.startX;
                slot.duck.hSpeed = 0.0f;
                slot.duck.offDir = (sbyte) 1;
                slot.ai.Release("LEFT");
                this._backWait -= 0.05f;
                Crowd.mood = Mood.Silent;
                if ((double) this._backWait < 0.0 && (flag1 || slot == this._slots[this._slots.Count - 1]))
                {
                  slot.state = RockThrow.Finished;
                  this._backWait = 1f;
                }
              }
            }
          }
          if (slot.follow)
            this._desiredScroll = slot.state == RockThrow.ThrowRock || slot.state == RockThrow.ShowScore ? slot.rock.position.x : slot.duck.position.x;
          if (Input.Pressed("START"))
          {
            foreach (Profile who in Profiles.all)
            {
              if (who.inputProfile != null && who.inputProfile.Pressed("START") && (!Network.isActive || who.connection == DuckNetwork.localConnection))
              {
                Vote.RegisterVote(who, VoteType.Skip);
                if (Network.isActive)
                  Send.Message((NetMessage) new NMVoteToSkip(who.networkIndex));
              }
            }
          }
          if (Vote.Passed(VoteType.Skip))
            this._skipFade = true;
        }
      }
      else
        Vote.CloseVoting();
      if (this._state == ScoreBoardState.MatchOver)
      {
        Network.isServer = isServer;
        this._controlSlide = Lerp.FloatSmooth(this._controlSlide, this.controlMessage == 1 ? 1f : 0.0f, 0.1f, 1.05f);
        if ((double) this._controlSlide < 0.00999999977648258)
          this.controlMessage = -1;
      }
      if (this._state == ScoreBoardState.ShowBoard)
      {
        Network.isServer = isServer;
        this._shiftCamera = true;
        this._controlSlide = Lerp.FloatSmooth(this._controlSlide, this.controlMessage == 1 ? 1f : 0.0f, 0.1f, 1.05f);
        if ((double) this._controlSlide < 0.00999999977648258)
          this.controlMessage = 1;
      }
      if (this._shiftCamera)
      {
        if (this._state == ScoreBoardState.ThrowRocks)
          this._controlSlide = Lerp.FloatSmooth(this._controlSlide, 0.0f, 0.1f, 1.05f);
        this._desiredScroll = -79f;
        if ((double) this._fieldScroll < 220.0)
        {
          this._fieldScroll += 4f;
        }
        else
        {
          if (this._state == ScoreBoardState.ThrowRocks)
            this._state = ScoreBoardState.ShowBoard;
          if (!this._scoreBoard.activated)
            this._scoreBoard.Activate();
          if (!this._finished && isServer && (Input.Pressed("START") || this._netCountdown != null && (double) this._netCountdown.timer <= 0.0))
            this._finished = true;
          Crowd.mood = Mood.Dead;
        }
      }
      if (this._skipFade)
      {
        Network.isServer = isServer;
        this.controlMessage = -1;
        DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 0.0f, 0.02f);
        if ((double) DuckGame.Graphics.fade < 0.00999999977648258)
        {
          this._skipFade = false;
          if (this._mode == ScoreBoardMode.ShowScores)
          {
            if (!this._matchOver)
            {
              this._state = ScoreBoardState.ShowBoard;
              this._fieldScroll = 220f;
              this._desiredScroll = -79f;
              this._field.scroll = this._desiredScroll;
            }
            else
            {
              this._state = ScoreBoardState.MatchOver;
              this._field.scroll = 0.0f;
              foreach (Slot3D slot in this._slots)
              {
                float num1 = (float) slot.duck.profile.team.rockScore;
                int num2 = GameMode.winsPerSet * 2;
                if ((double) num1 > (double) (num2 - 2))
                  num1 = (float) (num2 - 2) + Math.Min((float) (slot.duck.profile.team.rockScore - GameMode.winsPerSet * 2) / 16f, 1f);
                slot.rock.x = (float) ((double) slot.startX + 30.0 + (double) num1 / (double) num2 * (double) this._fieldWidth);
                if (RockScoreboard.wallMode && slot.duck.profile.team.rockScore >= GameMode.winsPerSet)
                  slot.rock.x -= 10f;
                slot.rock.hSpeed = 0.0f;
              }
            }
          }
          else if (this._afterHighlights)
          {
            this._fieldScroll = 220f;
            this._desiredScroll = -79f;
            this._field.scroll = this._desiredScroll;
            this._scoreBoard.Activate();
            this._viewBoard = true;
          }
          else
            Level.current = !isServer || !Network.isActive ? (Level) new HighlightLevel() : (Level) new RockScoreboard(RockScoreboard._returnLevel, ScoreBoardMode.ShowWinner, true);
        }
      }
      if (this._finished)
      {
        DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 0.0f, 0.02f);
        if ((double) DuckGame.Graphics.fade < 0.00999999977648258)
        {
          foreach (Team team in Teams.all)
            team.prevScoreboardScore = team.score;
          if (isServer)
          {
            if (this._mode == ScoreBoardMode.ShowWinner)
            {
              Main.ResetMatchStuff();
              if (this._hatSelect)
                Level.current = (Level) new TeamSelect2();
              else if (!this._quit)
              {
                Music.Stop();
                Level.current = RockScoreboard._returnLevel;
                DuckGame.Graphics.fade = 1f;
              }
              else
              {
                if (Network.isActive)
                  Network.Disconnect();
                Level.current = (Level) new TitleScreen();
              }
            }
            else if (this._state != ScoreBoardState.MatchOver)
            {
              Music.Stop();
              Level.current = RockScoreboard._returnLevel;
              DuckGame.Graphics.fade = 1f;
            }
            else
              Level.current = (Level) new RockScoreboard(RockScoreboard._returnLevel, ScoreBoardMode.ShowWinner);
          }
        }
      }
      else if (!this._skipFade && !this._finished)
        DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 1f, 0.02f);
      Network.isServer = isServer;
      if (this._mode == ScoreBoardMode.ShowWinner)
      {
        this._controlSlide = Lerp.FloatSmooth(this._controlSlide, this.controlMessage == 1 ? 1f : 0.0f, 0.1f, 1.05f);
        if ((double) this._controlSlide < 0.00999999977648258)
          this.controlMessage = 1;
        if (this._viewBoard)
        {
          this.controlMessage = 2;
          this._controlSlide = 1f;
        }
        if (!this._scoreBoard.activated)
        {
          if (isServer && (Input.Pressed("START") || this._netCountdown != null && (double) this._netCountdown.timer <= 0.0))
          {
            if (Network.isActive)
            {
              Level.current = (Level) new RockScoreboard(RockScoreboard._returnLevel, ScoreBoardMode.ShowWinner, true);
            }
            else
            {
              this._takePicture = true;
              HUD.CloseAllCorners();
            }
          }
          if (this._takePicture && this._flashSkipFrames == 0)
          {
            this._cameraWait -= 0.01f;
            if ((double) this._cameraWait < 0.600000023841858 && this._playedBeeps == 0)
            {
              this._playedBeeps = 1;
              SFX.Play("cameraBeep", pitch: -0.5f);
            }
            else if ((double) this._cameraWait < 0.300000011920929 && this._playedBeeps == 1)
            {
              this._playedBeeps = 2;
              SFX.Play("cameraBeep", pitch: -0.5f);
            }
            if ((double) this._cameraWait < 0.0 && !this._playedFlash)
            {
              this._playedFlash = true;
              SFX.Play("cameraFlash", 0.8f, 1f);
            }
            if ((double) this._cameraWait < 0.100000001490116)
            {
              this._cameraFadeVel += 3f / 1000f;
              if ((double) this._cameraWait < 0.0399999991059303)
                this._cameraFadeVel += 0.01f;
            }
            DuckGame.Graphics.fadeAdd += this._cameraFadeVel;
            if ((double) DuckGame.Graphics.fadeAdd > 1.0)
            {
              int screenWidth = MonoMain.screenWidth;
              int screenHeight = MonoMain.screenHeight;
              if (!RockScoreboard._sunEnabled)
              {
                int num1 = DuckGame.Graphics.height / 4 * 3 + 30;
                DuckGame.Graphics.fadeAdd = 0.0f;
                Layer.Background.fade = 0.8f;
                ++this._flashSkipFrames;
                RockScoreboard.finalImage = new RenderTarget2D(screenWidth, screenHeight);
                Layer.Game.visible = false;
                Rectangle scissor = this._field.scissor;
                this._field.scissor = new Rectangle(0.0f, 0.0f, (float) DuckGame.Graphics.width, (float) num1);
                this._field.visible = true;
                MonoMain.RenderGame(RockScoreboard.finalImage);
                Layer.Game.visible = true;
                Color backgroundColor = Level.current.backgroundColor;
                Level.current.backgroundColor = Color.Transparent;
                RockScoreboard.finalImage2 = new RenderTarget2D(screenWidth, screenHeight);
                Layer.allVisible = false;
                Layer.Game.visible = true;
                int num2 = num1 - 5;
                this._field.scissor = new Rectangle(0.0f, (float) num2, (float) screenWidth, (float) (screenHeight - num2));
                this._field.visible = true;
                MonoMain.RenderGame(RockScoreboard.finalImage2);
                this._field.scissor = scissor;
                Layer.allVisible = true;
                Level.current.backgroundColor = backgroundColor;
                this._getScreenshot = true;
                this._finalSprite = new Sprite(RockScoreboard.finalImage, 0.0f, 0.0f);
                Stream stream = (Stream) DuckFile.Create(DuckFile.albumDirectory + "album" + DateTime.Now.ToString("MM-dd-yy H;mm;ss") + ".png");
                ((Texture2D) this._finalSprite.texture.nativeObject).SaveAsPng(stream, screenWidth, screenHeight);
                stream.Dispose();
              }
              else
              {
                DuckGame.Graphics.fadeAdd = 0.0f;
                Layer.Background.fade = 0.8f;
                this._weather.Update();
                this.DoRender();
                RockScoreboard.finalImage = new RenderTarget2D(screenWidth, screenHeight);
                this.RenderFinalImage(RockScoreboard.finalImage);
                this._finalSprite = new Sprite(RockScoreboard.finalImage, 0.0f, 0.0f);
                this._getScreenshot = true;
                DuckGame.Graphics.fadeAdd = 1f;
                int width = screenWidth / 4;
                int height = screenHeight / 4;
                DuckGame.Graphics.fadeAdd = 0.0f;
                Layer.Background.fade = 0.8f;
                this.DoRender();
                RenderTarget2D image = new RenderTarget2D(width, height);
                this.RenderFinalImage(image);
                DuckGame.Graphics.fadeAdd = 1f;
                Stream stream = (Stream) DuckFile.Create(DuckFile.albumDirectory + DateTime.Now.ToString("MM-dd-yy H;mm") + ".png");
                (image.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).SaveAsPng(stream, width, height);
                stream.Dispose();
                this.DoRender();
              }
              DuckGame.Graphics.snap = 4f;
            }
          }
          if (this._getScreenshot && DuckGame.Graphics.screenCapture == null)
          {
            Level.current.simulatePhysics = false;
            ++this._flashSkipFrames;
            if (this._flashSkipFrames > 2)
              DuckGame.Graphics.fadeAdd = 1f;
            if (this._flashSkipFrames > 20)
              Level.current = (Level) new HighlightLevel();
          }
        }
        else if (!this._finished && isServer)
        {
          if (Input.Pressed("START") || this._netCountdown != null && (double) this._netCountdown.timer <= 0.0)
            this._finished = true;
          if (Input.Pressed("QUACK"))
          {
            this._finished = true;
            this._quit = true;
          }
          if (Input.Pressed("GRAB"))
          {
            this._finished = true;
            this._hatSelect = true;
          }
        }
      }
      Network.isServer = isServer;
      base.Update();
    }

    public override void Terminate()
    {
      if (this._mode == ScoreBoardMode.ShowWinner)
      {
        foreach (Team team in Teams.all)
          team.prevScoreboardScore = 0;
      }
      else
      {
        foreach (Team team in Teams.all)
          team.prevScoreboardScore = team.score;
      }
      Vote.CloseVoting();
    }

    public void RenderFinalImage(RenderTarget2D image)
    {
      if (this._sunshineMaterial == null)
        this._sunshineMaterial = (Material) new MaterialSunshine(this._screenTarget);
      DuckGame.Graphics.SetRenderTarget(image);
      DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, this.camera.getMatrix());
      DuckGame.Graphics.material = this._sunshineMaterial;
      float num = (float) DuckGame.Graphics.width / ((float) this._pixelTarget.width * ((float) DuckGame.Graphics.width / 320f));
      DuckGame.Graphics.Draw((Tex2D) this._pixelTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(num), SpriteEffects.None);
      DuckGame.Graphics.material = (Material) null;
      DuckGame.Graphics.screen.End();
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
    }

    public override void DoDraw()
    {
      if (NetworkDebugger.enabled)
        base.DoDraw();
      else if (!RockScoreboard._drawingSunTarget && RockScoreboard._sunEnabled)
      {
        if (RockScoreboard._sunEnabled)
          this.DoRender();
        if (this._sunshineMaterial == null)
          this._sunshineMaterial = (Material) new MaterialSunshine(this._screenTarget);
        if (NetworkDebugger.enabled)
        {
          DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, this.camera.getMatrix());
          float num = (float) DuckGame.Graphics.width / ((float) this._screenTarget.width * ((float) DuckGame.Graphics.width / 320f));
          DuckGame.Graphics.Draw((Tex2D) this._screenTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(num), SpriteEffects.None);
          DuckGame.Graphics.material = (Material) null;
          DuckGame.Graphics.screen.End();
        }
        else
        {
          DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, this.camera.getMatrix());
          DuckGame.Graphics.material = this._sunshineMaterial;
          float num = (float) DuckGame.Graphics.width / ((float) this._pixelTarget.width * ((float) DuckGame.Graphics.width / 320f));
          DuckGame.Graphics.Draw((Tex2D) this._pixelTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(num), SpriteEffects.None);
          DuckGame.Graphics.material = (Material) null;
          DuckGame.Graphics.screen.End();
        }
      }
      else
        base.DoDraw();
    }

    public override void Draw()
    {
      Layer.Game.perspective = this._mode == ScoreBoardMode.ShowScores;
      Layer.Game.projection = this._field.projection;
      Layer.Game.view = this._field.view;
      Layer.Background.perspective = true;
      Layer.Background.projection = this._field.projection;
      Layer.Background.view = this._field.view;
      Layer.Foreground.perspective = true;
      Layer.Foreground.projection = this._field.projection;
      Layer.Foreground.view = this._field.view;
      if (!RockScoreboard._sunEnabled)
        return;
      this._sunLayer.perspective = true;
      this._sunLayer.projection = this._field.projection;
      this._sunLayer.view = this._field.view;
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.HUD)
      {
        if (this._getScreenshot && DuckGame.Graphics.screenCapture == null)
        {
          this._finalSprite.scale = new Vec2(0.25f, 0.25f);
          DuckGame.Graphics.Draw(this._finalSprite, 0.0f, 0.0f);
        }
        if ((double) this._intermissionSlide > 0.00999999977648258)
        {
          this._intermissionText.depth = (Depth) 0.91f;
          float x1 = (float) ((double) this._intermissionSlide * 320.0 - 320.0);
          float y = 60f;
          DuckGame.Graphics.DrawRect(new Vec2(x1, y), new Vec2(x1 + 320f, y + 30f), Color.Black, (Depth) 0.9f);
          float x2 = (float) (320.0 - (double) this._intermissionSlide * 320.0);
          float num = 60f;
          DuckGame.Graphics.DrawRect(new Vec2(x2, num + 30f), new Vec2(x2 + 320f, num + 60f), Color.Black, (Depth) 0.9f);
          DuckGame.Graphics.Draw(this._intermissionText, (float) ((double) this._intermissionSlide * 336.0 - 320.0), num + 18f);
        }
      }
      else if (layer == Layer.Game)
      {
        if (this._mode == ScoreBoardMode.ShowWinner && !this._afterHighlights)
        {
          this._winnerPost.depth = (Depth) -0.962f;
          this._winnerBanner.depth = (Depth) -0.858f;
          float num1 = -10f;
          DuckGame.Graphics.Draw(this._winnerPost, 63f, 40f + num1);
          DuckGame.Graphics.Draw(this._winnerPost, 248f, 40f + num1);
          DuckGame.Graphics.Draw(this._winnerBanner, 70f, 43f + num1);
          string name = Results.winner.name;
          BitmapFont font = Results.winner.font;
          font.scale = new Vec2(2f, 2f);
          float num2 = 0.0f;
          float num3 = 0.0f;
          if (name.Length > 12)
          {
            font.scale = new Vec2(1f);
            num3 = 3f;
          }
          else if (name.Length > 9)
          {
            font.scale = new Vec2(1.5f);
            num2 = 2f;
            num3 = 1f;
          }
          font.Draw(name, (float) (160.0 - (double) font.GetWidth(name) / 2.0) + num2, 50f + num1 + num3, Color.Black, this._winnerBanner.depth + 1);
          font.scale = new Vec2(1f, 1f);
        }
      }
      else if (layer == Layer.Foreground)
      {
        int num4 = RockScoreboard._drawingSunTarget ? 1 : 0;
      }
      base.PostDrawLayer(layer);
    }
  }
}
