// Decompiled with JetBrains decompiler
// Type: DuckGame.UIModManagement
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DuckGame
{
  public class UIModManagement : UIMenu
  {
    private const int FO_DELETE = 3;
    private const int FOF_ALLOWUNDO = 64;
    private const int FOF_NOCONFIRMATION = 16;
    private const int boxHeight = 36;
    private const int scrollWidth = 12;
    private const int boxSideMargin = 14;
    private const int scrollBarHeight = 32;
    private UIMenu _openOnClose;
    private Sprite _moreArrow;
    private Sprite _noImage;
    private Sprite _steamIcon;
    private SpriteMap _cursor;
    private SpriteMap _localIcon;
    private SpriteMap _newIcon;
    private IList<Mod> _mods;
    private int _hoverIndex;
    private UIBox _box;
    private FancyBitmapFont _fancyFont;
    private int _maxModsToShow;
    private UIMenuItem _uploadItem;
    private UIMenuItem _disableOrEnableItem;
    private UIMenuItem _deleteOrUnsubItem;
    private UIMenuItem _visitItem;
    public UIMenu _editModMenu;
    public UIMenu _yesNoMenu;
    private UIMenuItem _yesNoYes;
    private UIMenuItem _yesNoNo;
    private WorkshopItem _transferItem;
    private bool _transferring;
    private bool _awaitingChanges;
    private Textbox _updateTextBox;
    private Rectangle _updateButton;
    private string _updateButtonText = "UPDATE MOD!";
    private int _pressWait;
    private bool _showingMenu;
    private bool _draggingScrollbar;
    private Vec2 _oldPos;
    private Mod _selectedMod;
    private bool modsChanged;
    private bool fixView = true;
    private int scrollBarTop;
    private int scrollBarBottom;
    private int scrollBarScrollableHeight;
    private int scrollBarOffset;
    private int _scrollItemOffset;
    private bool _gamepadMode = true;
    private bool _needsUpdateNotes;

    public override void Close()
    {
      if (!this.fixView)
      {
        this._showingMenu = false;
        this._editModMenu.Close();
        Layer.HUD.camera.width /= 2f;
        Layer.HUD.camera.height /= 2f;
        this.fixView = true;
        DevConsole.RestoreDevConsole();
      }
      base.Close();
    }

    private void EnableDisableMod()
    {
      this._awaitingChanges = true;
      if (this._selectedMod.configuration.disabled)
        this._selectedMod.configuration.Enable();
      else
        this._selectedMod.configuration.Disable();
      this.modsChanged = true;
      this._editModMenu.Close();
      this.Open();
    }

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHFileOperation(ref UIModManagement.SHFILEOPSTRUCT FileOp);

        private static void DeleteFileOrFolder(string path)
        {
            var val = new UIModManagement.SHFILEOPSTRUCT()
            {
                wFunc = 3,
                pFrom = path + (object)char.MinValue + (object)char.MinValue,
                fFlags = (short)80
            };
            UIModManagement.SHFileOperation(ref val);
        }

    private void DeleteMod() => this.ShowYesNo(this._editModMenu, (UIMenuActionCallFunction.Function) (() =>
    {
      this._awaitingChanges = true;
      if (this._selectedMod.configuration.workshopID == 0UL)
        UIModManagement.DeleteFileOrFolder(this._selectedMod.configuration.directory);
      else
        Steam.WorkshopUnsubscribe(this._selectedMod.configuration.workshopID);
      this._mods.Remove(this._selectedMod);
      this._hoverIndex = -1;
      this._yesNoMenu.Close();
      this._editModMenu.Close();
      this.Open();
    }));

    private void ShowYesNo(UIMenu goBackTo, UIMenuActionCallFunction.Function onYes)
    {
      this._yesNoNo.menuAction = (UIMenuAction) new UIMenuActionCallFunction((UIMenuActionCallFunction.Function) (() =>
      {
        this._yesNoMenu.Close();
        goBackTo.Open();
      }));
      this._yesNoYes.menuAction = (UIMenuAction) new UIMenuActionCallFunction(onYes);
      new UIMenuActionOpenMenu((UIComponent) this._editModMenu, (UIComponent) this._yesNoMenu).Activate();
    }

    private void UploadMod()
    {
      this._editModMenu.Close();
      this.Open();
      if (this._selectedMod.configuration.workshopID == 0UL)
      {
        this._transferItem = Steam.CreateItem();
      }
      else
      {
        this._transferItem = new WorkshopItem(this._selectedMod.configuration.workshopID);
        this._needsUpdateNotes = true;
        this._updateTextBox.GainFocus();
        this._gamepadMode = false;
      }
      this._transferring = false;
    }

    private void VisitModPage()
    {
      this._editModMenu.Close();
      this.Open();
      Steam.OverlayOpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=" + (object) this._selectedMod.configuration.workshopID);
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(sourceDirName);
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      if (!directoryInfo1.Exists)
        throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
      if (!Directory.Exists(destDirName))
        Directory.CreateDirectory(destDirName);
      foreach (FileInfo file in directoryInfo1.GetFiles())
      {
        string str = Path.Combine(destDirName, file.Name);
        file.CopyTo(str, false);
        File.SetAttributes(str, FileAttributes.Normal);
      }
      if (!copySubDirs)
        return;
      foreach (DirectoryInfo directoryInfo2 in directories)
      {
        string destDirName1 = Path.Combine(destDirName, directoryInfo2.Name);
        UIModManagement.DirectoryCopy(directoryInfo2.FullName, destDirName1, copySubDirs);
      }
    }

    public UIModManagement(
      UIMenu openOnClose,
      string title,
      float xpos,
      float ypos,
      float wide = -1f,
      float high = -1f,
      string conString = "",
      InputProfile conProfile = null)
      : base(title, xpos, ypos, wide, high, conString, conProfile)
    {
      this._splitter.topSection.components[0].align = UIAlign.Left;
      this._openOnClose = openOnClose;
      this._moreArrow = new Sprite("moreArrow");
      this._moreArrow.CenterOrigin();
      this._steamIcon = new Sprite("steamIconSmall");
      this._steamIcon.scale = new Vec2(1f) / 2f;
      this._localIcon = new SpriteMap("iconSheet", 16, 16);
      this._localIcon.scale = new Vec2(1f) / 2f;
      this._localIcon.SetFrameWithoutReset(1);
      this._newIcon = new SpriteMap("presents", 16, 16);
      this._newIcon.scale = new Vec2(2f);
      this._newIcon.SetFrameWithoutReset(0);
      this._noImage = new Sprite("notexture");
      this._noImage.scale = new Vec2(2f);
      this._cursor = new SpriteMap("cursors", 16, 16);
      this._mods = (IList<Mod>) ModLoader.allMods.Where<Mod>((Func<Mod, bool>) (a => !(a is CoreMod))).ToList<Mod>();
      this._mods.Add((Mod) null);
      this._maxModsToShow = 8;
      this._box = new UIBox(0.0f, 0.0f, high: ((float) (this._maxModsToShow * 36)), isVisible: false);
      this.Add((UIComponent) this._box, true);
      this._fancyFont = new FancyBitmapFont("smallFont");
      this._fancyFont.maxWidth = (int) this.width - 100;
      this._fancyFont.maxRows = 2;
      this.scrollBarOffset = 0;
      this._editModMenu = new UIMenu("<mod name>", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._editModMenu.Add((UIComponent) (this._disableOrEnableItem = new UIMenuItem("DISABLE", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.EnableDisableMod)))), true);
      this._deleteOrUnsubItem = new UIMenuItem("DELETE", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.DeleteMod)));
      this._uploadItem = new UIMenuItem("UPLOAD", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.UploadMod)));
      this._visitItem = new UIMenuItem("VISIT PAGE", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.VisitModPage)));
      this._editModMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._editModMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._editModMenu, (UIComponent) this)), true);
      this._editModMenu.Close();
      this._yesNoMenu = new UIMenu("ARE YOU SURE?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._yesNoMenu.Add((UIComponent) (this._yesNoYes = new UIMenuItem("YES")), true);
      this._yesNoMenu.Add((UIComponent) (this._yesNoNo = new UIMenuItem("NO")), true);
      this._yesNoMenu.Close();
      this._updateTextBox = new Textbox(0.0f, 0.0f, 0.0f, 0.0f);
      this._updateTextBox.depth = new Depth(0.9f);
      this._updateTextBox.maxLength = 5000;
    }

    public override void Open()
    {
      this._pressWait = 30;
      base.Open();
      DevConsole.SuppressDevConsole();
      this._oldPos = Mouse.positionScreen;
    }

    public override void Update()
    {
      if (this._pressWait > 0)
        --this._pressWait;
      if (this._editModMenu.open)
      {
        if (!UIMenu.globalUILock && (Input.Pressed("QUACK") || Keyboard.Pressed(Keys.Escape)))
        {
          this._editModMenu.Close();
          this.Open();
          return;
        }
      }
      else if (this.open)
      {
        if (this._transferItem != null && !this._needsUpdateNotes)
        {
          if (!this._transferring)
          {
            if (this._transferItem.result == SteamResult.OK)
            {
              WorkshopItemData dat = new WorkshopItemData();
              if (this._selectedMod.configuration.workshopID == 0UL)
              {
                this._selectedMod.configuration.SetWorkshopID(this._transferItem.id);
                dat.name = this._selectedMod.configuration.displayName;
                dat.description = this._selectedMod.configuration.description;
                dat.visibility = RemoteStoragePublishedFileVisibility.Private;
                dat.tags = new List<string>();
                dat.tags.Add("Mod");
              }
              else
                dat.changeNotes = this._updateTextBox.text;
              string pathString = this._selectedMod.configuration.directory + "/content/";
              DuckFile.CreatePath(pathString);
              string path1 = pathString + "screenshot.png";
              if (!File.Exists(path1))
              {
                File.Delete(path1);
                Tex2D screenshot = this._selectedMod.screenshot;
                Stream stream = (Stream) DuckFile.Create(path1);
                ((Texture2D) screenshot.nativeObject).SaveAsPng(stream, screenshot.width, screenshot.height);
                stream.Dispose();
              }
              dat.previewPath = path1;
              string str = DuckFile.workshopDirectory + (object) this._transferItem.id + "/content";
              if (Directory.Exists(str))
                Directory.Delete(str, true);
              DuckFile.CreatePath(str);
              UIModManagement.DirectoryCopy(this._selectedMod.configuration.directory, str + "/" + this._selectedMod.configuration.name, true);
              if (Directory.Exists(str + this._selectedMod.configuration.name + "/build"))
                Directory.Delete(str + this._selectedMod.configuration.name + "/build", true);
              if (Directory.Exists(str + this._selectedMod.configuration.name + "/.vs"))
                Directory.Delete(str + this._selectedMod.configuration.name + "/.vs", true);
              if (File.Exists(str + this._selectedMod.configuration.name + "/" + this._selectedMod.configuration.name + "_compiled.dll"))
              {
                string path2 = str + this._selectedMod.configuration.name + "/" + this._selectedMod.configuration.name + "_compiled.dll";
                File.SetAttributes(path2, FileAttributes.Normal);
                File.Delete(path2);
              }
              if (File.Exists(str + this._selectedMod.configuration.name + "/" + this._selectedMod.configuration.name + "_compiled.hash"))
              {
                string path2 = str + this._selectedMod.configuration.name + "/" + this._selectedMod.configuration.name + "_compiled.hash";
                File.SetAttributes(path2, FileAttributes.Normal);
                File.Delete(path2);
              }
              dat.contentFolder = str;
              this._transferItem.ApplyWorkshopData(dat);
              if (this._transferItem.needsLegal)
                Steam.ShowWorkshopLegalAgreement("312530");
              this._transferring = true;
              this._transferItem.ResetProcessing();
            }
          }
          else if (this._transferItem.finishedProcessing)
          {
            Steam.OverlayOpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=" + (object) this._transferItem.id);
            Directory.Delete(DuckFile.workshopDirectory + (object) this._transferItem.id + "/", true);
            this._transferItem.ResetProcessing();
            this._transferItem = (WorkshopItem) null;
            this._transferring = false;
          }
          base.Update();
          return;
        }
        if (this._gamepadMode)
        {
          if (this._hoverIndex < 0)
            this._hoverIndex = 0;
        }
        else
        {
          this._hoverIndex = -1;
          for (int index = 0; index < this._maxModsToShow && this._scrollItemOffset + index < this._mods.Count; ++index)
          {
            if (new Rectangle((float) (int) (this._box.x - this._box.halfWidth), (float) (int) (this._box.y - this._box.halfHeight + (float) (36 * index)), (float) ((int) this._box.width - 14), 36f).Contains(Mouse.position))
            {
              this._hoverIndex = this._scrollItemOffset + index;
              break;
            }
          }
        }
        if (this._transferItem != null)
        {
          if (this._updateTextBox != null)
          {
            Editor.hoverTextBox = false;
            this._updateTextBox.position = new Vec2((float) ((double) this._box.x - (double) this._box.halfWidth + 16.0), (float) ((double) this._box.y - (double) this._box.halfHeight + 48.0));
            this._updateTextBox.size = new Vec2(this._box.width - 32f, this._box.height - 80f);
            this._updateTextBox._maxLines = (int) ((double) this._updateTextBox.size.y / (double) this._fancyFont.characterHeight);
            this._updateTextBox.Update();
            float stringWidth = DuckGame.Graphics.GetStringWidth(this._updateButtonText, scale: 2f);
            float height = DuckGame.Graphics.GetStringHeight(this._updateButtonText) * 2f;
            this._updateButton = new Rectangle(this._box.x - stringWidth / 2f, (float) ((double) this._box.y + (double) this._box.halfHeight - 24.0), stringWidth, height);
            if (this._updateButton.Contains(Mouse.position) && Mouse.left == InputState.Pressed)
            {
              this._needsUpdateNotes = false;
              this._updateTextBox.LoseFocus();
            }
            else if (Keyboard.Pressed(Keys.Escape))
            {
              this._needsUpdateNotes = false;
              this._transferItem = (WorkshopItem) null;
              this._updateTextBox.LoseFocus();
              new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._editModMenu).Activate();
              return;
            }
          }
        }
        else if (this._hoverIndex != -1)
        {
          this._selectedMod = this._mods[this._hoverIndex];
          if (Input.Pressed("SHOOT"))
          {
            if (this._selectedMod != null && this._selectedMod.configuration != null)
            {
              if (this._selectedMod.configuration.disabled)
                this._selectedMod.configuration.Enable();
              else
                this._selectedMod.configuration.Disable();
              this.modsChanged = true;
              SFX.Play("rockHitGround", 0.8f);
            }
          }
          else if (Input.Pressed("SELECT") && this._pressWait == 0 && this._gamepadMode || !this._gamepadMode && Mouse.left == InputState.Pressed)
          {
            if (this._selectedMod != null)
            {
              this._editModMenu.title = this._selectedMod.configuration.loaded ? "|YELLOW|" + this._selectedMod.configuration.displayName : "|YELLOW|" + this._selectedMod.configuration.name;
              this._editModMenu.Remove((UIComponent) this._deleteOrUnsubItem);
              this._editModMenu.Remove((UIComponent) this._uploadItem);
              this._editModMenu.Remove((UIComponent) this._visitItem);
              if (!this._selectedMod.configuration.isWorkshop && this._selectedMod.configuration.loaded)
              {
                this._uploadItem.text = this._selectedMod.configuration.workshopID == 0UL ? "UPLOAD" : "UPDATE";
                this._editModMenu.Insert((UIComponent) this._uploadItem, 1, true);
              }
              if (!this._selectedMod.configuration.isWorkshop && !this._selectedMod.configuration.loaded)
              {
                this._deleteOrUnsubItem.text = "DELETE";
                this._editModMenu.Insert((UIComponent) this._deleteOrUnsubItem, 1, true);
              }
              else if (this._selectedMod.configuration.isWorkshop)
              {
                this._deleteOrUnsubItem.text = "UNSUBSCRIBE";
                this._editModMenu.Insert((UIComponent) this._deleteOrUnsubItem, 1, true);
              }
              if (this._selectedMod.configuration.isWorkshop)
                this._editModMenu.Insert((UIComponent) this._visitItem, 1, true);
              this._disableOrEnableItem.text = this._selectedMod.configuration.disabled ? "ENABLE" : "DISABLE";
              this._editModMenu.dirty = true;
              SFX.Play("rockHitGround", 0.8f);
              new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._editModMenu).Activate();
              return;
            }
            Steam.OverlayOpenURL("http://steamcommunity.com/workshop/browse/?appid=312530&searchtext=&childpublishedfileid=0&browsesort=trend&section=readytouseitems&requiredtags%5B%5D=Mod");
          }
        }
        else
          this._selectedMod = (Mod) null;
        if (this._gamepadMode)
        {
          this._draggingScrollbar = false;
          if (Input.Pressed("DOWN"))
            ++this._hoverIndex;
          else if (Input.Pressed("UP"))
            --this._hoverIndex;
          if (Input.Pressed("STRAFE"))
            this._hoverIndex -= 10;
          else if (Input.Pressed("RAGDOLL"))
            this._hoverIndex += 10;
          if (this._hoverIndex < 0)
            this._hoverIndex = 0;
          if ((double) (this._oldPos - Mouse.positionScreen).lengthSq > 200.0)
            this._gamepadMode = false;
        }
        else
        {
          if (!this._draggingScrollbar)
          {
            if (Mouse.left == InputState.Pressed && this.ScrollBarBox().Contains(Mouse.position))
            {
              this._draggingScrollbar = true;
              this._oldPos = Mouse.position;
            }
            if ((double) Mouse.scroll > 0.0)
            {
              this._scrollItemOffset += 5;
              this._hoverIndex += 5;
            }
            else if ((double) Mouse.scroll < 0.0)
            {
              this._scrollItemOffset -= 5;
              this._hoverIndex -= 5;
              if (this._hoverIndex < 0)
                this._hoverIndex = 0;
            }
          }
          else if (Mouse.left != InputState.Down)
          {
            this._draggingScrollbar = false;
          }
          else
          {
            Vec2 vec2 = Mouse.position - this._oldPos;
            this._oldPos = Mouse.position;
            this.scrollBarOffset += (int) vec2.y;
            if (this.scrollBarOffset > this.scrollBarScrollableHeight)
              this.scrollBarOffset = this.scrollBarScrollableHeight;
            else if (this.scrollBarOffset < 0)
              this.scrollBarOffset = 0;
            this._scrollItemOffset = (int) ((double) (this._mods.Count - this._maxModsToShow) * (double) ((float) this.scrollBarOffset / (float) this.scrollBarScrollableHeight));
          }
          if (Input.Pressed("ANY"))
          {
            this._gamepadMode = true;
            this._oldPos = Mouse.positionScreen;
          }
        }
        if (this._scrollItemOffset < 0)
          this._scrollItemOffset = 0;
        else if (this._scrollItemOffset > Math.Max(0, this._mods.Count - this._maxModsToShow))
          this._scrollItemOffset = Math.Max(0, this._mods.Count - this._maxModsToShow);
        if (this._hoverIndex >= this._mods.Count)
          this._hoverIndex = this._mods.Count - 1;
        else if (this._hoverIndex >= this._scrollItemOffset + this._maxModsToShow)
          this._scrollItemOffset += this._hoverIndex - (this._scrollItemOffset + this._maxModsToShow) + 1;
        else if (this._hoverIndex >= 0 && this._hoverIndex < this._scrollItemOffset)
          this._scrollItemOffset -= this._scrollItemOffset - this._hoverIndex;
        this.scrollBarOffset = this._scrollItemOffset == 0 ? 0 : (int) Lerp.FloatSmooth(0.0f, (float) this.scrollBarScrollableHeight, (float) this._scrollItemOffset / (float) (this._mods.Count - this._maxModsToShow));
        if (!Editor.hoverTextBox && !UIMenu.globalUILock && (Input.Pressed("QUACK") || Keyboard.Pressed(Keys.Escape)))
        {
          if (this.modsChanged)
          {
            this.Close();
            MonoMain.pauseMenu = DuckNetwork.OpenModsRestartWindow(this._openOnClose);
          }
          else
            new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._openOnClose).Activate();
          this.modsChanged = false;
          return;
        }
      }
      if (this._showingMenu)
      {
        HUD.CloseAllCorners();
        this._showingMenu = false;
      }
      base.Update();
    }

    private Rectangle ScrollBarBox() => new Rectangle((float) ((double) this._box.x + (double) this._box.halfWidth - 12.0 + 1.0), (float) ((double) this._box.y - (double) this._box.halfHeight + 1.0) + (float) this.scrollBarOffset, 10f, 32f);

    public override void Draw()
    {
      if (this.open)
      {
        this.scrollBarTop = (int) ((double) this._box.y - (double) this._box.halfHeight + 1.0 + 16.0);
        this.scrollBarBottom = (int) ((double) this._box.y + (double) this._box.halfHeight - 1.0 - 16.0);
        this.scrollBarScrollableHeight = this.scrollBarBottom - this.scrollBarTop;
        if (this.fixView)
        {
          Layer.HUD.camera.width *= 2f;
          Layer.HUD.camera.height *= 2f;
          this.fixView = false;
        }
        DuckGame.Graphics.DrawRect(new Vec2(this._box.x - this._box.halfWidth, this._box.y - this._box.halfHeight), new Vec2((float) ((double) this._box.x + (double) this._box.halfWidth - 12.0 - 2.0), this._box.y + this._box.halfHeight), Color.Black, new Depth(0.4f));
        DuckGame.Graphics.DrawRect(new Vec2((float) ((double) this._box.x + (double) this._box.halfWidth - 12.0), this._box.y - this._box.halfHeight), new Vec2(this._box.x + this._box.halfWidth, this._box.y + this._box.halfHeight), Color.Black, new Depth(0.4f));
        Rectangle r = this.ScrollBarBox();
        DuckGame.Graphics.DrawRect(r, this._draggingScrollbar || r.Contains(Mouse.position) ? Color.LightGray : Color.Gray, new Depth(0.5f));
        for (int index1 = 0; index1 < this._maxModsToShow; ++index1)
        {
          int index2 = this._scrollItemOffset + index1;
          if (index2 < this._mods.Count)
          {
            float x = this._box.x - this._box.halfWidth;
            float y = this._box.y - this._box.halfHeight + (float) (36 * index1);
            if (this._transferItem == null && this._hoverIndex == index2)
              DuckGame.Graphics.DrawRect(new Vec2(x, y), new Vec2((float) ((double) x + (double) this._box.width - 14.0), y + 36f), Color.White * 0.6f, new Depth(0.4f));
            else if ((index2 & 1) != 0)
              DuckGame.Graphics.DrawRect(new Vec2(x, y), new Vec2((float) ((double) x + (double) this._box.width - 14.0), y + 36f), Color.White * 0.1f, new Depth(0.4f));
            Mod mod = this._mods[index2];
            if (mod != null)
            {
              Tex2D previewTexture = mod.previewTexture;
              if (this._noImage.texture != previewTexture)
              {
                this._noImage.texture = previewTexture;
                this._noImage.scale = new Vec2(32f / (float) previewTexture.width);
              }
              DuckGame.Graphics.DrawRect(new Vec2(x + 2f, y + 2f), new Vec2((float) ((double) x + 36.0 - 2.0), (float) ((double) y + 36.0 - 2.0)), Color.Gray, new Depth(0.5f), false, 2f);
              DuckGame.Graphics.Draw(this._noImage, x + 2f, y + 2f, new Depth(0.5f));
              string str = "#" + (object) (index2 + 1) + ": ";
              string text;
              if (!mod.configuration.loaded)
                text = str + mod.configuration.name;
              else
                text = str + mod.configuration.displayName + "|WHITE| v" + mod.configuration.version.ToString() + " by |PURPLE|" + mod.configuration.author;
              this._fancyFont.Draw(text, new Vec2((float) ((double) x + 36.0 + 10.0), y + 2f), Color.Yellow, new Depth(0.5f));
              DuckGame.Graphics.Draw(!mod.configuration.isWorkshop ? (Sprite) this._localIcon : this._steamIcon, x + 36f, y + 2.5f, new Depth(0.5f));
              if (!mod.configuration.loaded)
              {
                if (mod.configuration.disabled)
                  this._fancyFont.Draw("Mod is disabled.", new Vec2(x + 36f, y + 6f + (float) this._fancyFont.characterHeight), Color.LightGray, new Depth(0.5f));
                else
                  this._fancyFont.Draw("|DGGREEN|Mod will be enabled on next restart.", new Vec2(x + 36f, y + 6f + (float) this._fancyFont.characterHeight), Color.Orange, new Depth(0.5f));
              }
              else if (mod.configuration.disabled)
                this._fancyFont.Draw("|DGRED|Mod will be disabled on next restart.", new Vec2(x + 36f, y + 6f + (float) this._fancyFont.characterHeight), Color.Orange, new Depth(0.5f));
              else
                this._fancyFont.Draw(mod.configuration.description, new Vec2(x + 36f, y + 6f + (float) this._fancyFont.characterHeight), Color.White, new Depth(0.5f));
            }
            else
            {
              DuckGame.Graphics.Draw((Sprite) this._newIcon, x + 2f, y + 1f, new Depth(0.5f));
              this._fancyFont.scale = new Vec2(1.5f);
              this._fancyFont.Draw("Get " + (this._mods.Count == 1 ? "some" : "more") + " mods!", new Vec2(x + 36f, y + 11f), Color.White, new Depth(0.5f));
              this._fancyFont.scale = new Vec2(1f);
            }
          }
          else
            break;
        }
        if (this._awaitingChanges)
          DuckGame.Graphics.DrawString("Restart required for some changes to take effect!", new Vec2((float) ((double) this.x - (double) this.halfWidth + 128.0), (float) ((double) this.y - (double) this.halfHeight + 8.0)), Color.Red, new Depth(0.6f));
        if (this._transferItem != null)
        {
          DuckGame.Graphics.DrawRect(new Rectangle(this._box.x - this._box.halfWidth, this._box.y - this._box.halfHeight, this._box.width, this._box.height), Color.Black * 0.9f, new Depth(0.7f));
          string text = "Creating item...";
          if (this._transferring)
          {
            TransferProgress uploadProgress = this._transferItem.GetUploadProgress();
            string str;
            switch (uploadProgress.status)
            {
              case ItemUpdateStatus.PreparingConfig:
                str = "Preparing config";
                break;
              case ItemUpdateStatus.PreparingContent:
                str = "Preparing content";
                break;
              case ItemUpdateStatus.UploadingContent:
                str = "Uploading content";
                break;
              case ItemUpdateStatus.UploadingPreviewFile:
                str = "Uploading preview";
                break;
              case ItemUpdateStatus.CommittingChanges:
                str = "Committing changes";
                break;
              default:
                str = "Waiting";
                break;
            }
            if (uploadProgress.bytesTotal != 0UL)
            {
              float amount = (float) uploadProgress.bytesDownloaded / (float) uploadProgress.bytesTotal;
              str = str + " (" + (object) (int) ((double) amount * 100.0) + "%)";
              DuckGame.Graphics.DrawRect(new Rectangle((float) ((double) this._box.x - (double) this._box.halfWidth + 8.0), this._box.y - 8f, this._box.width - 16f, 16f), Color.LightGray, new Depth(0.8f));
              DuckGame.Graphics.DrawRect(new Rectangle((float) ((double) this._box.x - (double) this._box.halfWidth + 8.0), this._box.y - 8f, Lerp.FloatSmooth(0.0f, this._box.width - 16f, amount), 16f), Color.Green, new Depth(0.8f));
            }
            text = str + "...";
          }
          else if (this._needsUpdateNotes)
          {
            DuckGame.Graphics.DrawRect(new Rectangle(this._updateTextBox.position.x - 1f, this._updateTextBox.position.y - 1f, this._updateTextBox.size.x + 2f, this._updateTextBox.size.y + 2f), Color.Gray, new Depth(0.85f), false);
            DuckGame.Graphics.DrawRect(new Rectangle(this._updateTextBox.position.x, this._updateTextBox.position.y, this._updateTextBox.size.x, this._updateTextBox.size.y), Color.Black, new Depth(0.85f));
            this._updateTextBox.Draw();
            text = "Enter change notes:";
            DuckGame.Graphics.DrawString(this._updateButtonText, new Vec2(this._updateButton.x, this._updateButton.y), this._updateButton.Contains(Mouse.position) ? Color.Yellow : Color.White, new Depth(0.9f), scale: 2f);
          }
          float stringWidth = DuckGame.Graphics.GetStringWidth(text, scale: 2f);
          DuckGame.Graphics.DrawString(text, new Vec2(this._box.x - stringWidth / 2f, (float) ((double) this._box.y - (double) this._box.halfHeight + 24.0)), Color.White, new Depth(0.8f), scale: 2f);
        }
        if (Mouse.available && !this._gamepadMode)
        {
          this._cursor.depth = new Depth(1f);
          this._cursor.scale = new Vec2(1f, 1f);
          this._cursor.position = Mouse.position;
          this._cursor.frame = 0;
          if (Editor.hoverTextBox)
          {
            this._cursor.frame = 5;
            this._cursor.position.y -= 4f;
            this._cursor.scale = new Vec2(0.5f, 1f);
          }
          this._cursor.Draw();
        }
      }
      base.Draw();
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
      public IntPtr hwnd;
      [MarshalAs(UnmanagedType.U4)]
      public int wFunc;
      public string pFrom;
      public string pTo;
      public short fFlags;
      [MarshalAs(UnmanagedType.Bool)]
      public bool fAnyOperationsAborted;
      public IntPtr hNameMappings;
      public string lpszProgressTitle;
    }
  }
}
