// Decompiled with JetBrains decompiler
// Type: DuckGame.UnlockableScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UnlockableScreen : Level
  {
    private HashSet<Unlockable> _unlocks;
    private UIUnlockBox _unlockBox;

    public UnlockableScreen() => this._centeredView = true;

    public override void Initialize()
    {
      base.Initialize();
      Unlockables.HasPendingUnlocks();
      this._unlocks = Unlockables.GetPendingUnlocks();
    }

    public override void Update()
    {
      if (this._unlockBox == null)
      {
        this._unlockBox = new UIUnlockBox(this._unlocks.ToList<Unlockable>(), Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f);
        MonoMain.pauseMenu = (UIComponent) this._unlockBox;
      }
      base.Update();
    }
  }
}
