// Decompiled with JetBrains decompiler
// Type: DuckGame.ForegroundTile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Xml.Linq;

namespace DuckGame
{
  public class ForegroundTile : Thing
  {
    public ForegroundTile(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.layer = Layer.Foreground;
      this._isStatic = true;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("frame", (object) (this.graphic as SpriteMap).frame);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      (this.graphic as SpriteMap).frame = node.GetProperty<int>("frame");
      return true;
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
