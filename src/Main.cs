// Decompiled with JetBrains decompiler
// Type: DuckGame.Main
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace DuckGame
{
  public class Main : MonoMain
  {
    public static bool isDemo = false;
    public static DuckGameEditor editor;
    public static string lastLevel = "";
    public static string SpecialCode = "";
    public static string SpecialCode2 = "";
    public static int codeNumber = 0;
    private BitmapFont _font;
    public static ulong connectID = 0;
    public static bool foundPurchaseInfo = false;
    public static float price = 10f;
    public static string currencyType = "USD";
    public static bool stopForever = false;
    private List<float> _velocityPoints = new List<float>();
    private List<float> _actionPoints = new List<float>();
    private List<float> _bonusPoints = new List<float>();
    private List<float> _coolnessPoints = new List<float>();
    private static Queue<long> _frameTimes = new Queue<long>();
    private static Stopwatch _frameTimer;
    public bool joinedLobby;

    public static string GetPriceString() => "|GREEN|" + Main.price.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture) + " " + Main.currencyType + "|WHITE|";

    public static void SetPurchaseDetails(float p, string ct)
    {
      Main.price = p;
      Main.currencyType = ct;
      Main.foundPurchaseInfo = true;
    }

    public static void ResetMatchStuff()
    {
      PurpleBlock.Reset();
      Highlights.ClearHighlights();
      Crowd.GoHome();
      GameMode.lastWinners.Clear();
      Deathmatch.levelsSinceRandom = 0;
      Deathmatch.levelsSinceCustom = 0;
      GameMode.numMatchesPlayed = 0;
      GameMode.showdown = false;
      RockWeather.Reset();
      Music.Reset();
      foreach (Team team in Teams.all)
      {
        team.prevScoreboardScore = team.score = 0;
        if (team.activeProfiles.Count > 0)
        {
          foreach (Profile activeProfile in team.activeProfiles)
          {
            activeProfile.stats.lastPlayed = activeProfile.stats.lastPlayed = DateTime.Now;
            activeProfile.RecordPreviousStats();
            Profiles.Save(activeProfile);
          }
        }
      }
      if (Profiles.all != null)
      {
        foreach (Profile profile in Profiles.all)
          profile?.RecordPreviousStats();
      }
      Global.Save();
      Options.Save();
      Crowd.InitializeCrowd();
    }

    public static void ResetGameStuff()
    {
      if (Profiles.all == null)
        return;
      foreach (Profile profile in Profiles.all)
      {
        if (profile != null)
          profile.wins = 0;
      }
    }

    protected override void OnStart()
    {
      Options.Initialize();
      Global.Initialize();
      Teams.Initialize();
      DuckRig.Initialize();
      Unlocks.Initialize();
      ConnectionStatusUI.Initialize();
      Unlocks.CalculateTreeValues();
      Profiles.Initialize();
      Dialogue.Initialize();
      DuckTitle.Initialize();
      News.Initialize();
      Script.Initialize();
      DuckNews.Initialize();
      VirtualBackground.InitializeBack();
      AmmoType.InitializeTypes();
      DestroyType.InitializeTypes();
      VirtualTransition.Initialize();
      Unlockables.Initialize();
      UIInviteMenu.Initialize();
      LevelGenerator.Initialize();
      DuckFile.CompleteSteamCloudInitializate();
      Main.ResetMatchStuff();
      foreach (Profile profile in Profiles.active)
        profile.RecordPreviousStats();
      Main.editor = new DuckGameEditor();
      foreach (string file in DuckFile.GetFiles(Directory.GetCurrentDirectory(), "*.hat"))
      {
        Team t = Team.Deserialize(file);
        if (t != null)
          Teams.AddExtraTeam(t);
      }
      Main.SetPurchaseDetails(9.99f, "USD");
      if (Main.connectID != 0UL)
      {
        NCSteam.inviteLobbyID = Main.connectID;
        Level.current = (Level) new JoinServer(Main.connectID);
      }
      else
        Level.current = !MonoMain.noIntro ? (Level) new BIOSScreen() : (!MonoMain.startInEditor ? (Level) new TitleScreen() : (Level) Main.editor);
      this._font = new BitmapFont("biosFont", 8);
      Highlights.StartRound();
    }

    public static float averageFrameTime
    {
      get
      {
        float num = 0.0f;
        foreach (long frameTime in Main._frameTimes)
          num += (float) frameTime;
        return num / (float) Main._frameTimes.Count;
      }
    }

    public static void StartFrameTimer()
    {
      if (Main._frameTimer == null)
        Main._frameTimer = new Stopwatch();
      Main._frameTimer.Reset();
      Main._frameTimer.Start();
    }

    public static void EndFrameTimer()
    {
      Main._frameTimes.Enqueue(Main._frameTimer.ElapsedMilliseconds);
      if (Main._frameTimes.Count <= 60)
        return;
      Main._frameTimes.Dequeue();
    }

    protected override void OnUpdate()
    {
      Main.isDemo = false;
      RockWeather.TickWeather();
      Persona.Update();
      RandomLevelDownloader.Update();
      if (!NetworkDebugger.enabled)
        FireManager.Update();
      DamageManager.Update();
      if (!Network.isActive)
        NetRand.generator = Rando.generator;
      if (this.joinedLobby || !Program.testServer || (Network.isActive || !Steam.lobbySearchComplete))
        return;
      if (Steam.lobbySearchResult != null)
      {
        Network.JoinServer("", 0, Steam.lobbySearchResult.id.ToString());
        this.joinedLobby = true;
      }
      else
      {
        User who = Steam.friends.Find((Predicate<User>) (x => x.name == "superjoebob"));
        if (who == null)
          return;
        Steam.SearchForLobby(who);
      }
    }

    protected override void OnDraw()
    {
    }
  }
}
