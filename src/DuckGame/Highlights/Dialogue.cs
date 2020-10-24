// Decompiled with JetBrains decompiler
// Type: DuckGame.Dialogue
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  public class Dialogue
  {
    private static MultiMap<string, string> _speechLines = new MultiMap<string, string>();

    public static void Initialize()
    {
      IEnumerable<XElement> source = XDocument.Load(TitleContainer.OpenStream("Content/dialogue/sportscaster.tlk")).Elements((XName) nameof (Dialogue));
      if (source == null)
        return;
      foreach (XElement element1 in source.Elements<XElement>())
      {
        foreach (XElement element2 in element1.Elements((XName) "Line"))
          Dialogue._speechLines.Add(element1.Name.LocalName, element2.Value);
      }
    }

    public static string GetLine(string type) => !Dialogue._speechLines.ContainsKey(type) ? (string) null : Dialogue._speechLines[type][Rando.Int(Dialogue._speechLines[type].Count - 1)];

    public static string GetRemark(
      string type,
      string name = null,
      string name2 = null,
      string extra01 = null,
      string extra02 = null)
    {
      string str = Dialogue.GetLine(type);
      if (str != null)
      {
        if (name != null)
          str = str.Replace("%NAME%", name);
        if (name2 != null)
          str = str.Replace("%NAME2%", name2);
        if (extra01 != null)
          str = str.Replace("%DATA%", extra01);
        if (extra02 != null)
          str = str.Replace("%DATA2%", extra02);
      }
      return str;
    }

    public static string GetRemark(string type, ResultData data) => data.multi ? Dialogue.GetTeamRemark(type, data.name) : Dialogue.GetIndividualRemark(type, data.name);

    public static string GetWinnerRemark(ResultData data) => data.multi ? Dialogue.GetLine("WinnerTeamRemark").Replace("%NAME%", data.name) : Dialogue.GetLine("WinnerIndividualRemark").Replace("%NAME%", data.name);

    public static string GetRunnerUpRemark(string type, ResultData data)
    {
      if (data.multi)
      {
        if (type.Contains("Positive"))
          return Dialogue.GetLine("PositiveRunnerUpTeamRemark").Replace("%NAME%", data.name);
        if (type.Contains("Neutral"))
          return Dialogue.GetLine("NeutralRunnerUpTeamRemark").Replace("%NAME%", data.name);
        return type.Contains("Negative") ? Dialogue.GetLine("NegativeRunnerUpTeamRemark").Replace("%NAME%", data.name) : "I don't know what to say!";
      }
      if (type.Contains("Positive"))
        return Dialogue.GetLine("PositiveRunnerUpIndividualRemark").Replace("%NAME%", data.name);
      if (type.Contains("Neutral"))
        return Dialogue.GetLine("NeutralRunnerUpIndividualRemark").Replace("%NAME%", data.name);
      return type.Contains("Negative") ? Dialogue.GetLine("NegativeRunnerUpIndividualRemark").Replace("%NAME%", data.name) : "I don't know what to say!";
    }

    public static string GetTeamRemark(string type, string name)
    {
      if (type.Contains("Positive"))
        return Dialogue.GetLine("PositiveTeamRemark").Replace("%NAME%", name);
      if (type.Contains("Neutral"))
        return Dialogue.GetLine("NeutralTeamRemark").Replace("%NAME%", name);
      return type.Contains("Negative") ? Dialogue.GetLine("NegativeTeamRemark").Replace("%NAME%", name) : "I don't know what to say!";
    }

    public static string GetIndividualRemark(string type, string name)
    {
      if (type.Contains("Positive"))
        return Dialogue.GetLine("PositiveIndividualRemark").Replace("%NAME%", name);
      if (type.Contains("Neutral"))
        return Dialogue.GetLine("NeutralIndividualRemark").Replace("%NAME%", name);
      return type.Contains("Negative") ? Dialogue.GetLine("NegativeIndividualRemark").Replace("%NAME%", name) : "I don't know what to say!";
    }
  }
}
