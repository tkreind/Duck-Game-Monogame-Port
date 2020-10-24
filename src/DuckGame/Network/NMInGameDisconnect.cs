// Decompiled with JetBrains decompiler
// Type: DuckGame.NMInGameDisconnect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMInGameDisconnect : NMEvent
  {
    public string id;

    public NMInGameDisconnect(string varID) => this.id = varID;

    public NMInGameDisconnect()
    {
    }

    public override void Activate()
    {
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection != null && profile.connection.identifier == this.id)
        {
          profile.connection.Disconnect();
          profile.networkStatus = DuckNetStatus.Disconnected;
          profile.slotType = SlotType.Reserved;
          profile.reservedUser = profile.connection.data;
          profile.reservedTeam = profile.team;
          profile.connection = (NetworkConnection) null;
        }
      }
    }
  }
}
