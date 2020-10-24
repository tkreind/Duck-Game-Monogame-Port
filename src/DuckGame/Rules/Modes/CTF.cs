// Decompiled with JetBrains decompiler
// Type: DuckGame.CTF
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class CTF : GameMode
  {
    public static bool hasWinner;
    public static bool winner;

    protected override void Initialize() => CTF.hasWinner = false;

    protected override void Start()
    {
    }

    public static void CaptureFlag(bool team)
    {
      CTF.hasWinner = true;
      CTF.winner = team;
    }

    protected override void Update()
    {
      if (!this._matchOver)
      {
        List<Team> teamList = new List<Team>();
        int num = 0;
        foreach (Team team in Teams.all)
        {
          if (team.activeProfiles.Count<Profile>() != 0)
          {
            foreach (Profile activeProfile in team.activeProfiles)
            {
              if (activeProfile.duck != null)
              {
                activeProfile.duck.ctfTeamIndex = num;
                if (!activeProfile.duck.dead)
                {
                  if (activeProfile.duck.converted != null && activeProfile.duck.converted.profile.team != activeProfile.team)
                  {
                    if (!teamList.Contains(activeProfile.duck.converted.profile.team))
                      teamList.Add(activeProfile.duck.converted.profile.team);
                  }
                  else if (!teamList.Contains(team))
                    teamList.Add(team);
                }
                else
                {
                  activeProfile.duck.position = activeProfile.duck.respawnPos;
                  if (Level.current.camera is FollowCam)
                    (Level.current.camera as FollowCam).Add((Thing) activeProfile.duck);
                  activeProfile.duck.respawnTime += 0.016f;
                  if ((double) activeProfile.duck.respawnTime > 1.5)
                  {
                    activeProfile.duck.respawnTime = 0.0f;
                    activeProfile.duck.dead = false;
                    if (activeProfile.duck.ragdoll != null)
                      activeProfile.duck.ragdoll.Unragdoll();
                    activeProfile.duck.position = activeProfile.duck.respawnPos;
                    activeProfile.duck.isGhost = true;
                    activeProfile.duck.immobilized = false;
                    activeProfile.duck.crouch = false;
                    activeProfile.duck.sliding = false;
                    activeProfile.duck._cooked = (CookedDuck) null;
                    activeProfile.duck.onFire = false;
                    activeProfile.duck.unfocus = 1f;
                    if (activeProfile.duck._trapped != null)
                      Level.Remove((Thing) activeProfile.duck._trapped);
                    activeProfile.duck._trapped = (TrappedDuck) null;
                    if (Level.current.camera is FollowCam)
                      (Level.current.camera as FollowCam).Add((Thing) activeProfile.duck);
                    Level.Add((Thing) activeProfile.duck);
                  }
                }
              }
            }
            ++num;
          }
        }
        if (CTF.hasWinner)
          this.EndMatch();
      }
      base.Update();
    }

    protected override List<Duck> AssignSpawns() => Spawn.SpawnCTF().OrderBy<Duck, float>((Func<Duck, float>) (sp => sp.x)).ToList<Duck>();

    protected override Level GetNextLevel() => (Level) new CTFLevel(Deathmatch.RandomLevelString(GameMode.previousLevel, "ctf"));

    protected override List<Profile> AddPoints()
    {
      List<Profile> profileList = new List<Profile>();
      List<Team> teamList = new List<Team>();
      List<Team> source = new List<Team>();
      int num1 = CTF.winner ? 0 : 1;
      int num2 = 0;
      foreach (Team team in Teams.all)
      {
        if (team.activeProfiles.Count<Profile>() != 0)
        {
          foreach (Profile activeProfile in team.activeProfiles)
          {
            if (activeProfile.duck != null && num2 == num1)
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
          ++num2;
        }
      }
      if (source.Count <= 1 && source.Count > 0)
      {
        source.AddRange((IEnumerable<Team>) teamList);
        List<int> idxs = new List<int>();
        foreach (Team team in source)
        {
          foreach (Profile activeProfile in team.activeProfiles)
          {
            if (activeProfile.duck != null && !activeProfile.duck.dead)
            {
              profileList.Add(activeProfile);
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
      return profileList;
    }

    public CTF()
      : base()
    {
    }
  }
}
