// Decompiled with JetBrains decompiler
// Type: DuckGame.DisconnectFromGame
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DisconnectFromGame : Level
  {
    private float _dots;
    private bool _disconnected;

    public DisconnectFromGame() => this._centeredView = true;

    public override void Initialize()
    {
      DuckNetwork.ClosePauseMenu();
      ++DuckNetwork.levelIndex;
      this._startCalled = true;
      Network.Disconnect();
      ConnectionStatusUI.Hide();
      base.Initialize();
    }

    public override void Update()
    {
      if (this._disconnected)
      {
        Graphics.fade = Lerp.Float(Graphics.fade, 0.0f, 0.05f);
        if ((double) Graphics.fade <= 0.0)
          Level.current = (Level) new TitleScreen();
      }
      base.Update();
    }

    public override void OnSessionEnded(DuckNetErrorInfo error) => this._disconnected = true;

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
      string text = "Disconnecting";
      Graphics.DrawString(text + str, new Vec2((float) ((double) Layer.HUD.width / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), (float) ((double) Layer.HUD.height / 2.0 - 4.0)), Color.White);
    }
  }
}
