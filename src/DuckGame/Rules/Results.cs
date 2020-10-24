// Decompiled with JetBrains decompiler
// Type: DuckGame.Results
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class Results
  {
    public static List<ResultData> teams
    {
      get
      {
        List<ResultData> resultDataList = new List<ResultData>();
        foreach (Team t in Teams.all)
        {
          if (t.activeProfiles.Count > 0)
            resultDataList.Add(new ResultData(t));
        }
        return resultDataList;
      }
    }

    public static ResultData winner
    {
      get
      {
        List<ResultData> teams = Results.teams;
        teams.Sort((Comparison<ResultData>) ((a, b) =>
        {
          if (a.score == b.score)
            return 0;
          return a.score >= b.score ? -1 : 1;
        }));
        return teams[0];
      }
    }

    public static ResultData runnerUp
    {
      get
      {
        List<ResultData> teams = Results.teams;
        teams.Sort((Comparison<ResultData>) ((a, b) =>
        {
          if (a.score == b.score)
            return 0;
          return a.score >= b.score ? -1 : 1;
        }));
        return teams[1];
      }
    }

    public static ResultData loser
    {
      get
      {
        List<ResultData> teams = Results.teams;
        teams.Sort((Comparison<ResultData>) ((a, b) =>
        {
          if (a.score == b.score)
            return 0;
          return a.score <= b.score ? -1 : 1;
        }));
        return teams[0];
      }
    }
  }
}
