// Decompiled with JetBrains decompiler
// Type: DuckGame.Thing
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;

namespace DuckGame
{
  /// <summary>
  /// The base class for everything in Duck Game. Things can be added to the world
  /// with Level.Add and they will be drawn and updated automatically.
  /// </summary>
  public abstract class Thing : Transform
  {
    public GhostPriority syncPriority;
    public StateBinding _classTypeBinding = new StateBinding(GhostPriority.Normal, nameof (ghostType));
    public StateBinding _authorityBinding = new StateBinding(GhostPriority.High, nameof (authority));
    public ushort _ghostType = 1;
    public bool isLocal = true;
    private int _networkSeed;
    private bool _initializedNetworkSeed;
    protected NetIndex8 _authority = (NetIndex8) 1;
    protected NetworkConnection _connection;
    protected int _framesSinceTransfer = 999;
    private int _networkDrawIndex;
    protected bool _isStateObject;
    protected bool _isStateObjectInitialized;
    public Dictionary<NetworkConnection, uint> currentTick = new Dictionary<NetworkConnection, uint>();
    private static ushort _staticGlobalIndex = 0;
    private static ushort _staticPhysicsIndex = 0;
    private ushort _globalIndex = Thing.GetGlobalIndex();
    protected ushort _physicsIndex;
    private Vec2 _lerpPosition = Vec2.Zero;
    private Vec2 _lerpVector = Vec2.Zero;
    private float _lerpSpeed;
    private Portal _portal;
    protected SequenceItem _sequence;
    protected string _type = "";
    protected Level _level;
    protected float _lastTeleportDirection;
    private bool _removeFromLevel;
    protected bool _placed;
    protected bool _canBeGrouped;
    protected Thing _owner;
    public Thing _prevOwner;
    protected Thing _lastThrownBy;
    protected bool _opaque;
    protected bool _opacityFromGraphic;
    protected Sprite _graphic;
    private DamageMap _damageMap;
    public bool lowLighting;
    private bool _visible = true;
    private Material _material;
    protected bool _enablePhysics = true;
    protected Profile _responsibleProfile;
    private System.Type _killThingType;
    public float _hSpeed;
    public float _vSpeed;
    protected bool _active = true;
    public bool serverOnly;
    private bool _action;
    private Anchor _anchor;
    public sbyte _offDir = 1;
    protected Layer _layer;
    protected bool _initialized;
    protected string _editorName = "";
    public Layer placementLayerOverride;
    protected bool _canFlip = true;
    protected bool _canFlipVert;
    protected bool _canHaveChance = true;
    protected float _likelyhoodToExist = 1f;
    protected bool _editorCanModify = true;
    protected bool _processedByEditor;
    protected bool _visibleInGame = true;
    private Vec2 _editorOffset = new Vec2();
    private WallHug _hugWalls;
    protected bool _editorImageCenter;
    private bool _isAccessible = true;
    protected bool _flipHorizontal;
    protected bool _flipVertical;
    private int _chanceGroup = -1;
    private static Effect _alphaTestEffect;
    private bool _skipPositioning;
    protected bool _solid = true;
    protected Vec2 _collisionOffset = new Vec2();
    protected Vec2 _collisionSize = new Vec2();
    protected float _topQuick;
    protected float _bottomQuick;
    protected float _leftQuick;
    protected float _rightQuick;
    protected bool _isStatic;
    public static bool skipLayerAdding = false;
    private bool _networkInitialized;
    public int wasSuperFondled;
    private GhostObject _ghostObject;
    private Dictionary<GhostManager, GhostObject> _ghostObjects;
    private bool _ignoreGhosting;
    public static bool doLerp = false;
    public bool shouldLerp;
    public Vec2 prevEndDrawPos = Vec2.Zero;
    public Vec2 prevEndVelocity = Vec2.Zero;
    public List<ushort> inputStates = new List<ushort>();
    private bool _redoLayer;

    /// <summary>
    /// Gets the path to an asset that the mod that this Thing is a part of.
    /// </summary>
    /// <param name="asset">The asset name, relative to the mods' Content folder.</param>
    /// <returns>The path.</returns>
    public string GetPath(string asset) => ModLoader._modAssemblies[this.GetType().Assembly].configuration.contentDirectory + asset.Replace('\\', '/');

    /// <summary>Gets the path to an asset from a mod.</summary>
    /// <typeparam name="T">The mod type to fetch from</typeparam>
    /// <param name="asset">The asset name, relative to the mods' Content folder.</param>
    /// <returns>The path.</returns>
    public static string GetPath<T>(string asset) where T : Mod => Mod.GetPath<T>(asset);

    public virtual Vec2 netPosition
    {
      get => this.position;
      set => this.position = value;
    }

    public ushort ghostType
    {
      get => !this._removeFromLevel ? this._ghostType : (ushort) 0;
      set
      {
        if ((int) this._ghostType == (int) value)
          return;
        this._ghostType = value;
      }
    }

    public int networkSeed
    {
      get
      {
        if (Network.isServer && !this._initializedNetworkSeed && this.isStateObject)
        {
          this._networkSeed = Rando.Int(2147483646);
          this._initializedNetworkSeed = true;
        }
        return this._networkSeed;
      }
      set
      {
        this._networkSeed = value;
        this._initializedNetworkSeed = true;
      }
    }

    public virtual NetIndex8 authority
    {
      get => this._authority;
      set
      {
        int num = value == 100 ? 1 : 0;
        this._authority = value;
      }
    }

    public virtual NetworkConnection connection
    {
      get => this._connection;
      set
      {
        if (value != this._connection && this.ghostObject != null)
          this.ghostObject.KillNetworkData();
        this._connection = value;
      }
    }

    public void Fondle(Thing t)
    {
      if (t == null || !Network.isActive || (!this.isServerForObject || !t.CanBeControlled()) || (this.connection != DuckNetwork.localConnection || t.connection == this.connection))
        return;
      t.connection = this.connection;
      ++t.authority;
    }

    public static void Fondle(Thing t, NetworkConnection c)
    {
      if (t == null || !Network.isActive || (c != DuckNetwork.localConnection || t.connection == c))
        return;
      t.connection = c;
      ++t.authority;
    }

