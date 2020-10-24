// Decompiled with JetBrains decompiler
// Type: DuckGame.ProfilesCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  public class ProfilesCore
  {
    public List<Profile> _profiles;
    public Team EnvironmentTeam = new Team("Environment", "hats/noHat", true);
    public Profile EnvironmentProfile;

    public IEnumerable<Profile> all => DuckNetwork.active ? (IEnumerable<Profile>) DuckNetwork.profiles : (IEnumerable<Profile>) this._profiles;

    public List<Profile> allCustomProfiles
    {
      get
      {
        List<Profile> profileList = new List<Profile>();
        for (int index = 4; index < this._profiles.Count; ++index)
        {
          if (this._profiles[index].steamID == 0UL || this._profiles[index] == Profiles.experienceProfile)
            profileList.Add(this._profiles[index]);
        }
        return profileList;
      }
    }

    public IEnumerable<Profile> universalProfileList
    {
      get
      {
        List<Profile> profileList = new List<Profile>((IEnumerable<Profile>) this._profiles);
        profileList.AddRange((IEnumerable<Profile>) DuckNetwork.profiles);
        return (IEnumerable<Profile>) profileList;
      }
    }

    public Profile DefaultExperienceProfile => this._profiles[0];

    public List<Profile> defaultProfiles => new List<Profile>((IEnumerable<Profile>) this._profiles.GetRange(0, 4));

    public Profile DefaultPlayer1 => this.all.ElementAt<Profile>(0);

    public Profile DefaultPlayer2 => this.all.ElementAt<Profile>(1);

    public Profile DefaultPlayer3 => this.all.ElementAt<Profile>(2);

    public Profile DefaultPlayer4 => this.all.ElementAt<Profile>(3);

    public ProfilesCore() => this.EnvironmentProfile = new Profile("Environment", InputProfile.Get("Blank"), this.EnvironmentTeam, Persona.Duck1);

    public int DefaultProfileNumber(Profile p) => this._profiles.IndexOf(p);

    public List<Profile> active
    {
      get
      {
        List<Profile> profileList = new List<Profile>();
        foreach (Profile profile in Profiles.all)
        {
          if (profile.team != null)
            profileList.Add(profile);
        }
        return profileList;
      }
    }

    public void Initialize()
    {
      this._profiles = new List<Profile>()
      {
        new Profile("Player1", InputProfile.Get("MPPlayer1"), Teams.Player1, Persona.Duck1, varID: "PLAYER1"),
        new Profile("Player2", InputProfile.Get("MPPlayer2"), Teams.Player2, Persona.Duck2, varID: "PLAYER2"),
        new Profile("Player3", InputProfile.Get("MPPlayer3"), Teams.Player3, Persona.Duck3, varID: "PLAYER3"),
        new Profile("Player4", InputProfile.Get("MPPlayer4"), Teams.Player4, Persona.Duck4, varID: "PLAYER4")
      };
      Profile.loading = true;
      string[] files = DuckFile.GetFiles(DuckFile.profileDirectory);
      List<Profile> profileList1 = new List<Profile>();
      foreach (string path in files)
      {
        XDocument xdocument = DuckFile.LoadXDocument(path);
        if (xdocument != null)
        {
          string name = xdocument.Element((XName) "Profile").Element((XName) "Name").Value;
          bool flag = false;
          Profile p = this._profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (pro => pro.name == name));
          if (p == null || !Profiles.IsDefault(p))
          {
            p = new Profile("");
            p.fileName = path;
            flag = true;
          }
          IEnumerable<XElement> source = xdocument.Elements((XName) "Profile");
          if (source != null)
          {
            using (IEnumerator<XElement> enumerator = source.Elements<XElement>().GetEnumerator())
            {
label_67:
              while (enumerator.MoveNext())
              {
                XElement current = enumerator.Current;
                if (current.Name.LocalName == "ID" && !Profiles.IsDefault(p))
                  p.SetID(current.Value);
                else if (current.Name.LocalName == "Name")
                  p.name = current.Value;
                else if (current.Name.LocalName == "Mood")
                  p.funslider = Change.ToSingle((object) current.Value);
                else if (current.Name.LocalName == "NS")
                  p.numSandwiches = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "MF")
                  p.milkFill = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "LML")
                  p.littleManLevel = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "NLM")
                  p.numLittleMen = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "LMB")
                  p.littleManBucks = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "RSXP")
                  p.roundsSinceXP = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "TimesMet")
                  p.timesMetVincent = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "TimesMet2")
                  p.timesMetVincentSale = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "TimesMet3")
                  p.timesMetVincentSell = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "TimesMet4")
                  p.timesMetVincentImport = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "TimeOfDay")
                  p.timeOfDay = Change.ToSingle((object) current.Value);
                else if (current.Name.LocalName == "CD")
                  p.currentDay = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "XtraPoints")
                  p.xp = Change.ToInt32((object) current.Value);
                else if (current.Name.LocalName == "FurniPositions")
                  p.furniturePositionData = BitBuffer.FromString(current.Value);
                else if (current.Name.LocalName == "Fowner")
                  p.furnitureOwnershipData = BitBuffer.FromString(current.Value);
                else if (current.Name.LocalName == "SteamID")
                {
                  p.steamID = Change.ToUInt64((object) current.Value);
                  if (p.steamID != 0UL)
                    profileList1.Add(p);
                }
                else if (current.Name.LocalName == "LastKnownName")
                  p.lastKnownName = current.Value;
                else if (current.Name.LocalName == "Stats")
                  p.stats.Deserialize(current);
                else if (current.Name.LocalName == "Unlocks")
                {
                  string[] strArray = current.Value.Split('|');
                  p.unlocks = new List<string>();
                  int index = 0;
                  while (true)
                  {
                    if (index < 100 && index < ((IEnumerable<string>) strArray).Count<string>())
                    {
                      if (!p.unlocks.Contains(strArray[index]))
                        p.unlocks.Add(strArray[index]);
                      ++index;
                    }
                    else
                      goto label_67;
                  }
                }
                else if (current.Name.LocalName == "Tickets")
                  p.ticketCount = Convert.ToInt32(current.Value);
                else if (current.Name.LocalName == "Mappings" && !MonoMain.defaultControls)
                {
                  p.inputMappingOverrides.Clear();
                  foreach (XElement element in current.Elements())
                  {
                    if (element.Name.LocalName == "InputMapping")
                    {
                      DeviceInputMapping deviceInputMapping = new DeviceInputMapping();
                      deviceInputMapping.Deserialize(element);
                      p.inputMappingOverrides.Add(deviceInputMapping);
                    }
                  }
                }
              }
            }
          }
          if (flag)
            this._profiles.Add(p);
        }
      }
      byte localFlippers = Profile.CalculateLocalFlippers();
      Profile p1 = (Profile) null;
      if (Steam.user != null && Steam.user.id != 0UL)
      {
        string str = Steam.user.id.ToString();
        foreach (Profile allCustomProfile in Profiles.allCustomProfiles)
        {
          if ((long) allCustomProfile.steamID == (long) Steam.user.id && allCustomProfile.id == str && allCustomProfile.rawName == str)
          {
            p1 = allCustomProfile;
            break;
          }
        }
        if (p1 == null)
        {
          p1 = new Profile(Steam.user.id.ToString(), varID: Steam.user.id.ToString());
          p1.steamID = Steam.user.id;
          Profiles.Add(p1);
          this.Save(p1);
        }
      }
      if (p1 != null)
      {
        this._profiles.Remove(p1);
        this._profiles.Insert(4, p1);
        List<Profile> source = new List<Profile>();
        List<Profile> profileList2 = new List<Profile>();
        foreach (Profile allCustomProfile in Profiles.allCustomProfiles)
        {
          string str = allCustomProfile.steamID.ToString();
          if (allCustomProfile.steamID != 0UL)
          {
            if (allCustomProfile.id == str && allCustomProfile.rawName == str)
              source.Add(allCustomProfile);
            else
              profileList2.Add(allCustomProfile);
          }
        }
        using (List<Profile>.Enumerator enumerator = profileList2.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Profile pro = enumerator.Current;
            Profile p2 = source.FirstOrDefault<Profile>((Func<Profile, bool>) (x => (long) x.steamID == (long) pro.steamID));
            if (p2 == null)
            {
              p2 = new Profile(pro.steamID.ToString(), varID: pro.steamID.ToString());
              p2.steamID = pro.steamID;
              Profiles.Add(p2);
            }
            p2.stats = (ProfileStats) ((DataClass) p2.stats + (DataClass) pro.stats);
            foreach (KeyValuePair<string, List<ChallengeSaveData>> keyValuePair in (MultiMap<string, ChallengeSaveData, List<ChallengeSaveData>>) Challenges.saveData)
            {
              ChallengeSaveData challengeSaveData1 = (ChallengeSaveData) null;
              List<ChallengeSaveData> challengeSaveDataList = new List<ChallengeSaveData>();
              foreach (ChallengeSaveData challengeSaveData2 in keyValuePair.Value)
              {
                if (challengeSaveData2.profileID == pro.id || challengeSaveData2.profileID == p2.id)
                {
                  challengeSaveData2.profileID = p2.id;
                  if (challengeSaveData1 == null)
                    challengeSaveData1 = challengeSaveData2;
                  else if (challengeSaveData2.trophy > challengeSaveData1.trophy)
                  {
                    challengeSaveDataList.Add(challengeSaveData1);
                    challengeSaveData1 = challengeSaveData2;
                  }
                  else
                    challengeSaveDataList.Add(challengeSaveData2);
                }
              }
              foreach (ChallengeSaveData challengeSaveData2 in challengeSaveDataList)
                challengeSaveData2.profileID += "OBSOLETE";
              if (challengeSaveData1 != null)
                Challenges.Save(keyValuePair.Key);
            }
            this._profiles.Remove(pro);
            DuckFile.Delete(pro.fileName);
            this.Save(p2);
          }
        }
      }
      foreach (Profile profile in this._profiles)
      {
        profile.flippers = localFlippers;
        profile.ticketCount = Challenges.GetTicketCount(profile);
        if (profile.ticketCount < 0)
          profile.ticketCount = 0;
      }
      Profile.loading = false;
    }

    public List<ProfileStatRank> GetEndOfRoundStatRankings(StatInfo stat)
    {
      List<ProfileStatRank> profileStatRankList = new List<ProfileStatRank>();
      foreach (Profile pro in this.active)
      {
        float statCalculation = pro.endOfRoundStats.GetStatCalculation(stat);
        bool flag = false;
        for (int index = 0; index < profileStatRankList.Count; ++index)
        {
          if ((double) statCalculation > (double) profileStatRankList[index].value)
          {
            profileStatRankList.Insert(index, new ProfileStatRank(stat, statCalculation, pro));
            flag = true;
            break;
          }
          if ((double) statCalculation == (double) profileStatRankList[index].value)
          {
            profileStatRankList[index].profiles.Add(pro);
            flag = true;
            break;
          }
        }
        if (!flag)
          profileStatRankList.Add(new ProfileStatRank(stat, statCalculation, pro));
      }
      return profileStatRankList;
    }

    public bool IsDefault(Profile p)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (this._profiles[index] == p)
          return true;
      }
      return false;
    }

    public void Add(Profile p)
    {
      this._profiles.Add(p);
      this.Save(p);
    }

    public void Remove(Profile p) => this._profiles.Remove(p);

    public void Delete(Profile p)
    {
      this._profiles.Remove(p);
      DuckFile.Delete(this.GetFileName(p));
    }

    public string GetFileName(Profile p)
    {
      if (p == this.EnvironmentProfile)
        return (string) null;
      if (p.linkedProfile != null)
        return this.GetFileName(p.linkedProfile);
      if (p.isNetworkProfile)
        return (string) null;
      string name = p.name;
      if (p.steamID != 0UL)
      {
        if (Steam.user == null || (long) p.steamID != (long) DG.localID)
          return (string) null;
        name = p.steamID.ToString();
      }
      return DuckFile.profileDirectory + DuckFile.ReplaceInvalidCharacters(name) + ".pro";
    }

    public void Save(Profile p)
    {
      if (p == this.EnvironmentProfile)
        return;
      if (p.linkedProfile != null)
      {
        this.Save(p.linkedProfile);
      }
      else
      {
        if (p.isNetworkProfile)
          return;
        string name = p.name;
        if (p.steamID != 0UL)
          name = p.steamID.ToString();
        XDocument doc = new XDocument();
        XElement xelement1 = new XElement((XName) "Profile");
        XElement xelement2 = new XElement((XName) "Name", (object) name);
        xelement1.Add((object) xelement2);
        XElement xelement3 = new XElement((XName) "ID", (object) p.id);
        xelement1.Add((object) xelement3);
        XElement xelement4 = new XElement((XName) "Mood", (object) p.funslider);
        xelement1.Add((object) xelement4);
        XElement xelement5 = new XElement((XName) "NS", (object) p.numSandwiches);
        xelement1.Add((object) xelement5);
        XElement xelement6 = new XElement((XName) "MF", (object) p.milkFill);
        xelement1.Add((object) xelement6);
        XElement xelement7 = new XElement((XName) "LML", (object) p.littleManLevel);
        xelement1.Add((object) xelement7);
        XElement xelement8 = new XElement((XName) "NLM", (object) p.numLittleMen);
        xelement1.Add((object) xelement8);
        XElement xelement9 = new XElement((XName) "RSXP", (object) p.roundsSinceXP);
        xelement1.Add((object) xelement9);
        XElement xelement10 = new XElement((XName) "LMB", (object) p.littleManBucks);
        xelement1.Add((object) xelement10);
        XElement xelement11 = new XElement((XName) "TimesMet", (object) p.timesMetVincent);
        xelement1.Add((object) xelement11);
        XElement xelement12 = new XElement((XName) "TimesMet2", (object) p.timesMetVincentSale);
        xelement1.Add((object) xelement12);
        XElement xelement13 = new XElement((XName) "TimesMet3", (object) p.timesMetVincentSell);
        xelement1.Add((object) xelement13);
        XElement xelement14 = new XElement((XName) "TimesMet4", (object) p.timesMetVincentImport);
        xelement1.Add((object) xelement14);
        XElement xelement15 = new XElement((XName) "TimeOfDay", (object) p.timeOfDay);
        xelement1.Add((object) xelement15);
        XElement xelement16 = new XElement((XName) "CD", (object) p.currentDay);
        xelement1.Add((object) xelement16);
        XElement xelement17 = new XElement((XName) "XtraPoints", (object) p.xp);
        xelement1.Add((object) xelement17);
        XElement xelement18 = new XElement((XName) "FurniPositions", (object) p.furniturePositionData.ToString());
        xelement1.Add((object) xelement18);
        XElement xelement19 = new XElement((XName) "Fowner", (object) p.furnitureOwnershipData.ToString());
        xelement1.Add((object) xelement19);
        XElement xelement20 = new XElement((XName) "SteamID", (object) p.steamID);
        xelement1.Add((object) xelement20);
        if (p.steamID != 0UL && Steam.user != null && (long) p.steamID == (long) Steam.user.id)
        {
          XElement xelement21 = new XElement((XName) "LastKnownName", (object) Steam.user.name);
          xelement1.Add((object) xelement21);
        }
        xelement1.Add((object) p.stats.Serialize());
        string str = "";
        foreach (string unlock in p.unlocks)
          str = str + unlock + "|";
        if (str.Length > 0)
          str = str.Substring(0, str.Length - 1);
        XElement xelement22 = new XElement((XName) "Unlocks", (object) str);
        xelement1.Add((object) xelement22);
        XElement xelement23 = new XElement((XName) "Tickets", (object) p.ticketCount);
        xelement1.Add((object) xelement23);
        XElement xelement24 = new XElement((XName) "Mappings");
        foreach (DeviceInputMapping inputMappingOverride in p.inputMappingOverrides)
          xelement24.Add((object) inputMappingOverride.Serialize());
        xelement1.Add((object) xelement24);
        doc.Add((object) xelement1);
        string path = DuckFile.profileDirectory + DuckFile.ReplaceInvalidCharacters(name) + ".pro";
        DuckFile.SaveXDocument(doc, path);
      }
    }
  }
}
