// Decompiled with JetBrains decompiler
// Type: DuckGame.MonoFileDialog
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class MonoFileDialog : ContextMenu
  {
    private bool _save;
    private string _currentDirectory;
    private string _rootFolder;
    private bool _scrollBar;
    private float _scrollPosition;
    private float _scrollLerp;
    private int _maxItems = 15;
    private float _scrollWait;
    private bool _doDeleteDialog;
    private bool _doOverwriteDialog;
    private ContextFileType _type;
    private Sprite _badTileset;
    private Sprite _badParallax;
    private Sprite _badArcade;
    private TextEntryDialog _dialog;
    private MessageDialogue _deleteDialog;
    private MessageDialogue _overwriteDialog;
    private string _overwriteName = "";
    private bool _selectLevels;
    private bool _loadLevel;
    public string result;
    private float hOffset = -86f;
    private float _fdHeight = 262f;
    private string _prevPreviewPath = "";
    private Tex2D _preview;
    private Sprite _previewSprite;
    public bool drag;

    public string rootFolder => this._rootFolder;

    public MonoFileDialog()
      : base((IContextListener) null)
    {
    }

    public override void Initialize()
    {
      this.layer = Layer.HUD;
      this.depth = (Depth) 0.9f;
      this._showBackground = false;
      this.itemSize = new Vec2(390f, 16f);
      this._root = true;
      this._dialog = new TextEntryDialog();
      Level.Add((Thing) this._dialog);
      this._deleteDialog = new MessageDialogue();
      Level.Add((Thing) this._deleteDialog);
      this._overwriteDialog = new MessageDialogue();
      Level.Add((Thing) this._overwriteDialog);
      this.drawControls = false;
    }

    public void Open(
      string rootFolder,
      string currentFolder,
      bool save,
      bool selectLevels = false,
      bool loadLevel = true,
      ContextFileType type = ContextFileType.Level)
    {
      this._type = type;
      this._selectLevels = selectLevels;
      this._loadLevel = loadLevel;
      if (this._type == ContextFileType.Block || this._type == ContextFileType.Background || this._type == ContextFileType.Platform)
        this._badTileset = new Sprite("badTileset");
      if (this._type == ContextFileType.Parallax)
        this._badParallax = new Sprite("badParallax");
      if (this._type == ContextFileType.ArcadeStyle)
        this._badArcade = new Sprite("badArcade");
      this._preview = (Tex2D) null;
      this._previewSprite = (Sprite) null;
      float num1 = 350f;
      float num2 = 350f;
      Vec2 vec2_1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0));
      Vec2 vec2_2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0));
      this.position = vec2_1 + new Vec2(4f, 40f);
      this._save = save;
      rootFolder = rootFolder.Replace('\\', '/');
      currentFolder = currentFolder.Replace('\\', '/');
      this._currentDirectory = !(currentFolder == "") ? currentFolder : rootFolder;
      this._rootFolder = rootFolder;
      this.SetDirectory(this._currentDirectory);
      Editor.lockInput = (ContextMenu) this;
      SFX.Play("openClick", 0.4f);
      this.opened = true;
    }

    public void Close()
    {
      Editor.lockInput = (ContextMenu) null;
      this.opened = false;
      this.ClearItems();
    }

    public string TypeExtension()
    {
      if (this._type == ContextFileType.Level)
        return ".lev";
      return this._type == ContextFileType.Block || this._type == ContextFileType.Background || (this._type == ContextFileType.Platform || this._type == ContextFileType.Parallax) || (this._type == ContextFileType.ArcadeAnimation || this._type == ContextFileType.ArcadeStyle) ? ".png" : "";
    }

    public void SetDirectory(string dir)
    {
      dir = Path.GetFullPath(dir);
      dir = dir.Replace('\\', '/');
      while (dir.StartsWith("/"))
        dir = dir.Substring(1);
      if (dir.EndsWith("/"))
        dir = dir.Substring(0, dir.Length - 1);
      if (dir.Length < this._rootFolder.Length)
        dir = this._rootFolder;
      int num1 = 0;
      this._currentDirectory = dir;
      if (this._currentDirectory != this._rootFolder)
        ++num1;
      if (this._save)
        ++num1;
      string[] directories = DuckFile.GetDirectories(this._currentDirectory);
      string[] files = DuckFile.GetFiles(this._currentDirectory);
      int num2 = num1 + (directories.Length + files.Length);
      float x = 338f;
      this._scrollBar = false;
      this._scrollPosition = 0.0f;
      if (num2 > this._maxItems)
      {
        x = 326f;
        this._scrollBar = true;
      }
      if (this._save)
        this.AddItem(new ContextMenu((IContextListener) this)
        {
          text = "@NEWICONTINY@New File...",
          data = "New File...",
          itemSize = new Vec2(x, 16f)
        });
      if (this._currentDirectory != this._rootFolder)
        this.AddItem(new ContextMenu((IContextListener) this)
        {
          text = "@LOADICONTINY@../",
          data = "../",
          itemSize = new Vec2(x, 16f)
        });
      foreach (string path in directories)
      {
        string fileName = Path.GetFileName(path);
        this.AddItem(new ContextMenu((IContextListener) this)
        {
          fancy = true,
          text = "@LOADICONTINY@" + fileName,
          data = fileName,
          itemSize = new Vec2(x, 16f)
        });
      }
      foreach (string path in files)
      {
        string fileName = Path.GetFileName(path);
        if (!this._selectLevels)
        {
          if (fileName.EndsWith(this.TypeExtension()))
          {
            ContextMenu contextMenu = new ContextMenu((IContextListener) this)
            {
              fancy = true,
              text = fileName
            };
            contextMenu.text = fileName.Substring(0, fileName.Length - 4);
            contextMenu.data = fileName;
            contextMenu.itemSize = new Vec2(x, 16f);
            this.AddItem(contextMenu);
          }
        }
        else
        {
          string str1 = path.Replace('\\', '/');
          string str2 = str1.Substring(0, str1.Length - 4);
          string str3 = str2.Substring(str2.IndexOf("/levels/", StringComparison.InvariantCultureIgnoreCase) + 8);
          ContextCheckBox contextCheckBox = new ContextCheckBox(fileName, (IContextListener) this);
          contextCheckBox.fancy = true;
          contextCheckBox.path = str3;
          contextCheckBox.isChecked = Editor.activatedLevels.Contains(str3);
          contextCheckBox.itemSize = new Vec2(x, 16f);
          this.AddItem((ContextMenu) contextCheckBox);
        }
      }
      int num3 = (int) Math.Round((double) (this._items.Count - 1 - this._maxItems) * (double) this._scrollPosition);
      int num4 = 0;
      for (int index = 0; index < this._items.Count; ++index)
      {
        if (index < num3 || index > num3 + this._maxItems)
        {
          this._items[index].visible = false;
        }
        else
        {
          this._items[index].visible = true;
          this._items[index].position = new Vec2(this._items[index].position.x, (float) ((double) this.y + 3.0 + (double) num4 * ((double) this._items[index].itemSize.y + 1.0)));
          ++num4;
        }
      }
      this.menuSize.y = this._fdHeight;
    }

    public override void Selected(ContextMenu item)
    {
      SFX.Play("highClick", 0.3f);
      if (item.text == "@NEWICONTINY@New File...")
      {
        this._dialog.Open("Save File As...");
        Editor.lockInput = (ContextMenu) this._dialog;
      }
      else if (item.data.EndsWith(this.TypeExtension()) && this._type != ContextFileType.All)
      {
        if (!this._selectLevels)
        {
          if (!this._save)
          {
            this.Close();
            string str = this._currentDirectory + "/" + item.data;
            if (this._loadLevel)
              (Level.current as Editor).LoadLevel(str);
            else if (this._type == ContextFileType.ArcadeStyle)
              this.result = Editor.TextureToString((Texture2D) Content.Load<Tex2D>(str));
            else
              this.result = str.Replace(this._rootFolder, "");
          }
          else
          {
            this._overwriteDialog.Open("OVERWRITE " + item.data + "?");
            Editor.lockInput = (ContextMenu) this._overwriteDialog;
            this._doOverwriteDialog = true;
            this._overwriteDialog.result = false;
            this._overwriteName = item.data;
            Editor.tookInput = true;
          }
        }
        else
        {
          if (!(item is ContextCheckBox contextCheckBox))
            return;
          contextCheckBox.isChecked = !contextCheckBox.isChecked;
          if (!contextCheckBox.isChecked)
            Editor.activatedLevels.Remove(contextCheckBox.path);
          else
            Editor.activatedLevels.Add(contextCheckBox.path);
        }
      }
      else
      {
        this.ClearItems();
        this.SetDirectory(this._currentDirectory + "/" + item.data);
      }
    }

    public override void Toggle(ContextMenu item)
    {
    }

    public override void Update()
    {
      if (!this.opened || this._dialog.opened || (this._deleteDialog.opened || this._overwriteDialog.opened) || this._opening)
      {
        this._opening = false;
        foreach (ContextMenu contextMenu in this._items)
          contextMenu.disabled = true;
      }
      else
      {
        bool flag = false;
        foreach (ContextMenu contextMenu in this._items)
        {
          contextMenu.disabled = false;
          if (!flag && contextMenu.hover)
          {
            flag = true;
            string str = "";
            int startIndex = this._currentDirectory.IndexOf(this._rootFolder);
            if (startIndex != -1)
              str = this._currentDirectory.Remove(startIndex, this._rootFolder.Length);
            if (str != "" && !str.EndsWith("/"))
              str += "/";
            if (str.StartsWith("/"))
              str = str.Substring(1, str.Length - 1);
            string path = str + contextMenu.data;
            if (this._prevPreviewPath != path)
            {
              if (path.EndsWith(".lev"))
              {
                if (this._preview == null || !(this._preview is RenderTarget2D))
                  this._preview = (Tex2D) new RenderTarget2D(320, 200);
                this._preview = (Tex2D) Content.GeneratePreview(path.Substring(0, path.Length - 4), this._preview as RenderTarget2D);
              }
              else if (path.EndsWith(".png"))
              {
                Texture2D texture2D = ContentPack.LoadTexture2D(this._currentDirectory + "/" + Path.GetFileName(path));
                this._preview = texture2D == null ? Content.invalidTexture : (this._type != ContextFileType.Block && this._type != ContextFileType.Background && this._type != ContextFileType.Platform || texture2D.Width == 128 && texture2D.Height == 128 ? (this._type != ContextFileType.Parallax || texture2D.Width == 320 && texture2D.Height == 240 ? (this._type != ContextFileType.ArcadeStyle || texture2D.Width == 27 && texture2D.Height == 34 || texture2D.Width == 54 && texture2D.Height == 68 ? (Tex2D) texture2D : this._badArcade.texture) : this._badParallax.texture) : this._badTileset.texture);
              }
              else
              {
                this._previewSprite = (Sprite) null;
                this._prevPreviewPath = (string) null;
              }
              this._previewSprite = new Sprite(this._preview);
              if (this._type == ContextFileType.Block || this._type == ContextFileType.Background || (this._type == ContextFileType.Platform || this._type == ContextFileType.Parallax))
                this._previewSprite.scale = new Vec2(2f, 2f);
              this._prevPreviewPath = path;
            }
          }
        }
        if (!flag && this._type == ContextFileType.ArcadeStyle)
        {
          this._preview = this._badArcade.texture;
          this._previewSprite = new Sprite(this._preview);
          this._prevPreviewPath = (string) null;
        }
        Editor.lockInput = (ContextMenu) this;
        base.Update();
        this._scrollWait = Lerp.Float(this._scrollWait, 0.0f, 0.2f);
        if (this._dialog.result != null && this._dialog.result != "")
        {
          Editor current = Level.current as Editor;
          current._guid = Guid.NewGuid().ToString();
          Editor.ResetWorkshopData();
          current.DoSave(this._currentDirectory + "/" + this._dialog.result);
          this._dialog.result = "";
          this.Close();
        }
        if (!this._overwriteDialog.opened && this._doOverwriteDialog)
        {
          this._doOverwriteDialog = false;
          if (this._overwriteDialog.result)
          {
            (Level.current as Editor).DoSave(this._currentDirectory + "/" + this._overwriteName);
            this._overwriteDialog.result = false;
            this._overwriteName = "";
            this.Close();
          }
        }
        if (!this._deleteDialog.opened && this._doDeleteDialog)
        {
          this._doDeleteDialog = false;
          if (this._deleteDialog.result)
          {
            foreach (ContextMenu contextMenu in this._items)
            {
              if (contextMenu.hover)
              {
                Editor.Delete(this._currentDirectory + "/" + contextMenu.text);
                break;
              }
            }
            this.ClearItems();
            this.SetDirectory(this._currentDirectory);
          }
        }
        if (Keyboard.Pressed(Keys.Escape) || Mouse.right == InputState.Pressed || Input.Pressed("QUACK"))
          this.Close();
        if (!this._selectLevels && Input.Pressed("GRAB"))
        {
          this._deleteDialog.Open("CONFIRM DELETE");
          Editor.lockInput = (ContextMenu) this._deleteDialog;
          this._doDeleteDialog = true;
          this._deleteDialog.result = false;
        }
        else
        {
          if (Input.Pressed("LEFT"))
            this._selectedIndex -= this._maxItems;
          else if (Input.Pressed("RIGHT"))
            this._selectedIndex += this._maxItems;
          this._selectedIndex = Maths.Clamp(this._selectedIndex, 0, this._items.Count - 1);
          if (this._items.Count <= this._maxItems)
            return;
          float num1 = 1f / (float) (this._items.Count - this._maxItems);
          if ((double) Mouse.scroll != 0.0)
          {
            this._scrollPosition += (float) Math.Sign(Mouse.scroll) * num1;
            if ((double) this._scrollPosition > 1.0)
              this._scrollPosition = 1f;
            if ((double) this._scrollPosition < 0.0)
              this._scrollPosition = 0.0f;
          }
          int num2 = (int) Math.Round(((double) (this._items.Count - this._maxItems) - 1.0) * (double) this._scrollPosition);
          int num3 = 0;
          int num4 = 0;
          for (int index = 0; index < this._items.Count; ++index)
          {
            if (this._items[index].hover)
            {
              num4 = index;
              break;
            }
          }
          if (Editor.gamepadMode)
          {
            if (num4 > num2 + this._maxItems)
              this._scrollPosition += (float) (num4 - (num2 + this._maxItems)) * num1;
            else if (num4 < num2)
              this._scrollPosition -= (float) (num2 - num4) * num1;
          }
          for (int index = 0; index < this._items.Count; ++index)
          {
            this._items[index].disabled = false;
            if (index < num2 || index > num2 + this._maxItems)
            {
              this._items[index].visible = false;
              this._items[index].hover = false;
            }
            else
            {
              ContextMenu contextMenu = this._items[index];
              this._items[index].visible = true;
              this._items[index].position = new Vec2(this._items[index].position.x, (float) ((double) this.y + 3.0 + (double) num3 * (double) this._items[index].itemSize.y));
              ++num3;
            }
          }
        }
      }
    }

    public override void Draw()
    {
      this.menuSize.y = this._fdHeight;
      if (!this.opened)
        return;
      base.Draw();
      float num1 = 350f;
      float num2 = this._fdHeight + 22f;
      Vec2 p1_1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0 - 15.0));
      Vec2 p2_1 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0 - 12.0));
      DuckGame.Graphics.DrawRect(p1_1, p2_1, new Color(70, 70, 70), this.depth, false);
      DuckGame.Graphics.DrawRect(p1_1, p2_1, new Color(30, 30, 30), this.depth - 8);
      DuckGame.Graphics.DrawRect(p1_1 + new Vec2(4f, 23f), p2_1 + new Vec2(-16f, -4f), new Color(10, 10, 10), this.depth - 4);
      Vec2 p1_2 = new Vec2(p2_1.x - 14f, p1_1.y + 23f);
      Vec2 p2_2 = p2_1 + new Vec2(-4f, -4f);
      DuckGame.Graphics.DrawRect(p1_2, p2_2, new Color(10, 10, 10), this.depth - 4);
      DuckGame.Graphics.DrawRect(p1_1 + new Vec2(3f, 3f), new Vec2(p2_1.x - 3f, p1_1.y + 19f), new Color(70, 70, 70), this.depth - 4);
      if (this._scrollBar)
      {
        this._scrollLerp = Lerp.Float(this._scrollLerp, this._scrollPosition, 0.05f);
        Vec2 p1_3 = new Vec2(p2_1.x - 12f, (float) ((double) this.topRight.y + 7.0 + (240.0 * (double) this._scrollLerp - 4.0)));
        Vec2 p2_3 = new Vec2(p2_1.x - 6f, (float) ((double) this.topRight.y + 11.0 + (240.0 * (double) this._scrollLerp + 8.0)));
        bool flag = false;
        if ((double) Mouse.x > (double) p1_3.x && (double) Mouse.x < (double) p2_3.x && ((double) Mouse.y > (double) p1_3.y && (double) Mouse.y < (double) p2_3.y))
        {
          flag = true;
          if (Mouse.left == InputState.Pressed)
            this.drag = true;
        }
        if (Mouse.left == InputState.None)
          this.drag = false;
        if (this.drag)
        {
          this._scrollPosition = (float) (((double) Mouse.y - (double) p1_2.y - 10.0) / ((double) p2_2.y - (double) p1_2.y - 20.0));
          if ((double) this._scrollPosition < 0.0)
            this._scrollPosition = 0.0f;
          if ((double) this._scrollPosition > 1.0)
            this._scrollPosition = 1f;
          this._scrollLerp = this._scrollPosition;
        }
        DuckGame.Graphics.DrawRect(p1_3, p2_3, this.drag ? new Color(190, 190, 190) : (flag ? new Color(120, 120, 120) : new Color(70, 70, 70)), this.depth + 4);
      }
      string str1 = this._currentDirectory;
      int startIndex1 = this._currentDirectory.IndexOf(this._rootFolder);
      if (startIndex1 != -1)
        str1 = this._currentDirectory.Remove(startIndex1, this._rootFolder.Length);
      string str2 = Path.GetFileName(this._rootFolder) + str1;
      if (str2 == "")
        str2 = this._type != ContextFileType.Block ? (this._type != ContextFileType.Platform ? (this._type != ContextFileType.Background ? (this._type != ContextFileType.Parallax ? (this._type != ContextFileType.ArcadeStyle ? "LEVELS" : "Custom/Arcade") : "Custom/Parallax") : "Custom/Background") : "Custom/Platform") : "Custom/Blocks";
      if (this._save)
        DuckGame.Graphics.DrawString("@SAVEICON@Save Level - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._selectLevels)
        DuckGame.Graphics.DrawString("Select Active Levels - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._type == ContextFileType.Block)
        DuckGame.Graphics.DrawString("@LOADICON@Custom - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._type == ContextFileType.Platform)
        DuckGame.Graphics.DrawString("@LOADICON@Custom - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._type == ContextFileType.Background)
        DuckGame.Graphics.DrawString("@LOADICON@Custom - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._type == ContextFileType.Parallax)
        DuckGame.Graphics.DrawString("@LOADICON@Custom - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else if (this._type == ContextFileType.ArcadeStyle)
        DuckGame.Graphics.DrawString("@LOADICON@Custom - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else
        DuckGame.Graphics.DrawString("@LOADICON@Load Level - " + str2, p1_1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      Vec2 p1_4 = new Vec2(p2_1.x + 2f, p1_1.y);
      Vec2 p2_4 = p1_4 + new Vec2(166f, 120f);
      if (this._previewSprite != null && this._previewSprite.texture != null && (this._type == ContextFileType.Block || this._type == ContextFileType.Background || (this._type == ContextFileType.Platform || this._type == ContextFileType.Parallax) || (this._type == ContextFileType.ArcadeStyle || this._type == ContextFileType.ArcadeAnimation)))
        p2_4 = this._type != ContextFileType.Parallax ? p1_4 + new Vec2((float) (this._previewSprite.width + 4), (float) (this._previewSprite.height + 4)) : p1_4 + new Vec2((float) (this._previewSprite.width / 2 + 4), (float) (this._previewSprite.height / 2 + 4));
      DuckGame.Graphics.DrawRect(p1_4, p2_4, new Color(70, 70, 70), this.depth, false);
      DuckGame.Graphics.DrawRect(p1_4, p2_4, new Color(30, 30, 30), this.depth - 8);
      if (this._previewSprite == null || this._previewSprite.texture == null)
        return;
      this._previewSprite.depth = (Depth) 0.95f;
      this._previewSprite.scale = new Vec2(0.5f);
      if (this._type == ContextFileType.Block || this._type == ContextFileType.Background || this._type == ContextFileType.Platform)
        this._previewSprite.scale = new Vec2(1f);
      DuckGame.Graphics.Draw(this._previewSprite, p1_4.x + 2f, p1_4.y + 2f);
      if (Content.renderingPreview || Content.previewLevel == null || !(Content.previewLevel.level + ".lev" == this._prevPreviewPath))
        return;
      bool flag1 = false;
      if (Content.previewLevel.things[typeof (ChallengeMode)].Count<Thing>() > 0)
        flag1 = true;
      bool flag2 = false;
      if (Content.previewLevel.things[typeof (SpawnPoint)].Count<Thing>() == 0)
        flag2 = true;
      bool flag3 = false;
      if (Content.previewLevel.things[typeof (ArcadeMode)].Count<Thing>() > 0)
        flag3 = true;
      string str3 = Content.previewLevel.level;
      int startIndex2 = str3.LastIndexOf("/");
      if (startIndex2 != -1)
        str3 = str3.Substring(startIndex2, str3.Length - startIndex2);
      if (str3.Length > 19)
      {
        string str4 = str3.Substring(0, 18) + ".";
      }
      if (flag2)
        DuckGame.Graphics.DrawString("STRANGE MAP", p1_4 + new Vec2(5f, 107f), Colors.DGPurple, this.depth + 8);
      else if (flag3)
        DuckGame.Graphics.DrawString("ARCADE LAYOUT", p1_4 + new Vec2(5f, 107f), Colors.DGYellow, this.depth + 8);
      else if (flag1)
        DuckGame.Graphics.DrawString("CHALLENGE MAP", p1_4 + new Vec2(5f, 107f), Colors.DGRed, this.depth + 8);
      else if (Content.previewLevel.onlineEnabled)
        DuckGame.Graphics.DrawString("ONLINE MAP", p1_4 + new Vec2(5f, 107f), Colors.DGGreen, this.depth + 8);
      else
        DuckGame.Graphics.DrawString("MULTIPLAYER MAP", p1_4 + new Vec2(5f, 107f), Colors.DGBlue, this.depth + 8);
    }
  }
}
