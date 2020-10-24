// Decompiled with JetBrains decompiler
// Type: DuckGame.NCSteam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class NCSteam : NCNetworkImplementation
  {
    private Lobby _lobby;
    private Lobby.UserStatusChangeDelegate _userChange;
    private Steam.ConnectionRequestedDelegate _connectionRequest;
    private Steam.ConnectionFailedDelegate _connectionFailed;
    private Steam.InviteReceivedDelegate _inviteReceived;
    private Steam.LobbySearchCompleteDelegate _lobbySearchComplete;
    private Steam.RequestCurrentStatsDelegate _requestStatsComplete;
    private string _serverIdentifier = "";
    private int _port;
    private ulong _connectionPacketIdentifier = 6094567099491692639;
    private bool _initializedSettings;
    private bool _lobbyCreationComplete;
    private List<Lobby> _activeLobbies = new List<Lobby>();
    public static ulong inviteLobbyID;

    public Lobby lobby => this._lobby;

    public NCSteam(Network c, int networkIndex)
      : base(c, networkIndex)
      => this.HookUpDelegates();

    public override NCError OnSendPacket(byte[] data, int length, object connection)
    {
      if (length < 1200)
        Steam.SendPacket(connection as User, data, (uint) length, P2PDataSendType.Unreliable);
      else
        Steam.SendPacket(connection as User, data, (uint) length, P2PDataSendType.Reliable);
      return (NCError) null;
    }

    public void HookUpDelegates()
    {
      if (this._connectionRequest != null)
        return;
      this._connectionRequest = new Steam.ConnectionRequestedDelegate(this.OnConnectionRequest);
      Steam.ConnectionRequested += this._connectionRequest;
      this._connectionFailed = new Steam.ConnectionFailedDelegate(this.OnConnectionFailed);
      Steam.ConnectionFailed += this._connectionFailed;
      this._inviteReceived = new Steam.InviteReceivedDelegate(this.OnInviteReceived);
      Steam.InviteReceived += this._inviteReceived;
      this._lobbySearchComplete = new Steam.LobbySearchCompleteDelegate(this.OnLobbySearchComplete);
      Steam.LobbySearchComplete += this._lobbySearchComplete;
      this._requestStatsComplete = new Steam.RequestCurrentStatsDelegate(this.OnRequestStatsComplete);
      Steam.RequestCurrentStatsComplete += this._requestStatsComplete;
    }

    public void UnhookDelegates()
    {
      if (this._connectionRequest == null)
        return;
      Steam.ConnectionRequested -= this._connectionRequest;
      Steam.ConnectionFailed -= this._connectionFailed;
      Steam.InviteReceived -= this._inviteReceived;
      Steam.LobbySearchComplete -= this._lobbySearchComplete;
      this._connectionRequest = (Steam.ConnectionRequestedDelegate) null;
    }

    public override NCError OnHostServer(
      string identifier,
      int port,
      NetworkLobbyType lobbyType,
      int maxConnections)
    {
      foreach (Lobby activeLobby in this._activeLobbies)
      {
        Steam.LeaveLobby(activeLobby);
        DevConsole.Log(DCSection.Steam, "|DGYELLOW|Leaving lobby to host new lobby.");
      }
      this._lobby = (Lobby) null;
      if (this._lobby != null)
        return new NCError("Server is already started...", NCErrorType.Error);
      this.HookUpDelegates();
      this._initializedSettings = false;
      this._lobby = Steam.CreateLobby((SteamLobbyType) lobbyType, maxConnections);
      this._activeLobbies.Add(this._lobby);
      if (this._lobby == null)
        return new NCError("|DGORANGE|STEAM |DGRED|Steam is not running.", NCErrorType.Error);
      this._userChange = new Lobby.UserStatusChangeDelegate(this.OnUserStatusChange);
      this._lobby.UserStatusChange += this._userChange;
      this._serverIdentifier = identifier;
      this._port = port;
      this.StartServerThread();
      return new NCError("|DGORANGE|STEAM |DGYELLOW|Attempting to create server lobby...", NCErrorType.Message);
    }

    public override NCError OnJoinServer(string identifier, int port, string ip)
    {
      foreach (Lobby activeLobby in this._activeLobbies)
      {
        Steam.LeaveLobby(activeLobby);
        DevConsole.Log(DCSection.Steam, "|DGYELLOW|Leaving lobby to join.");
      }
      this._lobby = (Lobby) null;
      if (this._lobby != null)
        return new NCError("Client is already started...", NCErrorType.Error);
      this.HookUpDelegates();
      this._serverIdentifier = identifier;
      this._lobby = Steam.JoinLobby(Convert.ToUInt64(identifier));
      this._activeLobbies.Add(this._lobby);
      if (this._lobby == null)
        return new NCError("Steam is not running.", NCErrorType.Error);
      this._userChange = new Lobby.UserStatusChangeDelegate(this.OnUserStatusChange);
      this._lobby.UserStatusChange += this._userChange;
      this._port = port;
      this.StartClientThread();
      return new NCError("|DGORANGE|STEAM |DGGREEN|Connecting to lobbyID " + identifier + ".", NCErrorType.Message);
    }

    public void OnUserStatusChange(User who, SteamLobbyUserStatusFlags flags, User responsible)
    {
      if ((flags & SteamLobbyUserStatusFlags.Entered) != (SteamLobbyUserStatusFlags) 0)
      {
        DevConsole.Log(DCSection.Steam, "|DGGREEN|" + who.name + " has joined the Steam lobby.");
        if (Network.isServer && DuckNetwork.localConnection.status == ConnectionStatus.Connected)
          this.AttemptConnection((object) who);
      }
      else if ((flags & SteamLobbyUserStatusFlags.Left) != (SteamLobbyUserStatusFlags) 0)
        DevConsole.Log(DCSection.Steam, "|DGRED|" + who.name + " has left the Steam lobby.");
      else if ((flags & SteamLobbyUserStatusFlags.Disconnected) != (SteamLobbyUserStatusFlags) 0)
        DevConsole.Log(DCSection.Steam, "|DGRED|" + who.name + " has disconnected from the Steam lobby.");
      if ((flags & SteamLobbyUserStatusFlags.Kicked) == (SteamLobbyUserStatusFlags) 0)
        return;
      DevConsole.Log(DCSection.Steam, "|DGYELLOW|" + responsible.name + " kicked " + who.name + ".");
    }

    public void OnConnectionRequest(User who)
    {
      if (this.GetConnection((object) who) == null)
        return;
      DevConsole.Log(DCSection.Steam, "|DGYELLOW|" + who.name + " has requested a connection.");
      Steam.AcceptConnection(who);
    }

    public void OnConnectionFailed(User who) => DevConsole.Log(DCSection.Steam, "|DGRED|Connection with " + who.name + " has failed!");

    public void OnInviteReceived(User who, Lobby lobby)
    {
      NCSteam.inviteLobbyID = lobby.id;
      Level.current = (Level) new JoinServer(lobby.id);
    }

    public void OnLobbySearchComplete(Lobby lobby)
    {
    }

    public void OnRequestStatsComplete()
    {
    }

    protected override object GetConnectionObject(string identifier) => (object) User.GetUser(Convert.ToUInt64(identifier));

    public override string GetConnectionIdentifier(object connection) => connection is User user ? user.id.ToString() : "no info";

    public override string GetConnectionName(object connection) => connection is User user ? user.name : "no info";

    protected override string OnGetLocalName() => Steam.user != null ? Steam.user.name : "no info";

    protected override NCError OnSpinServerThread()
    {
      if (this._lobby == null)
        return new NCError("|DGORANGE|STEAM |DGRED|Lobby was closed.", NCErrorType.CriticalError);
      if (this._lobby.processing)
        return (NCError) null;
      return this._lobby.id == 0UL ? new NCError("|DGORANGE|STEAM |DGRED|Failed to create lobby.", NCErrorType.CriticalError) : this.RunSharedLogic();
    }

    protected override NCError OnSpinClientThread()
    {
      if (this._lobby == null)
        return new NCError("|DGORANGE|STEAM |DGYELLOW|Lobby was closed.", NCErrorType.CriticalError);
      if (this._lobby.processing)
        return (NCError) null;
      return this._lobby.id == 0UL ? new NCError("|DGORANGE|STEAM |DGRED|Failed to join lobby.", NCErrorType.CriticalError) : this.RunSharedLogic();
    }

    protected NCError RunSharedLogic()
    {
      while (true)
      {
        SteamPacket steamPacket;
        bool flag;
        do
        {
          steamPacket = Steam.ReadPacket();
          if (steamPacket != null)
          {
            flag = false;
            if (steamPacket.data.Length == 8)
            {
              try
              {
                if ((long) BitConverter.ToUInt64(steamPacket.data, 0) == (long) this._connectionPacketIdentifier)
                  flag = true;
              }
              catch
              {
              }
            }
          }
          else
            goto label_7;
        }
        while (flag);
        this.OnPacket(steamPacket.data, (object) steamPacket.connection);
      }
label_7:
      return (NCError) null;
    }

    protected override void Disconnect(NetworkConnection c)
    {
      if (c != null && c.data is User data)
        Steam.CloseConnection(data);
      base.Disconnect(c);
    }

    protected override void KillConnection()
    {
      foreach (Lobby activeLobby in this._activeLobbies)
      {
        Steam.LeaveLobby(activeLobby);
        DevConsole.Log(DCSection.Steam, "|DGYELLOW|Exiting lobby.");
      }
      this._lobbyCreationComplete = false;
      this._initializedSettings = false;
      this._lobby = (Lobby) null;
      base.KillConnection();
    }

    public void ApplyLobbyData()
    {
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (matchSetting.value is int)
          this._lobby.SetLobbyData(matchSetting.id, ((int) matchSetting.value).ToString());
        else if (matchSetting.value is bool)
          this._lobby.SetLobbyData(matchSetting.id, ((bool) matchSetting.value ? 1 : 0).ToString());
      }
      foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
      {
        if (onlineSetting.value is int)
          this._lobby.SetLobbyData(onlineSetting.id, ((int) onlineSetting.value).ToString());
        else if (onlineSetting.value is bool)
          this._lobby.SetLobbyData(onlineSetting.id, ((bool) onlineSetting.value ? 1 : 0).ToString());
      }
      foreach (UnlockData allUnlock in Unlocks.allUnlocks)
        this._lobby.SetLobbyData(allUnlock.id, (allUnlock.enabled ? 1 : 0).ToString());
      this._lobby.SetLobbyData("customLevels", Editor.activatedLevels.Count.ToString());
    }

    public override void Update()
    {
      if (this._lobby != null && !this._lobby.processing && this._lobby.id != 0UL)
      {
        if (!this._lobbyCreationComplete)
        {
          this._lobbyCreationComplete = true;
          if (Network.isServer)
          {
            if (this._lobby.joinResult == SteamLobbyJoinResult.Success)
            {
              DevConsole.Log(DCSection.Steam, "|DGGREEN|Lobby created.");
            }
            else
            {
              DevConsole.Log(DCSection.Steam, "|DGGREEN|Lobby creation failed!");
              Network.Disconnect();
              return;
            }
          }
          else if (this._lobby.joinResult == SteamLobbyJoinResult.Success)
          {
            string lobbyData = this._lobby.GetLobbyData("version");
            NMVersionMismatch.Type type = DuckNetwork.CheckVersion(lobbyData);
            if (type != NMVersionMismatch.Type.Match)
            {
              DuckNetwork.FailWithVersionMismatch(lobbyData, type);
              DevConsole.Log(DCSection.Steam, "|DGRED|Lobby version mismatch! (" + type.ToString() + ")");
              return;
            }
            DevConsole.Log(DCSection.Steam, "|DGGREEN|Lobby Joined (" + this._lobby.owner.name + ")");
            this.AttemptConnection((object) this._lobby.owner, true);
          }
          else
          {
            DevConsole.Log(DCSection.Steam, "|DGGREEN|Failed to join lobby (" + this._lobby.joinResult.ToString() + ")");
            Network.Disconnect();
            return;
          }
        }
        if (Network.isServer && !this._initializedSettings && this._lobby.id != 0UL)
        {
          NCSteam.UpdateRandomID(this._lobby);
          this._lobby.SetLobbyData("started", "false");
          this._lobby.SetLobbyData("version", DG.version);
          this._lobby.SetLobbyData("beta", "2.0");
          this._lobby.SetLobbyData("dev", DG.devBuild ? "true" : "false");
          this._lobby.SetLobbyData("modifiers", "false");
          this._lobby.SetLobbyData("modhash", ModLoader.modHash);
          this._lobby.SetLobbyData("name", Steam.user.name + "'s Lobby");
          this._lobby.SetLobbyData("numSlots", DuckNetwork.numSlots.ToString());
          string str = "";
          bool flag = true;
          foreach (Mod accessibleMod in (IEnumerable<Mod>) ModLoader.accessibleMods)
          {
            if (!(accessibleMod is CoreMod) && accessibleMod.configuration != null && !accessibleMod.configuration.disabled)
            {
              if (!flag)
                str += "|";
              str = accessibleMod.configuration.isWorkshop ? str + accessibleMod.configuration.workshopID.ToString() : str + "LOCAL";
              flag = false;
            }
          }
          this._lobby.SetLobbyData("mods", str);
          this.ApplyLobbyData();
          this._initializedSettings = true;
        }
      }
      base.Update();
    }

    public static void UpdateRandomID(Lobby l)
    {
      l.randomID = Rando.Int(2147483646);
      l.SetLobbyData("randomID", l.randomID.ToString());
    }

    public override void Terminate()
    {
      this._initializedSettings = false;
      this.UnhookDelegates();
      base.Terminate();
    }
  }
}
