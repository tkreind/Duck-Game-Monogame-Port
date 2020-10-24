// Decompiled with JetBrains decompiler
// Type: DuckGame.SubBackgroundTile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Xml.Linq;

namespace DuckGame
{
  public class SubBackgroundTile : Thing, IStaticRender
  {
    public SubBackgroundTile(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public override void Initialize()
    {
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "frame", (object) (this.graphic as SpriteMap).frame));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "frame");
      if (xelement != null)
        (this.graphic as SpriteMap).frame = Convert.ToInt32(xelement.Value);
      return true;
    }

    public override ContextMenu GetContextMenu() => (ContextMenu) null;
  }
}
