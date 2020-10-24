// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelPlaylist
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  public class LevelPlaylist
  {
    public List<string> levels = new List<string>();

    public XElement Serialize()
    {
      XElement xelement1 = new XElement((XName) "playlist");
      foreach (object level in this.levels)
      {
        XElement xelement2 = new XElement((XName) "element", level);
        xelement1.Add((object) xelement2);
      }
      return xelement1;
    }

    public void Deserialize(XElement node)
    {
      this.levels.Clear();
      foreach (XElement element in node.Elements())
      {
        if (element.Name.LocalName == "element")
          this.levels.Add(element.Value);
      }
    }
  }
}
