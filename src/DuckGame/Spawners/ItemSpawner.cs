// Decompiled with JetBrains decompiler
// Type: DuckGame.ItemSpawner
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("spawns")]
  [BaggedProperty("isInDemo", true)]
  public class ItemSpawner : Thing, IContainAThing
  {
    public StateBinding _positionBinding = new StateBinding(nameof (netPosition));
    protected bool _hasContainedItem = true;
    protected SpriteMap _sprite;
    protected float _spawnWait;
    public float initialDelay;
    public float spawnTime = 10f;
    public bool spawnOnStart = true;
    public bool randomSpawn;
    public bool keepRandom;
    public int spawnNum = -1;
    private Holdable hoverItem;
    private SinWave _hoverSin = (SinWave) 0.05f;
    private SpawnerBall _ball1;
    private SpawnerBall _ball2;
    protected int _numSpawned;
    protected bool _seated;
    private bool _triedSeating;
    private int _seatingTries;
    private Thing previewThing;
    private Sprite previewSprite;
    private float _bob;
    public List<TypeProbPair> possible = new List<TypeProbPair>();

    public override Vec2 netPosition
    {
      get => this.position;
      set
      {
        if (!(this.position != value))
          return;
        this.position = value;
        if (this._ball1 == null || this._ball2 == null)
          return;
        this._ball1.position = this.position + new Vec2(0.0f, -1f);
        this._ball2.position = this.position + new Vec2(0.0f, -1f);
      }
    }

    public override void SetTranslation(Vec2 translation)
    {
      if (this._ball1 != null)
        this._ball1.SetTranslation(translation);
      if (this._ball2 != null)
        this._ball2.SetTranslation(translation);
      base.SetTranslation(translation);
    }

    public System.Type contains { get; set; }

    public Holdable _hoverItem
    {
      get => this.hoverItem;
      set => this.SetHoverItem(value);
    }

    public ItemSpawner(float xpos, float ypos, System.Type c = null)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("gunSpawner", 14, 6);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(7f, 0.0f);
      this.collisionSize = new Vec2(14f, 2f);
      this.collisionOffset = new Vec2(-7f, 0.0f);
      this.depth = (Depth) 0.0f;
      this.contains = c;
      this.hugWalls = WallHug.Floor;
      this._isStateObject = true;
      this._isStateObjectInitialized = true;
    }

    public override void Initialize()
    {
      this._ball1 = new SpawnerBall(this.x, this.y - 1f, false);
      this._ball2 = new SpawnerBall(this.x, this.y - 1f, true);
      Level.Add((Thing) this._ball1);
      Level.Add((Thing) this._ball2);
      if (this.spawnOnStart)
        this._spawnWait = this.spawnTime;
      if (Level.current is Editor)
        return;
      if (this.randomSpawn && this.keepRandom)
      {
        List<System.Type> physicsObjects = ItemBox.GetPhysicsObjects(Editor.Placeables);
        this.contains = physicsObjects[Rando.Int(physicsObjects.Count - 1)];
        this.randomSpawn = false;
      }
      else
      {
        if (this.possible.Count <= 0)
          return;
        System.Type type = MysteryGun.PickType(this.chanceGroup, this.possible);
        if (!(type != (System.Type) null))
          return;
        this.contains = type;
      }
    }

    public void BreakHoverBond()
    {
      if (this._hoverItem == null)
        return;
      this._hoverItem.gravMultiplier = 1f;
      this._hoverItem.hoverSpawner = (ItemSpawner) null;
      this._hoverItem = (Holdable) null;
    }

    public virtual void SpawnItem()
    {
      this._spawnWait = 0.0f;
      IReadOnlyPropertyBag bag = ContentProperties.GetBag(this.contains);
      PhysicsObject physicsObject = !Network.isActive || bag.GetOrDefault<bool>("isOnlineCapable", true) ? Editor.CreateThing(this.contains) as PhysicsObject : Activator.CreateInstance(typeof (Pistol), Editor.GetConstructorParameters(typeof (Pistol))) as PhysicsObject;
      if (physicsObject == null)
        return;
      physicsObject.x = this.x;
      physicsObject.y = (float) ((double) this.top + ((double) physicsObject.y - (double) physicsObject.bottom) - 6.0);
      physicsObject.vSpeed = -2f;
      physicsObject.spawnAnimation = true;
      physicsObject.isSpawned = true;
      Level.Add((Thing) physicsObject);
      if (!this._seated)
        return;
      this.SetHoverItem(physicsObject as Holdable);
    }

    public virtual void SetHoverItem(Holdable hover)
    {
      if (this._hoverItem == hover)
        return;
      if (this._hoverItem != null)
      {
        this._hoverItem.hoverSpawner = (ItemSpawner) null;
        this._hoverItem.grounded = false;
      }
      this.hoverItem = hover;
      if (this._hoverItem == null)
        return;
      this._hoverItem.hoverSpawner = this;
      this._hoverItem.grounded = true;
    }

    public void TrySeating()
    {
      if (this._seatingTries >= 3)
        return;
      if (Level.CheckPoint<IPlatform>(this.position + new Vec2(0.0f, 6f)) != null)
      {
        this._seated = true;
        this._seatingTries = 3;
      }
      else
        this._seated = false;
      ++this._seatingTries;
    }

    public override void EditorUpdate()
    {
      if (this.contains != (System.Type) null && !this.randomSpawn && Level.current is Editor && (this.previewThing == null || this.previewThing.GetType() != this.contains))
      {
        this.previewThing = Editor.GetThing(this.contains);
        if (this.previewThing != null)
          this.previewSprite = this.previewThing.GeneratePreview(32, 32, true);
      }
      base.EditorUpdate();
    }

    public override void Update()
    {
      this.TrySeating();
      if (this._hoverItem == null)
      {
        if (this._seated)
        {
          Holdable hover = Level.Nearest<Holdable>(this.x, this.y);
          if (hover != null && hover.owner == null && (hover != null && hover.canPickUp) && ((double) Math.Abs(hover.hSpeed) + (double) Math.Abs(hover.vSpeed) < 2.0 && (!(hover is Gun) || (hover as Gun).ammo > 0)))
          {
            float num = 999f;
            if (hover != null)
              num = (this.position - hover.position).length;
            if ((double) num < 16.0)
              this.SetHoverItem(hover);
          }
        }
        this._ball1.desiredOrbitDistance = 3f;
        this._ball2.desiredOrbitDistance = 3f;
        this._ball1.desiredOrbitHeight = 1f;
        this._ball2.desiredOrbitHeight = 1f;
        if (Level.current.simulatePhysics)
          this._spawnWait += 0.0166666f;
      }
      else if ((double) Math.Abs(this._hoverItem.hSpeed) + (double) Math.Abs(this._hoverItem.vSpeed) > 2.0 || ((double) (this._hoverItem.collisionCenter - this.position).length > 18.0 || this._hoverItem.destroyed || (this._hoverItem.removeFromLevel || this._hoverItem.owner != null)))
      {
        this.BreakHoverBond();
      }
      else
      {
        this._hoverItem.position = Lerp.Vec2Smooth(this._hoverItem.position, this.position + new Vec2(0.0f, (float) (-((double) this._hoverItem.bottom - (double) this._hoverItem.y) - 2.0 + (double) (float) this._hoverSin * 2.0)), 0.2f);
        this._hoverItem.vSpeed = 0.0f;
        this._hoverItem.gravMultiplier = 0.0f;
        this._ball1.desiredOrbitDistance = this._hoverItem.collisionSize.x / 2f;
        this._ball2.desiredOrbitDistance = this._hoverItem.collisionSize.x / 2f;
        this._ball1.desiredOrbitHeight = 4f;
        this._ball2.desiredOrbitHeight = 4f;
      }
      if (!Network.isServer || this._numSpawned >= this.spawnNum && this.spawnNum != -1 || (this._hoverItem != null || !(this.contains != (System.Type) null) && !this.randomSpawn) || (double) this._spawnWait < (double) this.spawnTime)
        return;
      if ((double) this.initialDelay > 0.0)
      {
        this.initialDelay -= 0.0166666f;
      }
      else
      {
        if (this.randomSpawn)
        {
          List<System.Type> physicsObjects = ItemBox.GetPhysicsObjects(Editor.Placeables);
          this.contains = physicsObjects[Rando.Int(physicsObjects.Count - 1)];
        }
        ++this._numSpawned;
        this.SpawnItem();
      }
    }

    public override void Draw()
    {
      if (this.contains != (System.Type) null && !this.randomSpawn && (Level.current is Editor && this.previewThing != null))
      {
        this._bob += 0.05f;
        this.previewSprite.CenterOrigin();
        this.previewSprite.alpha = 0.5f;
        Graphics.Draw(this.previewSprite, this.x, (float) ((double) this.y - 8.0 + Math.Sin((double) this._bob) * 2.0));
      }
      base.Draw();
    }

    public override void Terminate()
    {
      Level.Remove((Thing) this._ball1);
      Level.Remove((Thing) this._ball2);
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      if (this._hasContainedItem)
      {
        binaryClassChunk.AddProperty("contains", this.contains != (System.Type) null ? (object) ModLoader.SmallTypeName(this.contains) : (object) "");
        binaryClassChunk.AddProperty("randomSpawn", (object) this.randomSpawn);
        binaryClassChunk.AddProperty("keepRandom", (object) this.keepRandom);
      }
      binaryClassChunk.AddProperty("possible", (object) MysteryGun.SerializeTypeProb(this.possible));
      binaryClassChunk.AddProperty("spawnTime", (object) this.spawnTime);
      binaryClassChunk.AddProperty("initialDelay", (object) this.initialDelay);
      binaryClassChunk.AddProperty("spawnOnStart", (object) this.spawnOnStart);
      binaryClassChunk.AddProperty("spawnNum", (object) this.spawnNum);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      if (this._hasContainedItem)
      {
        this.contains = Editor.GetType(node.GetProperty<string>("contains"));
        this.randomSpawn = node.GetProperty<bool>("randomSpawn");
        this.keepRandom = node.GetProperty<bool>("keepRandom");
      }
      this.possible = MysteryGun.DeserializeTypeProb(node.GetProperty<string>("possible"));
      this.spawnTime = node.GetProperty<float>("spawnTime");
      this.initialDelay = node.GetProperty<float>("initialDelay");
      this.spawnOnStart = node.GetProperty<bool>("spawnOnStart");
      this.spawnNum = node.GetProperty<int>("spawnNum");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      if (this._hasContainedItem)
        xelement.Add((object) new XElement((XName) "contains", this.contains != (System.Type) null ? (object) this.contains.AssemblyQualifiedName : (object) ""));
      xelement.Add((object) new XElement((XName) "spawnTime", (object) Change.ToString((object) this.spawnTime)));
      xelement.Add((object) new XElement((XName) "initialDelay", (object) Change.ToString((object) this.initialDelay)));
      xelement.Add((object) new XElement((XName) "spawnOnStart", (object) Change.ToString((object) this.spawnOnStart)));
      if (this._hasContainedItem)
        xelement.Add((object) new XElement((XName) "randomSpawn", (object) Change.ToString((object) this.randomSpawn)));
      if (this._hasContainedItem)
        xelement.Add((object) new XElement((XName) "keepRandom", (object) Change.ToString((object) this.keepRandom)));
      xelement.Add((object) new XElement((XName) "spawnNum", (object) Change.ToString((object) this.spawnNum)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      if (this._hasContainedItem)
      {
        XElement xelement = node.Element((XName) "contains");
        if (xelement != null)
          this.contains = Editor.GetType(xelement.Value);
      }
      XElement xelement1 = node.Element((XName) "spawnTime");
      if (xelement1 != null)
        this.spawnTime = Change.ToSingle((object) xelement1.Value);
      XElement xelement2 = node.Element((XName) "initialDelay");
      if (xelement2 != null)
        this.initialDelay = Change.ToSingle((object) xelement2.Value);
      XElement xelement3 = node.Element((XName) "spawnOnStart");
      if (xelement3 != null)
        this.spawnOnStart = Convert.ToBoolean(xelement3.Value);
      if (this._hasContainedItem)
      {
        XElement xelement4 = node.Element((XName) "randomSpawn");
        if (xelement4 != null)
          this.randomSpawn = Convert.ToBoolean(xelement4.Value);
        XElement xelement5 = node.Element((XName) "keepRandom");
        if (xelement5 != null)
          this.keepRandom = Convert.ToBoolean(xelement5.Value);
      }
      XElement xelement6 = node.Element((XName) "spawnNum");
      if (xelement6 != null)
        this.spawnNum = Convert.ToInt32(xelement6.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      FieldBinding radioBinding = new FieldBinding((object) this, "contains");
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.AddItem((ContextMenu) new ContextSlider("Delay", (IContextListener) null, new FieldBinding((object) this, "spawnTime", 1f, 100f)));
      contextMenu.AddItem((ContextMenu) new ContextSlider("Initial Delay", (IContextListener) null, new FieldBinding((object) this, "initialDelay", max: 100f)));
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Start Spawned", (IContextListener) null, new FieldBinding((object) this, "spawnOnStart")));
      if (this._hasContainedItem)
      {
        contextMenu.AddItem((ContextMenu) new ContextCheckBox("Random", (IContextListener) null, new FieldBinding((object) this, "randomSpawn")));
        contextMenu.AddItem((ContextMenu) new ContextCheckBox("Keep Random", (IContextListener) null, new FieldBinding((object) this, "keepRandom")));
      }
      contextMenu.AddItem((ContextMenu) new ContextSlider("Number", (IContextListener) null, new FieldBinding((object) this, "spawnNum", -1f, 100f), 1f, "INF"));
      if (this._hasContainedItem)
      {
        EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) contextMenu);
        editorGroupMenu.InitializeGroups(new EditorGroup(typeof (PhysicsObject)), radioBinding);
        editorGroupMenu.text = "Contains";
        contextMenu.AddItem((ContextMenu) editorGroupMenu);
      }
      EditorGroupMenu editorGroupMenu1 = new EditorGroupMenu((IContextListener) contextMenu);
      editorGroupMenu1.InitializeGroups(new EditorGroup(typeof (PhysicsObject)), new FieldBinding((object) this, "possible"));
      editorGroupMenu1.text = "Possible";
      contextMenu.AddItem((ContextMenu) editorGroupMenu1);
      return (ContextMenu) contextMenu;
    }

    public override void DrawHoverInfo()
    {
      if (this.possible.Count > 0)
      {
        float num = 0.0f;
        foreach (TypeProbPair typeProbPair in this.possible)
        {
          if ((double) typeProbPair.probability > 0.0)
          {
            Color white = Color.White;
            Color color = (double) typeProbPair.probability != 0.0 ? ((double) typeProbPair.probability >= 0.300000011920929 ? ((double) typeProbPair.probability >= 0.699999988079071 ? Color.Green : Color.Orange) : Colors.DGRed) : Color.DarkGray;
            string text = typeProbPair.type.Name + ": " + typeProbPair.probability.ToString("0.000");
            Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text, scale: 0.5f) / 2.0), (float) -(16.0 + (double) num)), color, (Depth) 0.9f, scale: 0.5f);
            num += 4f;
          }
        }
      }
      else
      {
        string text = "EMPTY";
        if (this.contains != (System.Type) null)
          text = this.contains.Name;
        Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text) / 2.0), -16f), Color.White, (Depth) 0.9f);
      }
    }

    public override string GetDetailsString()
    {
      string str = "EMPTY";
      if (this.contains != (System.Type) null)
        str = this.contains.Name;
      if (this.contains == (System.Type) null && (double) this.spawnTime == 10.0)
        return base.GetDetailsString();
      return base.GetDetailsString() + "Contains: " + str + "\nTime: " + this.spawnTime.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
