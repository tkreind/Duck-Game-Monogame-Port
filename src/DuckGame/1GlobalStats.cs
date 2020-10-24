// Decompiled with JetBrains decompiler
// Type: DuckGame.Global
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  internal static class Global
  {
    private static GlobalData _data = new GlobalData();

    public static GlobalData data
    {
      get => Global._data;
      set => Global._data = value;
    }

    public static void Initialize() => Global.Load();

    public static void Kill(Duck d, DestroyType type)
    {
      if (!(d.team.name == "SWACK"))
        return;
      ++Global.data.killsAsSwack;
    }

    public static void WinLevel(Team t)
    {
    }

    public static void WinMatch(Team t)
    {
      if (!Global._data.hatWins.ContainsKey(t.name))
        Global._data.hatWins[t.name] = 0;
      Dictionary<string, int> hatWins;
      string name;
      (hatWins = Global._data.hatWins)[name = t.name] = hatWins[name] + 1;
    }

    public static void PlayCustomLevel(string lev)
    {
      if (!Global._data.customMapPlayCount.ContainsKey(lev))
        Global._data.customMapPlayCount[lev] = 0;
      Dictionary<string, int> customMapPlayCount;
      string key;
      (customMapPlayCount = Global._data.customMapPlayCount)[key = lev] = customMapPlayCount[key] + 1;
    }

    public static void Save()
    {
      XDocument doc = new XDocument();
      XElement xelement = new XElement((XName) "GlobalData");
      xelement.Add((object) Global._data.Serialize());
      doc.Add((object) xelement);
      string path = DuckFile.optionsDirectory + "/global.dat";
      DuckFile.SaveXDocument(doc, path);
    }

    public static void Load()
    {
      XDocument xdocument = DuckFile.LoadXDocument(DuckFile.optionsDirectory + "/global.dat");
      if (xdocument == null)
        return;
      Profile profile = new Profile("");
      IEnumerable<XElement> source = xdocument.Elements((XName) "GlobalData");
      if (source == null)
        return;
      foreach (XElement element in source.Elements<XElement>())
      {
        if (element.Name.LocalName == nameof (Global))
        {
          Global._data.Deserialize(element);
          break;
        }
      }
    }
  }
}
