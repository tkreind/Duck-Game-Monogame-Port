// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelSelectCompanionMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class LevelSelectCompanionMenu : UIMenu
  {
    private LevelSelect _levelSelector;
    private UIMenu _returnMenu;
    private bool _justOpened;

    public LevelSelectCompanionMenu(float xpos, float ypos, UIMenu returnMenu)
      : base("", xpos, ypos)
      => this._returnMenu = returnMenu;

    public override void Initialize() => base.Initialize();

    public override void Open()
    {
      if (!LevelSelect._skipCompanionOpening)
      {
        this._levelSelector = new LevelSelect(returnMenu: ((UIMenu) this));
        this._levelSelector.Initialize();
        Editor.selectingLevel = true;
        this._justOpened = true;
      }
      LevelSelect._skipCompanionOpening = false;
      base.Open();
    }

    public override void Update()
    {
      if (this.open)
      {
        if (!this._justOpened)
        {
          this._levelSelector.Update();
          if (this._levelSelector.isClosed)
          {
            Editor.selectingLevel = false;
            this._levelSelector.Terminate();
            new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._returnMenu).Activate();
            return;
          }
        }
        this._justOpened = false;
      }
      base.Update();
    }

    public override void Draw()
    {
      if (!this.open)
        return;
      this._levelSelector.DrawThings(true);
    }
  }
}
