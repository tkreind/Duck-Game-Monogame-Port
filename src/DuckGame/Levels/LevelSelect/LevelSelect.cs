// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelSelect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace DuckGame
{
  public class LevelSelect : Level
  {
    private string _currentDirectory;
    private string _rootDirectory;
    private List<LSItem> _items = new List<LSItem>();
    private int _topIndex;
    private int _maxItems = 15;
    private int _selectedItem;
    private LSItem _selectedLevel;
    private float _leftPos = 12f;
    private float _topPos = 21f;
    private RenderTarget2D _preview;
    private Sprite _previewSprite;
    private LSItem _previewItem;
    private LSItem _lastSelection;
    private BitmapFont _font;
    private Level _returnLevel;
    private bool _exiting;
    private TextEntryDialog _dialog;
    private UIMenu _returnMenu;
    private SpriteMap _iconSheet;
    private bool _onlineMode;
    private List<IFilterLSItems> _filters = new List<IFilterLSItems>();
    public bool isInitialized;
    public static bool _skipCompanionOpening;
    public bool showPlaylistOption;
    public bool isClosed;
    private UIMenu _confirmMenu;
    private UIMenu _notOnlineMenu;
    private MenuBoolean _deleteFile = new MenuBoolean();

    public List<IFilterLSItems> filters => this._filters;

    public LevelSelect(string root = "", Level returnLevel = null, UIMenu returnMenu = null, bool onlineMode = false)
    {
      this._centeredView = true;
      if (root == "")
        root = DuckFile.levelDirectory;
      root = root.TrimEnd('/');
      this._rootDirectory = root;
      this._font = new BitmapFont("biosFont", 8);
      this._returnLevel = returnLevel;
      this._dialog = new TextEntryDialog();
      this._returnMenu = returnMenu;
      this._iconSheet = new SpriteMap("iconSheet", 16, 16);
      this._onlineMode = onlineMode;
      this._filters.Add((IFilterLSItems) new LSFilterLevelType(LevelType.Deathmatch, true));
      this._filters.Add((IFilterLSItems) new LSFilterMods(true));
    }

    public void SetCurrentFolder(string folder)
    {
      this._currentDirectory = folder;
      HUD.CloseCorner(HUDCorner.TopRight);
      if (this._currentDirectory == this._rootDirectory)
      {
        this._selectedItem = 0;
        HUD.AddCornerControl(HUDCorner.TopRight, "@START@Done");
        HUD.AddCornerControl(HUDCorner.TopRight, "@GRAB@Delete");
      }
      else
      {
        this._selectedItem = 1;
        HUD.AddCornerControl(HUDCorner.TopRight, "@START@Done @QUACK@Back");
        HUD.AddCornerControl(HUDCorner.TopRight, "@GRAB@Delete");
      }
      this._topIndex = 0;
      this._items.Clear();
      if (this._currentDirectory != this._rootDirectory)
        this.AddItem(new LSItem(0.0f, 0.0f, this, "../"));
      else if (Steam.GetNumWorkshopItems() > 0)
        this.AddItem(new LSItem(0.0f, 0.0f, this, "@WORKSHOP@", true));
      if (folder.EndsWith(".play"))
      {
        foreach (string PATH in LSItem.GetLevelsInside(this, folder))
          this.AddItem(new LSItem(0.0f, 0.0f, this, PATH));
        this.PositionItems();
      }
      else if (folder == "@WORKSHOP@")
      {
        foreach (string PATH in LSItem.GetLevelsInside(this, folder))
          this.AddItem(new LSItem(0.0f, 0.0f, this, PATH));
        this.PositionItems();
      }
      else
      {
        string[] directories = DuckFile.GetDirectories(folder);
        string[] files = DuckFile.GetFiles(folder);
        foreach (string PATH in directories)
          this.AddItem(new LSItem(0.0f, 0.0f, this, PATH));
        List<string> stringList = new List<string>();
        foreach (string str in files)
        {
          string file = str;
          if (Path.GetExtension(file) == ".lev" && this._filters.TrueForAll((Predicate<IFilterLSItems>) (a => a.Filter(file))))
            stringList.Add(file);
          else if (Path.GetExtension(file) == ".play")
            this.AddItem(new LSItem(0.0f, 0.0f, this, file));
        }
        foreach (string PATH in stringList)
          this.AddItem(new LSItem(0.0f, 0.0f, this, PATH));
        this.PositionItems();
      }
    }

    public override void Initialize()
    {
      AnalogGamePad.repeat = true;
      Keyboard.repeat = true;
      this.SetCurrentFolder(this._rootDirectory);
      this.isInitialized = true;
      this._dialog.DoInitialize();
      float num1 = 320f;
      float num2 = 180f;
      this._confirmMenu = new UIMenu("DELETE FILE!?", num1 / 2f, num2 / 2f, 160f, conString: "@SELECT@SELECT @QUACK@OH NO!");
      if (this._returnMenu != null)
      {
        this._confirmMenu.Add((UIComponent) new UIMenuItem("WHAT? NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._confirmMenu, (UIComponent) this._returnMenu), backButton: true), true);
        this._confirmMenu.Add((UIComponent) new UIMenuItem("YEAH!", (UIMenuAction) new UIMenuActionOpenMenuSetBoolean((UIComponent) this._confirmMenu, (UIComponent) this._returnMenu, this._deleteFile)), true);
        this._notOnlineMenu = new UIMenu("NO WAY", num1 / 2f, num2 / 2f, 160f, conString: "@SELECT@OH :(");
        BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
        UIText uiText1 = new UIText("THIS LEVEL CONTAINS", Color.White);
        uiText1.SetFont(f);
        this._notOnlineMenu.Add((UIComponent) uiText1, true);
        UIText uiText2 = new UIText("OFFLINE ONLY STUFF.", Color.White);
        uiText2.SetFont(f);
        this._notOnlineMenu.Add((UIComponent) uiText2, true);
        UIText uiText3 = new UIText(" ", Color.White);
        uiText3.SetFont(f);
        this._notOnlineMenu.Add((UIComponent) uiText3, true);
        this._notOnlineMenu.Add((UIComponent) new UIMenuItem("OH", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._confirmMenu, (UIComponent) this._returnMenu), backButton: true), true);
      }
      else
      {
        this._confirmMenu.Add((UIComponent) new UIMenuItem("WHAT? NO!", (UIMenuAction) new UIMenuActionCloseMenu((UIComponent) this._confirmMenu), backButton: true), true);
        this._confirmMenu.Add((UIComponent) new UIMenuItem("YEAH!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean((UIComponent) this._confirmMenu, this._deleteFile)), true);
      }
      this._confirmMenu.Close();
      Level.Add((Thing) this._confirmMenu);
    }

    public void AddItem(LSItem item) => this._items.Add(item);

    public void PositionItems()
    {
      int num1 = 0;
      int num2 = 0;
      foreach (LSItem lsItem in this._items)
      {
        if (num1 >= this._topIndex + this._maxItems || num1 < this._topIndex)
        {
          lsItem.visible = false;
          ++num1;
        }
        else
        {
          lsItem.visible = true;
          lsItem.x = this._leftPos;
          lsItem.y = this._topPos + (float) (num2 * 10);
          if (num1 == this._selectedItem)
          {
            lsItem.selected = true;
            this._selectedLevel = lsItem;
          }
          else
            lsItem.selected = false;
          ++num1;
          ++num2;
        }
      }
    }

    public void FolderUp()
    {
      if (this._currentDirectory == "@WORKSHOP@")
        this.SetCurrentFolder(this._rootDirectory);
      else
        this.SetCurrentFolder(this._currentDirectory.Substring(0, this._currentDirectory.LastIndexOf('/')));
    }

    public override void Update()
    {
      HUD.CloseCorner(HUDCorner.TopLeft);
      this._dialog.DoUpdate();
      if (this._dialog.opened)
        return;
      Editor.lockInput = (ContextMenu) null;
      if (this._dialog.result != null && this._dialog.result != "")
      {
        string result = this._dialog.result;
        LevelPlaylist levelPlaylist = new LevelPlaylist();
        levelPlaylist.levels.AddRange((IEnumerable<string>) Editor.activatedLevels);
        XDocument doc = new XDocument();
        doc.Add((object) levelPlaylist.Serialize());
        DuckFile.SaveXDocument(doc, DuckFile.levelDirectory + result + ".play");
        this.SetCurrentFolder(this._rootDirectory);
        this._dialog.result = (string) null;
      }
      else
      {
        if (this._selectedLevel == null)
          this._exiting = true;
        if (Editor.activatedLevels.Count > 0)
        {
          if (!this.showPlaylistOption)
          {
            this.showPlaylistOption = true;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "@RAGDOLL@NEW PLAYLIST");
          }
        }
        else if (this.showPlaylistOption)
        {
          this.showPlaylistOption = false;
          HUD.CloseCorner(HUDCorner.BottomLeft);
        }
        if (this._deleteFile.value)
        {
          foreach (string str in this._selectedLevel.levelsInside)
            Editor.activatedLevels.Remove(str);
          Editor.activatedLevels.Remove(this._selectedLevel.path);
          if (this._selectedLevel.itemType == LSItemType.Folder)
            DuckFile.DeleteFolder(DuckFile.levelDirectory + this._selectedLevel.path);
          else if (this._selectedLevel.itemType == LSItemType.Playlist)
            DuckFile.Delete(DuckFile.levelDirectory + this._selectedLevel.path);
          else
            DuckFile.Delete(DuckFile.levelDirectory + this._selectedLevel.path + ".lev");
          Thread.Sleep(100);
          this.SetCurrentFolder(this._currentDirectory);
          this._deleteFile.value = false;
        }
        if (this._exiting)
        {
          HUD.CloseAllCorners();
          Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.04f);
          if ((double) Graphics.fade >= 0.00999999977648258)
            return;
          this.isClosed = true;
        }
        else
        {
          Graphics.fade = Lerp.Float(Graphics.fade, 1f, 0.04f);
          if (Input.Pressed("UP"))
          {
            if (this._selectedItem > 0)
              --this._selectedItem;
            if (this._selectedItem < this._topIndex)
              this._topIndex = this._selectedItem;
          }
          else if (Input.Pressed("DOWN"))
          {
            if (this._selectedItem < this._items.Count<LSItem>() - 1)
              ++this._selectedItem;
            if (this._selectedItem >= this._topIndex + this._maxItems)
              this._topIndex = this._selectedItem + 1 - this._maxItems;
          }
          else if (Input.Pressed("LEFT"))
          {
            this._selectedItem -= this._maxItems - 1;
            if (this._selectedItem < 0)
              this._selectedItem = 0;
            if (this._selectedItem < this._topIndex)
              this._topIndex = this._selectedItem;
          }
          else if (Input.Pressed("RIGHT"))
          {
            this._selectedItem += this._maxItems - 1;
            if (this._selectedItem > this._items.Count<LSItem>() - 1)
              this._selectedItem = this._items.Count<LSItem>() - 1;
            if (this._selectedItem >= this._topIndex + this._maxItems)
              this._topIndex = this._selectedItem + 1 - this._maxItems;
          }
          else if (Input.Pressed("SHOOT"))
          {
            if (this._selectedLevel.itemType != LSItemType.UpFolder)
            {
              if (this._selectedLevel.itemType == LSItemType.Folder || this._selectedLevel.itemType == LSItemType.Playlist || this._selectedLevel.itemType == LSItemType.Workshop)
              {
                if (!this._selectedLevel.enabled)
                {
                  this._selectedLevel.enabled = true;
                  this._selectedLevel.partiallyEnabled = false;
                  Editor.activatedLevels.AddRange((IEnumerable<string>) this._selectedLevel.levelsInside);
                }
                else
                {
                  this._selectedLevel.enabled = false;
                  this._selectedLevel.partiallyEnabled = false;
                  foreach (string str in this._selectedLevel.levelsInside)
                    Editor.activatedLevels.Remove(str);
                }
              }
              else if (Editor.activatedLevels.Contains(this._selectedLevel.path))
                Editor.activatedLevels.Remove(this._selectedLevel.path);
              else
                Editor.activatedLevels.Add(this._selectedLevel.path);
            }
          }
          else if (Input.Pressed("SELECT"))
          {
            if (this._selectedLevel.itemType == LSItemType.Workshop)
              this.SetCurrentFolder(this._selectedLevel.path);
            else if (this._selectedLevel.itemType == LSItemType.Folder || this._selectedLevel.itemType == LSItemType.Playlist)
              this.SetCurrentFolder(this._rootDirectory + this._selectedLevel.path);
            else if (this._selectedLevel.itemType == LSItemType.UpFolder)
              this.FolderUp();
          }
          else if (Input.Pressed("QUACK"))
          {
            if (this._currentDirectory != this._rootDirectory)
              this.FolderUp();
          }
          else if (Input.Pressed("START"))
            this._exiting = true;
          else if (Input.Pressed("RAGDOLL"))
          {
            this._dialog.Open("New Playlist...");
            Editor.lockInput = (ContextMenu) this._dialog;
          }
          else if (Input.Pressed("GRAB") && MonoMain.pauseMenu != this._confirmMenu && (this._selectedLevel.itemType != LSItemType.UpFolder && this._selectedLevel.itemType != LSItemType.Workshop))
          {
            LevelSelect._skipCompanionOpening = true;
            MonoMain.pauseMenu = (UIComponent) this._confirmMenu;
            this._confirmMenu.Open();
            SFX.Play("pause", 0.6f);
          }
          this.PositionItems();
          if (this._selectedLevel != this._lastSelection)
          {
            if (this._lastSelection == null || this._selectedLevel.itemType != this._lastSelection.itemType)
            {
              HUD.CloseCorner(HUDCorner.BottomRight);
              if (this._selectedLevel.itemType == LSItemType.UpFolder)
                HUD.AddCornerControl(HUDCorner.BottomRight, "@SELECT@Return");
              else if (this._selectedLevel.itemType == LSItemType.Folder || this._selectedLevel.itemType == LSItemType.Playlist || this._selectedLevel.itemType == LSItemType.Workshop)
              {
                HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@Toggle");
                HUD.AddCornerControl(HUDCorner.BottomRight, "@SELECT@Open");
              }
              else
                HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@Toggle");
            }
            this._lastSelection = this._selectedLevel;
          }
          if (this._selectedLevel != this._previewItem)
          {
            if (this._selectedLevel.itemType == LSItemType.Level)
            {
              this._preview = Content.GeneratePreview(this._selectedLevel.path);
              this._previewSprite = this._preview == null ? (Sprite) null : new Sprite(this._preview, 0.0f, 0.0f);
            }
            else
              this._previewSprite = (Sprite) null;
            this._previewItem = this._selectedLevel;
          }
          foreach (Thing thing in this._items)
            thing.Update();
        }
      }
    }

    public void DrawThings(bool drawBack = false)
    {
      if (drawBack)
        Graphics.DrawRect(new Vec2(0.0f, 0.0f), new Vec2(Layer.HUD.camera.width, Layer.HUD.camera.height), Color.Black, (Depth) -0.8f);
      foreach (LSItem lsItem in this._items)
      {
        if (lsItem.visible)
          lsItem.Draw();
      }
      Depth depth = this._font.depth;
      if (this._previewSprite != null)
      {
        this._previewSprite.scale = new Vec2(0.125f, 0.125f);
        this._previewSprite.depth = (Depth) 0.9f;
        Graphics.Draw(this._previewSprite, 150f, 45f);
      }
      this._font.depth = depth;
      this._font.Draw(!(this._currentDirectory == "@WORKSHOP@") ? "Levels" + this._currentDirectory.Substring(this._rootDirectory.Length, this._currentDirectory.Length - this._rootDirectory.Length) : "Levels/Workshop", this._leftPos, this._topPos - 10f, Color.LimeGreen);
      this._dialog.DoDraw();
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.HUD)
        this.DrawThings();
      base.PostDrawLayer(layer);
    }

    public override void Terminate()
    {
      this._items.Clear();
      AnalogGamePad.repeat = false;
      Keyboard.repeat = false;
    }
  }
}
