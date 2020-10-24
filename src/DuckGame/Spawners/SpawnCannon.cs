// Decompiled with JetBrains decompiler
// Type: DuckGame.SpawnCannon
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("spawns")]
  public class SpawnCannon : ItemSpawner, IWirePeripheral
  {
    public float fireDirection;
    public float firePower = 5f;
    private PhysicsObject _hoverThing;

    public float direction => this.fireDirection + (this.flipHorizontal ? 180f : 0.0f);

    public SpawnCannon(float xpos, float ypos, System.Type c = null)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("cannon", 18, 18);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(6f, 9f);
      this.collisionSize = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-6f, -6f);
      this.depth = (Depth) -0.6f;
      this.contains = c;
      this.hugWalls = WallHug.None;
    }

    public override void Initialize()
    {
      if (!this.spawnOnStart)
        return;
      this._spawnWait = this.spawnTime;
    }

    public void Spawn()
    {
      if (this.randomSpawn)
      {
        List<System.Type> physicsObjects = ItemBox.GetPhysicsObjects(Editor.Placeables);
        this.contains = physicsObjects[Rando.Int(physicsObjects.Count - 1)];
      }
      this._spawnWait = 0.0f;
      ++this._numSpawned;
      if (!(Editor.CreateThing(this.contains) is PhysicsObject thing))
        return;
      Vec2 vec2 = Maths.AngleToVec(Maths.DegToRad(this.direction)) * this.firePower;
      thing.position = this.position + vec2.normalized * 8f;
      thing.hSpeed = vec2.x;
      thing.vSpeed = vec2.y;
      Level.Add((Thing) thing);
      Level.Add((Thing) SmallSmoke.New(thing.x, thing.y));
      Level.Add((Thing) SmallSmoke.New(thing.x, thing.y));
      SFX.Play("netGunFire", Rando.Float(0.9f, 1f), Rando.Float(-0.1f, 0.1f));
      if (thing is Equipment)
        (thing as Equipment).autoEquipTime = 0.5f;
      if (thing is ChokeCollar)
      {
        (thing as ChokeCollar).ball.hSpeed = thing.hSpeed;
        (thing as ChokeCollar).ball.vSpeed = thing.vSpeed;
      }
      if (!(thing is Sword))
        return;
      (thing as Sword)._wasLifted = true;
      (thing as Sword)._framesExisting = 16;
    }

    public override void Update()
    {
      if (Level.current.simulatePhysics)
        this._spawnWait += 0.0166666f;
      if (Level.current.simulatePhysics && Network.isServer && (this._numSpawned < this.spawnNum || this.spawnNum == -1) && ((this.contains != (System.Type) null || this.randomSpawn) && (double) this._spawnWait >= (double) this.spawnTime))
        this.Spawn();
      this.angleDegrees = -this.direction;
    }

    public void Pulse(int type, WireTileset wire)
    {
      this.Spawn();
      SFX.Play("click");
    }

    public override void Terminate()
    {
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("fireDirection", (object) this.fireDirection);
      binaryClassChunk.AddProperty("firePower", (object) this.firePower);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.fireDirection = node.GetProperty<float>("fireDirection");
      this.firePower = node.GetProperty<float>("firePower");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "fireDirection", (object) Change.ToString((object) this.fireDirection)));
      xelement.Add((object) new XElement((XName) "firePower", (object) Change.ToString((object) this.firePower)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement1 = node.Element((XName) "fireDirection");
      if (xelement1 != null)
        this.fireDirection = Convert.ToSingle(xelement1.Value);
      XElement xelement2 = node.Element((XName) "firePower");
      if (xelement2 != null)
        this.firePower = Convert.ToSingle(xelement2.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.AddItem((ContextMenu) new ContextSlider("Angle", (IContextListener) null, new FieldBinding((object) this, "fireDirection", max: 360f), 1f));
      contextMenu.AddItem((ContextMenu) new ContextSlider("Power", (IContextListener) null, new FieldBinding((object) this, "firePower", 1f, 20f)));
      return (ContextMenu) contextMenu;
    }

    public override void DrawHoverInfo()
    {
      string text = "EMPTY";
      if (this.contains != (System.Type) null)
        text = this.contains.Name;
      Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), -16f), Color.White, (Depth) 0.9f);
      if (!(this.contains != (System.Type) null))
        return;
      if (this._hoverThing == null || this._hoverThing.GetType() != this.contains)
        this._hoverThing = Editor.CreateThing(this.contains) as PhysicsObject;
      if (this._hoverThing == null)
        return;
      Vec2 vec2 = Maths.AngleToVec(Maths.DegToRad(this.direction)) * this.firePower;
      this._hoverThing.position = this.position + vec2.normalized * 8f;
      this._hoverThing.hSpeed = vec2.x;
      this._hoverThing.vSpeed = vec2.y;
      Vec2 position = this._hoverThing.position;
      for (int index = 0; index < 100; ++index)
      {
        this._hoverThing.UpdatePhysics();
        Graphics.DrawLine(position, this._hoverThing.position, Color.Red, 2f, (Depth) 1f);
        position = this._hoverThing.position;
      }
    }

    public override void Draw()
    {
      this.angleDegrees = -this.direction;
      base.Draw();
    }
  }
}
