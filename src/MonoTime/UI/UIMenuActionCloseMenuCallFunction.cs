// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuActionCloseMenuCallFunction
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenuActionCloseMenuCallFunction : UIMenuAction
  {
    private UIComponent _menu;
    private UIMenuActionCloseMenuCallFunction.Function _function;

    public UIMenuActionCloseMenuCallFunction(
      UIComponent menu,
      UIMenuActionCloseMenuCallFunction.Function f)
    {
      this._menu = menu;
      this._function = f;
    }

    public override void Activate()
    {
      this._menu.Close();
      this._function();
    }

    public delegate void Function();
  }
}
