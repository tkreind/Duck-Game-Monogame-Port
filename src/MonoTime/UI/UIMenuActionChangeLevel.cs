// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuActionChangeLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenuActionChangeLevel : UIMenuAction
  {
    private UIComponent _menu;
    private Level _destination;
    private bool _activated;

    public UIMenuActionChangeLevel(UIComponent menu, Level destination)
    {
      this._menu = menu;
      this._destination = destination;
    }

    public override void Update()
    {
      if (!this._activated)
        return;
      Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.09f);
      if ((double) Graphics.fade != 0.0)
        return;
      Level.current = this._destination;
    }

    public override void Activate() => this._activated = true;
  }
}
