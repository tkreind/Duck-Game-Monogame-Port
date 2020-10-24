// Decompiled with JetBrains decompiler
// Type: DuckGame.PlacementMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PlacementMenu : EditorGroupMenu
  {
    private ContextMenu _noneMenu;

    public PlacementMenu(float xpos, float ypos)
      : base((IContextListener) null)
    {
      this.x = xpos;
      this.y = ypos;
      this._root = true;
      this.willOnlineGrayout = false;
      this._noneMenu = new ContextMenu((IContextListener) this);
      this._noneMenu.text = "None";
      this.AddItem(this._noneMenu);
      this.fancy = true;
      this.InitializeGroups(Editor.Placeables);
      this.AddItem((ContextMenu) new ContextToolbarItem((ContextMenu) this));
    }

    public override void Selected(ContextMenu item)
    {
      if (item == this._noneMenu)
      {
        if (!(Level.current is Editor current))
          return;
        current.placementType = (Thing) null;
        current.CloseMenu();
      }
      else
        base.Selected(item);
    }

    public override void Initialize()
    {
    }
  }
}
