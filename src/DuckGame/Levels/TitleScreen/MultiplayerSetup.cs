// Decompiled with JetBrains decompiler
// Type: DuckGame.MultiplayerSetup
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class MultiplayerSetup : Level
  {
    public int roundsPerSet = 8;
    public int setsPerGame = 3;

    public override void Initialize()
    {
      this.camera.x = 480f;
      UIMenu uiMenu = new UIMenu("MULTIPLAYER", (float) Graphics.width / 2f, (float) Graphics.height / 2f, 160f);
      uiMenu.scale = new Vec2(4f);
      uiMenu.Add((UIComponent) new UIMenuItemNumber("ROUNDS PER SET", field: new FieldBinding((object) this, "roundsPerSet", max: 50f)), true);
      uiMenu.Add((UIComponent) new UIMenuItemNumber("SETS PER GAME", field: new FieldBinding((object) this, "setsPerGame", max: 50f)), true);
      uiMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      uiMenu.Add((UIComponent) new UIMenuItem("START", (UIMenuAction) new UIMenuActionChangeLevel((UIComponent) uiMenu, (Level) new TeamSelect2())), true);
      uiMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionChangeLevel((UIComponent) uiMenu, (Level) new TitleScreen())), true);
      Level.Add((Thing) uiMenu);
      base.Initialize();
    }

    public override void Update()
    {
    }
  }
}
