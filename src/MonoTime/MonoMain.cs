// Decompiled with JetBrains decompiler
// Type: DuckGame.MonoMain
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DuckGame
{
  public class MonoMain : Game
  {
    private static MonoMainCore _core = new MonoMainCore();
    private static RenderTarget2D _screenCapture;
    private static bool _didPauseCapture = false;
    private static bool _started = false;
    private MTEffect _watermarkEffect;
    private Tex2D _watermarkTexture;
    public static MonoMain instance;
    private GraphicsDeviceManager graphics;
    private static int _screenWidth = 1280;
    private static int _screenHeight = 720;
    private bool _fullScreen;
    public static volatile int lazyLoadyBits = 0;
    public static volatile string loadMessage = "HOLD ON...";
    private SpriteMap _duckRun;
    private SpriteMap _duckArm;
    public static Thing thing;
    public static Thread mainThread;
    private bool _canStartLoading;
    public int _adapterW;
    public int _adapterH;
    private static List<Func<string>> _extraExceptionDetails = new List<Func<string>>()
    {
      (Func<string>) (() => "Date: " + DateTime.Now.ToShortDateString()),
      (Func<string>) (() => "Version: " + DG.version),
      (Func<string>) (() => "Platform: " + DG.platform),
      (Func<string>) (() => MonoMain.GetOnlineString()),
      (Func<string>) (() => "Editor: " + Editor.active.ToString()),
      (Func<string>) (() => "Time Played: " + MonoMain.TimeString(DateTime.Now - MonoMain.startTime)),
      (Func<string>) (() => "Special Code: " + Main.SpecialCode + " " + Main.SpecialCode2),
      (Func<string>) (() => "Code: " + Main.codeNumber.ToString()),
      (Func<string>) (() => "Adapter Resolution: " + (object) DuckGame.Graphics.baseDeviceWidth + "x" + (object) DuckGame.Graphics.baseDeviceHeight),
      (Func<string>) (() => "Game Resolution: " + (object) DuckGame.Graphics.width + "x" + (object) DuckGame.Graphics.height),
      (Func<string>) (() => "Fullscreen: " + (object) Options.Data.fullscreen),
      (Func<string>) (() => "Device: " + MonoMain.hasLostDevice.ToString()),
      (Func<string>) (() => "Level: " + MonoMain.GetLevelString()),
      (Func<string>) (() => "Mods: " + (object) ModLoader.numModsTotal + " (" + (object) ModLoader.numModsEnabled + " enabled)"),
      (Func<string>) (() => "Command Line: " + Program.commandLine)
    };
    public static DateTime startTime;
    private RenderTarget2D saveShot;
    public static string[] startupAssemblies;
    private RenderTarget2D targ;
    public static bool notOnlineError = false;
    public static string infiniteLoopDetails;
    public static bool hadInfiniteLoop;
    private Stopwatch _loopTimer = new Stopwatch();
    private Thread _infiniteLoopDetector;
    private bool _changedFullscreenState;
    private Queue<Action> _thingsToLoad = new Queue<Action>();
    private Queue<Action> _thingsToLazyLoad = new Queue<Action>();
    private Thread _lazyLoadThread;
    private static volatile bool _lockLoading = true;
    public static volatile bool loadingLocked = true;
    public static bool moddingEnabled = true;
    public static bool enableThreadedLoading = true;
    public static bool defaultControls = false;
    public static bool noFullscreen = false;
    public static bool disableCloud = false;
    public static bool cloudOnly = false;
    public static bool browseCloud = false;
    public static bool cloudNoLoad = false;
    public static bool cloudNoSave = false;
    public static bool disableSteam = false;
    public static bool noIntro = false;
    public static bool startInEditor = false;
    public static bool preloadModContent = true;
    public static bool breakSteam = false;
    public static bool modDebugging = false;
    private bool _threadedLoadingStarted;
    private static Thread _initializeThread;
    private static List<WorkshopItem> availableModsToDownload = new List<WorkshopItem>();
    public static bool downloadWorkshopMods = false;
    private bool _doStart;
    private volatile bool lostDevice;
    public static bool hasLostDevice = false;
    private bool deviceDisposed;
    public static bool _foreverLoad = true;
    public static bool _didReceiptCheck = false;
    public static bool _recordingStarted = false;
    public static bool _recordData = false;
    public static volatile bool contentLoadLockLoop = false;
    public static volatile bool contentLoadingLock = false;
    public bool doReset;
    private static bool shouldPauseGameplay;
    public static bool exit = false;
    private Stopwatch _loadingTimer = new Stopwatch();
    public static volatile bool pause = false;
    public static volatile bool paused = false;
    private bool _setCulture;
    public static bool autoPauseFade = true;
    private static MaterialPause _pauseMaterial;
    private bool _didFirstDraw;
    private RenderTarget2D _screenshotTarget;
    private int _numShots;
    private int waitFrames;
    private bool takingShot;
    public static bool doPauseFade = true;
    public static bool alternateFullscreen = false;
    public static volatile int loadyBits = 0;
    public static volatile int totalLoadyBits = 365;
    private int deviceLostWait;

    public static MonoMainCore core
    {
      get => MonoMain._core;
      set => MonoMain._core = value;
    }

    private static UIComponent _pauseMenu
    {
      get => MonoMain._core._pauseMenu;
      set => MonoMain._core._pauseMenu = value;
    }

    public static UIComponent pauseMenu
    {
      get => MonoMain._pauseMenu != null && !MonoMain._pauseMenu.inWorld && !MonoMain._pauseMenu.open ? (UIComponent) null : MonoMain._pauseMenu;
      set
      {
        if (MonoMain._pauseMenu != value && MonoMain._pauseMenu != null && (MonoMain._pauseMenu.open && !MonoMain._pauseMenu.inWorld))
          MonoMain._pauseMenu.Close();
        MonoMain._pauseMenu = value;
      }
    }

    public static List<UIComponent> closeMenuUpdate => MonoMain._core.closeMenuUpdate;

    public static RenderTarget2D screenCapture => MonoMain._screenCapture;

    public static bool started => MonoMain._started;

    public static int screenWidth => MonoMain._screenWidth;

    public static int screenHeight => MonoMain._screenHeight;

    public static string GetOnlineString()
    {
      if (!Network.isActive)
        return "Online: FALSE";
      return "Online: TRUE\r\n" + "Ping: " + (object) Network.activeNetwork.core.averagePing + "\r\n" + "Loss: " + (object) Network.activeNetwork.core.averagePacketLossPercent + "\r\n" + "Jitter: " + (object) Network.activeNetwork.core.averageJitter + "\r\n" + "IsHost: " + Network.activeNetwork.core.isServer.ToString();
    }

    public static string GetLevelString()
    {
      if (Level.current == null)
        return "null";
      if (!(Level.current is XMLLevel))
        return Level.current.GetType().ToString();
      XMLLevel current = Level.current as XMLLevel;
      if (current.level == "RANDOM")
        return "RANDOM";
      return current.data == null ? Level.current.GetType().ToString() : current.data.GetPath();
    }

    public static string GetExceptionString(UnhandledExceptionEventArgs e) => MonoMain.GetExceptionString(e.ExceptionObject);

    public static string GetExceptionString(object e)
    {
      string str1 = e.ToString();
      try
      {
        if (e is UnauthorizedAccessException)
        {
          UnauthorizedAccessException unauthorizedAccessException = e as UnauthorizedAccessException;
          int startIndex = unauthorizedAccessException.Message.IndexOf(":") - 1;
          if (startIndex > 0)
          {
            int num = unauthorizedAccessException.Message.LastIndexOf("'");
            if (num > 0)
            {
              FileAttributes attributes = new FileInfo(unauthorizedAccessException.Message.Substring(startIndex, num - startIndex)).Attributes;
              string str2 = "(";
              foreach (FileAttributes fileAttributes in System.Enum.GetValues(typeof (FileAttributes)))
              {
                if ((attributes & fileAttributes) > (FileAttributes) 0)
                  str2 = fileAttributes != FileAttributes.Encrypted ? str2 + fileAttributes.ToString() + "," : str2 + fileAttributes.ToString();
              }
              str1 = str2 + ")" + str1;
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      string str3 = str1 + "\r\n";
      foreach (Func<string> extraExceptionDetail in MonoMain._extraExceptionDetails)
      {
        string str2 = "FIELD FAILED";
        try
        {
          str2 = extraExceptionDetail();
        }
        catch
        {
        }
        str3 += "\r\n";
        str3 += str2;
      }
      return str3;
    }

    public static void UploadError(string message)
    {
      try
      {
        message = "RELEASE " + message;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("http://www.wonthelp.info/DuckWeb/sendError_beta.php");
        httpWebRequest.Method = "POST";
        byte[] bytes = Encoding.ASCII.GetBytes("sendRequest=duckGameWeb2000&data=" + message);
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
      }
      catch
      {
      }
    }

    public static string RequestCape(string data)
    {
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(string.Format("http://www.wonthelp.info/DuckWeb/getCape.php?sendRequest=IWannaUseADangOlCape&id=" + data));
        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        string str = "";
        if (response.StatusCode == HttpStatusCode.OK)
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
              str = streamReader.ReadToEnd();
          }
        }
        response.Close();
        return str;
      }
      catch
      {
      }
      return "";
    }

    public static void LogPlay() => new Thread(new ThreadStart(MonoMain.LogPlayAsync))
    {
      CurrentCulture = CultureInfo.InvariantCulture,
      Priority = ThreadPriority.BelowNormal,
      IsBackground = true
    }.Start();

    public void SaveShot() => new Thread(new ThreadStart(this.SaveShotThread))
    {
      CurrentCulture = CultureInfo.InvariantCulture,
      Priority = ThreadPriority.BelowNormal,
      IsBackground = true
    }.Start();

    public void SaveShotThread()
    {
      RenderTarget2D saveShot = this.saveShot;
      string str = DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortTimeString() + " " + this._numShots.ToString();
      ++this._numShots;
      FileStream fileStream = System.IO.File.OpenWrite("duckscreen-" + str.Replace("/", "_").Replace(":", "-").Replace(" ", "") + ".png");
      (saveShot.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).SaveAsPng((Stream) fileStream, saveShot.width, saveShot.height);
      fileStream.Close();
    }

    public static string TimeString(TimeSpan span, int places = 3, bool small = false)
    {
      if (!small)
        return (places > 2 ? (span.Hours < 10 ? "0" + Change.ToString((object) span.Hours) : Change.ToString((object) span.Hours)) + ":" : "") + (places > 1 ? (span.Minutes < 10 ? "0" + Change.ToString((object) span.Minutes) : Change.ToString((object) span.Minutes)) + ":" : "") + (span.Seconds < 10 ? "0" + Change.ToString((object) span.Seconds) : Change.ToString((object) span.Seconds));
      int num = (int) ((double) span.Milliseconds / 1000.0 * 99.0);
      return (places > 2 ? (span.Minutes < 10 ? "0" + Change.ToString((object) span.Minutes) : Change.ToString((object) span.Minutes)) + ":" : "") + (places > 1 ? (span.Seconds < 10 ? "0" + Change.ToString((object) span.Seconds) : Change.ToString((object) span.Seconds)) + ":" : "") + (num < 10 ? "0" + Change.ToString((object) num) : Change.ToString((object) num));
    }

    private static void LogPlayAsync()
    {
      try
      {
        string str = "Played for " + MonoMain.TimeString(DateTime.Now - MonoMain.startTime) + " IsDemo: " + (object) Main.isDemo;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("http://www.wonthelp.info/DuckWeb/logPlay.php");
        httpWebRequest.Method = "POST";
        byte[] bytes = Encoding.ASCII.GetBytes("sendRequest=duckGameWeb2000&data=" + str);
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) bytes.Length;
        httpWebRequest.Timeout = 6000;
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        MonoMain.startTime = DateTime.Now;
      }
      catch
      {
      }
    }

    public MonoMain()
    {
      MonoMain.startupAssemblies = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (x => !x.IsDynamic)).Select<Assembly, string>((Func<Assembly, string>) (assembly => assembly.Location)).ToArray<string>();
      DG.SetVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString());
      this.Content = (ContentManager) new SynchronizedContentManager((System.IServiceProvider) this.Services);
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
      this._adapterW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
      this._adapterH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
      int num1 = 1280;
      if (num1 > this._adapterW)
        num1 = 1024;
      if (num1 > this._adapterW)
        num1 = 640;
      if (num1 > this._adapterW)
        num1 = 320;
      if (this._adapterW > 1920)
        num1 = 1920;
      float num2 = (float) this._adapterH / (float) this._adapterW;
      if ((double) num2 < 0.560000002384186)
      {
        num2 = 9f / 16f;
        this._adapterH = (int) ((double) this._adapterW * (double) num2);
      }
      int num3 = (int) ((double) num2 * (double) num1);
      if (num3 > 1200)
        num3 = 1200;
      MonoMain._screenWidth = num1;
      MonoMain._screenHeight = num3;
      DuckGame.Graphics.InitializeBase(this.graphics, MonoMain.screenWidth, MonoMain.screenHeight);
      DuckFile.Initialize();
      DuckFile.InitializeCloud();
      Options.Load();
      if (MonoMain.noFullscreen)
        Options.Data.fullscreen = false;
      this.SetFullscreen(Options.Data.fullscreen);
      Resolution.Initialize(ref this.graphics, MonoMain.screenWidth, MonoMain.screenHeight);
      this.UpdateFullscreen();
    }

    public void InfiniteLoopDetector()
    {
      while (true)
      {
        do
        {
          Thread.Sleep(30);
        }
        while (this._loopTimer.Elapsed.TotalSeconds <= 5.0);
        MonoMain.mainThread.Suspend();
        MonoMain.infiniteLoopDetails = MonoMain.GetInfiniteLoopDetails();
        MonoMain.hadInfiniteLoop = true;
        MonoMain.mainThread.Resume();
        MonoMain.mainThread.Abort();
      }
    }

    public static string GetInfiniteLoopDetails()
    {
            // TODO
            //StackTrace stackTrace = new StackTrace(MonoMain.mainThread, true);
            //string str = "An infinite loop occurred.\r\n";
            //int num = 15;
            //for (int index = 0; index < num; ++index)
            //{
            //  if (index < stackTrace.FrameCount)
            //  {
            //    StackFrame frame = stackTrace.GetFrame(index);
            //    if (frame.GetFileName() != null)
            //      str = str + "  at " + frame.ToString();
            //    else
            //      ++num;
            //  }
            //}
            //return str;
            return "";
    }

    protected override void LoadContent() => base.LoadContent();

    protected override void Initialize()
    {
      MonoMain.startTime = DateTime.Now;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      this.IsFixedTimeStep = true;
      MonoMain.instance = this;
      MonoMain.contentLoadLockLoop = true;
      MonoMain.mainThread = Thread.CurrentThread;
      base.Initialize();
      DuckGame.Content.InitializeBase(this.Content);
      Rando.DoInitialize();
      NetRand.Initialize();
      this.InactiveSleepTime = new TimeSpan(0L);
      DuckGame.Graphics.Initialize(this.GraphicsDevice);
      Input.Initialize();
      MonoMain._screenCapture = new RenderTarget2D(DuckGame.Graphics.width, DuckGame.Graphics.height);
      Layer.InitializeLayers();
      this._duckRun = new SpriteMap("duck", 32, 32);
      this._duckRun.AddAnimation("run", 1f, true, 1, 2, 3, 4, 5, 6);
      this._duckRun.SetAnimation("run");
      this._duckArm = new SpriteMap("duckArms", 16, 16);
      DuckGame.Graphics.device.DeviceLost += new EventHandler<EventArgs>(this.DeviceLost);
      DuckGame.Graphics.device.DeviceReset += new EventHandler<EventArgs>(this.DeviceReset);
      this._canStartLoading = true;
    }

    public void KillEverything()
    {
      try
      {
        Music.Terminate();
        if (this._lazyLoadThread != null && this._lazyLoadThread.IsAlive)
          this._lazyLoadThread.Abort();
        if (MonoMain._initializeThread != null)
        {
          if (MonoMain._initializeThread.IsAlive)
            MonoMain._initializeThread.Abort();
        }
      }
      catch
      {
      }
      try
      {
        NetworkDebugger.TerminateThreads();
        Network.Terminate();
        Input.Terminate();
      }
      catch
      {
      }
      try
      {
        DeviceChangeNotifier.Stop();
      }
      catch
      {
      }
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
      this.KillEverything();
      Process.GetCurrentProcess().Kill();
    }

    private void SetFullscreen(bool fullscreen)
    {
      if (this._fullScreen == fullscreen)
        return;
      this._changedFullscreenState = true;
      this._fullScreen = fullscreen;
      Options.Data.fullscreen = this._fullScreen;
    }

    private void InitializeSystems()
    {
      while (this._thingsToLoad.Count > 0)
      {
        if (MonoMain.lockLoading)
        {
          MonoMain.loadingLocked = true;
          while (MonoMain.lockLoading)
            Thread.Sleep(10);
        }
        MonoMain.loadingLocked = false;
        this._thingsToLoad.Dequeue()();
      }
    }

    private void DoLazyLoading()
    {
      while (this._thingsToLazyLoad.Count > 0)
        this._thingsToLazyLoad.Dequeue()();
    }

    private void StartLazyLoad()
    {
      this._thingsToLazyLoad.Enqueue(new Action(SFX.Initialize));
      this._thingsToLazyLoad.Enqueue(new Action(DuckGame.Content.Initialize));
      if (MonoMain.enableThreadedLoading)
      {
        this._lazyLoadThread = new Thread(new ThreadStart(this.DoLazyLoading));
        this._lazyLoadThread.CurrentCulture = CultureInfo.InvariantCulture;
        this._lazyLoadThread.Priority = ThreadPriority.BelowNormal;
        this._lazyLoadThread.IsBackground = true;
        this._lazyLoadThread.Start();
      }
      else
        this.DoLazyLoading();
    }

    public static bool lockLoading
    {
      get => !MonoMain._started && MonoMain._lockLoading;
      set => MonoMain._lockLoading = value;
    }

    public static Thread initializeThread => MonoMain._initializeThread;

    private static void ResultFetched(object value0, WorkshopQueryResult result)
    {
      if (result == null || result.details == null)
        return;
      WorkshopItem publishedFile = result.details.publishedFile;
      int num1 = ((IEnumerable<string>) DuckFile.GetFiles(publishedFile.path)).Count<string>();
      int num2 = ((IEnumerable<string>) DuckFile.GetDirectories(publishedFile.path)).Count<string>();
      if ((num1 != 0 || num2 != 0) && ((publishedFile.stateFlags & WorkshopItemState.Installed) != WorkshopItemState.None && (publishedFile.stateFlags & WorkshopItemState.NeedsUpdate) == WorkshopItemState.None))
        return;
      MonoMain.availableModsToDownload.Add(publishedFile);
    }

    private void DownloadWorkshopItems()
    {
      MonoMain.loadMessage = "Downloading workshop mods...";
      if (!Steam.IsInitialized())
        return;
      bool done = false;
      WorkshopQueryUser queryUser = Steam.CreateQueryUser(Steam.user.id, WorkshopList.Subscribed, WorkshopType.UsableInGame, WorkshopSortOrder.TitleAsc);
      queryUser.requiredTags.Add("Mod");
      queryUser.onlyQueryIDs = true;
      queryUser.QueryFinished += (WorkshopQueryFinished) (sender => done = true);
      queryUser.ResultFetched += new WorkshopQueryResultFetched(MonoMain.ResultFetched);
      queryUser.Request();
      while (!done)
      {
        Steam.Update();
        Thread.Sleep(13);
      }
      MonoMain.totalLoadyBits = MonoMain.availableModsToDownload.Count;
      MonoMain.loadyBits = 0;
      foreach (WorkshopItem workshopItem in MonoMain.availableModsToDownload)
      {
        MonoMain.loadMessage = "Downloading workshop mods (" + MonoMain.loadyBits.ToString() + "/" + MonoMain.totalLoadyBits.ToString() + ")";
        if (Steam.DownloadWorkshopItem(workshopItem))
        {
          while (!workshopItem.finishedProcessing)
            Thread.Sleep(20);
        }
        ++MonoMain.loadyBits;
      }
    }

    private void StartThreadedLoading()
    {
      this._threadedLoadingStarted = true;
      Network.Initialize();
      Teams.Initialize();
      Chancy.Initialize();
      this._watermarkEffect = DuckGame.Content.Load<MTEffect>("Shaders/basicWatermark");
      this._watermarkTexture = DuckGame.Content.Load<Tex2D>("looptex");
      DuckNetwork.Initialize();
      Persona.Initialize();
      if (MonoMain.moddingEnabled)
        this._thingsToLoad.Enqueue(new Action(ManagedContent.InitializeMods));
      this._thingsToLoad.Enqueue(new Action(DeathCrate.Initialize));
      this._thingsToLoad.Enqueue(new Action(Editor.InitializeConstructorLists));
      this._thingsToLoad.Enqueue(new Action(DuckGame.Content.InitializeLevels));
      this._thingsToLoad.Enqueue(new Action(DuckGame.Content.InitializeEffects));
      this._thingsToLoad.Enqueue(new Action(Music.Initialize));
      this._thingsToLoad.Enqueue(new Action(Input.InitializeGraphics));
      this._thingsToLoad.Enqueue(new Action(SFX.Initialize));
      this._thingsToLoad.Enqueue(new Action(DuckGame.Content.Initialize));
      this._thingsToLoad.Enqueue(new Action(Editor.InitializePlaceableGroup));
      this._thingsToLoad.Enqueue(new Action(Challenges.Initialize));
      this._thingsToLoad.Enqueue(new Action(Collision.Initialize));
      this._thingsToLoad.Enqueue(new Action(Highlights.Initialize));
      this._thingsToLoad.Enqueue(new Action(Level.InitializeCollisionLists));
      this._thingsToLoad.Enqueue(new Action(Keyboard.InitTriggerImages));
      if (MonoMain.downloadWorkshopMods)
        this._thingsToLoad.Enqueue(new Action(this.DownloadWorkshopItems));
      this._thingsToLoad.Enqueue(new Action(this.SetStarted));
      if (!MonoMain.enableThreadedLoading)
        return;
      MonoMain._initializeThread = new Thread(new ThreadStart(this.InitializeSystems));
      MonoMain._initializeThread.CurrentCulture = CultureInfo.InvariantCulture;
      MonoMain._initializeThread.Priority = ThreadPriority.Highest;
      MonoMain._initializeThread.IsBackground = true;
      MonoMain._initializeThread.Start();
    }

    private void SetStarted() => this._doStart = true;

    private void Start()
    {
      this.OnStart();
      MonoMain._started = true;
    }

    protected virtual void OnStart()
    {
    }

    protected override void UnloadContent()
    {
    }

    public static void StartRecording(string name)
    {
      MonoMain._recordingStarted = true;
      MonoMain._recordData = true;
      if (!MonoMain._recordData)
        return;
      if (Recorder.globalRecording == null)
        Recorder.globalRecording = new FileRecording();
      Recorder.globalRecording.StartWriting(name);
    }

    public static void StartPlayback()
    {
      MonoMain._recordingStarted = true;
      MonoMain._recordData = false;
      Recorder.globalRecording.LoadAtlasFile();
    }

    public static void StopRecording()
    {
      MonoMain._recordingStarted = false;
      if (Recorder.globalRecording != null)
        Recorder.globalRecording.StopWriting();
      Recorder.globalRecording = (FileRecording) null;
    }

    private void DeviceLost(object obj, EventArgs args)
    {
      MonoMain.hasLostDevice = true;
      this.lostDevice = true;
    }

    private void DeviceReset(object obj, EventArgs args)
    {
      MonoMain.hasLostDevice = true;
      this.lostDevice = true;
    }

    private void DeviceDispose(object obj, EventArgs args)
    {
      MonoMain.hasLostDevice = true;
      this.lostDevice = true;
      this.deviceDisposed = true;
    }

    protected override void Update(GameTime gameTime)
    {
      try
      {
        this._loopTimer.Restart();
        MonoMain.contentLoadLockLoop = true;
        this.RunUpdate(gameTime);
        MonoMain.contentLoadLockLoop = false;
      }
      catch (ThreadAbortException ex)
      {
        Program.HandleGameCrash((Exception) ex);
      }
    }

    public static void UpdatePauseMenu()
    {
      MonoMain.shouldPauseGameplay = true;
      if (Network.isActive && (!(Level.current is TeamSelect2) || !(Level.current as TeamSelect2).MatchmakerOpen()))
        MonoMain.shouldPauseGameplay = false;
      if (MonoMain._pauseMenu != null)
      {
        if (!MonoMain._didPauseCapture)
        {
          DuckGame.Graphics.screenCapture = MonoMain._screenCapture;
          MonoMain._didPauseCapture = true;
        }
        if (MonoMain.shouldPauseGameplay)
        {
          HUD.Update();
          if (Level.current != null && Level.current._netDebug != null)
            Level.current._netDebug.Update(Network.activeNetwork);
          MonoMain._pauseMenu.Update();
        }
        else
        {
          MonoMain._pauseMenu.Update();
          Input.ignoreInput = true;
        }
        if (MonoMain._pauseMenu != null && !MonoMain._pauseMenu.open)
          MonoMain._pauseMenu = (UIComponent) null;
      }
      else
        MonoMain.shouldPauseGameplay = false;
      for (int index = 0; index < MonoMain.closeMenuUpdate.Count; ++index)
      {
        UIComponent uiComponent = MonoMain.closeMenuUpdate[index];
        uiComponent.Update();
        if (!uiComponent.animating)
        {
          MonoMain.closeMenuUpdate.RemoveAt(index);
          --index;
        }
      }
    }

    public void RunUpdate(GameTime gameTime)
    {
      ++DuckGame.Graphics.frame;
      Tasker.RunTasks();
      if (DuckGame.Graphics.device.PresentationParameters.BackBufferWidth != this.graphics.PreferredBackBufferWidth || DuckGame.Graphics.device.PresentationParameters.BackBufferHeight != this.graphics.PreferredBackBufferHeight)
      {
        this.graphics.PreferredBackBufferHeight = this.graphics.PreferredBackBufferHeight;
        this.graphics.PreferredBackBufferWidth = this.graphics.PreferredBackBufferWidth;
        this.graphics.ApplyChanges();
      }
      if (this._canStartLoading && !this._threadedLoadingStarted && this._didFirstDraw)
        this.StartThreadedLoading();
      if (MonoMain.enableThreadedLoading)
      {
        if (!MonoMain._started)
        {
          MonoMain.lockLoading = false;
          Thread.Sleep(16);
          MonoMain.lockLoading = true;
          int num = 0;
          while (!MonoMain.loadingLocked)
          {
            Thread.Sleep(4);
            ++num;
            if (MonoMain._initializeThread == null || !MonoMain._initializeThread.IsAlive || num > 500)
            {
              MonoMain.loadingLocked = true;
              break;
            }
          }
        }
      }
      else if (!MonoMain.enableThreadedLoading)
      {
        MonoMain.loadingLocked = false;
        MonoMain.lockLoading = false;
        if (this._thingsToLoad.Count > 0)
          this._thingsToLoad.Dequeue()();
      }
      if (this._doStart && !MonoMain._started && !MonoMain._foreverLoad)
      {
        this._doStart = false;
        this.Start();
      }
      if (DuckGame.Graphics.screenCapture != null)
        return;
      if (DuckGame.Graphics.inFocus)
        Input.Update();
      if (MonoMain._started && !NetworkDebugger.enabled)
      {
        InputProfile.Update();
        Network.PreUpdate();
      }
      MonoMain._foreverLoad = false;
      if (Keyboard.Pressed(Keys.F4) || (Keyboard.Pressed(Keys.LeftAlt) || Keyboard.Pressed(Keys.RightAlt)) && Keyboard.Pressed(Keys.Enter))
        this.SetFullscreen(!this._fullScreen);
      if (this._fullScreen != Options.Data.fullscreen)
        this.SetFullscreen(Options.Data.fullscreen);
      if (this._changedFullscreenState)
      {
        this.UpdateFullscreen();
        MonoMain._screenCapture = new RenderTarget2D(DuckGame.Graphics.width, DuckGame.Graphics.height);
      }
      else
      {
        Steam.Update();
        if (!MonoMain._started || MonoMain._foreverLoad)
          return;
        if (Keyboard.Pressed(Keys.F2))
          Program.MakeNetLog();
        if (MonoMain.exit || (Keyboard.Down(Keys.LeftAlt) || Keyboard.Down(Keys.RightAlt)) && Keyboard.Down(Keys.F4))
        {
          this.KillEverything();
          this.Exit();
        }
        else
        {
          if (!NetworkDebugger.enabled)
            DevConsole.Update();
          else
            this.IsMouseVisible = false;
          SFX.Update();
          Options.Update();
          AnalogGamePad.repeat = Level.current is Editor || MonoMain._pauseMenu != null || Editor.selectingLevel;
          Keyboard.repeat = Level.current is Editor || MonoMain._pauseMenu != null || (DevConsole.open || DuckNetwork.core.enteringText) || Editor.enteringText;
          MonoMain.UpdatePauseMenu();
          if (!MonoMain.shouldPauseGameplay)
          {
            if (MonoMain._pauseMenu == null)
              MonoMain._didPauseCapture = false;
            if (!MonoMain._recordingStarted || MonoMain._recordData)
            {
              if (DevConsole.rhythmMode && Level.current is GameLevel)
              {
                TimeSpan timeSpan = Music.position + new TimeSpan(0, 0, 0, 0, 80);
                float num1 = 140f;
                RhythmMode.TickSound((float) ((double) ((float) timeSpan.TotalMinutes * num1) % 1.0 / 1.0));
                timeSpan = Music.position;
                timeSpan += new TimeSpan(0, 0, 0, 0, 40);
                float num2 = 140f;
                RhythmMode.Tick((float) ((double) ((float) timeSpan.TotalMinutes * num2) % 1.0 / 1.0));
              }
              AutoUpdatables.Update();
              DuckGame.Content.Update();
              Music.Update();
              Level.UpdateLevelChange();
              Level.UpdateCurrentLevel();
              this.OnUpdate();
            }
          }
          DuckGame.Graphics.RunRenderTasks();
          Input.ignoreInput = false;
          base.Update(gameTime);
          FPSCounter.Tick(0);
          if (NetworkDebugger.enabled)
            return;
          Network.PostUpdate();
        }
      }
    }

    protected virtual void OnUpdate()
    {
    }

    public static void RenderGame(RenderTarget2D target)
    {
      DuckGame.Graphics.ResetDepthBias();
      int width = DuckGame.Graphics.width;
      int height = DuckGame.Graphics.height;
      DuckGame.Graphics.SetRenderTarget(target);
      Viewport viewport = new Viewport();
      viewport.X = viewport.Y = 0;
      viewport.Width = target.width;
      viewport.Height = target.height;
      viewport.MinDepth = 0.0f;
      viewport.MaxDepth = 1f;
      DuckGame.Graphics.viewport = viewport;
      DuckGame.Graphics.width = target.width;
      DuckGame.Graphics.height = target.height;
      Level.DrawCurrentLevel();
      DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
      MonoMain.instance.OnDraw();
      DuckGame.Graphics.screen.End();
      DuckGame.Graphics.width = width;
      DuckGame.Graphics.height = height;
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      Resolution.FullViewport();
      Resolution.ResetViewport();
    }

    public static MaterialPause pauseMaterial => MonoMain._pauseMaterial;

    protected override void Draw(GameTime gameTime)
    {
      try
      {
        do
          ;
        while (MonoMain.contentLoadingLock);
        this.RunDraw(gameTime);
        DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
        this.lostDevice = false;
      }
      catch (ThreadAbortException ex)
      {
        Program.HandleGameCrash((Exception) ex);
      }
    }

    private void UpdateFullscreen()
    {
      if (!this._fullScreen)
      {
        if (MonoMain.alternateFullscreen)
        {
          Application.EnableVisualStyles();
          ((Form) Control.FromHandle(this.Window.Handle)).FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        Resolution.SetResolution(MonoMain.screenWidth, MonoMain.screenHeight, false);
        DuckGame.Graphics.SetSize(MonoMain.screenWidth, MonoMain.screenHeight);
      }
      else if (MonoMain.alternateFullscreen)
      {
        Application.EnableVisualStyles();
        Form form = (Form) Control.FromHandle(this.Window.Handle);
        form.FormBorderStyle = FormBorderStyle.None;
        DuckGame.Graphics._manager.SynchronizeWithVerticalRetrace = false;
        form.Location = new System.Drawing.Point(0, 0);
        Resolution.SetResolution(this._adapterW, this._adapterH, false);
        this.IsMouseVisible = false;
        DuckGame.Graphics.mouseVisible = false;
        int w = this._adapterW;
        int h = this._adapterH;
        if (w > 1920)
        {
          float num = (float) h / (float) w;
          w = 1920;
          h = (int) ((double) w * (double) num);
          if (h > 1200)
            h = 1200;
        }
        DuckGame.Graphics.SetSize(w, h);
      }
      else
      {
        if (this._adapterW > 1920)
        {
          float num = (float) this._adapterH / (float) this._adapterW;
          this._adapterW = 1920;
          this._adapterH = (int) ((double) this._adapterW * (double) num);
          if (this._adapterH > 1200)
            this._adapterH = 1200;
        }
        Resolution.SetResolution(this._adapterW, this._adapterH, true);
        this.IsMouseVisible = false;
        DuckGame.Graphics.mouseVisible = false;
        DuckGame.Graphics.SetSize(this._adapterW, this._adapterH);
      }
      Form form1 = (Form) Control.FromHandle(this.Window.Handle);
      if (form1.Location.Y < 0)
        form1.Location = new System.Drawing.Point(form1.Location.X, 0);
      this._changedFullscreenState = false;
    }

    protected void RunDraw(GameTime gameTime)
    {
      if (this.doReset)
        return;
      if (this.lostDevice)
      {
        DuckGame.Graphics.Clear(Color.Black);
      }
      else
      {
        this._didFirstDraw = true;
        DuckGame.Graphics.frameFlipFlop = !DuckGame.Graphics.frameFlipFlop;
        DuckGame.Content.UpdateRender();
        if (DuckGame.Graphics.device.IsDisposed)
        {
          this.doReset = true;
        }
        else
        {
          DuckGame.Graphics.SetScissorRectangle(new Rectangle(0.0f, 0.0f, (float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height));
          if (Recorder.currentRecording != null)
            Recorder.currentRecording.NextFrame();
          if (Recorder.globalRecording != null)
          {
            Recorder.globalRecording.NextFrame();
            if (MonoMain._recordingStarted && !MonoMain._recordData)
            {
              DuckGame.Graphics.ResetDepthBias();
              DuckGame.Graphics.Clear(new Color(0, 0, 0));
              Recorder.globalRecording.RenderFrame();
              Recorder.globalRecording.IncrementFrame(1f - InputProfile.Get("MPPlayer1").rightTrigger);
              return;
            }
          }
          if (!MonoMain._started)
          {
            DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
            MonoMain._pauseMaterial = new MaterialPause();
            if (!this._setCulture)
            {
              Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
              this._setCulture = true;
            }
            DuckGame.Graphics.ResetDepthBias();
            DuckGame.Graphics.Clear(new Color(0, 0, 0));
            Camera camera = new Camera(0.0f, 0.0f, (float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height);
            DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect) null, camera.getMatrix());
            Vec2 p1 = new Vec2(50f, (float) (DuckGame.Graphics.height - 50));
            Vec2 vec2_1 = new Vec2((float) (DuckGame.Graphics.width - 100), 20f);
            DuckGame.Graphics.DrawRect(p1, p1 + vec2_1, Color.DarkGray * 0.1f, new Depth(0.5f));
            float num = (float) MonoMain.loadyBits / (float) MonoMain.totalLoadyBits;
            if ((double) num > 1.0)
              num = 1f;
            DuckGame.Graphics.DrawRect(p1, p1 + new Vec2(vec2_1.x * num, vec2_1.y), Color.White * 0.1f, new Depth(0.6f));
            string text = MonoMain.loadMessage;
            if (MonoMain._foreverLoad && MonoMain._didReceiptCheck)
              text = !MonoMain.notOnlineError ? "DRM FAILURE(ughhh). Gotta crack it. Or it's broke, I could have broke it. Sorry dude (I'm so sorry)." : "Couldn't get DRM crap (ughhh), have you a web connection? Do you need... an adapter?";
            DuckGame.Graphics.DrawString(text, p1 + new Vec2(0.0f, -16f), Color.White, new Depth(1f));
            this._duckRun.speed = 0.15f;
            this._duckRun.scale = new Vec2(4f, 4f);
            this._duckRun.depth = new Depth(0.7f);
            this._duckRun.color = new Color(80, 80, 80);
            Vec2 vec2_2 = new Vec2((float) (DuckGame.Graphics.width - this._duckRun.width * 4 - 50), (float) (DuckGame.Graphics.height - this._duckRun.height * 4 - 55));
            DuckGame.Graphics.Draw((Sprite) this._duckRun, vec2_2.x, vec2_2.y);
            this._duckArm.frame = this._duckRun.imageIndex;
            this._duckArm.scale = new Vec2(4f, 4f);
            this._duckArm.depth = new Depth(0.6f);
            this._duckArm.color = new Color(80, 80, 80);
            DuckGame.Graphics.Draw((Sprite) this._duckArm, vec2_2.x + 20f, vec2_2.y + 56f);
            DuckGame.Graphics.screen.End();
            Resolution.FullViewport();
            Resolution.ResetViewport();
            DuckGame.Graphics.ResetDepthBias();
          }
          else
          {
            if (Level.current == null)
              return;
            DuckGame.Reflection.Render();
            Resolution.clearColor = new Color((int) byte.MaxValue, 0, (int) byte.MaxValue);
            if (!this.takingShot)
            {
              this.takingShot = true;
              if (Keyboard.Pressed(Keys.F12) || this.waitFrames < 0)
              {
                if (this._screenshotTarget == null)
                  this._screenshotTarget = new RenderTarget2D(DuckGame.Graphics.width, DuckGame.Graphics.height);
                DuckGame.Graphics.screenCapture = this._screenshotTarget;
                this.RunDraw(gameTime);
                this.waitFrames = 60 + Rando.Int(60);
              }
              this.takingShot = false;
            }
            if (DuckGame.Graphics.screenCapture != null)
            {
              DuckGame.Graphics.ResetDepthBias();
              int width = DuckGame.Graphics.width;
              int height = DuckGame.Graphics.height;
              DuckGame.Graphics.SetRenderTarget(DuckGame.Graphics.screenCapture);
              Viewport viewport = new Viewport();
              viewport.X = viewport.Y = 0;
              viewport.Width = DuckGame.Graphics.screenCapture.width;
              viewport.Height = DuckGame.Graphics.screenCapture.height;
              viewport.MinDepth = 0.0f;
              viewport.MaxDepth = 1f;
              DuckGame.Graphics.viewport = viewport;
              DuckGame.Graphics.width = DuckGame.Graphics.screenCapture.width;
              DuckGame.Graphics.height = DuckGame.Graphics.screenCapture.height;
              HUD.hide = true;
              Level.DrawCurrentLevel();
              DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
              this.OnDraw();
              DuckGame.Graphics.screen.End();
              HUD.hide = false;
              DuckGame.Graphics.screenCapture = (RenderTarget2D) null;
              DuckGame.Graphics.width = width;
              DuckGame.Graphics.height = height;
              DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
              Resolution.FullViewport();
              Resolution.ResetViewport();
            }
            if (this._screenshotTarget != null)
            {
              this.saveShot = this._screenshotTarget;
              this._screenshotTarget = (RenderTarget2D) null;
              this.SaveShot();
            }
            if (DuckGame.Graphics.screenTarget != null)
            {
              DuckGame.Graphics.ResetDepthBias();
              int width = DuckGame.Graphics.width;
              int height = DuckGame.Graphics.height;
              DuckGame.Graphics.SetRenderTarget(DuckGame.Graphics.screenTarget);
              Viewport viewport = new Viewport();
              viewport.X = viewport.Y = 0;
              viewport.Width = DuckGame.Graphics.screenTarget.width;
              viewport.Height = DuckGame.Graphics.screenTarget.height;
              viewport.MinDepth = 0.0f;
              viewport.MaxDepth = 1f;
              DuckGame.Graphics.viewport = viewport;
              DuckGame.Graphics.width = DuckGame.Graphics.screenTarget.width;
              DuckGame.Graphics.height = DuckGame.Graphics.screenTarget.height;
              HUD.hide = true;
              Level.DrawCurrentLevel();
              DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
              this.OnDraw();
              DuckGame.Graphics.screen.End();
              HUD.hide = false;
              DuckGame.Graphics.width = width;
              DuckGame.Graphics.height = height;
              DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
              Resolution.FullViewport();
              Resolution.ResetViewport();
              DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
              DuckGame.Graphics.Draw((Tex2D) DuckGame.Graphics.screenTarget, Vec2.Zero, new Rectangle?(), Color.White, 0.0f, Vec2.Zero, Vec2.One, SpriteEffects.None);
              DuckGame.Graphics.screen.End();
            }
            else
            {
              Resolution.FullViewport();
              Resolution.ResetViewport();
              DuckGame.Graphics.ResetDepthBias();
              bool flag = true;
              if (Network.isActive && (!(Level.current is TeamSelect2) || !(Level.current as TeamSelect2).MatchmakerOpen()))
                flag = false;
              if (MonoMain._pauseMenu != null && MonoMain._didPauseCapture && DuckGame.Graphics.screenCapture == null)
              {
                DuckGame.Graphics.Clear(Color.Black * DuckGame.Graphics.fade);
                if (MonoMain.autoPauseFade)
                {
                  MonoMain._pauseMaterial.fade = Lerp.FloatSmooth(MonoMain._pauseMaterial.fade, MonoMain.doPauseFade ? 0.6f : 0.0f, 0.1f, 1.1f);
                  MonoMain._pauseMaterial.dim = Lerp.FloatSmooth(MonoMain._pauseMaterial.dim, MonoMain.doPauseFade ? 0.6f : 1f, 0.1f, 1.1f);
                }
                Vec2 scale = new Vec2(Layer.HUD.camera.width / (float) MonoMain._screenCapture.width, Layer.HUD.camera.height / (float) MonoMain._screenCapture.height);
                DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, (MTEffect) null, Layer.HUD.camera.getMatrix());
                DuckGame.Graphics.material = (Material) MonoMain._pauseMaterial;
                DuckGame.Graphics.Draw((Tex2D) MonoMain._screenCapture, new Vec2(0.0f, 0.0f), new Rectangle?(), new Color(120, 120, 120), 0.0f, Vec2.Zero, scale, SpriteEffects.None, new Depth(-0.9f));
                DuckGame.Graphics.material = (Material) null;
                DuckGame.Graphics.screen.End();
                Layer.HUD.Begin(true);
                MonoMain._pauseMenu.Draw();
                foreach (Thing thing in MonoMain.closeMenuUpdate)
                  thing.Draw();
                HUD.Draw();
                Layer.HUD.End(true);
                Layer.Console.Begin(true);
                DevConsole.Draw();
                if (Level.current != null && Level.current._netDebug != null)
                  Level.current._netDebug.Draw(Network.activeNetwork);
                Level.current.PostDrawLayer(Layer.Console);
                Layer.Console.End(true);
                if (!flag)
                  MonoMain._didPauseCapture = false;
              }
              else
              {
                if (MonoMain.autoPauseFade)
                {
                  MonoMain._pauseMaterial.fade = 0.0f;
                  MonoMain._pauseMaterial.dim = 0.6f;
                }
                DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
                Level.DrawCurrentLevel();
                DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, Resolution.getTransformationMatrix());
                this.OnDraw();
                DuckGame.Graphics.screen.End();
                if (MonoMain.closeMenuUpdate.Count > 0)
                {
                  Layer.HUD.Begin(true);
                  foreach (Thing thing in MonoMain.closeMenuUpdate)
                    thing.DoDraw();
                  Layer.HUD.End(true);
                }
              }
              base.Draw(gameTime);
            }
          }
        }
      }
    }

    protected virtual void OnDraw()
    {
    }
  }
}
