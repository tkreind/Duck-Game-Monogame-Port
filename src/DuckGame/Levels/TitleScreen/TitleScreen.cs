// Decompiled with JetBrains decompiler
// Type: DuckGame.TitleScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class TitleScreen : Level
  {
    public List<StarParticle> particles = new List<StarParticle>();
    private int wait;
    private float dim = 0.8f;
    private float moveSpeed = 1f;
    private float moveWait = 1f;
    private float flash;
    private TitleMenuSelection _selection = TitleMenuSelection.Play;
    private TitleMenuSelection _desiredSelection = TitleMenuSelection.Play;
    private BigTitle _title;
    private BitmapFont _font;
    private Sprite _background;
    private Sprite _optionsPlatform;
    private Sprite _rightPlatform;
    private Sprite _leftPlatform;
    private Sprite _beamPlatform;
    private Sprite _upperMonitor;
    private Sprite _optionsTV;
    private Sprite _libraryBookcase;
    private Sprite _bigUButton;
    private Sprite _airlock;
    private SpriteMap _controls;
    private Sprite _starField;
    public int roundsPerSet = 8;
    public int setsPerGame = 3;
    private SpaceBackgroundMenu _space;
    private bool _fadeIn;
    private bool _fadeInFull;
    private float _pressStartBlink;
    private string _selectionText = "";
    private string _selectionTextDesired = "MULTIPLAYER";
    private float _selectionFade = 1f;
    private int _controlsFrame;
    private int _controlsFrameDesired = 1;
    private float _controlsFade = 1f;
    private OptionsBeam _optionsBeam;
    private LibraryBeam _libraryBeam;
    private MultiBeam _multiBeam;
    private EditorBeam _editorBeam;
    private Duck _duck;
    private bool _enterMultiplayer;
    private MenuBoolean _quit = new MenuBoolean();
    private MenuBoolean _dontQuit = new MenuBoolean();
    private UIMenu _quitMenu;
    private UIMenu _optionsMenu;
    private UIMenu _controlConfigMenu;
    private UIMenu _graphicsMenu;
    private UIMenu _betaMenu;
    private UIMenu _modConfigMenu;
    private UIMenu _cloudConfigMenu;
    private UIMenu _cloudUploadConfirmMenu;
    private UIMenu _cloudDownloadConfirmMenu;
    private UIMenu _cloudDeleteConfirmMenu;
    private UIComponent _optionsGroup;
    private bool _enterEditor;
    private bool _enterCredits;
    private static bool _hasMenusOpen = false;
    public static bool modsChanged = false;
    private static bool firstStart = true;
    public List<List<string>> creditsRoll = new List<List<string>>();
    private bool _fadeBackground;
    private bool _enterLibrary;
    private bool _enterBuyScreen;
    private bool _startedMusic;
    private float starWait;
    private float switchWait = 1f;
    private float creditsScroll;
    private bool startStars = true;
    private int cpick;

    public TitleScreen() => this._centeredView = true;

    public bool menuOpen => Options.menuOpen || this._enterMultiplayer;

    private void CloudUpload() => DuckFile.UploadAllCloudData();

    private void CloudDownload() => DuckFile.DownloadAllCloudData();

    private void CloudDelete() => DuckFile.DeleteAllCloudData();

    public static bool hasMenusOpen => TitleScreen._hasMenusOpen;

    private void AddCreditLine(params string[] s) => this.creditsRoll.Add(new List<string>((IEnumerable<string>) s));

    public override void Initialize()
    {
      this.AddCreditLine("DUCK GAME");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@COMMUNITY HERO@RWINGGRAY@");
      this.AddCreditLine("John \"BroDuck\" Pichardo");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@LEAD TESTERS@RWINGGRAY@");
      this.AddCreditLine("Jacob Paul");
      this.AddCreditLine("Tyler Molz");
      this.AddCreditLine("Andrew Morrish");
      this.AddCreditLine("Dayton McKay");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@TESTERS@RWINGGRAY@");
      this.AddCreditLine("John Pichardo", "Lotad");
      this.AddCreditLine("Tufukins", "Sleepy Jirachi");
      this.AddCreditLine("Paul Hartling", "thebluecosmonaut");
      this.AddCreditLine("Dan Gaechter", "James Nieves");
      this.AddCreditLine("Dr. Docter", "svennieke");
      this.AddCreditLine("JadeFlames", "RealGnomeTasty");
      this.AddCreditLine("Karim Aifi", "Zaahck");
      this.AddCreditLine("dino rex (guy)", "Peter Smith");
      this.AddCreditLine("Colin Jacobson", "mage legend");
      this.AddCreditLine("YvngXero", "Trevor Etzold");
      this.AddCreditLine("Fluury", "Phantom329");
      this.AddCreditLine("Kevin Duffy", "Michael Niemann");
      this.AddCreditLine("Ben");
      this.AddCreditLine("James \"Sunder\" Beliakoff");
      this.AddCreditLine("David Sabosky (SidDaSloth)");
      this.AddCreditLine("Jordan \"Renim\" Gauge");
      this.AddCreditLine("Tommaso \"Giampiero\" Bresciani");
      this.AddCreditLine("Nicodemo \"Nikkodemus\" Bresciani");
      this.AddCreditLine("Valentin Zeyfang (RedMser)");
      this.AddCreditLine("Luke Bromley (mrred55)");
      this.AddCreditLine("");
      this.AddCreditLine("Dord");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@BETA TESTERS@RWINGGRAY@");
      this.AddCreditLine("oko", "Julian Cortinas");
      this.AddCreditLine("YoloCrayolo3", "VirtualFishbowl");
      this.AddCreditLine("Adam Benali", "Evilpie");
      this.AddCreditLine("Jonkki", "Hunter Armentrout");
      this.AddCreditLine("Owain Bolt", "Dallas Ball");
      this.AddCreditLine("Banjo Ward", "Lachlan Marr");
      this.AddCreditLine("Keith \"killerspecialk\" Santee");
      this.AddCreditLine("", "");
      this.AddCreditLine("", "");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@A DUCK GAME COSPLAYER@RWINGGRAY@");
      this.AddCreditLine("Colin Lamb");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@SLEEPY AND FRIENDS@RWINGGRAY@");
      this.AddCreditLine("Lotad");
      this.AddCreditLine("Sleepy Jirachi");
      this.AddCreditLine("Silverlace");
      this.AddCreditLine("Slimy");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@FEATHERS WILL FLY CREW@RWINGGRAY@");
      this.AddCreditLine("Dan \"lucidinertia\" Myszak");
      this.AddCreditLine("Yannick \"Becer\" Marcotte-Gourde");
      this.AddCreditLine("Aleksander \"Acrimonious Defect\" K.D.");
      this.AddCreditLine("The Entire FWF Community!");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@RUSS MONEY@RWINGGRAY@");
      this.AddCreditLine("AS HIMSELF");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("DEVELOPMENT TEAM");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@ART, PROGRAMMING, MUSIC@RWINGGRAY@");
      this.AddCreditLine("Landon Podbielski");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@ROOM FURNITURE ART@RWINGGRAY@");
      this.AddCreditLine("Dayton McKay");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@NEW HAT ART@RWINGGRAY@");
      this.AddCreditLine("Dayton McKay");
      this.AddCreditLine("");
      this.AddCreditLine("|CREDITSGRAY|@LWINGGRAY@MOD SUPPORT PROGRAMMER@RWINGGRAY@");
      this.AddCreditLine("Paril");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("Thank you OUYA for publishing");
      this.AddCreditLine("the original version of Duck Game.");
      this.AddCreditLine("Especially Bob Mills, who");
      this.AddCreditLine("made it all happen.");
      this.AddCreditLine("");
      this.AddCreditLine("We need to go camping again.");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("Thank you ADULT SWIM GAMES");
      this.AddCreditLine("for publishing Duck Game, and");
      this.AddCreditLine("for doing so much promotion and");
      this.AddCreditLine("testing.");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("Seriously, thank you Paril for");
      this.AddCreditLine("writing the mod support for Duck Game.");
      this.AddCreditLine("Mods wouldn't have been possible");
      this.AddCreditLine("without you.");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("BroDuck you've been a huge help");
      this.AddCreditLine("keeping the community running,");
      this.AddCreditLine("I don't know what would have happened");
      this.AddCreditLine("without your help.");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("");
      this.AddCreditLine("Thank you everyone for playing");
      this.AddCreditLine("Duck Game, for all your support,");
      this.AddCreditLine("and for being so kind.");
      if (!DG.InitializeDRM())
      {
        Level.current = (Level) new BetaScreen();
      }
      else
      {
        this._starField = new Sprite("background/starField");
        TeamSelect2.DefaultSettings();
        Network.Disconnect();
        if (Music.currentSong != "Title" && Music.currentSong != "TitleDemo" || Music.finished)
          Music.Play("Title");
        if (GameMode.playedGame)
          GameMode.playedGame = false;
        this._optionsGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
        this._optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST @QUACK@EXIT");
        this._controlConfigMenu = (UIMenu) new UIControlConfig(this._optionsMenu, "@WRENCH@DEVICE DEFAULTS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 194f, conString: "@DPAD@@SELECT@ADJUST @QUACK@BACK");
        this._graphicsMenu = new UIMenu("@WRENCH@GRAPHICS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST @QUACK@EXIT");
        this._graphicsMenu.Add((UIComponent) new UIMenuItemToggle("FIRE GLOW", field: new FieldBinding((object) Options.Data, "fireGlow")), true);
        this._graphicsMenu.Add((UIComponent) new UIMenuItemToggle("LIGHTING", field: new FieldBinding((object) Options.Data, "lighting")), true);
        this._graphicsMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._graphicsMenu, (UIComponent) this._optionsMenu), backButton: true), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItemSlider("SFX VOLUME", field: new FieldBinding((object) Options.Data, "sfxVolume"), step: 0.125f), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItemSlider("MUSIC VOLUME", field: new FieldBinding((object) Options.Data, "musicVolume"), step: 0.125f), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItemToggle("SHENANIGANS", field: new FieldBinding((object) Options.Data, "shennanigans")), true);
        this._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItemToggle("FULLSCREEN", field: new FieldBinding((object) Options.Data, "fullscreen")), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItem("GRAPHICS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._optionsMenu, (UIComponent) this._graphicsMenu), backButton: true), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItem("EDIT CONTROLS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._optionsMenu, (UIComponent) this._controlConfigMenu), backButton: true), true);
        if (MonoMain.moddingEnabled)
        {
          this._modConfigMenu = (UIMenu) new UIModManagement(this._optionsMenu, "@WRENCH@MANAGE MODS@SCREWDRIVER@", Layer.HUD.camera.width, Layer.HUD.camera.height, 550f, conString: "@DPAD@@SELECT@ADJUST @SHOOT@TOGGLE @QUACK@BACK");
          this._cloudConfigMenu = new UIMenu("@WRENCH@CLOUD@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@DPAD@ADJUST @QUACK@EXIT");
          this._cloudUploadConfirmMenu = new UIMenu("UPLOAD DATA?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@SELECT@SELECT");
          this._cloudDownloadConfirmMenu = new UIMenu("DOWNLOAD DATA?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@SELECT@SELECT");
          this._cloudDeleteConfirmMenu = new UIMenu("DELETE DATA?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 280f, conString: "@SELECT@SELECT");
          this._cloudConfigMenu.Add((UIComponent) new UIText("If enabled, Steam Cloud will", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("automatically keep your Duck Game", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("save data synchronized", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("between computers.", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText(" ", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIMenuItemToggle("USE CLOUD", field: new FieldBinding((object) Options.Data, "cloud")), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText(" ", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("If disabled, you can", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("manage cloud data manually.", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("Note that local save data", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("is uploaded automatically", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText("even if 'use cloud' is disabled!", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIText(" ", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIMenuItem("UPLOAD", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudConfigMenu, (UIComponent) this._cloudUploadConfirmMenu), backButton: true), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText("This will replace all data", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText("(Profiles, Options, Levels)", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText("in your duck game cloud with the", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText("current files in your", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText("documents folder!", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIText(" ", Color.Red), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudUploadConfirmMenu, (UIComponent) this._cloudConfigMenu)), true);
          this._cloudUploadConfirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionOpenMenuCallFunction((UIComponent) this._cloudUploadConfirmMenu, (UIComponent) this._cloudConfigMenu, new UIMenuActionOpenMenuCallFunction.Function(this.CloudUpload))), true);
          this._cloudUploadConfirmMenu.Close();
          this._cloudConfigMenu.Add((UIComponent) new UIMenuItem("DOWNLOAD", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudConfigMenu, (UIComponent) this._cloudDownloadConfirmMenu), backButton: true), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText("This will replace all files", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText("(Profiles, Options, Levels)", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText("in your documents folder", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText("with the current data in your", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText("duck game cloud!", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIText(" ", Color.Red), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudDownloadConfirmMenu, (UIComponent) this._cloudConfigMenu)), true);
          this._cloudDownloadConfirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionOpenMenuCallFunction((UIComponent) this._cloudDownloadConfirmMenu, (UIComponent) this._cloudConfigMenu, new UIMenuActionOpenMenuCallFunction.Function(this.CloudDownload))), true);
          this._cloudDownloadConfirmMenu.Close();
          this._cloudConfigMenu.Add((UIComponent) new UIMenuItem("DELETE", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudConfigMenu, (UIComponent) this._cloudDeleteConfirmMenu), backButton: true), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText("This will DELETE all files", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText("(Profiles, Options, Levels)", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText("from your duck game cloud!", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText("Do not do this, unless you're", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText("absolutely sure!", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIText(" ", Color.Red), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudDeleteConfirmMenu, (UIComponent) this._cloudConfigMenu)), true);
          this._cloudDeleteConfirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionOpenMenuCallFunction((UIComponent) this._cloudDeleteConfirmMenu, (UIComponent) this._cloudConfigMenu, new UIMenuActionOpenMenuCallFunction.Function(this.CloudDelete))), true);
          this._cloudDeleteConfirmMenu.Close();
          this._cloudConfigMenu.Add((UIComponent) new UIText(" ", Colors.DGBlue), true);
          this._cloudConfigMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._cloudConfigMenu, (UIComponent) this._optionsMenu), backButton: true), true);
          this._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
          this._optionsMenu.Add((UIComponent) new UIMenuItem("MANAGE MODS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._optionsMenu, (UIComponent) this._modConfigMenu), backButton: true), true);
          this._optionsMenu.Add((UIComponent) new UIMenuItem("STEAM CLOUD", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._optionsMenu, (UIComponent) this._cloudConfigMenu), backButton: true), true);
        }
        this._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        this._optionsMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(this._optionsGroup, new UIMenuActionCloseMenuCallFunction.Function(Options.OptionsMenuClosed)), backButton: true), true);
        this._optionsMenu.Close();
        this._optionsGroup.Add((UIComponent) this._optionsMenu, false);
        this._controlConfigMenu.Close();
        if (MonoMain.moddingEnabled)
        {
          this._modConfigMenu.Close();
          this._cloudConfigMenu.Close();
        }
        this._optionsGroup.Add((UIComponent) this._controlConfigMenu, false);
        this._optionsGroup.Add((UIComponent) (this._controlConfigMenu as UIControlConfig)._confirmMenu, false);
        this._optionsGroup.Add((UIComponent) this._graphicsMenu, false);
        if (MonoMain.moddingEnabled)
        {
          this._optionsGroup.Add((UIComponent) this._modConfigMenu, false);
          this._optionsGroup.Add((UIComponent) (this._modConfigMenu as UIModManagement)._editModMenu, false);
          this._optionsGroup.Add((UIComponent) (this._modConfigMenu as UIModManagement)._yesNoMenu, false);
          this._optionsGroup.Add((UIComponent) this._cloudConfigMenu, false);
          this._optionsGroup.Add((UIComponent) this._cloudUploadConfirmMenu, false);
          this._optionsGroup.Add((UIComponent) this._cloudDownloadConfirmMenu, false);
          this._optionsGroup.Add((UIComponent) this._cloudDeleteConfirmMenu, false);
        }
        this._optionsGroup.Close();
        Level.Add((Thing) this._optionsGroup);
        this._betaMenu = new UIMenu("@WRENCH@WELCOME TO BETA!@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@QUACK@OK!");
        this._betaMenu.Add((UIComponent) new UIImage(new Sprite("message"), UIAlign.Center, 0.25f, 51f), true);
        this._betaMenu.Close();
        this._betaMenu._backButton = new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionCloseMenu((UIComponent) this._betaMenu), backButton: true);
        this._betaMenu._isMenu = true;
        Level.Add((Thing) this._betaMenu);
        this._quitMenu = new UIMenu("REALLY QUIT?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
        this._quitMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean((UIComponent) this._quitMenu, this._dontQuit)), true);
        this._quitMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean((UIComponent) this._quitMenu, this._quit)), true);
        this._quitMenu.Close();
        Level.Add((Thing) this._quitMenu);
        this._font = new BitmapFont("biosFont", 8);
        this._background = new Sprite("title/background");
        this._optionsPlatform = new Sprite("title/optionsPlatform");
        this._optionsPlatform.depth = new Depth(0.9f);
        this._rightPlatform = new Sprite("title/rightPlatform");
        this._rightPlatform.depth = new Depth(0.9f);
        this._beamPlatform = new Sprite("title/beamPlatform");
        this._beamPlatform.depth = new Depth(0.9f);
        this._upperMonitor = new Sprite("title/upperMonitor");
        this._upperMonitor.depth = new Depth(0.85f);
        this._airlock = new Sprite("title/airlock");
        this._airlock.depth = new Depth(-0.85f);
        this._leftPlatform = new Sprite("title/leftPlatform");
        this._leftPlatform.depth = new Depth(0.9f);
        this._optionsTV = new Sprite("title/optionsTV");
        this._optionsTV.depth = new Depth(-0.9f);
        this._libraryBookcase = new Sprite("title/libraryBookcase");
        this._libraryBookcase.depth = new Depth(-0.9f);
        this._bigUButton = new Sprite("title/bigUButtonPC");
        this._bigUButton.CenterOrigin();
        this._bigUButton.depth = new Depth(0.95f);
        this._controls = new SpriteMap("title/controlsPC", 100, 11);
        this._controls.CenterOrigin();
        this._controls.depth = new Depth(0.95f);
        this._multiBeam = new MultiBeam(160f, -30f);
        Level.Add((Thing) this._multiBeam);
        this._optionsBeam = new OptionsBeam(28f, -110f);
        Level.Add((Thing) this._optionsBeam);
        this._libraryBeam = new LibraryBeam(292f, -110f);
        Level.Add((Thing) this._libraryBeam);
        this._editorBeam = new EditorBeam(292f, 100f);
        Level.Add((Thing) this._editorBeam);
        for (int index = 0; index < 18; ++index)
        {
          SpaceTileset spaceTileset = new SpaceTileset((float) (index * 16 - 6), 176f);
          spaceTileset.frame = 3;
          if (index == 17)
            spaceTileset.frame = 4;
          spaceTileset.layer = Layer.Game;
          spaceTileset.setLayer = false;
          this.AddThing((Thing) spaceTileset);
        }
        SpriteMap spriteMap = new SpriteMap("duck", 32, 32);
        this._space = new SpaceBackgroundMenu(-999f, -999f, true, 0.6f);
        this._space.update = false;
        Level.Add((Thing) this._space);
        this._things.RefreshState();
        Layer.Game.fade = 0.0f;
        Layer.Foreground.fade = 0.0f;
        Level.Add((Thing) new Block(120f, 155f, 80f, 30f, PhysicsMaterial.Metal));
        Level.Add((Thing) new Block(134f, 148f, 52f, 30f, PhysicsMaterial.Metal));
        Level.Add((Thing) new Block(0.0f, 61f, 63f, 70f, PhysicsMaterial.Metal));
        Level.Add((Thing) new Block(257f, 61f, 63f, 60f, PhysicsMaterial.Metal));
        Level.Add((Thing) new Spring(90f, 160f, 0.3f));
        Level.Add((Thing) new Spring(229f, 160f, 0.32f));
        Level.Add((Thing) new VerticalDoor(51f, 160f));
        foreach (Team team in Teams.all)
          team.prevScoreboardScore = team.score = 0;
        foreach (Profile prof in Profiles.all)
        {
          if (prof.team != null)
            prof.team.Leave(prof);
          prof.inputProfile = (InputProfile) null;
        }
        Teams.Player1.Join(Profiles.DefaultPlayer1);
        Profiles.DefaultPlayer1.inputProfile = InputProfile.Get("MPPlayer1");
        Teams.Player2.Join(Profiles.DefaultPlayer2);
        Profiles.DefaultPlayer2.inputProfile = InputProfile.Get("MPPlayer2");
        Teams.Player3.Join(Profiles.DefaultPlayer3);
        Profiles.DefaultPlayer3.inputProfile = InputProfile.Get("MPPlayer3");
        Teams.Player4.Join(Profiles.DefaultPlayer4);
        Profiles.DefaultPlayer4.inputProfile = InputProfile.Get("MPPlayer4");
        if (!DuckNetwork.ShowUserXPGain() && Unlockables.HasPendingUnlocks())
          MonoMain.pauseMenu = (UIComponent) new UIUnlockBox(Unlockables.GetPendingUnlocks().ToList<Unlockable>(), Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f);
        if (MonoMain.browseCloud)
        {
          Layer.HUD.camera.width *= 2f;
          Layer.HUD.camera.height *= 2f;
          MonoFileDialog monoFileDialog = new MonoFileDialog();
          Level.Add((Thing) monoFileDialog);
          monoFileDialog.Open(DuckFile.saveDirectory, "", false, loadLevel: false, type: ContextFileType.All);
        }
        base.Initialize();
      }
    }

    public override void OnSessionEnded(DuckNetErrorInfo error)
    {
      Teams.Player1.Join(Profiles.DefaultPlayer1);
      Profiles.DefaultPlayer1.inputProfile = InputProfile.Get("MPPlayer1");
      Teams.Player2.Join(Profiles.DefaultPlayer2);
      Profiles.DefaultPlayer2.inputProfile = InputProfile.Get("MPPlayer2");
      Teams.Player3.Join(Profiles.DefaultPlayer3);
      Profiles.DefaultPlayer3.inputProfile = InputProfile.Get("MPPlayer3");
      Teams.Player4.Join(Profiles.DefaultPlayer4);
      Profiles.DefaultPlayer4.inputProfile = InputProfile.Get("MPPlayer4");
    }

    public override void Update()
    {
      int num = 1;
      if (this.startStars)
      {
        num = 250;
        this.startStars = false;
      }
      for (int index = 0; index < num; ++index)
      {
        this.starWait -= Maths.IncFrameTimer();
        if ((double) this.starWait < 0.0)
        {
          this.starWait = 0.1f + Rando.Float(0.2f);
          Color color = Colors.DGRed;
          if (this.cpick == 1)
            color = Colors.DGBlue;
          else if (this.cpick == 2)
            color = Colors.DGGreen;
          if ((double) Rando.Float(1f) > 0.995000004768372)
            color = Colors.DGPink;
          this.particles.Add(new StarParticle()
          {
            pos = new Vec2(0.0f, (float) (int) ((double) Rando.Float(0.0f, 150f) / 1.0)),
            speed = new Vec2(Rando.Float(0.5f, 1f), 0.0f),
            color = color,
            flicker = Rando.Float(100f, 230f)
          });
          ++this.cpick;
          if (this.cpick > 2)
            this.cpick = 0;
        }
        List<StarParticle> starParticleList = new List<StarParticle>();
        foreach (StarParticle particle in this.particles)
        {
          particle.pos += particle.speed;
          if ((double) particle.pos.x > 300.0 && !this._enterCredits || (double) particle.pos.x > 680.0)
            starParticleList.Add(particle);
        }
        foreach (StarParticle starParticle in starParticleList)
          this.particles.Remove(starParticle);
      }
      if (this._enterCredits)
      {
        if ((double) this.camera.x < 100.0)
        {
          this.flashDissipationSpeed = 0.08f;
          Graphics.flashAdd = 2f;
          this.camera.x += 320f;
          foreach (StarParticle particle in this.particles)
            particle.pos.x += 320f;
        }
        else
        {
          this.switchWait -= 0.04f;
          if ((double) this.switchWait > 0.0)
            return;
          if (!this._startedMusic)
            Music.volumeMult = Lerp.Float(Music.volumeMult, 0.0f, 3f / 500f);
          if ((double) Layer.Parallax.camera.y > -22.0)
          {
            this.camera.y += 0.064f;
            Layer.Parallax.camera.y -= 0.08f;
          }
          else
          {
            if (!this._startedMusic)
            {
              Music.volumeMult = 1.2f;
              Music.Play("tabledoodles", false);
              this._startedMusic = true;
            }
            this.creditsScroll += 0.15f;
            if (!Input.Down("JUMP"))
              return;
            this.creditsScroll += 0.2f;
          }
        }
      }
      else
      {
        TitleScreen._hasMenusOpen = this.menuOpen;
        if (!this._enterMultiplayer && !this._enterEditor && (!this._enterLibrary && !this._enterBuyScreen))
        {
          if ((double) Graphics.fade < 1.0)
            Graphics.fade += 1f / 1000f;
          else
            Graphics.fade = 1f;
        }
        else
        {
          Graphics.fade -= 0.05f;
          if ((double) Graphics.fade <= 0.0)
          {
            Graphics.fade = 0.0f;
            Music.Stop();
            if (this._enterMultiplayer)
            {
              foreach (Team team in Teams.all)
                team.ClearProfiles();
              Level.current = (Level) new TeamSelect2();
            }
            else if (this._enterEditor)
              Level.current = (Level) Main.editor;
            else if (this._enterLibrary)
              Level.current = (Level) new DoorRoom();
            else if (this._enterBuyScreen)
              Level.current = (Level) new BuyScreen(Main.currencyType, Main.price);
          }
        }
        this._pressStartBlink += 0.01f;
        if ((double) this._pressStartBlink > 1.0)
          --this._pressStartBlink;
        if (this._duck != null)
        {
          if (this._dontQuit.value)
          {
            this._dontQuit.value = false;
            this._duck.hSpeed = 10f;
          }
          if (this._quit.value)
          {
            MonoMain.exit = true;
            return;
          }
          if ((double) this._duck.x < 30.0 && (double) this._duck.y > 100.0 && (double) this._duck.hSpeed < 0.0)
          {
            MonoMain.pauseMenu = (UIComponent) this._quitMenu;
            this._quitMenu.Open();
            return;
          }
          if (InputProfile.active.Pressed("START") && Main.foundPurchaseInfo && Main.isDemo)
          {
            this._enterBuyScreen = true;
            this._duck.immobilized = true;
          }
        }
        if (this._multiBeam.entered)
        {
          this._selectionTextDesired = "MULTIPLAYER";
          this._desiredSelection = TitleMenuSelection.Play;
          if (!this._enterMultiplayer && InputProfile.active.Pressed("SELECT"))
          {
            SFX.Play("plasmaFire");
            this._enterMultiplayer = true;
            this._duck.immobilized = true;
          }
        }
        else if (this._optionsBeam.entered)
        {
          this._selectionTextDesired = "OPTIONS";
          this._desiredSelection = TitleMenuSelection.Options;
          if (!Options.menuOpen && InputProfile.active.Pressed("SELECT"))
          {
            SFX.Play("plasmaFire");
            this._optionsGroup.Open();
            this._optionsMenu.Open();
            MonoMain.pauseMenu = this._optionsGroup;
            this._duck.immobilized = true;
          }
        }
        else if (this._libraryBeam.entered)
        {
          this._selectionTextDesired = "LIBRARY";
          this._desiredSelection = TitleMenuSelection.Stats;
          if (InputProfile.active.Pressed("SELECT"))
          {
            SFX.Play("plasmaFire");
            this._enterLibrary = true;
            this._duck.immobilized = true;
          }
        }
        else if (this._editorBeam.entered)
        {
          this._selectionTextDesired = "LEVEL EDITOR";
          this._desiredSelection = TitleMenuSelection.Editor;
          if (InputProfile.active.Pressed("SELECT"))
          {
            SFX.Play("plasmaFire");
            this._enterEditor = true;
            this._duck.immobilized = true;
          }
        }
        else
        {
          this._selectionTextDesired = " ";
          this._desiredSelection = TitleMenuSelection.None;
        }
        this._controlsFrameDesired = !(this._selectionTextDesired != " ") ? 2 : 1;
        if (this._selectionText != this._selectionTextDesired)
        {
          this._selectionFade -= 0.1f;
          if ((double) this._selectionFade <= 0.0)
          {
            this._selectionFade = 0.0f;
            this._selectionText = this._selectionTextDesired;
            this._selection = this._desiredSelection;
          }
        }
        else
          this._selectionFade = Lerp.Float(this._selectionFade, 1f, 0.1f);
        if (this._controlsFrame != this._controlsFrameDesired)
        {
          this._controlsFade -= 0.1f;
          if ((double) this._controlsFade <= 0.0)
          {
            this._controlsFade = 0.0f;
            this._controlsFrame = this._controlsFrameDesired;
          }
        }
        else
          this._controlsFade = Lerp.Float(this._controlsFade, 1f, 0.1f);
        if (this.menuOpen)
        {
          Layer.Game.fade = Lerp.Float(Layer.Game.fade, 0.2f, 0.02f);
          Layer.Foreground.fade = Lerp.Float(Layer.Foreground.fade, 0.2f, 0.02f);
          Layer.Background.fade = Lerp.Float(Layer.Foreground.fade, 0.2f, 0.02f);
        }
        else
        {
          Layer.Game.fade = Lerp.Float(Layer.Game.fade, this._fadeInFull ? 1f : (this._fadeIn ? 0.5f : 0.0f), this._fadeInFull ? 0.01f : 3f / 500f);
          Layer.Foreground.fade = Lerp.Float(Layer.Foreground.fade, this._fadeIn ? 1f : 0.0f, 0.01f);
          Layer.Background.fade = Lerp.Float(Layer.Background.fade, this._fadeBackground ? 0.0f : 1f, 0.02f);
        }
        if (this._fadeIn && !this._fadeInFull)
        {
          this._duck = (Duck) null;
          if (InputProfile.Get("MPPlayer1").Pressed("START"))
          {
            Profile pro = Profiles.all.First<Profile>((Func<Profile, bool>) (p => p.team != null && p.persona == Persona.Duck1));
            if (pro == null)
            {
              Teams.Player1.Join(Profiles.DefaultPlayer1);
              pro = Profiles.DefaultPlayer1;
            }
            this._duck = new Duck(160f, 60f, pro);
          }
          else if (InputProfile.Get("MPPlayer2").Pressed("START"))
          {
            Profile pro = Profiles.all.First<Profile>((Func<Profile, bool>) (p => p.team != null && p.persona == Persona.Duck2));
            if (pro == null)
            {
              Teams.Player1.Join(Profiles.DefaultPlayer2);
              pro = Profiles.DefaultPlayer2;
            }
            this._duck = new Duck(160f, 60f, pro);
          }
          else if (InputProfile.Get("MPPlayer3").Pressed("START"))
          {
            Profile pro = Profiles.all.First<Profile>((Func<Profile, bool>) (p => p.team != null && p.persona == Persona.Duck3));
            if (pro == null)
            {
              Teams.Player1.Join(Profiles.DefaultPlayer3);
              pro = Profiles.DefaultPlayer3;
            }
            this._duck = new Duck(160f, 60f, pro);
          }
          else if (InputProfile.Get("MPPlayer4").Pressed("START"))
          {
            Profile pro = Profiles.all.First<Profile>((Func<Profile, bool>) (p => p.team != null && p.persona == Persona.Duck4));
            if (pro == null)
            {
              Teams.Player1.Join(Profiles.DefaultPlayer4);
              pro = Profiles.DefaultPlayer4;
            }
            this._duck = new Duck(160f, 60f, pro);
          }
          if (this._duck != null)
          {
            if (Main.foundPurchaseInfo && Main.isDemo)
              HUD.AddCornerControl(HUDCorner.TopRight, "@START@BUY GAME", this._duck.inputProfile);
            InputProfile.active = this._duck.profile.inputProfile;
            this._fadeInFull = true;
            this._title.fade = true;
            Level.Add((Thing) this._duck);
          }
        }
        this._space.parallax.y = -80f;
        this.moveWait -= 0.02f;
        if ((double) this.moveWait < 0.0)
        {
          if (this._title == null)
          {
            this._title = new BigTitle();
            this._title.x = (float) ((double) Layer.HUD.camera.width / 2.0 - (double) (this._title.graphic.w / 2) + 3.0);
            this._title.y = Layer.HUD.camera.height / 2f;
            this.AddThing((Thing) this._title);
          }
          this.moveSpeed = Maths.LerpTowards(this.moveSpeed, 0.0f, 0.0015f);
        }
        if (this._title == null)
          return;
        ++this.wait;
        int wait = this.wait;
        if (this.wait == 60)
          this.flash = 1f;
        if (this.wait == 60)
        {
          this._title.graphic.color = Color.White;
          this._title.alpha = 1f;
          this._fadeIn = true;
        }
        if ((double) this.flash > 0.0)
        {
          this.flash -= 0.016f;
          this.dim -= 0.08f;
          if ((double) this.dim >= 0.0)
            return;
          this.dim = 0.0f;
        }
        else
          this.flash = 0.0f;
      }
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.Foreground)
      {
        Graphics.Draw(this._upperMonitor, 84f, 0.0f);
        if (this._fadeInFull)
        {
          this._font.alpha = this._selectionFade;
          this._font.inputProfile = this._duck.inputProfile;
          if (this._selection == TitleMenuSelection.None)
          {
            string text = "@DPAD@MOVE @JUMP@JUMP";
            this._font.Draw(text, Level.current.camera.PercentW(50f) - this._font.GetWidth(text) / 2f, 16f, Color.White, new Depth(0.95f));
          }
          else if (this._selection == TitleMenuSelection.Play)
          {
            string text = "@SELECT@PLAY GAME";
            this._font.Draw(text, Level.current.camera.PercentW(50f) - this._font.GetWidth(text) / 2f, 16f, Color.White, new Depth(0.95f));
          }
          else if (this._selection == TitleMenuSelection.Stats)
          {
            string text = "@SELECT@LIBRARY";
            this._font.Draw(text, Level.current.camera.PercentW(50f) - this._font.GetWidth(text) / 2f, 16f, Color.White, new Depth(0.95f));
          }
          else if (this._selection == TitleMenuSelection.Options)
          {
            string text = "@SELECT@OPTIONS";
            this._font.Draw(text, Level.current.camera.PercentW(50f) - this._font.GetWidth(text) / 2f, 16f, Color.White, new Depth(0.95f));
          }
          else if (this._selection == TitleMenuSelection.Editor)
          {
            string text = "@SELECT@EDITOR";
            this._font.Draw(text, Level.current.camera.PercentW(50f) - this._font.GetWidth(text) / 2f, 16f, Color.White, new Depth(0.95f));
          }
        }
        else if ((double) this._pressStartBlink >= 0.5)
        {
          this._font.Draw("PRESS START", Level.current.camera.PercentW(50f) - this._font.GetWidth("PRESS START") / 2f, 15f, Color.White, new Depth(0.95f));
        }
        else
        {
          InputProfile profileWithDevice = InputProfile.FirstProfileWithDevice;
          if (profileWithDevice != null && profileWithDevice.lastActiveDevice != null && profileWithDevice.lastActiveDevice is GenericController)
            Graphics.Draw(this._bigUButton, Level.current.camera.PercentW(50f) - 1f, 18f);
          else
            Graphics.DrawString("@START@", new Vec2(Level.current.camera.PercentW(50f) - 7f, 16f), Color.White, new Depth(0.9f));
        }
      }
      else if (layer == Layer.Game)
      {
        Graphics.Draw(this._leftPlatform, 0.0f, 61f);
        Graphics.Draw(this._airlock, 0.0f, 135f);
        Graphics.Draw(this._rightPlatform, (float) byte.MaxValue, 61f);
        Graphics.Draw(this._beamPlatform, 118f, 146f);
        Graphics.Draw(this._optionsTV, 0.0f, 19f);
        Graphics.Draw(this._libraryBookcase, 263f, 12f);
        if ((double) this.creditsScroll > 0.100000001490116)
        {
          Graphics.caseSensitiveStringDrawing = true;
          float num = 0.0f;
          foreach (List<string> stringList in this.creditsRoll)
          {
            if (stringList.Count == 1)
            {
              float stringWidth = Graphics.GetStringWidth(stringList[0]);
              Graphics.DrawStringColoredSymbols(stringList[0], new Vec2((float) (320.0 + (160.0 - (double) stringWidth / 2.0)), num + (200f - this.creditsScroll)), Color.White, new Depth(1f));
            }
            else
            {
              double stringWidth1 = (double) Graphics.GetStringWidth(stringList[0]);
              Graphics.DrawStringColoredSymbols(stringList[0], new Vec2(337f, num + (200f - this.creditsScroll)), Color.White, new Depth(1f));
              double stringWidth2 = (double) Graphics.GetStringWidth(stringList[1]);
              Graphics.DrawStringColoredSymbols(stringList[1], new Vec2(497f, num + (200f - this.creditsScroll)), Color.White, new Depth(1f));
            }
            num += 11f;
          }
          Graphics.caseSensitiveStringDrawing = false;
        }
      }
      else if (layer == Layer.Parallax)
      {
        float num = 0.0f;
        if ((double) this.camera.y > 4.0)
        {
          this._starField.alpha = num + (float) (((double) this.camera.y - 4.0) / 13.0);
          Graphics.Draw(this._starField, 0.0f, layer.camera.y - 58f, new Depth(-0.99f));
        }
      }
      else if (layer == Layer.Background)
      {
        foreach (StarParticle particle in this.particles)
        {
          float num1 = Math.Max(1f - Math.Min(Math.Abs(particle.pos.x - particle.flicker) / 10f, 1f), 0.0f);
          float num2 = 0.2f;
          if ((double) this.camera.y > 0.0)
            num2 += this.camera.y / 52f;
          Graphics.DrawRect(particle.pos, particle.pos + new Vec2(1f, 1f), Color.White * (num2 + num1 * 0.6f), new Depth(-0.3f));
          float num3 = 0.1f;
          if ((double) this.camera.y > 0.0)
            num3 += this.camera.y / 52f;
          Vec2 pos = particle.pos;
          int num4 = 4;
          for (int index = 0; index < num4; ++index)
          {
            float num5 = particle.speed.x * 8f;
            Graphics.DrawLine(pos + new Vec2(-num5, 0.5f), pos + new Vec2(0.0f, 0.5f), particle.color * ((float) (1.0 - (double) index / (double) num4) * num3), depth: (new Depth(-0.4f)));
            pos.x -= num5;
          }
        }
        this._background.depth = new Depth(0.0f);
        Rectangle sourceRectangle = new Rectangle(0.0f, 0.0f, 90f, (float) this._background.height);
        Graphics.Draw(this._background, 0.0f, 0.0f, sourceRectangle);
        sourceRectangle = new Rectangle(63f, 107f, 194f, 61f);
        Graphics.Draw(this._background, sourceRectangle.x, sourceRectangle.y, sourceRectangle);
        sourceRectangle = new Rectangle(230f, 61f, 28f, 61f);
        Graphics.Draw(this._background, sourceRectangle.x, sourceRectangle.y, sourceRectangle);
        sourceRectangle = new Rectangle(230f, 0.0f, 90f, 61f);
        Graphics.Draw(this._background, sourceRectangle.x, sourceRectangle.y, sourceRectangle);
        sourceRectangle = new Rectangle(230f, 124f, 90f, 56f);
        Graphics.Draw(this._background, sourceRectangle.x, sourceRectangle.y, sourceRectangle);
        sourceRectangle = new Rectangle(90f, 0.0f, 140f, 50f);
        Graphics.Draw(this._background, sourceRectangle.x, sourceRectangle.y, sourceRectangle);
      }
      base.PostDrawLayer(layer);
    }
  }
}
