// Decompiled with JetBrains decompiler
// Type: DuckGame.ItemBox
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("spawns")]
  [BaggedProperty("isInDemo", true)]
  public class ItemBox : Block, IPathNodeBlocker, IContainAThing
  {
    public StateBinding _positionBinding = new StateBinding("position");
    public StateBinding _containedObjectBinding = new StateBinding(nameof (containedObject));
    public StateBinding _boxStateBinding = (StateBinding) new StateFlagBinding(new string[1]
    {
      nameof (_hit)
    });
    public StateBinding _chargingBinding = new StateBinding(nameof (charging), 9);
    public StateBinding _netDisarmIndexBinding = new StateBinding(nameof (netDisarmIndex));
    public StateBinding _netHitSoundBinding = (StateBinding) new NetSoundBinding(nameof (_netHitSound));
    public NetSoundEffect _netHitSound = new NetSoundEffect(new string[1]
    {
      "hitBox"
    })
    {
      volume = 1f
    };
    public byte netDisarmIndex;
    public byte localNetDisarm;
    public float bounceAmount;
    public bool _hit;
    public int charging;
    public float startY = -99999f;
    protected List<PhysicsObject> _aboveList = new List<PhysicsObject>();
    private PhysicsObject _containedObject;
    protected SpriteMap _sprite;
    public bool _canBounce = true;
    public PhysicsObject lastSpawnItem;

    public PhysicsObject containedObject
    {
      get => this._containedObject;
      set => this._containedObject = value;
    }

    public System.Type contains { get; set; }

    public bool canBounce => this._canBounce;

    public ItemBox(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("itemBox", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(0.5f);
      this._canFlip = false;
    }

    public void Pop()
    {
      this.Bounce();
      if (this._hit)
        return;
      this.SpawnItem();
    }

    public void Bounce()
    {
      if (!this._canBounce)
        return;
      this.bounceAmount = 8f;
      this._canBounce = false;
      if (Network.isActive)
      {
        ++this.netDisarmIndex;
      }
      else
      {
        this._aboveList = Level.CheckRectAll<PhysicsObject>(this.topLeft + new Vec2(1f, -4f), this.bottomRight + new Vec2(-1f, -12f)).ToList<PhysicsObject>();
        foreach (PhysicsObject above in this._aboveList)
        {
          if (above.grounded || (double) above.vSpeed > 0.0 || (double) above.vSpeed == 0.0)
          {
            this.Fondle((Thing) above);
            above.y -= 2f;
            above.vSpeed = -3f;
            if (above is Duck duck)
              duck.Disarm((Thing) this);
          }
        }
      }
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (from != ImpactedFrom.Bottom || !with.isServerForObject)
        return;
      with.Fondle((Thing) this);
      if (this.containedObject != null)
        with.Fondle((Thing) this.containedObject);
      this.Pop();
    }

    public virtual void UpdateCharging()
    {
      if (!this.isServerForObject)
        return;
      if (this.charging > 0)
      {
        --this.charging;
      }
      else
      {
        this.charging = 0;
        this._hit = false;
      }
    }

    public virtual void UpdateContainedObject()
    {
      if (!Network.isActive || !this.isServerForObject || this.containedObject != null)
        return;
      this.containedObject = this.GetSpawnItem();
      if (this.containedObject == null)
        return;
      this.containedObject.visible = false;
      this.containedObject.active = false;
      this.containedObject.position = this.position;
      Level.Add((Thing) this.containedObject);
    }

    public override void Update()
    {
      this.UpdateContainedObject();
      this._aboveList.Clear();
      if ((double) this.startY < -9999.0)
        this.startY = this.y;
      this._sprite.frame = this._hit ? 1 : 0;
      if ((int) this.netDisarmIndex != (int) this.localNetDisarm)
      {
        this.localNetDisarm = this.netDisarmIndex;
        this._aboveList = Level.CheckRectAll<PhysicsObject>(this.topLeft + new Vec2(1f, -4f), this.bottomRight + new Vec2(-1f, -12f)).ToList<PhysicsObject>();
        foreach (PhysicsObject above in this._aboveList)
        {
          if (this.isServerForObject && above.owner == null)
            this.Fondle((Thing) above);
          if (above.isServerForObject && (above.grounded || (double) above.vSpeed > 0.0 || (double) above.vSpeed == 0.0))
          {
            above.y -= 2f;
            above.vSpeed = -3f;
            if (above is Duck duck)
              duck.Disarm((Thing) this);
          }
        }
      }
      this.UpdateCharging();
      if ((double) this.bounceAmount > 0.0)
        this.bounceAmount -= 0.8f;
      else
        this.bounceAmount = 0.0f;
      this.y -= this.bounceAmount;
      if (this._canBounce)
        return;
      if ((double) this.y < (double) this.startY)
        this.y += (float) (0.800000011920929 + (double) Math.Abs(this.y - this.startY) * 0.400000005960464);
      if ((double) this.y > (double) this.startY)
        this.y -= (float) (0.800000011920929 - (double) Math.Abs(this.y - this.startY) * 0.400000005960464);
      if ((double) Math.Abs(this.y - this.startY) >= 0.800000011920929)
        return;
      this._canBounce = true;
      this.y = this.startY;
    }

    public virtual PhysicsObject GetSpawnItem()
    {
      if (this.contains == (System.Type) null)
        return (PhysicsObject) null;
      IReadOnlyPropertyBag bag = ContentProperties.GetBag(this.contains);
      return !Network.isActive || bag.GetOrDefault<bool>("isOnlineCapable", true) ? Editor.CreateThing(this.contains) as PhysicsObject : Activator.CreateInstance(typeof (Pistol), Editor.GetConstructorParameters(typeof (Pistol))) as PhysicsObject;
    }

    public virtual void SpawnItem()
    {
      this.charging = 500;
      if (!Network.isActive && this.contains == (System.Type) null && !(this is ItemBoxRandom) || Network.isActive && this.containedObject == null && (this is PurpleBlock && this.contains == (System.Type) null))
        return;
      PhysicsObject physicsObject;
      if (!Network.isActive || this is PurpleBlock)
      {
        physicsObject = this.GetSpawnItem();
      }
      else
      {
        if (this.containedObject == null)
          return;
        physicsObject = this.containedObject;
        physicsObject.active = true;
        physicsObject.visible = true;
      }
      this._hit = true;
      this.lastSpawnItem = physicsObject;
      if (physicsObject == null)
        return;
      foreach (PhysicsObject above in this._aboveList)
        physicsObject.clip.Add((MaterialThing) above);
      physicsObject.x = this.x;
      physicsObject.bottom = this.bottom;
      physicsObject.y -= 12f;
      physicsObject.vSpeed = -3.5f;
      physicsObject.clip.Add((MaterialThing) this);
      if (physicsObject is Gun)
      {
        Gun gun = physicsObject as Gun;
        if (gun.CanSpin())
          gun.angleDegrees = 180f;
      }
      Block block1 = Level.CheckPoint<Block>(this.position + new Vec2(-16f, 0.0f));
      if (block1 != null)
        physicsObject.clip.Add((MaterialThing) block1);
      Block block2 = Level.CheckPoint<Block>(this.position + new Vec2(16f, 0.0f));
      if (block2 != null)
        physicsObject.clip.Add((MaterialThing) block2);
      if (!Network.isActive || this is PurpleBlock)
        Level.Add((Thing) physicsObject);
      if (!Network.isActive)
        SFX.Play("hitBox");
      else if (this.isServerForObject)
        this._netHitSound.Play();
      Thing.Fondle((Thing) physicsObject, DuckNetwork.localConnection);
      this.containedObject = (PhysicsObject) null;
    }

    public static List<System.Type> GetPhysicsObjects(EditorGroup group) => Editor.ThingTypes.Where<System.Type>((Func<System.Type, bool>) (t =>
    {
      if (t.IsAbstract || !t.IsSubclassOf(typeof (PhysicsObject)) || t.GetCustomAttributes(typeof (EditorGroupAttribute), false).Length == 0)
        return false;
      IReadOnlyPropertyBag bag = ContentProperties.GetBag(t);
      return bag.GetOrDefault<bool>("canSpawn", true) && (!Network.isActive || !bag.GetOrDefault<bool>("noRandomSpawningOnline", false)) && ((!Network.isActive || bag.GetOrDefault<bool>("isOnlineCapable", true)) && (Main.isDemo || !bag.GetOrDefault<bool>("onlySpawnInDemo", false)));
    })).ToList<System.Type>();

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
      Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), -16f), Color.White, new Depth(0.9f));
    }
  }
}
