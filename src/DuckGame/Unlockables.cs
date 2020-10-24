// Decompiled with JetBrains decompiler
// Type: DuckGame.Unlockables
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Unlockables
  {
    private static List<Unlockable> _unlocks = new List<Unlockable>();
    private static HashSet<Unlockable> _pendingUnlocks = new HashSet<Unlockable>();

    public static void Initialize()
    {
      Unlockables._unlocks.Add((Unlockable) new UnlockableHats("hatpack1", new List<Team>()
      {
        Teams.GetTeam("BAWB"),
        Teams.GetTeam("Frank"),
        Teams.GetTeam("Meeee")
      }, (Func<bool>) (() => Unlocks.IsUnlocked("HATTY1")), "Hat Pack 1", "Check out these nifty cool hats."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHats("hatpack2", new List<Team>()
      {
        Teams.GetTeam("Pulpy"),
        Teams.GetTeam("Joey"),
        Teams.GetTeam("Cowboys")
      }, (Func<bool>) (() => Unlocks.IsUnlocked("HATTY2")), "Hat Pack 2", "More cool hats! WOW!"));
      bool futz = false;
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("gamerduck", (Func<bool>) (() => Global.data.timesSpawned > (futz ? 0 : 99)), "Duck Gamer", "Spawn 100 times.", "play100"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("chancyHat", Teams.GetTeam("Chancy"), (Func<bool>) (() => Unlocks.IsUnlocked("ULTIMATE")), "Chancy", "Got platinum on all challenges", "chancy"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("ritual", (Func<bool>) (() => Global.data.timesSpawned > (futz ? 1 : 999)), "Ritual", "Spawn 1000 times.", "play1000"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("skully", Teams.GetTeam("SKULLY"), (Func<bool>) (() => Global.data.kills > (futz ? 3 : 999)), "SKULLY", "Kill 1000 Ducks", "kill1000"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("endurance", (Func<bool>) (() => Global.data.longestMatchPlayed > (futz ? 5 : 49)), "Endurance", "Play through a 50 point match", "endurance"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("outgoing", (Func<bool>) (() => Global.data.onlineWins > (futz ? 0 : 9)), "Outgoing", "Win 10 online matches", "online10"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("basement", (Func<bool>) (() => Unlocks.IsUnlocked("BASEMENTKEY")), "Basement Dweller", "Unlock the basement", "basement"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("poweruser", (Func<bool>) (() => Global.data.customMapPlayCount.Count > (futz ? 0 : 9)), "Power User", "Play on 10 different custom maps", "editor"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("drawbreaker", (Func<bool>) (() => Global.data.drawsPlayed > (futz ? 0 : 9)), "Draw Breaker", "Break 10 draws", "drawbreaker"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("hotstuff", (Func<bool>) (() => (double) Profiles.MostTimeOnFire() > (futz ? 2.0 : 899.0)), "Hot Stuff", "Spend 15 minutes on fire with any one profile", "fire"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("myboy", (Func<bool>) (() => Profiles.experienceProfile != null && Profiles.experienceProfile.numLittleMen > 0), "That's My Boy", "Raise a little man.", "myboy"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("jukebox", (Func<bool>) (() => Profiles.experienceProfile != null && Profiles.experienceProfile.numLittleMen > 7), "Jukebox Hero", "Raise eight little men.", "jukebox"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableAchievement("kingme", (Func<bool>) (() => Profiles.experienceProfile != null && Profiles.experienceProfile.xp >= DuckNetwork.GetLevel(999).xpRequired), "King Me", "Level up all the way.", "kingme"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("ballz", Teams.GetTeam("BALLZ"), (Func<bool>) (() => Global.data.ducksCrushed > (futz ? 0 : 49)), "BALLZ", "Crush 50 Ducks", "crate"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("hearts", Teams.GetTeam("Hearts"), (Func<bool>) (() => Global.data.matchesPlayed > (futz ? 0 : 49)), "<3", "Finish 50 whole matches.", "finish50"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("swackHat", Teams.GetTeam("SWACK"), (Func<bool>) (() => Global.data.matchesPlayed > 0), "SWACK", "Play through a match"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("BRODUCK", Teams.GetTeam("BRODUCK"), (Func<bool>) (() => Global.data.strafeDistance > (futz ? 0.25f : 10f)), "BRODUCK", "Strafe 10 Kilometers"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("astropal", Teams.GetTeam("astropal"), (Func<bool>) (() => Global.data.jetFuelUsed > (futz ? 5f : 200f)), "ASTROPAL", "Burn 200 gallons of rocket fuel."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("eggpal", Teams.GetTeam("eggpal"), (Func<bool>) (() => Global.data.winsAsSwack > (futz ? 0 : 4)), "EGGPAL", "Win 5 rounds as SWACK"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("brad", Teams.GetTeam("brad"), (Func<bool>) (() => Global.data.disarms > (futz ? 0 : 99)), "BRAD DUNGEON", "Disarm 100 ducks."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("brick", Teams.GetTeam("BRICK"), (Func<bool>) (() => Global.data.laserBulletsFired > (futz ? 0 : 149)), "BRICK", "Fire 150 laser bullets."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("ducks", Teams.GetTeam("DUCKS"), (Func<bool>) (() => Global.data.quacks > 999), "DUCK", "Quack 1000 times."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("funnyman", Teams.GetTeam("FUNNYMAN"), (Func<bool>) (() => Global.data.hatsStolen > 100), "FUNNYMAN", "Wear 100 different faces."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("wizards", Teams.GetTeam("Wizards"), (Func<bool>) (() => Global.data.angleShots > 25), "Wizard", "Make 25 angle trick shots."));
      string str1 = "CYCLOPS";
      if (Teams.GetTeam(str1) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str1.ToLowerInvariant(), Teams.GetTeam(str1), (Func<bool>) (() => Global.data.boughtHats.Contains("CYCLOPS")), str1, Teams.GetTeam(str1).description));
      string str2 = "MOTHERS";
      if (Teams.GetTeam(str2) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str2.ToLowerInvariant(), Teams.GetTeam(str2), (Func<bool>) (() => Global.data.boughtHats.Contains("MOTHERS")), str2, Teams.GetTeam(str2).description));
      string str3 = "BIG ROBO";
      if (Teams.GetTeam(str3) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str3.ToLowerInvariant(), Teams.GetTeam(str3), (Func<bool>) (() => Global.data.boughtHats.Contains("BIG ROBO")), str3, Teams.GetTeam(str3).description));
      string str4 = "TINCAN";
      if (Teams.GetTeam(str4) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str4.ToLowerInvariant(), Teams.GetTeam(str4), (Func<bool>) (() => Global.data.boughtHats.Contains("TINCAN")), str4, Teams.GetTeam(str4).description));
      string str5 = "WELDERS";
      if (Teams.GetTeam(str5) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str5.ToLowerInvariant(), Teams.GetTeam(str5), (Func<bool>) (() => Global.data.boughtHats.Contains("WELDERS")), str5, Teams.GetTeam(str5).description));
      string str6 = "PONYCAP";
      if (Teams.GetTeam(str6) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str6.ToLowerInvariant(), Teams.GetTeam(str6), (Func<bool>) (() => Global.data.boughtHats.Contains("PONYCAP")), str6, Teams.GetTeam(str6).description));
      string str7 = "TRICORNE";
      if (Teams.GetTeam(str7) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str7.ToLowerInvariant(), Teams.GetTeam(str7), (Func<bool>) (() => Global.data.boughtHats.Contains("TRICORNE")), str7, Teams.GetTeam(str7).description));
      string str8 = "TWINTAIL";
      if (Teams.GetTeam(str8) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str8.ToLowerInvariant(), Teams.GetTeam(str8), (Func<bool>) (() => Global.data.boughtHats.Contains("TWINTAIL")), str8, Teams.GetTeam(str8).description));
      string str9 = "MAJESTY";
      if (Teams.GetTeam(str9) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str9.ToLowerInvariant(), Teams.GetTeam(str9), (Func<bool>) (() => Global.data.boughtHats.Contains("MAJESTY")), str9, "Max out your level (holy crap!!)"));
      string str10 = "MOONWALK";
      if (Teams.GetTeam(str10) != null)
        Unlockables._unlocks.Add((Unlockable) new UnlockableHat(str10.ToLowerInvariant(), Teams.GetTeam(str10), (Func<bool>) (() => Global.data.boughtHats.Contains("MOONWALK")), str10, "Raise 8 little men."));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("devtimes", Teams.GetTeam("CAPTAIN"), (Func<bool>) (() => ((int) Profile.CalculateLocalFlippers() & 16) != 0), "UR THE BEST", "Thank you for playing Duck Game <3"));
      Unlockables._unlocks.Add((Unlockable) new UnlockableHat("eyebob", Teams.GetTeam("eyebob"), (Func<bool>) (() => Global.data.giantLaserKills > 24), "CHARGE SHOT", "Get 25 Kills With the Giant Death Laser"));
      foreach (Unlockable unlock in Unlockables._unlocks)
      {
        unlock.Initialize();
        if (unlock.CheckCondition())
          unlock.DoUnlock();
      }
    }

    public static bool HasPendingUnlocks()
    {
      if (Profiles.experienceProfile != null)
      {
        if (Profiles.experienceProfile.numLittleMen >= 7 && !Global.data.boughtHats.Contains("MOONWALK"))
          Global.data.boughtHats += "|MOONWALK";
        if (Profiles.experienceProfile.xp >= DuckNetwork.GetLevel(999).xpRequired && !Global.data.boughtHats.Contains("MAJESTY"))
          Global.data.boughtHats += "|MAJESTY";
      }
      Unlockables._pendingUnlocks.Clear();
      foreach (Unlockable unlock in Unlockables._unlocks)
      {
        if (unlock.locked)
        {
          if (unlock.CheckCondition())
          {
            if (unlock.showScreen)
              Unlockables._pendingUnlocks.Add(unlock);
            else
              unlock.DoUnlock();
          }
          else
            unlock.DoLock();
        }
      }
      Steam.StoreStats();
      return Unlockables._pendingUnlocks.Count > 0;
    }

    public static void CheckAchievements()
    {
      foreach (Unlockable unlock in Unlockables._unlocks)
      {
        if (unlock.locked && !unlock.showScreen)
        {
          if (unlock.CheckCondition())
            unlock.DoUnlock();
          else
            unlock.DoLock();
        }
      }
    }

    public static Unlockable GetUnlock(string identifier) => Unlockables._unlocks.FirstOrDefault<Unlockable>((Func<Unlockable, bool>) (x => x.id == identifier));

    public static HashSet<Unlockable> GetPendingUnlocks() => Unlockables._pendingUnlocks;
  }
}
