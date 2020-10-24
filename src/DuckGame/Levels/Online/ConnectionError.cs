// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectionError
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ConnectionError : Level
  {
    private string _text;
    public static Lobby joinLobby;
    private UIMenu _downloadModsMenu;

    public ConnectionError(string text)
    {
      this._text = text;
      this._centeredView = true;
    }

    public override void Initialize()
    {
      if (this._text == "INCOMPATIBLE MOD SETUP!")
      {
        this._downloadModsMenu = new UIMenu("MODS REQUIRED!", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 290f, conString: "@SELECT@SELECT");
        this._downloadModsMenu.Add((UIComponent) new UIText("You're missing the mods required", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIText("to join this game. Would you", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIText("like to automatically subscribe to", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIText("all required mods, restart and", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIText("join the game?", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIText("", Colors.DGBlue), true);
        this._downloadModsMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionCloseMenu((UIComponent) this._downloadModsMenu)), true);
        this._downloadModsMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuCallFunction((UIComponent) this._downloadModsMenu, new UIMenuActionCloseMenuCallFunction.Function(UIServerBrowser.SubscribeAndRestart))), true);
        this._downloadModsMenu.Close();
        this._downloadModsMenu.Open();
        MonoMain.pauseMenu = (UIComponent) this._downloadModsMenu;
      }
      DuckNetwork.ClosePauseMenu();
      ConnectionStatusUI.Hide();
      this._startCalled = true;
      HUD.AddCornerMessage(HUDCorner.BottomRight, "@START@CONTINUE");
      base.Initialize();
    }

    public override void Update()
    {
      if (this._downloadModsMenu != null && this._downloadModsMenu.open)
        this._downloadModsMenu.DoUpdate();
      else if (Input.Pressed("START"))
      {
        Level.current = (Level) new TitleScreen();
        ConnectionError.joinLobby = (Lobby) null;
      }
      base.Update();
    }

    public override void Draw()
    {
      if (this._downloadModsMenu != null && this._downloadModsMenu.open)
        this._downloadModsMenu.DoDraw();
      string[] strArray = this._text.Split('{');
      float num = (float) (-(((IEnumerable<string>) strArray).Count<string>() - 1) * 8);
      foreach (string text in strArray)
      {
        float stringHeight = Graphics.GetStringHeight(text);
        Graphics.DrawString(text, new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 - (double) stringHeight / 2.0)), Color.White);
        num = stringHeight + 8f;
      }
    }
  }
}
