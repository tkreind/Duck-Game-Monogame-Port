// Decompiled with JetBrains decompiler
// Type: DuckGame.LSItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace DuckGame
{
  public class LSItem : Thing
  {
    public static Dictionary<string, LevelData> bullshitLevelCache = new Dictionary<string, LevelData>();
    private bool _selected;
    private bool _enabled;
    private bool _partiallyEnabled;
    private string _path = "";
    private string _name;
    private LSItemType _itemType;
    private SpriteMap _icons;
    private BitmapFont _font;
    private Sprite _steamIcon;
    private List<string> _levelsInside = new List<string>();
    private LevelSelect _select;

    public bool selected
    {
      get => this._selected;
      set => this._selected = value;
    }

    public bool enabled
    {
      get => this._enabled;
      set => this._enabled = value;
    }

    public bool partiallyEnabled
    {
      get => this._partiallyEnabled;
      set => this._partiallyEnabled = value;
    }

    public string path
    {
      get => this._path;
      set => this._path = value;
    }

    public LSItemType itemType => this._itemType;

    public bool isFolder => this._itemType == LSItemType.Folder;

    public bool isPlaylist => this._itemType == LSItemType.Playlist;

    public List<string> levelsInside => this._levelsInside;

    public LSItem(float xpos, float ypos, LevelSelect select, string PATH, bool isWorkshop = false)
      : base(xpos, ypos)
    {
      this._select = select;
      this._icons = new SpriteMap("tinyIcons", 8, 8);
      this._font = new BitmapFont("biosFont", 8);
      this._path = PATH;
      if (this._path == "../")
      {
        this._name = "../";
        this._itemType = LSItemType.UpFolder;
      }
      else
      {
        string extension = Path.GetExtension(this._path);
        this._itemType = !(extension == ".lev") ? (!(extension == ".play") ? LSItemType.Folder : LSItemType.Playlist) : LSItemType.Level;
        if (isWorkshop)
          this._itemType = LSItemType.Workshop;
        this._name = Path.GetFileNameWithoutExtension(this._path);
        string str1 = this._path.Replace('\\', '/');
        if (isWorkshop)
        {
          this._path = "@WORKSHOP@";
          this._levelsInside = LSItem.GetLevelsInside(this._select, "@WORKSHOP@");
        }
        else
        {
          if (!this.isFolder && !this.isPlaylist)
            str1 = str1.Substring(0, str1.Length - 4);
          string str2 = str1.Substring(str1.LastIndexOf("/levels/", StringComparison.InvariantCultureIgnoreCase) + 8);
          if (this.isFolder || this.isPlaylist)
          {
            this._levelsInside = LSItem.GetLevelsInside(this._select, this._path);
            this._path = "/" + str2;
          }
          else
            this._path = str1 + ".lev";
        }
        bool flag1 = false;
        bool flag2 = true;
        foreach (string str2 in this._levelsInside)
        {
          if (Editor.activatedLevels.Contains(str2))
            flag1 = true;
          else
            flag2 = false;
        }
        this.enabled = flag1;
        this._partiallyEnabled = flag1 && !flag2;
      }
    }

    public static List<string> GetLevelsInside(LevelSelect selector, string path)
    {
      List<string> stringList = new List<string>();
      if (path == "@WORKSHOP@")
      {
        foreach (WorkshopItem allWorkshopItem in Steam.GetAllWorkshopItems())
        {
          if ((allWorkshopItem.stateFlags & WorkshopItemState.Installed) != WorkshopItemState.None && Directory.Exists(allWorkshopItem.path))
          {
            foreach (string file in DuckFile.GetFiles(allWorkshopItem.path))
            {
              string lName = file;
              if (lName.EndsWith(".lev") && selector.filters.TrueForAll((Predicate<IFilterLSItems>) (a => a.Filter(lName, LevelLocation.Workshop))))
                stringList.Add(lName);
            }
          }
        }
      }
      else if (path.EndsWith(".play"))
      {
        XElement node = DuckFile.LoadXDocument(path).Element((XName) "playlist");
        if (node != null)
        {
          LevelPlaylist levelPlaylist = new LevelPlaylist();
          levelPlaylist.Deserialize(node);
          foreach (string level in levelPlaylist.levels)
          {
            string lName = level;
            if (selector.filters.TrueForAll((Predicate<IFilterLSItems>) (a => a.Filter(lName))))
              stringList.Add(lName);
          }
        }
      }
      else
      {
        foreach (string directory in DuckFile.GetDirectories(path))
          stringList.AddRange((IEnumerable<string>) LSItem.GetLevelsInside(selector, directory));
        string[] files = Content.GetFiles(path);
        foreach (string str1 in files)
        {
          string file = str1;
          if (selector.filters.TrueForAll((Predicate<IFilterLSItems>) (a => a.Filter(file))))
          {
            string str2 = file.Replace('\\', '/');
            stringList.Add(str2);
          }
        }
      }
      return stringList;
    }

    public override void Update()
    {
      if (this._itemType == LSItemType.UpFolder || this._itemType == LSItemType.Folder || (this._itemType == LSItemType.Playlist || this._itemType == LSItemType.Workshop))
        return;
      this.enabled = Editor.activatedLevels.Contains(this.path);
    }

    public override void Draw()
    {
      float x = this.x;
      if (this._selected)
      {
        this._icons.frame = 3;
        Graphics.Draw((Sprite) this._icons, x - 8f, this.y);
      }
      string text = this._name;
      if (text.Length > 15)
        text = text.Substring(0, 14) + ".";
      if (this._itemType != LSItemType.UpFolder)
      {
        this._icons.frame = this._partiallyEnabled ? 4 : (this._enabled ? 1 : 0);
        Graphics.Draw((Sprite) this._icons, x, this.y);
        x += 10f;
      }
      if (this._itemType == LSItemType.Folder || this._itemType == LSItemType.UpFolder)
      {
        this._icons.frame = 2;
        Graphics.Draw((Sprite) this._icons, x, this.y);
        x += 10f;
      }
      if (this._itemType == LSItemType.Playlist)
      {
        this._icons.frame = 5;
        Graphics.Draw((Sprite) this._icons, x, this.y);
        x += 10f;
      }
      if (this._itemType == LSItemType.Workshop)
      {
        if (this._steamIcon == null)
          this._steamIcon = new Sprite("steamIcon");
        this._steamIcon.scale = new Vec2(0.25f, 0.25f);
        Graphics.Draw(this._steamIcon, x, this.y);
        x += 10f;
        text = "Workshop";
      }
      if (this.isPlaylist)
        this._font.Draw(text, x, this.y, this._selected ? Colors.DGBlue : Colors.DGBlue * 0.75f, new Depth(0.8f));
      else
        this._font.Draw(text, x, this.y, this._selected ? Color.White : Color.Gray, new Depth(0.8f));
    }
  }
}
