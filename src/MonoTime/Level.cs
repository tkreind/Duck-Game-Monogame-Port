// Decompiled with JetBrains decompiler
// Type: DuckGame.Level
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DuckGame
{
  public class Level
  {
    public bool isCustomLevel;
    public static bool flipH = false;
    public static bool symmetry = false;
    public static bool leftSymmetry = true;
    public static bool loadingOppositeSymmetry = false;
    protected string _level = "";
    private static LevelCore _core = new LevelCore();
    public static bool skipInitialize = false;
    public bool isPreview;
    private static Queue<List<object>> _collisionLists = new Queue<List<object>>();
    private static List<float> _chanceGroups = new List<float>()
    {
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f
    };
    private static List<float> _chanceGroups2 = new List<float>()
    {
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f
    };
    private bool _simulatePhysics = true;
    private Color _backgroundColor = Color.Black;
    protected QuadTreeObjectList _things = new QuadTreeObjectList();
    protected string _id = "";
    protected Camera _camera = new Camera();
    public NetDebugInterface _netDebug = new NetDebugInterface()
    {
      position = new Vec2(10f, 10f)
    };
    protected NetLevelStatus _networkStatus;
    public float transitionSpeedMultiplier = 1f;
    public float lowestPoint = 500f;
    private bool _lowestPointInitialized;
    protected bool _initialized;
    private bool _levelStart;
    protected bool _startCalled;
    protected bool _centeredView;
    private bool _waitingOnNewData;
    public byte networkIndex;
    private bool _notifiedReady;
    private bool _initializeLater;
    public bool bareInitialize;
    protected Vec2 _topLeft = new Vec2(99999f, 99999f);
    protected Vec2 _bottomRight = new Vec2(-99999f, -99999f);
    protected bool _readyForTransition = true;
    public bool _waitingOnTransition;
    protected bool _sentLevelChange;
    protected int _updateWaitFrames;
    private bool _calledAllClientsReady;
    private bool _aiInitialized;
    private bool _refreshState;
    private bool initPaths;
    public float flashDissipationSpeed = 0.15f;
    public bool skipCurrentLevelReset;
    public bool skipTest;
    private bool _clearScreen = true;
    private Sprite _burnGlow;
    private Sprite _burnGlowWide;
    private Sprite _burnGlowWideLeft;
    private Sprite _burnGlowWideRight;

    public string level => this._level;

    public static LevelCore core
    {
      get => Level._core;
      set => Level._core = value;
    }

    public static void InitializeCollisionLists()
    {
      MonoMain.loadMessage = "Loading Collision Lists";
      for (int index = 0; index < 10; ++index)
        Level._collisionLists.Enqueue(new List<object>());
    }

    public static List<object> GetNextCollisionList() => new List<object>();

    public static bool PassedChanceGroup(int group, float val) => group == -1 ? (double) Rando.Float(1f) < (double) val : (double) Level._chanceGroups[group] < (double) val;

    public static bool PassedChanceGroup2(int group, float val) => group == -1 ? (double) Rando.Float(1f) < (double) val : (double) Level._chanceGroups2[group] < (double) val;

    public static float GetChanceGroup2(int group) => group == -1 ? Rando.Float(1f) : Level._chanceGroups2[group];

    public bool simulatePhysics
    {
      get => this._simulatePhysics;
      set => this._simulatePhysics = value;
    }

    public static bool sendCustomLevels
    {
      get => Level._core.sendCustomLevels;
      set => Level._core.sendCustomLevels = value;
    }

    public static Level current
    {
      get => Level._core.nextLevel == null ? Level._core.currentLevel : Level._core.nextLevel;
      set
      {
        if (Level._core.nextLevel != value && Level._core.currentLevel != value && Level._core.currentLevel != null)
          Level._core.currentLevel._sentLevelChange = false;
        Level._core.nextLevel = value;
      }
    }

    public static Level activeLevel
    {
      get => Level._core.currentLevel;
      set => Level._core.currentLevel = value;
    }

    public Color backgroundColor
    {
      get => this._backgroundColor;
      set => this._backgroundColor = value;
    }

    public static void Add(Thing thing)
    {
      if (Level._core.currentLevel == null)
        return;
      Level._core.currentLevel.AddThing(thing);
    }

    public static void Remove(Thing thing)
    {
      if (Level._core.currentLevel == null)
        return;
      Level._core.currentLevel.RemoveThing(thing);
    }

    public static void ClearThings()
    {
      if (Level._core.currentLevel == null)
        return;
      Level._core.currentLevel.Clear();
    }

    public static void UpdateCurrentLevel()
    {
      if (Level._core.currentLevel == null)
        return;
      Level._core.currentLevel.DoUpdate();
    }

    public static void DrawCurrentLevel()
    {
      if (Level._core.currentLevel == null)
        return;
      Level._core.currentLevel.DoDraw();
    }

    public static Thing First<T>()
    {
      IEnumerable<Thing> thing = Level.current.things[typeof (T)];
      return thing.Count<Thing>() > 0 ? thing.First<Thing>() : default (Thing);
    }

    public Thing FirstOfType<T>()
    {
      IEnumerable<Thing> thing = this.things[typeof (T)];
      return thing.Count<Thing>() > 0 ? thing.First<Thing>() : default (Thing);
    }

    public QuadTreeObjectList things => this._things;

    public string id => this._id;

    public Camera camera
    {
      get => this._camera;
      set => this._camera = value;
    }

    public NetLevelStatus networkStatus => this._networkStatus;

    public bool initialized => this._initialized;

    public bool waitingOnNewData
    {
      get => this._waitingOnNewData;
      set => this._waitingOnNewData = value;
    }

    public virtual void DoInitialize()
    {
      if (this.waitingOnNewData)
      {
        this._initializeLater = true;
        this._initialized = true;
      }
      else if (!this._initialized)
      {
        this.Initialize();
        if (!Network.isActive || Level.current is TeamSelect2)
          this.DoStart();
        this._things.RefreshState();
        this.CalculateBounds();
        this._initialized = true;
        if (this._centeredView)
          this.camera.centerY -= (float) (((double) (DuckGame.Graphics.aspect * this.camera.width) - (double) (9f / 16f * this.camera.width)) / 2.0);
        if (VirtualTransition.active)
          return;
        StaticRenderer.Update();
      }
      else
      {
        foreach (Thing thing in this._things)
          thing.AddToLayer();
      }
    }

    public virtual void LevelLoaded()
    {
    }

    public virtual void Initialize()
    {
      this._levelStart = true;
      Vote.ClearVotes();
    }

    private void DoStart()
    {
      if (this._startCalled)
        return;
      this.Start();
      this._startCalled = true;
    }

    public void SkipStart() => this._startCalled = true;

    public virtual void Start()
    {
    }

    public virtual void Terminate() => this.Clear();

    public void AddThing(Thing t)
    {
      if (Thread.CurrentThread == Content.previewThread && this != Content.previewLevel)
        Content.previewLevel.AddThing(t);
      else if (t is ThingContainer)
      {
        foreach (Thing thing in (t as ThingContainer).things)
          this.AddThing(thing);
      }
      else
      {
        if (t.level == this)
          return;
        this._things.Add(t);
        if (Level.skipInitialize)
          return;
        t.Added(this, !this.bareInitialize, false);
      }
    }

    public void RemoveThing(Thing t)
    {
      if (t == null)
        return;
      t.DoTerminate();
      t.Removed();
      this._things.Remove(t);
    }

    public void Clear()
    {
      foreach (Thing thing in this._things)
        thing.Removed();
      Layer.ClearLayers();
      this._things.Clear();
    }

    public Vec2 topLeft => this._topLeft;

    public Vec2 bottomRight => this._bottomRight;

    public static void InitChanceGroups()
    {
      for (int index = 0; index < Level._chanceGroups.Count; ++index)
        Level._chanceGroups[index] = Rando.Float(1f);
      for (int index = 0; index < Level._chanceGroups2.Count; ++index)
        Level._chanceGroups2[index] = Rando.Float(1f);
    }

    public static void UpdateLevelChange()
    {
      if (Level._core.nextLevel != null)
      {
        if (Level._core.currentLevel is IHaveAVirtualTransition && Level._core.nextLevel is IHaveAVirtualTransition && !(Level._core.nextLevel is TeamSelect2))
          VirtualTransition.GoVirtual();
        if (Network.isActive && Level.activeLevel != null && !Level.activeLevel._sentLevelChange)
        {
          DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Performing level swap (" + (object) DuckNetwork.levelIndex + ")");
          GhostManager.context.Clear();
          foreach (Profile profile in Profiles.active)
          {
            if (profile.connection != null)
              profile.connection.manager.Reset();
          }
          if (Level._core.currentLevel is TeamSelect2 && !(Level._core.nextLevel is TeamSelect2))
            DuckNetwork.ClosePauseMenu();
          if (!(Level._core.currentLevel is TeamSelect2) && Level._core.nextLevel is TeamSelect2)
            DuckNetwork.ClosePauseMenu();
          if (Network.isServer)
          {
            ++DuckNetwork.levelIndex;
            DuckNetwork.compressedLevelData = (MemoryStream) null;
            DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Incrementing level index (" + (object) ((int) DuckNetwork.levelIndex - 1) + "->" + (object) DuckNetwork.levelIndex + ")");
            uint varChecksum = 0;
            bool varNeedsChecksum = false;
            string lev = "";
            if (!(Level._core.currentLevel is TeamSelect2) && Level._core.nextLevel is TeamSelect2)
              lev = "@TEAMSELECT";
            else if (Level._core.nextLevel is GameLevel)
            {
              GameLevel nextLevel = Level._core.nextLevel as GameLevel;
              if (nextLevel.customLevel)
              {
                varNeedsChecksum = true;
                varChecksum = nextLevel.checksum;
                DuckNetwork.compressedLevelData = new MemoryStream(nextLevel.compressedData, 0, nextLevel.compressedData.Length, false, true);
              }
              lev = nextLevel.level;
            }
            else if (Level._core.nextLevel is RockIntro)
              lev = "@ROCKINTRO";
            else if (Level._core.nextLevel is RockScoreboard)
            {
              GhostManager.context.SetGhostIndex((NetIndex16) 32);
              lev = (Level._core.nextLevel as RockScoreboard).mode != ScoreBoardMode.ShowScores ? (!(Level._core.nextLevel as RockScoreboard).afterHighlights ? "@ROCKTHROW|SHOWWINNER" : "@ROCKTHROW|SHOWEND") : "@ROCKTHROW|SHOWSCORE";
            }
            if (lev != "")
            {
              foreach (Profile profile in DuckNetwork.profiles)
              {
                if (profile.connection != null)
                {
                  profile.connection.manager.ClearAllMessages();
                  if (Level._core.nextLevel is GameLevel && (Level._core.nextLevel as GameLevel).level == "RANDOM")
                    Send.Message((NetMessage) new NMSwitchLevelRandom(lev, DuckNetwork.levelIndex, (ushort) (int) GhostManager.context.currentGhostIndex, (Level._core.nextLevel as GameLevel).seed), NetMessagePriority.ReliableOrdered, profile.connection);
                  else
                    Send.Message((NetMessage) new NMSwitchLevel(lev, DuckNetwork.levelIndex, (ushort) (int) GhostManager.context.currentGhostIndex, varNeedsChecksum, varChecksum), NetMessagePriority.ReliableOrdered, profile.connection);
                }
              }
            }
          }
          Level.activeLevel._sentLevelChange = true;
        }
        if (!VirtualTransition.active)
        {
          Level.InitChanceGroups();
          DamageManager.ClearHits();
          Layer.ResetLayers();
          HUD.ClearCorners();
          if (Level._core.currentLevel != null)
            Level._core.currentLevel.Terminate();
          if (Network.isActive && GhostManager.context != null)
            GhostManager.context.particleManager.ChangeLevels();
          Level._core.currentLevel = Level._core.nextLevel;
          Level._core.nextLevel = (Level) null;
          Layer.lighting = false;
          AutoUpdatables.ClearSounds();
          SequenceItem.sequenceItems.Clear();
          GC.Collect(1, GCCollectionMode.Optimized);
          foreach (Profile profile in Profiles.active)
            profile.duck = (Duck) null;
          SFX.StopAllSounds();
          Level._core.currentLevel.DoInitialize();
          if (MonoMain.pauseMenu != null && MonoMain.pauseMenu.inWorld)
            Level._core.currentLevel.AddThing((Thing) MonoMain.pauseMenu);
          if (Network.isActive && DuckNetwork.duckNetUIGroup != null && DuckNetwork.duckNetUIGroup.open)
            Level._core.currentLevel.AddThing((Thing) DuckNetwork.duckNetUIGroup);
          if (Recorder.globalRecording != null)
            Recorder.globalRecording.UpdateAtlasFile();
          Level.current._networkStatus = NetLevelStatus.WaitingForDataTransfer;
          if (!(Level._core.currentLevel is IOnlyTransitionIn) && Level._core.currentLevel is IHaveAVirtualTransition && (!(Level._core.currentLevel is TeamSelect2) && VirtualTransition.isVirtual))
          {
            if (Level.current._readyForTransition)
            {
              VirtualTransition.GoUnVirtual();
              DuckGame.Graphics.fade = 1f;
            }
            else
            {
              Level.current._waitingOnTransition = true;
              if (Network.isActive)
                ConnectionStatusUI.Show();
            }
          }
        }
      }
      if (!Level.current._waitingOnTransition || !Level.current._readyForTransition)
        return;
      Level.current._waitingOnTransition = false;
      VirtualTransition.GoUnVirtual();
      if (!Network.isActive)
        return;
      ConnectionStatusUI.Hide();
    }

    public virtual void OnMessage(NetMessage message)
    {
    }

    public virtual void OnNetworkConnecting(Profile p)
    {
    }

    public virtual void OnNetworkConnected(Profile p)
    {
    }

    public virtual void OnSessionEnded(DuckNetErrorInfo error)
    {
      if (error != null)
        Level.current = (Level) new ConnectionError(error.message);
      else
        Level.current = (Level) new ConnectionError("|RED|Disconnected from game.");
    }

    public virtual void OnDisconnect(NetworkConnection n)
    {
    }

    public bool calledAllClientsReady => this._calledAllClientsReady;

    public virtual void DoAllClientsReady()
    {
      if (this._calledAllClientsReady)
        return;
      this._calledAllClientsReady = true;
      this.OnAllClientsReady();
    }

    protected virtual void OnAllClientsReady()
    {
      this._networkStatus = NetLevelStatus.Ready;
      Level.current._readyForTransition = true;
      this.DoStart();
    }

    public void TransferComplete(NetworkConnection c)
    {
      if (!this._initialized)
        this.Initialize();
      this._networkStatus = NetLevelStatus.WaitingForTransition;
      this.OnTransferComplete(c);
    }

    protected virtual void OnTransferComplete(NetworkConnection c)
    {
    }

    public virtual void SendLevelData(NetworkConnection c)
    {
    }

    public void IgnoreLowestPoint()
    {
      this._lowestPointInitialized = true;
      this.lowestPoint = 999999f;
      this._topLeft = new Vec2(-99999f, -99999f);
      this._bottomRight = new Vec2(99999f, 99999f);
    }

    public void CalculateBounds()
    {
      this._lowestPointInitialized = true;
      this.lowestPoint = 0.0f;
      CameraBounds cameraBounds = (CameraBounds)this.FirstOfType<CameraBounds>();
      if (cameraBounds != null)
      {
        this._topLeft = new Vec2(cameraBounds.x - (float) ((int) cameraBounds.wide / 2), cameraBounds.y - (float) ((int) cameraBounds.high / 2));
        this._bottomRight = new Vec2(cameraBounds.x + (float) ((int) cameraBounds.wide / 2), cameraBounds.y + (float) ((int) cameraBounds.high / 2));
        this.lowestPoint = this._bottomRight.y;
      }
      else
      {
        foreach (Block block in this._things[typeof (Block)])
        {
          if (!(block is RockWall))
          {
            if ((double) block.right > (double) this._bottomRight.x)
              this._bottomRight.x = block.right;
            if ((double) block.left < (double) this._topLeft.x)
              this._topLeft.x = block.left;
            if ((double) block.bottom > (double) this._bottomRight.y)
              this._bottomRight.y = block.bottom;
            if ((double) block.top < (double) this._topLeft.y)
              this._topLeft.y = block.top;
            if ((double) block.bottom > (double) this.lowestPoint)
              this.lowestPoint = block.bottom;
          }
        }
        foreach (AutoPlatform autoPlatform in this._things[typeof (AutoPlatform)])
        {
          if ((double) autoPlatform.right > (double) this._bottomRight.x)
            this._bottomRight.x = autoPlatform.right;
          if ((double) autoPlatform.left < (double) this._topLeft.x)
            this._topLeft.x = autoPlatform.left;
          if ((double) autoPlatform.bottom > (double) this._bottomRight.y)
            this._bottomRight.y = autoPlatform.bottom;
          if ((double) autoPlatform.top < (double) this._topLeft.y)
            this._topLeft.y = autoPlatform.top;
          if ((double) autoPlatform.bottom > (double) this.lowestPoint)
            this.lowestPoint = autoPlatform.bottom;
        }
      }
    }

    public virtual void DoUpdate()
    {
      if (this._updateWaitFrames > 0)
      {
        if (!this._refreshState)
        {
          this._things.RefreshState();
          VirtualTransition.Update();
          this._refreshState = true;
        }
        --this._updateWaitFrames;
        if (this._lowestPointInitialized)
          return;
        this.CalculateBounds();
      }
      else
      {
        Level currentLevel = Level._core.currentLevel;
        Level._core.currentLevel = this;
        if ((double) DuckGame.Graphics.flashAdd > 0.0)
          DuckGame.Graphics.flashAdd -= this.flashDissipationSpeed;
        else
          DuckGame.Graphics.flashAdd = 0.0f;
        if (this._levelStart)
        {
          DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 1f, 0.05f);
          if ((double) DuckGame.Graphics.fade == 1.0)
            this._levelStart = false;
        }
        if (Network.isActive && DevConsole.enableNetworkDebugging && !NetworkDebugger.enabled)
        {
          MonoMain.instance.IsMouseVisible = true;
          this._netDebug.Update(Network.activeNetwork);
        }
        if (Network.isActive)
        {
          foreach (NetworkConnection connection in Network.connections)
            connection.manager.UpdateSynchronizedEvents();
        }
        if (Level._core.nextLevel == null && (!Network.isActive || this._startCalled) && !this._waitingOnTransition)
        {
          if (this._camera != null)
            this._camera.Update();
          this.Update();
          Layer.UpdateLayers();
          this.UpdateThings();
          this._things.UpdateIslands();
          this._things.RefreshState();
          Vote.Update();
          HUD.Update();
          if (Network.isActive)
            ConnectionIndicator.Update();
        }
        if (!this._notifiedReady && this._initialized && !this.waitingOnNewData)
        {
          DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Initializing level (" + (object) DuckNetwork.levelIndex + ")");
          if (Network.isActive && Network.isClient)
          {
            switch (this)
            {
              case GameLevel _:
              case TeamSelect2 _:
              case RockScoreboard _:
              case RockIntro _:
                Send.Message((NetMessage) new NMClientLoadedLevel(DuckNetwork.levelIndex), NetMessagePriority.ReliableOrdered);
                break;
            }
          }
          if (this._initializeLater)
          {
            this._initialized = false;
            this._initializeLater = false;
            this.DoInitialize();
          }
          this._notifiedReady = true;
        }
        VirtualTransition.Update();
        ConnectionStatusUI.Update();
        if (!this._aiInitialized)
        {
          AI.InitializeLevelPaths();
          this._aiInitialized = true;
        }
        if (this.skipCurrentLevelReset)
          return;
        Level._core.currentLevel = currentLevel;
      }
    }

    public virtual void NetworkDebuggerPrepare()
    {
    }

    public virtual void UpdateThings()
    {
      Network.PostDraw();
      if (Network.isActive)
      {
        foreach (Thing update in this._things.updateList)
        {
          if (update.active && (update.ghostObject == null || update.connection == null || update.connection == DuckNetwork.localConnection && update.ghostObject != null && update.ghostObject.IsInitialized() || this.skipTest) && update.level != null)
            update.DoUpdate();
          if (Level._core.nextLevel != null)
            break;
        }
      }
      else
      {
        foreach (Thing update in this._things.updateList)
        {
          if (update.active && update.level != null)
            update.DoUpdate();
          if (Level._core.nextLevel != null)
            break;
        }
      }
    }

    public virtual void Update()
    {
    }

    public bool clearScreen
    {
      get => this._clearScreen;
      set => this._clearScreen = value;
    }

    public virtual void StartDrawing()
    {
    }

    public virtual void DoDraw()
    {
      this.StartDrawing();
      Layer.DrawTargetLayers();
      Vec3 vec = this.backgroundColor.ToVector3() * DuckGame.Graphics.fade;
      vec.x += DuckGame.Graphics.flashAdd;
      vec.y += DuckGame.Graphics.flashAdd;
      vec.z += DuckGame.Graphics.flashAdd;
      vec = new Vec3(vec.x + DuckGame.Graphics.fadeAdd, vec.y + DuckGame.Graphics.fadeAdd, vec.z + DuckGame.Graphics.fadeAdd);
      Color c = new Color(vec);
      c.a = this.backgroundColor.a;
      if (this.clearScreen)
        DuckGame.Graphics.Clear(c);
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogBackgroundColor(this.backgroundColor);
      if (Recorder.globalRecording != null)
        Recorder.globalRecording.LogBackgroundColor(this.backgroundColor);
      this.BeforeDraw();
      DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, this.camera.getMatrix());
      this.Draw();
      this.things.Draw();
      DuckGame.Graphics.screen.End();
      if (Input.Pressed("GRAB"))
        Layer.ignoreTransparent = !Layer.ignoreTransparent;
      if (DevConsole.splitScreen && this is GameLevel)
        SplitScreen.Draw();
      else
        Layer.DrawLayers();
      if (DevConsole.rhythmMode && this is GameLevel)
      {
        DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Layer.HUD.camera.getMatrix());
        RhythmMode.Draw();
        DuckGame.Graphics.screen.End();
      }
      this.AfterDrawLayers();
    }

    public virtual void InitializeDraw(Layer l)
    {
      if (l != Layer.HUD || !this._centeredView)
        return;
      float num = (float) ((double) DuckGame.Graphics.width * (double) DuckGame.Graphics.aspect - (double) DuckGame.Graphics.width * (9.0 / 16.0));
      if ((double) num <= 0.0)
        return;
      DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect) null, Matrix.Identity);
      DuckGame.Graphics.DrawRect(Vec2.Zero, new Vec2((float) DuckGame.Graphics.width, num / 2f), Color.Black, new Depth(0.9f));
      DuckGame.Graphics.DrawRect(new Vec2(0.0f, (float) DuckGame.Graphics.height - num / 2f), new Vec2((float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height), Color.Black, new Depth(0.9f));
      DuckGame.Graphics.screen.End();
    }

    public virtual void BeforeDraw()
    {
    }

    public virtual void AfterDrawLayers()
    {
    }

    public virtual void Draw()
    {
    }

    public virtual void PreDrawLayer(Layer layer)
    {
    }

    public virtual void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.Console)
      {
        DevConsole.Draw();
        if (!NetworkDebugger.enabled)
          this._netDebug.Draw(Network.activeNetwork);
        if (!Network.isActive)
          return;
        DuckNetwork.Draw();
      }
      else if (layer == Layer.HUD)
      {
        Vote.Draw();
        HUD.Draw();
        ConnectionStatusUI.Draw();
      }
      else
      {
        if (layer == Layer.Lighting)
          return;
        if (layer == Layer.Glow && Options.Data.fireGlow)
        {
          foreach (Teleporter teleporter in this.things[typeof (Teleporter)])
            teleporter.DrawWarpLines();
          foreach (MaterialThing materialThing in this.things[typeof (MaterialThing)])
          {
            if (materialThing is Gun && (double) materialThing.heat > 0.300000011920929)
            {
              Gun gun = materialThing as Gun;
              if (this._burnGlow == null)
              {
                this._burnGlow = new Sprite("redHotGlow");
                this._burnGlow.CenterOrigin();
              }
              this._burnGlow.alpha = (float) ((double) Math.Min(materialThing.heat, 1f) / 1.0 - 0.200000002980232);
              DuckGame.Graphics.Draw(this._burnGlow, materialThing.x + gun.handOffset.x, materialThing.y + gun.handOffset.y);
            }
            materialThing.DrawGlow();
          }
          foreach (SmallFire smallFire in this.things[typeof (SmallFire)])
          {
            if (this._burnGlow == null)
            {
              this._burnGlow = new Sprite("redGlow");
              this._burnGlow.CenterOrigin();
            }
            this._burnGlow.alpha = 0.65f * smallFire.alpha;
            DuckGame.Graphics.Draw(this._burnGlow, smallFire.x, smallFire.y - 4f);
          }
          foreach (FluidPuddle fluidPuddle in this.things[typeof (FluidPuddle)])
          {
            if ((fluidPuddle.onFire || (double) fluidPuddle.data.heat > 0.5) && (double) fluidPuddle.alpha > 0.5)
            {
              float num1 = fluidPuddle.right - fluidPuddle.left;
              float num2 = 16f;
              Math.Sin((double) fluidPuddle.fluidWave);
              if (this._burnGlowWide == null)
              {
                this._burnGlowWide = new Sprite("redGlowWideSharp");
                this._burnGlowWide.CenterOrigin();
                this._burnGlowWide.alpha = 0.75f;
                this._burnGlowWideLeft = new Sprite("redGlowWideLeft");
                this._burnGlowWideLeft.center = new Vec2((float) this._burnGlowWideLeft.width, (float) (this._burnGlowWideLeft.height / 2));
                this._burnGlowWideLeft.alpha = 0.75f;
                this._burnGlowWideRight = new Sprite("redGlowWideRight");
                this._burnGlowWideRight.center = new Vec2(0.0f, (float) (this._burnGlowWideRight.height / 2));
                this._burnGlowWideRight.alpha = 0.75f;
              }
              int num3 = (int) Math.Floor((double) num1 / (double) num2);
              if ((double) fluidPuddle.collisionSize.y > 8.0)
              {
                this._burnGlowWide.xscale = 16f;
                for (int index = 0; index < num3; ++index)
                {
                  float x = (float) ((double) fluidPuddle.bottomLeft.x + (double) index * (double) num2 + 11.0 - 8.0);
                  float y = fluidPuddle.top - 1f + (float) Math.Sin((double) fluidPuddle.fluidWave + (double) index * 0.699999988079071);
                  DuckGame.Graphics.Draw(this._burnGlowWide, x, y);
                  if (index == 0)
                    DuckGame.Graphics.Draw(this._burnGlowWideLeft, x, y);
                  else if (index == num3 - 1)
                    DuckGame.Graphics.Draw(this._burnGlowWideRight, x + 16f, y);
                }
              }
              else
              {
                DuckGame.Graphics.doSnap = false;
                this._burnGlowWide.xscale = fluidPuddle.collisionSize.x;
                DuckGame.Graphics.Draw(this._burnGlowWide, fluidPuddle.left, fluidPuddle.bottom - 2f);
                DuckGame.Graphics.Draw(this._burnGlowWideLeft, fluidPuddle.left, fluidPuddle.bottom - 2f);
                DuckGame.Graphics.Draw(this._burnGlowWideRight, fluidPuddle.right, fluidPuddle.bottom - 2f);
                DuckGame.Graphics.doSnap = true;
              }
            }
          }
        }
        else if (layer == Layer.Virtual)
        {
          VirtualTransition.Draw();
        }
        else
        {
          if (layer != Layer.Game || !Network.isActive || VirtualTransition.active)
            return;
          ConnectionIndicator.Draw();
        }
      }
    }

    public static T Nearest<T>(float x, float y, Thing ignore = null, Layer layer = null) => Level.current.NearestThing<T>(new Vec2(x, y), ignore, layer: layer);

    public static T Nearest<T>(Vec2 point, Thing ignore = null, int nearIndex = 0, Layer layer = null) => Level.current.NearestThing<T>(point, ignore, nearIndex, layer);

    public static T CheckCircle<T>(float p1x, float p1y, float radius, Thing ignore = null) => Level.current.CollisionCircle<T>(new Vec2(p1x, p1y), radius, ignore);

    public static T CheckCircle<T>(Vec2 p1, float radius, Thing ignore = null) => Level.current.CollisionCircle<T>(p1, radius, ignore);

    public static IEnumerable<T> CheckCircleAll<T>(Vec2 p1, float radius) => Level.current.CollisionCircleAll<T>(p1, radius);

    public T CollisionCircle<T>(float p1x, float p1y, float radius, Thing ignore = null) => this.CollisionCircle<T>(new Vec2(p1x, p1y), radius, ignore);

    public static T CheckRect<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore = null) => Level.current.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

    public static T CheckRect<T>(Vec2 p1, Vec2 p2, Thing ignore = null) => Level.current.CollisionRect<T>(p1, p2, ignore);

    public static IEnumerable<T> CheckRectAll<T>(Vec2 p1, Vec2 p2) => Level.current.CollisionRectAll<T>(p1, p2);

    public T CollisionRect<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore = null) => this.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

    public static T CheckLine<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore = null) => Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

    public static T CheckLine<T>(
      float p1x,
      float p1y,
      float p2x,
      float p2y,
      out Vec2 position,
      Thing ignore = null)
    {
      return Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), out position, ignore);
    }

    public static T CheckLine<T>(Vec2 p1, Vec2 p2, Thing ignore = null) => Level.current.CollisionLine<T>(p1, p2, ignore);

    public static T CheckLine<T>(Vec2 p1, Vec2 p2, out Vec2 position, Thing ignore = null) => Level.current.CollisionLine<T>(p1, p2, out position, ignore);

    public T CollisionLine<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore = null) => this.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

    public static IEnumerable<T> CheckLineAll<T>(Vec2 p1, Vec2 p2) => Level.current.CollisionLineAll<T>(p1, p2);

    public IEnumerable<T> CheckLineAll<T>(float p1x, float p1y, float p2x, float p2y) => this.CollisionLineAll<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

    public static T CheckPoint<T>(float x, float y, Thing ignore = null, Layer layer = null) => Level.current.CollisionPoint<T>(new Vec2(x, y), ignore, layer);

    public static T CheckPointPlacementLayer<T>(float x, float y, Thing ignore = null, Layer layer = null) => Level.current.CollisionPointPlacementLayer<T>(new Vec2(x, y), ignore, layer);

    public static T CheckPoint<T>(Vec2 point, Thing ignore = null, Layer layer = null) => Level.current.CollisionPoint<T>(point, ignore, layer);

    public static T CheckPointPlacementLayer<T>(Vec2 point, Thing ignore = null, Layer layer = null) => Level.current.CollisionPointPlacementLayer<T>(point, ignore, layer);

    public static IEnumerable<T> CheckPointAll<T>(float x, float y, Layer layer = null) => Level.current.CollisionPointAll<T>(new Vec2(x, y), layer);

    public static IEnumerable<T> CheckPointAll<T>(Vec2 point, Layer layer = null) => Level.current.CollisionPointAll<T>(point, layer);

    public T CollisionPoint<T>(float x, float y, Thing ignore = null, Layer layer = null) => this.CollisionPoint<T>(new Vec2(x, y), ignore, layer);

    public List<KeyValuePair<float, Thing>> nearest(
      Vec2 point,
      IEnumerable<Thing> things,
      Thing ignore,
      Layer layer = null,
      bool placementLayer = false)
    {
      List<KeyValuePair<float, Thing>> keyValuePairList = new List<KeyValuePair<float, Thing>>();
      foreach (Thing thing in things)
      {
        if (thing.ghostType == (ushort) 0 || thing != ignore && (layer == null || (placementLayer || thing.layer == layer) && thing.placementLayer == layer))
          keyValuePairList.Add(new KeyValuePair<float, Thing>((point - thing.position).lengthSq, thing));
      }
      keyValuePairList.Sort((Comparison<KeyValuePair<float, Thing>>) ((x, y) => (double) x.Key >= (double) y.Key ? 1 : -1));
      return keyValuePairList;
    }

    public T NearestThing<T>(Vec2 point, Thing ignore = null, int nearIndex = 0, Layer layer = null)
    {
      System.Type key = typeof (T);
      if (key == typeof (Thing))
      {
        List<KeyValuePair<float, Thing>> keyValuePairList = this.nearest(point, this._things[typeof (Thing)], ignore, layer);
        if (keyValuePairList.Count > nearIndex)
          return (T)(Object)keyValuePairList[nearIndex].Value;
      }
      List<KeyValuePair<float, Thing>> keyValuePairList1 = this.nearest(point, this._things[key], ignore, layer);
      return keyValuePairList1.Count > nearIndex ? (T)(Object)keyValuePairList1[nearIndex].Value : default (T);
    }

    public T CollisionCircle<T>(Vec2 p1, float radius, Thing ignore = null)
    {
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && dynamicObject != ignore && Collision.Circle(p1, radius, dynamicObject))
          return (T)(Object)dynamicObject;
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckCircle<T>(p1, radius, ignore) : default (T);
    }

    public IEnumerable<T> CollisionCircleAll<T>(Vec2 p1, float radius)
    {
      List<object> nextCollisionList = Level.GetNextCollisionList();
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && Collision.Circle(p1, radius, dynamicObject))
          nextCollisionList.Add((object) dynamicObject);
      }
      if (this._things.HasStaticObjects(key))
        this._things.quadTree.CheckCircleAll<T>(p1, radius, nextCollisionList);
      return nextCollisionList.AsEnumerable<object>().Cast<T>();
    }

    public T CollisionRect<T>(Vec2 p1, Vec2 p2, Thing ignore = null)
    {
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && dynamicObject != ignore && Collision.Rect(p1, p2, dynamicObject))
          return (T)(Object)dynamicObject;
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckRectangle<T>(p1, p2, ignore) : default (T);
    }

    public IEnumerable<T> CollisionRectAll<T>(Vec2 p1, Vec2 p2)
    {
      List<object> nextCollisionList = Level.GetNextCollisionList();
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && Collision.Rect(p1, p2, dynamicObject))
          nextCollisionList.Add((object) dynamicObject);
      }
      if (this._things.HasStaticObjects(key))
        this._things.quadTree.CheckRectangleAll<T>(p1, p2, nextCollisionList);
      return nextCollisionList.AsEnumerable<object>().Cast<T>();
    }

    public T CollisionLine<T>(Vec2 p1, Vec2 p2, Thing ignore = null)
    {
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && dynamicObject != ignore && Collision.Line(p1, p2, dynamicObject))
          return (T)(Object)dynamicObject;
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLine<T>(p1, p2, ignore) : default (T);
    }

    public T CollisionLine<T>(Vec2 p1, Vec2 p2, out Vec2 position, Thing ignore = null)
    {
      position = new Vec2(0.0f, 0.0f);
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject != ignore && dynamicObject.ghostType != (ushort) 0)
        {
          Vec2 vec2 = Collision.LinePoint(p1, p2, dynamicObject);
          if (vec2 != Vec2.Zero)
          {
            position = vec2;
            return (T)(Object)dynamicObject;
          }
        }
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLinePoint<T>(p1, p2, out position, ignore) : default (T);
    }

    public IEnumerable<T> CollisionLineAll<T>(Vec2 p1, Vec2 p2)
    {
      List<object> nextCollisionList = Level.GetNextCollisionList();
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && Collision.Line(p1, p2, dynamicObject))
          nextCollisionList.Add((object) dynamicObject);
      }
      if (this._things.HasStaticObjects(key))
      {
        List<T> source = this._things.quadTree.CheckLineAll<T>(p1, p2);
        nextCollisionList.AddRange(source.Cast<object>());
      }
      return nextCollisionList.AsEnumerable<object>().Cast<T>();
    }

    public T CollisionPoint<T>(Vec2 point, Thing ignore = null, Layer layer = null)
    {
      System.Type key = typeof (T);
      if (key == typeof (Thing))
      {
        foreach (Thing thing in this._things)
        {
          if (thing.ghostType != (ushort) 0 && thing != ignore && Collision.Point(point, thing) && (layer == null || layer == thing.layer))
            return (T)(Object)thing;
        }
      }
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && dynamicObject != ignore && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.layer))
          return (T)(Object)dynamicObject;
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPoint<T>(point, ignore, layer) : default (T);
    }

    public T CollisionPointPlacementLayer<T>(Vec2 point, Thing ignore = null, Layer layer = null)
    {
      System.Type key = typeof (T);
      if (key == typeof (Thing))
      {
        foreach (Thing thing in this._things)
        {
          if (thing.ghostType != (ushort) 0 && thing != ignore && Collision.Point(point, thing) && (layer == null || layer == thing.placementLayer))
            return (T)(Object)thing;
        }
      }
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && dynamicObject != ignore && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.placementLayer))
          return (T)(Object)dynamicObject;
      }
      return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPointPlacementLayer<T>(point, ignore, layer) : default (T);
    }

    public IEnumerable<T> CollisionPointAll<T>(Vec2 point, Layer layer = null)
    {
      List<object> nextCollisionList = Level.GetNextCollisionList();
      System.Type key = typeof (T);
      foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
      {
        if (dynamicObject.ghostType != (ushort) 0 && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.layer))
          nextCollisionList.Add((object) dynamicObject);
      }
      if (this._things.HasStaticObjects(key))
      {
        T obj = this._things.quadTree.CheckPoint<T>(point, layer: layer);
        if ((object) obj != null)
          nextCollisionList.Add((object) obj);
      }
      return nextCollisionList.AsEnumerable<object>().Cast<T>();
    }

    public static T CheckRay<T>(Vec2 start, Vec2 end) => Level.current.CollisionRay<T>(start, end);

    public T CollisionRay<T>(Vec2 start, Vec2 end) => Level.CheckRay<T>(start, end, out Vec2 _);

    public static T CheckRay<T>(Vec2 start, Vec2 end, out Vec2 hitPos) => Level.current.CollisionRay<T>(start, end, out hitPos);

    public static T CheckRay<T>(Vec2 start, Vec2 end, Thing ignore, out Vec2 hitPos) => Level.current.CollisionRay<T>(start, end, ignore, out hitPos);

    public T CollisionRay<T>(Vec2 start, Vec2 end, out Vec2 hitPos)
    {
      Vec2 dir = end - start;
      float length = dir.length;
      dir.Normalize();
      Math.Ceiling((double) length);
      Stack<TravelInfo> travelInfoStack = new Stack<TravelInfo>();
      travelInfoStack.Push(new TravelInfo(start, end, length));
      while (travelInfoStack.Count > 0)
      {
        TravelInfo travelInfo = travelInfoStack.Pop();
        if ((object) Level.current.CollisionLine<T>(travelInfo.p1, travelInfo.p2) != null)
        {
          if ((double) travelInfo.length < 8.0)
          {
            T obj = this.Raycast<T>(travelInfo.p1, dir, travelInfo.length, out hitPos);
            if ((object) obj != null)
              return obj;
          }
          else
          {
            float len = travelInfo.length * 0.5f;
            Vec2 vec2 = travelInfo.p1 + dir * len;
            travelInfoStack.Push(new TravelInfo(vec2, travelInfo.p2, len));
            travelInfoStack.Push(new TravelInfo(travelInfo.p1, vec2, len));
          }
        }
      }
      hitPos = end;
      return default (T);
    }

    public T CollisionRay<T>(Vec2 start, Vec2 end, Thing ignore, out Vec2 hitPos)
    {
      Vec2 dir = end - start;
      float length = dir.length;
      dir.Normalize();
      Math.Ceiling((double) length);
      Stack<TravelInfo> travelInfoStack = new Stack<TravelInfo>();
      travelInfoStack.Push(new TravelInfo(start, end, length));
      while (travelInfoStack.Count > 0)
      {
        TravelInfo travelInfo = travelInfoStack.Pop();
        if ((object) Level.current.CollisionLine<T>(travelInfo.p1, travelInfo.p2, ignore) != null)
        {
          if ((double) travelInfo.length < 8.0)
          {
            T obj = this.Raycast<T>(travelInfo.p1, dir, ignore, travelInfo.length, out hitPos);
            if ((object) obj != null)
              return obj;
          }
          else
          {
            float len = travelInfo.length * 0.5f;
            Vec2 vec2 = travelInfo.p1 + dir * len;
            travelInfoStack.Push(new TravelInfo(vec2, travelInfo.p2, len));
            travelInfoStack.Push(new TravelInfo(travelInfo.p1, vec2, len));
          }
        }
      }
      hitPos = end;
      return default (T);
    }

    private T Raycast<T>(Vec2 p1, Vec2 dir, float length, out Vec2 hit)
    {
      int num = (int) Math.Ceiling((double) length);
      Vec2 point = p1;
      do
      {
        --num;
        T obj = Level.current.CollisionPoint<T>(point);
        if ((object) obj != null)
        {
          hit = point;
          return obj;
        }
        point += dir;
      }
      while (num > 0);
      hit = point;
      return default (T);
    }

    private T Raycast<T>(Vec2 p1, Vec2 dir, Thing ignore, float length, out Vec2 hit)
    {
      int num = (int) Math.Ceiling((double) length);
      Vec2 point = p1;
      do
      {
        --num;
        T obj = Level.current.CollisionPoint<T>(point, ignore);
        if ((object) obj != null)
        {
          hit = point;
          return obj;
        }
        point += dir;
      }
      while (num > 0);
      hit = point;
      return default (T);
    }

    private T Rectcast<T>(Vec2 p1, Vec2 p2, Rectangle rect, out Vec2 hit)
    {
      Vec2 vec2_1 = p2 - p1;
      int num = (int) Math.Ceiling((double) vec2_1.length);
      vec2_1.Normalize();
      Vec2 vec2_2 = p1;
      do
      {
        --num;
        T obj = Level.current.CollisionRect<T>(vec2_2 + new Vec2(rect.Top, rect.Left), vec2_2 + new Vec2(rect.Bottom, rect.Right));
        if ((object) obj != null)
        {
          hit = vec2_2;
          return obj;
        }
        vec2_2 += vec2_1;
      }
      while (num > 0);
      hit = vec2_2;
      return default (T);
    }
  }
}