    public static void SuperFondle(Thing t, NetworkConnection c)
    {
      if (t == null || !Network.isActive || c != DuckNetwork.localConnection)
        return;
      t.connection = c;
      t.authority += 40;
    }

    public virtual bool CanBeControlled() => true;

    public void IgnoreNetworkSync()
    {
      this._isStateObject = false;
      this._isStateObjectInitialized = true;
    }

    public virtual bool TransferControl(NetworkConnection to, NetIndex8 auth)
    {
      if (to == this.connection)
        return true;
      if (auth < this.authority || this.connection != null && this.CanBeControlled() && (auth == this.authority && to.profile != null) && (this.connection.profile != null && (int) this.connection.profile.networkIndex < (int) to.profile.networkIndex))
        return false;
      if (NetIndex8.Difference(auth, this.authority) > 39)
        this.wasSuperFondled = 120;
      this._framesSinceTransfer = 0;
      this.connection = to;
      this.authority = auth;
      return true;
    }

    public virtual void SpecialNetworkUpdate()
    {
    }

    public int networkDrawIndex
    {
      get => this._networkDrawIndex;
      set => this._networkDrawIndex = value;
    }

    public bool isStateObject
    {
      get
      {
        if (!this._isStateObjectInitialized)
        {
          this._isStateObject = Editor.AllStateFields[this.GetType()].Length > 2;
          this._isStateObjectInitialized = true;
        }
        return this._isStateObject;
      }
    }

    public bool isServerForObject => !Network.isActive || this.connection == null || this.connection == DuckNetwork.localConnection;

    public virtual void SetTranslation(Vec2 translation)
    {
      Thing thing = this;
      thing.position = thing.position + translation;
    }

    public virtual Vec2 cameraPosition => this.position;

    public static ushort GetGlobalIndex()
    {
      Thing._staticGlobalIndex = (ushort) (((int) Thing._staticGlobalIndex + 1) % (int) ushort.MaxValue);
      if (Thing._staticGlobalIndex == (ushort) 0)
        ++Thing._staticGlobalIndex;
      return Thing._staticGlobalIndex;
    }

    public static ushort GetPhysicsIndex()
    {
      Thing._staticPhysicsIndex = (ushort) (((int) Thing._staticPhysicsIndex + 1) % (int) ushort.MaxValue);
      if (Thing._staticPhysicsIndex == (ushort) 0)
        ++Thing._staticPhysicsIndex;
      return Thing._staticPhysicsIndex;
    }

    public ushort globalIndex
    {
      get => this._globalIndex;
      set => this._globalIndex = value;
    }

    public ushort physicsIndex
    {
      get => this._globalIndex;
      set => this._globalIndex = value;
    }

    public Vec2 lerpPosition
    {
      get => this._lerpPosition;
      set => this._lerpPosition = value;
    }

    public Vec2 lerpVector
    {
      get => this._lerpVector;
      set => this._lerpVector = value;
    }

    public float lerpSpeed
    {
      get => this._lerpSpeed;
      set => this._lerpSpeed = value;
    }

    public Portal portal
    {
      get => this._portal;
      set => this._portal = value;
    }

    public SequenceItem sequence
    {
      get => this._sequence;
      set => this._sequence = value;
    }

    public string type => this._type;

    public Level level
    {
      get => this._level;
      set => this._level = value;
    }

    public float lastTeleportDirection
    {
      get => this._lastTeleportDirection;
      set => this._lastTeleportDirection = value;
    }

    public bool removeFromLevel => this._removeFromLevel;

    public virtual int frame
    {
      get => !(this.graphic is SpriteMap) ? 0 : (this.graphic as SpriteMap).frame;
      set
      {
        if (!(this.graphic is SpriteMap))
          return;
        (this.graphic as SpriteMap).frame = value;
      }
    }

    public bool placed
    {
      get => this._placed;
      set => this._placed = value;
    }

    public bool canBeGrouped => this._canBeGrouped;

    public virtual Thing realObject => this;

    public virtual Thing owner
    {
      get => this._owner;
      set
      {
        if (this._owner != value)
          this._prevOwner = this._owner;
        this._lastThrownBy = this._owner;
        this._owner = value;
      }
    }

    public Thing prevOwner => this._prevOwner;

    public Thing lastThrownBy => this._lastThrownBy;

    public bool opaque => false;

    public virtual Sprite graphic
    {
      get => this._graphic;
      set => this._graphic = value;
    }

    public DamageMap damageMap
    {
      get => this._damageMap;
      set => this._damageMap = value;
    }

    public virtual bool visible
    {
      get => this._visible;
      set => this._visible = value;
    }

    public Material material
    {
      get => this._material;
      set => this._material = value;
    }

    public virtual bool enablePhysics
    {
      get => this._enablePhysics;
      set => this._enablePhysics = value;
    }

    public Profile responsibleProfile
    {
      set => this._responsibleProfile = value;
      get
      {
        if (this._responsibleProfile != null)
          return this._responsibleProfile;
        if (!(this is Duck duck) && !(this.owner is Duck duck) && (!(this.prevOwner is Duck duck) && this.owner != null) && (!(this.owner.owner is Duck duck) && !(this.owner.prevOwner is Duck duck) && (this.prevOwner != null && !(this.prevOwner.owner is Duck duck))))
          duck = this.prevOwner.prevOwner as Duck;
        if (duck == null && this is Bullet)
        {
          Bullet bullet = this as Bullet;
          if (bullet.firedFrom != null && !(bullet.firedFrom is Bullet))
            return bullet.firedFrom.responsibleProfile;
        }
        return duck?.profile;
      }
    }

    public System.Type killThingType
    {
      get
      {
        if (this._killThingType != (System.Type) null)
          return this._killThingType;
        if (this is Bullet)
        {
          Bullet bullet = this as Bullet;
          if (bullet.firedFrom != null)
            return bullet.firedFrom.GetType();
        }
        if (this is SmallFire)
        {
          SmallFire smallFire = this as SmallFire;
          if (smallFire.firedFrom != null)
            return smallFire.firedFrom.GetType();
        }
        return this.GetType();
      }
      set => this._killThingType = value;
    }

    public virtual float hSpeed
    {
      get => this._hSpeed;
      set => this._hSpeed = value;
    }

