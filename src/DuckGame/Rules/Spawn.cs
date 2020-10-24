// Decompiled with JetBrains decompiler
// Type: DuckGame.Spawn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Spawn
  {
    private static SpawnPoint AttemptTeamSpawn(
      Team team,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      Level current = Level.current;
      List<TeamSpawn> teamSpawnList = new List<TeamSpawn>();
      foreach (TeamSpawn teamSpawn in Level.current.things[typeof (TeamSpawn)])
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

    private static SpawnPoint AttemptFreeSpawn(
      Profile profile,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      List<SpawnPoint> spawnPointList = new List<SpawnPoint>();
      foreach (FreeSpawn freeSpawn in Level.current.things[typeof (FreeSpawn)])
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

    private static SpawnPoint AttemptCTFSpawn(
      Profile profile,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned,
      bool red)
    {
      int num = red ? 1 : 2;
      List<SpawnPoint> spawnPointList = new List<SpawnPoint>();
      foreach (FreeSpawn freeSpawn in Level.current.things[typeof (FreeSpawn)])
      {
        if (!usedSpawns.Contains((SpawnPoint) freeSpawn) && (int) freeSpawn.spawnType == num)
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

    private static SpawnPoint AttemptAnySpawn(
      Profile profile,
      List<SpawnPoint> usedSpawns,
      List<Duck> spawned)
    {
      List<SpawnPoint> spawnPointList = new List<SpawnPoint>();
      foreach (SpawnPoint spawnPoint in Level.current.things[typeof (SpawnPoint)])
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

    public static List<Duck> SpawnPlayers()
    {
      List<Duck> spawned = new List<Duck>();
      List<SpawnPoint> usedSpawns = new List<SpawnPoint>();
      List<Team> teamList1 = Teams.allRandomized;
      if (GameMode.showdown)
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
          if (team.activeProfiles.Count<Profile>() == 1)
          {
            if ((Spawn.AttemptFreeSpawn(team.activeProfiles[0], usedSpawns, spawned) ?? Spawn.AttemptAnySpawn(team.activeProfiles[0], usedSpawns, spawned)) == null)
              return spawned;
          }
          else if (Spawn.AttemptTeamSpawn(team, usedSpawns, spawned) == null)
          {
            foreach (Profile activeProfile in team.activeProfiles)
            {
              if ((Spawn.AttemptFreeSpawn(activeProfile, usedSpawns, spawned) ?? Spawn.AttemptAnySpawn(activeProfile, usedSpawns, spawned)) == null)
                return spawned;
            }
          }
        }
      }
      return spawned;
    }

    public static List<Duck> SpawnCTF()
    {
      List<Duck> spawned = new List<Duck>();
      List<SpawnPoint> usedSpawns = new List<SpawnPoint>();
      List<Team> all = Teams.all;
      int num = 0;
      foreach (Team team in all)
      {
        if (team.activeProfiles.Count<Profile>() != 0)
        {
          foreach (Profile activeProfile in team.activeProfiles)
            ++activeProfile.stats.timesSpawned;
          foreach (Profile activeProfile in team.activeProfiles)
            Spawn.AttemptCTFSpawn(activeProfile, usedSpawns, spawned, num == 0);
          ++num;
        }
      }
      return spawned;
    }
  }
}
