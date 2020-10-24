// Decompiled with JetBrains decompiler
// Type: DuckGame.Deathmatch
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Deathmatch : Thing
  {
    private bool _matchOver;
    private float _deadTimer = 1f;
    public static bool showdown = false;
    private static int numMatches = 0;
    private static Queue<string> _recentLevels = new Queue<string>();
    private static List<string> _demoLevels = new List<string>()
    {
      "deathmatch/forest02",
      "deathmatch/office02",
      "deathmatch/forest04",
      "deathmatch/office07",
      "deathmatch/office10",
      "deathmatch/office05"
    };
    private static int _winsPerSet = 5;
    private static int _roundsBetweenIntermission = 5;
    private static int _userMapsPercent = 0;
    private static bool _enableRandom = true;
    private static bool _randomMapsOnly = false;
    public static List<Profile> lastWinners = new List<Profile>();
    private static float _wait = 0.0f;
    private static bool _endedHighlights = false;
    private static string _currentSong = "";
    private Sprite _bottomWedge;
    private bool _addedPoints;
    private UIComponent _pauseGroup;
    private UIMenu _pauseMenu;
    private UIMenu _confirmMenu;
    private new Level _level;
    private MenuBoolean _quit = new MenuBoolean();
    private static List<string> _networkLevels = (List<string>) null;
    public static int levelsSinceRandom = 0;
    public static int levelsSinceWorkshop = 0;
    public static int levelsSinceCustom = 0;
    private bool _paused;
    private bool switched;

    public static int winsPerSet
    {
      get => Deathmatch._winsPerSet;
      set => Deathmatch._winsPerSet = value;
    }

    public static int roundsBetweenIntermission
    {
      get => Deathmatch._roundsBetweenIntermission;
      set => Deathmatch._roundsBetweenIntermission = value;
    }

    public static int userMapsPercent
    {
      get => Deathmatch._userMapsPercent;
      set => Deathmatch._userMapsPercent = value;
    }

    public static bool enableRandom
    {
      get => Deathmatch._enableRandom;
      set => Deathmatch._enableRandom = value;
    }

    public static bool randomMapsOnly
    {
      get => Deathmatch._randomMapsOnly;
      set => Deathmatch._randomMapsOnly = value;
    }

    public Deathmatch(Level l)
      : base()
    {
      this._level = l;
      this.layer = Layer.HUD;
      this._bottomWedge = new Sprite("bottomWedge");
    }

    public override void Initialize()
    {
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._pauseMenu = new UIMenu("@LWING@PAUSE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@DPAD@MOVE  @SELECT@SELECT");
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
      Highlights.StartRound();
    }

    public override void Terminate() => Options.openOnClose = (UIMenu) null;

    public static string RandomLevelString(string ignore = "", string folder = "deathmatch")
    {
      List<string> source1 = Content.GetLevels(folder, LevelLocation.Content);
      List<string> levels = Content.GetLevels(folder + "/rare", LevelLocation.Content);
      foreach (string str in levels)
        source1.Remove(str);
      foreach (string level in Content.GetLevels(folder + "/special", LevelLocation.Content))
        source1.Remove(level);
      if (Network.isActive)
      {
        if (Deathmatch._networkLevels == null)
          Deathmatch._networkLevels = new List<string>()
          {
            Content.GetLevelID("deathmatch/level"),
            Content.GetLevelID("deathmatch/level2"),
            Content.GetLevelID("deathmatch/level3"),
            Content.GetLevelID("deathmatch/level4"),
            Content.GetLevelID("deathmatch/level5"),
            Content.GetLevelID("deathmatch/level6"),
            Content.GetLevelID("deathmatch/level7"),
            Content.GetLevelID("deathmatch/level8"),
            Content.GetLevelID("deathmatch/level9"),
            Content.GetLevelID("deathmatch/industrial01"),
            Content.GetLevelID("deathmatch/industrial02"),
            Content.GetLevelID("deathmatch/industrial03"),
            Content.GetLevelID("deathmatch/industrial04"),
            Content.GetLevelID("deathmatch/industrial05"),
            Content.GetLevelID("deathmatch/industrial06"),
            Content.GetLevelID("deathmatch/bunker01"),
            Content.GetLevelID("deathmatch/bunker02"),
            Content.GetLevelID("deathmatch/bunker03"),
            Content.GetLevelID("deathmatch/bunker04"),
            Content.GetLevelID("deathmatch/bunker05"),
            Content.GetLevelID("deathmatch/bunker06"),
            Content.GetLevelID("deathmatch/forest01"),
            Content.GetLevelID("deathmatch/forest02"),
            Content.GetLevelID("deathmatch/forest03"),
            Content.GetLevelID("deathmatch/forest04"),
            Content.GetLevelID("deathmatch/forest05"),
            Content.GetLevelID("deathmatch/forest06"),
            Content.GetLevelID("deathmatch/forest07"),
            Content.GetLevelID("deathmatch/forest08"),
            Content.GetLevelID("deathmatch/forest09"),
            Content.GetLevelID("deathmatch/forest10"),
            Content.GetLevelID("deathmatch/forest11"),
            Content.GetLevelID("deathmatch/forest12"),
            Content.GetLevelID("deathmatch/forest13"),
            Content.GetLevelID("deathmatch/forest14"),
            Content.GetLevelID("deathmatch/forest18"),
            Content.GetLevelID("deathmatch/forest20"),
            Content.GetLevelID("deathmatch/office01"),
            Content.GetLevelID("deathmatch/office02"),
            Content.GetLevelID("deathmatch/office03"),
            Content.GetLevelID("deathmatch/office04"),
            Content.GetLevelID("deathmatch/office05"),
            Content.GetLevelID("deathmatch/office06"),
            Content.GetLevelID("deathmatch/office07"),
            Content.GetLevelID("deathmatch/office10"),
            Content.GetLevelID("deathmatch/office11"),
            Content.GetLevelID("deathmatch/office12"),
            Content.GetLevelID("deathmatch/office13"),
            Content.GetLevelID("deathmatch/office14"),
            Content.GetLevelID("deathmatch/office15"),
            Content.GetLevelID("deathmatch/office16"),
            Content.GetLevelID("deathmatch/office17"),
            Content.GetLevelID("deathmatch/office20"),
            Content.GetLevelID("deathmatch/office21"),
            Content.GetLevelID("deathmatch/snow01"),
            Content.GetLevelID("deathmatch/snow02"),
            Content.GetLevelID("deathmatch/snow03"),
            Content.GetLevelID("deathmatch/snow04"),
            Content.GetLevelID("deathmatch/snow05"),
            Content.GetLevelID("deathmatch/snow06"),
            Content.GetLevelID("deathmatch/snow07"),
            Content.GetLevelID("deathmatch/snow08"),
            Content.GetLevelID("deathmatch/snow09"),
            Content.GetLevelID("deathmatch/snow10"),
            Content.GetLevelID("deathmatch/snow11"),
            Content.GetLevelID("deathmatch/space01"),
            Content.GetLevelID("deathmatch/space02"),
            Content.GetLevelID("deathmatch/space03"),
            Content.GetLevelID("deathmatch/space05"),
            Content.GetLevelID("deathmatch/space06"),
            Content.GetLevelID("deathmatch/space07"),
            Content.GetLevelID("deathmatch/space08"),
            Content.GetLevelID("deathmatch/space09"),
            Content.GetLevelID("deathmatch/space10"),
            Content.GetLevelID("deathmatch/space11"),
            Content.GetLevelID("deathmatch/space12"),
            Content.GetLevelID("deathmatch/space13"),
            Content.GetLevelID("deathmatch/space14"),
            Content.GetLevelID("deathmatch/space15"),
            Content.GetLevelID("deathmatch/space16"),
            Content.GetLevelID("deathmatch/space17"),
            Content.GetLevelID("deathmatch/space18")
          };
        source1 = new List<string>((IEnumerable<string>) Deathmatch._networkLevels);
      }
      if (DateTime.Now.Month == 12)
      {
        if (DateTime.Now.Day <= 25)
          source1.Add("23ec9c56-dbcc-4384-9507-5b0f80cb0111");
        else if (DateTime.Now.Day == 24 || DateTime.Now.Day == 25)
        {
          source1.Add("23ec9c56-dbcc-4384-9507-5b0f80cb0111");
          source1.Add("23ec9c56-dbcc-4384-9507-5b0f80cb0111");
          source1.Add("23ec9c56-dbcc-4384-9507-5b0f80cb0111");
        }
      }
      if (ignore == "")
        levels.Clear();
      source1.AddRange((IEnumerable<string>) levels);
      if (TeamSelect2.normalMapPercent != 100)
      {
        List<MapRollGroup> source2 = new List<MapRollGroup>();
        source2.Add(new MapRollGroup()
        {
          type = MapRollType.Normal,
          chance = TeamSelect2.normalMapPercent
        });
        if (!NetworkDebugger.enabled)
          source2.Add(new MapRollGroup()
          {
            type = MapRollType.Random,
            chance = TeamSelect2.randomMapPercent
          });
        source2.Add(new MapRollGroup()
        {
          type = MapRollType.Custom,
          chance = Deathmatch.userMapsPercent
        });
        source2.Add(new MapRollGroup()
        {
          type = MapRollType.Internet,
          chance = TeamSelect2.workshopMapPercent
        });
        IOrderedEnumerable<MapRollGroup> orderedEnumerable = source2.OrderBy<MapRollGroup, int>((Func<MapRollGroup, int>) (x => Rando.Int(2147483646)));
        MapRollGroup mapRollGroup1 = (MapRollGroup) null;
        MapRollGroup mapRollGroup2 = (MapRollGroup) null;
        foreach (MapRollGroup mapRollGroup3 in (IEnumerable<MapRollGroup>) orderedEnumerable)
        {
          if ((mapRollGroup3.type != MapRollType.Custom || Editor.activatedLevels.Count != 0) && (mapRollGroup3.type != MapRollType.Internet || RandomLevelDownloader.PeekNextLevel() != null))
          {
            if (mapRollGroup1 == null || mapRollGroup3.chance > mapRollGroup1.chance || mapRollGroup1.chance == 0 && mapRollGroup3.chance == 0 && mapRollGroup3.type == MapRollType.Normal)
              mapRollGroup1 = mapRollGroup3;
            if (Rando.Int(100) < mapRollGroup3.chance && (mapRollGroup2 == null || mapRollGroup3.chance < mapRollGroup2.chance))
              mapRollGroup2 = mapRollGroup3;
          }
        }
        if (mapRollGroup2 == null)
          mapRollGroup2 = mapRollGroup1;
        if (mapRollGroup2.type == MapRollType.Random)
          return "RANDOM";
        if (mapRollGroup2.type == MapRollType.Internet)
          return "WORKSHOP";
        if (mapRollGroup2.type == MapRollType.Custom)
        {
          source1.Clear();
          foreach (string activatedLevel in Editor.activatedLevels)
            source1.Add(activatedLevel + ".custom");
        }
      }
      if (Main.isDemo)
        source1.RemoveAll((Predicate<string>) (x => !Deathmatch._demoLevels.Contains(x)));
      if (Deathmatch._recentLevels.Count > 0 && Deathmatch._recentLevels.Count > source1.Count - 5)
        Deathmatch._recentLevels.Dequeue();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) source1);
      string str1 = "";
      while (str1 == "")
      {
        if (source1.Count == 0 && Deathmatch._recentLevels.Count > 0)
        {
          str1 = Deathmatch._recentLevels.Dequeue();
          if (!stringList.Contains(str1))
            str1 = "";
        }
        else if (source1.Count == 0)
        {
          str1 = stringList[0];
        }
        else
        {
          str1 = source1[Rando.Int(source1.Count<string>() - 1)];
          if (str1 == ignore && source1.Count > 1)
          {
            source1.Remove(str1);
            str1 = "";
          }
          else if (!levels.Contains(str1) || (double) Rando.Float(1f) > 0.75)
          {
            if (Deathmatch._recentLevels.Contains(str1))
            {
              if ((double) Rando.Float(1f) > 0.100000001490116)
              {
                source1.Remove(str1);
                if (source1.Count > 0)
                  str1 = "";
              }
              else
                str1 = "";
            }
          }
          else
          {
            source1.Remove(str1);
            str1 = "";
          }
        }
      }
      if (str1 != "RANDOM")
        Deathmatch._recentLevels.Enqueue(str1);
      if (str1.EndsWith(".custom"))
      {
        LevelData levelData = DuckFile.LoadLevel(str1.Substring(0, str1.Length - 7));
        if (levelData != null)
          str1 = levelData.metaData.guid + ".custom";
      }
      return str1;
    }

    public override void Update()
    {
      if ((double) Graphics.fade > 0.899999976158142 && Input.Pressed("START") && !NetworkDebugger.enabled)
      {
        this._pauseGroup.Open();
        this._pauseMenu.Open();
        MonoMain.pauseMenu = this._pauseGroup;
        if (this._paused)
          return;
        Music.Pause();
        SFX.Play("pause", 0.6f);
        this._paused = true;
      }
      else
      {
        if (this._paused && MonoMain.pauseMenu == null)
        {
          this._paused = false;
          SFX.Play("resume", 0.6f);
          Music.Resume();
        }
        if (this._quit.value)
        {
          Graphics.fade -= 0.04f;
          if ((double) Graphics.fade >= 0.00999999977648258)
            return;
          Level.current = (Level) new TitleScreen();
        }
        else
        {
          if (Music.finished)
            Deathmatch._wait -= 0.0006f;
          if (!this._matchOver)
          {
            List<Team> teamList = new List<Team>();
            foreach (Team team in Teams.all)
            {
              foreach (Profile activeProfile in team.activeProfiles)
              {
                if (activeProfile.duck != null && !activeProfile.duck.dead)
                {
                  if (activeProfile.duck.converted != null && activeProfile.duck.converted.profile.team != activeProfile.team)
                  {
                    if (!teamList.Contains(activeProfile.duck.converted.profile.team))
                    {
                      teamList.Add(activeProfile.duck.converted.profile.team);
                      break;
                    }
                    break;
                  }
                  if (!teamList.Contains(team))
                  {
                    teamList.Add(team);
                    break;
                  }
                  break;
                }
              }
            }
            if (teamList.Count <= 1)
            {
              this._matchOver = true;
              ++Deathmatch.numMatches;
              if (Deathmatch.numMatches >= Deathmatch.roundsBetweenIntermission || Deathmatch.showdown)
                Deathmatch.numMatches = 0;
            }
          }
          if (this._matchOver)
            this._deadTimer -= 0.005f;
          if ((double) this._deadTimer < 0.5 && !this._addedPoints)
          {
            List<Team> teamList = new List<Team>();
            List<Team> source = new List<Team>();
            foreach (Team team in Teams.all)
            {
              foreach (Profile activeProfile in team.activeProfiles)
              {
                if (activeProfile.duck != null && !activeProfile.duck.dead)
                {
                  if (activeProfile.duck.converted != null && activeProfile.duck.converted.profile.team != activeProfile.team)
                  {
                    if (!source.Contains(activeProfile.duck.converted.profile.team))
                      source.Add(activeProfile.duck.converted.profile.team);
                    if (!teamList.Contains(activeProfile.duck.profile.team))
                    {
                      teamList.Add(activeProfile.duck.profile.team);
                      break;
                    }
                    break;
                  }
                  if (!source.Contains(team))
                  {
                    source.Add(team);
                    break;
                  }
                  break;
                }
              }
            }
            if (source.Count <= 1)
            {
              Highlights.highlightRatingMultiplier = 0.0f;
              Deathmatch.lastWinners.Clear();
              if (source.Count > 0)
              {
                SFX.Play("scoreDing", 0.8f);
                Event.Log((Event) new RoundEndEvent());
                source.AddRange((IEnumerable<Team>) teamList);
                List<int> idxs = new List<int>();
                foreach (Team team in source)
                {
                  foreach (Profile activeProfile in team.activeProfiles)
                  {
                    if (activeProfile.duck != null && !activeProfile.duck.dead)
                    {
                      Deathmatch.lastWinners.Add(activeProfile);
                      activeProfile.stats.lastWon = DateTime.Now;
                      ++activeProfile.stats.matchesWon;
                      Profile p = activeProfile;
                      if (activeProfile.duck.converted != null)
                        p = activeProfile.duck.converted.profile;
                      PlusOne plusOne = new PlusOne(0.0f, 0.0f, p);
                      plusOne.anchor = (Anchor) (Thing) activeProfile.duck;
                      plusOne.anchor.offset = new Vec2(0.0f, -16f);
                      idxs.Add((int) activeProfile.duck.netProfileIndex);
                      Level.Add((Thing) plusOne);
                    }
                  }
                }
                if (Network.isActive && Network.isServer)
                  Send.Message((NetMessage) new NMAssignWin(idxs, (byte) 4));
                ++source.First<Team>().score;
              }
            }
            this._addedPoints = true;
          }
          if ((double) this._deadTimer < 0.100000001490116 && !Deathmatch._endedHighlights)
          {
            Deathmatch._endedHighlights = true;
            Highlights.FinishRound();
          }
          if ((double) this._deadTimer >= 0.0 || this.switched || Network.isActive)
            return;
          foreach (Team team in Teams.all)
          {
            foreach (Profile activeProfile in team.activeProfiles)
              Profiles.Save(activeProfile);
          }
          int num = 0;
          List<Team> winning = Teams.winning;
          if (winning.Count > 0)
            num = winning[0].score;
          if (num <= 4)
            return;
          foreach (Team team in Teams.active)
          {
            if (team.score != num)
            {
              if (team.score < 1)
              {
                foreach (Profile activeProfile in team.activeProfiles)
                  Party.AddRandomPerk(activeProfile);
              }
              else if (team.score < 2 && (double) Rando.Float(1f) > 0.300000011920929)
              {
                foreach (Profile activeProfile in team.activeProfiles)
                  Party.AddRandomPerk(activeProfile);
              }
              else if (team.score < 5 && (double) Rando.Float(1f) > 0.600000023841858)
              {
                foreach (Profile activeProfile in team.activeProfiles)
                  Party.AddRandomPerk(activeProfile);
              }
              else if (team.score < 7 && (double) Rando.Float(1f) > 0.850000023841858)
              {
                foreach (Profile activeProfile in team.activeProfiles)
                  Party.AddRandomPerk(activeProfile);
              }
              else if (team.score < num && (double) Rando.Float(1f) > 0.899999976158142)
              {
                foreach (Profile activeProfile in team.activeProfiles)
                  Party.AddRandomPerk(activeProfile);
              }
            }
          }
        }
      }
    }

    public void PlayMusic()
    {
      string music = Music.RandomTrack("InGame", Deathmatch._currentSong);
      Music.Play(music, false);
      Deathmatch._currentSong = music;
      Deathmatch._wait = 1f;
    }

    private SpawnPoint AttemptTeamSpawn(
      Team team,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      List<TeamSpawn> teamSpawnList = new List<TeamSpawn>();
      foreach (TeamSpawn teamSpawn in this._level.things[typeof (TeamSpawn)])
      {
        if (!usedSpawns.Contains((SpawnPoint) teamSpawn))
          teamSpawnList.Add(teamSpawn);
      }
      if (teamSpawnList.Count <= 0)
        return (SpawnPoint) null;
      TeamSpawn teamSpawn1 = teamSpawnList[Rando.Int(teamSpawnList.Count - 1)];
      usedSpawns.Add((SpawnPoint) teamSpawn1);
      for (int index = 0; index < team.numMembers; ++index)
      {
        Vec2 position = teamSpawn1.position;
        if (team.numMembers == 2)
        {
          float num = 18.82353f;
          position.x = (float) ((double) teamSpawn1.position.x - 16.0 + (double) num * (double) index);
        }
        else if (team.numMembers == 3)
        {
          float num = 9.411764f;
          position.x = (float) ((double) teamSpawn1.position.x - 16.0 + (double) num * (double) index);
        }
        Duck duck = new Duck(position.x, position.y - 7f, team.activeProfiles[index]);
        duck.offDir = teamSpawn1.offDir;
        spawned.Add(duck);
      }
      return (SpawnPoint) teamSpawn1;
    }

    private SpawnPoint AttemptFreeSpawn(
      Profile profile,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      List<SpawnPoint> spawnPointList = new List<SpawnPoint>();
      foreach (FreeSpawn freeSpawn in this._level.things[typeof (FreeSpawn)])
      {
        if (!usedSpawns.Contains((SpawnPoint) freeSpawn))
          spawnPointList.Add((SpawnPoint) freeSpawn);
      }
      if (spawnPointList.Count == 0)
        return (SpawnPoint) null;
      SpawnPoint spawnPoint = spawnPointList[Rando.Int(spawnPointList.Count - 1)];
      usedSpawns.Add(spawnPoint);
      Duck duck = new Duck(spawnPoint.x, spawnPoint.y - 7f, profile);
      duck.offDir = spawnPoint.offDir;
      spawned.Add(duck);
      return spawnPoint;
    }

    private SpawnPoint AttemptAnySpawn(
      Profile profile,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      List<SpawnPoint> spawnPointList = new List<SpawnPoint>();
      foreach (SpawnPoint spawnPoint in this._level.things[typeof (SpawnPoint)])
      {
        if (!usedSpawns.Contains(spawnPoint))
          spawnPointList.Add(spawnPoint);
      }
      if (spawnPointList.Count == 0)
      {
        if (usedSpawns.Count <= 0)
          return (SpawnPoint) null;
        spawnPointList.AddRange((IEnumerable<SpawnPoint>) usedSpawns);
      }
      SpawnPoint spawnPoint1 = spawnPointList[Rando.Int(spawnPointList.Count - 1)];
      usedSpawns.Add(spawnPoint1);
      Duck duck = new Duck(spawnPoint1.x, spawnPoint1.y - 7f, profile);
      duck.offDir = spawnPoint1.offDir;
      spawned.Add(duck);
      return spawnPoint1;
    }

    public List<Duck> SpawnPlayers()
    {
      List<Duck> spawned = new List<Duck>();
      List<SpawnPoint> usedSpawns = new List<SpawnPoint>();
      List<Team> teamList1 = Teams.allRandomized;
      if (Deathmatch.showdown)
      {
        List<Team> teamList2 = new List<Team>();
        int num = 0;
        foreach (Team team in teamList1)
        {
          if (team.score > num)
            num = team.score;
        }
        foreach (Team team in teamList1)
        {
          if (team.score == num)
            teamList2.Add(team);
        }
        teamList1 = teamList2;
      }
      foreach (Team team in teamList1)
      {
        if (team.activeProfiles.Count<Profile>() != 0)
        {
          foreach (Profile activeProfile in team.activeProfiles)
            ++activeProfile.stats.timesSpawned;
          if (team.activeProfiles.Count<Profile>() == 1)
          {
            if ((this.AttemptFreeSpawn(team.activeProfiles[0], usedSpawns, spawned) ?? this.AttemptAnySpawn(team.activeProfiles[0], usedSpawns, spawned)) == null)
              return spawned;
          }
          else if (this.AttemptTeamSpawn(team, usedSpawns, spawned) == null)
          {
            foreach (Profile activeProfile in team.activeProfiles)
            {
              if ((this.AttemptFreeSpawn(activeProfile, usedSpawns, spawned) ?? this.AttemptAnySpawn(activeProfile, usedSpawns, spawned)) == null)
                return spawned;
            }
          }
        }
      }
      return spawned;
    }

    public override void Draw()
    {
    }
  }
}
