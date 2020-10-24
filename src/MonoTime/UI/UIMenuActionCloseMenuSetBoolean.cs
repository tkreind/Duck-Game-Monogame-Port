// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuActionCloseMenuSetBoolean
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenuActionCloseMenuSetBoolean : UIMenuAction
  {
    private UIComponent _menu;
    private MenuBoolean _value;

    public UIMenuActionCloseMenuSetBoolean(UIComponent menu, MenuBoolean value)
    {
      this._menu = menu;
      this._value = value;
    }

    public override void Activate()
    {
      this._menu.Close();
      this._value.value = true;
    }
  }
}