    public virtual float vSpeed
    {
      get => this._vSpeed;
      set => this._vSpeed = value;
    }

    public Vec2 velocity
    {
      get => new Vec2(this.hSpeed, this.vSpeed);
      set
      {
        this._hSpeed = value.x;
        this._vSpeed = value.y;
      }
    }

    public void ApplyForce(Vec2 force)
    {
      this._hSpeed += force.x;
      this._vSpeed += force.y;
    }

    public void ApplyForce(Vec2 force, Vec2 limits)
    {
      limits = new Vec2(Math.Abs(limits.x), Math.Abs(limits.y));
      if ((double) force.x < 0.0 && (double) this._hSpeed > -(double) limits.x || (double) force.x > 0.0 && (double) this._hSpeed < (double) limits.x)
        this._hSpeed += force.x;
      if (((double) force.y >= 0.0 || (double) this._vSpeed <= -(double) limits.y) && ((double) force.y <= 0.0 || (double) this._vSpeed >= (double) limits.y))
        return;
      this._vSpeed += force.y;
    }

    public void ApplyForceLimited(Vec2 force)
    {
      this._hSpeed += force.x;
      if ((double) force.x < 0.0 && (double) this._hSpeed < (double) force.x || (double) force.x > 0.0 && (double) this._hSpeed > (double) force.x)
        this._hSpeed = force.x;
      this._vSpeed += force.y;
      if (((double) force.y >= 0.0 || (double) this._vSpeed >= (double) force.y) && ((double) force.y <= 0.0 || (double) this._vSpeed <= (double) force.y))
        return;
      this._vSpeed = force.y;
    }

    public virtual bool active
    {
      get => this._active;
      set => this._active = value;
    }

    public virtual bool ShouldUpdate() => true;

    public virtual bool action
    {
      get => this._action;
      set => this._action = value;
    }

    public Anchor anchor
    {
      get => this._anchor;
      set => this._anchor = value;
    }

    public virtual Vec2 anchorPosition => this.position;

    public virtual sbyte offDir
    {
      get => this._offDir;
      set => this._offDir = value;
    }

    public Layer layer
    {
      get => this._layer;
      set
      {
        if (this._layer == value)
          return;
        if (this._level != null)
        {
          if (this._layer != null)
            this._layer.Remove(this);
          value.Add(this);
        }
        this._layer = value;
      }
    }

    public bool isInitialized => this._initialized;

    public List<System.Type> GetAllTypes() => Editor.AllBaseTypes[this.GetType()];

    public List<System.Type> GetAllTypesFiltered(System.Type stopAt) => Thing.GetAllTypes(this.GetType(), stopAt);

    public static List<System.Type> GetAllTypes(System.Type t, System.Type stopAt = null)
    {
      List<System.Type> typeList = new List<System.Type>((IEnumerable<System.Type>) t.GetInterfaces());
      typeList.Add(t);
      for (System.Type baseType = t.BaseType; baseType != (System.Type) null && (!(baseType == typeof (Thing)) && !(baseType == typeof (object))) && !(baseType == stopAt); baseType = baseType.BaseType)
        typeList.Add(baseType);
      return typeList;
    }

    public string editorName
    {
      get
      {
        if (this._editorName == "")
          this._editorName = this.GetType().Name;
        return this._editorName;
      }
    }

    public void SetEditorName(string s) => this._editorName = s;

    public Layer placementLayer => this.placementLayerOverride != null ? this.placementLayerOverride : this.layer;

    public float likelyhoodToExist
    {
      get => this._likelyhoodToExist;
      set => this._likelyhoodToExist = value;
    }

    public bool editorCanModify => this._editorCanModify;

    public bool processedByEditor
    {
      get => this._processedByEditor;
      set => this._processedByEditor = value;
    }

    public bool visibleInGame => this._visibleInGame;

    public Vec2 editorOffset
    {
      get => this._editorOffset;
      set => this._editorOffset = value;
    }

    public WallHug hugWalls
    {
      get => this._hugWalls;
      set => this._hugWalls = value;
    }

    public static Thing Instantiate(System.Type t) => Editor.CreateThing(t);

    public static Thing LoadThing(BinaryClassChunk node, bool chance = true)
    {
      System.Type type = Editor.GetType(node.GetProperty<string>("type"));
      if (!(type != (System.Type) null))
        return (Thing) null;
      Thing thing = Editor.CreateThing(type);
      if (!thing.Deserialize(node))
        thing = (Thing) null;
      return Level.current is Editor || !chance || ((double) thing.likelyhoodToExist == 1.0 || Level.PassedChanceGroup(thing.chanceGroup, thing.likelyhoodToExist)) ? thing : (Thing) null;
    }

    public virtual BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = new BinaryClassChunk();
      System.Type type = this.GetType();
      binaryClassChunk.AddProperty("type", (object) ModLoader.SmallTypeName(type));
      binaryClassChunk.AddProperty("x", (object) this.x);
      binaryClassChunk.AddProperty("y", (object) this.y);
      binaryClassChunk.AddProperty("chance", (object) this._likelyhoodToExist);
      binaryClassChunk.AddProperty("accessible", (object) this._isAccessible);
      binaryClassChunk.AddProperty("chanceGroup", (object) this._chanceGroup);
      binaryClassChunk.AddProperty("flipHorizontal", (object) this._flipHorizontal);
      if (this._canFlipVert)
        binaryClassChunk.AddProperty("flipVertical", (object) this.flipVertical);
      if (this.sequence != null)
      {
        binaryClassChunk.AddProperty("loop", (object) this.sequence.loop);
        binaryClassChunk.AddProperty("popUpOrder", (object) this.sequence.order);
        binaryClassChunk.AddProperty("waitTillOrder", (object) this.sequence.waitTillOrder);
      }
      foreach (System.Type key in Editor.AllBaseTypes[type])
      {
        if (!key.IsInterface)
        {
          foreach (FieldInfo fieldInfo in Editor.AllEditorFields[key])
          {
            object obj1 = fieldInfo.GetValue((object) this);
            obj1.GetType().GetProperty("info").GetValue(obj1, (object[]) null);
            object obj2 = obj1.GetType().GetProperty("value").GetValue(obj1, (object[]) null);
            binaryClassChunk.AddProperty(fieldInfo.Name, obj2);
          }
        }
      }
      return binaryClassChunk;
    }

