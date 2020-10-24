// Decompiled with JetBrains decompiler
// Type: DuckGame.Profiles
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Threading;

namespace DuckGame
{
  public class Profiles
  {
    private static ProfilesCore _core = new ProfilesCore();

    public static ProfilesCore core
    {
      get
      {
        if (Thread.CurrentThread != MonoMain.mainThread)
          throw new Exception("Profiles can only be accessed on the main thread.");
        return Profiles._core;
      }
      set => Profiles._core = value;
    }

    public static IEnumerable<Profile> all => Profiles._core.all;

    public static List<Profile> allCustomProfiles => Profiles._core.allCustomProfiles;

    public static IEnumerable<Profile> universalProfileList => Profiles._core.universalProfileList;

    public static List<Profile> defaultProfiles => Profiles._core.defaultProfiles;

    public static Profile DefaultPlayer1 => Profiles._core.DefaultPlayer1;

    public static Profile DefaultPlayer2 => Profiles._core.DefaultPlayer2;

    public static Profile DefaultPlayer3 => Profiles._core.DefaultPlayer3;

    public static Profile DefaultPlayer4 => Profiles._core.DefaultPlayer4;

    public static Team EnvironmentTeam => Profiles._core.EnvironmentTeam;

    public static Profile EnvironmentProfile => Profiles._core.EnvironmentProfile;

    public static int DefaultProfileNumber(Profile p) => Profiles._core.DefaultProfileNumber(p);

    public static List<Profile> active => Profiles._core.active;

    public static void Initialize() => Profiles._core.Initialize();

    public static List<ProfileStatRank> GetEndOfRoundStatRankings(StatInfo stat) => Profiles._core.GetEndOfRoundStatRankings(stat);

    public static bool IsDefault(Profile p) => Profiles._core.IsDefault(p);

    public static void Add(Profile p) => Profiles._core.Add(p);

    public static void Remove(Profile p) => Profiles._core.Remove(p);

    public static void Save(Profile p) => Profiles._core.Save(p);

    public static Profile experienceProfile
    {
      get
      {
        if (Steam.user == null)
          return (Profile) null;
        foreach (Profile profile in Profiles._core._profiles)
        {
          if ((long) profile.steamID == (long) Steam.user.id)
            return profile;
        }
        return (Profile) null;
      }
    }

    public static void Delete(Profile p) => Profiles._core.Delete(p);

    public static float MostTimeOnFire()
    {
      float num = 0.0f;
      foreach (Profile profile in Profiles.all)
      {
        if ((double) profile.stats.timeOnFire > (double) num)
          num = profile.stats.timeOnFire;
      }
      return num;
    }
  }
}
