// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamSelect2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class TeamSelect2 : Level, IHaveAVirtualTransition
  {
    private float dim;
    public static bool fakeOnlineImmediately = false;
    public static int customLevels = 0;
    public static int prevCustomLevels = 0;
    public static bool startCountdown = false;
    private BitmapFont _font;
    private SpriteMap _countdown;
    private float _countTime = 1.5f;
    private List<ProfileBox2> _profiles = new List<ProfileBox2>();
    public static List<MatchSetting> matchSettings = new List<MatchSetting>()
    {
      new MatchSetting()
      {
        id = "requiredwins",
        name = "REQUIRED WINS",
        value = (object) 10,
        min = 5,
        max = 100,
        step = 5,
        stepMap = new Dictionary<int, int>()
        {
          {
            50,
            5
          },
          {
            100,
            10
          },
          {
            500,
            50
          },
          {
            1000,
            100
          }
        }
      },
      new MatchSetting()
      {
        id = "restsevery",
        name = "RESTS EVERY",
        value = (object) 10,
        min = 5,
        max = 100,
        step = 5,
        stepMap = new Dictionary<int, int>()
        {
          {
            50,
            5
          },
          {
            100,
            10
          },
          {
            500,
            50
          },
          {
            1000,
            100
          }
        }
      },
      new MatchSetting()
      {
        id = "wallmode",
        name = "WALL MODE",
        value = (object) false
      },
      new MatchSetting()
      {
        id = "normalmaps",
        name = "@NORMALICON@|DGBLUE|NORMAL MAPS",
        value = (object) 90,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "randommaps",
          "custommaps",
          "workshopmaps"
        }
      },
      new MatchSetting()
      {
        id = "randommaps",
        name = "@RANDOMICON@|DGBLUE|RANDOM MAPS",
        value = (object) 10,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "workshopmaps",
          "custommaps"
        }
      },
      new MatchSetting()
      {
        id = "custommaps",
        name = "@CUSTOMICON@|DGBLUE|CUSTOM MAPS",
        value = (object) 0,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "randommaps",
          "workshopmaps"
        }
      },
      new MatchSetting()
      {
        id = "workshopmaps",
        name = "@RAINBOWICON@|DGBLUE|INTERNET MAPS",
        value = (object) 0,
        suffix = "%",
        min = 0,
        max = 100,
        step = 5,
        percentageLinks = new List<string>()
        {
          "normalmaps",
          "custommaps",
          "randommaps"
        }
      }
    };
    public static List<MatchSetting> onlineSettings = new List<MatchSetting>()
    {
      new MatchSetting()
      {
        id = "maxplayers",
        name = "MAX PLAYERS",
        value = (object) 4,
        min = 2,
        max = 4,
        step = 1
      },
      new MatchSetting()
      {
        id = "teams",
        name = "TEAMS",
        value = (object) false
      },
      new MatchSetting()
      {
        id = "modifiers",
        name = "MODIFIERS",
        value = (object) false,
        filtered = true,
        filterOnly = true
      },
      new MatchSetting()
      {
        id = "type",
        name = "TYPE",
        value = (object) 2,
        min = 0,
        max = 2,
        createOnly = true,
        valueStrings = new List<string>()
        {
          "PRIVATE",
          "FRIENDS",
          "PUBLIC"
        }
      }
    };
    private SpriteMap _buttons;
    private bool _matchSetup;
    private float _setupFade;
    private bool _starting;
    public static int requiredWins = 10;
    public static int roundsBetweenIntermission = 10;
    public static bool userMapsOnly = false;
    public static bool enableRandom = false;
    public static bool randomMapsOnly = false;
    public static int randomMapPercent = 10;
    public static int normalMapPercent = 90;
    public static int workshopMapPercent = 0;
    public static bool partyMode = false;
    public static bool ctfMode = false;
    private static Dictionary<string, bool> _modifierStatus = new Dictionary<string, bool>();
    public static bool doCalc = false;
    public int setsPerGame = 3;
    private UIMenu _multiplayerMenu;
    private UIMenu _modifierMenu;
    private TeamBeam _beam;
    private Sprite _countdownScreen;
    private UIComponent _pauseGroup;
    private UIMenu _pauseMenu;
    private UIComponent _localPauseGroup;
    private UIMenu _localPauseMenu;
    private UIComponent _playOnlineGroup;
    private UIMenu _playOnlineMenu;
    private UIMenu _joinGameMenu;
    private UIMenu _filtersMenu;
    private UIMenu _filterModifierMenu;
    private UIMenu _hostGameMenu;
    private UIMenu _miniHostGameMenu;
    private UIMenu _hostGameSettingsMenu;
    private UIMenu _hostGameModifierMenu;
    private UIMenu _browseGamesMenu;
    private UIMatchmakingBox _matchmaker;
    private MenuBoolean _returnToMenu = new MenuBoolean();
    private MenuBoolean _inviteFriends = new MenuBoolean();
    private MenuBoolean _findGame = new MenuBoolean();
    private MenuBoolean _backOut = new MenuBoolean();
    private MenuBoolean _localBackOut = new MenuBoolean();
    private MenuBoolean _createGame = new MenuBoolean();
    private MenuBoolean _hostGame = new MenuBoolean();
    public bool openLevelSelect;
    private LevelSelect _levelSelector;
    private bool _rebuildPauseMenu;
    private UIMenu _inviteMenu;
    private UIComponent _configGroup;
    private UIMenu _levelSelectMenu;
    private BitmapFont _littleFont;
    private ProfileBox2 _whodid;
    private bool _singlePlayer;
    private int activePlayers;
    private static bool _attemptingToInvite = false;
    private static List<User> _invitedUsers = new List<User>();
    private static bool _didHost = false;
    private int fakeOnlineWait = 40;
    private float _afkTimeout;
    private float _timeoutFade;
    private float _topScroll;
    private float _afkMaxTimeout = 300f;
    private float _afkShowTimeout = 241f;
    private int _timeoutBeep;

    public ProfileBox2 GetBox(byte box) => this._profiles[(int) box];

    public static void DefaultSettings()
    {
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
        matchSetting.value = matchSetting.defaultValue;
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
        unlock.enabled = false;
      TeamSelect2.UpdateModifierStatus();
    }

    public static MatchSetting GetMatchSetting(string id) => TeamSelect2.matchSettings.FirstOrDefault<MatchSetting>((Func<MatchSetting, bool>) (x => x.id == id));

    public static MatchSetting GetOnlineSetting(string id) => TeamSelect2.onlineSettings.FirstOrDefault<MatchSetting>((Func<MatchSetting, bool>) (x => x.id == id));

    public static int GetSettingInt(string id)
    {
      foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
      {
        if (onlineSetting.id == id && onlineSetting.value is int)
          return (int) onlineSetting.value;
      }
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (matchSetting.id == id && matchSetting.value is int)
          return (int) matchSetting.value;
      }
      return -1;
    }

    public void ClearTeam(int index)
    {
      if (index < 0 || index >= 4 || (this._profiles == null || this._profiles.Count != 4) || this._profiles[index]._hatSelector == null)
        return;
      this._profiles[index]._hatSelector._desiredTeamSelection = (short) (sbyte) index;
      if (this._profiles[index].duck != null)
        this._profiles[index].duck.profile.team = Teams.all[index];
      this._profiles[index]._hatSelector.ConfirmTeamSelection();
      this._profiles[index]._hatSelector._teamSelection = this._profiles[index]._hatSelector._desiredTeamSelection = (short) (sbyte) index;
    }

    public static bool GetSettingBool(string id)
    {
      foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
      {
        if (onlineSetting.id == id && onlineSetting.value is bool)
          return (bool) onlineSetting.value;
      }
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (matchSetting.id == id && matchSetting.value is bool)
          return (bool) matchSetting.value;
      }
      return false;
    }

    public TeamSelect2()
    {
      this._centeredView = true;
      TeamSelect2.startCountdown = false;
    }

    public void CloseAllDialogs()
    {
      this._playOnlineGroup.Close();
      this._playOnlineMenu.Close();
      this._joinGameMenu.Close();
      this._filtersMenu.Close();
      this._filterModifierMenu.Close();
      this._hostGameMenu.Close();
      this._hostGameSettingsMenu.Close();
      this._hostGameModifierMenu.Close();
      this._matchmaker.Close();
    }

    public bool menuOpen => this._multiplayerMenu.open || this._modifierMenu.open || MonoMain.pauseMenu != null;

    public static bool Enabled(string id, bool ignoreTeamSelect = false)
    {
      if (!ignoreTeamSelect)
      {
        switch (Level.current)
        {
          case TeamSelect2 _:
          case RockScoreboard _:
            return false;
        }
      }
      UnlockData unlock = Unlocks.GetUnlock(id);
      if (unlock == null || Network.isActive && !unlock.onlineEnabled)
        return false;
      bool flag = false;
      TeamSelect2._modifierStatus.TryGetValue(id, out flag);
      return flag;
    }

    public static void UpdateModifierStatus()
    {
      bool flag = false;
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        TeamSelect2._modifierStatus[unlock.id] = false;
        if (unlock.enabled)
        {
          flag = true;
          TeamSelect2._modifierStatus[unlock.id] = true;
        }
      }
      if (!Network.isActive || !Network.isServer || Steam.lobby == null)
        return;
      Steam.lobby.SetLobbyData("modifiers", flag ? "true" : "false");
    }

    public bool MatchmakerOpen() => this._matchmaker != null && this._matchmaker.open;

    public void OpenDoor(int index, Duck d) => this._profiles[index].OpenDoor(d);

    public void PrepareForOnline()
    {
      if (Network.isServer)
        GhostManager.context.SetGhostIndex((NetIndex16) 32);
      int index = 0;
      foreach (ProfileBox2 profile in this._profiles)
      {
        profile.ChangeProfile(DuckNetwork.profiles[index]);
        if (!Level.core.gameInProgress)
        {
          this.ClearTeam(index);
          if (DuckNetwork.profiles[index].connection == null)
            DuckNetwork.profiles[index].team = (Team) null;
        }
        ++index;
      }
      this.things.RefreshState();
      foreach (Thing thing in this.things)
      {
        thing.DoNetworkInitialize();
        if (thing.ghostObject != null && Network.isServer)
          thing.ghostObject.ForceInitialize();
      }
      this._rebuildPauseMenu = true;
    }

    public void BuildPauseMenu(bool online)
    {
      if (this._pauseGroup != null)
        Level.Remove((Thing) this._pauseGroup);
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._pauseMenu = new UIMenu("MULTIPLAYER", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
      this._inviteMenu = (UIMenu) new UIInviteMenu("INVITE FRIENDS", (UIMenuAction) null, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
      ((UIInviteMenu) this._inviteMenu).SetAction((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._inviteMenu, (UIComponent) this._pauseMenu));
      if (Network.isActive)
      {
        this._pauseMenu.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup)), true);
        if (Network.isServer)
          this._pauseMenu.Add((UIComponent) new UIMenuItem("END SESSION", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._backOut)), true);
        else
          this._pauseMenu.Add((UIComponent) new UIMenuItem("DISCONNECT", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._backOut)), true);
      }
      else
      {
        this._pauseMenu.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup)), true);
        this._pauseMenu.Add((UIComponent) new UIMenuItem("BACK OUT", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._backOut)), true);
        this._pauseMenu.Add((UIComponent) new UIMenuItem("MAIN MENU", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._returnToMenu)), true);
      }
      if (Network.available)
      {
        this._pauseMenu.Add((UIComponent) new UIText("", Color.White), true);
        this._pauseMenu.Add((UIComponent) new UIMenuItem("INVITE FRIENDS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._pauseMenu, (UIComponent) this._inviteMenu), c: Color.Lime), true);
      }
      this._pauseMenu.Close();
      this._pauseGroup.Add((UIComponent) this._pauseMenu, false);
      this._inviteMenu.Close();
      this._pauseGroup.Add((UIComponent) this._inviteMenu, false);
      this._pauseGroup.Close();
      Level.Add((Thing) this._pauseGroup);
      if (this._localPauseGroup != null)
        Level.Remove((Thing) this._localPauseGroup);
      this._localPauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._localPauseMenu = new UIMenu("MULTIPLAYER", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
      this._localPauseMenu.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenu(this._localPauseGroup)), true);
      this._localPauseMenu.Add((UIComponent) new UIMenuItem("BACK OUT", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._localPauseGroup, this._localBackOut)), true);
      this._localPauseMenu.Close();
      this._localPauseGroup.Add((UIComponent) this._localPauseMenu, false);
      this._localPauseGroup.Close();
      Level.Add((Thing) this._localPauseGroup);
    }

    public void ClearFilters()
    {
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
        matchSetting.filtered = false;
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        unlock.filtered = false;
        unlock.enabled = false;
      }
    }

    public void ClosedOnline()
    {
      foreach (MatchmakingPlayer matchmakingProfile in UIMatchmakingBox.matchmakingProfiles)
      {
        this._profiles[(int) matchmakingProfile.duckIndex].profile.team = Teams.all[(int) matchmakingProfile.duckIndex];
        this._profiles[(int) matchmakingProfile.duckIndex].profile.inputProfile = matchmakingProfile.inputProfile;
      }
    }

    public static List<byte> GetNetworkModifierList()
    {
      List<byte> byteList = new List<byte>();
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        if (unlock.unlocked && unlock.enabled && Unlocks.modifierToByte.ContainsKey(unlock.id))
          byteList.Add(Unlocks.modifierToByte[unlock.id]);
      }
      return byteList;
    }

    public static string GetMatchSettingString()
    {
      string str = "" + TeamSelect2.GetSettingInt("requiredwins").ToString() + TeamSelect2.GetSettingInt("restsevery").ToString() + TeamSelect2.GetSettingInt("randommaps").ToString() + TeamSelect2.GetSettingInt("workshopmaps").ToString() + TeamSelect2.GetSettingInt("normalmaps").ToString() + ((bool) TeamSelect2.GetOnlineSetting("teams").value).ToString() + TeamSelect2.GetSettingInt("custommaps").ToString() + Editor.activatedLevels.Count.ToString() + TeamSelect2.GetSettingBool("wallmode").ToString();
      foreach (byte networkModifier in TeamSelect2.GetNetworkModifierList())
        str += networkModifier.ToString();
      return str;
    }

    public static void SendMatchSettings(NetworkConnection c = null, bool initial = false)
    {
      TeamSelect2.UpdateModifierStatus();
      if (!Network.isActive)
        return;
      Send.Message((NetMessage) new NMMatchSettings(initial, (byte) TeamSelect2.GetSettingInt("requiredwins"), (byte) TeamSelect2.GetSettingInt("restsevery"), (byte) TeamSelect2.GetSettingInt("randommaps"), (byte) TeamSelect2.GetSettingInt("workshopmaps"), (byte) TeamSelect2.GetSettingInt("normalmaps"), (bool) TeamSelect2.GetOnlineSetting("teams").value, (byte) TeamSelect2.GetSettingInt("custommaps"), Editor.activatedLevels.Count, TeamSelect2.GetSettingBool("wallmode"), TeamSelect2.GetNetworkModifierList()), c);
    }

    public void JoinLocalPlayer(InputProfile p)
    {
      if (!Network.isServer)
        return;
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.team != null && profile.inputProfile == p)
          return;
      }
      DuckNetwork.JoinLocalDuck(p);
    }

    public void OpenFindGameMenu()
    {
      this._playOnlineMenu.Open();
      MonoMain.pauseMenu = (UIComponent) this._playOnlineMenu;
      new UIMenuActionOpenMenu((UIComponent) this._playOnlineMenu, (UIComponent) this._joinGameMenu).Activate();
    }

    public void OpenCreateGameMenu()
    {
      this._playOnlineMenu.Open();
      MonoMain.pauseMenu = (UIComponent) this._playOnlineMenu;
      new UIMenuActionOpenMenu((UIComponent) this._playOnlineMenu, (UIComponent) this._hostGameMenu).Activate();
    }

    public void OpenNoModsFindGame() => DuckNetwork.OpenNoModsWindow(new UIMenuActionCloseMenuCallFunction.Function(this.OpenFindGameMenu));

    public void OpenNoModsCreateGame() => DuckNetwork.OpenNoModsWindow(new UIMenuActionCloseMenuCallFunction.Function(this.OpenCreateGameMenu));

    public override void Initialize()
    {
      ++Global.data.bootedSinceUpdate;
      Global.Save();
      TeamSelect2.customLevels = TeamSelect2.prevCustomLevels = 0;
      if (!Network.isActive)
        Level.core.gameInProgress = false;
      if (!Level.core.gameInProgress)
      {
        Main.ResetMatchStuff();
        Main.ResetGameStuff();
        DuckNetwork.ClosePauseMenu();
      }
      else
      {
        ConnectionStatusUI.Hide();
        if (Network.isServer)
        {
          if (Steam.lobby != null)
          {
            Steam.lobby.SetLobbyData("started", "false");
            Steam.lobby.joinable = true;
          }
          DuckNetwork.inGame = false;
          foreach (Profile profile in DuckNetwork.profiles)
          {
            if (profile.connection == null && profile.slotType != SlotType.Reserved)
              profile.slotType = SlotType.Closed;
          }
        }
      }
      if (Network.isActive && Network.isServer)
        DuckNetwork.ChangeSlotSettings();
      this._littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
      this._countdownScreen = new Sprite("title/wideScreen");
      this.backgroundColor = Color.Black;
      DuckNetwork.levelIndex = (byte) 0;
      if (Network.isActive && Network.isServer)
        GhostManager.context.SetGhostIndex((NetIndex16) 32);
      this._countdown = new SpriteMap("countdown", 32, 32);
      this._countdown.center = new Vec2(16f, 16f);
      Profile defaultProfile1 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.team != null && x.persona == Persona.Duck1)) ?? Profiles.DefaultPlayer1;
      Profile defaultProfile2 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.team != null && x.persona == Persona.Duck2)) ?? Profiles.DefaultPlayer2;
      Profile defaultProfile3 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.team != null && x.persona == Persona.Duck3)) ?? Profiles.DefaultPlayer3;
      Profile defaultProfile4 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.team != null && x.persona == Persona.Duck4)) ?? Profiles.DefaultPlayer4;
      float xpos = 1f;
      ProfileBox2 profileBox2_1 = new ProfileBox2(xpos, 1f, InputProfile.Get("MPPlayer1"), defaultProfile1, this, 0);
      this._profiles.Add(profileBox2_1);
      Level.Add((Thing) profileBox2_1);
      ProfileBox2 profileBox2_2 = new ProfileBox2(xpos + 178f, 1f, InputProfile.Get("MPPlayer2"), defaultProfile2, this, 1);
      this._profiles.Add(profileBox2_2);
      Level.Add((Thing) profileBox2_2);
      ProfileBox2 profileBox2_3 = new ProfileBox2(xpos, 90f, InputProfile.Get("MPPlayer3"), defaultProfile3, this, 2);
      this._profiles.Add(profileBox2_3);
      Level.Add((Thing) profileBox2_3);
      ProfileBox2 profileBox2_4 = new ProfileBox2(xpos + 178f, 90f, InputProfile.Get("MPPlayer4"), defaultProfile4, this, 3);
      this._profiles.Add(profileBox2_4);
      Level.Add((Thing) profileBox2_4);
      if (Network.isActive)
        this.PrepareForOnline();
      else
        this.BuildPauseMenu(false);
      this._font = new BitmapFont("biosFont", 8);
      this._font.scale = new Vec2(1f, 1f);
      this._buttons = new SpriteMap("buttons", 14, 14);
      this._buttons.CenterOrigin();
      this._buttons.depth = (Depth) 0.9f;
      Music.Play("CharacterSelect");
      this._beam = new TeamBeam(160f, 0.0f);
      Level.Add((Thing) this._beam);
      TeamSelect2.UpdateModifierStatus();
      this._configGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._multiplayerMenu = new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._modifierMenu = new UIMenu("MODIFIERS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@DPAD@ADJUST  @QUACK@BACK");
      this._levelSelectMenu = (UIMenu) new LevelSelectCompanionMenu(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, this._multiplayerMenu);
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        if (unlock.unlocked)
          this._modifierMenu.Add((UIComponent) new UIMenuItemToggle(unlock.shortName, field: new FieldBinding((object) unlock, "enabled")), true);
        else
          this._modifierMenu.Add((UIComponent) new UIMenuItem("@TINYLOCK@LOCKED", c: Color.Red), true);
      }
      this._modifierMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._modifierMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._modifierMenu, (UIComponent) this._multiplayerMenu), backButton: true), true);
      this._modifierMenu.Close();
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (!(matchSetting.id == "workshopmaps") || Network.available)
          this._multiplayerMenu.AddMatchSetting(matchSetting, false);
      }
      this._multiplayerMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._multiplayerMenu.Add((UIComponent) new UIModifierMenuItem((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._multiplayerMenu, (UIComponent) this._modifierMenu)), true);
      this._multiplayerMenu.Add((UIComponent) new UICustomLevelMenu((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._multiplayerMenu, (UIComponent) this._levelSelectMenu)), true);
      this._multiplayerMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._multiplayerMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionCloseMenu(this._configGroup), backButton: true), true);
      this._multiplayerMenu.Close();
      this._configGroup.Add((UIComponent) this._multiplayerMenu, false);
      this._configGroup.Add((UIComponent) this._modifierMenu, false);
      this._configGroup.Add((UIComponent) this._levelSelectMenu, false);
      this._configGroup.Close();
      Level.Add((Thing) this._configGroup);
      this._playOnlineGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._playOnlineMenu = new UIMenu("@PLANET@PLAY ONLINE@PLANET@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._hostGameMenu = new UIMenu("@LWING@CREATE GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._browseGamesMenu = (UIMenu) new UIServerBrowser(this._playOnlineMenu, "SERVER BROWSER", Layer.HUD.camera.width, Layer.HUD.camera.height, 550f, conString: "@DPAD@@SELECT@JOIN @SHOOT@REFRESH @QUACK@BACK");
      this._miniHostGameMenu = new UIMenu("@LWING@HOST GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._joinGameMenu = new UIMenu("@LWING@FIND GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._filtersMenu = new UIMenu("@LWING@FILTERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@SELECT@SELECT  @GRAB@TYPE");
      this._filterModifierMenu = new UIMenu("@LWING@FILTER MODIFIERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._matchmaker = new UIMatchmakingBox(this._joinGameMenu, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f);
      this._hostGameSettingsMenu = new UIMenu("@LWING@SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      this._hostGameModifierMenu = new UIMenu("@LWING@MODIFIERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@DPAD@ADJUST  @SELECT@SELECT");
      if (ModLoader.modHash != "nomods")
      {
        this._playOnlineMenu.Add((UIComponent) new UIMenuItem("FIND GAME", (UIMenuAction) new UIMenuActionCloseMenuCallFunction((UIComponent) this._playOnlineMenu, new UIMenuActionCloseMenuCallFunction.Function(this.OpenNoModsFindGame))), true);
        this._playOnlineMenu.Add((UIComponent) new UIMenuItem("CREATE GAME", (UIMenuAction) new UIMenuActionCloseMenuCallFunction((UIComponent) this._playOnlineMenu, new UIMenuActionCloseMenuCallFunction.Function(this.OpenNoModsCreateGame))), true);
      }
      else
      {
        this._playOnlineMenu.Add((UIComponent) new UIMenuItem("FIND GAME", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._playOnlineMenu, (UIComponent) this._joinGameMenu)), true);
        this._playOnlineMenu.Add((UIComponent) new UIMenuItem("CREATE GAME", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._playOnlineMenu, (UIComponent) this._hostGameMenu)), true);
      }
      this._playOnlineMenu.Add((UIComponent) new UIMenuItem("BROWSE GAMES", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._playOnlineMenu, (UIComponent) this._browseGamesMenu)), true);
      this._playOnlineMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._playOnlineMenu.Add((UIComponent) new UIMenuItem("CANCEL", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(this._playOnlineGroup, new UIMenuActionCloseMenuCallFunction.Function(this.ClosedOnline)), backButton: true), true);
      this._playOnlineMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._playOnlineMenu, false);
      foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
      {
        if (!onlineSetting.filterOnly)
          this._hostGameMenu.AddMatchSetting(onlineSetting, false);
      }
      this._hostGameMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._hostGameMenu.Add((UIComponent) new UIMenuItem("CREATE GAME", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._playOnlineGroup, this._createGame)), true);
      this._hostGameMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._hostGameMenu, (UIComponent) this._playOnlineMenu), backButton: true), true);
      this._hostGameMenu.Close();
      this._browseGamesMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._browseGamesMenu, false);
      this._playOnlineGroup.Add((UIComponent) this._hostGameMenu, false);
      this._miniHostGameMenu.AddMatchSetting(TeamSelect2.GetOnlineSetting("type"), false);
      this._miniHostGameMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._miniHostGameMenu.Add((UIComponent) new UIMenuItem("HOST GAME", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean((UIComponent) this._miniHostGameMenu, this._hostGame)), true);
      this._miniHostGameMenu.Add((UIComponent) new UIMenuItem("CANCEL", (UIMenuAction) new UIMenuActionCloseMenu((UIComponent) this._miniHostGameMenu), backButton: true), true);
      this._miniHostGameMenu.Close();
      Level.Add((Thing) this._miniHostGameMenu);
      foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
      {
        if (!onlineSetting.createOnly)
          this._joinGameMenu.AddMatchSetting(onlineSetting, true);
      }
      this._joinGameMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._joinGameMenu.Add((UIComponent) new UIMenuItem("FIND GAME", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._joinGameMenu, (UIComponent) this._matchmaker)), true);
      this._joinGameMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._joinGameMenu, (UIComponent) this._playOnlineMenu), backButton: true), true);
      this._joinGameMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._joinGameMenu, false);
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (!(matchSetting.id == "workshopmaps") || Network.available)
          this._filtersMenu.AddMatchSetting(matchSetting, true);
      }
      this._filtersMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._filtersMenu.Add((UIComponent) new UIModifierMenuItem((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._filtersMenu, (UIComponent) this._filterModifierMenu)), true);
      this._filtersMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._filtersMenu.Add((UIComponent) new UIMenuItem("|DGBLUE|CLEAR FILTERS", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.ClearFilters))), true);
      this._filtersMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._filtersMenu, (UIComponent) this._joinGameMenu), backButton: true), true);
      this._filtersMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._filtersMenu, false);
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
        this._filterModifierMenu.Add((UIComponent) new UIMenuItemToggle(unlock.shortName, field: new FieldBinding((object) unlock, "enabled"), filterBinding: new FieldBinding((object) unlock, "filtered")), true);
      this._filterModifierMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._filterModifierMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._filterModifierMenu, (UIComponent) this._filtersMenu), backButton: true), true);
      this._filterModifierMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._filterModifierMenu, false);
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (!(matchSetting.id == "workshopmaps") || Network.available)
          this._hostGameSettingsMenu.AddMatchSetting(matchSetting, false);
      }
      this._hostGameSettingsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._hostGameSettingsMenu.Add((UIComponent) new UIModifierMenuItem((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._hostGameSettingsMenu, (UIComponent) this._hostGameModifierMenu)), true);
      this._hostGameSettingsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._hostGameSettingsMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._hostGameSettingsMenu, (UIComponent) this._hostGameMenu), backButton: true), true);
      this._hostGameSettingsMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._hostGameSettingsMenu, false);
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
        this._hostGameModifierMenu.Add((UIComponent) new UIMenuItemToggle(unlock.shortName, field: new FieldBinding((object) unlock, "enabled")), true);
      this._hostGameModifierMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._hostGameModifierMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._hostGameModifierMenu, (UIComponent) this._hostGameSettingsMenu), backButton: true), true);
      this._hostGameModifierMenu.Close();
      this._playOnlineGroup.Add((UIComponent) this._hostGameModifierMenu, false);
      this._matchmaker.Close();
      this._playOnlineGroup.Add((UIComponent) this._matchmaker, false);
      this._playOnlineGroup.Close();
      Level.Add((Thing) this._playOnlineGroup);
      Graphics.fade = 0.0f;
      Layer l = new Layer("HUD2", -85, new Camera());
      l.camera.width /= 2f;
      l.camera.height /= 2f;
      Layer.Add(l);
      Layer hud = Layer.HUD;
      Layer.HUD = l;
      Editor.gamepadMode = true;
      Layer.HUD = hud;
      if (DuckNetwork.ShowUserXPGain() || !Unlockables.HasPendingUnlocks())
        return;
      MonoMain.pauseMenu = (UIComponent) new UIUnlockBox(Unlockables.GetPendingUnlocks().ToList<Unlockable>(), Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f);
    }

    public void OpenPauseMenu(ProfileBox2 whodid)
    {
      this._whodid = whodid;
      if (Network.isActive && (int) whodid.profile.networkIndex != DuckNetwork.localDuckIndex)
      {
        this._localPauseGroup.Open();
        this._localPauseMenu.Open();
        MonoMain.pauseMenu = this._localPauseGroup;
      }
      else
      {
        this._pauseGroup.Open();
        this._pauseMenu.Open();
        MonoMain.pauseMenu = this._pauseGroup;
      }
    }

    public override void NetworkDebuggerPrepare() => this.PrepareForOnline();

    public static void DoInvite()
    {
      if (!Network.isActive)
      {
        TeamSelect2.FillMatchmakingProfiles();
        DuckNetwork.Host(4, NetworkLobbyType.FriendsOnly);
        (Level.current as TeamSelect2).PrepareForOnline();
        TeamSelect2._didHost = true;
      }
      TeamSelect2._attemptingToInvite = true;
    }

    public static void InvitedFriend(User u)
    {
      if (!(Level.current is TeamSelect2) || u == null)
        return;
      TeamSelect2._invitedUsers.Add(u);
      DuckNetwork._invitedFriends.Add(u.id);
      if (ModLoader.modHash != "nomods")
        DuckNetwork.OpenNoModsWindow(new UIMenuActionCloseMenuCallFunction.Function(TeamSelect2.DoInvite));
      else
        TeamSelect2.DoInvite();
    }

    public override void Update()
    {
      GameMode.roundsBetweenIntermission = TeamSelect2.roundsBetweenIntermission;
      if (this._rebuildPauseMenu && MonoMain.pauseMenu == null)
      {
        this.BuildPauseMenu(true);
        this._rebuildPauseMenu = false;
      }
      if (this._findGame.value)
      {
        this._findGame.value = false;
        int num = Network.isActive ? 1 : 0;
      }
      if (this._createGame.value)
      {
        this._createGame.value = false;
        DuckNetwork.Host(TeamSelect2.GetSettingInt("maxplayers"), (NetworkLobbyType) TeamSelect2.GetSettingInt("type"));
        this.PrepareForOnline();
      }
      if (this._hostGame.value)
      {
        TeamSelect2.FillMatchmakingProfiles();
        DuckNetwork.Host(TeamSelect2.GetSettingInt("maxplayers"), (NetworkLobbyType) TeamSelect2.GetSettingInt("type"));
        this.PrepareForOnline();
        this._beam.ClearBeam();
        this._hostGame.value = false;
      }
      if (this._inviteFriends.value || TeamSelect2._invitedUsers.Count > 0)
      {
        this._inviteFriends.value = false;
        if (!Network.isActive)
        {
          TeamSelect2.FillMatchmakingProfiles();
          DuckNetwork.Host(4, NetworkLobbyType.FriendsOnly);
          this.PrepareForOnline();
        }
        TeamSelect2._attemptingToInvite = true;
      }
      if (TeamSelect2._attemptingToInvite && Network.isActive && (!TeamSelect2._didHost || Steam.lobby != null && !Steam.lobby.processing))
      {
        foreach (User invitedUser in TeamSelect2._invitedUsers)
          Steam.InviteUser(invitedUser, Steam.lobby);
        TeamSelect2._invitedUsers.Clear();
        TeamSelect2._attemptingToInvite = false;
      }
      if (Network.isActive)
      {
        foreach (InputProfile defaultProfile in InputProfile.defaultProfiles)
        {
          if (defaultProfile.Pressed("START"))
            this.JoinLocalPlayer(defaultProfile);
        }
      }
      if (Network.isActive && NetworkDebugger.enabled && NetworkDebugger.interfaces[NetworkDebugger.networkDrawingIndex].visible)
      {
        foreach (InputProfile defaultProfile in InputProfile.defaultProfiles)
        {
          bool flag = true;
          foreach (ProfileBox2 profile in this._profiles)
          {
            if (profile.profile != null && (profile.playerActive || profile.profile.connection != null) && profile.profile.inputProfile.genericController == defaultProfile.genericController)
            {
              flag = false;
              break;
            }
          }
          if (flag && defaultProfile.Pressed("START"))
          {
            foreach (ProfileBox2 profile in this._profiles)
            {
              if (profile.profile.connection == null)
              {
                Profile p = DuckNetwork.JoinLocalDuck(defaultProfile);
                if (p != null)
                {
                  p.inputProfile = defaultProfile;
                  profile.OpenDoor();
                  profile.ChangeProfile(p);
                  break;
                }
              }
            }
          }
        }
      }
      if (this._levelSelector != null)
      {
        if (this._levelSelector.isClosed)
        {
          this._levelSelector.Terminate();
          this._levelSelector = (LevelSelect) null;
          Layer.skipDrawing = false;
          this._beam.active = true;
          this._beam.visible = true;
          Editor.selectingLevel = false;
        }
        else
        {
          this._levelSelector.Update();
          this._beam.active = false;
          this._beam.visible = false;
          Editor.selectingLevel = true;
          return;
        }
      }
      if (this.openLevelSelect)
      {
        Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.04f);
        if ((double) Graphics.fade >= 0.00999999977648258)
          return;
        this._levelSelector = new LevelSelect(returnLevel: ((Level) this));
        this._levelSelector.Initialize();
        this.openLevelSelect = false;
        Layer.skipDrawing = true;
      }
      else
      {
        if (TeamSelect2.roundsBetweenIntermission > TeamSelect2.requiredWins)
          TeamSelect2.roundsBetweenIntermission = TeamSelect2.requiredWins;
        int num1 = 0;
        this.activePlayers = 0;
        foreach (ProfileBox2 profile in this._profiles)
        {
          if (profile.ready)
            ++num1;
          if (profile.playerActive)
            ++this.activePlayers;
        }
        foreach (Thing profile in this._profiles)
          profile.active = !this.menuOpen;
        this._beam.active = !this.menuOpen;
        if (!this.menuOpen)
          HUD.CloseAllCorners();
        if (this._backOut.value)
        {
          if (Network.isActive)
          {
            Level.current = (Level) new DisconnectFromGame();
          }
          else
          {
            this._backOut.value = false;
            this._whodid.CloseDoor();
            this._whodid = (ProfileBox2) null;
          }
        }
        if ((double) Graphics.fade <= 0.0 && this._returnToMenu.value)
          Level.current = (Level) new TitleScreen();
        if (!Network.isActive)
          TeamSelect2.startCountdown = this._starting;
        if (Keyboard.Down(Keys.F1))
        {
          num1 = 2;
          this.activePlayers = 2;
        }
        int num2 = 1;
        if (Network.isActive)
        {
          num2 = 0;
          foreach (Profile profile in DuckNetwork.profiles)
          {
            if (profile.connection == DuckNetwork.localConnection)
              ++num2;
          }
        }
        if (this.activePlayers == num1 && (!Network.isActive && num1 > 0 || Network.isActive && num1 > num2))
        {
          this._singlePlayer = num1 == 1;
          if (TeamSelect2.startCountdown)
          {
            DuckNetwork.inGame = true;
            this.dim = Maths.LerpTowards(this.dim, 0.8f, 0.02f);
            this._countTime -= 0.006666667f;
            if ((double) this._countTime <= 0.0 && Network.isServer && ((double) Graphics.fade <= 0.0 || NetworkDebugger.enabled))
            {
              TeamSelect2.UpdateModifierStatus();
              DevConsole.qwopMode = TeamSelect2.Enabled("QWOPPY", true);
              DevConsole.splitScreen = TeamSelect2.Enabled("SPLATSCR", true);
              DevConsole.rhythmMode = TeamSelect2.Enabled("RHYM", true);
              MonoMain.startTime = DateTime.Now;
              GameMode.winsPerSet = TeamSelect2.GetSettingInt("requiredwins");
              GameMode.roundsBetweenIntermission = TeamSelect2.GetSettingInt("restsevery");
              Deathmatch.userMapsPercent = TeamSelect2.GetSettingInt("custommaps");
              TeamSelect2.randomMapPercent = TeamSelect2.GetSettingInt("randommaps");
              TeamSelect2.normalMapPercent = TeamSelect2.GetSettingInt("normalmaps");
              TeamSelect2.workshopMapPercent = TeamSelect2.GetSettingInt("workshopmaps");
              RockScoreboard.wallMode = TeamSelect2.GetSettingBool("wallmode");
              TeamSelect2.partyMode = TeamSelect2.GetSettingBool("partymode");
              if (Network.isActive)
                TeamSelect2.SendMatchSettings();
              if (!Level.core.gameInProgress)
                Main.ResetMatchStuff();
              Music.Stop();
              if (this._singlePlayer)
              {
                Level.current.Clear();
                Level.current = (Level) new ArcadeLevel(Content.GetLevelID("arcade"));
              }
              else
              {
                if (!Network.isServer)
                  return;
                foreach (Profile profile in DuckNetwork.profiles)
                {
                  profile.reservedUser = (object) null;
                  if ((profile.connection == null || profile.connection.status != ConnectionStatus.Connected) && profile.slotType == SlotType.Reserved)
                    profile.slotType = SlotType.Closed;
                }
                Level level = !TeamSelect2.ctfMode ? (Level) new GameLevel(Deathmatch.RandomLevelString()) : (Level) new CTFLevel(Deathmatch.RandomLevelString(folder: "ctf"));
                Main.lastLevel = level.level;
                if (Network.isActive && Network.isServer)
                {
                  if (Steam.lobby != null)
                  {
                    Steam.lobby.SetLobbyData("started", "true");
                    Steam.lobby.joinable = false;
                  }
                  DuckNetwork.inGame = true;
                }
                Level.sendCustomLevels = true;
                Level.current = level;
                return;
              }
            }
          }
          else
          {
            DuckNetwork.inGame = false;
            this.dim = Maths.LerpTowards(this.dim, 0.0f, 0.1f);
            if ((double) this.dim < 0.0500000007450581)
              this._countTime = 1.5f;
          }
          this._matchSetup = true;
          if (Network.isServer)
          {
            if (!Network.isActive)
            {
              if (!this._singlePlayer && !this._starting && (!this.menuOpen && Input.Pressed("SHOOT")))
              {
                this._configGroup.Open();
                this._multiplayerMenu.Open();
                MonoMain.pauseMenu = this._configGroup;
              }
              if (!this._singlePlayer && !this._starting && (!this.menuOpen && Input.Pressed("GRAB")) && Profiles.active.Count <= 3)
              {
                if (ModLoader.modHash != "nomods")
                  DuckNetwork.OpenNoModsWindow(new UIMenuActionCloseMenuCallFunction.Function(this.HostOnlineMultipleLocalPlayers));
                else
                  this.HostOnlineMultipleLocalPlayers();
              }
            }
            if (!this.menuOpen && Input.Pressed("SELECT") && (!this._singlePlayer || !Main.isDemo))
            {
              if (Network.isActive)
                Send.Message((NetMessage) new NMBeginCountdown());
              else
                this._starting = true;
            }
            if (this._singlePlayer && Network.available && (!this._starting && !this.menuOpen) && Input.Pressed("GRAB"))
              this.PlayOnlineSinglePlayer();
          }
        }
        else
        {
          this.dim = Maths.LerpTowards(this.dim, 0.0f, 0.1f);
          if ((double) this.dim < 0.0500000007450581)
            this._countTime = 1.5f;
          this._matchSetup = false;
          this._starting = false;
          TeamSelect2.startCountdown = false;
        }
        base.Update();
        if (Network.isActive)
        {
          this._afkTimeout += Maths.IncFrameTimer();
          foreach (Profile profile in DuckNetwork.profiles)
          {
            if (profile.localPlayer && profile.inputProfile != null && profile.inputProfile.Pressed("ANY", true))
              this._afkTimeout = 0.0f;
          }
          if ((double) this._afkTimeout > (double) this._afkShowTimeout && (int) this._afkTimeout != this._timeoutBeep)
          {
            this._timeoutBeep = (int) this._afkTimeout;
            SFX.Play("cameraBeep");
          }
          if ((double) this._afkTimeout > (double) this._afkMaxTimeout)
            Level.current = (Level) new DisconnectFromGame();
        }
        else
          this._afkTimeout = 0.0f;
        Graphics.fade = Lerp.Float(Graphics.fade, this._returnToMenu.value || (double) this._countTime <= 0.0 ? 0.0f : 1f, 0.02f);
        this._bottomRight = new Vec2(1000f, 1000f);
        this.lowestPoint = 1000f;
        this._setupFade = Lerp.Float(this._setupFade, !this._matchSetup || this.menuOpen || TeamSelect2.startCountdown ? 0.0f : 1f, 0.05f);
        Layer.Game.fade = Lerp.Float(Layer.Game.fade, this._matchSetup ? 0.5f : 1f, 0.05f);
      }
    }

    private void PlayOnlineSinglePlayer()
    {
      TeamSelect2.FillMatchmakingProfiles();
      this._playOnlineGroup.Open();
      this._playOnlineMenu.Open();
      MonoMain.pauseMenu = this._playOnlineGroup;
    }

    private void HostOnlineMultipleLocalPlayers()
    {
      this._miniHostGameMenu.Open();
      MonoMain.pauseMenu = (UIComponent) this._miniHostGameMenu;
    }

    public static void FillMatchmakingProfiles()
    {
      for (int index = 0; index < 4; ++index)
      {
        if (Level.current is TeamSelect2)
          (Level.current as TeamSelect2).ClearTeam(index);
      }
      UIMatchmakingBox.matchmakingProfiles.Clear();
      foreach (Profile profile in Profiles.active)
      {
        profile.team = Teams.all[Persona.Number(profile.persona)];
        MatchmakingPlayer matchmakingPlayer = new MatchmakingPlayer()
        {
          duckIndex = (byte) Persona.Number(profile.persona),
          inputProfile = profile.inputProfile,
          team = profile.team
        };
        matchmakingPlayer.customData = (byte[]) null;
        UIMatchmakingBox.matchmakingProfiles.Add(matchmakingPlayer);
      }
    }

    public override void Draw()
    {
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (this._levelSelector != null)
      {
        if (!this._levelSelector.isInitialized)
          return;
        this._levelSelector.PostDrawLayer(layer);
      }
      else
      {
        Layer game = Layer.Game;
        Layer background = Layer.Background;
        if (layer == Layer.HUD)
        {
          if ((double) this._afkTimeout >= (double) this._afkShowTimeout)
          {
            this._timeoutFade = Lerp.Float(this._timeoutFade, 1f, 0.05f);
            Graphics.DrawRect(new Vec2(-1000f, -1000f), new Vec2(10000f, 10000f), Color.Black * 0.7f * this._timeoutFade, (Depth) 0.95f);
            string text1 = "AFK TIMEOUT IN";
            string text2 = ((int) ((double) this._afkMaxTimeout - (double) this._afkTimeout)).ToString();
            Graphics.DrawString(text1, new Vec2((float) ((double) layer.width / 2.0 - (double) Graphics.GetStringWidth(text1) / 2.0), (float) ((double) layer.height / 2.0 - 8.0)), Color.White * this._timeoutFade, (Depth) 0.96f);
            Graphics.DrawString(text2, new Vec2(layer.width / 2f - Graphics.GetStringWidth(text2), (float) ((double) layer.height / 2.0 + 4.0)), Color.White * this._timeoutFade, (Depth) 0.96f, scale: 2f);
          }
          else
            this._timeoutFade = Lerp.Float(this._timeoutFade, 0.0f, 0.05f);
          bool flag = false;
          foreach (Profile profile in DuckNetwork.profiles)
          {
            if (profile.reservedUser != null && profile.slotType == SlotType.Reserved)
            {
              flag = true;
              break;
            }
          }
          if (flag && Level.core.gameInProgress)
          {
            Vec2 vec2 = new Vec2(0.0f, Layer.HUD.barSize);
            Graphics.DrawRect(new Vec2(0.0f, vec2.y), new Vec2(320f, vec2.y + 10f), Color.Black, (Depth) 0.9f);
            this._littleFont.depth = (Depth) 0.95f;
            string str = "";
            foreach (Profile profile in DuckNetwork.profiles)
            {
              if (profile.reservedUser != null && profile.slotType == SlotType.Reserved)
              {
                if (str != "")
                  str += ", ";
                str += profile.name;
              }
            }
            if (str == "")
              str = "SOMEONE";
            string text1 = "GAME STILL IN PROGRESS! " + str + " DISCONNECTED!";
            string text2 = "";
            if (text1.Length > 0)
            {
              int index = 0;
              int num = text1.Length * 2;
              if (num < 90)
                num = 90;
              while (text2.Length < num)
              {
                text2 += (string) (object) text1[index];
                ++index;
                if (index >= text1.Length)
                {
                  index = 0;
                  text2 += " ";
                }
              }
            }
            float num1 = 0.01f;
            if (text1.Length > 20)
              num1 = 0.005f;
            if (text1.Length > 30)
              num1 = 1f / 500f;
            this._topScroll += num1;
            if ((double) this._topScroll > 1.0)
              --this._topScroll;
            if ((double) this._topScroll < 0.0)
              ++this._topScroll;
            this._littleFont.Draw(text2, new Vec2((float) (1.0 - (double) this._topScroll * ((double) this._littleFont.GetWidth(text1) + 7.0)), vec2.y + 3f), Color.White, (Depth) 0.95f);
          }
          if ((double) this._setupFade > 0.00999999977648258)
          {
            float num = (float) ((double) this.camera.height / 2.0 - 28.0);
            string str = "@GRAB@PLAY ONLINE";
            if (!Network.available)
            {
              str = "|GRAY|ONLINE UNAVAILABLE (NO STEAM)";
              if (Steam.user != null && Steam.user.state == SteamUserState.Offline)
                str = "|GRAY|ONLINE UNAVAILABLE (OFFLINE MODE)";
            }
            else if (Profiles.active.Count > 3)
              str = "|GRAY|ONLINE UNAVAILABLE (FULL GAME)";
            if (this._singlePlayer)
            {
              if (Network.available)
              {
                string text1 = "@SELECT@CHALLENGE ARCADE";
                this._font.alpha = this._setupFade;
                this._font.Draw(text1, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), num + 15f, Color.White, (Depth) 0.9f);
                string text2 = str;
                this._font.alpha = this._setupFade;
                this._font.Draw(text2, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), (float) ((double) num + 12.0 + 17.0), Color.White, (Depth) 0.9f);
              }
              else
              {
                string text1 = "@SELECT@ CHALLENGE ARCADE";
                if (Main.isDemo)
                  text1 = "NO CHALLENGE MODE IN DEMO :(";
                this._font.alpha = this._setupFade;
                this._font.Draw(text1, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), num + 15f, Color.White, (Depth) 0.9f);
                string text2 = str;
                this._font.alpha = this._setupFade;
                this._font.Draw(text2, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), (float) ((double) num + 12.0 + 17.0), Color.White, (Depth) 0.9f);
              }
            }
            else
            {
              this._font.alpha = this._setupFade;
              if (Network.isClient)
              {
                string text = "WAITING FOR HOST TO START";
                this._font.Draw(text, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text) / 2.0), num + 22f, Color.White, (Depth) 0.9f);
              }
              else if (!Network.isActive)
              {
                string text1 = "@SELECT@START MATCH";
                this._font.Draw(text1, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), num + 9f, Color.White, (Depth) 0.9f);
                string text2 = "@SHOOT@MATCH SETTINGS";
                this._font.Draw(text2, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), num + 22f, Color.White, (Depth) 0.9f);
                string text3 = str;
                this._font.Draw(text3, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text3) / 2.0), num + 35f, Color.White, (Depth) 0.9f);
              }
              else
              {
                string text = "@SELECT@START MATCH";
                this._font.Draw(text, (float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text) / 2.0), num + 22f, Color.White, (Depth) 0.9f);
              }
            }
            this._countdownScreen.alpha = this._setupFade;
            this._countdownScreen.depth = (Depth) 0.8f;
            this._countdownScreen.centery = (float) (this._countdownScreen.height / 2);
            Vec2 vec2 = new Vec2(this.camera.x, this.camera.height / 2f);
            Graphics.Draw(this._countdownScreen, vec2.x, vec2.y);
          }
          if ((double) this.dim > 0.00999999977648258)
          {
            this._countdownScreen.alpha = 1f;
            this._countdownScreen.depth = (Depth) 0.8f;
            Vec2 vec2 = new Vec2(this.camera.x, this.camera.height / 2f);
            Graphics.Draw(this._countdownScreen, vec2.x, vec2.y);
            this._countdown.alpha = this.dim * 1.2f;
            this._countdown.depth = (Depth) 0.98f;
            this._countdown.frame = (int) (float) Math.Ceiling((1.0 - (double) this._countTime) * 2.0);
            this._countdown.centery = (float) (this._countdown.height / 2);
            Graphics.Draw((Sprite) this._countdown, 160f, (float) ((double) this.camera.height / 2.0 - 3.0));
          }
        }
        base.PostDrawLayer(layer);
      }
    }

    public HatSelector GetHatSelector(int index) => this._profiles[index]._hatSelector;

    public override void OnMessage(NetMessage m)
    {
    }

    public override void OnNetworkConnected(Profile p)
    {
    }

    public override void OnNetworkConnecting(Profile p) => this._profiles[(int) p.networkIndex].PrepareDoor();

    public override void OnSessionEnded(DuckNetErrorInfo error)
    {
      if (this._matchmaker != null && this._matchmaker.open)
        this._matchmaker.OnSessionEnded(error);
      else
        base.OnSessionEnded(error);
    }

    public override void OnDisconnect(NetworkConnection n)
    {
      if (this._matchmaker == null || !this._matchmaker.open)
        return;
      this._matchmaker.OnDisconnect(n);
    }
  }
}
