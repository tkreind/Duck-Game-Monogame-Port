// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuActionOpenMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenuActionOpenMenu : UIMenuAction
  {
    private UIComponent _menu;
    private UIComponent _open;

    public UIMenuActionOpenMenu(UIComponent menu, UIComponent open)
    {
      this._menu = menu;
      this._open = open;
    }

    public override void Activate()
    {
      UIComponent pauseMenu = MonoMain.pauseMenu;
      this._menu.Close();
      this._open.Open();
      if (pauseMenu != this._menu)
        return;
      MonoMain.pauseMenu = this._open;
    }
  }
}
