// Decompiled with JetBrains decompiler
// Type: DuckGame.UIServerBrowser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace DuckGame
{
  public class UIServerBrowser : UIMenu
  {
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
    private int _hoverIndex;
    private UIBox _box;
    private FancyBitmapFont _fancyFont;
    private int _maxLobbiesToShow;
    public UIMenu _editModMenu;
    public UIMenu _yesNoMenu;
    private UIMenuItem _yesNoYes;
    private UIMenuItem _yesNoNo;
    private Textbox _updateTextBox;
    private int _pressWait;
    private Tex2D defaultImage;
    private UIMenu _downloadModsMenu;
    private bool _showingMenu;
    private bool _draggingScrollbar;
    private Vec2 _oldPos;
    public static UIServerBrowser.LobbyData _selectedLobby;
    private bool _doLobbySearch = true;
    private List<UIServerBrowser.LobbyData> _lobbies = new List<UIServerBrowser.LobbyData>();
    private bool fixView = true;
    private int scrollBarTop;
    private int scrollBarBottom;
    private int scrollBarScrollableHeight;
    private int scrollBarOffset;
    private int _scrollItemOffset;
    private bool _gamepadMode = true;
    private static Dictionary<ulong, Tex2D> _previewMap = new Dictionary<ulong, Tex2D>();
    private static Dictionary<object, ulong> _clientMap = new Dictionary<object, ulong>();

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

    public static void SubscribeAndRestart()
    {
      foreach (Mod allMod in (IEnumerable<Mod>) ModLoader.allMods)
        allMod.configuration.disabled = true;
      if (ConnectionError.joinLobby != null)
      {
        UIServerBrowser._selectedLobby = new UIServerBrowser.LobbyData();
        UIServerBrowser._selectedLobby.lobby = ConnectionError.joinLobby;
        string lobbyData = ConnectionError.joinLobby.GetLobbyData("mods");
        if (lobbyData != null && lobbyData != "")
        {
          string str1 = lobbyData;
          char[] chArray = new char[1]{ '|' };
          foreach (string str2 in str1.Split(chArray))
          {
            if (str2 != "LOCAL")
            {
              WorkshopItem workshopItem = WorkshopItem.GetItem(Convert.ToUInt64(str2));
              UIServerBrowser._selectedLobby.workshopItems.Add(workshopItem);
            }
          }
        }
      }
      using (List<WorkshopItem>.Enumerator enumerator = UIServerBrowser._selectedLobby.workshopItems.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          WorkshopItem w = enumerator.Current;
          Mod mod = ModLoader.allMods.FirstOrDefault<Mod>((Func<Mod, bool>) (x => (long) x.configuration.workshopID == (long) w.id));
          if (mod != null)
            mod.configuration.disabled = false;
          Steam.WorkshopSubscribe(w.id);
        }
      }
      Program.commandLine = Program.commandLine + " -downloadmods +connect_lobby " + (object) UIServerBrowser._selectedLobby.lobby.id;
      ModLoader.DisabledModsChanged();
      ModLoader.RestartGame();
    }

    public UIServerBrowser(
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
      this.defaultImage = Content.Load<Tex2D>("server_default");
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
      this._maxLobbiesToShow = 8;
      this._box = new UIBox(0.0f, 0.0f, high: ((float) (this._maxLobbiesToShow * 36)), isVisible: false);
      this.Add((UIComponent) this._box, true);
      this._fancyFont = new FancyBitmapFont("smallFont");
      this._fancyFont.maxWidth = (int) this.width - 100;
      this._fancyFont.maxRows = 2;
      this.scrollBarOffset = 0;
      this._editModMenu = new UIMenu("<mod name>", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._editModMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._editModMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._editModMenu, (UIComponent) this)), true);
      this._editModMenu.Close();
      this._yesNoMenu = new UIMenu("ARE YOU SURE?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._yesNoMenu.Add((UIComponent) (this._yesNoYes = new UIMenuItem("YES")), true);
      this._yesNoMenu.Add((UIComponent) (this._yesNoNo = new UIMenuItem("NO")), true);
      this._yesNoMenu.Close();
      this._updateTextBox = new Textbox(0.0f, 0.0f, 0.0f, 0.0f);
      this._updateTextBox.depth = (Depth) 0.9f;
      this._updateTextBox.maxLength = 5000;
      this._downloadModsMenu = new UIMenu("MODS REQUIRED!", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 290f, conString: "@SELECT@SELECT");
      this._downloadModsMenu.Add((UIComponent) new UIText("You're missing the mods required", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIText("to join this game. Would you", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIText("like to automatically subscribe to", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIText("all required mods, restart and", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIText("join the game?", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIText("", Colors.DGBlue), true);
      this._downloadModsMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._downloadModsMenu, (UIComponent) this)), true);
      this._downloadModsMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuCallFunction((UIComponent) this._downloadModsMenu, new UIMenuActionCloseMenuCallFunction.Function(UIServerBrowser.SubscribeAndRestart))), true);
      this._downloadModsMenu.Close();
    }

    public override void Open()
    {
      UIServerBrowser._selectedLobby = (UIServerBrowser.LobbyData) null;
      this._pressWait = 30;
      base.Open();
      DevConsole.SuppressDevConsole();
      this._oldPos = Mouse.positionScreen;
      this.RefreshLobbySearch();
    }

    public void RefreshLobbySearch()
    {
      this._lobbies.Clear();
      UIServerBrowser._selectedLobby = (UIServerBrowser.LobbyData) null;
      this._doLobbySearch = true;
    }

    public override void Update()
    {
      if (this.open)
      {
        if (this._doLobbySearch && this._lobbies.Count == 0)
        {
          this._lobbies.Clear();
          this._doLobbySearch = false;
          Steam.SearchForLobby((User) null);
        }
        if (!this._doLobbySearch && this._lobbies.Count == 0 && Steam.lobbySearchComplete)
        {
          int lobbiesFound = Steam.lobbiesFound;
          List<WorkshopItem> items = new List<WorkshopItem>();
          for (int index = 0; index < lobbiesFound; ++index)
          {
            Lobby searchLobbyAtIndex = Steam.GetSearchLobbyAtIndex(index);
            UIServerBrowser.LobbyData lobbyData1 = new UIServerBrowser.LobbyData();
            lobbyData1.lobby = searchLobbyAtIndex;
            string str1 = searchLobbyAtIndex.GetLobbyData("name");
            if (str1 == null || str1 == "")
              str1 = "DG Lobby";
            lobbyData1.name = str1;
            lobbyData1.modHash = searchLobbyAtIndex.GetLobbyData("modhash");
            lobbyData1.requiredWins = searchLobbyAtIndex.GetLobbyData("requiredwins");
            lobbyData1.restsEvery = searchLobbyAtIndex.GetLobbyData("restsevery");
            lobbyData1.wallMode = searchLobbyAtIndex.GetLobbyData("wallmode");
            lobbyData1.customLevels = searchLobbyAtIndex.GetLobbyData("customLevels");
            lobbyData1.version = searchLobbyAtIndex.GetLobbyData("version");
            lobbyData1.started = searchLobbyAtIndex.GetLobbyData("started");
            lobbyData1.type = searchLobbyAtIndex.GetLobbyData("type");
            lobbyData1.numSlots = searchLobbyAtIndex.GetLobbyData("numSlots");
            string lobbyData2 = searchLobbyAtIndex.GetLobbyData("mods");
            if (lobbyData2 != null && lobbyData2 != "")
            {
              string str2 = lobbyData2;
              char[] chArray = new char[1]{ '|' };
              foreach (string str3 in str2.Split(chArray))
              {
                if (str3 == "LOCAL")
                {
                  lobbyData1.hasLocalMods = true;
                }
                else
                {
                  WorkshopItem workshopItem = WorkshopItem.GetItem(Convert.ToUInt64(str3));
                  items.Add(workshopItem);
                  lobbyData1.workshopItems.Add(workshopItem);
                }
              }
            }
            lobbyData1.maxPlayers = searchLobbyAtIndex.GetLobbyData("maxplayers");
            this._lobbies.Add(lobbyData1);
          }
          if (items.Count > 0)
            Steam.RequestWorkshopInfo(items);
          this._doLobbySearch = true;
        }
      }
      if (this._pressWait > 0)
        --this._pressWait;
      if (this._downloadModsMenu.open)
      {
        this._downloadModsMenu.DoUpdate();
        if (!UIMenu.globalUILock && (Input.Pressed("QUACK") || Keyboard.Pressed(Keys.Escape)))
        {
          this._downloadModsMenu.Close();
          this.Open();
          return;
        }
      }
      else if (this.open)
      {
        if (this._gamepadMode)
        {
          if (this._hoverIndex < 0)
            this._hoverIndex = 0;
        }
        else
        {
          this._hoverIndex = -1;
          for (int index = 0; index < this._maxLobbiesToShow && this._scrollItemOffset + index < this._lobbies.Count; ++index)
          {
            if (new Rectangle((float) (int) (this._box.x - this._box.halfWidth), (float) (int) (this._box.y - this._box.halfHeight + (float) (36 * index)), (float) ((int) this._box.width - 14), 36f).Contains(Mouse.position))
            {
              this._hoverIndex = this._scrollItemOffset + index;
              break;
            }
          }
        }
        if (this._hoverIndex != -1)
        {
          if (Input.Pressed("SHOOT"))
          {
            this.RefreshLobbySearch();
            SFX.Play("rockHitGround", 0.8f);
          }
          if (this._lobbies.Count > 0)
          {
            UIServerBrowser._selectedLobby = this._lobbies[this._hoverIndex];
            if (Input.Pressed("SELECT") && this._pressWait == 0 && this._gamepadMode || !this._gamepadMode && Mouse.left == InputState.Pressed)
            {
              if (!UIServerBrowser._selectedLobby.canJoin)
              {
                SFX.Play("consoleError");
              }
              else
              {
                SFX.Play("consoleSelect");
                if (UIServerBrowser._selectedLobby.workshopItems.Count == 0 || UIServerBrowser._selectedLobby.hasFirstMod && UIServerBrowser._selectedLobby.hasRestOfMods)
                {
                  MonoMain.pauseMenu = (UIComponent) null;
                  MonoMain.closeMenuUpdate.Clear();
                  Level.current = (Level) new JoinServer(UIServerBrowser._selectedLobby.lobby.id);
                  return;
                }
                new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._downloadModsMenu).Activate();
              }
            }
          }
        }
        else
          UIServerBrowser._selectedLobby = (UIServerBrowser.LobbyData) null;
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
            this._scrollItemOffset = (int) ((double) (this._lobbies.Count - this._maxLobbiesToShow) * (double) ((float) this.scrollBarOffset / (float) this.scrollBarScrollableHeight));
          }
          if (Input.Pressed("ANY"))
          {
            this._gamepadMode = true;
            this._oldPos = Mouse.positionScreen;
          }
        }
        if (this._scrollItemOffset < 0)
          this._scrollItemOffset = 0;
        else if (this._scrollItemOffset > Math.Max(0, this._lobbies.Count - this._maxLobbiesToShow))
          this._scrollItemOffset = Math.Max(0, this._lobbies.Count - this._maxLobbiesToShow);
        if (this._hoverIndex >= this._lobbies.Count)
          this._hoverIndex = this._lobbies.Count - 1;
        else if (this._hoverIndex >= this._scrollItemOffset + this._maxLobbiesToShow)
          this._scrollItemOffset += this._hoverIndex - (this._scrollItemOffset + this._maxLobbiesToShow) + 1;
        else if (this._hoverIndex >= 0 && this._hoverIndex < this._scrollItemOffset)
          this._scrollItemOffset -= this._scrollItemOffset - this._hoverIndex;
        this.scrollBarOffset = this._scrollItemOffset == 0 ? 0 : (int) Lerp.FloatSmooth(0.0f, (float) this.scrollBarScrollableHeight, (float) this._scrollItemOffset / (float) (this._lobbies.Count - this._maxLobbiesToShow));
        if (!Editor.hoverTextBox && !UIMenu.globalUILock && (Input.Pressed("QUACK") || Keyboard.Pressed(Keys.Escape)))
        {
          new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._openOnClose).Activate();
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

    private void Completed(object sender, AsyncCompletedEventArgs e)
    {
      if (!UIServerBrowser._clientMap.ContainsKey(sender))
        return;
      ulong client = UIServerBrowser._clientMap[sender];
      UIServerBrowser._clientMap.Remove(sender);
      if (!UIServerBrowser._previewMap.ContainsKey(client))
        return;
      Texture2D texture2D = ContentPack.LoadTexture2D(this.PreviewPathForWorkshopItem(client), false);
      if (texture2D == null)
        return;
      Tex2D tex2D = (Tex2D) texture2D;
      if (tex2D == null)
        return;
      UIServerBrowser._previewMap[client] = tex2D;
    }

    public string PreviewPathForWorkshopItem(ulong id) => DuckFile.workshopDirectory + "/modPreview" + id.ToString() + "preview.png";

    public override void Draw()
    {
      if (this._downloadModsMenu.open)
        this._downloadModsMenu.DoDraw();
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
        DuckGame.Graphics.DrawRect(new Vec2(this._box.x - this._box.halfWidth, this._box.y - this._box.halfHeight), new Vec2((float) ((double) this._box.x + (double) this._box.halfWidth - 12.0 - 2.0), this._box.y + this._box.halfHeight), Color.Black, (Depth) 0.4f);
        DuckGame.Graphics.DrawRect(new Vec2((float) ((double) this._box.x + (double) this._box.halfWidth - 12.0), this._box.y - this._box.halfHeight), new Vec2(this._box.x + this._box.halfWidth, this._box.y + this._box.halfHeight), Color.Black, (Depth) 0.4f);
        Rectangle r = this.ScrollBarBox();
        DuckGame.Graphics.DrawRect(r, this._draggingScrollbar || r.Contains(Mouse.position) ? Color.LightGray : Color.Gray, (Depth) 0.5f);
        if (this._lobbies.Count == 0)
          this._fancyFont.Draw("No games found!", new Vec2(this._box.x - this._box.halfWidth + 10f, (float) ((double) this._box.y - (double) this._box.halfHeight + 0.0) + 2f), Color.Yellow, (Depth) 0.5f);
        this._lobbies = this._lobbies.OrderByDescending<UIServerBrowser.LobbyData, bool>((Func<UIServerBrowser.LobbyData, bool>) (x => x.canJoin)).ToList<UIServerBrowser.LobbyData>();
        for (int index1 = 0; index1 < this._maxLobbiesToShow; ++index1)
        {
          int index2 = this._scrollItemOffset + index1;
          if (index2 < this._lobbies.Count)
          {
            float x1 = this._box.x - this._box.halfWidth;
            float y = this._box.y - this._box.halfHeight + (float) (36 * index1);
            if (this._hoverIndex == index2)
              DuckGame.Graphics.DrawRect(new Vec2(x1, y), new Vec2((float) ((double) x1 + (double) this._box.width - 14.0), y + 36f), Color.White * 0.6f, (Depth) 0.4f);
            else if ((index2 & 1) != 0)
              DuckGame.Graphics.DrawRect(new Vec2(x1, y), new Vec2((float) ((double) x1 + (double) this._box.width - 14.0), y + 36f), Color.White * 0.1f, (Depth) 0.4f);
            UIServerBrowser.LobbyData lobby = this._lobbies[index2];
            if (lobby != null)
            {
              this._noImage.texture = this.defaultImage;
              this._noImage.scale = new Vec2(1f, 1f);
              List<Tex2D> tex2DList = new List<Tex2D>();
              string name = lobby.name;
              string text1 = "|WHITE||GRAY|\n";
              if (lobby.workshopItems.Count > 0)
              {
                WorkshopItem workshopItem1 = lobby.workshopItems[0];
                if (workshopItem1.data != null)
                {
                  lobby.workshopItems = lobby.workshopItems.OrderByDescending<WorkshopItem, int>((Func<WorkshopItem, int>) (x => x.data == null ? 0 : x.data.votesUp)).ToList<WorkshopItem>();
                  if (!lobby.downloadedWorkshopItems)
                  {
                    lobby.hasFirstMod = true;
                    lobby.hasRestOfMods = true;
                    bool flag = true;
                    foreach (WorkshopItem workshopItem2 in lobby.workshopItems)
                    {
                      ulong id = workshopItem2.id;
                      if (ModLoader.accessibleMods.FirstOrDefault<Mod>((Func<Mod, bool>) (x => (long) x.configuration.workshopID == (long) id)) == null)
                      {
                        if (flag)
                          lobby.hasFirstMod = false;
                        else
                          lobby.hasRestOfMods = false;
                      }
                      flag = false;
                    }
                    lobby.downloadedWorkshopItems = true;
                  }
                  string str1 = !lobby.hasFirstMod ? "|RED|Requires " + workshopItem1.name : "|DGGREEN|Requires " + workshopItem1.name;
                  string str2 = lobby.hasRestOfMods ? "|DGGREEN|" : "|RED|";
                  if (lobby.workshopItems.Count == 2)
                    str1 = str1 + str2 + " +" + (lobby.workshopItems.Count - 1).ToString() + " other mod.";
                  else if (lobby.workshopItems.Count > 2)
                    str1 = str1 + str2 + " +" + (lobby.workshopItems.Count - 1).ToString() + " other mods.";
                  text1 = str1 + "\n|GRAY|";
                  if (!UIServerBrowser._previewMap.ContainsKey(workshopItem1.id))
                  {
                    if (workshopItem1.data.previewPath != null)
                    {
                      if (workshopItem1.data.previewPath != "")
                      {
                        try
                        {
                          WebClient webClient = new WebClient();
                          string str3 = this.PreviewPathForWorkshopItem(workshopItem1.id);
                          DuckFile.CreatePath(str3);
                          if (System.IO.File.Exists(str3))
                            DuckFile.Delete(str3);
                          webClient.DownloadFileAsync(new Uri(workshopItem1.data.previewPath), str3);
                          webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Completed);
                          UIServerBrowser._clientMap[(object) webClient] = workshopItem1.id;
                        }
                        catch (Exception ex)
                        {
                        }
                      }
                    }
                    UIServerBrowser._previewMap[workshopItem1.id] = (Tex2D) null;
                  }
                  else
                  {
                    Tex2D preview = UIServerBrowser._previewMap[workshopItem1.id];
                    if (preview != null)
                      tex2DList.Add(preview);
                  }
                }
              }
              if (lobby.wallMode == "1")
                text1 += "Wall Mode. ";
              if (lobby.requiredWins != "")
                text1 = text1 + "First to " + lobby.requiredWins.ToString() + " ";
              if (lobby.restsEvery != "")
                text1 = text1 + "rests every " + lobby.restsEvery.ToString() + ". ";
              if (lobby.customLevels != "" && lobby.customLevels != "0")
                text1 = text1 + lobby.customLevels.ToString() + " Custom Levels. ";
              DuckGame.Graphics.DrawRect(new Vec2(x1 + 2f, y + 2f), new Vec2((float) ((double) x1 + 36.0 - 2.0), (float) ((double) y + 36.0 - 2.0)), Color.Gray, (Depth) 0.5f, false, 2f);
              if (tex2DList.Count > 0)
              {
                Vec2 zero = Vec2.Zero;
                for (int index3 = 0; index3 < 4; ++index3)
                {
                  if (index3 < tex2DList.Count)
                  {
                    this._noImage.texture = tex2DList[index3];
                    if (tex2DList.Count > 1)
                      this._noImage.scale = new Vec2(16f / (float) this._noImage.texture.width);
                    else
                      this._noImage.scale = new Vec2(32f / (float) this._noImage.texture.width);
                    if (this._noImage.texture.width != this._noImage.texture.height)
                    {
                      if (this._noImage.texture.width > this._noImage.texture.height)
                      {
                        this._noImage.scale = new Vec2(32f / (float) this._noImage.texture.height);
                        DuckGame.Graphics.Draw(this._noImage, x1 + 2f + zero.x, y + 2f + zero.y, new Rectangle((float) (this._noImage.texture.width / 2 - this._noImage.texture.height / 2), 0.0f, (float) this._noImage.texture.height, (float) this._noImage.texture.height), (Depth) 0.5f);
                      }
                      else
                        DuckGame.Graphics.Draw(this._noImage, x1 + 2f + zero.x, y + 2f + zero.y, new Rectangle(0.0f, 0.0f, (float) this._noImage.texture.width, (float) this._noImage.texture.width), (Depth) 0.5f);
                    }
                    else
                      DuckGame.Graphics.Draw(this._noImage, x1 + 2f + zero.x, y + 2f + zero.y, (Depth) 0.5f);
                    zero.x += 16f;
                    if ((double) zero.x >= 32.0)
                    {
                      zero.x = 0.0f;
                      zero.y += 16f;
                    }
                  }
                }
              }
              else
                DuckGame.Graphics.Draw(this._noImage, x1 + 2f, y + 2f, (Depth) 0.5f);
              string text2 = name;
              if (lobby.maxPlayers != "")
                text2 = text2 + " (" + lobby.lobby.users.Count.ToString() + "/" + lobby.numSlots.ToString() + ")";
              if (!lobby.canJoin)
              {
                string str = text2 + " |DGRED|(";
                if (lobby.version != DG.version)
                {
                  switch (DuckNetwork.CheckVersion(lobby.version))
                  {
                    case NMVersionMismatch.Type.Older:
                      str += "They have an older version.";
                      break;
                    case NMVersionMismatch.Type.Newer:
                      str += "They have a newer version.";
                      break;
                    default:
                      str += "They have a different version.";
                      break;
                  }
                }
                else if (lobby.started == "true")
                  str += "This game is in progress.";
                else if (lobby.numSlots != "" && lobby.lobby.users.Count >= Convert.ToInt32(lobby.numSlots))
                  str += "Lobby is full.";
                else if (lobby.type != "2")
                  str += "This game is not public.";
                else if (lobby.hasLocalMods)
                  str += "This game is using non-workshop mods.";
                text2 = str + ")";
                DuckGame.Graphics.DrawRect(new Vec2(x1, y), new Vec2((float) ((double) x1 + (double) this._box.width - 14.0), y + 36f), Color.Black * 0.5f, (Depth) 0.99f);
              }
              this._fancyFont.maxWidth = 1000;
              this._fancyFont.Draw(text2, new Vec2((float) ((double) x1 + 36.0 + 10.0), y + 2f), Color.Yellow, (Depth) 0.5f);
              if (lobby.version == DG.version)
                this._fancyFont.Draw(lobby.version, new Vec2((float) ((double) x1 + 430.0 + 10.0), y + 2f), Colors.DGGreen * 0.35f, (Depth) 0.5f);
              else
                this._fancyFont.Draw(lobby.version, new Vec2((float) ((double) x1 + 430.0 + 10.0), y + 2f), Colors.DGRed * 0.35f, (Depth) 0.5f);
              DuckGame.Graphics.Draw(this._steamIcon, x1 + 36f, y + 2.5f, (Depth) 0.5f);
              this._fancyFont.Draw(text1, new Vec2(x1 + 36f, y + 6f + (float) this._fancyFont.characterHeight), Color.LightGray, (Depth) 0.5f);
            }
          }
          else
            break;
        }
        if (Mouse.available && !this._gamepadMode)
        {
          this._cursor.depth = (Depth) 1f;
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

    public class LobbyData
    {
      public string name;
      public string maxPlayers;
      public string restsEvery;
      public string requiredWins;
      public string wallMode;
      public string customLevels;
      public string version;
      public string started;
      public string type;
      public string numSlots;
      public string modHash;
      public bool hasLocalMods;
      public bool hasFirstMod;
      public bool hasRestOfMods;
      public bool downloadedWorkshopItems;
      public Lobby lobby;
      public List<WorkshopItem> workshopItems = new List<WorkshopItem>();

      public bool canJoin => DG.version == this.version && this.started == "false" && (!this.hasLocalMods || ModLoader.modHash == this.modHash) && (this.numSlots != "" && this.lobby.users.Count < Convert.ToInt32(this.numSlots)) && this.type == "2";
    }
  }
}
