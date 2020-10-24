// Decompiled with JetBrains decompiler
// Type: DuckGame.Network
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Network
  {
    private int _networkIndex;
    private static int _simTick;
    private NCNetworkImplementation _core;
    private static Network _activeNetwork;
    private uint _currentTick;
    private NetIndex16 _tickSync = (NetIndex16) 0;
    private static Map<ushort, System.Type> _typeToMessageID = new Map<ushort, System.Type>();
    private static IEnumerable<NetMessagePriority> _netMessagePriorities;
    public static List<string> synchronizedTriggers = new List<string>()
    {
      "LEFT",
      "RIGHT",
      "UP",
      "DOWN",
      "SHOOT",
      "JUMP",
      "GRAB",
      "QUACK",
      "START",
      "RAGDOLL",
      "STRAFE"
    };
    private static int _inputDelayFrames = 0;

    public int networkIndex => this._networkIndex;

    public Network(int networkIndex = 0) => this._networkIndex = networkIndex;

    public static int simTick
    {
      get => Network._simTick;
      set => Network._simTick = value;
    }

    public static bool available => Steam.IsInitialized() && Steam.user != null;

    public NCNetworkImplementation core
    {
      get => this._core;
      set => this._core = value;
    }

    public static Network activeNetwork
    {
      get
      {
        if (Network._activeNetwork == null)
          Network._activeNetwork = new Network();
        return Network._activeNetwork;
      }
      set => Network._activeNetwork = value;
    }

    public static int frame
    {
      get => Network.activeNetwork.core.frame;
      set => Network.activeNetwork.core.frame = value;
    }

    public static NetGraph netGraph => Network._activeNetwork.core.netGraph;

    public static double Time => 0.0;

    public static uint Tick => Network.activeNetwork._currentTick;

    public static NetIndex16 TickSync => Network.activeNetwork._tickSync;

    public static float ping => Network.activeNetwork.core.averagePing;

    public static float highestPing
    {
      get
      {
        float num = 0.0f;
        foreach (NetworkConnection connection in Network.connections)
        {
          if (connection.status == ConnectionStatus.Connected && (double) connection.manager.ping > (double) num)
            num = connection.manager.ping;
        }
        return num;
      }
    }

    public static NetworkConnection host
    {
      get
      {
        if (DuckNetwork.localConnection.isHost)
          return DuckNetwork.localConnection;
        foreach (NetworkConnection connection in Network.connections)
        {
          if (connection.isHost)
            return connection;
        }
        return (NetworkConnection) null;
      }
    }

    public static Map<ushort, System.Type> typeToMessageID => Network._typeToMessageID;

    public static IEnumerable<NetMessagePriority> netMessagePriorities => Network._netMessagePriorities;

    public static int inputDelayFrames
    {
      get => Network._inputDelayFrames;
      set => Network._inputDelayFrames = value;
    }

    public static bool hasHostConnection
    {
      get
      {
        if (DuckNetwork.localConnection.isHost)
          return true;
        foreach (NetworkConnection connection in Network.connections)
        {
          if (connection.isHost)
            return true;
        }
        return false;
      }
    }

    public static bool isServer
    {
      get => Network.activeNetwork.core.isServer;
      set => Network.activeNetwork.core.isServer = value;
    }

    public static bool isClient => !Network.isServer;

    public static bool isActive => Network.activeNetwork != null && Network.activeNetwork.core != null && Network.activeNetwork.core.isActive;

    public static bool connected => Network.connections.Count > 0;

    public static List<NetworkConnection> connections => Network.activeNetwork.core.connections;

    public System.Type GetClassType(string name)
    {
      string fullName = typeof (Duck).Assembly.FullName;
      return Editor.GetType("DuckGame." + name + ", " + fullName);
    }

    public static void JoinServer(string nameVal, int portVal = 1337, string ip = "localhost") => Network.activeNetwork.DoJoinServer(nameVal, portVal, ip);

    private void DoJoinServer(string nameVal, int portVal = 1337, string ip = "localhost") => this.core.JoinServer(nameVal, portVal, ip);

    public static void HostServer(
      NetworkLobbyType lobbyType,
      int maxConnectionsVal = 32,
      string nameVal = "duckGameServer",
      int portVal = 1337)
    {
      Network.activeNetwork.DoHostServer(lobbyType, maxConnectionsVal, nameVal, portVal);
    }

    private void DoHostServer(
      NetworkLobbyType lobbyType,
      int maxConnectionsVal = 32,
      string nameVal = "duckGameServer",
      int portVal = 1337)
    {
      this.core.HostServer(nameVal, portVal, lobbyType, maxConnectionsVal);
    }

    public static void OnMessageStatic(NetMessage m) => Network._activeNetwork.OnMessage(m);

    private void OnMessage(NetMessage m)
    {
      if (m is NMConsoleMessage)
        DevConsole.Log((m as NMConsoleMessage).message, Color.Lime);
      else if (Network.isServer)
        this.OnMessageServer(m);
      else
        this.OnMessageClient(m);
    }

    private void OnMessageServer(NetMessage m) => Level.current.OnMessage(m);

    private void OnMessageClient(NetMessage m) => Level.current.OnMessage(m);

    public void OnConnection(NetworkConnection connection) => UIMatchmakingBox.pulseNetwork = true;

    public void QueueMessage(NetMessage msg, NetworkConnection who = null)
    {
      if (who == null)
      {
        if (Network.connections.Count == 1)
        {
          Network.connections[0].manager.QueueMessage(msg);
        }
        else
        {
          foreach (NetworkConnection connection in Network.connections)
          {
            NetMessage instance = Activator.CreateInstance(msg.GetType(), (object[]) null) as NetMessage;
            Editor.CopyClass((object) msg, (object) instance);
            instance.ClearSerializedData();
            connection.manager.QueueMessage(instance);
          }
        }
      }
      else
        who.manager.QueueMessage(msg);
    }

    public void QueueMessageForAllBut(NetMessage msg, NetworkConnection who)
    {
      foreach (NetworkConnection connection in Network.connections)
      {
        if (who != connection)
        {
          NetMessage instance = Activator.CreateInstance(msg.GetType(), (object[]) null) as NetMessage;
          Editor.CopyClass((object) msg, (object) instance);
          instance.ClearSerializedData();
          connection.manager.QueueMessage(instance);
        }
      }
    }

    public void QueueMessage(NetMessage msg, NetMessagePriority priority, NetworkConnection who = null)
    {
      msg.priority = priority;
      this.QueueMessage(msg, who);
    }

    public void QueueMessageForAllBut(
      NetMessage msg,
      NetMessagePriority priority,
      NetworkConnection who)
    {
      msg.priority = priority;
      this.QueueMessageForAllBut(msg, who);
    }

    public static void Disconnect()
    {
      foreach (NetworkConnection sessionConnection in Network.activeNetwork.core.sessionConnections)
        sessionConnection.Disconnect();
      if (Network.activeNetwork.core.sessionConnections.Count != 0)
        return;
      Network.activeNetwork.core.OnSessionEnded();
    }

    public static void Initialize()
    {
      IEnumerable<System.Type> subclasses = Editor.GetSubclasses(typeof (NetMessage));
      Network._typeToMessageID.Clear();
      ushort key = 1;
      foreach (System.Type type in subclasses)
      {
        if (type.GetCustomAttributes(typeof (FixedNetworkID), false).Length != 0)
        {
          FixedNetworkID customAttribute = (FixedNetworkID) type.GetCustomAttributes(typeof (FixedNetworkID), false)[0];
          if (customAttribute != null)
            Network._typeToMessageID.Add(type, customAttribute.FixedID);
        }
      }
      foreach (System.Type type in subclasses)
      {
        if (!Network._typeToMessageID.ContainsValue(type))
        {
          while (Network._typeToMessageID.ContainsKey(key))
            ++key;
          Network._typeToMessageID.Add(type, key);
          ++key;
        }
      }
      Network._netMessagePriorities = Enum.GetValues(typeof (NetMessagePriority)).Cast<NetMessagePriority>();
      Network.activeNetwork.DoInitialize();
    }

    public void DoInitialize() => this._core = (NCNetworkImplementation) new NCSteam(Network.activeNetwork, this._networkIndex);

    public static void Terminate() => Network.activeNetwork.core.Terminate();

    public void Reset()
    {
      this._currentTick = 0U;
      this._tickSync = (NetIndex16) 0;
    }

    public static void PreUpdate() => Network.activeNetwork.DoPreUpdate();

    public void DoPreUpdate()
    {
      ++this._currentTick;
      this._tickSync += 1;
      this._core.Update();
      DuckNetwork.Update();
    }

    public static void PostUpdate() => Network.activeNetwork.DoPostUpdate();

    public void DoPostUpdate() => this._core.PostUpdate();

    public static void PostDraw() => Network.activeNetwork.DoPostDraw();

    public void DoPostDraw() => this._core.PostDraw();
  }
}