    public virtual bool Deserialize(BinaryClassChunk node)
    {
      this.x = node.GetProperty<float>("x");
      this.y = node.GetProperty<float>("y");
      this._likelyhoodToExist = node.GetProperty<float>("chance");
      this._isAccessible = node.GetProperty<bool>("accessible");
      this.chanceGroup = node.GetProperty<int>("chanceGroup");
      this.flipHorizontal = node.GetProperty<bool>("flipHorizontal");
      if (this._canFlipVert)
        this.flipVertical = node.GetProperty<bool>("flipVertical");
      if (this.sequence != null)
      {
        this.sequence.loop = node.GetProperty<bool>("loop");
        this.sequence.order = node.GetProperty<int>("popUpOrder");
        this.sequence.waitTillOrder = node.GetProperty<bool>("waitTillOrder");
      }
      foreach (System.Type key in Editor.AllBaseTypes[this.GetType()])
      {
        if (!key.IsInterface)
        {
          foreach (FieldInfo fieldInfo in Editor.AllEditorFields[key])
          {
            System.Type genericArgument = fieldInfo.FieldType.GetGenericArguments()[0];
            object obj = fieldInfo.GetValue((object) this);
            obj.GetType().GetProperty("value").SetValue(obj, node.GetProperty(fieldInfo.Name), (object[]) null);
          }
        }
      }
      return true;
    }

    public static Thing LegacyLoadThing(XElement node, bool chance = true)
    {
      System.Type type = Editor.GetType(node.Element((XName) "type").Value);
      if (!(type != (System.Type) null))
        return (Thing) null;
      Thing thing = Editor.CreateThing(type);
      if (!thing.LegacyDeserialize(node))
        thing = (Thing) null;
      return Level.current is Editor || !chance || ((double) thing.likelyhoodToExist == 1.0 || Level.PassedChanceGroup(thing.chanceGroup, thing.likelyhoodToExist)) ? thing : (Thing) null;
    }

    public bool isAccessible
    {
      get => this._isAccessible;
      set => this._isAccessible = value;
    }

    public virtual XElement LegacySerialize()
    {
      XElement xelement = new XElement((XName) "Object");
      System.Type type = this.GetType();
      xelement.Add((object) new XElement((XName) "type", (object) type.AssemblyQualifiedName));
      xelement.Add((object) new XElement((XName) "x", (object) this.x));
      xelement.Add((object) new XElement((XName) "y", (object) this.y));
      xelement.Add((object) new XElement((XName) "chance", (object) this._likelyhoodToExist));
      xelement.Add((object) new XElement((XName) "accessible", (object) this._isAccessible));
      xelement.Add((object) new XElement((XName) "chanceGroup", (object) this._chanceGroup));
      xelement.Add((object) new XElement((XName) "flipHorizontal", (object) this._flipHorizontal));
      if (this._canFlipVert)
        xelement.Add((object) new XElement((XName) "flipVertical", (object) this._flipVertical));
      if (this.sequence != null)
      {
        xelement.Add((object) new XElement((XName) "loop", (object) this.sequence.loop));
        xelement.Add((object) new XElement((XName) "popUpOrder", (object) this.sequence.order));
        xelement.Add((object) new XElement((XName) "waitTillOrder", (object) this.sequence.waitTillOrder));
      }
      foreach (System.Type key in Editor.AllBaseTypes[type])
      {
        if (!key.IsInterface)
        {
          foreach (FieldInfo fieldInfo in Editor.AllEditorFields[key])
          {
            object obj = fieldInfo.GetValue((object) this);
            object content = obj.GetType().GetProperty("value").GetValue(obj, (object[]) null);
            xelement.Add((object) new XElement((XName) fieldInfo.Name, content));
          }
        }
      }
      return xelement;
    }

    public virtual bool LegacyDeserialize(XElement node)
    {
      XElement xelement1 = node.Element((XName) "x");
      if (xelement1 != null)
        this.x = Change.ToSingle((object) xelement1.Value);
      XElement xelement2 = node.Element((XName) "y");
      if (xelement2 != null)
        this.y = Change.ToSingle((object) xelement2.Value);
      XElement xelement3 = node.Element((XName) "chance");
      if (xelement3 != null)
        this._likelyhoodToExist = Change.ToSingle((object) xelement3.Value);
      XElement xelement4 = node.Element((XName) "accessible");
      if (xelement4 != null)
        this._isAccessible = Change.ToBoolean((object) xelement4.Value);
      XElement xelement5 = node.Element((XName) "chanceGroup");
      if (xelement5 != null)
        this.chanceGroup = Convert.ToInt32(xelement5.Value);
      XElement xelement6 = node.Element((XName) "flipHorizontal");
      if (xelement6 != null)
        this.flipHorizontal = Convert.ToBoolean(xelement6.Value);
      if (this._canFlipVert)
      {
        XElement xelement7 = node.Element((XName) "flipVertical");
        if (xelement7 != null)
          this.flipVertical = Convert.ToBoolean(xelement7.Value);
      }
      if (this.sequence != null)
      {
        XElement xelement7 = node.Element((XName) "loop");
        if (xelement7 != null)
          this.sequence.loop = Convert.ToBoolean(xelement7.Value);
        XElement xelement8 = node.Element((XName) "popUpOrder");
        if (xelement8 != null)
          this.sequence.order = Convert.ToInt32(xelement8.Value);
        XElement xelement9 = node.Element((XName) "waitTillOrder");
        if (xelement9 != null)
          this.sequence.waitTillOrder = Convert.ToBoolean(xelement9.Value);
      }
      foreach (System.Type key in Editor.AllBaseTypes[this.GetType()])
      {
        if (!key.IsInterface)
        {
          foreach (FieldInfo fieldInfo in Editor.AllEditorFields[key])
          {
            XElement xelement7 = node.Element((XName) fieldInfo.Name);
            if (xelement7 != null)
            {
              System.Type genericArgument = fieldInfo.FieldType.GetGenericArguments()[0];
              object obj = fieldInfo.GetValue((object) this);
              PropertyInfo property = obj.GetType().GetProperty("value");
              if (genericArgument == typeof (int))
                property.SetValue(obj, (object) Convert.ToInt32(xelement7.Value), (object[]) null);
              else if (genericArgument == typeof (float))
                property.SetValue(obj, (object) Convert.ToSingle(xelement7.Value), (object[]) null);
              else if (genericArgument == typeof (string))
              {
                EditorPropertyInfo editorPropertyInfo = obj.GetType().GetProperty("info").GetValue(obj, (object[]) null) as EditorPropertyInfo;
                object guid = (object) xelement7.Value;
                if (editorPropertyInfo.isLevel)
                {
                  LevelData levelData = DuckFile.LoadLevel(Content.path + "levels/" + guid + ".lev");
                  if (levelData != null)
                    guid = (object) levelData.metaData.guid;
                }
                property.SetValue(obj, guid, (object[]) null);
              }
              else if (genericArgument == typeof (bool))
                property.SetValue(obj, (object) Convert.ToBoolean(xelement7.Value), (object[]) null);
              else if (genericArgument == typeof (byte))
                property.SetValue(obj, (object) Convert.ToByte(xelement7.Value), (object[]) null);
              else if (genericArgument == typeof (short))
                property.SetValue(obj, (object) Convert.ToInt16(xelement7.Value), (object[]) null);
              else if (genericArgument == typeof (long))
                property.SetValue(obj, (object) Convert.ToInt64(xelement7.Value), (object[]) null);
            }
          }
        }
      }
      return true;
    }

