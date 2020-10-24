// Decompiled with JetBrains decompiler
// Type: DuckGame.CameraZoom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("special")]
  public class CameraZoom : Thing
  {
    private float _zoomMult = 1f;
    public EditorProperty<bool> overFollow = new EditorProperty<bool>(false);
    public EditorProperty<bool> allowWarps = new EditorProperty<bool>(false);

    public float zoomMult
    {
      get => this._zoomMult;
      set => this._zoomMult = value;
    }

    public CameraZoom()
      : base()
    {
      this.graphic = new Sprite("swirl");
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._canFlip = false;
      this._visibleInGame = false;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("zoom", (object) this._zoomMult);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this._zoomMult = node.GetProperty<float>("zoom");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "zoom", (object) Change.ToString((object) this._zoomMult)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "zoom");
      if (xelement != null)
        this._zoomMult = Convert.ToSingle(xelement.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      ContextMenu contextMenu = base.GetContextMenu();
      contextMenu.AddItem((ContextMenu) new ContextSlider("Zoom", (IContextListener) null, new FieldBinding((object) this, "zoomMult", 0.5f, 4f), 0.1f));
      return contextMenu;
    }
  }
}
