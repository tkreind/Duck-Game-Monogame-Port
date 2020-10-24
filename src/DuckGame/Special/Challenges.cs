// Decompiled with JetBrains decompiler
// Type: DuckGame.Challenges
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  public class Challenges
  {
    public static int valueBronze = 15;
    public static int valueSilver = 5;
    public static int valueGold = 5;
    public static int valuePlatinum = 12;
    private static Dictionary<string, ChallengeData> _challenges = new Dictionary<string, ChallengeData>();
    private static MultiMap<string, ChallengeSaveData> _saveData = new MultiMap<string, ChallengeSaveData>();
    private static List<ChallengeData> _challengesInArcade;

    public static Dictionary<string, ChallengeData> challenges => Challenges._challenges;

    public static MultiMap<string, ChallengeSaveData> saveData => Challenges._saveData;

    public static void Initialize()
    {
      foreach (string level1 in Content.GetLevels("challenge", LevelLocation.Content))
      {
        LevelData level2 = Content.GetLevel(level1);
        string guid = level2.metaData.guid;
        foreach (BinaryClassChunk node in level2.objects.objects)
        {
          string property = node.GetProperty<string>("type");
          if (property != null)
          {
            if (property.Contains("DuckGame.ChallengeMode,"))
            {
              try
              {
                if (Thing.LoadThing(node, false) is ChallengeMode challengeMode)
                {
                  challengeMode.challenge.fileName = level1;
                  challengeMode.challenge.levelID = guid;
                  challengeMode.challenge.preview = level2.previewData.preview;
                  if (challengeMode.challenge.trophies[5].goodies == -1 && challengeMode.challenge.trophies[5].targets == -1)
                  {
                    int timeRequirement = challengeMode.challenge.trophies[5].timeRequirement;
                  }
                  Challenges._challenges.Add(level2.metaData.guid, challengeMode.challenge);
                }
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
      }
      foreach (string file in DuckFile.GetFiles(DuckFile.challengeDirectory))
      {
        XDocument xdocument = DuckFile.LoadXDocument(file);
        if (xdocument != null)
        {
          string withoutExtension = Path.GetFileNameWithoutExtension(file);
          XElement xelement = xdocument.Element((XName) "Data");
          if (xelement != null)
          {
            foreach (XElement element1 in xelement.Elements((XName) "challengeSaveData"))
            {
              ChallengeSaveData element2 = new ChallengeSaveData();
              element2.LegacyDeserialize(element1);
              element2.challenge = withoutExtension;
              Challenges._saveData.Add(withoutExtension, element2);
            }
          }
        }
      }
    }

    public static int GetNumTrophies(Profile p)
    {
      int num = 0;
      foreach (KeyValuePair<string, ChallengeData> challenge in Challenges._challenges)
      {
        ChallengeSaveData saveData = Challenges.GetSaveData(challenge.Value.levelID, p, true);
        if (saveData != null && saveData.trophy != TrophyType.Baseline)
          ++num;
      }
      return num;
    }

    public static ChallengeSaveData GetSaveData(
      string guid,
      Profile p,
      bool canBeNull = false)
    {
      if (Challenges._saveData.ContainsKey(guid))
      {
        foreach (ChallengeSaveData challengeSaveData in Challenges._saveData[guid])
        {
          if (challengeSaveData.profileID == p.id)
            return challengeSaveData;
        }
      }
      else if (canBeNull)
        return (ChallengeSaveData) null;
      ChallengeSaveData element = new ChallengeSaveData();
      element.profileID = p.id;
      element.challenge = guid;
      Challenges._saveData.Add(guid, element);
      return element;
    }

    public static List<ChallengeSaveData> GetAllSaveData(Profile p)
    {
      List<ChallengeSaveData> challengeSaveDataList = new List<ChallengeSaveData>();
      foreach (KeyValuePair<string, List<ChallengeSaveData>> keyValuePair in (MultiMap<string, ChallengeSaveData, List<ChallengeSaveData>>) Challenges._saveData)
      {
        foreach (ChallengeSaveData challengeSaveData in keyValuePair.Value)
        {
          if (challengeSaveData.profileID == p.id)
            challengeSaveDataList.Add(challengeSaveData);
        }
      }
      return challengeSaveDataList;
    }

    public static List<ChallengeSaveData> GetAllSaveData()
    {
      List<ChallengeSaveData> challengeSaveDataList = new List<ChallengeSaveData>();
      foreach (KeyValuePair<string, List<ChallengeSaveData>> keyValuePair in (MultiMap<string, ChallengeSaveData, List<ChallengeSaveData>>) Challenges._saveData)
      {
        foreach (ChallengeSaveData challengeSaveData in keyValuePair.Value)
          challengeSaveDataList.Add(challengeSaveData);
      }
      return challengeSaveDataList;
    }

    public static void Save(string guid)
    {
      if (!Challenges._saveData.ContainsKey(guid))
        return;
      XDocument doc = new XDocument();
      XElement xelement = new XElement((XName) "Data");
      foreach (ChallengeSaveData challengeSaveData in Challenges._saveData[guid])
        xelement.Add((object) challengeSaveData.LegacySerialize());
      doc.Add((object) xelement);
      string path = DuckFile.challengeDirectory + guid + ".dat";
      DuckFile.SaveXDocument(doc, path);
    }

    public static ChallengeData GetChallenge(string name)
    {
      ChallengeData challengeData = (ChallengeData) null;
      Challenges._challenges.TryGetValue(name, out challengeData);
      return challengeData;
    }

    public static List<ChallengeData> GetEligibleChancyChallenges(Profile p)
    {
      List<ChallengeData> challengeDataList = new List<ChallengeData>();
      foreach (KeyValuePair<string, ChallengeData> challenge in Challenges.challenges)
      {
        if (challenge.Value.requirement != "" && challenge.Value.CheckRequirement(p))
        {
          if (challenge.Value.prevchal != "")
          {
            ChallengeSaveData saveData = Challenges.GetSaveData(Challenges.GetChallenge(challenge.Value.prevchal).levelID, p, true);
            if (saveData != null && saveData.trophy > TrophyType.Baseline)
              challengeDataList.Add(challenge.Value);
          }
          else
            challengeDataList.Add(challenge.Value);
        }
      }
      return challengeDataList;
    }

    public static List<ChallengeData> GetAllChancyChallenges(
      List<ChallengeData> available = null)
    {
      List<ChallengeData> challengeDataList = new List<ChallengeData>();
      using (Dictionary<string, ChallengeData>.Enumerator enumerator = Challenges.challenges.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, ChallengeData> d = enumerator.Current;
          if (d.Value.requirement != "" && (d.Value.prevchal == null || (d.Value.prevchal == "" || available == null) || available.FirstOrDefault<ChallengeData>((Func<ChallengeData, bool>) (x => x.fileName == d.Value.prevchal)) != null))
            challengeDataList.Add(d.Value);
        }
      }
      return challengeDataList;
    }

    public static List<ChallengeData> GetEligibleIncompleteChancyChallenges(
      Profile p)
    {
      List<ChallengeData> chancyChallenges = Challenges.GetEligibleChancyChallenges(p);
      List<ChallengeData> challengeDataList = new List<ChallengeData>();
      foreach (ChallengeData challengeData in chancyChallenges)
      {
        ChallengeSaveData saveData = Challenges.GetSaveData(challengeData.levelID, Profiles.active[0], true);
        if (saveData == null || saveData.trophy < TrophyType.Bronze)
          challengeDataList.Add(challengeData);
      }
      return challengeDataList;
    }

    public static float GetChallengeSkillIndex()
    {
      int num1 = 0;
      int num2 = 0;
      List<ChallengeData> available = new List<ChallengeData>();
      if (!(Level.current is ArcadeLevel arcadeLevel))
        arcadeLevel = ArcadeLevel.currentArcade;
      if (arcadeLevel == null)
        return 0.0f;
      foreach (ArcadeMachine challenge1 in arcadeLevel._challenges)
      {
        foreach (string challenge2 in challenge1.data.challenges)
          available.Add(Challenges.GetChallenge(challenge2));
      }
      foreach (ChallengeData allChancyChallenge in Challenges.GetAllChancyChallenges(available))
        available.Add(allChancyChallenge);
      foreach (KeyValuePair<string, ChallengeData> challenge in Challenges._challenges)
      {
        if (available.Contains(challenge.Value))
        {
          num2 += 4;
          ChallengeSaveData saveData = Challenges.GetSaveData(challenge.Value.levelID, Profiles.active[0], true);
          if (saveData != null)
            num1 += (int) saveData.trophy;
        }
      }
      return (float) num1 / (float) num2;
    }

    public static List<ChallengeData> challengesInArcade
    {
      get
      {
        if (Challenges._challengesInArcade == null)
        {
          Challenges._challengesInArcade = new List<ChallengeData>();
          ArcadeLevel arcadeLevel = new ArcadeLevel(Content.GetLevelID("arcade"));
          arcadeLevel.bareInitialize = true;
          arcadeLevel.InitializeMachines();
          if (arcadeLevel != null)
          {
            foreach (ArcadeMachine challenge1 in arcadeLevel._challenges)
            {
              foreach (string challenge2 in challenge1.data.challenges)
                Challenges.challengesInArcade.Add(Challenges.GetChallenge(challenge2));
            }
            foreach (ChallengeData allChancyChallenge in Challenges.GetAllChancyChallenges(Challenges._challengesInArcade))
              Challenges._challengesInArcade.Add(allChancyChallenge);
          }
        }
        return Challenges._challengesInArcade;
      }
    }

    public static int GetTicketCount(Profile p)
    {
      int num = 0;
      foreach (KeyValuePair<string, ChallengeData> challenge in Challenges._challenges)
      {
        if (Challenges.challengesInArcade.Contains(challenge.Value))
        {
          ChallengeSaveData saveData = Challenges.GetSaveData(challenge.Value.levelID, p, true);
          if (saveData != null)
          {
            if (saveData.trophy >= TrophyType.Bronze)
              num += Challenges.valueBronze;
            if (saveData.trophy >= TrophyType.Silver)
              num += Challenges.valueSilver;
            if (saveData.trophy >= TrophyType.Gold)
              num += Challenges.valueGold;
            if (saveData.trophy >= TrophyType.Platinum)
              num += Challenges.valuePlatinum;
          }
        }
      }
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Any))
      {
        if (unlock.ProfileUnlocked(p))
          num -= unlock.cost;
      }
      return num;
    }
  }
}
