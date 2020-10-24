// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMatchmakingBox
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UIMatchmakingBox : UIMenu
  {
    private Sprite _frame;
    private SpriteMap _matchmakingSignal;
    private List<SpriteMap> _matchmakingStars = new List<SpriteMap>();
    private BitmapFont _font;
    private FancyBitmapFont _fancyFont;
    private float _scroll;
    private Lobby _tryHostingLobby;
    private Lobby _tryConnectLobby;
    private int _tries;
    private float _tryConnectTimeout;
    private bool _quit;
    public static bool pulseLocal;
    public static bool pulseNetwork;
    private SpriteMap _signalCrossLocal;
    private SpriteMap _signalCrossNetwork;
    public static List<MatchmakingPlayer> matchmakingProfiles = new List<MatchmakingPlayer>();
    private List<BlacklistServer> _failedAttempts = new List<BlacklistServer>();
    private List<BlacklistServer> _permenantBlacklist = new List<BlacklistServer>();
    public static List<ulong> nonPreferredServers = new List<ulong>();
    private static MatchmakingState _state = MatchmakingState.None;
    private List<string> _statusList = new List<string>();
    private List<string> _newStatusList = new List<string>();
    private float _newStatusWait = 1f;
    private float _tryHostingWait;
    private UIMenu _openOnClose;
    private float _stateWait;
    private MatchmakingState _pendingState;
    private int searchTryIndex;
    private bool triedHostingAlready;
    private int _totalLobbiesFound = -1;
    private int _totalInGameLobbies;
    private int _triesSinceSearch;
    private long _globalKills;
    private float _dots;

    public static MatchmakingState state => UIMatchmakingBox._state;

    public UIMatchmakingBox(UIMenu openOnClose, float xpos, float ypos, float wide = -1f, float high = -1f)
      : base("", xpos, ypos, wide, high)
    {
      this._openOnClose = openOnClose;
      Graphics.fade = 1f;
      this._frame = new Sprite("online/matchmakingBeta");
      this._frame.CenterOrigin();
      this._font = new BitmapFont("biosFontUI", 8, 7);
      this._fancyFont = new FancyBitmapFont("smallFont");
      this._matchmakingSignal = new SpriteMap("online/matchmakingSignal", 4, 9);
      this._matchmakingSignal.CenterOrigin();
      SpriteMap spriteMap1 = new SpriteMap("online/matchmakingStar", 7, 7);
      spriteMap1.AddAnimation("flicker", 0.08f, true, 0, 1, 2, 1);
      spriteMap1.SetAnimation("flicker");
      spriteMap1.CenterOrigin();
      this._signalCrossLocal = new SpriteMap("online/signalCross", 5, 5);
      this._signalCrossLocal.AddAnimation("idle", 0.12f, true, new int[1]);
      this._signalCrossLocal.AddAnimation("flicker", 0.12f, false, 1, 2, 3);
      this._signalCrossLocal.SetAnimation("idle");
      this._signalCrossLocal.CenterOrigin();
      this._signalCrossNetwork = new SpriteMap("online/signalCross", 5, 5);
      this._signalCrossNetwork.AddAnimation("idle", 0.12f, true, new int[1]);
      this._signalCrossNetwork.AddAnimation("flicker", 0.12f, false, 1, 2, 3);
      this._signalCrossNetwork.SetAnimation("idle");
      this._signalCrossNetwork.CenterOrigin();
      this._matchmakingStars.Add(spriteMap1);
      SpriteMap spriteMap2 = new SpriteMap("online/matchmakingStar", 7, 7);
      spriteMap2.AddAnimation("flicker", 0.11f, true, 0, 1, 2, 1);
      spriteMap2.SetAnimation("flicker");
      spriteMap2.CenterOrigin();
      this._matchmakingStars.Add(spriteMap2);
      SpriteMap spriteMap3 = new SpriteMap("online/matchmakingStar", 7, 7);
      spriteMap3.AddAnimation("flicker", 0.03f, true, 0, 1, 2, 1);
      spriteMap3.SetAnimation("flicker");
      spriteMap3.CenterOrigin();
      this._matchmakingStars.Add(spriteMap3);
      SpriteMap spriteMap4 = new SpriteMap("online/matchmakingStar", 7, 7);
      spriteMap4.AddAnimation("flicker", 0.03f, true, 0, 1, 2, 1);
      spriteMap4.SetAnimation("flicker");
      spriteMap4.CenterOrigin();
      this._matchmakingStars.Add(spriteMap4);
    }

    public void ChangeState(MatchmakingState s, float wait = 0.0f)
    {
      if (s == MatchmakingState.Waiting)
        return;
      DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|CHANGE STATE " + s.ToString(), Color.White);
      if ((double) wait == 0.0)
      {
        this.OnStateChange(s);
      }
      else
      {
        UIMatchmakingBox._state = MatchmakingState.Waiting;
        this._pendingState = s;
        this._stateWait = wait;
      }
    }

    public bool IsBlacklisted(ulong lobby) => this._permenantBlacklist.FirstOrDefault<BlacklistServer>((Func<BlacklistServer, bool>) (x => (long) x.lobby == (long) lobby)) != null || this._failedAttempts.FirstOrDefault<BlacklistServer>((Func<BlacklistServer, bool>) (x => (long) x.lobby == (long) lobby)) != null;

    private void OnStateChange(MatchmakingState s)
    {
      UIMatchmakingBox._state = s;
      this._stateWait = 0.0f;
      switch (UIMatchmakingBox._state)
      {
        case MatchmakingState.Searching:
          Steam.SearchForLobby((User) null);
          DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Searching for lobbies.", Color.White);
          break;
        case MatchmakingState.Disconnect:
          Network.Disconnect();
          break;
        case MatchmakingState.Connecting:
          this._tryConnectTimeout = 9f + Rando.Float(2f);
          DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Attempting connection to server.", Color.White);
          break;
      }
    }

    public override void Open()
    {
      this.searchTryIndex = 0;
      this._permenantBlacklist.Clear();
      this._newStatusList.Add("|DGYELLOW|Connecting to servers on the Moon.");
      HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@ABORT");
      Music.Play("jazzroom");
      this._triesSinceSearch = 0;
      this.triedHostingAlready = false;
      this._tryConnectLobby = (Lobby) null;
      this._tryHostingLobby = (Lobby) null;
      this.ChangeState(MatchmakingState.ConnectToMoon);
      this._tryHostingWait = 0.0f;
      this._tryConnectTimeout = 0.0f;
      this._quit = false;
      this._tries = 0;
      this._tryHostingWait = 0.0f;
      this._totalLobbiesFound = -1;
      this._failedAttempts.Clear();
      base.Open();
    }

    public override void Close()
    {
      this.ChangeState(MatchmakingState.None);
      this._tryHostingWait = 0.0f;
      this._quit = false;
      this._newStatusList.Clear();
      this._statusList.Clear();
      this._tryConnectTimeout = 0.0f;
      base.Close();
    }

    public void OnDisconnect(NetworkConnection n)
    {
      if (!this.open || UIMatchmakingBox._state != MatchmakingState.Connecting || (this._tryHostingLobby == null || Network.connections.Count != 0))
        return;
      this.ChangeState(MatchmakingState.SearchForLobbies);
      DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Client disconnect, continuing search.", Color.White);
    }

    public void OnSessionEnded(DuckNetErrorInfo error)
    {
      if (!this.open)
        return;
      if (UIMatchmakingBox._state == MatchmakingState.Disconnect)
      {
        if (this._tryHostingLobby != null)
          this._tries = 0;
        this._tryHostingLobby = (Lobby) null;
        if (this._quit)
        {
          HUD.CloseAllCorners();
          this.Close();
          this._openOnClose.Open();
        }
        else if (this._tryConnectLobby != null)
        {
          DuckNetwork.Join(this._tryConnectLobby.id.ToString());
          this.ChangeState(MatchmakingState.Connecting);
        }
        else
          this.ChangeState(MatchmakingState.SearchForLobbies);
      }
      else
      {
        if (error != null)
        {
          if (error.error == DuckNetError.VersionMismatch)
          {
            if (error.tooNew)
              this._newStatusList.Add("|DGRED|Their version was older.");
            else
              this._newStatusList.Add("|DGRED|Their version was newer.");
            if (this._tryConnectLobby != null)
              this._permenantBlacklist.Add(new BlacklistServer()
              {
                lobby = this._tryConnectLobby.id,
                cooldown = 15f
              });
          }
          else if (error.error == DuckNetError.FullServer)
            this._newStatusList.Add("|DGRED|Failed (FULL SERVER)");
          else if (error.error == DuckNetError.ConnectionTimeout)
            this._newStatusList.Add("|DGRED|Failed (TIMEOUT)");
          else if (error.error == DuckNetError.GameInProgress)
          {
            this._newStatusList.Add("|DGRED|Failed (IN PROGRESS)");
          }
          else
          {
            this._newStatusList.Add("|DGRED|Unknown connection error.");
            if (this._tryConnectLobby != null)
              this._permenantBlacklist.Add(new BlacklistServer()
              {
                lobby = this._tryConnectLobby.id,
                cooldown = 15f
              });
          }
        }
        else
        {
          this._newStatusList.Add("|DGRED|Connection timeout.");
          if (this._tryConnectLobby != null)
            this._permenantBlacklist.Add(new BlacklistServer()
            {
              lobby = this._tryConnectLobby.id,
              cooldown = 15f
            });
        }
        if (this._tryConnectLobby != null)
          this._failedAttempts.Add(new BlacklistServer()
          {
            lobby = this._tryConnectLobby.id,
            cooldown = 15f
          });
        DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Connection failure, continuing search.", Color.White);
        this._tryConnectLobby = (Lobby) null;
        this.ChangeState(MatchmakingState.SearchForLobbies);
      }
    }

    public override void Update()
    {
      this._scroll += 0.1f;
      if ((double) this._scroll > 9.0)
        this._scroll = 0.0f;
      this._dots += 0.01f;
      if ((double) this._dots > 1.0)
        this._dots = 0.0f;
      if (this.open)
      {
        foreach (BlacklistServer failedAttempt in this._failedAttempts)
          failedAttempt.cooldown = Lerp.Float(failedAttempt.cooldown, 0.0f, Maths.IncFrameTimer());
        if (this._signalCrossLocal.currentAnimation == "idle")
        {
          if (UIMatchmakingBox.pulseLocal)
          {
            this._signalCrossLocal.SetAnimation("flicker");
            UIMatchmakingBox.pulseLocal = false;
          }
        }
        else if (this._signalCrossLocal.finished)
          this._signalCrossLocal.SetAnimation("idle");
        if (this._signalCrossNetwork.currentAnimation == "idle")
        {
          if (UIMatchmakingBox.pulseNetwork)
          {
            this._signalCrossNetwork.SetAnimation("flicker");
            UIMatchmakingBox.pulseNetwork = false;
          }
        }
        else if (this._signalCrossNetwork.finished)
          this._signalCrossNetwork.SetAnimation("idle");
        if (Network.connections.Count > 0 && UIMatchmakingBox._state != MatchmakingState.Connecting)
        {
          this.ChangeState(MatchmakingState.Connecting);
          DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Network appears to be connecting...", Color.White);
        }
        if (DuckNetwork.status == DuckNetStatus.Connected)
        {
          if (this._tryHostingLobby != null)
          {
            (Level.current as TeamSelect2).CloseAllDialogs();
            Level.current = (Level) new TeamSelect2();
            DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Finished! (HOST).", Color.White);
            return;
          }
          if (Level.current is TeamSelect2)
          {
            (Level.current as TeamSelect2).CloseAllDialogs();
            Level.current = (Level) new ConnectingScreen();
            DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Finished! (CLIENT).", Color.White);
            return;
          }
          Network.Disconnect();
          this.ChangeState(MatchmakingState.SearchForLobbies);
          DevConsole.Log("|PURPLE|MATCHMAKING |DGGREEN|Last minute connection error.", Color.White);
          return;
        }
        switch (UIMatchmakingBox._state)
        {
          case MatchmakingState.ConnectToMoon:
            Steam.AddLobbyStringFilter("started", "true", SteamLobbyComparison.Equal);
            Steam.SearchForLobby((User) null);
            Steam.RequestGlobalStats();
            UIMatchmakingBox.pulseLocal = true;
            this.ChangeState(MatchmakingState.ConnectingToMoon);
            break;
          case MatchmakingState.ConnectingToMoon:
            if (Steam.lobbySearchComplete)
            {
              if (this.searchTryIndex == 0)
              {
                this._totalInGameLobbies = Steam.lobbiesFound;
                if (this._totalInGameLobbies < 0)
                  this._totalInGameLobbies = 0;
                ++this.searchTryIndex;
                Steam.AddLobbyStringFilter("started", "false", SteamLobbyComparison.Equal);
                Steam.SearchForLobby((User) null);
                break;
              }
              UIMatchmakingBox.pulseNetwork = true;
              this._totalLobbiesFound = Steam.lobbiesFound;
              List<User> users = Steam.GetSearchLobbyAtIndex(0).users;
              this._newStatusList.Add("|DGGREEN|Connected to Moon!");
              this._newStatusList.Add("");
              this._newStatusList.Add("|DGYELLOW|Searching for companions.");
              this.ChangeState(MatchmakingState.SearchForLobbies);
              break;
            }
            break;
          case MatchmakingState.SearchForLobbies:
            if (this._triesSinceSearch == 3)
            {
              Steam.AddLobbyStringFilter("started", "true", SteamLobbyComparison.Equal);
              Steam.SearchForLobby((User) null);
              this.ChangeState(MatchmakingState.CheckingTotalGames);
              return;
            }
            if (this._tries > 0 && this._tryHostingLobby == null)
            {
              DuckNetwork.Host(TeamSelect2.GetSettingInt("maxplayers"), NetworkLobbyType.Public);
              this._tryHostingLobby = (Network.activeNetwork.core as NCSteam).lobby;
              if (!this.triedHostingAlready)
                this._newStatusList.Add("|DGYELLOW|Searching even harder.");
              else
                this._newStatusList.Add("|DGYELLOW|Searching.");
              this.triedHostingAlready = true;
              DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Opened lobby while searching.", Color.White);
              this._tryHostingWait = 5f + Rando.Float(2f);
            }
            foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
            {
              if (matchSetting.value is int)
              {
                if (matchSetting.filtered)
                  Steam.AddLobbyNumericalFilter(matchSetting.id, (int) matchSetting.value, (SteamLobbyComparison) matchSetting.filterMode);
                else if (!matchSetting.filtered)
                  Steam.AddLobbyNearFilter(matchSetting.id, (int) matchSetting.defaultValue);
              }
              if (matchSetting.value is bool)
              {
                if (matchSetting.filtered)
                  Steam.AddLobbyNumericalFilter(matchSetting.id, (bool) matchSetting.value ? 1 : 0, (SteamLobbyComparison) matchSetting.filterMode);
                else if (!matchSetting.filtered)
                  Steam.AddLobbyNearFilter(matchSetting.id, (bool) matchSetting.defaultValue ? 1 : 0);
              }
            }
            foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
            {
              if (onlineSetting.value is int)
              {
                if (onlineSetting.filtered)
                  Steam.AddLobbyNumericalFilter(onlineSetting.id, (int) onlineSetting.value, (SteamLobbyComparison) onlineSetting.filterMode);
                else if (!onlineSetting.filtered)
                  Steam.AddLobbyNearFilter(onlineSetting.id, (int) onlineSetting.defaultValue);
              }
              if (onlineSetting.value is bool)
              {
                if (onlineSetting.id == "modifiers")
                {
                  if (onlineSetting.filtered)
                    Steam.AddLobbyStringFilter(onlineSetting.id, (bool) onlineSetting.value ? "true" : "false", SteamLobbyComparison.Equal);
                }
                else if (onlineSetting.filtered)
                  Steam.AddLobbyNumericalFilter(onlineSetting.id, (bool) onlineSetting.value ? 1 : 0, (SteamLobbyComparison) onlineSetting.filterMode);
                else if (!onlineSetting.filtered)
                  Steam.AddLobbyNearFilter(onlineSetting.id, (bool) onlineSetting.defaultValue ? 1 : 0);
              }
            }
            Steam.AddLobbyStringFilter("started", "false", SteamLobbyComparison.Equal);
            Steam.AddLobbyStringFilter("beta", "2.0", SteamLobbyComparison.Equal);
            Steam.AddLobbyStringFilter("dev", DG.devBuild ? "true" : "false", SteamLobbyComparison.Equal);
            Steam.AddLobbyStringFilter("modhash", ModLoader.modHash, SteamLobbyComparison.Equal);
            if (!Steam.waitingForGlobalStats)
              this._globalKills = (long) Steam.GetDailyGlobalStat("kills");
            Steam.RequestGlobalStats();
            UIMatchmakingBox.pulseLocal = true;
            this.ChangeState(MatchmakingState.Searching);
            ++this._triesSinceSearch;
            ++this._tries;
            break;
          case MatchmakingState.CheckingTotalGames:
            if (Steam.lobbySearchComplete)
            {
              this._totalInGameLobbies = Steam.lobbiesFound;
              if (this._totalInGameLobbies < 0)
                this._totalInGameLobbies = 0;
              this.ChangeState(MatchmakingState.SearchForLobbies);
              this._triesSinceSearch = 0;
              break;
            }
            break;
          case MatchmakingState.Searching:
            if (Steam.lobbySearchComplete)
            {
              this._totalLobbiesFound = Steam.lobbiesFound;
              if (this._tryHostingLobby != null)
                --this._totalLobbiesFound;
              List<Lobby> lobbyList = new List<Lobby>();
              DevConsole.Log("|PURPLE|MATCHMAKING |LIME|found " + (object) Math.Max(this._totalLobbiesFound, 0) + " lobbies.", Color.White);
              for (int index1 = 0; index1 < 2; ++index1)
              {
                int num1 = index1 != 0 ? lobbyList.Count : Steam.lobbiesFound;
                for (int index2 = 0; index2 < num1; ++index2)
                {
                  Lobby lobby = index1 != 0 ? lobbyList[index2] : Steam.GetSearchLobbyAtIndex(index2);
                  if (this._tryHostingLobby == null || (long) lobby.id != (long) this._tryHostingLobby.id)
                  {
                    if (index2 == Steam.lobbiesFound - 1)
                      this._failedAttempts.RemoveAll((Predicate<BlacklistServer>) (x => (double) x.cooldown <= 0.0));
                    if (UIMatchmakingBox.nonPreferredServers.Contains(lobby.id) && index1 == 0)
                    {
                      lobbyList.Add(lobby);
                      DevConsole.Log("|PURPLE|MATCHMAKING |DGRED|Skipping " + (object) lobby.id + " (NOT PREFERRED)", Color.White);
                    }
                    else if (this.IsBlacklisted(lobby.id))
                    {
                      DevConsole.Log("|PURPLE|MATCHMAKING |DGRED|Skipping " + (object) lobby.id + " (BLACKLISTED)", Color.White);
                    }
                    else
                    {
                      if (this._tryHostingLobby != null)
                      {
                        int num2 = -1;
                        try
                        {
                          string lobbyData = lobby.GetLobbyData("randomID");
                          if (lobbyData != "")
                            num2 = Convert.ToInt32(lobbyData);
                        }
                        catch
                        {
                        }
                        if (num2 == -1)
                        {
                          DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Bad lobby seed.", Color.White);
                          num2 = Rando.Int(2147483646);
                        }
                        if (num2 >= this._tryHostingLobby.randomID)
                        {
                          DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Lobby beats own lobby, Attempting join.", Color.White);
                        }
                        else
                        {
                          DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Skipping lobby (Chose to keep hosting).", Color.White);
                          NCSteam.UpdateRandomID(this._tryHostingLobby);
                          continue;
                        }
                      }
                      this._tryConnectLobby = lobby;
                      if (lobby.owner != null)
                        this._newStatusList.Add("|LIME|Trying to join " + lobby.owner.name + ".");
                      else
                        this._newStatusList.Add("|LIME|Trying to join server.");
                      this.ChangeState(MatchmakingState.Disconnect);
                      break;
                    }
                  }
                }
              }
              if (this._tryConnectLobby == null)
              {
                DevConsole.Log("|PURPLE|MATCHMAKING |DGYELLOW|Found no valid lobbies.", Color.White);
                this.ChangeState(MatchmakingState.SearchForLobbies, 3f);
                break;
              }
              break;
            }
            break;
          case MatchmakingState.Waiting:
            this._stateWait -= Maths.IncFrameTimer();
            if ((double) this._stateWait <= 0.0)
            {
              this._stateWait = 0.0f;
              this.OnStateChange(this._pendingState);
              break;
            }
            break;
          default:
            int state = (int) UIMatchmakingBox._state;
            break;
        }
        if (Input.Pressed("QUACK"))
        {
          this._quit = true;
          this.ChangeState(MatchmakingState.Disconnect);
        }
      }
      if (this._newStatusList.Count > 0)
      {
        this._newStatusWait -= 0.1f;
        if ((double) this._newStatusWait <= 0.0)
        {
          this._newStatusWait = 1f;
          while ((double) this._fancyFont.GetWidth(this._newStatusList[0]) > 98.0)
            this._newStatusList[0] = this._newStatusList[0].Substring(0, this._newStatusList[0].Length - 1);
          this._statusList.Add(this._newStatusList[0]);
          if (this._statusList.Count > 7)
            this._statusList.RemoveAt(0);
          this._newStatusList.RemoveAt(0);
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      this._frame.depth = this.depth;
      Graphics.Draw(this._frame, this.x, this.y);
      for (int index = 0; index < 7; ++index)
      {
        float num1 = this.x - 28f;
        float x = num1 + (float) (index * 9) + (float) Math.Round((double) this._scroll);
        float num2 = num1 + 63f;
        float num3 = (float) (((double) x - (double) num1) / ((double) num2 - (double) num1));
        this._matchmakingSignal.depth = this.depth + 4;
        if ((double) num3 > -0.100000001490116)
          this._matchmakingSignal.frame = 0;
        if ((double) num3 > 0.0500000007450581)
          this._matchmakingSignal.frame = 1;
        if ((double) num3 > 0.100000001490116)
          this._matchmakingSignal.frame = 2;
        if ((double) num3 > 0.899999976158142)
          this._matchmakingSignal.frame = 1;
        if ((double) num3 > 0.949999988079071)
          this._matchmakingSignal.frame = 0;
        Graphics.Draw((Sprite) this._matchmakingSignal, x, this.y - 21f);
      }
      this._matchmakingStars[0].depth = this.depth + 2;
      Graphics.Draw((Sprite) this._matchmakingStars[0], this.x - 9f, this.y - 18f);
      this._matchmakingStars[1].depth = this.depth + 2;
      Graphics.Draw((Sprite) this._matchmakingStars[1], this.x + 31f, this.y - 22f);
      this._matchmakingStars[2].depth = this.depth + 2;
      Graphics.Draw((Sprite) this._matchmakingStars[2], this.x + 12f, this.y - 20f);
      this._matchmakingStars[3].depth = this.depth + 2;
      Graphics.Draw((Sprite) this._matchmakingStars[3], this.x - 23f, this.y - 21f);
      this._signalCrossLocal.depth = this.depth + 2;
      Graphics.Draw((Sprite) this._signalCrossLocal, this.x - 35f, this.y - 19f);
      this._signalCrossNetwork.depth = this.depth + 2;
      Graphics.Draw((Sprite) this._signalCrossNetwork, this.x + 45f, this.y - 23f);
      string text = "LOOKING";
      Vec2 vec2 = new Vec2((float) -((double) this._font.GetWidth(text) / 2.0), -42f);
      this._font.DrawOutline(text, this.position + vec2, Color.White, Color.Black, this.depth + 2);
      this._fancyFont.scale = new Vec2(0.5f);
      int num4 = 0;
      int num5 = 0;
      foreach (string status in this._statusList)
      {
        string str1 = status;
        if (num5 == this._statusList.Count - 1 && this._newStatusList.Count == 0)
        {
          string str2 = "";
          string str3 = ".";
          if (str1.Count<char>() > 0 && str1.Last<char>() == '!' || (str1.Last<char>() == '.' || str1.Last<char>() == '?'))
          {
            str3 = string.Concat((object) str1.Last<char>());
            str1 = str1.Substring(0, str1.Length - 1);
          }
          for (int index = 0; index < 3; ++index)
          {
            if ((double) this._dots * 4.0 > (double) (index + 1))
              str2 += str3;
          }
          str1 += str2;
        }
        this._fancyFont.Draw(str1, new Vec2(this.x - 52f, this.y - 8f + (float) (num4 * 6)), Color.White, this.depth + 2);
        ++num4;
        ++num5;
      }
      if (this._totalLobbiesFound != -1)
      {
        string str = "games";
        if (this._totalLobbiesFound == 1)
          str = "game";
        if (this._totalInGameLobbies > 0)
          this._fancyFont.Draw(this._totalLobbiesFound.ToString() + " open " + str + " |DGYELLOW|(" + this._totalInGameLobbies.ToString() + " in progress)", this.position + new Vec2(-55f, 38f), Color.Black, this.depth + 2);
        else
          this._fancyFont.Draw(this._totalLobbiesFound.ToString() + " open " + str, this.position + new Vec2(-55f, 38f), Color.Black, this.depth + 2);
      }
      else
        this._fancyFont.Draw("Querying moon...", this.position + new Vec2(-55f, 38f), Color.Black, this.depth + 2);
    }
  }
}
