﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ArcadeLevel : XMLLevel
  {
    protected FollowCam _followCam;
    private List<Duck> _pendingSpawns;
    private Duck _duck;
    private ArcadeMode _arcade;
    private UIComponent _pauseGroup;
    private UIMenu _pauseMenu;
    private UIMenu _confirmMenu;
    private ArcadeState _state;
    private ArcadeState _desiredState;
    private ArcadeHUD _hud;
    private UnlockScreen _unlockScreen;
    private List<ArcadeMachine> _unlockMachines = new List<ArcadeMachine>();
    private bool _unlockingMachine;
    public List<ArcadeMachine> _challenges = new List<ArcadeMachine>();
    private PrizeTable _prizeTable;
    private object _hoverThing;
    private ArcadeMachine _hoverMachine;
    public static ArcadeLevel currentArcade;
    private bool _launchedChallenge;
    private bool _flipState;
    private float _unlockMachineWait = 1f;
    private bool _paused;
    private bool _quitting;
    private MenuBoolean _quit = new MenuBoolean();
    private bool _afterChallenge;
    private List<ArcadeFrame> _frames = new List<ArcadeFrame>();
    public bool basementWasUnlocked;
    private bool spawnKey;
    private float spawnKeyWait = 0.2f;
    public bool returnToChallengeList;
    public bool launchSpecialChallenge;

    public FollowCam followCam => this._followCam;

    public ArcadeLevel(string name)
      : base(name)
    {
      this._followCam = new FollowCam();
      this._followCam.lerpMult = 2f;
      this._followCam.startCentered = false;
      this.camera = (Camera) this._followCam;
    }

    public void UpdateDefault()
    {
      if (Profiles.IsDefault(Profiles.active[0]))
      {
        InputProfile inputProfile = Profiles.active[0].inputProfile;
        Team team = Profiles.active[0].team;
        Profiles.active[0].team = (Team) null;
        Profiles.DefaultPlayer1.team = team;
        Profiles.DefaultPlayer1.inputProfile = inputProfile;
      }
      if (Level.current == null)
        return;
      foreach (Door door in Level.current.things[typeof (Door)])
      {
        if (door._lockDoor)
          door.locked = !Unlocks.IsUnlocked("BASEMENTKEY", Profiles.active[0]);
      }
    }

    public void CheckFrames()
    {
      float challengeSkillIndex = Challenges.GetChallengeSkillIndex();
      foreach (ArcadeFrame frame in this._frames)
      {
        if ((double) challengeSkillIndex >= (double) (float) frame.respect && ChallengeData.CheckRequirement(Profiles.active[0], (string) frame.requirement))
          frame.visible = true;
        else
          frame.visible = false;
      }
    }

    public ArcadeFrame GetFrame()
    {
      float challengeSkillIndex = Challenges.GetChallengeSkillIndex();
      foreach (ArcadeFrame arcadeFrame in (IEnumerable<ArcadeFrame>) this._frames.OrderBy<ArcadeFrame, int>((Func<ArcadeFrame, int>) (x => x.saveData == null ? Rando.Int(100) : Rando.Int(100) + 200)))
      {
        if ((double) challengeSkillIndex >= (double) (float) arcadeFrame.respect && ChallengeData.CheckRequirement(Profiles.active[0], (string) arcadeFrame.requirement))
          return arcadeFrame;
      }
      return (ArcadeFrame) null;
    }

    public ArcadeFrame GetFrame(string id) => this._frames.FirstOrDefault<ArcadeFrame>((Func<ArcadeFrame, bool>) (x => x._identifier == id));

    public void InitializeMachines()
    {
      base.Initialize();
      foreach (ArcadeMachine arcadeMachine in this.things[typeof (ArcadeMachine)])
        this._challenges.Add(arcadeMachine);
    }

    public override void Initialize()
    {
      TeamSelect2.DefaultSettings();
      base.Initialize();
      this.UpdateDefault();
      bool flag = true;
      foreach (Profile prof in Profiles.active)
      {
        if (flag)
        {
          flag = false;
        }
        else
        {
          if (prof.team != null)
            prof.team.Leave(prof);
          prof.inputProfile = (InputProfile) null;
        }
      }
      this._pendingSpawns = new Deathmatch((Level) this).SpawnPlayers();
      this._pendingSpawns = this._pendingSpawns.OrderBy<Duck, float>((Func<Duck, float>) (sp => sp.x)).ToList<Duck>();
      foreach (Duck pendingSpawn in this._pendingSpawns)
      {
        this.followCam.Add((Thing) pendingSpawn);
        ((ArcadeHatConsole)Level.First<ArcadeHatConsole>())?.MakeHatSelector(pendingSpawn);
      }
      this.followCam.Adjust();
      foreach (ArcadeMachine arcadeMachine in this.things[typeof (ArcadeMachine)])
        this._challenges.Add(arcadeMachine);
      Profiles.active[0].ticketCount = Challenges.GetTicketCount(Profiles.active[0]);
      if (Profiles.active[0].ticketCount < 0)
        Profiles.active[0].ticketCount = 0;
      foreach (ArcadeFrame arcadeFrame in this.things[typeof (ArcadeFrame)])
        this._frames.Add(arcadeFrame);
      foreach (ChallengeSaveData challengeSaveData in Challenges.GetAllSaveData())
      {
        if (challengeSaveData.frameID != "")
        {
          ArcadeFrame frame = this.GetFrame(challengeSaveData.frameID);
          if (frame != null)
            frame.saveData = challengeSaveData;
        }
      }
      foreach (ArcadeMachine challenge in this._challenges)
        challenge.unlocked = challenge.CheckUnlocked(false);
      this._hud = new ArcadeHUD();
      this._hud.alpha = 0.0f;
      Level.Add((Thing) this._hud);
      this._unlockScreen = new UnlockScreen();
      this._unlockScreen.alpha = 0.0f;
      Level.Add((Thing) this._unlockScreen);
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._pauseMenu = new UIMenu("@LWING@CHALLENGE MODE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@DPAD@MOVE  @SELECT@SELECT");
      this._confirmMenu = new UIMenu("REALLY QUIT?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      UIDivider uiDivider = new UIDivider(true, 0.8f);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("OPTIONS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) Options.optionsMenu), UIAlign.Left), true);
      uiDivider.leftSection.Add((UIComponent) new UIMenuItem("QUIT", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) this._confirmMenu), UIAlign.Left), true);
      uiDivider.rightSection.Add((UIComponent) new UIImage("pauseIcons", UIAlign.Right), true);
      this._pauseMenu.Add((UIComponent) uiDivider, true);
      this._pauseMenu.Close();
      this._pauseGroup.Add((UIComponent) this._pauseMenu, false);
      this._pauseGroup.Add((UIComponent) Options.optionsMenu, false);
      Options.openOnClose = this._pauseMenu;
      this._confirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._confirmMenu, (UIComponent) this._pauseMenu)), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._quit)), true);
      this._confirmMenu.Close();
      this._pauseGroup.Add((UIComponent) this._confirmMenu, false);
      this._pauseGroup.Close();
      Level.Add((Thing) this._pauseGroup);
      this._prizeTable = this.things[typeof (PrizeTable)].FirstOrDefault<Thing>() as PrizeTable;
      if (this._prizeTable == null)
        this._prizeTable = new PrizeTable(730f, 124f);
      Chancy.activeChallenge = (ChallengeData) null;
      Chancy.atCounter = true;
      Chancy.lookingAtChallenge = false;
      Graphics.fade = 1f;
      this.basementWasUnlocked = Unlocks.IsUnlocked("BASEMENTKEY", Profiles.active[0]);
      Level.Add((Thing) this._prizeTable);
      Music.Play("Arcade");
    }

    public override void Terminate()
    {
    }

    public override void Update()
    {
      Options.openOnClose = this._pauseMenu;
      if (this.spawnKey)
      {
        if ((double) this.spawnKeyWait > 0.0)
        {
          this.spawnKeyWait -= Maths.IncFrameTimer();
        }
        else
        {
          SFX.Play("ching");
          this.spawnKey = false;
          Key key = new Key(this._prizeTable.x, this._prizeTable.y);
          key.vSpeed = -4f;
          key.depth = this._duck.depth + 50;
          Level.Add((Thing) SmallSmoke.New(key.x + Rando.Float(-4f, 4f), key.y + Rando.Float(-4f, 4f)));
          Level.Add((Thing) SmallSmoke.New(key.x + Rando.Float(-4f, 4f), key.y + Rando.Float(-4f, 4f)));
          Level.Add((Thing) SmallSmoke.New(key.x + Rando.Float(-4f, 4f), key.y + Rando.Float(-4f, 4f)));
          Level.Add((Thing) SmallSmoke.New(key.x + Rando.Float(-4f, 4f), key.y + Rando.Float(-4f, 4f)));
          Level.Add((Thing) key);
        }
      }
      Chancy.Update();
      if (this._pendingSpawns != null && this._pendingSpawns.Count > 0)
      {
        Duck pendingSpawn = this._pendingSpawns[0];
        this.AddThing((Thing) pendingSpawn);
        this._pendingSpawns.RemoveAt(0);
        this._duck = pendingSpawn;
        this._arcade = this.things[typeof (ArcadeMode)].First<Thing>() as ArcadeMode;
      }
      Layer.Lighting.fade = Layer.Lighting2.fade = 1f - Math.Min(1f, Math.Max(0.0f, (float) ((1.0 - (double) Layer.Game.fade) * 1.5)));
      this.backgroundColor = Color.Black;
      if (UnlockScreen.open || ArcadeHUD.open)
      {
        foreach (Thing challenge in this._challenges)
          challenge.visible = false;
        this._prizeTable.visible = false;
      }
      else
      {
        foreach (Thing challenge in this._challenges)
          challenge.visible = true;
        this._prizeTable.visible = true;
      }
      if (this._state == this._desiredState && this._state != ArcadeState.UnlockMachine && this._state != ArcadeState.LaunchChallenge)
      {
        if (!this._quitting)
        {
          if (Input.Pressed("START"))
          {
            this._pauseGroup.Open();
            this._pauseMenu.Open();
            MonoMain.pauseMenu = this._pauseGroup;
            if (!this._paused)
            {
              Music.Pause();
              SFX.Play("pause", 0.6f);
              this._paused = true;
              this._duck.immobilized = true;
            }
            this.simulatePhysics = false;
            return;
          }
          if (this._paused && MonoMain.pauseMenu == null)
          {
            this._paused = false;
            SFX.Play("resume", 0.6f);
            if (this._quit.value)
            {
              this._quitting = true;
            }
            else
            {
              Music.Resume();
              this._duck.immobilized = false;
              this.simulatePhysics = true;
            }
          }
        }
        else
        {
          Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.02f);
          if ((double) Graphics.fade > 0.00999999977648258)
            return;
          Level.current = (Level) new TitleScreen();
          return;
        }
      }
      if (this._paused)
        return;
      if (this._hud.launchChallenge)
        this._desiredState = ArcadeState.LaunchChallenge;
      if (this._desiredState != this._state)
      {
        this._duck.active = false;
        bool flag = false;
        if (this._desiredState == ArcadeState.ViewChallenge)
        {
          this._duck.alpha = Lerp.FloatSmooth(this._duck.alpha, 0.0f, 0.1f);
          this._followCam.manualViewSize = Lerp.FloatSmooth(this._followCam.manualViewSize, 2f, 0.16f);
          if ((double) this._followCam.manualViewSize < 30.0)
          {
            Layer.Game.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.08f);
            Layer.Background.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.08f);
            this._hud.alpha = Lerp.Float(this._hud.alpha, 1f, 0.08f);
            if ((double) this._followCam.manualViewSize < 3.0 && (double) this._hud.alpha == 1.0 && (double) Layer.Game.fade == 0.0)
              flag = true;
          }
        }
        else if (this._desiredState == ArcadeState.Normal)
        {
          if (!this._flipState)
          {
            this._followCam.Clear();
            this._followCam.Add((Thing) this._duck);
            HUD.CloseAllCorners();
          }
          this._duck.alpha = Lerp.FloatSmooth(this._duck.alpha, 1f, 0.1f, 1.1f);
          if (this._state == ArcadeState.ViewChallenge || this._state == ArcadeState.UnlockScreen)
            this._followCam.manualViewSize = Lerp.FloatSmooth(this._followCam.manualViewSize, this._followCam.viewSize, 0.14f, 1.05f);
          Layer.Game.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          Layer.Background.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          this._hud.alpha = Lerp.Float(this._hud.alpha, 0.0f, 0.08f);
          this._unlockScreen.alpha = Lerp.Float(this._unlockScreen.alpha, 0.0f, 0.08f);
          if (((double) this._followCam.manualViewSize < 0.0 || (double) this._followCam.manualViewSize == (double) this._followCam.viewSize) && ((double) this._hud.alpha == 0.0 && (double) Layer.Game.fade == 1.0))
          {
            flag = true;
            this._followCam.manualViewSize = -1f;
            this._duck.alpha = 1f;
          }
          if (Unlockables.HasPendingUnlocks())
            MonoMain.pauseMenu = (UIComponent) new UIUnlockBox(Unlockables.GetPendingUnlocks().ToList<Unlockable>(), Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f);
        }
        else if (this._desiredState == ArcadeState.ViewSpecialChallenge || this._desiredState == ArcadeState.ViewChallengeList || this._desiredState == ArcadeState.ViewProfileSelector)
        {
          if (!this._flipState)
          {
            this._followCam.Clear();
            this._followCam.Add((Thing) this._duck);
            HUD.CloseAllCorners();
          }
          this._duck.alpha = Lerp.FloatSmooth(this._duck.alpha, 1f, 0.1f, 1.1f);
          if (this._state == ArcadeState.ViewChallenge || this._state == ArcadeState.UnlockScreen)
            this._followCam.manualViewSize = Lerp.FloatSmooth(this._followCam.manualViewSize, this._followCam.viewSize, 0.14f, 1.05f);
          Layer.Game.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          Layer.Background.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          this._hud.alpha = Lerp.Float(this._hud.alpha, 0.0f, 0.08f);
          this._unlockScreen.alpha = Lerp.Float(this._unlockScreen.alpha, 0.0f, 0.08f);
          if (((double) this._followCam.manualViewSize < 0.0 || (double) this._followCam.manualViewSize == (double) this._followCam.viewSize) && ((double) this._hud.alpha == 0.0 && (double) Layer.Game.fade == 1.0))
          {
            flag = true;
            this._followCam.manualViewSize = -1f;
            this._duck.alpha = 1f;
          }
        }
        else if (this._desiredState == ArcadeState.UnlockMachine)
        {
          if (!this._flipState)
          {
            this._followCam.Clear();
            this._followCam.Add((Thing) this._unlockMachines[0]);
            HUD.CloseAllCorners();
          }
          if (this._state == ArcadeState.ViewChallenge)
            this._followCam.manualViewSize = Lerp.FloatSmooth(this._followCam.manualViewSize, this._followCam.viewSize, 0.14f, 1.05f);
          this._duck.alpha = Lerp.FloatSmooth(this._duck.alpha, 1f, 0.1f, 1.1f);
          Layer.Game.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          Layer.Background.fade = Lerp.Float(Layer.Game.fade, 1f, 0.05f);
          this._hud.alpha = Lerp.Float(this._hud.alpha, 0.0f, 0.08f);
          this._unlockScreen.alpha = Lerp.Float(this._unlockScreen.alpha, 0.0f, 0.08f);
          this._unlockMachineWait = 1f;
          if (((double) this._followCam.manualViewSize < 0.0 || (double) this._followCam.manualViewSize == (double) this._followCam.viewSize) && ((double) this._hud.alpha == 0.0 && (double) Layer.Game.fade == 1.0))
          {
            flag = true;
            this._followCam.manualViewSize = -1f;
            this._duck.alpha = 1f;
          }
        }
        else if (this._desiredState == ArcadeState.LaunchChallenge)
        {
          if (!this._flipState)
            HUD.CloseAllCorners();
          Music.volume = Lerp.Float(Music.volume, 0.0f, 0.01f);
          this._hud.alpha = Lerp.Float(this._hud.alpha, 0.0f, 0.02f);
          this._unlockScreen.alpha = Lerp.Float(this._unlockScreen.alpha, 0.0f, 0.08f);
          if ((double) this._hud.alpha == 0.0)
            flag = true;
        }
        if (this._desiredState == ArcadeState.UnlockScreen)
        {
          this._duck.alpha = Lerp.FloatSmooth(this._duck.alpha, 0.0f, 0.1f);
          this._followCam.manualViewSize = Lerp.FloatSmooth(this._followCam.manualViewSize, 2f, 0.16f);
          if ((double) this._followCam.manualViewSize < 30.0)
          {
            Layer.Game.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.08f);
            Layer.Background.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.08f);
            this._unlockScreen.alpha = Lerp.Float(this._unlockScreen.alpha, 1f, 0.08f);
            if ((double) this._followCam.manualViewSize < 3.0 && (double) this._unlockScreen.alpha == 1.0 && (double) Layer.Game.fade == 0.0)
              flag = true;
          }
        }
        this._flipState = true;
        if (this._launchedChallenge)
        {
          Layer.Background.fade = 0.0f;
          Layer.Game.fade = 0.0f;
        }
        if (!flag)
          return;
        this._flipState = false;
        HUD.CloseAllCorners();
        this._state = this._desiredState;
        if (this._state == ArcadeState.ViewChallenge)
        {
          if (this._afterChallenge)
          {
            Music.Play("Arcade");
            this._afterChallenge = false;
          }
          this._hud.MakeActive();
          Level.Add((Thing) this._hud);
          this._duck.active = false;
        }
        else if (this._state == ArcadeState.LaunchChallenge)
        {
          ArcadeLevel.currentArcade = this;
          foreach (Thing thing in this.things[typeof (ChallengeConfetti)])
            Level.Remove(thing);
          Music.Stop();
          Level.current = (Level) new ChallengeLevel(this._hud.selected.challenge.fileName);
          if (!this.launchSpecialChallenge)
          {
            this._desiredState = ArcadeState.ViewChallenge;
            this._hud.launchChallenge = false;
            this._launchedChallenge = false;
            this._afterChallenge = true;
          }
          else
          {
            this._desiredState = ArcadeState.ViewSpecialChallenge;
            this._hud.launchChallenge = false;
            this._launchedChallenge = false;
            this._afterChallenge = true;
            this.launchSpecialChallenge = false;
          }
        }
        else
        {
          if (this._state == ArcadeState.UnlockMachine)
            return;
          if (this._state == ArcadeState.Normal)
          {
            this._unlockMachines.Clear();
            foreach (ArcadeMachine challenge in this._challenges)
            {
              if (challenge.CheckUnlocked())
                this._unlockMachines.Add(challenge);
            }
            if (this._unlockMachines.Count > 0)
            {
              this._desiredState = ArcadeState.UnlockMachine;
            }
            else
            {
              if (!this.basementWasUnlocked && Unlocks.IsUnlocked("BASEMENTKEY", Profiles.active[0]))
              {
                this.spawnKey = true;
                this.basementWasUnlocked = true;
              }
              this._duck.active = true;
            }
          }
          else if (this._state == ArcadeState.ViewSpecialChallenge)
          {
            this._duck.active = false;
            if (this._afterChallenge)
            {
              Music.Play("Arcade");
              this._afterChallenge = false;
              HUD.AddCornerCounter(HUDCorner.TopLeft, "@TICKET@ ", new FieldBinding((object) Profiles.active[0], "ticketCount"), animateCount: true);
              Chancy.afterChallenge = true;
              Chancy.afterChallengeWait = 1f;
            }
            else
            {
              HUD.AddCornerControl(HUDCorner.BottomLeft, "ACCEPT@SELECT@");
              HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@CANCEL");
              HUD.AddCornerCounter(HUDCorner.TopLeft, "@TICKET@ ", new FieldBinding((object) Profiles.active[0], "ticketCount"), animateCount: true);
            }
            this._duck.active = false;
          }
          else if (this._state == ArcadeState.ViewProfileSelector)
          {
            this._duck.active = false;
            ArcadeHatConsole arcadeHatConsole = (ArcadeHatConsole)Level.First<ArcadeHatConsole>();
            if (arcadeHatConsole == null)
              return;
            HUD.CloseAllCorners();
            arcadeHatConsole.Open();
          }
          else if (this._state == ArcadeState.ViewChallengeList)
          {
            this._duck.active = false;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "ACCEPT@SELECT@");
            HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@BACK");
          }
          else
          {
            if (this._state != ArcadeState.UnlockScreen)
              return;
            this.basementWasUnlocked = Unlocks.IsUnlocked("BASEMENTKEY", Profiles.active[0]);
            this._unlockScreen.MakeActive();
            this._duck.active = false;
          }
        }
      }
      else if (this._state == ArcadeState.Normal || this._state == ArcadeState.UnlockMachine)
      {
        Layer.Game.fade = Lerp.Float(Layer.Game.fade, 1f, 0.08f);
        Layer.Background.fade = Lerp.Float(Layer.Game.fade, 1f, 0.08f);
        this._hud.alpha = Lerp.Float(this._hud.alpha, 0.0f, 0.08f);
        if (this._state == ArcadeState.Normal)
        {
          object obj = (object) null;
          foreach (ArcadeMachine challenge in this._challenges)
          {
            double length = (double) (this._duck.position - challenge.position).length;
            if (challenge.hover)
            {
              obj = (object) challenge;
              if (Input.Pressed("SHOOT"))
              {
                this._hud.activeChallengeGroup = challenge.data;
                this._desiredState = ArcadeState.ViewChallenge;
                this._followCam.manualViewSize = this._followCam.viewSize;
                this._followCam.Clear();
                this._followCam.Add((Thing) challenge);
                HUD.CloseAllCorners();
                this._hoverMachine = (ArcadeMachine) null;
                this._hoverThing = (object) null;
                return;
              }
            }
            if (this._prizeTable.hover)
            {
              obj = (object) this._prizeTable;
              if (Input.Pressed("SHOOT"))
              {
                this._desiredState = ArcadeState.UnlockScreen;
                this._followCam.manualViewSize = this._followCam.viewSize;
                this._followCam.Clear();
                this._followCam.Add((Thing) this._prizeTable);
                HUD.CloseAllCorners();
                this._hoverMachine = (ArcadeMachine) null;
                this._hoverThing = (object) null;
                return;
              }
            }
          }
          if (Chancy.hover && Input.Pressed("SHOOT"))
          {
            this._desiredState = ArcadeState.ViewSpecialChallenge;
            HUD.CloseAllCorners();
            this._hoverMachine = (ArcadeMachine) null;
            this._hoverThing = (object) null;
            Chancy.hover = false;
            Chancy.lookingAtChallenge = true;
            Chancy.OpenChallengeView();
          }
          else
          {
            ArcadeHatConsole arcadeHatConsole = (ArcadeHatConsole)Level.First<ArcadeHatConsole>();
            if (arcadeHatConsole != null && Input.Pressed("SHOOT") && arcadeHatConsole.hover)
            {
              this._desiredState = ArcadeState.ViewProfileSelector;
              HUD.CloseAllCorners();
              this._hoverMachine = (ArcadeMachine) null;
              this._hoverThing = (object) null;
            }
            else
            {
              Chancy.hover = false;
              if (!Chancy.atCounter)
              {
                if ((double) (this._duck.position - Chancy.standingPosition).length < 22.0)
                {
                  obj = (object) Chancy.context;
                  Chancy.hover = true;
                }
                if ((double) Chancy.standingPosition.x < (double) Layer.Game.camera.left - 16.0 || (double) Chancy.standingPosition.x > (double) Layer.Game.camera.right + 16.0 || ((double) Chancy.standingPosition.y < (double) Layer.Game.camera.top - 16.0 || (double) Chancy.standingPosition.y > (double) Layer.Game.camera.bottom + 16.0))
                {
                  Chancy.atCounter = true;
                  Chancy.activeChallenge = (ChallengeData) null;
                }
              }
              else if (this._prizeTable.hoverChancyChallenge)
              {
                obj = (object) this._arcade;
                if (Input.Pressed("SHOOT"))
                {
                  this._desiredState = ArcadeState.ViewChallengeList;
                  HUD.CloseAllCorners();
                  Chancy.OpenChallengeList();
                  this._hoverMachine = (ArcadeMachine) null;
                  this._hoverThing = (object) null;
                  Chancy.hover = false;
                  Chancy.lookingAtList = true;
                  return;
                }
              }
              if (this._hoverThing == obj)
                return;
              HUD.CloseAllCorners();
              this._hoverThing = obj;
              this._hoverMachine = !(this._hoverThing is ArcadeMachine) ? (ArcadeMachine) null : obj as ArcadeMachine;
              if (this._hoverMachine != null)
              {
                HUD.AddCornerControl(HUDCorner.BottomLeft, "PLAY@SHOOT@");
                string text = this._hoverMachine.data.name + " ";
                foreach (string challenge1 in this._hoverMachine.data.challenges)
                {
                  ChallengeData challenge2 = Challenges.GetChallenge(challenge1);
                  if (challenge2 != null)
                  {
                    ChallengeSaveData saveData = Challenges.GetSaveData(challenge2.levelID, this._duck.profile);
                    if (saveData.trophy == TrophyType.Baseline)
                      text += "@BASELINE@";
                    else if (saveData.trophy == TrophyType.Bronze)
                      text += "@BRONZE@";
                    else if (saveData.trophy == TrophyType.Silver)
                      text += "@SILVER@";
                    else if (saveData.trophy == TrophyType.Gold)
                      text += "@GOLD@";
                    else if (saveData.trophy == TrophyType.Platinum)
                      text += "@PLATINUM@";
                    else if (saveData.trophy == TrophyType.Developer)
                      text += "@DEVELOPER@";
                  }
                }
                HUD.AddCornerMessage(HUDCorner.TopLeft, text);
              }
              else if (this._prizeTable.hover)
              {
                if (this._prizeTable.hoverChancyChallenge)
                {
                  HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@VIEW CHALLENGES");
                }
                else
                {
                  HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@SPEND TICKETS");
                  HUD.AddCornerCounter(HUDCorner.BottomLeft, "@TICKET@ ", new FieldBinding((object) Profiles.active[0], "ticketCount"), animateCount: true);
                }
              }
              else
              {
                switch (obj)
                {
                  case ArcadeMode _:
                    if (!this._prizeTable.hoverChancyChallenge)
                      break;
                    HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@VIEW CHALLENGES");
                    break;
                  case Chancy _:
                    HUD.AddCornerControl(HUDCorner.BottomLeft, "CHANCY@SHOOT@");
                    break;
                }
              }
            }
          }
        }
        else
        {
          if (this._state != ArcadeState.UnlockMachine)
            return;
          this._unlockMachineWait -= 0.02f;
          Layer.Lighting2.targetFade = Lerp.Float(Layer.Lighting2.targetFade, 0.5f, 0.01f);
          if ((double) this._unlockMachineWait >= 0.0)
            return;
          if (this._unlockingMachine)
          {
            this._unlockingMachine = false;
            this._followCam.Clear();
            this._followCam.Add((Thing) this._unlockMachines[0]);
            this._unlockMachineWait = 1f;
          }
          else if (this._unlockMachines.Count > 0)
          {
            this._unlockMachines[0].unlocked = true;
            this._unlockMachines.RemoveAt(0);
            this._unlockingMachine = this._unlockMachines.Count > 0;
            SFX.Play("lightTurnOn", pitch: Rando.Float(-0.1f, 0.1f));
            this._unlockMachineWait = 1f;
            Layer.Lighting2.targetFade = 1f;
          }
          else
            this._desiredState = ArcadeState.Normal;
        }
      }
      else if (this._state == ArcadeState.ViewChallenge)
      {
        Graphics.fade = Lerp.Float(Graphics.fade, 1f, 0.05f);
        Layer.Game.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.05f);
        Layer.Background.fade = Lerp.Float(Layer.Game.fade, 0.0f, 0.05f);
        this._hud.alpha = Lerp.Float(this._hud.alpha, 1f, 0.05f);
        if (!this._hud.quitOut)
          return;
        this._hud.quitOut = false;
        this._desiredState = ArcadeState.Normal;
        if (Chancy.activeChallenge != null)
          return;
        List<ChallengeData> chancyChallenges = Challenges.GetEligibleIncompleteChancyChallenges(Profiles.active[0]);
        if (chancyChallenges.Count <= 0)
          return;
        Vec2 position = this._duck.position;
        ArcadeMachine arcadeMachine = Level.Nearest<ArcadeMachine>(this._duck.x, this._duck.y);
        if (arcadeMachine != null)
          position = arcadeMachine.position;
        chancyChallenges.OrderBy<ChallengeData, int>((Func<ChallengeData, int>) (v => v.GetRequirementValue()));
        Chancy.AddProposition(chancyChallenges[chancyChallenges.Count - 1], position);
      }
      else if (this._state == ArcadeState.UnlockScreen)
      {
        if (!this._unlockScreen.quitOut)
          return;
        this._unlockScreen.quitOut = false;
        this._desiredState = ArcadeState.Normal;
      }
      else if (this._state == ArcadeState.ViewSpecialChallenge)
      {
        if (!this.launchSpecialChallenge)
        {
          Graphics.fade = Lerp.Float(Graphics.fade, 1f, 0.05f);
          if (Input.Pressed("QUACK"))
          {
            if (this.returnToChallengeList)
            {
              this._desiredState = ArcadeState.ViewChallengeList;
              Chancy.hover = false;
              Chancy.lookingAtList = true;
            }
            else
              this._desiredState = ArcadeState.Normal;
            Chancy.lookingAtChallenge = false;
            HUD.CloseAllCorners();
            SFX.Play("consoleCancel");
          }
          else
          {
            if (!Input.Pressed("SELECT"))
              return;
            this.launchSpecialChallenge = true;
            SFX.Play("consoleSelect");
          }
        }
        else
        {
          Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.05f);
          if ((double) Graphics.fade >= 0.00999999977648258)
            return;
          this._hud.launchChallenge = true;
          this._hud.selected = new ChallengeCard(0.0f, 0.0f, Chancy.activeChallenge);
          HUD.CloseAllCorners();
        }
      }
      else if (this._state == ArcadeState.ViewChallengeList)
      {
        Graphics.fade = Lerp.Float(Graphics.fade, 1f, 0.05f);
        if (Input.Pressed("QUACK"))
        {
          this._desiredState = ArcadeState.Normal;
          Chancy.lookingAtChallenge = false;
          Chancy.lookingAtList = false;
          HUD.CloseAllCorners();
          SFX.Play("consoleCancel");
        }
        else
        {
          if (!Input.Pressed("SELECT"))
            return;
          Chancy.AddProposition(Chancy.selectedChallenge, Chancy.standingPosition);
          this.returnToChallengeList = true;
          this._desiredState = ArcadeState.ViewSpecialChallenge;
          HUD.CloseAllCorners();
          this._hoverMachine = (ArcadeMachine) null;
          this._hoverThing = (object) null;
          Chancy.hover = false;
          Chancy.lookingAtChallenge = true;
          Chancy.lookingAtList = false;
          Chancy.OpenChallengeView();
        }
      }
      else
      {
        if (this._state != ArcadeState.ViewProfileSelector)
          return;
        Graphics.fade = Lerp.Float(Graphics.fade, 1f, 0.05f);
        ArcadeHatConsole arcadeHatConsole = (ArcadeHatConsole)Level.First<ArcadeHatConsole>();
        if (arcadeHatConsole == null || arcadeHatConsole.IsOpen())
          return;
        foreach (ArcadeMachine challenge in this._challenges)
          challenge.unlocked = challenge.CheckUnlocked(false);
        this._unlockMachines.Clear();
        this.UpdateDefault();
        this._desiredState = ArcadeState.Normal;
      }
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.HUD)
        Chancy.Draw();
      if (layer == Layer.Game)
        Chancy.DrawGameLayer();
      base.PostDrawLayer(layer);
    }
  }
}
