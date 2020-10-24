// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectingScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ConnectingScreen : Level
  {
    private float _dots;

    public ConnectingScreen() => this._centeredView = true;

    public override void Initialize()
    {
      DuckNetwork.ClosePauseMenu();
      ConnectionStatusUI.Hide();
      base.Initialize();
    }

    public override void Draw()
    {
      this._dots += 0.01f;
      if ((double) this._dots > 1.0)
        this._dots = 0.0f;
      string str = "";
      for (int index = 0; index < 3; ++index)
      {
        if ((double) this._dots * 4.0 > (double) (index + 1))
          str += ".";
      }
      string text = "Connecting";
      Graphics.DrawString(text + str, new Vec2((float) ((double) Layer.HUD.width / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), (float) ((double) Layer.HUD.height / 2.0 - 4.0)), Color.White);
    }
  }
}
