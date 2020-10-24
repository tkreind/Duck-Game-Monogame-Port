﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Crowd
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Crowd : Thing
  {
    private static CrowdCore _core = new CrowdCore();
    public static int crowdSeed = 0;
    private static Dictionary<Profile, FanNum> fanList = new Dictionary<Profile, FanNum>();
    private static int extraFans;

    public static CrowdCore core
    {
      get => Crowd._core;
      set => Crowd._core = value;
    }

    public static Mood mood
    {
      get => Crowd._core._mood;
      set => Crowd._core._newMood = value;
    }

    public static List<List<CrowdDuck>> _members => Crowd._core._members;

    public static int fansUsed
    {
      get => Crowd._core.fansUsed;
      set => Crowd._core.fansUsed = value;
    }

    public Crowd()
      : base()
    {
    }

    public BitBuffer NetSerialize()
    {
      BitBuffer bitBuffer = new BitBuffer();
      int num = 0;
      foreach (List<CrowdDuck> member in Crowd._members)
      {
        foreach (CrowdDuck crowdDuck in member)
        {
          if (crowdDuck.empty)
          {
            ++num;
          }
          else
          {
            if (num > 0)
            {
              bitBuffer.Write(false);
              bitBuffer.Write((byte) num);
              num = 0;
            }
            bitBuffer.Write(true);
            bitBuffer.WriteBits((object) crowdDuck.duckColor, 2);
            bitBuffer.WriteBits((object) ((crowdDuck.lastLoyalty != null ? (int) crowdDuck.lastLoyalty.networkIndex : -1) + 1), 3);
            bitBuffer.WriteBits((object) ((crowdDuck.loyalty != null ? (int) crowdDuck.loyalty.networkIndex : -1) + 1), 3);
            if (crowdDuck.loyalty != null || crowdDuck.lastLoyalty != null)
              bitBuffer.Write(crowdDuck.loyal);
          }
        }
      }
      if (num > 0)
      {
        bitBuffer.Write(false);
        bitBuffer.Write((byte) num);
      }
      bitBuffer.Write(Crowd.crowdSeed);
      return bitBuffer;
    }

    public void NetDeserialize(BitBuffer data)
    {
      foreach (List<CrowdDuck> member in Crowd._members)
      {
        foreach (Thing thing in member)
          Level.Remove(thing);
      }
      Crowd._members.Clear();
      int num1 = 0;
      float num2 = -18f;
      float num3 = 2f;
      float zpos = -30f;
      for (int index = 0; index < 4; ++index)
      {
        Crowd._members.Add(new List<CrowdDuck>());
        for (int dist = 0; dist < 45; ++dist)
        {
          Profile varLoyalty = (Profile) null;
          Profile varLastLoyalty = (Profile) null;
          bool varLoyal = false;
          int varColor = 0;
          bool flag = false;
          if (num1 == 0)
          {
            if (!data.ReadBool())
            {
              num1 = (int) data.ReadByte();
            }
            else
            {
              varColor = (int) data.ReadBits(typeof (int), 2);
              int num4 = (int) data.ReadBits(typeof (int), 3);
              int num5 = (int) data.ReadBits(typeof (int), 3);
              if (num4 > 0 || num5 > 0)
              {
                if (num5 > 0)
                  varLoyalty = DuckNetwork.profiles[num5 - 1];
                if (num4 > 0)
                  varLastLoyalty = DuckNetwork.profiles[num4 - 1];
                varLoyal = data.ReadBool();
              }
            }
          }
          if (num1 > 0)
          {
            flag = true;
            --num1;
          }
          int facing = dist < 9 ? 0 : (dist < 16 ? 1 : 2);
          CrowdDuck crowdDuck = new CrowdDuck((float) (dist * 30 - 30 + 3), num3 + num2, zpos, facing, index, dist, flag ? 0 : 1, varLoyalty, varLastLoyalty, varLoyal, varColor);
          Crowd._members[index].Add(crowdDuck);
        }
        zpos -= 20f;
        num3 -= 11f;
      }
      Crowd.crowdSeed = data.ReadInt();
      foreach (List<CrowdDuck> member in Crowd._members)
      {
        foreach (Thing thing in member)
          Level.Add(thing);
      }
      Crowd.InitSigns();
    }

    public static void GoHome()
    {
      Crowd._members.Clear();
      Crowd.fansUsed = 0;
    }

    public static void ThrowHats(Profile p)
    {
      foreach (List<CrowdDuck> member in Crowd._members)
      {
        foreach (CrowdDuck crowdDuck in member)
          crowdDuck.ThrowHat(p);
      }
    }

    public static void InitializeCrowd()
    {
      if (Network.isClient)
        return;
      if (Crowd._members.Count == 0)
      {
        Crowd.fanList.Clear();
        foreach (Profile key in Profiles.active)
          Crowd.fanList[key] = new FanNum()
          {
            profile = key,
            loyalFans = key.stats.loyalFans,
            unloyalFans = key.stats.unloyalFans
          };
        int totalFansThisGame = Profile.totalFansThisGame;
        int max = (int) (float) (20.0 + (double) Profile.totalFansThisGame * 0.100000001490116);
        if (max > 36)
          max = 36;
        Crowd.extraFans = Rando.Int(max / 2, max);
        float num = -18f;
        Crowd._members.Add(new List<CrowdDuck>());
        List<int> list1 = new List<int>();
        for (int index = 0; index < 45; ++index)
          list1.Add(index);
        list1.Shuffle<int>();
        foreach (int dist in list1)
        {
          int facing = dist < 9 ? 0 : (dist < 16 ? 1 : 2);
          Crowd._members[0].Add(new CrowdDuck((float) (dist * 30 - 30 + 3), 2f + num, -30f, facing, 0, dist));
        }
        Crowd._members[0] = Crowd._members[0].OrderBy<CrowdDuck, int>((Func<CrowdDuck, int>) (chair => chair.distVal)).ToList<CrowdDuck>();
        Crowd._members.Add(new List<CrowdDuck>());
        List<int> list2 = new List<int>();
        for (int index = 0; index < 45; ++index)
          list2.Add(index);
        list2.Shuffle<int>();
        foreach (int dist in list2)
        {
          int facing = dist < 9 ? 0 : (dist < 16 ? 1 : 2);
          Crowd._members[1].Add(new CrowdDuck((float) (dist * 30 - 30 + 3), num - 9f, -50f, facing, 1, dist));
        }
        Crowd._members[1] = Crowd._members[1].OrderBy<CrowdDuck, int>((Func<CrowdDuck, int>) (chair => chair.distVal)).ToList<CrowdDuck>();
        Crowd._members.Add(new List<CrowdDuck>());
        List<int> list3 = new List<int>();
        for (int index = 0; index < 45; ++index)
          list3.Add(index);
        list3.Shuffle<int>();
        foreach (int dist in list3)
        {
          int facing = dist < 9 ? 0 : (dist < 16 ? 1 : 2);
          Crowd._members[2].Add(new CrowdDuck((float) (dist * 30 - 30 + 3), num - 20f, -70f, facing, 2, dist));
        }
        Crowd._members[2] = Crowd._members[2].OrderBy<CrowdDuck, int>((Func<CrowdDuck, int>) (chair => chair.distVal)).ToList<CrowdDuck>();
        Crowd._members.Add(new List<CrowdDuck>());
        List<int> list4 = new List<int>();
        for (int index = 0; index < 45; ++index)
          list4.Add(index);
        list4.Shuffle<int>();
        foreach (int dist in list4)
        {
          int facing = dist < 9 ? 0 : (dist < 16 ? 1 : 2);
          Crowd._members[3].Add(new CrowdDuck((float) (dist * 30 - 30 + 3), num - 31f, -90f, facing, 3, dist));
        }
        Crowd._members[3] = Crowd._members[3].OrderBy<CrowdDuck, int>((Func<CrowdDuck, int>) (chair => chair.distVal)).ToList<CrowdDuck>();
      }
      if (Level.current is RockScoreboard)
      {
        foreach (List<CrowdDuck> member in Crowd._members)
        {
          foreach (CrowdDuck crowdDuck in member)
          {
            crowdDuck.ClearActions();
            Level.Add((Thing) crowdDuck);
          }
        }
      }
      Crowd.crowdSeed = Rando.Int(1999999);
      Crowd.InitSigns();
    }

    public static void UpdateFans()
    {
      float num1 = 0.0f;
      float num2 = 999f;
      float num3 = -999f;
      List<float> floatList = new List<float>();
      foreach (Profile profile in Profiles.active)
      {
        float profileScore = profile.endOfRoundStats.CalculateProfileScore();
        if ((double) profileScore < (double) num2)
          num2 = profileScore;
        else if ((double) profileScore > (double) num3)
          num3 = profileScore;
        num1 += profileScore;
        floatList.Add(profileScore);
      }
      float num4 = num1 / (float) Profiles.active.Count;
      foreach (List<CrowdDuck> member in Crowd._members)
      {
        foreach (CrowdDuck crowdDuck in member)
        {
          if (!crowdDuck.empty)
          {
            for (int index = 0; index < floatList.Count; ++index)
            {
              float awesomeness = floatList[index] - num4;
              if ((double) awesomeness > 0.5)
                awesomeness = 0.5f;
              if ((double) awesomeness < -0.5)
                awesomeness = -0.5f;
              crowdDuck.TryChangingAllegiance(Profiles.active[index], awesomeness);
            }
          }
        }
      }
    }

    public static bool HasFansLeft()
    {
      foreach (KeyValuePair<Profile, FanNum> fan in Crowd.fanList)
      {
        if (fan.Value.totalFans > 0)
          return true;
      }
      return Crowd.extraFans > 0;
    }

    public static FanNum GetFan()
    {
      if (Crowd.extraFans > 0 && (double) Rando.Float(1f) > 0.5)
      {
        --Crowd.extraFans;
        return (FanNum) null;
      }
      List<FanNum> fanNumList = new List<FanNum>();
      foreach (KeyValuePair<Profile, FanNum> fan in Crowd.fanList)
      {
        if (fan.Value.totalFans > 0)
          fanNumList.Add(fan.Value);
      }
      if (fanNumList.Count == 0)
        return (FanNum) null;
      FanNum fanNum;
      while (true)
      {
        do
        {
          fanNum = fanNumList[Rando.Int(fanNumList.Count - 1)];
          if (fanNumList.Count == 1)
            goto label_14;
        }
        while ((double) ((float) ((double) Math.Min(fanNum.loyalFans, 100) / 100.0 * 0.5) + Rando.Float(0.5f)) >= (double) Rando.Float(1f));
        fanNumList.Remove(fanNum);
      }
label_14:
      Profile profile = fanNum.profile;
      if (fanNum.loyalFans > 0 && fanNum.unloyalFans == 0 | (double) Rando.Float(1f) > 0.300000011920929)
      {
        --fanNum.loyalFans;
        return new FanNum()
        {
          profile = profile,
          loyalFans = 1
        };
      }
      --fanNum.unloyalFans;
      return new FanNum()
      {
        profile = profile,
        unloyalFans = 1
      };
    }

    public static int totalFans
    {
      get
      {
        int extraFans = Crowd.extraFans;
        foreach (KeyValuePair<Profile, FanNum> fan in Crowd.fanList)
          extraFans += fan.Value.totalFans;
        return extraFans;
      }
    }

    private static void InitSigns()
    {
      Random generator = Rando.generator;
      Rando.generator = new Random(Crowd.crowdSeed);
      for (int rowOnly = 0; rowOnly < 4; ++rowOnly)
      {
        string str = "DUCK GAME";
        if (Rando.Int(10000) == 1)
          str = "LOL";
        else if (Rando.Int(100) == 1)
          str = "WE LOVE IT";
        else if (Rando.Int(20) == 1)
          str = "LETS ROCK";
        else if (Rando.Int(1000000) == 1)
          str = "www.wonthelp.info";
        Profile p = (Profile) null;
        if ((double) Rando.Float(1f) > 0.5)
        {
          List<Team> winning = Teams.winning;
          if (winning.Count > 0)
          {
            Team team = winning[Rando.Int(winning.Count - 1)];
            Profile activeProfile = team.activeProfiles[Rando.Int(team.activeProfiles.Count - 1)];
            str = !Profiles.IsDefault(activeProfile) ? activeProfile.name : activeProfile.team.name;
            p = activeProfile;
          }
        }
        List<CrowdDuck> availableRow = Crowd.GetAvailableRow(str.Length, p, rowOnly);
        if ((double) Rando.Float(1f) > 0.959999978542328 && availableRow != null)
        {
          int num = 0;
          foreach (CrowdDuck crowdDuck in availableRow)
          {
            crowdDuck.SetLetter(str.Substring(num, 1), num, p: p);
            ++num;
          }
        }
      }
      Rando.generator = generator;
    }

    public override void Initialize()
    {
      base.Initialize();
      Crowd.InitializeCrowd();
    }

    private static List<CrowdDuck> GetAvailableRow(int num, Profile p, int rowOnly = -1)
    {
      List<List<CrowdDuck>> crowdDuckListList = new List<List<CrowdDuck>>();
      int index = 0;
      if (rowOnly != -1)
        index = rowOnly;
      for (; index < (rowOnly != -1 ? rowOnly + 1 : 4); ++index)
      {
        List<CrowdDuck> crowdDuckList = new List<CrowdDuck>();
        foreach (CrowdDuck crowdDuck in Crowd._members[index])
        {
          if (!crowdDuck.empty && !crowdDuck.busy && (p == null || crowdDuck.loyalty == p))
          {
            crowdDuckList.Add(crowdDuck);
          }
          else
          {
            if (crowdDuckList.Count >= num)
              crowdDuckListList.Add(crowdDuckList);
            crowdDuckList = new List<CrowdDuck>();
          }
        }
        if (crowdDuckList.Count >= num)
          crowdDuckListList.Add(crowdDuckList);
      }
      if (crowdDuckListList.Count <= 0)
        return (List<CrowdDuck>) null;
      List<CrowdDuck> range = crowdDuckListList[Rando.Int(crowdDuckListList.Count - 1)];
      if (range.Count > num)
        range = range.GetRange(Rando.Int(range.Count - num), num);
      return range;
    }

    public override void Update()
    {
      if (Crowd._core._newMood == Crowd._core._mood)
        return;
      Crowd._core._moodWait -= 0.15f;
      if ((double) Crowd._core._moodWait >= 0.0)
        return;
      Crowd._core._mood = Crowd._core._newMood;
      Crowd._core._moodWait = 1f;
    }
  }
}
