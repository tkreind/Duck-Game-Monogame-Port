// Decompiled with JetBrains decompiler
// Type: DuckGame.Party
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Party
  {
    private static Dictionary<Profile, int> _drinks = new Dictionary<Profile, int>();
    private static Dictionary<Profile, List<PartyPerks>> _perks = new Dictionary<Profile, List<PartyPerks>>();

    public static void AddDrink(Profile p, int num)
    {
      if (!Party._drinks.ContainsKey(p))
        Party._drinks[p] = 0;
      Dictionary<Profile, int> drinks;
      Profile key;
      (drinks = Party._drinks)[key = p] = drinks[key] + num;
    }

    public static void AddPerk(Profile p, PartyPerks perk)
    {
      if (!Party._perks.ContainsKey(p))
        Party._perks[p] = new List<PartyPerks>();
      if (Party._perks[p].Contains(perk))
        return;
      Party._perks[p].Add(perk);
    }

    public static bool HasPerk(Profile p, PartyPerks perk) => TeamSelect2.partyMode && Party._perks.ContainsKey(p) && Party._perks[p].Contains(perk);

    public static void AddRandomPerk(Profile p)
    {
      IEnumerable<PartyPerks> source = Enum.GetValues(typeof (PartyPerks)).Cast<PartyPerks>();
      Party.AddPerk(p, source.ElementAt<PartyPerks>(Rando.Int(source.Count<PartyPerks>() - 1)));
    }

    public static int GetDrinks(Profile p) => Party._drinks.ContainsKey(p) ? Party._drinks[p] : 0;

    public static List<PartyPerks> GetPerks(Profile p) => Party._perks.ContainsKey(p) ? Party._perks[p] : new List<PartyPerks>();

    public static void Clear()
    {
      List<Profile> profileList = new List<Profile>();
      foreach (KeyValuePair<Profile, int> drink in Party._drinks)
        profileList.Add(drink.Key);
      foreach (Profile key in profileList)
        Party._drinks[key] = 0;
      profileList.Clear();
      foreach (KeyValuePair<Profile, List<PartyPerks>> perk in Party._perks)
        profileList.Add(perk.Key);
      foreach (Profile key in profileList)
        Party._perks[key].Clear();
    }
  }
}
