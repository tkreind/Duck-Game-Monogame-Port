// Decompiled with JetBrains decompiler
// Type: DuckGame.RandomLevelDownloader
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public static class RandomLevelDownloader
  {
    private static WorkshopQueryAll _currentQuery = (WorkshopQueryAll) null;
    public static bool ready = false;
    public static int numToHaveReady = 10;
    public static int numSinceLowRating = 0;
    private static List<LevelData> _readyLevels = new List<LevelData>();
    private static int _toFetchIndex = -1;
    private static int _numFetch = 0;
    private static WorkshopItem _downloading;
    private static WorkshopQueryFilterOrder _orderMode = WorkshopQueryFilterOrder.RankedByVote;

    public static LevelData GetNextLevel()
    {
      if (RandomLevelDownloader._readyLevels.Count == 0)
        return (LevelData) null;
      LevelData levelData = RandomLevelDownloader._readyLevels.First<LevelData>();
      RandomLevelDownloader._readyLevels.RemoveAt(0);
      return levelData;
    }

    public static LevelData PeekNextLevel() => RandomLevelDownloader._readyLevels.Count == 0 ? (LevelData) null : RandomLevelDownloader._readyLevels.First<LevelData>();

    private static void Fetched(object sender, WorkshopQueryResult result)
    {
      if (RandomLevelDownloader._toFetchIndex == -1)
        RandomLevelDownloader._toFetchIndex = Rando.Int((int) (sender as WorkshopQueryAll).numResultsFetched);
      if (RandomLevelDownloader._toFetchIndex == RandomLevelDownloader._numFetch && Steam.DownloadWorkshopItem(result.details.publishedFile))
        RandomLevelDownloader._downloading = result.details.publishedFile;
      RandomLevelDownloader._currentQuery = (WorkshopQueryAll) null;
      ++RandomLevelDownloader._numFetch;
    }

    private static void FinishedTotalQuery(object sender)
    {
      WorkshopQueryAll workshopQueryAll = sender as WorkshopQueryAll;
      if (workshopQueryAll.numResultsTotal <= 0U)
        return;
      int num = Rando.Int((int) (workshopQueryAll.numResultsTotal / 50U)) + 1;
      if (RandomLevelDownloader.numSinceLowRating > 3)
        RandomLevelDownloader.numSinceLowRating = 0;
      else
        num %= 10;
      RandomLevelDownloader._orderMode = RandomLevelDownloader.numSinceLowRating != 2 ? WorkshopQueryFilterOrder.RankedByVote : WorkshopQueryFilterOrder.RankedByTrend;
      if (num == 0)
        num = 1;
      ++RandomLevelDownloader.numSinceLowRating;
      WorkshopQueryAll queryAll = Steam.CreateQueryAll(RandomLevelDownloader._orderMode, WorkshopType.Items);
      queryAll.requiredTags.Add("Deathmatch");
      queryAll.ResultFetched += new WorkshopQueryResultFetched(RandomLevelDownloader.Fetched);
      queryAll.page = (uint) num;
      queryAll.justOnePage = true;
      queryAll.Request();
    }

    private static void SearchDirLevels(string dir, LevelLocation location)
    {
      foreach (string path in location == LevelLocation.Content ? Content.GetFiles(dir) : DuckFile.GetFiles(dir))
        RandomLevelDownloader.ProcessLevel(path, location);
      foreach (string dir1 in location == LevelLocation.Content ? Content.GetDirectories(dir) : DuckFile.GetDirectories(dir))
        RandomLevelDownloader.SearchDirLevels(dir1, location);
    }

    private static void ProcessLevel(string path, LevelLocation location)
    {
      Main.SpecialCode = "Loading Level " + path != null ? path : "null";
      if (!path.EndsWith(".lev"))
        return;
      path = path.Replace('\\', '/');
      LevelData levelData = DuckFile.LoadLevel(path);
      levelData.SetPath(path);
      path = path.Substring(0, path.Length - 4);
      path.Substring(path.IndexOf("/levels/") + 8);
      bool flag1 = true;
      if (levelData.modData.workshopIDs.Count != 0)
      {
        foreach (ulong workshopId in levelData.modData.workshopIDs)
        {
          bool flag2 = false;
          foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
          {
            if (accessibleMod.configuration != null && (long) accessibleMod.configuration.workshopID == (long) workshopId)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            flag1 = false;
            break;
          }
        }
      }
      if (flag1 && !levelData.modData.hasLocalMods)
      {
        RandomLevelDownloader._readyLevels.Add(levelData);
        DevConsole.Log(DCSection.Steam, "Downloaded random level " + RandomLevelDownloader._readyLevels.Count.ToString() + "/" + RandomLevelDownloader.numToHaveReady.ToString());
      }
      else
        DevConsole.Log(DCSection.Steam, "Downloaded level had mods, and was ignored!");
    }

    public static void Update()
    {
      if (!Steam.IsInitialized() || !Network.isServer || TeamSelect2.GetSettingInt("workshopmaps") <= 0)
        return;
      if (RandomLevelDownloader._downloading != null)
      {
        if (!RandomLevelDownloader._downloading.finishedProcessing)
          return;
        if (RandomLevelDownloader._downloading.downloadResult == SteamResult.OK)
          RandomLevelDownloader.SearchDirLevels(RandomLevelDownloader._downloading.path, LevelLocation.Workshop);
        RandomLevelDownloader._downloading = (WorkshopItem) null;
      }
      else
      {
        if (RandomLevelDownloader._currentQuery != null || RandomLevelDownloader._readyLevels.Count == RandomLevelDownloader.numToHaveReady)
          return;
        RandomLevelDownloader._toFetchIndex = -1;
        RandomLevelDownloader._numFetch = 0;
        RandomLevelDownloader._currentQuery = Steam.CreateQueryAll(RandomLevelDownloader._orderMode, WorkshopType.Items);
        RandomLevelDownloader._currentQuery.requiredTags.Add("Deathmatch");
        RandomLevelDownloader._currentQuery.QueryFinished += new WorkshopQueryFinished(RandomLevelDownloader.FinishedTotalQuery);
        RandomLevelDownloader._currentQuery.fetchedData = WorkshopQueryData.TotalOnly;
        RandomLevelDownloader._currentQuery.Request();
        DevConsole.Log(DCSection.Steam, "Querying for random levels.");
      }
    }
  }
}
