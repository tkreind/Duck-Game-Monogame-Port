// Decompiled with JetBrains decompiler
// Type: DuckGame.Editor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DuckGame
{
  public class Editor : Level
  {
    private static Dictionary<System.Type, List<ClassMember>> _classMembers = new Dictionary<System.Type, List<ClassMember>>();
    private static Dictionary<System.Type, Dictionary<string, ClassMember>> _classMemberNames = new Dictionary<System.Type, Dictionary<string, ClassMember>>();
    public static Dictionary<System.Type, Dictionary<string, AccessorInfo>> _accessorCache = new Dictionary<System.Type, Dictionary<string, AccessorInfo>>();
    private static Stack<object> focusStack = new Stack<object>();
    private static int numPops = 0;
    private EditorCam _editorCam;
    private SpriteMap _cursor;
    private SpriteMap _tileset;
    private BitmapFont _font;
    private ContextMenu _placementMenu;
    private ContextMenu _objectMenu;
    private CursorMode _cursorMode;
    public static bool active = false;
    public static bool selectingLevel = false;
    private static bool _onlineMode = false;
    private static bool _onlineSettingChanged = false;
    public static Texture2D previewCapture;
    private BinaryClassChunk _eyeDropperSerialized;
    private static List<string> _activatedLevels = new List<string>();
    private static Dictionary<System.Type, Thing> _thingMap = new Dictionary<System.Type, Thing>();
    private static EditorGroup _placeables;
    protected List<Thing> _levelThings = new List<Thing>();
    public static string placementItemDetails = "";
    private string _saveName = "";
    private SaveFileDialog _saveForm = new SaveFileDialog();
    private OpenFileDialog _loadForm = new OpenFileDialog();
    public static bool enteringText = false;
    private static ContextMenu _lockInput;
    private static ContextMenu _lockInputChange;
    private int _lastCommand = -1;
    private List<Command> _commands = new List<Command>();
    public static bool clickedMenu = false;
    public bool clicked;
    private bool _updateEvenWhenInactive;
    private bool _pathNodesDirty;
    private NotifyDialogue _notify;
    private bool _placementMode = true;
    private bool _editMode;
    private static string _infoText = "";
    public static bool gamepadMode = false;
    private SpriteMap _editorButtons;
    private bool _loadingLevel;
    private List<Thing> _placeObjects = new List<Thing>();
    public bool minimalConversionLoad;
    public string _guid = "";
    private bool _looseClear;
    public static string workshopName = "";
    public static string workshopAuthor = "";
    public static string workshopDescription = "";
    public static bool workshopLevelDeathmatchReady = false;
    public static int workshopVisibility = 0;
    public static ulong workshopID = 0;
    public static List<string> workshopTags = new List<string>();
    public static bool saving = false;
    private int _gridW = 40;
    private int _gridH = 24;
    private float _cellSize = 16f;
    private Vec2 _camSize = new Vec2();
    private Vec2 _panAnchor = new Vec2();
    private Vec2 _tilePosition = new Vec2();
    private bool _closeMenu;
    private bool _placingTiles;
    private bool _dragMode;
    private bool _deleteMode;
    private bool _didPan;
    private static bool _listLoaded = false;
    private Thing _placementType;
    private RenderTarget2D _steamPreviewTarget;
    private MonoFileDialog _fileDialog;
    private SteamUploadDialog _uploadDialog;
    private static string _initialDirectory;
    private bool _menuOpen;
    private Layer _gridLayer;
    private Layer _procLayer;
    public bool _pathNorth;
    public bool _pathSouth;
    public bool _pathEast;
    public bool _pathWest;
    private bool _quitting;
    private Sprite _sideArrow;
    private Sprite _sideArrowHover;
    private Sprite _die;
    private Sprite _dieHover;
    private HashSet<Thing> _selection = new HashSet<Thing>();
    private Sprite _singleBlock;
    private Sprite _multiBlock;
    public bool _miniMode;
    public float _chance = 1f;
    public int _maxPerLevel = 2;
    public bool _enableSingle;
    public bool _enableMulti;
    public bool _canMirror = true;
    public bool _isMirrored;
    public Vec2 _genSize = new Vec2(3f, 3f);
    public Vec2 _genTilePos = new Vec2(1f, 1f);
    public Vec2 _editTilePos = new Vec2(1f, 1f);
    public Vec2 _prevEditTilePos = new Vec2(1f, 1f);
    public Material _selectionMaterial;
    private string _additionalSaveDirectory;
    private bool _doingResave;
    private List<string> existingGUID = new List<string>();
    private static Dictionary<System.Type, object[]> _constructorParameters = new Dictionary<System.Type, object[]>();
    private static Dictionary<System.Type, Func<object>> _constructorParameterExpressions = new Dictionary<System.Type, Func<object>>();
    public static List<System.Type> ThingTypes;
    public static List<System.Type> GroupThingTypes;
    public static Dictionary<System.Type, List<System.Type>> AllBaseTypes;
    public static Dictionary<System.Type, IEnumerable<FieldInfo>> AllEditorFields;
    public static Dictionary<System.Type, FieldInfo[]> AllStateFields;
    public static Map<ushort, System.Type> IDToType = new Map<ushort, System.Type>();
    public static Dictionary<System.Type, Thing> _typeInstances = new Dictionary<System.Type, Thing>();
    public bool tabletMode;
    private Thing _hover;
    private Thing _secondaryHover;
    private Thing _move;
    private bool _showPlacementMenu;
    private static InputProfile _input = (InputProfile) null;
    public static bool copying = false;
    private Vec2 _tilePositionPrev = Vec2.Zero;
    public static bool tookInput = false;
    private ContextMenu _hoverMenu;
    private int _hoverMode;
    private GameContext _procContext;
    protected int _procSeed;
    private TileButton _hoverButton;
    public bool _doGen;
    private int _prevProcX;
    private int _prevProcY;
    private RandomLevelNode _currentMapNode;
    private int _loadPosX;
    private int _loadPosY;
    public static bool skipFrame = false;
    private Vec2 _dragStart;
    private bool _startedDrag;
    private Vec2 middleClickPos;
    private Vec2 lastMousePos = Vec2.Zero;
    public static string tooltip = (string) null;
    public static int _procXPos = 1;
    public static int _procYPos = 1;
    public static int _procTilesWide = 3;
    public static int _procTilesHigh = 3;
    public static bool hoverTextBox = false;
    private RenderTarget2D _procTarget;
    private Vec2 _procDrawOffset = Vec2.Zero;
    public bool hadGUID;
    protected RandomLevelData _centerTile;

    public static List<ClassMember> GetMembers<T>() => Editor.GetMembers(typeof (T));

    public static List<ClassMember> GetMembers(System.Type t)
    {
      List<ClassMember> classMemberList1 = (List<ClassMember>) null;
      if (Editor._classMembers.TryGetValue(t, out classMemberList1))
        return classMemberList1;
      Editor._classMemberNames[t] = new Dictionary<string, ClassMember>();
      List<ClassMember> classMemberList2 = new List<ClassMember>();
      FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      foreach (FieldInfo field in fields)
      {
        ClassMember classMember = new ClassMember(field.Name, t, field);
        Editor._classMemberNames[t][field.Name] = classMember;
        classMemberList2.Add(classMember);
      }
      foreach (PropertyInfo property in properties)
      {
        ClassMember classMember = new ClassMember(property.Name, t, property);
        Editor._classMemberNames[t][property.Name] = classMember;
        classMemberList2.Add(classMember);
      }
      Editor._classMembers[t] = classMemberList2;
      return classMemberList2;
    }

    public static ClassMember GetMember<T>(string name) => Editor.GetMember(typeof (T), name);

    public static ClassMember GetMember(System.Type t, string name)
    {
      Dictionary<string, ClassMember> dictionary = (Dictionary<string, ClassMember>) null;
      if (!Editor._classMemberNames.TryGetValue(t, out dictionary))
      {
        Editor.GetMembers(t);
        if (!Editor._classMemberNames.TryGetValue(t, out dictionary))
          return (ClassMember) null;
      }
      ClassMember classMember = (ClassMember) null;
      dictionary.TryGetValue(name, out classMember);
      return classMember;
    }

    internal static System.Type GetType(string name) => ModLoader.GetType(name);

    public static void CopyClass(object source, object destination)
    {
      foreach (FieldInfo field in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        field.SetValue(destination, field.GetValue(source));
    }

    public static IEnumerable<System.Type> GetSubclasses(System.Type parentType) => (IEnumerable<System.Type>) ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (assembly => (IEnumerable<System.Type>) assembly.GetTypes())).Where<System.Type>((Func<System.Type, bool>) (type => type.IsSubclassOf(parentType))).OrderBy<System.Type, string>((Func<System.Type, string>) (t => t.FullName)).ToArray<System.Type>();

    public static IEnumerable<System.Type> GetSubclassesAndInterfaces(System.Type parentType) => (IEnumerable<System.Type>) ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (assembly => (IEnumerable<System.Type>) assembly.GetTypes())).Where<System.Type>((Func<System.Type, bool>) (type => parentType.IsAssignableFrom(type))).OrderBy<System.Type, string>((Func<System.Type, string>) (t => t.FullName)).ToArray<System.Type>();

    public static AccessorInfo GetAccessorInfo(
      System.Type t,
      string name,
      FieldInfo field = null,
      PropertyInfo property = null)
    {
      AccessorInfo accessorInfo = (AccessorInfo) null;
      Dictionary<string, AccessorInfo> dictionary = (Dictionary<string, AccessorInfo>) null;
      if (Editor._accessorCache.TryGetValue(t, out dictionary))
      {
        if (dictionary.TryGetValue(name, out accessorInfo))
          return accessorInfo;
      }
      else
        Editor._accessorCache[t] = new Dictionary<string, AccessorInfo>();
      AccessorInfo accessor = Editor.CreateAccessor(field, property, t, name);
      Editor._accessorCache[t][name] = accessor;
      return accessor;
    }

    public static AccessorInfo CreateAccessor(
      FieldInfo field,
      PropertyInfo property,
      System.Type t,
      string name)
    {
      if (field == (FieldInfo) null && property == (PropertyInfo) null)
      {
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        field = t.GetField(name, bindingAttr);
        if (field == (FieldInfo) null)
          property = t.GetProperty(name, bindingAttr);
      }
      AccessorInfo accessorInfo = (AccessorInfo) null;
      if (field != (FieldInfo) null)
      {
        accessorInfo = new AccessorInfo();
        accessorInfo.type = field.FieldType;
        //accessorInfo.setAccessor = Editor.BuildSetAccessorField(t, field);
        //accessorInfo.getAccessor = Editor.BuildGetAccessorField(t, field);
      }
      else if (property != (PropertyInfo) null)
      {
        accessorInfo = new AccessorInfo();
        accessorInfo.type = property.PropertyType;
        MethodInfo setMethod = property.GetSetMethod(true);
                if (setMethod != (MethodInfo)null)
                {
                    //accessorInfo.setAccessor = Editor.BuildSetAccessorProperty(t, setMethod);
                }
        //accessorInfo.getAccessor = Editor.BuildGetAccessorProperty(t, property);
      }
      return accessorInfo;
    }

        // TODO
    //public static Action<object, object> BuildSetAccessorProperty(System.Type _, MethodInfo method)
    //{
    //  ParameterExpression parameterExpression1;
    //  ParameterExpression parameterExpression2;
    //  return Expression.Lambda<Action<object, object>>(
    //      (Expression) Expression.Call(
    //          (Expression) Expression.Convert(
    //              (Expression) parameterExpression1, method.DeclaringType),
    //          method,
    //          (Expression) Expression.Convert((Expression) parameterExpression2, method.GetParameters()[0].ParameterType)
    //          ), parameterExpression1, 
    //      parameterExpression2).Compile();
    //}

    //public static Func<object, object> BuildGetAccessorProperty(System.Type t, PropertyInfo property)
    //{
    //  ParameterExpression parameterExpression;
    //  return Expression.Lambda<Func<object, object>>((Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, t), property), typeof (object)), parameterExpression).Compile();
    //}

    //public static Action<object, object> BuildSetAccessorField(System.Type t, FieldInfo field)
    //{
    //  ParameterExpression parameterExpression;
    //  return ((Expression<Action<object, object>>) ((target, value) => Expression.Assign((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression, t), field), (Expression) Expression.Convert(value, field.FieldType)))).Compile();
    //}

    //public static Func<object, object> BuildGetAccessorField(System.Type t, FieldInfo field)
    //{
    //  ParameterExpression parameterExpression;
    //  return Expression.Lambda<Func<object, object>>((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression, t), field), typeof (object)), parameterExpression).Compile();
    //}

    public static void PopFocus() => ++Editor.numPops;

    public static void PopFocusNow()
    {
      if (Editor.focusStack.Count <= 0)
        return;
      Editor.focusStack.Pop();
    }

    public static object PeekFocus() => Editor.focusStack.Count > 0 ? Editor.focusStack.Peek() : (object) null;

    public static void PushFocus(object o) => Editor.focusStack.Push(o);

    public static bool HasFocus() => Editor.focusStack.Count != 0;

    public static bool onlineMode
    {
      get => Editor._onlineMode;
      set
      {
        if (Editor._onlineMode == value)
          return;
        Editor._onlineMode = value;
        Editor._onlineSettingChanged = true;
      }
    }

    public static List<string> activatedLevels => Editor._activatedLevels;

    public static MemoryStream GetCompressedActiveLevelData()
    {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress));
      foreach (string activatedLevel in Editor.activatedLevels)
      {
        binaryWriter.Write(true);
        binaryWriter.Write(activatedLevel);
        byte[] buffer = File.ReadAllBytes(DuckFile.levelDirectory + activatedLevel + ".lev");
        binaryWriter.Write(buffer.Length);
        binaryWriter.Write(buffer);
      }
      binaryWriter.Write(false);
      return memoryStream;
    }

    public static MemoryStream GetCompressedLevelData(string level)
    {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress));
      binaryWriter.Write(level);
      byte[] buffer = File.ReadAllBytes(DuckFile.levelDirectory + level + ".lev");
      binaryWriter.Write(buffer.Length);
      binaryWriter.Write(buffer);
      return memoryStream;
    }

    public static ReceivedLevelInfo ReadCompressedLevelData(MemoryStream stream)
    {
      stream.Position = 0L;
      BinaryReader binaryReader = new BinaryReader((Stream) new GZipStream((Stream) stream, CompressionMode.Decompress));
      string str = binaryReader.ReadString();
      int count = binaryReader.ReadInt32();
      LevelData levelData = DuckFile.LoadLevel(binaryReader.ReadBytes(count));
      return new ReceivedLevelInfo()
      {
        data = levelData,
        name = str
      };
    }

    public static uint Checksum(byte[] data) => CRC32.Generate(data);

    public static uint Checksum(byte[] data, int start, int length) => CRC32.Generate(data, start, length);

    public static Dictionary<System.Type, Thing> thingMap => Editor._thingMap;

    public static void MapThing(Thing t) => Editor._thingMap[t.GetType()] = t;

    public static Thing GetThing(System.Type t)
    {
      Thing thing = (Thing) null;
      Editor._thingMap.TryGetValue(t, out thing);
      return thing;
    }

    public static EditorGroup Placeables
    {
      get
      {
        while (!Editor._listLoaded)
          Thread.Sleep(16);
        return Editor._placeables;
      }
    }

    public List<Thing> levelThings => this._levelThings;

    public string saveName
    {
      get => this._saveName;
      set => this._saveName = value;
    }

    public static ContextMenu lockInput
    {
      get => Editor._lockInput;
      set => Editor._lockInputChange = value;
    }

    public static string infoText
    {
      get => Editor._infoText;
      set
      {
        if (!(Editor._infoText != value))
          return;
        HUD.CloseAllCorners();
        Editor._infoText = value;
        if (!(Editor._infoText != ""))
          return;
        HUD.AddCornerMessage(HUDCorner.BottomLeft, Editor._infoText);
      }
    }

    private void RunCommand(Command command)
    {
      if (this._lastCommand < this._commands.Count - 1)
        this._commands.RemoveRange(this._lastCommand + 1, this._commands.Count - (this._lastCommand + 1));
      this._commands.Add(command);
      command.Do();
      ++this._lastCommand;
    }

    private void UndoCommand()
    {
      if (this._lastCommand < 0)
        return;
      this._commands[this._lastCommand].Undo();
      --this._lastCommand;
    }

    private void RedoCommand()
    {
      if (this._lastCommand >= this._commands.Count - 1)
        return;
      ++this._lastCommand;
      this._commands[this._lastCommand].Do();
    }

    public void AddObject(Thing obj)
    {
      switch (obj)
      {
        case null:
          return;
        case ThingContainer _:
          using (List<Thing>.Enumerator enumerator = (obj as ThingContainer).things.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.AddObject(enumerator.Current);
            return;
          }
        case BackgroundUpdater _:
          for (int index = 0; index < this._levelThings.Count; ++index)
          {
            Thing levelThing = this._levelThings[index];
            if (levelThing is BackgroundUpdater)
            {
              this.RunCommand(new CommandAddObject(levelThing).Inverse());
              --index;
            }
          }
          break;
      }
      obj.active = false;
      this.AddThing(obj);
      this._levelThings.Add(obj);
      if (!this._loadingLevel && obj is IDontMove)
        this._placeObjects.Add(obj);
      Editor.workshopLevelDeathmatchReady = false;
    }

    public void RemoveObject(Thing obj)
    {
      Level.current.RemoveThing(obj);
      this._levelThings.Remove(obj);
      if (!(obj is IDontMove))
        return;
      this._placeObjects.Add(obj);
    }

    public void ClearEverything()
    {
      Editor.onlineMode = false;
      foreach (Thing levelThing in this._levelThings)
        Level.current.RemoveThing(levelThing);
      this._levelThings.Clear();
      this._lastCommand = -1;
      this._commands.Clear();
      if (!this._looseClear)
      {
        this._procContext = (GameContext) null;
        this._procTarget = (RenderTarget2D) null;
      }
      this._chance = 1f;
      this._enableSingle = false;
      this._enableMulti = false;
      this._canMirror = true;
      this._isMirrored = false;
      this._pathNorth = false;
      this._pathSouth = false;
      this._pathWest = false;
      this._pathEast = false;
      this.things.quadTree.Clear();
      Custom.ClearCustomData();
      Editor.ResetWorkshopData();
      this._guid = Guid.NewGuid().ToString();
      Editor.previewCapture = (Texture2D) null;
    }

    public static void ResetWorkshopData()
    {
      Editor.workshopLevelDeathmatchReady = false;
      Editor.workshopName = "";
      Editor.workshopAuthor = "";
      Editor.workshopDescription = "";
      Editor.workshopVisibility = 0;
      Editor.workshopID = 0UL;
      Editor.workshopTags = new List<string>();
    }

    public float cellSize
    {
      get => this._cellSize;
      set => this._cellSize = value;
    }

    private float width => (float) this._gridW * this._cellSize;

    private float height => (float) this._gridH * this._cellSize;

    public Thing placementType
    {
      set
      {
        this._placementType = value;
        this._eyeDropperSerialized = (BinaryClassChunk) null;
      }
    }

    private LevelType GetLevelType()
    {
      if (Editor.arcadeMachineMode)
        return LevelType.ArcadeMachine;
      LevelType levelType = LevelType.Deathmatch;
      if (this._levelThings.FirstOrDefault<Thing>((Func<Thing, bool>) (x => x is ChallengeMode)) != null)
        levelType = LevelType.Challenge;
      else if (this._levelThings.FirstOrDefault<Thing>((Func<Thing, bool>) (x => x is ArcadeMode)) != null)
        levelType = LevelType.Arcade;
      return levelType;
    }

    private LevelSize GetLevelSize()
    {
      this._topLeft = new Vec2(99999f, 99999f);
      this._bottomRight = new Vec2(-99999f, -99999f);
      this.CalculateBounds();
      float length = (this.topLeft - this.bottomRight).length;
      LevelSize levelSize = LevelSize.Ginormous;
      if ((double) length < 900.0)
        levelSize = LevelSize.Large;
      if ((double) length < 650.0)
        levelSize = LevelSize.Medium;
      if ((double) length < 400.0)
        levelSize = LevelSize.Small;
      if ((double) length < 200.0)
        levelSize = LevelSize.Tiny;
      return levelSize;
    }

    private bool LevelIsOnlineCapable()
    {
      foreach (object levelThing in this._levelThings)
      {
        if (!ContentProperties.GetBag(levelThing.GetType()).GetOrDefault<bool>("isOnlineCapable", true))
          return false;
      }
      return true;
    }

    public void SteamUpload()
    {
      if (this._saveName == "")
      {
        this.DoMenuClose();
        this._closeMenu = false;
        this._notify.Open("Please save the map first...");
      }
      else
      {
        this.Save();
        this._steamPreviewTarget = new RenderTarget2D(1280, 720);
        LevelType levelType = this.GetLevelType();
        LevelSize levelSize = this.GetLevelSize();
        List<string> stringList = new List<string>();
        if (this._levelThings.Exists((Predicate<Thing>) (x => x is CustomCamera)))
          stringList.Add("Fixed Camera");
        if (Editor.arcadeMachineMode)
        {
          this._steamPreviewTarget = new RenderTarget2D(256, 256);
          Content.customPreviewWidth = 128;
          Content.customPreviewHeight = 128;
          Content.customPreviewCenter = this._levelThings[0].position;
        }
        Content.GeneratePreview(this._saveName, this._steamPreviewTarget, true);
        this._uploadDialog.Open(this._steamPreviewTarget, this._saveName, Editor.workshopID, levelType, levelSize, stringList.ToArray());
        this.DoMenuClose();
        this._closeMenu = false;
        Content.customPreviewWidth = 0;
        Content.customPreviewHeight = 0;
        Content.customPreviewCenter = Vec2.Zero;
      }
    }

    public MonoFileDialog fileDialog => this._fileDialog;

    public static string initialDirectory => Editor._initialDirectory;

    public void EnterEditor()
    {
      this._placementType = (Thing) null;
      Layer.ClearLayers();
      this._gridLayer = new Layer("GRID", Layer.Background.depth + 5, Layer.Background.camera);
      Layer.Add(this._gridLayer);
      this._procLayer = new Layer("PROC", Layer.Background.depth + 25, new Camera(0.0f, 0.0f, (float) DuckGame.Graphics.width, (float) DuckGame.Graphics.height));
      Layer.Add(this._procLayer);
      Layer.HUD.camera.width *= 2f;
      Layer.HUD.camera.height *= 2f;
      Layer.Game.camera.width *= 2f;
      Layer.Game.camera.height *= 2f;
      this.CenterView();
      this._tilePosition = new Vec2(10f * this._cellSize, 10f * this._cellSize);
      this.backgroundColor = new Color(20, 20, 20);
      Editor.focusStack.Clear();
      Editor.active = true;
    }

    public void Quit() => this._quitting = true;

    public override void DoInitialize()
    {
      SFX.StopAllSounds();
      if (!this._initialized)
      {
        this.Initialize();
        this._initialized = true;
      }
      else
      {
        this.EnterEditor();
        base.DoInitialize();
      }
    }

    public override void Terminate()
    {
    }

    public string additionalSaveDirectory => this._additionalSaveDirectory;

    public override void Initialize()
    {
      while (!Editor._listLoaded)
        Thread.Sleep(16);
      this._editorCam = new EditorCam();
      this.camera = (Camera) this._editorCam;
      this._selectionMaterial = new Material("Shaders/selection");
      this._cursor = new SpriteMap("cursors", 16, 16);
      this._tileset = new SpriteMap("industrialTileset", 16, 16);
      this._sideArrow = new Sprite("Editor/sideArrow");
      this._sideArrow.CenterOrigin();
      this._sideArrowHover = new Sprite("Editor/sideArrowHover");
      this._sideArrowHover.CenterOrigin();
      this._die = new Sprite("die");
      this._dieHover = new Sprite("dieHover");
      this._singleBlock = new Sprite("Editor/singleplayerBlock");
      this._multiBlock = new Sprite("Editor/multiplayerBlock");
      this.EnterEditor();
      this._camSize = new Vec2(this.camera.width, this.camera.height);
      this._font = new BitmapFont("biosFont", 8);
      Editor._input = InputProfile.Get("MPPlayer1");
      this._tilePosition = new Vec2(10f * this._cellSize, 10f * this._cellSize);
      this._tilePositionPrev = this._tilePosition;
      this._objectMenu = (ContextMenu) new PlacementMenu(0.0f, 0.0f);
      Level.Add((Thing) this._objectMenu);
      this._objectMenu.visible = this._objectMenu.active = false;
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_chance", increment: 0.05f), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/dieBlock", 16, 16), "CHANCE - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridBottomLeft));
      Level.Add((Thing) new TileButton(0.0f, 16f, new FieldBinding((object) this, "_maxPerLevel", -1f, 8f, 1f), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/numBlock", 16, 16), "MAX IN LEVEL - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridBottomLeft));
      Level.Add((Thing) new TileButton(-16f, 0.0f, new FieldBinding((object) this, "_enableSingle"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/singleplayerBlock", 16, 16), "AVAILABLE IN SINGLE PLAYER - @SELECT@TOGGLE", TileButtonAlign.TileGridBottomRight));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_enableMulti"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/multiplayerBlock", 16, 16), "AVAILABLE IN MULTI PLAYER - @SELECT@TOGGLE", TileButtonAlign.TileGridBottomRight));
      Level.Add((Thing) new TileButton(-16f, 16f, new FieldBinding((object) this, "_canMirror"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/canMirror", 16, 16), "TILE CAN BE MIRRORED - @SELECT@TOGGLE", TileButtonAlign.TileGridBottomRight));
      Level.Add((Thing) new TileButton(0.0f, 16f, new FieldBinding((object) this, "_isMirrored"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/isMirrored", 16, 16), "PRE MIRRORED TILE - @SELECT@TOGGLE", TileButtonAlign.TileGridBottomRight));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_pathEast"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/sideArrow", 32, 16), "CONNECTS EAST - @SELECT@TOGGLE", TileButtonAlign.TileGridRight, 90f));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_pathWest"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/sideArrow", 32, 16), "CONNECTS WEST - @SELECT@TOGGLE", TileButtonAlign.TileGridLeft, -90f));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_pathNorth"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/sideArrow", 32, 16), "CONNECTS NORTH - @SELECT@TOGGLE", TileButtonAlign.TileGridTop));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_pathSouth"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/sideArrow", 32, 16), "CONNECTS SOUTH - @SELECT@TOGGLE", TileButtonAlign.TileGridBottom, 180f));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_genSize", 1f, 6f, 1f), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/resizeBlock", 16, 16), "RESIZE GEN - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridTopLeft));
      Level.Add((Thing) new TileButton(16f, 0.0f, new FieldBinding((object) this, "_genTilePos", max: 6f, increment: 1f), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/moveBlock", 16, 16), "MOVE GEN - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridTopLeft));
      Level.Add((Thing) new TileButton(32f, 0.0f, new FieldBinding((object) this, "_editTilePos", max: 6f, increment: 1f), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/editBlock", 16, 16), "MOVE GEN - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridTopLeft));
      Level.Add((Thing) new TileButton(0.0f, 0.0f, new FieldBinding((object) this, "_doGen"), new FieldBinding((object) this, "_miniMode"), new SpriteMap("Editor/regenBlock", 16, 16), "REGENERATE - HOLD @SELECT@ AND MOVE @DPAD@", TileButtonAlign.TileGridTopRight));
      this._notify = new NotifyDialogue();
      Level.Add((Thing) this._notify);
      Editor._initialDirectory = DuckFile.levelDirectory;
      string path = Directory.GetCurrentDirectory() + "\\..\\..\\..\\assets\\levels";
      if (Directory.Exists(path))
      {
        this._additionalSaveDirectory = Editor._initialDirectory;
        Editor._initialDirectory = path;
      }
      Editor._initialDirectory = Path.GetFullPath(Editor._initialDirectory);
      this._fileDialog = new MonoFileDialog();
      Level.Add((Thing) this._fileDialog);
      this._uploadDialog = new SteamUploadDialog();
      Level.Add((Thing) this._uploadDialog);
      this._editorButtons = new SpriteMap("editorButtons", 32, 32);
      Editor.ResetWorkshopData();
      this._guid = Guid.NewGuid().ToString();
      Editor.onlineMode = false;
      this._doingResave = true;
      this._doingResave = false;
    }

    public Vec2 GetAlignOffset(TileButtonAlign align)
    {
      switch (align)
      {
        case TileButtonAlign.ProcGridTopLeft:
          int num1 = 192;
          int num2 = 144;
          return new Vec2()
          {
            x = (float) (-(Editor._procTilesWide - (Editor._procTilesWide - Editor._procXPos)) * num1),
            y = (float) (-(Editor._procTilesHigh - (Editor._procTilesHigh - Editor._procYPos)) * num2 - 16)
          };
        case TileButtonAlign.TileGridTopLeft:
          return new Vec2() { x = 0.0f, y = -16f };
        case TileButtonAlign.TileGridTopRight:
          int num3 = 192;
          return new Vec2()
          {
            x = (float) (num3 - 16),
            y = -16f
          };
        case TileButtonAlign.TileGridBottomLeft:
          int num4 = 144;
          return new Vec2() { x = 0.0f, y = (float) num4 };
        case TileButtonAlign.TileGridBottomRight:
          int num5 = 144;
          int num6 = 192;
          return new Vec2()
          {
            x = (float) (num6 - 16),
            y = (float) num5
          };
        case TileButtonAlign.TileGridRight:
          return new Vec2(192f, (float) (144 / 2 - 8));
        case TileButtonAlign.TileGridTop:
          return new Vec2((float) (192 / 2 - 8), -16f);
        case TileButtonAlign.TileGridLeft:
          return new Vec2(-16f, (float) (144 / 2 - 8));
        case TileButtonAlign.TileGridBottom:
          return new Vec2((float) (192 / 2 - 8), 144f);
        default:
          return Vec2.Zero;
      }
    }

    private void Resave(string root)
    {
      foreach (string load in DuckFile.GetFilesNoCloud(root, "*.lev"))
      {
        try
        {
          this.LoadLevel(load);
          this._things.RefreshState();
          this._updateEvenWhenInactive = true;
          this.Update();
          this._updateEvenWhenInactive = false;
          if (this.existingGUID.Contains(this._guid))
            this._guid = Guid.NewGuid().ToString();
          this.existingGUID.Add(this._guid);
          this.Save();
          Thread.Sleep(10);
        }
        catch (Exception ex)
        {
        }
      }
      foreach (string root1 in DuckFile.GetDirectoriesNoCloud(root))
        this.Resave(root1);
    }

    private static object GetDefaultValue(System.Type t) => t.IsValueType ? Activator.CreateInstance(t) : (object) null;

    public static Thing GetOrCreateTypeInstance(System.Type t)
    {
      Thing thing1 = (Thing) null;
      if (!Editor._thingMap.TryGetValue(t, out thing1) && Editor.CreateObject(t) is Thing thing2)
      {
        Editor._thingMap[t] = thing2;
        thing1 = thing2;
      }
      return thing1;
    }

    public static object CreateObject(System.Type t)
    {
      Func<object> func = (Func<object>) null;
      return Editor._constructorParameterExpressions.TryGetValue(t, out func) ? func() : (object) null;
    }

    public static void InitializeConstructorLists()
    {
      MonoMain.loadMessage = "Loading Constructor Lists";
      if (MonoMain.moddingEnabled)
      {
        MonoMain.loadMessage = "Loading Constructor Lists";
        Editor.ThingTypes = ManagedContent.Things.SortedTypes.ToList<System.Type>();
      }
      else
        Editor.ThingTypes = Editor.GetSubclasses(typeof (Thing)).ToList<System.Type>();
      Editor.GroupThingTypes = new List<System.Type>();
      Editor.GroupThingTypes.AddRange((IEnumerable<System.Type>) Editor.ThingTypes);
      Editor.AllBaseTypes = new Dictionary<System.Type, List<System.Type>>();
      Editor.AllEditorFields = new Dictionary<System.Type, IEnumerable<FieldInfo>>();
      Editor.AllStateFields = new Dictionary<System.Type, FieldInfo[]>();
      System.Type editorFieldType = typeof (EditorProperty<>);
      System.Type stateFieldType = typeof (StateBinding);
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      ushort key = 2;
      foreach (System.Type thingType in Editor.ThingTypes)
      {
        Editor.AllBaseTypes[thingType] = Thing.GetAllTypes(thingType);
        FieldInfo[] fields = thingType.GetFields(bindingAttr);
        Editor.AllEditorFields[thingType] = (IEnumerable<FieldInfo>) ((IEnumerable<FieldInfo>) fields).Where<FieldInfo>((Func<FieldInfo, bool>) (val => val.FieldType.IsGenericType && val.FieldType.GetGenericTypeDefinition() == editorFieldType)).ToArray<FieldInfo>();
        Editor.AllStateFields[thingType] = ((IEnumerable<FieldInfo>) fields).Where<FieldInfo>((Func<FieldInfo, bool>) (val => val.FieldType == stateFieldType)).ToArray<FieldInfo>();
        if (((IEnumerable<FieldInfo>) Editor.AllStateFields[thingType]).Count<FieldInfo>() > 2)
        {
          Editor.IDToType[key] = thingType;
          ++key;
        }
      }
      foreach (System.Type thingType in Editor.ThingTypes)
      {
        foreach (MethodBase constructor in thingType.GetConstructors())
        {
          ParameterInfo[] parameters = constructor.GetParameters();
          if (parameters.Length == 0)
          {
            Editor._constructorParameters[thingType] = new object[0];
          }
          else
          {
            object[] objArray = new object[parameters.Length];
            int index = 0;
            foreach (ParameterInfo parameterInfo in parameters)
            {
              System.Type parameterType = parameterInfo.ParameterType;
              objArray[index] = parameterInfo.DefaultValue == null || !(parameterInfo.DefaultValue.GetType() != typeof (DBNull)) ? Editor.GetDefaultValue(parameterType) : parameterInfo.DefaultValue;
              ++index;
            }
            Editor._constructorParameters[thingType] = objArray;
          }
        }
        ++MonoMain.loadyBits;
      }
      Program.constructorsLoaded = Editor._constructorParameters.Count;
      Program.thingTypes = Editor.ThingTypes.Count;
      foreach (System.Type thingType in Editor.ThingTypes)
      {
        ConstructorInfo[] constructors = thingType.GetConstructors();
        foreach (ConstructorInfo constructorInfo in constructors)
        {
          ConstructorInfo info = constructorInfo;
          ParameterInfo[] parameters = info.GetParameters();
          if (parameters.Length == 0)
          {
            Editor._constructorParameterExpressions[thingType] = (Func<object>) (() => info.Invoke((object[]) null));
          }
          else
          {
            int index = 0;
            object[] vals = new object[parameters.Length];
            foreach (ParameterInfo parameterInfo in parameters)
            {
              System.Type parameterType = parameterInfo.ParameterType;
              vals[index] = parameterInfo.DefaultValue == null || !(parameterInfo.DefaultValue.GetType() != typeof (DBNull)) ? Editor.GetDefaultValue(parameterType) : parameterInfo.DefaultValue;
              ++index;
            }
            Editor._constructorParameterExpressions[thingType] = (Func<object>) (() => info.Invoke(vals));
          }
        }
        ++MonoMain.loadyBits;
      }
    }

    public static void InitializePlaceableList()
    {
      if (Editor._placeables != null)
        return;
      Editor.InitializeConstructorLists();
      Editor.InitializePlaceableGroup();
    }

    public static void InitializePlaceableGroup()
    {
      MonoMain.loadMessage = "Loading Editor Groups";
      Editor._placeables = new EditorGroup((System.Type) null, (HashSet<System.Type>) null);
      AutoUpdatables.Clear();
      Editor._listLoaded = true;
    }

    public static bool HasConstructorParameter(System.Type t) => Editor._constructorParameters.ContainsKey(t);

    public static object[] GetConstructorParameters(System.Type t)
    {
      object[] objArray = (object[]) null;
      Editor._constructorParameters.TryGetValue(t, out objArray);
      if (objArray == null)
      {
        int num = 0;
        try
        {
          Editor.ThingTypes = !MonoMain.moddingEnabled ? Editor.GetSubclasses(typeof (Thing)).ToList<System.Type>() : ManagedContent.Things.SortedTypes.ToList<System.Type>();
          num = Editor.ThingTypes.Count;
        }
        catch (Exception ex)
        {
        }
        throw new Exception("Error loading constructor parameters for type " + t.ToString() + "(" + Editor._constructorParameters.Count.ToString() + " parms vs " + Program.thingTypes.ToString() + ", " + Program.constructorsLoaded.ToString() + ", " + num.ToString() + " things vs " + Program.thingTypes.ToString() + ")");
      }
      return objArray;
    }

    public static InputProfile input => Editor._input;

    public void OpenMenu(ContextMenu menu)
    {
      menu.active = menu.visible = true;
      menu.x = Mouse.x;
      menu.y = Mouse.y;
      if (this._showPlacementMenu)
      {
        menu.x = 96f;
        menu.y = 32f;
        this._showPlacementMenu = false;
      }
      if (Editor.gamepadMode)
      {
        menu.x = 16f;
        menu.y = 16f;
      }
      menu.opened = true;
      this._placementMenu = menu;
    }

    public static Thing CreateThing(System.Type t) => Activator.CreateInstance(t, Editor.GetConstructorParameters(t)) as Thing;

    public static Thing CreateThing(System.Type t, object[] p) => Activator.CreateInstance(t, p) as Thing;

    public override void Update()
    {
      Editor.tooltip = (string) null;
      foreach (Thing thing in this.things)
        thing.DoEditorUpdate();
      if (!DuckGame.Graphics.inFocus && !this._updateEvenWhenInactive)
        return;
      if (Editor.clickedMenu)
      {
        Editor.clickedMenu = false;
      }
      else
      {
        if (this._notify.opened)
          return;
        Editor.hoverTextBox = false;
        if (this.lastMousePos == Vec2.Zero)
          this.lastMousePos = Mouse.position;
        if (Mouse.left == InputState.Pressed || Mouse.right == InputState.Pressed || Mouse.middle == InputState.Pressed || (double) (this.lastMousePos - Mouse.position).length > 3.0)
        {
          Editor.gamepadMode = false;
          if (Editor._input != null)
            Editor._input.lastActiveDevice = Editor._input.GetDevice(typeof (Keyboard));
        }
        this.lastMousePos = Mouse.position;
        if (Editor.numPops > 0)
        {
          for (int index = 0; index < Editor.numPops && Editor.focusStack.Count != 0; ++index)
            Editor.focusStack.Pop();
          Editor.numPops = 0;
        }
        Editor.tookInput = false;
        if (Editor.focusStack.Count > 0 || Editor.skipFrame)
          Editor.skipFrame = false;
        else if (Editor.lockInput != null)
        {
          if (Editor._lockInputChange == Editor.lockInput)
            return;
          Editor._lockInput = Editor._lockInputChange;
        }
        else
        {
          if (Editor._lockInputChange != Editor.lockInput)
            Editor._lockInput = Editor._lockInputChange;
          if (Keyboard.Pressed(Keys.F10))
            this._miniMode = !this._miniMode;
          if (Editor._onlineSettingChanged && this._placementMenu != null && this._placementMenu is EditorGroupMenu)
          {
            (this._placementMenu as EditorGroupMenu).UpdateGrayout();
            Editor._onlineSettingChanged = false;
          }
          Layer.Background.fade = 0.5f;
          DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, this._quitting ? 0.0f : 1f, 0.02f);
          if (this._quitting && (double) DuckGame.Graphics.fade < 0.00999999977648258)
          {
            this._quitting = false;
            Editor.active = false;
            Level.current = (Level) new TitleScreen();
          }
          if ((double) DuckGame.Graphics.fade < 0.949999988079071)
            return;
          if (!Editor.gamepadMode && InputProfile.active.Pressed("ANY", true) && (!Keyboard.Pressed(Keys.A, true) || InputProfile.active.Pressed("LEFT") || (InputProfile.active.Pressed("RIGHT") || InputProfile.active.Pressed("UP")) || InputProfile.active.Pressed("DOWN")))
          {
            Editor.gamepadMode = true;
            this._tilePosition = new Vec2(10f * this._cellSize, 10f * this._cellSize);
            this._tilePositionPrev = this._tilePosition;
          }
          Layer layer = this._placementType != null ? this._placementType.placementLayer : Layer.Game;
          if (this._placementType != null && this._placementType.placementLayerOverride != null)
            layer = this._placementType.placementLayerOverride;
          if (this._placementType is AutoBlock)
            layer = Layer.Blocks;
          if (layer == null)
            layer = Layer.Game;
          this.clicked = Mouse.left == InputState.Pressed;
          if (Mouse.middle == InputState.Pressed)
            this.middleClickPos = Mouse.position;
          if (Keyboard.Down(Keys.RightShift) || Keyboard.Down(Keys.LeftShift))
          {
            Vec2 vec2 = new Vec2(0.0f, 0.0f);
            if (Keyboard.Pressed(Keys.Up))
              vec2.y -= 16f;
            if (Keyboard.Pressed(Keys.Down))
              vec2.y += 16f;
            if (Keyboard.Pressed(Keys.Left))
              vec2.x -= 16f;
            if (Keyboard.Pressed(Keys.Right))
              vec2.x += 16f;
            if (vec2 != Vec2.Zero)
            {
              foreach (Thing thing in Level.current.things)
                thing.position = thing.position + vec2;
            }
          }
          this._menuOpen = false;
          foreach (ContextMenu contextMenu in this.things[typeof (ContextMenu)])
          {
            if (contextMenu.visible && contextMenu.opened)
            {
              this.clicked = false;
              this._menuOpen = true;
            }
          }
          if (Editor.gamepadMode)
          {
            Editor._input = InputProfile.active;
            if (InputProfile.active.Pressed("RAGDOLL"))
              this.cellSize = (double) this.cellSize != 16.0 ? 16f : 8f;
          }
          if (this._prevEditTilePos != this._editTilePos)
          {
            if ((double) this._editTilePos.x < 0.0)
              this._editTilePos.x = 0.0f;
            if ((double) this._editTilePos.x >= (double) Editor._procTilesWide)
              this._editTilePos.x = (float) (Editor._procTilesWide - 1);
            if ((double) this._editTilePos.y < 0.0)
              this._editTilePos.y = 0.0f;
            if ((double) this._editTilePos.y >= (double) Editor._procTilesHigh)
              this._editTilePos.y = (float) (Editor._procTilesHigh - 1);
            if (this._currentMapNode != null)
            {
              RandomLevelData data = this._currentMapNode.map[(int) this._editTilePos.x, (int) this._editTilePos.y].data;
              if (this._levelThings.Count > 0)
                this.Save();
              this._looseClear = true;
              if (data == null)
              {
                this.ClearEverything();
                this._saveName = "";
              }
              else
                this.LoadLevel(Directory.GetCurrentDirectory() + "\\..\\..\\..\\assets\\levels\\" + data.file + ".lev");
              Editor._procXPos = (int) this._editTilePos.x;
              Editor._procYPos = (int) this._editTilePos.y;
              this._genTilePos = new Vec2((float) Editor._procXPos, (float) Editor._procYPos);
              this._prevEditTilePos = this._editTilePos;
              int num1 = 144;
              int num2 = 192;
              this._procDrawOffset += new Vec2((float) ((Editor._procXPos - this._prevProcX) * num2), (float) ((Editor._procYPos - this._prevProcY) * num1));
              this._prevProcX = Editor._procXPos;
              this._prevProcY = Editor._procYPos;
            }
          }
          if (Editor._procXPos != this._prevProcX)
            this._doGen = true;
          else if (Editor._procYPos != this._prevProcY)
            this._doGen = true;
          this._prevEditTilePos = this._editTilePos;
          this._prevProcX = Editor._procXPos;
          this._prevProcY = Editor._procYPos;
          if (this._miniMode && (Keyboard.Pressed(Keys.F1) || this._doGen) && !this._doingResave)
          {
            if (this._saveName == "")
              this._saveName = Editor._initialDirectory + "/pyramid/" + Guid.NewGuid().ToString() + ".lev";
            LevelGenerator.ReInitialize();
            this._procSeed = Rando.Int(2147483646);
            string str = this._saveName.Substring(this._saveName.LastIndexOf("assets/levels/") + "assets/levels/".Length);
            string realName = str.Substring(0, str.Length - 4);
            RandomLevelData tile = LevelGenerator.LoadInTile(this.SaveTempVersion(), realName);
            this._loadPosX = Editor._procXPos;
            this._loadPosY = Editor._procYPos;
            LevGenType type = LevGenType.Any;
            if (this._enableSingle && !this._enableMulti)
              type = LevGenType.SinglePlayer;
            else if (!this._enableSingle && this._enableMulti)
              type = LevGenType.Deathmatch;
            this._editTilePos = this._prevEditTilePos = this._genTilePos;
            this._currentMapNode = LevelGenerator.MakeLevel(tile, this._pathEast && this._pathWest, this._procSeed, type, Editor._procTilesWide, Editor._procTilesHigh, this._loadPosX, this._loadPosY);
            this._procDrawOffset = new Vec2(0.0f, 0.0f);
            this._procContext = new GameContext();
            this._procContext.ApplyStates();
            Level level = new Level();
            level.backgroundColor = new Color(0, 0, 0, 0);
            Level.core.currentLevel = level;
            this._currentMapNode.LoadParts(0.0f, 0.0f, level, this._procSeed);
            this._procContext.RevertStates();
            this._doGen = false;
          }
          this._looseClear = false;
          if (Mouse.middle == InputState.Pressed)
            this.CloseMenu();
          if (this._procContext != null)
            this._procContext.Update();
          if (this.tabletMode && this.clicked)
          {
            if ((double) Mouse.x < 32.0 && (double) Mouse.y < 32.0)
            {
              this._placementMode = true;
              this._editMode = false;
              this.clicked = false;
              return;
            }
            if ((double) Mouse.x < 64.0 && (double) Mouse.y < 32.0)
            {
              this._placementMode = false;
              this._editMode = true;
              this.clicked = false;
              return;
            }
            if ((double) Mouse.x < 96.0 && (double) Mouse.y < 32.0)
            {
              if (this._placementMenu == null)
                this._showPlacementMenu = true;
              else
                this.CloseMenu();
              this.clicked = false;
              return;
            }
          }
          if (this._pathNodesDirty)
          {
            foreach (PathNode pathNode in this.things[typeof (PathNode)])
            {
              pathNode.UninitializeLinks();
              pathNode.Update();
            }
            this._pathNodesDirty = false;
          }
          this.things.RefreshState();
          if (this._placeObjects.Count > 0)
          {
            foreach (Thing placeObject in this._placeObjects)
            {
              foreach (Thing thing in Level.CheckRectAll<IDontMove>(placeObject.topLeft + new Vec2(-16f, -16f), placeObject.bottomRight + new Vec2(16f, 16f)))
                thing.EditorObjectsChanged();
            }
            this._placeObjects.Clear();
          }
          if (this._placementMenu != null)
          {
            if (Mouse.right == InputState.Pressed)
              this.CloseMenu();
          }
          else
            Editor.infoText = "";
          if (Keyboard.Down(Keys.LeftControl) || Keyboard.Down(Keys.RightControl))
          {
            bool flag = false;
            if (Keyboard.Down(Keys.LeftShift) || Keyboard.Down(Keys.RightShift))
              flag = true;
            if (Keyboard.Pressed(Keys.Z))
            {
              if (flag)
                this.RedoCommand();
              else
                this.UndoCommand();
            }
          }
          if (Editor._input.Pressed("GRAB") || this._showPlacementMenu)
          {
            if (this._placementMenu == null)
            {
              this._placementMenu = this._objectMenu;
              this.OpenMenu(this._placementMenu);
              SFX.Play("openClick", 0.4f);
            }
            else
              this.CloseMenu();
          }
          if (this._placementType is AutoBlock)
            this.cellSize = 16f;
          if (this._placementMenu == null && this._hoverMode == 0)
          {
            IEnumerable<Thing> things = !Editor.gamepadMode ? this.CollisionPointAll<Thing>(Mouse.positionScreen) : this.CollisionPointAll<Thing>(this._tilePosition);
            Thing hover = this._hover;
            this._hover = (Thing) null;
            this._secondaryHover = (Thing) null;
            List<Thing> source = new List<Thing>();
            foreach (Thing thing in things)
            {
              if (!(thing is TileButton) && Editor._placeables.Contains(thing.GetType()) && (!(this._placementType is WireTileset) || !(thing is IWirePeripheral)) && (!(this._placementType is IWirePeripheral) || !(thing is WireTileset)))
              {
                if (thing.placementLayer != layer)
                  source.Add(thing);
                else if (this._hover == null)
                {
                  if (this._placementType != null && this._placementType is BackgroundTile)
                  {
                    if (thing.editorCanModify && this._things.Contains(thing))
                    {
                      if (thing.GetType() == this._placementType.GetType())
                        this._hover = thing;
                      else
                        source.Add(thing);
                    }
                  }
                  else if (thing.editorCanModify && this._things.Contains(thing))
                    this._hover = thing;
                }
                else if (thing != this._hover)
                  source.Add(thing);
              }
            }
            Vec2 vec2;
            if (this._hover == null && !(this._placementType is BackgroundTile))
            {
              List<KeyValuePair<float, Thing>> keyValuePairList = Level.current.nearest(this._tilePosition, this._levelThings.AsEnumerable<Thing>(), (Thing) null, layer, true);
              if (keyValuePairList.Count > 0 && (!(this._placementType is WireTileset) || !(keyValuePairList[0].Value is IWirePeripheral)) && (!(this._placementType is IWirePeripheral) || !(keyValuePairList[0].Value is WireTileset)))
              {
                vec2 = keyValuePairList[0].Value.position - this._tilePosition;
                if ((double) vec2.length < 8.0)
                  this._hover = keyValuePairList[0].Value;
              }
            }
            if (this._hover == null || hover == null || this._hover.GetType() != hover.GetType())
              this._hoverMenu = this._hover != null ? this._hover.GetContextMenu() : (ContextMenu) null;
            if ((this._hover == null || this._placementType is BackgroundTile) && source.Count > 0)
            {
              this._secondaryHover = source.OrderBy<Thing, int>((Func<Thing, int>) (x => x.placementLayer == null ? -99999 : x.placementLayer.depth)).First<Thing>();
              if (this._hoverMenu == null)
                this._hoverMenu = this._secondaryHover.GetContextMenu();
            }
            bool flag = false;
            if (Mouse.middle == InputState.Released)
            {
              vec2 = this.middleClickPos - Mouse.position;
              if ((double) vec2.length < 2.0)
                flag = true;
            }
            if (this._secondaryHover != null)
            {
              if (Input.Pressed("QUACK") || flag)
              {
                Editor.copying = true;
                this._eyeDropperSerialized = this._secondaryHover.Serialize();
                Editor.copying = false;
                this._placementType = Thing.LoadThing(this._eyeDropperSerialized);
              }
            }
            else if (this._hover != null && (Input.Pressed("QUACK") || flag))
            {
              Editor.copying = true;
              this._eyeDropperSerialized = this._hover.Serialize();
              Editor.copying = false;
              this._placementType = Thing.LoadThing(this._eyeDropperSerialized);
            }
            TileButton tileButton = this.CollisionPoint<TileButton>(this._tilePosition);
            if (tileButton != null)
            {
              if (!tileButton.visible)
              {
                tileButton = (TileButton) null;
              }
              else
              {
                tileButton.hover = true;
                tileButton.focus = Editor._input.Down("SELECT") || Mouse.left == InputState.Down || Mouse.left == InputState.Pressed ? Editor._input : (InputProfile) null;
              }
            }
            if (tileButton != this._hoverButton && this._hoverButton != null)
              this._hoverButton.focus = (InputProfile) null;
            this._hoverButton = tileButton;
          }
          if (this._hoverMenu != null && !this._placingTiles && (Mouse.right == InputState.Pressed || Editor._input.Pressed("SHOOT")))
          {
            if (this._placementMenu == null)
            {
              if (this._hover != null)
              {
                this._placementMenu = this._hover.GetContextMenu();
                if (this._placementMenu != null)
                  this.AddThing((Thing) this._placementMenu);
              }
              if (this._placementMenu != null)
              {
                this.OpenMenu(this._placementMenu);
                SFX.Play("openClick", 0.4f);
              }
            }
            else if (Mouse.right == InputState.Pressed)
              this.CloseMenu();
          }
          if (this._hoverMenu == null && Mouse.right == InputState.Pressed)
          {
            if (this._hover is BackgroundTile)
            {
              if (this._placingTiles && this._placementMenu == null)
              {
                int frame = this._placementType.frame;
                this._placementMenu = (ContextMenu) new ContextBackgroundTile(this._placementType, (IContextListener) null, false);
                this._placementMenu.opened = true;
                SFX.Play("openClick", 0.4f);
                this._placementMenu.x = 16f;
                this._placementMenu.y = 16f;
                this._placementMenu.selectedIndex = frame;
                Level.Add((Thing) this._placementMenu);
              }
            }
            else if (this._placementMenu == null)
            {
              this._placementMenu = this._objectMenu;
              this.OpenMenu(this._placementMenu);
              SFX.Play("openClick", 0.4f);
            }
            else
              this.CloseMenu();
          }
          if (Editor.gamepadMode && Editor._input.Pressed("QUACK") && this._placementMenu != null)
            this.CloseMenu();
          if (this._placementMenu == null)
          {
            float num1 = Mouse.scroll;
            if (Editor.gamepadMode)
            {
              num1 = Editor._input.leftTrigger - Editor._input.rightTrigger;
              float num2 = (float) ((double) this.camera.width / (double) MonoMain.screenWidth * 5.0);
              if (Editor._input.Down("STRAFE"))
                num2 *= 2f;
              if ((double) num2 < 5.0)
                num2 = 5f;
              this.camera.x += Editor._input.rightStick.x * num2;
              this.camera.y -= Editor._input.rightStick.y * num2;
            }
            if ((double) num1 != 0.0)
            {
              int num2 = Math.Sign(num1);
              Vec2 vec2_1 = Mouse.position - this.camera.center;
              double num3 = (double) this.camera.height / (double) this.camera.width;
              float num4 = (float) num2 * 64f;
              if (Editor.gamepadMode)
                num4 = num1 * 32f;
              Vec2 vec2_2 = new Vec2(this.camera.width, this.camera.height);
              Vec2 vec2_3 = this.camera.transformScreenVector(Mouse.mousePos);
              if (Editor.gamepadMode)
                vec2_3 = this._tilePosition;
              this.camera.width += num4;
              if ((double) this.camera.width < 64.0)
                this.camera.width = 64f;
              this.camera.height = this.camera.width * DuckGame.Graphics.aspect;
              Vec2 position = this.camera.position;
              Vec3 translation;
              (Matrix.CreateTranslation(new Vec3(position.x, position.y, 0.0f)) * Matrix.CreateTranslation(new Vec3(-vec2_3.x, -vec2_3.y, 0.0f)) * Matrix.CreateScale(this.camera.width / vec2_2.x, this.camera.height / vec2_2.y, 1f) * Matrix.CreateTranslation(new Vec3(vec2_3.x, vec2_3.y, 0.0f))).Decompose(out Vec3 _, out Quaternion _, out translation);
              this.camera.position = new Vec2(translation.x, translation.y);
            }
            if (Mouse.middle == InputState.Pressed)
              this._panAnchor = Mouse.position;
            if (Mouse.middle == InputState.Down)
            {
              Vec2 vec2 = Mouse.position - this._panAnchor;
              this._panAnchor = Mouse.position;
              float num2 = this.camera.width / Layer.HUD.width;
              if ((double) vec2.length > 0.01)
                this._didPan = true;
              this.camera.x -= vec2.x * num2;
              this.camera.y -= vec2.y * num2;
            }
            if (Mouse.middle == InputState.Released)
            {
              int num2 = this._didPan ? 1 : 0;
              this._didPan = false;
            }
            bool flag = false;
            if (Editor.gamepadMode)
            {
              int num2 = 1;
              if (Editor._input.Down("STRAFE"))
                num2 = 4;
              this._tilePosition = this._tilePositionPrev;
              if ((double) this._tilePosition.x < (double) this.camera.left)
                this._tilePosition.x = this.camera.left + 32f;
              if ((double) this._tilePosition.x > (double) this.camera.right)
                this._tilePosition.x = this.camera.right - 32f;
              if ((double) this._tilePosition.y < (double) this.camera.top)
                this._tilePosition.y = this.camera.top + 32f;
              if ((double) this._tilePosition.y > (double) this.camera.bottom)
                this._tilePosition.y = this.camera.bottom - 32f;
              int num3 = 0;
              int num4 = 0;
              if (this._hoverMode == 0 && (this._hoverButton == null || this._hoverButton.focus == null))
              {
                if (Editor._input.Pressed("LEFT"))
                  num4 = -1;
                if (Editor._input.Pressed("RIGHT"))
                  num4 = 1;
                if (Editor._input.Pressed("UP"))
                  num3 = -1;
                if (Editor._input.Pressed("DOWN"))
                  num3 = 1;
              }
              float num5 = this._cellSize * (float) num2 * (float) num4;
              float num6 = this._cellSize * (float) num2 * (float) num3;
              this._tilePosition.x += num5;
              this._tilePosition.y += num6;
              if ((double) this._tilePosition.x < (double) this.camera.left || (double) this._tilePosition.x > (double) this.camera.right)
                this.camera.x += num5;
              if ((double) this._tilePosition.y < (double) this.camera.top || (double) this._tilePosition.y > (double) this.camera.bottom)
                this.camera.y += num6;
              this._tilePosition.x = (float) Math.Round((double) this._tilePosition.x / (double) this._cellSize) * this._cellSize;
              this._tilePosition.y = (float) Math.Round((double) this._tilePosition.y / (double) this._cellSize) * this._cellSize;
              this._tilePositionPrev = this._tilePosition;
            }
            else if (Keyboard.Down(Keys.LeftAlt) || Keyboard.Down(Keys.RightAlt))
            {
              this._tilePosition.x = (float) Math.Round((double) Mouse.positionScreen.x / 1.0) * 1f;
              this._tilePosition.y = (float) Math.Round((double) Mouse.positionScreen.y / 1.0) * 1f;
              flag = true;
            }
            else
            {
              this._tilePosition.x = (float) Math.Round((double) Mouse.positionScreen.x / (double) this._cellSize) * this._cellSize;
              this._tilePosition.y = (float) Math.Round((double) Mouse.positionScreen.y / (double) this._cellSize) * this._cellSize;
            }
            if (this._placementType != null)
            {
              this._tilePosition += this._placementType.editorOffset;
              if (!flag)
              {
                if ((this._placementType.hugWalls & WallHug.Right) != WallHug.None && this.CollisionLine<IPlatform>(this._tilePosition, this._tilePosition + new Vec2(16f, 0.0f), this._placementType) is Thing thing && thing.GetType() != this._placementType.GetType())
                  this._tilePosition.x = thing.left - this._placementType.collisionSize.x - this._placementType.collisionOffset.x;
                if (
                                    (this._placementType.hugWalls & WallHug.Left) != WallHug.None && 
                                    this.CollisionLine<IPlatform>(this._tilePosition, this._tilePosition + new Vec2(-16f, 0.0f), this._placementType) is Thing thing1 && 
                                    thing1.GetType() != this._placementType.GetType())
                  this._tilePosition.x = thing1.right - this._placementType.collisionOffset.x;
                if ((this._placementType.hugWalls & WallHug.Ceiling) != WallHug.None && this.CollisionLine<IPlatform>(this._tilePosition, this._tilePosition + new Vec2(0.0f, -16f), this._placementType) is Thing thing2 && thing2.GetType() != this._placementType.GetType())
                  this._tilePosition.y = thing2.bottom - this._placementType.collisionOffset.y;
                if ((this._placementType.hugWalls & WallHug.Floor) != WallHug.None && this.CollisionLine<IPlatform>(this._tilePosition, this._tilePosition + new Vec2(0.0f, 16f), this._placementType) is Thing thing3 && thing3.GetType() != this._placementType.GetType())
                  this._tilePosition.y = thing3.top - this._placementType.collisionSize.y - this._placementType.collisionOffset.y;
              }
            }
            if (this._move != null)
              this._move.position = new Vec2(this._tilePosition);
            if (this._cursorMode == CursorMode.Selection)
            {
              if (Mouse.left == InputState.Pressed)
              {
                this._dragStart = this._tilePosition;
                this._startedDrag = true;
              }
              if (Mouse.left == InputState.Released)
              {
                Vec2 p1 = this._tilePosition;
                Vec2 p2 = this._dragStart;
                if ((double) p1.x > (double) p2.x || (double) p1.y > (double) p2.y)
                {
                  Vec2 vec2 = p1;
                  p1 = p2;
                  p2 = vec2;
                }
                foreach (Thing thing in Level.CheckRectAll<Thing>(p1, p2))
                {
                  if (!(thing is ContextMenu))
                    this._selection.Add(thing);
                }
                this._dragStart = this._tilePosition;
                this._startedDrag = false;
              }
            }
            else if (this._cursorMode == CursorMode.Normal)
            {
              if ((Mouse.left == InputState.Pressed || Editor._input.Pressed("SELECT")) && (this._placementMode && this._hoverMode == 0) && (this._hoverButton == null || this._hoverButton.focus == null))
              {
                this._dragMode = true;
                Thing hover = this._hover;
                if (hover != null && (!(this._hover is BackgroundTile) || this._placementType != null && this._hover.GetType() == this._placementType.GetType()))
                {
                  if (Keyboard.Down(Keys.LeftControl) || Keyboard.Down(Keys.RightControl))
                    this._move = hover;
                  else
                    this._deleteMode = true;
                }
              }
              if (this._dragMode)
              {
                if (!this._deleteMode && this._placementType != null)
                {
                  Thing thing1 = this._hover;
                  if (thing1 == null && !(this._placementType is BackgroundTile))
                    thing1 = this.CollisionPointPlacementLayer<Thing>(new Vec2(this._tilePosition.x, this._tilePosition.y), layer: layer);
                  if (thing1 is TileButton)
                    thing1 = (Thing) null;
                  if (thing1 == null || this._placementType is WireTileset && thing1 is IWirePeripheral || this._placementType is IWirePeripheral && thing1 is WireTileset)
                  {
                    System.Type type = this._placementType.GetType();
                    Thing thing2 = this._eyeDropperSerialized != null ? Thing.LoadThing(this._eyeDropperSerialized) : Editor.CreateThing(type);
                    thing2.x = this._tilePosition.x;
                    thing2.y = this._tilePosition.y;
                    if (this._placementType is SubBackgroundTile)
                      (thing2.graphic as SpriteMap).frame = ((this._placementType as SubBackgroundTile).graphic as SpriteMap).frame;
                    if (this._placementType is BackgroundTile)
                      (thing2.graphic as SpriteMap).frame = ((this._placementType as BackgroundTile).graphic as SpriteMap).frame;
                    else if (this._placementType is ForegroundTile)
                      (thing2.graphic as SpriteMap).frame = ((this._placementType as ForegroundTile).graphic as SpriteMap).frame;
                    if (this._hover is BackgroundTile)
                      thing2.depth = this._hover.depth + 1;
                    this.RunCommand((Command) new CommandAddObject(thing2));
                    if (thing2 is PathNode)
                      this._pathNodesDirty = true;
                  }
                }
                else
                {
                  Thing hover = this._hover;
                  if (hover != null)
                  {
                    this.RunCommand(new CommandAddObject(hover).Inverse());
                    if (hover is PathNode)
                      this._pathNodesDirty = true;
                    this._hover = (Thing) null;
                  }
                }
              }
              if (Mouse.left == InputState.Released || Editor._input.Released("SELECT"))
              {
                this._dragMode = false;
                this._deleteMode = false;
                if (this._move != null)
                  this._move = (Thing) null;
              }
            }
          }
          this._placingTiles = false;
          if (this._placementType is BackgroundTile)
            this._placingTiles = true;
          if (this._placingTiles && this._placementMenu == null && Editor._input.Pressed("SHOOT"))
          {
            int frame = this._placementType.frame;
            this._placementMenu = (ContextMenu) new ContextBackgroundTile(this._placementType, (IContextListener) null, false);
            this._placementMenu.opened = true;
            SFX.Play("openClick", 0.4f);
            this._placementMenu.x = 16f;
            this._placementMenu.y = 16f;
            this._placementMenu.selectedIndex = frame;
            Level.Add((Thing) this._placementMenu);
          }
          if (this._editMode && this.clicked && (this._placementMenu == null && this._hover != null))
          {
            this._placementMenu = this._hover.GetContextMenu();
            if (this._placementMenu != null)
            {
              this._placementMenu.x = 96f;
              this._placementMenu.y = 32f;
              if (Editor.gamepadMode)
              {
                this._placementMenu.x = 16f;
                this._placementMenu.y = 16f;
              }
              this.AddThing((Thing) this._placementMenu);
              this._placementMenu.opened = true;
              SFX.Play("openClick", 0.4f);
              this.clicked = false;
            }
          }
          if (this._closeMenu)
            this.DoMenuClose();
          base.Update();
        }
      }
    }

    public void DoMenuClose()
    {
      if (this._placementMenu != null)
      {
        if (this._placementMenu != this._objectMenu)
        {
          this.RemoveThing((Thing) this._placementMenu);
        }
        else
        {
          this._placementMenu.visible = false;
          this._placementMenu.active = false;
          this._placementMenu.opened = false;
        }
      }
      this._placementMenu = (ContextMenu) null;
      this._closeMenu = false;
    }

    public override void Draw() => base.Draw();

    public static bool arcadeMachineMode => Level.current is Editor current && current._levelThings.Count == 1 && current._levelThings[0] is ArcadeMachine;

    public override void PostDrawLayer(Layer layer)
    {
      base.PostDrawLayer(layer);
      foreach (Thing thing in this.things)
      {
        if (thing.layer == layer)
          thing.DoEditorRender();
      }
      if (layer == this._procLayer && this._procTarget != null)
        DuckGame.Graphics.Draw((Tex2D) this._procTarget, new Vec2(0.0f, 0.0f), new Rectangle?(), Color.White * 0.5f, 0.0f, Vec2.Zero, new Vec2(1f, 1f), SpriteEffects.None);
      if (layer == this._gridLayer)
      {
        Color col = new Color(38, 38, 38);
        if (Editor.arcadeMachineMode)
        {
          DuckGame.Graphics.DrawRect(this._levelThings[0].position + new Vec2(-17f, -21f), this._levelThings[0].position + new Vec2(18f, 21f), col, new Depth(-0.9f), false);
        }
        else
        {
          float x = (float) (-(double) this._cellSize / 2.0);
          float y = (float) (-(double) this._cellSize / 2.0);
          int num1 = this._gridW;
          int num2 = this._gridH;
          if (this._miniMode)
          {
            num1 = 12;
            num2 = 9;
          }
          int num3 = num1 * 16;
          int num4 = num2 * 16;
          int num5 = (int) ((double) num3 / (double) this._cellSize);
          int num6 = (int) ((double) num4 / (double) this._cellSize);
          for (int index = 0; index < num5 + 1; ++index)
            DuckGame.Graphics.DrawLine(new Vec2(x + (float) index * this._cellSize, y), new Vec2(x + (float) index * this._cellSize, y + (float) num6 * this._cellSize), col, 2f, new Depth(-0.9f));
          for (int index = 0; index < num6 + 1; ++index)
            DuckGame.Graphics.DrawLine(new Vec2(x, y + (float) index * this._cellSize), new Vec2(x + (float) num5 * this._cellSize, y + (float) index * this._cellSize), col, 2f, new Depth(-0.9f));
          if (this._miniMode)
          {
            int num7 = 0;
            if (!this._pathNorth)
            {
              this._sideArrow.color = new Color(80, 80, 80);
            }
            else
            {
              this._sideArrow.color = new Color(100, 200, 100);
              DuckGame.Graphics.DrawLine(new Vec2(x + (float) (num3 / 2), y - 10f), new Vec2(x + (float) (num3 / 2), (float) ((double) y + (double) (num4 / 2) - 8.0)), Color.Lime * 0.06f, 16f);
              ++num7;
            }
            if (!this._pathWest)
            {
              this._sideArrow.color = new Color(80, 80, 80);
            }
            else
            {
              this._sideArrow.color = new Color(100, 200, 100);
              DuckGame.Graphics.DrawLine(new Vec2(x - 10f, y + (float) (num4 / 2)), new Vec2((float) ((double) x + (double) (num3 / 2) - 8.0), y + (float) (num4 / 2)), Color.Lime * 0.06f, 16f);
              ++num7;
            }
            if (!this._pathEast)
            {
              this._sideArrow.color = new Color(80, 80, 80);
            }
            else
            {
              this._sideArrow.color = new Color(100, 200, 100);
              DuckGame.Graphics.DrawLine(new Vec2((float) ((double) x + (double) (num3 / 2) + 8.0), y + (float) (num4 / 2)), new Vec2((float) ((double) x + (double) num3 + 10.0), y + (float) (num4 / 2)), Color.Lime * 0.06f, 16f);
              ++num7;
            }
            if (!this._pathSouth)
            {
              this._sideArrow.color = new Color(80, 80, 80);
            }
            else
            {
              this._sideArrow.color = new Color(100, 200, 100);
              DuckGame.Graphics.DrawLine(new Vec2(x + (float) (num3 / 2), (float) ((double) y + (double) (num4 / 2) + 8.0)), new Vec2(x + (float) (num3 / 2), (float) ((double) y + (double) num4 + 10.0)), Color.Lime * 0.06f, 16f);
              ++num7;
            }
            if (num7 > 0)
              DuckGame.Graphics.DrawLine(new Vec2((float) ((double) x + (double) (num3 / 2) - 8.0), y + (float) (num4 / 2)), new Vec2((float) ((double) x + (double) (num3 / 2) + 8.0), y + (float) (num4 / 2)), Color.Lime * 0.06f, 16f);
          }
        }
      }
      if (layer == Layer.Foreground)
      {
        float num1 = (float) (-(double) this._cellSize / 2.0);
        float num2 = (float) (-(double) this._cellSize / 2.0);
        int num3 = this._gridW;
        int num4 = this._gridH;
        if (this._miniMode)
        {
          num3 = 12;
          num4 = 9;
        }
        int num5 = num3 * 16;
        int num6 = num4 * 16;
        if (this._miniMode)
        {
          Editor._procTilesWide = (int) this._genSize.x;
          Editor._procTilesHigh = (int) this._genSize.y;
          Editor._procXPos = (int) this._genTilePos.x;
          Editor._procYPos = (int) this._genTilePos.y;
          if (Editor._procXPos > Editor._procTilesWide)
            Editor._procXPos = Editor._procTilesWide;
          if (Editor._procYPos > Editor._procTilesHigh)
            Editor._procYPos = Editor._procTilesHigh;
          for (int index1 = 0; index1 < Editor._procTilesWide; ++index1)
          {
            for (int index2 = 0; index2 < Editor._procTilesHigh; ++index2)
            {
              int num7 = index1 - Editor._procXPos;
              int num8 = index2 - Editor._procYPos;
              if (index1 != Editor._procXPos || index2 != Editor._procYPos)
                DuckGame.Graphics.DrawRect(new Vec2(num1 + (float) (num5 * num7), num2 + (float) (num6 * num8)), new Vec2(num1 + (float) (num5 * (num7 + 1)), num2 + (float) (num6 * (num8 + 1))), Color.White * 0.2f, new Depth(1f), false);
            }
          }
        }
        if (this._hoverButton == null)
        {
          if (this._secondaryHover != null && this._placementMode)
            DuckGame.Graphics.DrawRect(this._secondaryHover.topLeft, this._secondaryHover.bottomRight, Color.White * 0.5f, new Depth(1f), false);
          else if (this._hover != null && this._placementMode)
          {
            DuckGame.Graphics.DrawRect(this._hover.topLeft, this._hover.bottomRight, Color.White * 0.5f, new Depth(1f), false);
            this._hover.DrawHoverInfo();
          }
          else if (this._editMode)
          {
            foreach (Thing thing in this.things)
              thing.DrawHoverInfo();
          }
          if (this._hover == null && Editor.gamepadMode)
            DuckGame.Graphics.DrawRect(this._tilePosition - new Vec2(this._cellSize / 2f, this._cellSize / 2f), this._tilePosition + new Vec2(this._cellSize / 2f, this._cellSize / 2f), Color.White * 0.5f, new Depth(1f), false);
          if (this._hover == null && this._placementMode && this._placementType != null)
          {
            this._placementType.depth = new Depth(0.9f);
            this._placementType.x = this._tilePosition.x;
            this._placementType.y = this._tilePosition.y;
            this._placementType.Draw();
          }
        }
        if (this._cursorMode == CursorMode.Selection)
        {
          Vec2 p1 = this._tilePosition - new Vec2(this._cellSize / 2f, this._cellSize / 2f);
          Vec2 p2 = this._tilePosition + new Vec2(this._cellSize / 2f, this._cellSize / 2f);
          if (this._startedDrag)
          {
            if ((double) this._dragStart.x > (double) this._tilePosition.x || (double) this._dragStart.y > (double) this._tilePosition.y)
            {
              p1 = this._dragStart + new Vec2(this._cellSize / 2f, this._cellSize / 2f);
              p2 = this._tilePosition - new Vec2(this._cellSize / 2f, this._cellSize / 2f);
            }
            else
            {
              p1 = this._dragStart - new Vec2(this._cellSize / 2f, this._cellSize / 2f);
              p2 = this._tilePosition + new Vec2(this._cellSize / 2f, this._cellSize / 2f);
            }
          }
          DuckGame.Graphics.DrawDottedRect(p1, p2, Color.White * 0.5f, new Depth(1f), 2f, 4f);
          foreach (Thing thing in Level.current.things)
            thing.material = (Material) null;
          foreach (Thing thing in this._selection)
            thing.material = this._selectionMaterial;
        }
      }
      if (layer != Layer.HUD)
        return;
      if (Editor.tooltip != null)
        DuckGame.Graphics.DrawFancyString(Editor.tooltip, new Vec2(4f, 4f), Color.White, new Depth(0.99f));
      bool flag = Editor._input.lastActiveDevice is Keyboard;
      if (this._hoverMode == 0 && this._hoverButton == null)
      {
        string str1 = "@QUACK@";
        string str2 = "@SELECT@";
        string str3 = "@QUACK@";
        string str4 = "@GRAB@";
        if (flag)
        {
          str1 = "@RIGHTMOUSE@" + str1;
          str2 = "@LEFTMOUSE@" + str2;
          str3 = "@MIDDLEMOUSE@" + str3;
          str4 = "@RIGHTMOUSE@" + str4;
        }
        string text;
        if (this._fileDialog.opened)
          text = "@DPAD@MOVE  " + str2 + "SELECT  " + str1 + "CANCEL  @GRAB@DELETE";
        else if (this._menuOpen)
        {
          text = "@DPAD@MOVE  " + str2 + "SELECT  " + str1 + "CLOSE  @RIGHT@EXPAND";
        }
        else
        {
          text = "@DPAD@MOVE  " + str2 + "PLACE  " + str4 + "MENU";
          if (this._placingTiles)
            text = text + "  @SHOOT@TILES" + "  " + str3 + "COPY";
          else if (this._secondaryHover != null)
            text = text + "  " + str3 + "COPY";
          else if (this._hover != null)
          {
            if (this._hoverMenu != null)
              text += "  @SHOOT@EDIT";
            text = text + "  " + str3 + "COPY";
          }
        }
        if (text != "")
        {
          float width = this._font.GetWidth(text);
          Vec2 vec2 = new Vec2(layer.width - 28f - width, layer.height - 28f);
          this._font.depth = new Depth(0.8f);
          this._font.Draw(text, vec2.x, vec2.y, Color.White, new Depth(0.7f), Editor._input);
          DuckGame.Graphics.DrawRect(vec2 + new Vec2(-2f, -2f), vec2 + new Vec2(width + 2f, 9f), Color.Black * 0.5f, new Depth(0.6f));
        }
        this._font.scale = new Vec2(0.5f, 0.5f);
        float num1 = 0.0f;
        if (Editor.infoText == "" && this._placementType != null)
        {
          Vec2 vec2 = new Vec2(this._placementType.width, this._placementType.height);
          vec2.x += 4f;
          vec2.y += 4f;
          if ((double) vec2.x < 32.0)
            vec2.x = 32f;
          if ((double) vec2.y < 32.0)
            vec2.y = 32f;
          Vec2 p1 = new Vec2(19f, layer.height - 19f - vec2.y);
          string detailsString = this._placementType.GetDetailsString();
          float x = this._font.GetWidth(detailsString) + 8f;
          if (detailsString != "")
            this._font.Draw(detailsString, (float) ((double) p1.x + (double) vec2.x + 4.0), p1.y + 4f, Color.White, new Depth(0.7f));
          else
            x = 0.0f;
          DuckGame.Graphics.DrawRect(p1, p1 + vec2 + new Vec2(x, 0.0f), Color.Black * 0.5f, new Depth(0.6f));
          this._placementType.left = p1.x + (float) ((double) vec2.x / 2.0 - (double) this._placementType.w / 2.0);
          this._placementType.top = p1.y + (float) ((double) vec2.y / 2.0 - (double) this._placementType.h / 2.0);
          this._placementType.depth = new Depth(0.7f);
          this._placementType.Draw();
          this._font.Draw("Placing", p1.x, p1.y - 6f, Color.White, new Depth(0.7f));
          num1 = vec2.y;
        }
        else
        {
          int num2 = Editor.infoText != "" ? 1 : 0;
        }
        Thing thing = this._hover;
        if (this._secondaryHover != null)
          thing = this._secondaryHover;
        if (Editor.infoText == "" && thing != null && this._hoverMode == 0)
        {
          Vec2 vec2 = new Vec2(thing.width, thing.height);
          vec2.x += 4f;
          vec2.y += 4f;
          if ((double) vec2.x < 32.0)
            vec2.x = 32f;
          if ((double) vec2.y < 32.0)
            vec2.y = 32f;
          Vec2 p1 = new Vec2(19f, (float) ((double) layer.height - 19.0 - (double) vec2.y - ((double) num1 + 10.0)));
          string detailsString = thing.GetDetailsString();
          float x = this._font.GetWidth(detailsString) + 8f;
          if (detailsString != "")
            this._font.Draw(detailsString, (float) ((double) p1.x + (double) vec2.x + 4.0), p1.y + 4f, Color.White, new Depth(0.7f));
          else
            x = 0.0f;
          DuckGame.Graphics.DrawRect(p1, p1 + vec2 + new Vec2(x, 0.0f), Color.Black * 0.5f, new Depth(0.6f));
          Vec2 position = thing.position;
          Depth depth = thing.depth;
          thing.left = p1.x + (float) ((double) vec2.x / 2.0 - (double) thing.w / 2.0);
          thing.top = p1.y + (float) ((double) vec2.y / 2.0 - (double) thing.h / 2.0);
          thing.depth = new Depth(0.7f);
          thing.Draw();
          thing.position = position;
          thing.depth = depth;
          this._font.Draw("Hovering", p1.x, p1.y - 6f, Color.White);
        }
      }
      else if (this._hoverButton != null)
      {
        string hoverText = this._hoverButton.hoverText;
        if (hoverText != null)
        {
          float width = this._font.GetWidth(hoverText);
          Vec2 vec2 = new Vec2(layer.width - 28f - width, layer.height - 28f);
          this._font.depth = new Depth(0.8f);
          this._font.Draw(hoverText, vec2.x, vec2.y, Color.White, new Depth(0.8f));
          DuckGame.Graphics.DrawRect(vec2 + new Vec2(-2f, -2f), vec2 + new Vec2(width + 2f, 9f), Color.Black * 0.5f, new Depth(0.6f));
        }
      }
      this._font.scale = new Vec2(1f, 1f);
      if (!Mouse.available || Editor.gamepadMode)
        return;
      this._cursor.depth = new Depth(1f);
      this._cursor.scale = new Vec2(1f, 1f);
      this._cursor.position = Mouse.position;
      if (this._cursorMode == CursorMode.Normal)
        this._cursor.frame = 0;
      else if (this._cursorMode == CursorMode.Selection)
        this._cursor.frame = 2;
      if (Editor.hoverTextBox)
      {
        this._cursor.frame = 5;
        this._cursor.position.y -= 4f;
        this._cursor.scale = new Vec2(0.5f, 1f);
      }
      this._cursor.Draw();
    }

    public override void StartDrawing()
    {
      if (this._procTarget == null)
        this._procTarget = new RenderTarget2D(DuckGame.Graphics.width, DuckGame.Graphics.height);
      if (this._procContext == null)
        return;
      this._procContext.Draw(this._procTarget, Level.current.camera, this._procDrawOffset);
    }

    public void CloseMenu() => this._closeMenu = true;

    public void DoSave(string saveName)
    {
      this._saveName = saveName;
      if (!this._saveName.EndsWith(".lev"))
        this._saveName += ".lev";
      this.Save();
    }

    private void onLoad(object sender, CancelEventArgs e)
    {
      if (e.Cancel)
        return;
      string fileName = this._loadForm.FileName;
      this._saveName = fileName;
      IEnumerable<XElement> source = XDocument.Load(fileName).Element((XName) "Level").Elements((XName) "Objects");
      if (source == null)
        return;
      this.ClearEverything();
      foreach (XElement element in source.Elements<XElement>((XName) "Object"))
        this.AddObject(Thing.LegacyLoadThing(element));
    }

    public void LoadLevel(string load)
    {
      load = load.Replace('\\', '/');
      while (load.StartsWith("/"))
        load = load.Substring(1);
      LevelData levelData = DuckFile.LoadLevel(load);
      this._saveName = load;
      this.ClearEverything();
      this._guid = levelData.metaData.guid;
      Editor.workshopID = levelData.metaData.workshopID;
      Editor.onlineMode = levelData.metaData.onlineMode;
      if (levelData.customData != null)
      {
        if (levelData.customData.customTileset01Data != null)
          Custom.ApplyCustomData(levelData.customData.customTileset01Data.GetTileData(), 0, CustomType.Block);
        if (levelData.customData.customTileset02Data != null)
          Custom.ApplyCustomData(levelData.customData.customTileset02Data.GetTileData(), 1, CustomType.Block);
        if (levelData.customData.customTileset03Data != null)
          Custom.ApplyCustomData(levelData.customData.customTileset03Data.GetTileData(), 2, CustomType.Block);
        if (levelData.customData.customBackground01Data != null)
          Custom.ApplyCustomData(levelData.customData.customBackground01Data.GetTileData(), 0, CustomType.Background);
        if (levelData.customData.customBackground02Data != null)
          Custom.ApplyCustomData(levelData.customData.customBackground02Data.GetTileData(), 1, CustomType.Background);
        if (levelData.customData.customBackground03Data != null)
          Custom.ApplyCustomData(levelData.customData.customBackground03Data.GetTileData(), 2, CustomType.Background);
        if (levelData.customData.customPlatform01Data != null)
          Custom.ApplyCustomData(levelData.customData.customPlatform01Data.GetTileData(), 0, CustomType.Platform);
        if (levelData.customData.customPlatform02Data != null)
          Custom.ApplyCustomData(levelData.customData.customPlatform02Data.GetTileData(), 1, CustomType.Platform);
        if (levelData.customData.customPlatform03Data != null)
          Custom.ApplyCustomData(levelData.customData.customPlatform03Data.GetTileData(), 2, CustomType.Platform);
        if (levelData.customData.customParallaxData != null)
          Custom.ApplyCustomData(levelData.customData.customParallaxData.GetTileData(), 0, CustomType.Parallax);
      }
      Editor.previewCapture = Editor.LoadPreview(levelData.previewData.preview);
      this._pathNorth = false;
      this._pathSouth = false;
      this._pathEast = false;
      this._pathWest = false;
      this._miniMode = false;
      int sideMask = levelData.proceduralData.sideMask;
      if ((sideMask & 1) != 0)
        this._pathNorth = true;
      if ((sideMask & 2) != 0)
        this._pathEast = true;
      if ((sideMask & 4) != 0)
        this._pathSouth = true;
      if ((sideMask & 8) != 0)
        this._pathWest = true;
      if (sideMask != 0)
        this._miniMode = true;
      if (levelData.workshopData != null)
      {
        Editor.workshopName = levelData.workshopData.name;
        Editor.workshopAuthor = levelData.workshopData.author;
        Editor.workshopDescription = levelData.workshopData.description;
        Editor.workshopVisibility = (int) levelData.workshopData.visibility;
        Editor.workshopTags = levelData.workshopData.tags.ToList<string>();
      }
      this._chance = levelData.proceduralData.chance;
      this._maxPerLevel = levelData.proceduralData.maxPerLevel;
      this._enableSingle = levelData.proceduralData.enableSingle;
      this._enableMulti = levelData.proceduralData.enableMulti;
      this._canMirror = levelData.proceduralData.canMirror;
      this._isMirrored = levelData.proceduralData.isMirrored;
      this._loadingLevel = true;
      foreach (BinaryClassChunk node in levelData.objects.objects)
        this.AddObject(Thing.LoadThing(node));
      this._loadingLevel = false;
      this._pathNodesDirty = true;
      if (this._looseClear)
        return;
      this.CenterView();
    }

    public void LegacyLoadLevel(string load)
    {
      load = load.Replace('\\', '/');
      while (load.StartsWith("/"))
        load = load.Substring(1);
      XDocument doc = this._additionalSaveDirectory != null ? XDocument.Load(load) : DuckFile.LoadXDocument(load);
      this._saveName = load;
      this.LegacyLoadLevelParts(doc);
    }

    public void LegacyLoadLevelParts(XDocument doc)
    {
      this.hadGUID = false;
      this.ClearEverything();
      XElement e = doc.Element((XName) "Level");
      XElement xelement1 = e.Element((XName) "ID");
      if (xelement1 != null)
      {
        this._guid = xelement1.Value;
        this.hadGUID = true;
      }
      XElement xelement2 = e.Element((XName) "ONLINE");
      Editor.onlineMode = xelement2 != null && Convert.ToBoolean(xelement2.Value);
      Editor.previewCapture = Editor.LegacyLoadPreview(e);
      this._pathNorth = false;
      this._pathSouth = false;
      this._pathEast = false;
      this._pathWest = false;
      this._miniMode = false;
      XElement xelement3 = e.Element((XName) "PathMask");
      if (xelement3 != null)
      {
        int int32 = Convert.ToInt32(xelement3.Value);
        if ((int32 & 1) != 0)
          this._pathNorth = true;
        if ((int32 & 2) != 0)
          this._pathEast = true;
        if ((int32 & 4) != 0)
          this._pathSouth = true;
        if ((int32 & 8) != 0)
          this._pathWest = true;
        if (int32 != 0)
          this._miniMode = true;
      }
      XElement xelement4 = e.Element((XName) "workshopID");
      if (xelement4 != null)
        Editor.workshopID = Convert.ToUInt64(xelement4.Value);
      XElement xelement5 = e.Element((XName) "workshopName");
      if (xelement5 != null)
        Editor.workshopName = xelement5.Value;
      XElement xelement6 = e.Element((XName) "workshopDescription");
      if (xelement6 != null)
        Editor.workshopDescription = xelement6.Value;
      XElement xelement7 = e.Element((XName) "workshopVisibility");
      if (xelement7 != null)
        Editor.workshopVisibility = Convert.ToInt32(xelement7.Value);
      XElement xelement8 = e.Element((XName) "workshopTags");
      if (xelement8 != null)
      {
        string[] strArray = xelement8.Value.Split('|');
        Editor.workshopTags = new List<string>();
        if (((IEnumerable<string>) strArray).Count<string>() != 0 && strArray[0] != "")
          Editor.workshopTags = ((IEnumerable<string>) strArray).ToList<string>();
      }
      XElement xelement9 = e.Element((XName) "Chance");
      if (xelement9 != null)
        this._chance = Convert.ToSingle(xelement9.Value);
      XElement xelement10 = e.Element((XName) "MaxPerLev");
      if (xelement10 != null)
        this._maxPerLevel = Convert.ToInt32(xelement10.Value);
      XElement xelement11 = e.Element((XName) "Single");
      if (xelement11 != null)
        this._enableSingle = Convert.ToBoolean(xelement11.Value);
      XElement xelement12 = e.Element((XName) "Multi");
      if (xelement12 != null)
        this._enableMulti = Convert.ToBoolean(xelement12.Value);
      XElement xelement13 = e.Element((XName) "CanMirror");
      if (xelement13 != null)
        this._canMirror = Convert.ToBoolean(xelement13.Value);
      XElement xelement14 = e.Element((XName) "IsMirrored");
      if (xelement14 != null)
        this._isMirrored = Convert.ToBoolean(xelement14.Value);
      this._loadingLevel = true;
      IEnumerable<XElement> source = e.Elements((XName) "Objects");
      if (source != null)
      {
        foreach (XElement element in source.Elements<XElement>((XName) "Object"))
          this.AddObject(Thing.LegacyLoadThing(element));
      }
      this._loadingLevel = false;
      this._pathNodesDirty = true;
      if (this._looseClear)
        return;
      this.CenterView();
    }

    private void CenterView()
    {
      this.camera.width = (float) (this._gridW * 16);
      this.camera.height = this.camera.width * DuckGame.Graphics.aspect;
      this.camera.centerX = (float) ((double) this.camera.width / 2.0 - 8.0);
      this.camera.centerY = (float) ((double) this.camera.height / 2.0 - 8.0);
      float width = this.camera.width;
      float height = this.camera.height;
      this.camera.width *= 1.2f;
      this.camera.height *= 1.2f;
      this.camera.centerX -= (float) (((double) this.camera.width - (double) width) / 2.0);
      this.camera.centerY -= (float) (((double) this.camera.height - (double) height) / 2.0);
    }

    public static Texture2D LoadPreview(string s)
    {
      try
      {
        return s != null ? Texture2D.FromStream(DuckGame.Graphics.device, (Stream) new MemoryStream(Convert.FromBase64String(s))) : (Texture2D) null;
      }
      catch
      {
        return (Texture2D) null;
      }
    }

    public static Texture2D LegacyLoadPreview(XElement e)
    {
      try
      {
        XElement xelement = e.Element((XName) "Preview");
        return xelement != null ? Texture2D.FromStream(DuckGame.Graphics.device, (Stream) new MemoryStream(Convert.FromBase64String(xelement.Value))) : (Texture2D) null;
      }
      catch
      {
        return (Texture2D) null;
      }
    }

    public static string LegacyLoadPreviewString(XElement e)
    {
      try
      {
        return e.Element((XName) "Preview")?.Value;
      }
      catch
      {
        return (string) null;
      }
    }

    public static string ScriptToString(byte[] scriptData)
    {
      try
      {
        return Convert.ToBase64String(scriptData);
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    public static byte[] StringToScript(string script)
    {
      try
      {
        return Convert.FromBase64String(script);
      }
      catch (Exception ex)
      {
        return (byte[]) null;
      }
    }

    public static string TextureToString(Texture2D tex)
    {
      try
      {
        MemoryStream memoryStream = new MemoryStream();
        tex.SaveAsPng((Stream) memoryStream, tex.Width, tex.Height);
        memoryStream.Flush();
        return Convert.ToBase64String(memoryStream.ToArray());
      }
      catch (Exception ex)
      {
        return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAPUExURQAAAGwXbeBu4P///8AgLYwkid8AAAC9SURBVDhPY2RgYPgPxGQDsAE54rkQHhCUhBdDWRDQs7IXyoIAZHmFSQoMTFA2BpCfKA/Gk19MAmNcAKsBII0HFfVQMC5DwF54kPcAwgMCmGZswP7+JYZciTwoj4FhysvJuL0AAiANIIwPYBgAsgGmEdk2XACrC0AaidEMAnijETk8YC4iKRrRNWMDeAORGIDTgIf5D4kKTIx0AEu6oISD7AWQgSCAnLQJpgNiAE4DQM6GeQFmOzZAYXZmYAAAEzJYPzQv17kAAAAASUVORK5CYII=";
      }
    }

    public static Texture2D StringToTexture(string tex)
    {
      try
      {
        return Texture2D.FromStream(DuckGame.Graphics.device, (Stream) new MemoryStream(Convert.FromBase64String(tex)));
      }
      catch
      {
        return (Texture2D) null;
      }
    }

    public LevelData CreateSaveData()
    {
      Level currentLevel = Level.core.currentLevel;
      Level.core.currentLevel = (Level) this;
      LevelData levelData = new LevelData();
      levelData.metaData.type = this.GetLevelType();
      levelData.metaData.size = this.GetLevelSize();
      levelData.metaData.online = this.LevelIsOnlineCapable();
      levelData.metaData.guid = this._guid;
      levelData.metaData.workshopID = Editor.workshopID;
      levelData.metaData.deathmatchReady = Editor.workshopLevelDeathmatchReady;
      levelData.metaData.onlineMode = Editor.onlineMode;
      CustomTileData data1 = Custom.GetData(0, CustomType.Block);
      if (data1 != null && data1.path != null && data1.texture != null)
        data1.ApplyToChunk(levelData.customData.customTileset01Data);
      CustomTileData data2 = Custom.GetData(1, CustomType.Block);
      if (data2 != null && data2.path != null && data2.texture != null)
        data2.ApplyToChunk(levelData.customData.customTileset02Data);
      CustomTileData data3 = Custom.GetData(2, CustomType.Block);
      if (data3 != null && data3.path != null && data3.texture != null)
        data3.ApplyToChunk(levelData.customData.customTileset03Data);
      CustomTileData data4 = Custom.GetData(0, CustomType.Background);
      if (data4 != null && data4.path != null && data4.texture != null)
        data4.ApplyToChunk(levelData.customData.customBackground01Data);
      CustomTileData data5 = Custom.GetData(1, CustomType.Background);
      if (data5 != null && data5.path != null && data5.texture != null)
        data5.ApplyToChunk(levelData.customData.customBackground02Data);
      CustomTileData data6 = Custom.GetData(2, CustomType.Background);
      if (data6 != null && data6.path != null && data6.texture != null)
        data6.ApplyToChunk(levelData.customData.customBackground03Data);
      CustomTileData data7 = Custom.GetData(0, CustomType.Platform);
      if (data7 != null && data7.path != null && data7.texture != null)
        data7.ApplyToChunk(levelData.customData.customPlatform01Data);
      CustomTileData data8 = Custom.GetData(1, CustomType.Platform);
      if (data8 != null && data8.path != null && data8.texture != null)
        data8.ApplyToChunk(levelData.customData.customPlatform02Data);
      CustomTileData data9 = Custom.GetData(2, CustomType.Platform);
      if (data9 != null && data9.path != null && data9.texture != null)
        data9.ApplyToChunk(levelData.customData.customPlatform03Data);
      CustomTileData data10 = Custom.GetData(0, CustomType.Parallax);
      if (data10 != null && data10.path != null && data10.texture != null)
        data10.ApplyToChunk(levelData.customData.customParallaxData);
      levelData.workshopData.author = Editor.workshopAuthor;
      levelData.workshopData.name = Editor.workshopName;
      levelData.workshopData.description = Editor.workshopDescription;
      levelData.workshopData.visibility = (RemoteStoragePublishedFileVisibility) Editor.workshopVisibility;
      levelData.workshopData.tags = new List<string>((IEnumerable<string>) Editor.workshopTags);
      if (this._things.Count > 0)
      {
        HashSet<Mod> modSet = new HashSet<Mod>();
        foreach (Thing thing in this._things)
        {
          modSet.Add(ModLoader.GetModFromType(thing.GetType()));
          if (thing is IContainThings)
          {
            IContainThings containThings = (IContainThings) thing;
            if (containThings.contains != null)
            {
              foreach (System.Type contain in containThings.contains)
                modSet.Add(ModLoader.GetModFromType(contain));
            }
          }
          else if (thing is IContainAThing)
          {
            IContainAThing containAthing = (IContainAThing) thing;
            if (containAthing.contains != (System.Type) null)
              modSet.Add(ModLoader.GetModFromType(containAthing.contains));
          }
        }
        modSet.RemoveWhere((Predicate<Mod>) (a =>
        {
          switch (a)
          {
            case null:
            case CoreMod _:
              return true;
            default:
              return a is DisabledMod;
          }
        }));
        if (modSet.Count != 0)
        {
          foreach (Mod mod in modSet)
          {
            if (mod.configuration.isWorkshop)
              levelData.modData.workshopIDs.Add(mod.configuration.workshopID);
            else
              levelData.modData.hasLocalMods = true;
          }
        }
      }
      if (Editor.previewCapture != null)
        levelData.previewData.preview = Editor.TextureToString(Editor.previewCapture);
      string str1 = "";
      string str2 = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      if (this._levelThings.Count > 0)
      {
        MultiMap<System.Type, Thing> multiMap = new MultiMap<System.Type, Thing>();
        foreach (Thing levelThing in this._levelThings)
        {
          if (levelThing.editorCanModify && !levelThing.processedByEditor)
          {
            if (this._miniMode)
            {
              switch (levelThing)
              {
                case Key _:
                  ++num6;
                  break;
                case Door _ when (levelThing as Door).locked:
                  ++num5;
                  break;
                case Equipment _:
                  if (levelThing is ChestPlate || levelThing is Helmet || levelThing is KnightHelmet)
                  {
                    ++num1;
                    break;
                  }
                  ++num2;
                  break;
                case Gun _:
                  if (str1 != "")
                    str1 += "|";
                  str1 += ModLoader.SmallTypeName(levelThing.GetType());
                  break;
                case ItemSpawner _:
                  ItemSpawner itemSpawner = levelThing as ItemSpawner;
                  if (typeof (Gun).IsAssignableFrom(itemSpawner.contains) && (double) itemSpawner.likelyhoodToExist == 1.0 && !itemSpawner.randomSpawn)
                  {
                    if (itemSpawner.spawnNum < 1 && (double) itemSpawner.spawnTime < 8.0 && itemSpawner.isAccessible)
                    {
                      if (str2 != "")
                        str2 += "|";
                      str2 += ModLoader.SmallTypeName(itemSpawner.contains);
                    }
                    if (str1 != "")
                      str1 += "|";
                    str1 += ModLoader.SmallTypeName(itemSpawner.contains);
                    break;
                  }
                  break;
                default:
                  if (levelThing.GetType() == typeof (ItemBox))
                  {
                    ItemBox itemBox = levelThing as ItemBox;
                    if (typeof (Gun).IsAssignableFrom(itemBox.contains) && (double) itemBox.likelyhoodToExist == 1.0 && itemBox.isAccessible)
                    {
                      if (str2 != "")
                        str2 += "|";
                      str2 += ModLoader.SmallTypeName(itemBox.contains);
                      if (str1 != "")
                        str1 += "|";
                      str1 += ModLoader.SmallTypeName(itemBox.contains);
                      break;
                    }
                    break;
                  }
                  switch (levelThing)
                  {
                    case SpawnPoint _:
                      ++num3;
                      break;
                    // TODO figure out this case
                    //case TeamSpawn _:
                    //  ++num4;
                    //  break;
                  }
                  break;
              }
            }
            levelThing.processedByEditor = true;
            if (levelThing.canBeGrouped)
              multiMap.Add(levelThing.GetType(), levelThing);
            else
              levelData.objects.Add(levelThing.Serialize());
          }
        }
        foreach (KeyValuePair<System.Type, List<Thing>> keyValuePair in (MultiMap<System.Type, Thing, List<Thing>>) multiMap)
          levelData.objects.Add(new ThingContainer(keyValuePair.Value, keyValuePair.Key)
          {
            quickSerialize = this.minimalConversionLoad
          }.Serialize());
      }
      if (this._miniMode)
      {
        int num7 = 0;
        if (this._pathNorth)
          num7 |= 1;
        if (this._pathEast)
          num7 |= 2;
        if (this._pathSouth)
          num7 |= 4;
        if (this._pathWest)
          num7 |= 8;
        levelData.proceduralData.sideMask = num7;
        levelData.proceduralData.chance = this._chance;
        levelData.proceduralData.maxPerLevel = this._maxPerLevel;
        levelData.proceduralData.enableSingle = this._enableSingle;
        levelData.proceduralData.enableMulti = this._enableMulti;
        levelData.proceduralData.canMirror = this._canMirror;
        levelData.proceduralData.isMirrored = this._isMirrored;
        levelData.proceduralData.weaponConfig = str1;
        levelData.proceduralData.spawnerConfig = str2;
        levelData.proceduralData.numArmor = num1;
        levelData.proceduralData.numEquipment = num2;
        levelData.proceduralData.numSpawns = num3;
        levelData.proceduralData.numTeamSpawns = num4;
        levelData.proceduralData.numLockedDoors = num5;
        levelData.proceduralData.numKeys = num6;
      }
      Level.core.currentLevel = currentLevel;
      return levelData;
    }

    public void Save()
    {
      if (this._saveName == "")
      {
        this.SaveAs();
      }
      else
      {
        Editor.saving = true;
        LevelData saveData = this.CreateSaveData();
        saveData.SetPath(this._saveName);
        DuckFile.SaveChunk((BinaryClassChunk) saveData, this._saveName);
        Content.MapLevel(saveData.metaData.guid, saveData, LevelLocation.Custom);
        if (this._additionalSaveDirectory != null && this._saveName.LastIndexOf("assets/levels/") != -1)
        {
          string str = this._saveName.Substring(this._saveName.LastIndexOf("assets/levels/") + "assets/levels/".Length);
          File.Copy(this._saveName, Directory.GetCurrentDirectory() + "/Content/levels/" + str, true);
          File.SetAttributes(this._saveName, FileAttributes.Normal);
        }
        if (this._miniMode && !this._doingResave)
          LevelGenerator.ReInitialize();
        foreach (Thing levelThing in this._levelThings)
          levelThing.processedByEditor = false;
        Editor.saving = false;
      }
    }

    public XDocument LegacyCreateSaveData()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement((XName) "Level");
      XElement xelement2 = new XElement((XName) "Metadata");
      xelement2.Add((object) new XElement((XName) "Type", (object) (int) this.GetLevelType()));
      xelement2.Add((object) new XElement((XName) "Size", (object) (int) this.GetLevelSize()));
      xelement2.Add((object) new XElement((XName) "Online", (object) this.LevelIsOnlineCapable()));
      xelement1.Add((object) xelement2);
      xelement1.Add((object) new XElement((XName) "ID", (object) this._guid));
      xelement1.Add((object) new XElement((XName) "ONLINE", (object) Editor.onlineMode.ToString()));
      if (this._miniMode)
      {
        int num = 0;
        if (this._pathNorth)
          num |= 1;
        if (this._pathEast)
          num |= 2;
        if (this._pathSouth)
          num |= 4;
        if (this._pathWest)
          num |= 8;
        xelement1.Add((object) new XElement((XName) "PathMask", (object) num));
        xelement1.Add((object) new XElement((XName) "Chance", (object) this._chance));
        xelement1.Add((object) new XElement((XName) "MaxPerLev", (object) this._maxPerLevel));
        xelement1.Add((object) new XElement((XName) "Single", (object) this._enableSingle));
        xelement1.Add((object) new XElement((XName) "Multi", (object) this._enableMulti));
        xelement1.Add((object) new XElement((XName) "CanMirror", (object) this._canMirror));
        xelement1.Add((object) new XElement((XName) "IsMirrored", (object) this._isMirrored));
      }
      xelement1.Add((object) new XElement((XName) "workshopID", (object) Editor.workshopID));
      xelement1.Add((object) new XElement((XName) "workshopName", (object) Editor.workshopName));
      xelement1.Add((object) new XElement((XName) "workshopDescription", (object) Editor.workshopDescription));
      xelement1.Add((object) new XElement((XName) "workshopVisibility", (object) Editor.workshopVisibility));
      string str1 = "";
      for (int index = 0; index < Editor.workshopTags.Count; ++index)
      {
        str1 += Editor.workshopTags[index];
        if (index != Editor.workshopTags.Count - 1)
          str1 += "|";
      }
      xelement1.Add((object) new XElement((XName) "workshopTags", (object) str1));
      if (Editor.previewCapture != null)
        xelement1.Add((object) new XElement((XName) "Preview", (object) Editor.TextureToString(Editor.previewCapture)));
      string str2 = "";
      string str3 = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      if (this._levelThings.Count > 0)
      {
        XElement xelement3 = new XElement((XName) "Objects");
        MultiMap<System.Type, Thing> multiMap = new MultiMap<System.Type, Thing>();
        foreach (Thing levelThing in this._levelThings)
        {
          if (levelThing.editorCanModify && !levelThing.processedByEditor)
          {
            if (this._miniMode)
            {
              switch (levelThing)
              {
                case Key _:
                  ++num6;
                  break;
                case Door _ when (levelThing as Door).locked:
                  ++num5;
                  break;
                case Equipment _:
                  if (levelThing is ChestPlate || levelThing is Helmet || levelThing is KnightHelmet)
                  {
                    ++num1;
                    break;
                  }
                  ++num2;
                  break;
                case Gun _:
                  if (str2 != "")
                    str2 += "|";
                  str2 += levelThing.GetType().AssemblyQualifiedName;
                  break;
                case ItemSpawner _:
                  ItemSpawner itemSpawner = levelThing as ItemSpawner;
                  if (typeof (Gun).IsAssignableFrom(itemSpawner.contains) && (double) itemSpawner.likelyhoodToExist == 1.0 && !itemSpawner.randomSpawn)
                  {
                    if (itemSpawner.spawnNum < 1 && (double) itemSpawner.spawnTime < 8.0 && itemSpawner.isAccessible)
                    {
                      if (str3 != "")
                        str3 += "|";
                      str3 += itemSpawner.contains.AssemblyQualifiedName;
                    }
                    if (str2 != "")
                      str2 += "|";
                    str2 += itemSpawner.contains.AssemblyQualifiedName;
                    break;
                  }
                  break;
                default:
                  if (levelThing.GetType() == typeof (ItemBox))
                  {
                    ItemBox itemBox = levelThing as ItemBox;
                    if (typeof (Gun).IsAssignableFrom(itemBox.contains) && (double) itemBox.likelyhoodToExist == 1.0 && itemBox.isAccessible)
                    {
                      if (str3 != "")
                        str3 += "|";
                      str3 += itemBox.contains.AssemblyQualifiedName;
                      if (str2 != "")
                        str2 += "|";
                      str2 += itemBox.contains.AssemblyQualifiedName;
                      break;
                    }
                    break;
                  }
                  switch (levelThing)
                  {
                    case SpawnPoint _:
                      ++num3;
                      break;
                                            // TODO
                    //case TeamSpawn _:
                    //  ++num4;
                    //  break;
                  }
                  break;
              }
            }
            levelThing.processedByEditor = true;
            if (levelThing.canBeGrouped)
              multiMap.Add(levelThing.GetType(), levelThing);
            else
              xelement3.Add((object) levelThing.LegacySerialize());
          }
        }
        foreach (KeyValuePair<System.Type, List<Thing>> keyValuePair in (MultiMap<System.Type, Thing, List<Thing>>) multiMap)
        {
          ThingContainer thingContainer = new ThingContainer(keyValuePair.Value, keyValuePair.Key);
          xelement3.Add((object) thingContainer.LegacySerialize());
        }
        xelement1.Add((object) xelement3);
      }
      if (this._miniMode)
      {
        xelement1.Add((object) new XElement((XName) "WeaponConfigString", (object) str2));
        xelement1.Add((object) new XElement((XName) "SpawnerConfigString", (object) str3));
        string str4 = "" + num1.ToString() + "|" + num2.ToString() + "|" + num3.ToString() + "|" + num4.ToString() + "|" + num5.ToString() + "|" + num6.ToString();
        xelement1.Add((object) new XElement((XName) "ItemConfigString", (object) str4));
      }
      xdocument.Add((object) xelement1);
      return xdocument;
    }

    public void LegacySave()
    {
      if (this._saveName == "")
      {
        this.SaveAs();
      }
      else
      {
        Editor.saving = true;
        XDocument saveData = this.LegacyCreateSaveData();
        if (this._additionalSaveDirectory == null)
        {
          DuckFile.SaveXDocument(saveData, this._saveName);
        }
        else
        {
          saveData.Save(this._saveName);
          if (this._saveName.LastIndexOf("assets/levels/") != -1)
          {
            string str = this._saveName.Substring(this._saveName.LastIndexOf("assets/levels/") + "assets/levels/".Length);
            File.Copy(this._saveName, Directory.GetCurrentDirectory() + "/Content/levels/" + str, true);
            File.SetAttributes(this._saveName, FileAttributes.Normal);
          }
        }
        if (this._miniMode)
          LevelGenerator.ReInitialize();
        foreach (Thing levelThing in this._levelThings)
          levelThing.processedByEditor = false;
        Editor.saving = false;
      }
    }

    public static void Delete(string file)
    {
      file = file.Replace('\\', '/');
      while (file.StartsWith("/"))
        file = file.Substring(1);
      File.SetAttributes(file, FileAttributes.Normal);
      File.Delete(file);
    }

    public void SaveAs()
    {
      this._fileDialog.Open(Editor._initialDirectory, Editor._initialDirectory, true);
      this.DoMenuClose();
      this._closeMenu = false;
    }

    public void Load()
    {
      this._fileDialog.Open(Editor._initialDirectory, Editor._initialDirectory, false);
      this.DoMenuClose();
      this._closeMenu = false;
    }

    public string SaveTempVersion()
    {
      string saveName = this._saveName;
      string str = Directory.GetCurrentDirectory() + "\\Content\\_tempPlayLevel.lev";
      this._saveName = str;
      this.Save();
      this._saveName = saveName;
      return str;
    }

    public void Play()
    {
      string name;
      if (this._miniMode && this._procContext != null)
      {
        LevelGenerator.ReInitialize();
        this._centerTile = LevelGenerator.LoadInTile(this.SaveTempVersion());
        name = "RANDOM";
      }
      else
        name = this.SaveTempVersion();
      this.CloseMenu();
      this.RunTestLevel(name);
    }

    public virtual void RunTestLevel(string name)
    {
      Level.current = (Level) new TestArea(this, name, this._procSeed, this._centerTile);
      Level.current.AddThing((Thing) new EditorTestLevel(this));
    }
  }
}
