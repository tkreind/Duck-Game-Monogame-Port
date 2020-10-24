// Decompiled with JetBrains decompiler
// Type: DuckGame.DM
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class DM : GameMode
  {
    public DM(bool validityTest = false, bool editorTestMode = false)
      : base(validityTest, editorTestMode)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
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
          this.EndMatch();
      }
      base.Update();
    }

    protected override List<Duck> AssignSpawns() => Spawn.SpawnPlayers().OrderBy<Duck, float>((Func<Duck, float>) (sp => sp.x)).ToList<Duck>();

    protected override Level GetNextLevel() => this._editorTestMode ? (Level) new GameLevel((Level.current as GameLevel).levelInputString, editorTestMode: true) : (Level) new GameLevel(Deathmatch.RandomLevelString(GameMode.previousLevel));

    protected override List<Profile> AddPoints()
    {
      List<Profile> profileList = new List<Profile>();
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
      if (source.Count <= 1 && source.Count > 0)
      {
        source.AddRange((IEnumerable<Team>) teamList);
        byte winteam = 4;
        List<int> idxs = new List<int>();
        GameMode.lastWinners.Clear();
        foreach (Team team in source)
        {
          foreach (Profile activeProfile in team.activeProfiles)
          {
            if (activeProfile.duck != null && !activeProfile.duck.dead)
            {
              if (!this._editorTestMode)
              {
                if (Teams.active.Count > 1 && Network.isActive && activeProfile.connection == DuckNetwork.localConnection)
                  DuckNetwork.GiveXP("Rounds Won", 1, 4, firstCap: 10, secondCap: 20);
                activeProfile.stats.lastWon = DateTime.Now;
                ++activeProfile.stats.matchesWon;
              }
              profileList.Add(activeProfile);
              Profile p = activeProfile;
              if (activeProfile.duck.converted != null)
              {
                p = activeProfile.duck.converted.profile;
                winteam = p.networkIndex;
              }
              GameMode.lastWinners.Add(activeProfile);
              PlusOne plusOne = new PlusOne(0.0f, 0.0f, p);
              plusOne.anchor = (Anchor) (Thing) activeProfile.duck;
              plusOne.anchor.offset = new Vec2(0.0f, -16f);
              idxs.Add((int) activeProfile.duck.netProfileIndex);
              Level.Add((Thing) plusOne);
            }
          }
        }
        if (Network.isActive && Network.isServer)
          Send.Message((NetMessage) new NMAssignWin(idxs, winteam));
        ++source.First<Team>().score;
      }
      return profileList;
    }
  }
}
