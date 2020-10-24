// Decompiled with JetBrains decompiler
// Type: DuckGame.WireMount
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("stuff|wires")]
  [BaggedProperty("isOnlineCapable", true)]
  public class WireMount : Thing, IWirePeripheral
  {
    private SpriteMap _sprite;
    public StateBinding _containedThingBinding = new StateBinding(nameof (_containedThing));
    public StateBinding _actionBinding = (StateBinding) new StateFlagBinding(new string[1]
    {
      "action"
    });
    public Thing _containedThing;
    private System.Type _contains;
    public EditorProperty<float> mountAngle = new EditorProperty<float>(0.0f, min: -360f, max: 360f, increment: 5f);

    public System.Type contains
    {
      get => this._contains;
      set
      {
        if (this._contains != value && value != (System.Type) null)
          this._containedThing = Editor.CreateObject(value) as Thing;
        this._contains = value;
      }
    }

    public WireMount(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("wireMount", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Wire Mount";
      this.layer = Layer.Foreground;
      this._canFlip = true;
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
    }

    public override void Initialize()
    {
      if (!(Level.current is Editor) && this._containedThing != null && Network.isServer)
      {
        this._containedThing.owner = (Thing) this;
        Level.Add(this._containedThing);
      }
      base.Initialize();
    }

    public override void Update()
    {
      if (this._containedThing != null)
      {
        this._containedThing.owner = (Thing) this;
        if (this._containedThing.removeFromLevel)
        {
          this._containedThing = (Thing) null;
        }
        else
        {
          this._containedThing.offDir = this.flipHorizontal ? (sbyte) -1 : (sbyte) 1;
          this._containedThing.position = this.position;
          this._containedThing.depth = this.depth + 10;
          this._containedThing.layer = this.layer;
          this._containedThing.angleDegrees = this.mountAngle.value;
          if (this._containedThing is Gun)
          {
            Gun containedThing1 = this._containedThing as Gun;
            Vec2 vec2 = -containedThing1.barrelVector * (containedThing1.kick * 5f);
            Thing containedThing2 = this._containedThing;
            containedThing2.position = containedThing2.position + vec2;
          }
        }
      }
      base.Update();
    }

    public override void Terminate() => base.Terminate();

    public override void Draw()
    {
      if (this._containedThing != null && Level.current is Editor)
      {
        this._containedThing.offDir = this.flipHorizontal ? (sbyte) -1 : (sbyte) 1;
        this._containedThing.position = this.position;
        this._containedThing.depth = this.depth + 10;
        this._containedThing.layer = this.layer;
        this._containedThing.angleDegrees = this.mountAngle.value;
        this._containedThing.Draw();
      }
      base.Draw();
    }

    public void Pulse(int type, WireTileset wire)
    {
      Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      if (!(this._containedThing is Holdable containedThing))
        return;
      Thing.Fondle((Thing) containedThing, DuckNetwork.localConnection);
      switch (type)
      {
        case 0:
          this.action = true;
          containedThing.UpdateAction();
          this.action = false;
          containedThing.UpdateAction();
          break;
        case 1:
          this.action = true;
          break;
        case 2:
          this.action = false;
          break;
      }
    }
  }
}
