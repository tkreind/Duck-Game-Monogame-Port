// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelGenerator
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class LevelGenerator
  {
    public const int tileWidth = 12;
    public const int tileHeight = 9;
    private static List<RandomLevelData> _tiles = new List<RandomLevelData>();
    private static MultiMap<TileConnection, RandomLevelData> _connections = new MultiMap<TileConnection, RandomLevelData>();
    private static Dictionary<string, int> _used = new Dictionary<string, int>();

    public static List<RandomLevelData> tiles => LevelGenerator._tiles;

    public static List<RandomLevelData> GetTiles(
      TileConnection requirement,
      TileConnection filter)
    {
      if (requirement == TileConnection.None)
        return new List<RandomLevelData>((IEnumerable<RandomLevelData>) LevelGenerator._tiles);
      bool flag1 = (requirement & TileConnection.Left) != TileConnection.None;
      bool flag2 = (requirement & TileConnection.Right) != TileConnection.None;
      bool flag3 = (requirement & TileConnection.Up) != TileConnection.None;
      bool flag4 = (requirement & TileConnection.Down) != TileConnection.None;
      bool flag5 = (filter & TileConnection.Left) != TileConnection.None;
      bool flag6 = (filter & TileConnection.Right) != TileConnection.None;
      bool flag7 = (filter & TileConnection.Up) != TileConnection.None;
      bool flag8 = (filter & TileConnection.Down) != TileConnection.None;
      List<RandomLevelData> randomLevelDataList = new List<RandomLevelData>();
      foreach (RandomLevelData tile in LevelGenerator._tiles)
      {
        if ((tile.left || !flag1) && (tile.right || !flag2) && ((tile.up || !flag3) && (tile.down || !flag4)) && ((!tile.left || !flag5) && (!tile.right || !flag6) && ((!tile.up || !flag7) && (!tile.down || !flag8))))
          randomLevelDataList.Add(tile);
      }
      return randomLevelDataList;
    }

    public static RandomLevelData GetTile(
      TileConnection requirement,
      RandomLevelData current,
      bool canBeNull = true,
      LevGenType type = LevGenType.Any,
      Func<RandomLevelData, bool> lambdaReq = null,
      bool mirror = false,
      TileConnection filter = TileConnection.None,
      bool requiresSpawns = false)
    {
      List<RandomLevelData> tiles = LevelGenerator.GetTiles(requirement, filter);
      RandomLevelData randomLevelData1 = new RandomLevelData();
      bool flag = false;
      while (tiles.Count != 0)
      {
        RandomLevelData randomLevelData2 = tiles[Rando.Int(tiles.Count - 1)];
        if (randomLevelData2.numSpawns <= 0 && requiresSpawns)
          tiles.Remove(randomLevelData2);
        else if (lambdaReq != null && !lambdaReq(randomLevelData2))
          tiles.Remove(randomLevelData2);
        else if (mirror && !randomLevelData2.canMirror)
          tiles.Remove(randomLevelData2);
        else if (mirror && (randomLevelData2.flip || requirement == TileConnection.Right && !randomLevelData2.left))
        {
          tiles.Remove(randomLevelData2);
        }
        else
        {
          int num = 0;
          if (LevelGenerator._used.TryGetValue(randomLevelData2.file, out num) && num >= randomLevelData2.max)
          {
            tiles.Remove(randomLevelData2);
          }
          else
          {
            if (tiles.Count == 1 && !canBeNull)
            {
              if (flag)
                return randomLevelData1;
              randomLevelData2 = tiles.First<RandomLevelData>();
            }
            else if ((double) randomLevelData2.chance != 1.0 && (double) Rando.Float(1f) >= (double) randomLevelData2.chance)
            {
              randomLevelData1 = randomLevelData2;
              tiles.Remove(randomLevelData2);
              flag = true;
              randomLevelData2 = (RandomLevelData) null;
            }
            if (randomLevelData2 != null)
            {
              if (LevelGenerator._used.ContainsKey(randomLevelData2.file))
              {
                Dictionary<string, int> used;
                string file;
                (used = LevelGenerator._used)[file = randomLevelData2.file] = used[file] + 1;
              }
              else
                LevelGenerator._used[randomLevelData2.file] = 1;
              return randomLevelData2;
            }
            if (tiles.Count == 0)
              return flag ? randomLevelData1 : (RandomLevelData) null;
          }
        }
      }
      return flag ? randomLevelData1 : (RandomLevelData) null;
    }

    public static void ReInitialize()
    {
      LevelGenerator._tiles.Clear();
      LevelGenerator._connections.Clear();
      Content.ReloadLevels("procedural");
      Content.ReloadLevels("pyramid");
      LevelGenerator.Initialize();
    }

    public static RandomLevelData LoadInTile(string tile, string realName = null)
    {
      RandomLevelData element = new RandomLevelData();
      element.file = tile;
      if (realName != null)
        element.file = realName;
      LevelData levelData = Content.GetLevel(tile) ?? DuckFile.LoadLevel(tile);
      int sideMask = levelData.proceduralData.sideMask;
      if (sideMask == 0)
        return (RandomLevelData) null;
      if ((sideMask & 1) != 0)
        element.up = true;
      if ((sideMask & 2) != 0)
        element.right = true;
      if ((sideMask & 4) != 0)
        element.down = true;
      if ((sideMask & 8) != 0)
        element.left = true;
      element.chance = levelData.proceduralData.chance;
      element.max = levelData.proceduralData.maxPerLevel;
      element.single = levelData.proceduralData.enableSingle;
      element.multi = levelData.proceduralData.enableMulti;
      element.ApplyWeaponData(levelData.proceduralData.weaponConfig);
      element.ApplySpawnerData(levelData.proceduralData.spawnerConfig);
      element.numArmor = levelData.proceduralData.numArmor;
      element.numEquipment = levelData.proceduralData.numEquipment;
      element.numKeys = levelData.proceduralData.numKeys;
      element.numLockedDoors = levelData.proceduralData.numLockedDoors;
      element.numSpawns = levelData.proceduralData.numSpawns;
      element.numTeamSpawns = levelData.proceduralData.numTeamSpawns;
      element.canMirror = levelData.proceduralData.canMirror;
      element.isMirrored = levelData.proceduralData.isMirrored;
      element.data = levelData.objects.objects;
      LevelGenerator._tiles.Add(element);
      if (element.up)
        LevelGenerator._connections.Add(TileConnection.Up, element);
      if (element.down)
        LevelGenerator._connections.Add(TileConnection.Down, element);
      if (element.left)
      {
        LevelGenerator._connections.Add(TileConnection.Left, element);
        LevelGenerator._connections.Add(TileConnection.Right, element.Flipped());
      }
      if (element.right)
      {
        LevelGenerator._connections.Add(TileConnection.Right, element);
        LevelGenerator._connections.Add(TileConnection.Left, element.Flipped());
      }
      LevelGenerator._tiles.Add(element.Flipped());
      return element;
    }

    public static void Initialize()
    {
      foreach (string level in Content.GetLevels("pyramid", LevelLocation.Content))
        LevelGenerator.LoadInTile(level);
    }

    private static bool TryReroll(
      RandomLevelData combined,
      List<RandomLevelNode> available,
      Func<RandomLevelData, bool> problem,
      Func<RandomLevelData, bool> solution,
      RandomLevelNode specific = null)
    {
      if (available.Count == 0 && specific == null)
        return false;
      if (problem(combined))
      {
        List<RandomLevelNode> randomLevelNodeList = new List<RandomLevelNode>();
        if (specific != null)
          randomLevelNodeList.Add(specific);
        else
          randomLevelNodeList.AddRange((IEnumerable<RandomLevelNode>) available);
        do
        {
          RandomLevelNode randomLevelNode = randomLevelNodeList[Rando.Int(randomLevelNodeList.Count - 1)];
          if (randomLevelNode.Reroll(solution))
          {
            available.Remove(randomLevelNode);
            goto label_10;
          }
          else
            randomLevelNodeList.Remove(randomLevelNode);
        }
        while (randomLevelNodeList.Count != 0);
        return false;
      }
label_10:
      return true;
    }

    public static RandomLevelNode MakeLevel(
      RandomLevelData tile = null,
      bool allowSymmetry = true,
      int seed = 0,
      LevGenType type = LevGenType.Any,
      int varwide = 0,
      int varhigh = 0,
      int genX = 1,
      int genY = 1,
      List<GeneratorRule> rules = null)
    {
      Random generator = Rando.generator;
      if (seed == 0)
        seed = Rando.Int(2147483646);
      Rando.generator = new Random(seed);
      bool flag1 = true;
      int num1 = 0;
      int length1;
      int length2;
      RandomLevelNode[,] randomLevelNodeArray;
      while (true)
      {
        LevelGenerator._used.Clear();
        length1 = varwide;
        length2 = varhigh;
        if (varwide == 0)
          length1 = (double) Rando.Float(1f) <= 0.800000011920929 ? 3 : 2;
        if (varhigh == 0)
        {
          float num2 = Rando.Float(1f);
          if ((double) num2 > 0.800000011920929)
            ;
          length2 = (double) num2 <= 0.349999994039536 ? 3 : 2;
        }
        if (flag1)
          length1 = length2 = 3;
        genX = length1 != 3 ? 0 : 1;
        genY = length2 != 3 ? 0 : 1;
        if (genX > length1 - 1)
          genX = length1 - 1;
        if (genY > length2 - 1)
          genY = length2 - 1;
        randomLevelNodeArray = new RandomLevelNode[length1, length2];
        for (int index1 = 0; index1 < length1; ++index1)
        {
          for (int index2 = 0; index2 < length2; ++index2)
          {
            randomLevelNodeArray[index1, index2] = new RandomLevelNode();
            randomLevelNodeArray[index1, index2].map = randomLevelNodeArray;
          }
        }
        for (int index1 = 0; index1 < length1; ++index1)
        {
          for (int index2 = 0; index2 < length2; ++index2)
          {
            RandomLevelNode randomLevelNode = randomLevelNodeArray[index1, index2];
            if (index1 > 0)
              randomLevelNode.left = randomLevelNodeArray[index1 - 1, index2];
            if (index1 < length1 - 1)
              randomLevelNode.right = randomLevelNodeArray[index1 + 1, index2];
            if (index2 > 0)
              randomLevelNode.up = randomLevelNodeArray[index1, index2 - 1];
            if (index2 < length2 - 1)
              randomLevelNode.down = randomLevelNodeArray[index1, index2 + 1];
          }
        }
        if (tile != null)
          LevelGenerator._used[tile.file] = 1;
        randomLevelNodeArray[genX, genY].kingTile = true;
        randomLevelNodeArray[genX, genY].GenerateTiles(tile, type, (double) Rando.Float(1f) > 0.300000011920929);
        List<RandomLevelNode> available = new List<RandomLevelNode>();
        for (int index1 = 0; index1 < length1; ++index1)
        {
          for (int index2 = 0; index2 < length2; ++index2)
          {
            RandomLevelNode randomLevelNode = randomLevelNodeArray[index1, index2];
            if (randomLevelNode.data != null)
              available.Add(randomLevelNode);
          }
        }
        if (rules == null)
        {
          rules = new List<GeneratorRule>();
          rules.Add(new GeneratorRule((Func<RandomLevelData, bool>) (problem => problem.numPermanentFatalWeapons < 1), (Func<RandomLevelData, bool>) (solution => solution.numPermanentFatalWeapons > 0), varMandatory: true));
          rules.Add(new GeneratorRule((Func<RandomLevelData, bool>) (problem => problem.numLockedDoors > 0 && problem.numKeys == 0), (Func<RandomLevelData, bool>) (solution => solution.numLockedDoors == 0 && solution.numKeys > 0)));
        }
        bool flag2 = false;
        foreach (GeneratorRule rule in rules)
        {
          if ((double) rule.chance == 1.0 || (double) Rando.Float(1f) < (double) rule.chance)
          {
            RandomLevelNode specific = (RandomLevelNode) null;
            if (rule.special == SpecialRule.AffectCenterTile && length1 == 3)
              specific = randomLevelNodeArray[1, Rando.Int(length2 - 1)];
            if (!LevelGenerator.TryReroll(randomLevelNodeArray[genX, genY].totalData, available, rule.problem, rule.solution, specific) && rule.mandatory)
            {
              flag2 = true;
              break;
            }
          }
        }
        if (flag2 && num1 < 6)
        {
          if (num1 > 3)
            flag1 = true;
          ++num1;
        }
        else
          break;
      }
      Rando.generator = generator;
      randomLevelNodeArray[genX, genY].seed = seed;
      randomLevelNodeArray[genX, genY].tilesWide = length1;
      randomLevelNodeArray[genX, genY].tilesHigh = length2;
      randomLevelNodeArray[genX, genY].tiles = randomLevelNodeArray;
      return randomLevelNodeArray[genX, genY];
    }
  }
}
