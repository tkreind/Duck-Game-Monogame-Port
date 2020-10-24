// Decompiled with JetBrains decompiler
// Type: DuckGame.Equipper
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("spawns")]
  [BaggedProperty("isOnlineCapable", true)]
  public class Equipper : Thing
  {
    private System.Type _contains;
    public EditorProperty<int> radius = new EditorProperty<int>(0, max: 128f, increment: 1f, minSpecial: "INF");
    public EditorProperty<bool> infinite = new EditorProperty<bool>(false);
    private RenderTarget2D _preview;
    private Sprite _previewSprite;

    public System.Type contains
    {
      get => this._contains;
      set
      {
        this._contains = value;
        if (Level.skipInitialize)
          return;
        if (this._preview == null)
          this._preview = new RenderTarget2D(32, 32);
        Thing containedInstance = this.GetContainedInstance();
        if (containedInstance == null)
          return;
        this._previewSprite = containedInstance.GetEditorImage(32, 32, true, target: this._preview);
      }
    }

    public Equipper(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.serverOnly = true;
      this.graphic = new Sprite("equipper");
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(14f, 14f);
      this.collisionOffset = new Vec2(-7f, -7f);
      this.depth = (Depth) 0.5f;
      this._canFlip = false;
      this._visibleInGame = false;
    }

    public Thing GetContainedInstance(Vec2 pos = default (Vec2))
    {
      if (this.contains == (System.Type) null)
        return (Thing) null;
      object[] constructorParameters = Editor.GetConstructorParameters(this.contains);
      if (((IEnumerable<object>) constructorParameters).Count<object>() > 1)
      {
        constructorParameters[0] = (object) pos.x;
        constructorParameters[1] = (object) pos.y;
      }
      PhysicsObject thing = Editor.CreateThing(this.contains, constructorParameters) as PhysicsObject;
      if (thing is Gun)
        (thing as Gun).infinite = this.infinite;
      return (Thing) thing;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("contains", this.contains != (System.Type) null ? (object) ModLoader.SmallTypeName(this.contains) : (object) "");
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.contains = Editor.GetType(node.GetProperty<string>("contains"));
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "contains", this.contains != (System.Type) null ? (object) this.contains.AssemblyQualifiedName : (object) ""));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "contains");
      if (xelement != null)
        this.contains = Editor.GetType(xelement.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      FieldBinding radioBinding = new FieldBinding((object) this, "contains");
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.InitializeGroups(new EditorGroup(typeof (PhysicsObject)), radioBinding);
      return (ContextMenu) contextMenu;
    }

    public override string GetDetailsString()
    {
      string str = "EMPTY";
      if (this.contains != (System.Type) null)
        str = this.contains.Name;
      return this.contains == (System.Type) null ? base.GetDetailsString() : base.GetDetailsString() + "Contains: " + str;
    }

    public override void DrawHoverInfo()
    {
      string text = "EMPTY";
      if (this.contains != (System.Type) null)
        text = this.contains.Name;
      Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), -16f), Color.White, (Depth) 0.9f);
      if (this.radius.value == 0)
        return;
      Graphics.DrawCircle(this.position, (float) this.radius.value, Color.Red, depth: ((Depth) 0.9f));
    }

    public override void Draw()
    {
      base.Draw();
      if (this._previewSprite == null)
        return;
      this._previewSprite.depth = this.depth + 1;
      this._previewSprite.scale = new Vec2(0.5f, 0.5f);
      this._previewSprite.center = new Vec2(16f, 16f);
      Graphics.Draw(this._previewSprite, this.x, this.y);
    }
  }
}