    public virtual void EditorPropertyChanged(object property)
    {
    }

    public virtual void EditorObjectsChanged()
    {
    }

    public virtual bool flipHorizontal
    {
      get => this._flipHorizontal;
      set
      {
        this._flipHorizontal = value;
        this.offDir = this._flipHorizontal ? (sbyte) -1 : (sbyte) 1;
      }
    }

    public virtual bool flipVertical
    {
      get => this._flipVertical;
      set => this._flipVertical = value;
    }

    public int chanceGroup
    {
      get => this._chanceGroup;
      set => this._chanceGroup = value;
    }

    public virtual ContextMenu GetContextMenu()
    {
      EditorGroupMenu editorGroupMenu1 = new EditorGroupMenu((IContextListener) null, true);
      if (this._canFlip)
        editorGroupMenu1.AddItem((ContextMenu) new ContextCheckBox("Flip", (IContextListener) null, new FieldBinding((object) this, "flipHorizontal")));
      if (this._canFlipVert)
        editorGroupMenu1.AddItem((ContextMenu) new ContextCheckBox("Flip V", (IContextListener) null, new FieldBinding((object) this, "flipVertical")));
      if (this._canHaveChance)
      {
        EditorGroupMenu editorGroupMenu2 = new EditorGroupMenu((IContextListener) editorGroupMenu1);
        editorGroupMenu2.text = "Chance";
        editorGroupMenu2.tooltip = "Likelyhood for this object to exist in the level.";
        editorGroupMenu1.AddItem((ContextMenu) editorGroupMenu2);
        editorGroupMenu2.AddItem((ContextMenu) new ContextSlider("Chance", (IContextListener) null, new FieldBinding((object) this, "likelyhoodToExist"), 0.05f, (string) null, false, (System.Type) null, "Chance for object to exist. 1.0 = 100% chance."));
        editorGroupMenu2.AddItem((ContextMenu) new ContextSlider("Chance Group", (IContextListener) null, new FieldBinding((object) this, "chanceGroup", -1f, 10f), 1f, (string) null, false, (System.Type) null, "All objects in a chance group will exist, if their group's chance roll is met. -1 means no grouping."));
        editorGroupMenu2.AddItem((ContextMenu) new ContextCheckBox("Accessible", (IContextListener) null, new FieldBinding((object) this, "isAccessible"), (System.Type) null, "Flag for level generation, set this to false if the object is behind a locked door and not neccesarily accessible."));
      }
      if (this.sequence != null)
      {
        EditorGroupMenu editorGroupMenu2 = new EditorGroupMenu((IContextListener) editorGroupMenu1);
        editorGroupMenu2.text = "Sequence";
        editorGroupMenu1.AddItem((ContextMenu) editorGroupMenu2);
        editorGroupMenu2.AddItem((ContextMenu) new ContextCheckBox("Loop", (IContextListener) null, new FieldBinding((object) this.sequence, "loop")));
        editorGroupMenu2.AddItem((ContextMenu) new ContextSlider("Order", (IContextListener) null, new FieldBinding((object) this.sequence, "order", max: 100f), 1f, "RAND"));
        editorGroupMenu2.AddItem((ContextMenu) new ContextCheckBox("Wait", (IContextListener) null, new FieldBinding((object) this.sequence, "waitTillOrder")));
      }
      List<string> stringList = new List<string>();
      foreach (System.Type key in Editor.AllBaseTypes[this.GetType()])
      {
        if (!key.IsInterface)
        {
          foreach (FieldInfo fieldInfo in Editor.AllEditorFields[key])
          {
            if (!stringList.Contains(fieldInfo.Name))
            {
              object thing = fieldInfo.GetValue((object) this);
              EditorPropertyInfo editorPropertyInfo = thing.GetType().GetProperty("info").GetValue(thing, (object[]) null) as EditorPropertyInfo;
              if (editorPropertyInfo.value.GetType() == typeof (int) || editorPropertyInfo.value.GetType() == typeof (float))
                editorGroupMenu1.AddItem((ContextMenu) new ContextSlider(fieldInfo.Name, (IContextListener) null, new FieldBinding(thing, "value", editorPropertyInfo.min, editorPropertyInfo.max), editorPropertyInfo.increment, editorPropertyInfo.minSpecial, editorPropertyInfo.isTime, (System.Type) null, editorPropertyInfo.tooltip));
              else if (editorPropertyInfo.value.GetType() == typeof (bool))
                editorGroupMenu1.AddItem((ContextMenu) new ContextCheckBox(fieldInfo.Name, (IContextListener) null, new FieldBinding(thing, "value"), (System.Type) null, editorPropertyInfo.tooltip));
              else if (editorPropertyInfo.value.GetType() == typeof (string))
              {
                if (editorPropertyInfo.isLevel)
                  editorGroupMenu1.AddItem((ContextMenu) new ContextFile(fieldInfo.Name, (IContextListener) null, new FieldBinding(thing, "value"), ContextFileType.Level, editorPropertyInfo.tooltip));
                else
                  editorGroupMenu1.AddItem((ContextMenu) new ContextTextbox(fieldInfo.Name, (IContextListener) null, new FieldBinding(thing, "value"), editorPropertyInfo.tooltip));
              }
              stringList.Add(fieldInfo.Name);
            }
          }
        }
      }
      return (ContextMenu) editorGroupMenu1;
    }

