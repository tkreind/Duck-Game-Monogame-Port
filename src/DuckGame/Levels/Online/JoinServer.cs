// Decompiled with JetBrains decompiler
// Type: DuckGame.JoinServer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class JoinServer : Level
  {
    private bool _attemptedConnection;
    private ulong _lobbyID;
    private float _dots;
    private bool _startedJoining;

    public JoinServer(ulong lobbyAddress)
    {
      this._lobbyID = lobbyAddress;
      this._centeredView = true;
    }

    public override void Initialize()
    {
      DuckNetwork.ClosePauseMenu();
      ConnectionStatusUI.Hide();
      Network.Disconnect();
      base.Initialize();
    }

    public override void Update()
    {
      if (DuckNetwork.status == DuckNetStatus.Disconnected && !this._attemptedConnection)
      {
        this._startedJoining = true;
        if (Profiles.active.Count == 0)
          Profiles.DefaultPlayer1.team = Teams.Player1;
        TeamSelect2.FillMatchmakingProfiles();
        if (this._lobbyID == 0UL)
          DuckNetwork.Join("duckGameServer");
        else
          DuckNetwork.Join(this._lobbyID.ToString());
        this._attemptedConnection = true;
      }
      else if (DuckNetwork.status == DuckNetStatus.Disconnected && this._attemptedConnection)
        Level.current = (Level) new ConnectionError("|RED|CONNECTION FAILED!");
      base.Update();
    }

    public override void OnSessionEnded(DuckNetErrorInfo error)
    {
      if (!this._startedJoining)
        return;
      if (error != null)
        Level.current = (Level) new ConnectionError(error.message);
      else
        Level.current = (Level) new ConnectionError("|RED|CONNECTION FAILED!");
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
