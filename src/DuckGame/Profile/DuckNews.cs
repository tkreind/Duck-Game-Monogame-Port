// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckNews
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  public class DuckNews
  {
    private static List<DuckNews> _stories = new List<DuckNews>();
    private NewsSection _section;
    private string _name;
    private List<ScriptStatement> _requirements = new List<ScriptStatement>();
    private CycleMode _cycle;
    private ScriptStatement _valueCalculation;
    private ScriptStatement _valueCalculation2;
    private List<DuckNews> _subStories = new List<DuckNews>();
    private List<string> _dialogue = new List<string>();

    public static void Initialize()
    {
      foreach (string file in Content.GetFiles("Content/news"))
      {
        IEnumerable<XElement> source1 = XDocument.Load(TitleContainer.OpenStream(file)).Elements((XName) "NewsStory");
        if (source1 != null)
        {
          if (DG.isHalloween)
          {
            IEnumerable<XElement> source2 = source1.Elements<XElement>((XName) "NewsStoryHalloween");
            if (source2 != null && source2.Count<XElement>() > 0)
              source1 = source2;
          }
          DuckNews duckNews = DuckNews.Parse(source1.ElementAt<XElement>(0));
          if (duckNews != null)
            DuckNews._stories.Add(duckNews);
        }
      }
      DuckNews._stories = DuckNews._stories.OrderBy<DuckNews, int>((Func<DuckNews, int>) (x => (int) x._section)).ToList<DuckNews>();
    }

    public static List<DuckStory> CalculateStories()
    {
      foreach (Profile profile in Profiles.active)
        profile.endOfRoundStats = (ProfileStats) null;
      List<DuckStory> duckStoryList = new List<DuckStory>();
      foreach (DuckNews storey in DuckNews._stories)
      {
        List<DuckStory> story = storey.CalculateStory();
        duckStoryList.AddRange((IEnumerable<DuckStory>) story);
      }
      return duckStoryList;
    }

    public string FillString(string text, List<Profile> p)
    {
      if (p != null)
      {
        if (p.Count > 0)
          text = text.Replace("%NAME%", p[0].name);
        if (p.Count > 1)
          text = text.Replace("%NAME2%", p[1].name);
        if (p.Count > 2)
          text = text.Replace("%NAME3%", p[2].name);
        if (p.Count > 3)
          text = text.Replace("%NAME4%", p[3].name);
      }
      text = text.Replace("%PRICE%", Main.GetPriceString());
      if (this.valueCalculation != null)
      {
        object result = this.valueCalculation.result;
        switch (result)
        {
          case float _:
          case int _:
            float single = Change.ToSingle(result);
            text = text.Replace("%VALUE%", Change.ToString((object) single));
            int int32 = Convert.ToInt32(result);
            text = text.Replace("%INTVALUE%", Change.ToString((object) int32));
            break;
          case string _:
            text = text.Replace("%VALUE%", result as string);
            break;
        }
      }
      if (this.valueCalculation2 != null)
      {
        object result = this.valueCalculation2.result;
        switch (result)
        {
          case float _:
          case int _:
            float single = Change.ToSingle(result);
            text = text.Replace("%VALUE2%", Change.ToString((object) single));
            int int32 = Convert.ToInt32(result);
            text = text.Replace("%INTVALUE2%", Change.ToString((object) int32));
            break;
          case string _:
            text = text.Replace("%VALUE2%", result as string);
            break;
        }
      }
      return text;
    }

    public List<DuckStory> CalculateStory()
    {
      List<DuckStory> duckStoryList = new List<DuckStory>();
      List<Profile> p = new List<Profile>();
      if (this._cycle == CycleMode.Once)
      {
        p.Add(Profiles.DefaultPlayer1);
        duckStoryList.AddRange((IEnumerable<DuckStory>) this.CalculateStory(p));
      }
      else if (this._cycle == CycleMode.PerProfile)
      {
        foreach (Profile profile in Profiles.active)
        {
          p.Add(profile);
          duckStoryList.AddRange((IEnumerable<DuckStory>) this.CalculateStory(p));
          p.Clear();
        }
      }
      else if (this._cycle == CycleMode.PerPosition && this._valueCalculation != null)
      {
        List<List<Profile>> source = new List<List<Profile>>();
        List<Profile> active = Profiles.active;
        foreach (Profile profile in Profiles.active)
        {
          float num = -999999f;
          Script.activeProfile = profile;
          object result = this.valueCalculation.result;
          if (result != null && result is float || (result is int || result is double))
            num = Change.ToSingle(result);
          profile.storeValue = num;
          bool flag = false;
          for (int index = 0; index < source.Count; ++index)
          {
            if ((double) source[index][0].storeValue < (double) num)
            {
              source.Insert(index, new List<Profile>());
              source[index].Add(profile);
              flag = true;
              break;
            }
            if ((double) source[index][0].storeValue == (double) num)
            {
              source[index].Add(profile);
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            source.Add(new List<Profile>());
            source.Last<List<Profile>>().Add(profile);
          }
        }
        source.Reverse();
        Script.positions = source;
        int num1 = source.Count - 1;
        foreach (List<Profile> profileList in source)
        {
          Script.currentPosition = num1;
          p.AddRange((IEnumerable<Profile>) profileList);
          duckStoryList.AddRange((IEnumerable<DuckStory>) this.CalculateStory(p));
          p.Clear();
          --num1;
        }
      }
      return duckStoryList;
    }

    public List<DuckStory> CalculateStory(List<Profile> p)
    {
      List<DuckStory> duckStoryList = new List<DuckStory>();
      Script.activeNewsStory = this;
      if (p == null || p.Count > 0)
        Script.activeProfile = p[0];
      foreach (ScriptStatement requirement in this._requirements)
      {
        if (requirement.result is bool result && !result)
          return duckStoryList;
      }
      if (this._dialogue.Count > 0)
      {
        DuckStory duckStory = new DuckStory()
        {
          section = this._section,
          text = this._dialogue[Rando.Int(this._dialogue.Count - 1)]
        };
        duckStory.text = this.FillString(duckStory.text, p);
        duckStoryList.Add(duckStory);
      }
      foreach (DuckNews subStorey in this._subStories)
      {
        if (subStorey._valueCalculation == null)
          subStorey._valueCalculation = this._valueCalculation;
        if (subStorey._valueCalculation2 == null)
          subStorey._valueCalculation2 = this._valueCalculation2;
        if (subStorey._section == NewsSection.None)
          subStorey._section = this._section;
        if (subStorey._cycle == CycleMode.None)
          duckStoryList.AddRange((IEnumerable<DuckStory>) subStorey.CalculateStory(p));
        else
          duckStoryList.AddRange((IEnumerable<DuckStory>) subStorey.CalculateStory());
      }
      return duckStoryList;
    }

    public static DuckNews Parse(XElement rootElement)
    {
      DuckNews duckNews1 = new DuckNews();
      XAttribute xattribute1 = rootElement.Attributes((XName) "name").FirstOrDefault<XAttribute>();
      if (xattribute1 != null)
        duckNews1._name = xattribute1.Value;
      foreach (XElement element in rootElement.Elements())
      {
        if (element.Name == (XName) "Section")
        {
          XAttribute xattribute2 = element.Attributes((XName) "name").FirstOrDefault<XAttribute>();
          if (xattribute2 != null)
          {
            try
            {
              duckNews1._section = (NewsSection) Enum.Parse(typeof (NewsSection), xattribute2.Value);
            }
            catch
            {
              return (DuckNews) null;
            }
          }
        }
        else if (element.Name == (XName) "Requirement")
        {
          Script.activeProfile = Profiles.DefaultPlayer1;
          duckNews1._requirements.Add(ScriptStatement.Parse(element.Value + " "));
        }
        else if (element.Name == (XName) "Dialogue")
        {
          XAttribute xattribute2 = element.Attributes((XName) "value").FirstOrDefault<XAttribute>();
          if (xattribute2 != null)
            duckNews1._dialogue.Add(xattribute2.Value);
        }
        else if (element.Name == (XName) "VALUE")
        {
          Script.activeProfile = Profiles.DefaultPlayer1;
          duckNews1._valueCalculation = ScriptStatement.Parse(element.Value + " ");
        }
        else if (element.Name == (XName) "VALUE2")
        {
          Script.activeProfile = Profiles.DefaultPlayer1;
          duckNews1._valueCalculation2 = ScriptStatement.Parse(element.Value + " ");
        }
        else if (element.Name == (XName) "Cycle")
        {
          XAttribute xattribute2 = element.Attributes((XName) "value").FirstOrDefault<XAttribute>();
          if (xattribute2 != null)
            duckNews1._cycle = (CycleMode) Enum.Parse(typeof (CycleMode), xattribute2.Value);
        }
        else if (element.Name == (XName) "SubStory")
        {
          DuckNews duckNews2 = DuckNews.Parse(element);
          duckNews1._subStories.Add(duckNews2);
        }
      }
      return duckNews1;
    }

    public ScriptStatement valueCalculation => this._valueCalculation;

    public ScriptStatement valueCalculation2 => this._valueCalculation2;
  }
}