    public virtual void DrawHoverInfo()
    {
    }

    public Sprite GetEditorImage(
      int wide = 16,
      int high = 16,
      bool transparentBack = false,
      Effect effect = null,
      RenderTarget2D target = null)
    {
      if (Thread.CurrentThread != MonoMain.mainThread)
        return new Sprite("basketBall");
      if (Thing._alphaTestEffect == null)
        Thing._alphaTestEffect = (Effect) Content.Load<MTEffect>("Shaders/alphatest");
      if (this.graphic != null)
      {
        if (wide <= 0)
          wide = this.graphic.w;
        if (high <= 0)
          high = this.graphic.h;
      }
      int num1 = wide > high ? wide : high;
      if (target == null)
        target = new RenderTarget2D(wide, high, true);
      if (this.graphic == null)
        return new Sprite(target, 0.0f, 0.0f);
      float num2 = (float) num1 / (float) this.graphic.width;
      Camera camera = new Camera(0.0f, 0.0f, (float) wide, (float) high);
      camera.position = new Vec2(this.x - this.centerx * num2, this.y - this.centery * num2);
      DuckGame.Graphics.SetRenderTarget(target);
      DepthStencilState depthStencilState = new DepthStencilState()
      {
        StencilEnable = true,
        StencilFunction = CompareFunction.Always,
        StencilPass = StencilOperation.Replace,
        ReferenceStencil = 1,
        DepthBufferEnable = false
      };
      DuckGame.Graphics.Clear(transparentBack ? new Color(0, 0, 0, 0) : new Color(15, 4, 16));
      DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, depthStencilState, RasterizerState.CullNone, (MTEffect) (effect == null ? Thing._alphaTestEffect : effect), camera.getMatrix());
      this.Draw();
      DuckGame.Graphics.screen.End();
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      return new Sprite(target, 0.0f, 0.0f);
    }

    public Sprite GeneratePreview(
      int wide = 16,
      int high = 16,
      bool transparentBack = false,
      Effect effect = null,
      RenderTarget2D target = null)
    {
      if (Thread.CurrentThread != MonoMain.mainThread)
        return new Sprite("basketBall");
      if (Thing._alphaTestEffect == null)
        Thing._alphaTestEffect = (Effect) Content.Load<MTEffect>("Shaders/alphatest");
      if (this.graphic != null)
      {
        if (wide <= 0)
          wide = this.graphic.w;
        if (high <= 0)
          high = this.graphic.h;
      }
      if (target == null)
        target = new RenderTarget2D(wide, high, true);
      if (this.graphic == null)
        return new Sprite(target, 0.0f, 0.0f);
      Camera camera = new Camera(0.0f, 0.0f, (float) wide, (float) high);
      camera.position = new Vec2(this.x - (float) (wide / 2), this.y - (float) (high / 2));
      DuckGame.Graphics.SetRenderTarget(target);
      DepthStencilState depthStencilState = new DepthStencilState()
      {
        StencilEnable = true,
        StencilFunction = CompareFunction.Always,
        StencilPass = StencilOperation.Replace,
        ReferenceStencil = 1,
        DepthBufferEnable = false
      };
      DuckGame.Graphics.Clear(transparentBack ? new Color(0, 0, 0, 0) : new Color(15, 4, 16));
      DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, depthStencilState, RasterizerState.CullNone, (MTEffect) (effect == null ? Thing._alphaTestEffect : effect), camera.getMatrix());
      this.Draw();
      DuckGame.Graphics.screen.End();
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      return new Sprite(target, 0.0f, 0.0f);
    }

    public virtual bool solid
    {
      get => this._solid;
      set
      {
        if (value && !this._solid)
          this.FixClipping();
        this._solid = value;
      }
    }

    public void FixClipping()
    {
      foreach (Block block in Level.CheckRectAll<Block>(this.topLeft, this.bottomRight))
        ;
    }

    private string GetPropertyDetails()
    {
      string str = "";
      foreach (FieldInfo fieldInfo in Editor.AllEditorFields[this.GetType()])
      {
        object obj = fieldInfo.GetValue((object) this);
        EditorPropertyInfo editorPropertyInfo = obj.GetType().GetProperty("info").GetValue(obj, (object[]) null) as EditorPropertyInfo;
        if (editorPropertyInfo.value.GetType() == typeof (int) || editorPropertyInfo.value.GetType() == typeof (float))
          str = str + fieldInfo.Name + ": " + Convert.ToString(editorPropertyInfo.value) + "\n";
      }
      return str;
    }

    public virtual string GetDetailsString()
    {
      if ((double) this._likelyhoodToExist == 1.0 && this._chanceGroup == -1)
        return this.GetPropertyDetails();
      return "Chance: " + (object) Math.Round((double) this.likelyhoodToExist / 1.0 * 100.0) + "%\nChance Group: " + (this._chanceGroup == -1 ? (object) "None" : (object) this._chanceGroup.ToString((IFormatProvider) CultureInfo.InvariantCulture)) + "\n" + this.GetPropertyDetails();
    }

    public virtual void ReturnItemToWorld(Thing t)
    {
      Block block1 = Level.CheckLine<Block>(this.position, this.position + new Vec2(16f, 0.0f));
      if (block1 != null && block1.solid && (double) t.right > (double) block1.left)
        t.right = block1.left;
      Block block2 = Level.CheckLine<Block>(this.position, this.position - new Vec2(16f, 0.0f));
      if (block2 != null && block2.solid && (double) t.left < (double) block2.right)
        t.left = block2.right;
      Block block3 = Level.CheckLine<Block>(this.position, this.position + new Vec2(0.0f, -16f));
      if (block3 != null && block3.solid && (double) t.top < (double) block3.bottom)
        t.top = block3.bottom;
      Block block4 = Level.CheckLine<Block>(this.position, this.position + new Vec2(0.0f, 16f));
      if (block4 == null || !block4.solid || (double) t.bottom <= (double) block4.top)
        return;
      t.bottom = block4.top;
    }

    public virtual Vec2 collisionOffset
    {
      get => this._collisionOffset;
      set => this._collisionOffset = value;
    }

    public virtual Vec2 collisionSize
    {
      get => this._collisionSize;
      set => this._collisionSize = value;
    }

    public float topQuick => this._topQuick;

    public float bottomQuick => this._bottomQuick;

    public float leftQuick => this._leftQuick;

    public float rightQuick => this._rightQuick;

    public virtual float top
    {
      get => this.y + this.collisionOffset.y;
      set => this.y = value + (this.y - this.top);
    }

    public virtual float bottom
    {
      get => this.y + this.collisionOffset.y + this.collisionSize.y;
      set => this.y = value + (this.y - this.bottom);
    }

    public virtual float topLocal => this.collisionOffset.y;

    public virtual float bottomLocal => this.collisionOffset.y + this.collisionSize.y;

    public virtual float left
    {
      get => this.offDir <= (sbyte) 0 ? this.x - this.collisionSize.x - this.collisionOffset.x : this.x + this.collisionOffset.x;
      set => this.x = value + (this.x - this.left);
    }

    public virtual float right
    {
      get => this.offDir <= (sbyte) 0 ? this.x - this.collisionOffset.x : this.x + this.collisionOffset.x + this.collisionSize.x;
      set => this.x = value + (this.x - this.right);
    }

    public Vec2 topLeft => new Vec2(this.left, this.top);

    public Vec2 topRight => new Vec2(this.right, this.top);

    public Vec2 bottomLeft => new Vec2(this.left, this.bottom);

    public Vec2 bottomRight => new Vec2(this.right, this.bottom);

    public Vec2 NearestCorner(Vec2 to)
    {
      Vec2 vec2 = this.topLeft;
      float num = (this.topLeft - to).length;
      float length1 = (this.topRight - to).length;
      if ((double) length1 < (double) num)
      {
        vec2 = this.topRight;
        num = length1;
      }
      float length2 = (this.bottomLeft - to).length;
      if ((double) length2 < (double) num)
      {
        vec2 = this.bottomLeft;
        num = length2;
      }
      if ((double) (this.bottomRight - to).length < (double) num)
        vec2 = this.bottomRight;
      return vec2;
    }

    public Vec2 NearestOpenCorner(Vec2 to)
    {
      Vec2 vec2 = Vec2.Zero;
      float num = 9999999f;
      float length1 = (this.topLeft - to).length;
      if ((double) length1 < (double) num && Level.CheckCircle<Block>(this.topLeft, 2f, this) == null)
      {
        vec2 = this.topLeft;
        num = length1;
      }
      float length2 = (this.topRight - to).length;
      if ((double) length2 < (double) num && Level.CheckCircle<Block>(this.topRight, 2f, this) == null)
      {
        vec2 = this.topRight;
        num = length2;
      }
      float length3 = (this.bottomLeft - to).length;
      if ((double) length3 < (double) num && Level.CheckCircle<Block>(this.bottomLeft, 2f, this) == null)
      {
        vec2 = this.bottomLeft;
        num = length3;
      }
      if ((double) (this.bottomRight - to).length < (double) num && Level.CheckCircle<Block>(this.bottomRight, 2f, this) == null)
        vec2 = this.bottomRight;
      return vec2;
    }

    public bool isStatic
    {
      get => this._isStatic;
      set => this._isStatic = value;
    }

    public float halfWidth => this.width / 2f;

    public float halfHeight => this.height / 2f;

    public virtual float width => this._collisionSize.x * this.scale.x;

    public virtual float height => this._collisionSize.y * this.scale.y;

    public float w => this.width;

    public float h => this.height;

    public Rectangle rectangle => new Rectangle((float) (int) this.left, (float) (int) this.top, (float) (int) ((double) this.right - (double) this.left), (float) (int) ((double) this.bottom - (double) this.top));

    public Vec2 collisionCenter => new Vec2(this.left + this.collisionSize.x / 2f, this.top + this.collisionSize.y / 2f);

    public Thing(float xval = 0.0f, float yval = 0.0f, Sprite sprite = null)
    {
      this.x = xval;
      this.y = yval;
      this.graphic = sprite;
      if (sprite != null)
        this._collisionSize = new Vec2((float) sprite.w, (float) sprite.h);
      if (!Network.isActive)
        return;
      this.connection = DuckNetwork.localConnection;
    }

    public virtual Vec2 OffsetLocal(Vec2 pos)
    {
      Vec2 vec2 = pos * this.scale;
      if (this.offDir < (sbyte) 0)
        vec2.x *= -1f;
      return vec2.Rotate(this.angle, new Vec2(0.0f, 0.0f));
    }

    public virtual Vec2 ReverseOffsetLocal(Vec2 pos)
    {
      Vec2 vec2 = pos * this.scale;
      vec2 = vec2.Rotate(-this.angle, new Vec2(0.0f, 0.0f));
      return vec2;
    }

    public virtual Vec2 Offset(Vec2 pos) => this.position + this.OffsetLocal(pos);

    public virtual Vec2 ReverseOffset(Vec2 pos)
    {
      pos -= this.position;
      return this.ReverseOffsetLocal(pos);
    }

    public virtual float OffsetX(float pos)
    {
      Vec2 vec2 = new Vec2(pos, 0.0f);
      if (this.offDir < (sbyte) 0)
        vec2.x *= -1f;
      return (this.position + vec2.Rotate(this.angle, new Vec2(0.0f, 0.0f))).x;
    }

    public virtual float OffsetY(float pos)
    {
      Vec2 vec2 = new Vec2(0.0f, pos);
      if (this.offDir < (sbyte) 0)
        vec2.x *= -1f;
      return (this.position + vec2.Rotate(this.angle, new Vec2(0.0f, 0.0f))).y;
    }

    public virtual void ResetProperties()
    {
      this._level = (Level) null;
      this._removeFromLevel = false;
      this._initialized = false;
      this.prevEndDrawPos = Vec2.Zero;
    }

    public void AddToLayer()
    {
      if (this._layer == null)
        this._layer = Layer.Game;
      if (Thing.skipLayerAdding)
        return;
      this._layer.Add(this);
    }

    public void DoNetworkInitialize()
    {
      if (this._networkInitialized)
        return;
      this._networkDrawIndex = NetworkDebugger.networkDrawingIndex;
      if (this.isStateObject)
      {
        this._ghostType = Editor.IDToType[this.GetType()];
        if (Network.isServer)
          this.connection = DuckNetwork.localConnection;
      }
      this._networkInitialized = true;
    }

    public virtual void DoInitialize()
    {
      if (this._redoLayer)
      {
        this.AddToLayer();
        this._redoLayer = false;
      }
      if (this._initialized)
        return;
      if (Network.isActive)
        this.DoNetworkInitialize();
      this.Initialize();
      this._initialized = true;
    }

    public virtual void Initialize()
    {
    }

    public virtual void DoUpdate()
    {
      if (this.wasSuperFondled > 0)
        --this.wasSuperFondled;
      if (this._anchor != (Thing) null)
        this.position = this._anchor.position;
      this.Update();
      this._topQuick = this.top;
      this._bottomQuick = this.bottom;
      this._leftQuick = this.left;
      this._rightQuick = this.right;
    }

    public virtual void Update()
    {
    }

    public virtual void DoEditorUpdate()
    {
      this._topQuick = this.top;
      this._bottomQuick = this.bottom;
      this._leftQuick = this.left;
      this._rightQuick = this.right;
      this.EditorUpdate();
    }

    public virtual void EditorUpdate()
    {
    }

    public virtual void DoEditorRender() => this.EditorRender();

    public virtual void EditorRender()
    {
    }

    public void Glitch()
    {
      if (!(this.material is MaterialGlitch))
        return;
      (this.material as MaterialGlitch).yoffset = Rando.Float(1f);
      (this.material as MaterialGlitch).amount = Rando.Float(0.9f, 1.2f);
    }

    public Dictionary<GhostManager, GhostObject> ghostObjectMap
    {
      get
      {
        if (this._ghostObjects == null)
          this._ghostObjects = new Dictionary<GhostManager, GhostObject>();
        return this._ghostObjects;
      }
    }

    public GhostObject ghostObject
    {
      get => this._ghostObject;
      set => this._ghostObject = value;
    }

    public bool ignoreGhosting
    {
      get => this._ignoreGhosting;
      set => this._ignoreGhosting = value;
    }

    public virtual void DoDraw()
    {
      if (NetworkDebugger.networkDrawingIndex >= 0 && NetworkDebugger.networkDrawingIndex != this._networkDrawIndex)
        return;
      DuckGame.Graphics.currentObjectIndex = (int) this._globalIndex;
      DuckGame.Graphics.currentDrawingObject = this;
      DuckGame.Graphics.material = this._material;
      if (this._material != null)
        this._material.Update();
      if (this.shouldLerp || Thing.doLerp && Network.isActive && this.connection != DuckNetwork.localConnection)
      {
        if (this.prevEndDrawPos == Vec2.Zero)
          this.prevEndDrawPos = this.position;
        Vec2 position = this.position;
        double length = (double) (this.position - this.prevEndDrawPos).length;
        this.position = Vec2.Hermite(this.prevEndDrawPos, this.prevEndVelocity, this.position, this.velocity, 0.1f);
        this.prevEndDrawPos = this.position;
        this.prevEndVelocity = this.velocity;
        this.Draw();
        this.position = position;
      }
      else
        this.Draw();
      if (Network.isActive && NetworkDebugger.enabled && this.isStateObject)
      {
        if (this.connection == DuckNetwork.localConnection || this.connection == null)
          DuckGame.Graphics.DrawRect(this.topLeft, this.bottomRight, Color.Red * 0.8f, (Depth) 1f, false);
        if (this.ghostObject != null)
          DuckGame.Graphics.DrawString(this.ghostObject._tickIncrementAmount.ToString("0.00"), this.position + new Vec2(-8f, -8f), Color.White, (Depth) 0.99f);
      }
      DuckGame.Graphics.material = (Material) null;
      DuckGame.Graphics.currentObjectIndex = -1;
      DuckGame.Graphics.currentDrawingObject = (Thing) null;
    }

    public virtual void Draw()
    {
      if (this._graphic == null)
        return;
      if (!this._skipPositioning)
      {
        this._graphic.position = this.position;
        this._graphic.alpha = this.alpha;
        this._graphic.angle = this.angle;
        this._graphic.depth = this.depth;
        this._graphic.scale = this.scale;
        this._graphic.center = this.center;
      }
      this._graphic.Draw();
    }

    public void Draw(Sprite spr, float xpos, float ypos, int d = 1) => this.Draw(spr, new Vec2(xpos, ypos), d);

    public void Draw(Sprite spr, Vec2 pos, int d = 1)
    {
      Vec2 vec2 = this.Offset(pos);
      spr.flipH = this.graphic.flipH;
      spr.angle = this.angle;
      spr.alpha = this.alpha;
      spr.depth = this.depth + d;
      spr.scale = this.scale;
      DuckGame.Graphics.Draw(spr, vec2.x, vec2.y);
    }

    public void DrawIgnoreAngle(Sprite spr, Vec2 pos, int d = 1)
    {
      Vec2 vec2 = this.Offset(pos);
      spr.alpha = this.alpha;
      spr.depth = this.depth + d;
      spr.scale = this.scale;
      DuckGame.Graphics.Draw(spr, vec2.x, vec2.y);
    }

    public virtual void OnTeleport()
    {
    }

    public virtual void DoTerminate() => this.Terminate();

    public virtual void Terminate()
    {
    }

    public virtual void Added(Level parent)
    {
      this._removeFromLevel = false;
      this._redoLayer = true;
      this._level = parent;
      this.DoInitialize();
    }

    public virtual void Added(Level parent, bool redoLayer, bool reinit)
    {
      if (reinit)
        this._initialized = false;
      this._removeFromLevel = false;
      this._redoLayer = redoLayer;
      this._level = parent;
      this.DoInitialize();
    }

    public virtual void Removed() => this._removeFromLevel = true;

    public virtual void NetworkUpdate()
    {
    }

    public virtual void OnSequenceActivate()
    {
    }
  }
}
