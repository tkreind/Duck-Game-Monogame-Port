// Decompiled with JetBrains decompiler
// Type: DuckGame.Teams
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public static class Teams
  {
    private static TeamsCore _core;

    public static TeamsCore core
    {
      get => Teams._core;
      set => Teams._core = value;
    }

    public static SpriteMap hats => Teams._core.hats;

    public static Team Player1 => Teams._core.teams[0];

    public static Team Player2 => Teams._core.teams[1];

    public static Team Player3 => Teams._core.teams[2];

    public static Team Player4 => Teams._core.teams[3];

    public static Team NullTeam => Teams._core.nullTeam;

    public static int numTeams => Teams._core.all.Count;

    public static Team GetTeam(string name) => Teams._core.all.FirstOrDefault<Team>((Func<Team, bool>) (x => x.name == name)) ?? Teams._core.teams[4];

    public static int IndexOf(Team t) => Teams._core.all.IndexOf(t);

    public static int CurrentGameTeamIndex(Team t)
    {
      List<Team> teamList = new List<Team>();
      foreach (Team team in Teams.active)
      {
        if (team.activeProfiles.Count > 1)
          teamList.Add(team);
      }
      return teamList.IndexOf(t);
    }

    public static List<Team> all => Teams._core.all;

    public static List<Team> allStock => Teams._core.allStock;

    public static List<Team> allRandomized
    {
      get
      {
        List<Team> teamList1 = new List<Team>();
        teamList1.AddRange((IEnumerable<Team>) Teams._core.all);
        List<Team> teamList2 = new List<Team>();
        while (teamList1.Count > 0)
        {
          int index = Rando.Int(teamList1.Count - 1);
          teamList2.Add(teamList1[index]);
          teamList1.RemoveAt(index);
        }
        return teamList2;
      }
    }

    public static List<Team> active
    {
      get
      {
        List<Team> teamList = new List<Team>();
        foreach (Team team in Teams.all)
        {
          if (team.activeProfiles.Count > 0)
            teamList.Add(team);
        }
        return teamList;
      }
    }

    public static List<Team> winning
    {
      get
      {
        List<Team> teamList = new List<Team>();
        foreach (Team team in Teams.all)
        {
          if (team.activeProfiles.Count > 0)
          {
            if (teamList.Count == 0 || team.score > teamList[0].score)
            {
              teamList.Clear();
              teamList.Add(team);
            }
            else if (teamList.Count != 0 && team.score == teamList[0].score)
              teamList.Add(team);
          }
        }
        return teamList;
      }
    }

    public static void AddExtraTeam(Team t) => Teams._core.extraTeams.Add(t);

    public static void Initialize()
    {
      if (Teams._core != null)
        return;
      Teams._core = new TeamsCore();
      Teams._core.Initialize();
    }
  }
}
