// Decompiled with JetBrains decompiler
// Type: DuckGame.NetworkDebugger
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class NetworkDebugger : Level
  {
    private static int _networkDrawingIndex = 0;
    public static List<InputProfile> inputProfiles = new List<InputProfile>();
    private static bool _enabled = false;
    public static List<NetworkInstance> _instances = new List<NetworkInstance>();
    private Level _startLevel;
    private LayerCore _startLayer;
    private static List<NetDebugInterface> _interfaces = new List<NetDebugInterface>()
    {
      new NetDebugInterface()
      {
        position = new Vec2(10f, 10f),
        index = 0
      },
      new NetDebugInterface()
      {
        position = new Vec2((float) (DuckGame.Graphics.width / 2 + 10), 10f),
        index = 1
      },
      new NetDebugInterface()
      {
        position = new Vec2(10f, (float) (DuckGame.Graphics.height / 2 + 10)),
        index = 2
      },
      new NetDebugInterface()
      {
        position = new Vec2((float) (DuckGame.Graphics.width / 2 + 10), (float) (DuckGame.Graphics.height / 2 + 10)),
        index = 3
      }
    };
    private Network oldNetwork;
    private DuckNetworkCore oldDuckNetworkCore;
    private VirtualTransitionCore oldVirtualCore;
    private LevelCore oldLevelCore;
    private ProfilesCore oldProfileCore;
    private TeamsCore oldTeamCore;
    private LayerCore oldLayerCore;
    private InputProfileCore oldInputCore;
    private DevConsoleCore oDevCore;
    private CrowdCore oldCrowdCore;
    private GameModeCore oldGameModeCore;
    private ConnectionStatusUICore oldConnectionUICore;
    private MonoMainCore oldMonoCore;
    public bool lefpres;
    public static Dictionary<string, Dictionary<string, float>> _sentPulse = new Dictionary<string, Dictionary<string, float>>();
    public static Dictionary<string, Dictionary<string, float>> _receivedPulse = new Dictionary<string, Dictionary<string, float>>();
    private SpriteMap _connectionArrow;
    private Sprite _connectionX;

    public static int networkDrawingIndex => NetworkDebugger._networkDrawingIndex;

    public static bool enabled => NetworkDebugger._enabled;

    public NetworkDebugger(Level level = null, LayerCore startLayer = null)
    {
      this._startLevel = level;
      this._startLayer = startLayer;
      if (level == null)
      {
        foreach (Profile profile in Profiles.all)
          profile.team = (Team) null;
      }
      for (int index = 0; index < 4; ++index)
        NetworkDebugger.inputProfiles.Add(new InputProfile());
    }

    public static bool Mute(int index) => NetworkDebugger._interfaces[index].mute;

    public static List<NetDebugInterface> interfaces => NetworkDebugger._interfaces;

    public void CreateInstance(int init, bool isHost)
    {
      NetworkDebugger._networkDrawingIndex = init;
      NetworkInstance networkInstance = new NetworkInstance()
      {
        network = new Network(NetworkDebugger._networkDrawingIndex)
      };
      if (this._startLevel == null)
      {
        networkInstance.teamsCore = new TeamsCore();
        networkInstance.teamsCore.Initialize();
      }
      else
      {
        networkInstance.teamsCore = Teams.core;
        List<Team> teamList = new List<Team>((IEnumerable<Team>) Teams.core.extraTeams);
        Teams.core = new TeamsCore();
        Teams.core.Initialize();
        Teams.core.extraTeams = teamList;
      }
      if (this._startLayer != null)
      {
        networkInstance.layerCore = this._startLayer;
        this._startLayer = (LayerCore) null;
      }
      else
      {
        networkInstance.layerCore = new LayerCore();
        networkInstance.layerCore.InitializeLayers();
      }
      networkInstance.virtualCore = new VirtualTransitionCore();
      networkInstance.virtualCore.Initialize();
      if (this._startLevel == null)
      {
        networkInstance.profileCore = new ProfilesCore();
        networkInstance.profileCore.Initialize();
        networkInstance.profileCore.DefaultPlayer2.team = (Team) null;
      }
      else
      {
        networkInstance.profileCore = Profiles.core;
        Profiles.core = new ProfilesCore();
        Profiles.core.Initialize();
      }
      networkInstance.inputProfile = new InputProfileCore();
      InputProfileCore core1 = InputProfile.core;
      InputProfile.core = networkInstance.inputProfile;
      Input.InitDefaultProfiles();
      networkInstance.levelCore = new LevelCore();
      LevelCore core2 = Level.core;
      Level.core = networkInstance.levelCore;
      networkInstance.crowdCore = new CrowdCore();
      CrowdCore core3 = Crowd.core;
      Crowd.core = networkInstance.crowdCore;
      TeamsCore core4 = Teams.core;
      networkInstance.teamsCore.extraTeams = new List<Team>((IEnumerable<Team>) Teams.core.extraTeams);
      Teams.core = networkInstance.teamsCore;
      ProfilesCore core5 = Profiles.core;
      Profiles.core = networkInstance.profileCore;
      LayerCore core6 = Layer.core;
      Layer.core = networkInstance.layerCore;
      VirtualTransitionCore core7 = VirtualTransition.core;
      VirtualTransition.core = networkInstance.virtualCore;
      Network activeNetwork = Network.activeNetwork;
      Network.activeNetwork = networkInstance.network;
      networkInstance.network.DoInitialize();
      networkInstance.duckNetworkCore = new DuckNetworkCore();
      GameModeCore gameModeCore = new GameModeCore();
      networkInstance.gameModeCore = gameModeCore;
      GameModeCore core8 = GameMode.core;
      GameMode.core = gameModeCore;
      networkInstance.connectionUICore = new ConnectionStatusUICore();
      ConnectionStatusUICore core9 = ConnectionStatusUI.core;
      ConnectionStatusUI.core = networkInstance.connectionUICore;
      networkInstance.consoleCore = new DevConsoleCore();
      DevConsoleCore core10 = DevConsole.core;
      DevConsole.core = networkInstance.consoleCore;
      DuckNetworkCore core11 = DuckNetwork.core;
      DuckNetwork.core = networkInstance.duckNetworkCore;
      DuckNetwork.Initialize();
      networkInstance.monoCore = new MonoMainCore();
      MonoMainCore core12 = MonoMain.core;
      MonoMain.core = networkInstance.monoCore;
      if (isHost)
      {
        foreach (Profile profile in Profiles.all)
        {
          if (profile.team != null)
            NetworkDebugger.inputProfiles[Persona.Number(profile.persona)] = InputProfile.defaultProfiles[Persona.Number(profile.persona)];
        }
        DuckNetwork.Host(4, NetworkLobbyType.Public);
        if (this._startLevel != null)
        {
          Level.current = this._startLevel;
          this._startLevel.NetworkDebuggerPrepare();
          this._startLevel = (Level) null;
        }
        else
          Level.current = (Level) new TeamSelect2();
      }
      else
        Level.current = (Level) new JoinServer(0UL);
      networkInstance.joined = true;
      if (init >= NetworkDebugger._instances.Count)
        NetworkDebugger._instances.Add(networkInstance);
      else
        NetworkDebugger._instances[init] = networkInstance;
      base.Initialize();
      InputProfile.core = core1;
      Network.activeNetwork = activeNetwork;
      DuckNetwork.core = core11;
      Teams.core = core4;
      Profiles.core = core5;
      Layer.core = core6;
      VirtualTransition.core = core7;
      Level.core = core2;
      DevConsole.core = core10;
      Crowd.core = core3;
      GameMode.core = core8;
      ConnectionStatusUI.core = core9;
      MonoMain.core = core12;
    }

    public void LockInstance(NetworkInstance instance)
    {
      this.oldNetwork = Network.activeNetwork;
      Network.activeNetwork = instance.network;
      this.oldDuckNetworkCore = DuckNetwork.core;
      DuckNetwork.core = instance.duckNetworkCore;
      this.oldVirtualCore = VirtualTransition.core;
      VirtualTransition.core = instance.virtualCore;
      this.oldLevelCore = Level.core;
      Level.core = instance.levelCore;
      this.oldProfileCore = Profiles.core;
      Profiles.core = instance.profileCore;
      this.oldTeamCore = Teams.core;
      Teams.core = instance.teamsCore;
      this.oldLayerCore = Layer.core;
      Layer.core = instance.layerCore;
      this.oldInputCore = InputProfile.core;
      InputProfile.core = instance.inputProfile;
      this.oDevCore = DevConsole.core;
      DevConsole.core = instance.consoleCore;
      this.oldCrowdCore = Crowd.core;
      Crowd.core = instance.crowdCore;
      this.oldGameModeCore = GameMode.core;
      GameMode.core = instance.gameModeCore;
      this.oldConnectionUICore = ConnectionStatusUI.core;
      ConnectionStatusUI.core = instance.connectionUICore;
      this.oldMonoCore = MonoMain.core;
      MonoMain.core = instance.monoCore;
    }

    public void UnlockInstance()
    {
      Network.activeNetwork = this.oldNetwork;
      DuckNetwork.core = this.oldDuckNetworkCore;
      Teams.core = this.oldTeamCore;
      Layer.core = this.oldLayerCore;
      VirtualTransition.core = this.oldVirtualCore;
      Level.core = this.oldLevelCore;
      Profiles.core = this.oldProfileCore;
      InputProfile.core = this.oldInputCore;
      DevConsole.core = this.oDevCore;
      Crowd.core = this.oldCrowdCore;
      GameMode.core = this.oldGameModeCore;
      ConnectionStatusUI.core = this.oldConnectionUICore;
      MonoMain.core = this.oldMonoCore;
    }

    public override void Initialize()
    {
      NetworkDebugger._enabled = true;
      NetworkDebugger._networkDrawingIndex = 0;
      this.CreateInstance(0, true);
      Level.activeLevel = (Level) this;
      base.Initialize();
    }

    public static void TerminateThreads()
    {
      foreach (NetworkInstance instance in NetworkDebugger._instances)
        instance.network.core.Terminate();
    }

    public override void DoUpdate()
    {
      this.lefpres = Mouse.left == InputState.Pressed;
      List<DCLine> dcLineList = (List<DCLine>) null;
      lock (DevConsole.debuggerLines)
      {
        dcLineList = DevConsole.debuggerLines;
        DevConsole.debuggerLines = new List<DCLine>();
      }
      for (int index1 = 0; index1 < 4; ++index1)
      {
        if (index1 >= NetworkDebugger._instances.Count || !NetworkDebugger._instances[index1].active)
        {
          if (index1 <= NetworkDebugger._instances.Count)
          {
            int init = -1;
            InputProfile inputProfile1 = (InputProfile) null;
            foreach (InputProfile defaultProfile in InputProfile.defaultProfiles)
            {
              if (defaultProfile.Pressed("START"))
              {
                foreach (InputProfile inputProfile2 in NetworkDebugger.inputProfiles)
                {
                  if (defaultProfile == inputProfile2)
                    break;
                }
                for (int index2 = 0; index2 < 4; ++index2)
                {
                  if (NetworkDebugger.inputProfiles[index2].name == "")
                  {
                    bool flag = false;
                    foreach (NetDebugInterface netDebugInterface in NetworkDebugger._interfaces)
                    {
                      if (netDebugInterface.visible)
                      {
                        flag = true;
                        break;
                      }
                    }
                    if (!flag)
                    {
                      init = index1;
                      inputProfile1 = defaultProfile;
                    }
                    NetworkDebugger.inputProfiles[index2] = defaultProfile;
                    break;
                  }
                }
              }
            }
            if (init != -1)
            {
              this.CreateInstance(init, false);
              NetworkDebugger._instances[index1].ipro = inputProfile1;
            }
          }
        }
        else
        {
          NetworkInstance instance = NetworkDebugger._instances[index1];
          NetworkDebugger._networkDrawingIndex = index1;
          this.LockInstance(instance);
          float volume = SFX.volume;
          if (NetworkDebugger.Mute(NetworkDebugger._networkDrawingIndex))
            SFX.volume = 0.0f;
          InputProfile.Update();
          foreach (DCLine dcLine in dcLineList)
          {
            if (dcLine.threadIndex == index1)
              DevConsole.core.lines.Enqueue(dcLine);
          }
          DevConsole.Update();
          Network.PreUpdate();
          FireManager.Update();
          Level.UpdateLevelChange();
          Level.UpdateCurrentLevel();
          MonoMain.UpdatePauseMenu();
          int index2 = 0;
          foreach (NetDebugInterface netDebugInterface in NetworkDebugger._interfaces)
          {
            MonoMain.instance.IsMouseVisible = true;
            if (index2 == index1 && index2 < NetworkDebugger._instances.Count)
              netDebugInterface.Update(NetworkDebugger._instances[index2].network);
            ++index2;
          }
          Network.PostUpdate();
          if (DuckNetwork.status == DuckNetStatus.Disconnected)
          {
            NetworkDebugger._instances[index1].active = false;
            foreach (Profile profile in NetworkDebugger._instances[index1].duckNetworkCore.profiles)
            {
              for (int index3 = 0; index3 < 4; ++index3)
              {
                if (NetworkDebugger.inputProfiles[index3] == profile.inputProfile || NetworkDebugger.inputProfiles[index3] == NetworkDebugger._instances[index1].ipro)
                  NetworkDebugger.inputProfiles[index3] = new InputProfile();
              }
            }
          }
          SFX.volume = volume;
          this.UnlockInstance();
        }
      }
      if (!Keyboard.Pressed(Keys.F11))
        return;
      foreach (NetworkInstance instance in NetworkDebugger._instances)
        instance.network.core.ForcefulTermination();
      Network.activeNetwork.core.ForcefulTermination();
      NetworkDebugger._instances.Clear();
      NetworkDebugger.inputProfiles.Clear();
      Level.current = (Level) new NetworkDebugger();
    }

    public override void DoDraw()
    {
      if (NetworkDebugger._instances.Count == 0)
        return;
      if (NetworkDebugger._instances.Count == 1)
      {
        this.LockInstance(NetworkDebugger._instances[0]);
        NetworkDebugger._networkDrawingIndex = 0;
        Level.current.clearScreen = true;
        Level.DrawCurrentLevel();
        Network.netGraph.Draw();
        this.UnlockInstance();
      }
      else
      {
        int num = -1;
        foreach (NetworkInstance instance in NetworkDebugger._instances)
        {
          ++num;
          if (instance.active)
          {
            NetworkDebugger._networkDrawingIndex = num;
            this.LockInstance(instance);
            Layer.HUD.camera.width = 320f;
            Layer.HUD.camera.height = 180f;
            Viewport viewport = DuckGame.Graphics.viewport;
            switch (num)
            {
              case 0:
                DuckGame.Graphics.viewport = new Viewport(0, 0, viewport.Width / 2, viewport.Height / 2);
                break;
              case 1:
                DuckGame.Graphics.viewport = new Viewport(viewport.Width / 2, 0, viewport.Width / 2, viewport.Height / 2);
                break;
              case 2:
                DuckGame.Graphics.viewport = new Viewport(0, viewport.Height / 2, viewport.Width / 2, viewport.Height / 2);
                break;
              case 3:
                DuckGame.Graphics.viewport = new Viewport(viewport.Width / 2, viewport.Height / 2, viewport.Width / 2, viewport.Height / 2);
                break;
            }
            Level.current.clearScreen = num == 0;
            Level.DrawCurrentLevel();
            if (MonoMain.pauseMenu != null)
            {
              Layer.HUD.Begin(true);
              MonoMain.pauseMenu.Draw();
              foreach (Thing thing in MonoMain.closeMenuUpdate)
                thing.Draw();
              HUD.Draw();
              Layer.HUD.End(true);
            }
            Network.netGraph.Draw();
            FPSCounter.Tick(NetworkDebugger.networkDrawingIndex);
            FPSCounter.Render(DuckGame.Graphics.device, index: NetworkDebugger.networkDrawingIndex);
            DuckGame.Graphics.viewport = viewport;
            this.UnlockInstance();
          }
        }
      }
      this.clearScreen = false;
      base.DoDraw();
    }

    public static void LogSend(string from, string to)
    {
      if (!NetworkDebugger._sentPulse.ContainsKey(from))
        NetworkDebugger._sentPulse[from] = new Dictionary<string, float>();
      if (!NetworkDebugger._sentPulse[from].ContainsKey(to))
        NetworkDebugger._sentPulse[from][to] = 0.0f;
      Dictionary<string, float> dictionary;
      string key;
      (dictionary = NetworkDebugger._sentPulse[from])[key = to] = dictionary[key] + 1f;
    }

    public static float GetSent(string key, string to)
    {
      if (!NetworkDebugger._sentPulse.ContainsKey(key) || !NetworkDebugger._sentPulse[key].ContainsKey(to))
        return 0.0f;
      if ((double) NetworkDebugger._sentPulse[key][to] > 1.0)
        NetworkDebugger._sentPulse[key][to] = 1f;
      Dictionary<string, float> dictionary;
      string key1;
      (dictionary = NetworkDebugger._sentPulse[key])[key1 = to] = dictionary[key1] - 0.1f;
      if ((double) NetworkDebugger._sentPulse[key][to] < 0.0)
        NetworkDebugger._sentPulse[key][to] = 0.0f;
      return NetworkDebugger._sentPulse[key][to];
    }

    public static void LogReceive(string to, string from)
    {
      if (!NetworkDebugger._receivedPulse.ContainsKey(to))
        NetworkDebugger._receivedPulse[to] = new Dictionary<string, float>();
      if (!NetworkDebugger._receivedPulse[to].ContainsKey(from))
        NetworkDebugger._receivedPulse[to][from] = 0.0f;
      Dictionary<string, float> dictionary;
      string key;
      (dictionary = NetworkDebugger._receivedPulse[to])[key = from] = dictionary[key] + 1f;
    }

    public static float GetReceived(string key, string from)
    {
      if (!NetworkDebugger._receivedPulse.ContainsKey(key) || !NetworkDebugger._receivedPulse[key].ContainsKey(from))
        return 0.0f;
      if ((double) NetworkDebugger._receivedPulse[key][from] > 1.0)
        NetworkDebugger._receivedPulse[key][from] = 1f;
      Dictionary<string, float> dictionary;
      string key1;
      (dictionary = NetworkDebugger._receivedPulse[key])[key1 = from] = dictionary[key1] - 0.1f;
      if ((double) NetworkDebugger._receivedPulse[key][from] < 0.0)
        NetworkDebugger._receivedPulse[key][from] = 0.0f;
      return NetworkDebugger._receivedPulse[key][from];
    }

    public static string GetID(int index)
    {
      switch (index)
      {
        case 0:
          return "127.0.0.1:1337";
        case 1:
          return "127.0.0.1:1338";
        case 2:
          return "127.0.0.1:1339";
        case 3:
          return "127.0.0.1:1340";
        default:
          return "";
      }
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (NetworkDebugger._instances.Count == 0)
        return;
      if (layer == Layer.Console)
      {
        if (NetworkDebugger._interfaces[0].pin || (double) Mouse.xConsole < (double) Layer.Console.width / 2.0 && (double) Mouse.yConsole < (double) Layer.Console.height / 2.0)
        {
          DevConsoleCore core = DevConsole.core;
          DevConsole.core = NetworkDebugger._instances[0].consoleCore;
          NetworkDebugger._interfaces[0].Draw(NetworkDebugger._instances[0].network);
          DevConsole.core = core;
        }
        else
          NetworkDebugger._interfaces[0].visible = false;
        if (NetworkDebugger._interfaces[1].pin || (double) Mouse.xConsole > (double) Layer.Console.width / 2.0 && (double) Mouse.yConsole < (double) Layer.Console.height / 2.0)
        {
          if (NetworkDebugger._instances.Count > 1)
          {
            DevConsoleCore core = DevConsole.core;
            DevConsole.core = NetworkDebugger._instances[1].consoleCore;
            NetworkDebugger._interfaces[1].Draw(NetworkDebugger._instances[1].network);
            DevConsole.core = core;
          }
        }
        else
          NetworkDebugger._interfaces[1].visible = false;
        if (NetworkDebugger._interfaces[2].pin || (double) Mouse.xConsole < (double) Layer.Console.width / 2.0 && (double) Mouse.yConsole > (double) Layer.Console.height / 2.0)
        {
          if (NetworkDebugger._instances.Count > 2)
          {
            DevConsoleCore core = DevConsole.core;
            DevConsole.core = NetworkDebugger._instances[2].consoleCore;
            NetworkDebugger._interfaces[2].Draw(NetworkDebugger._instances[2].network);
            DevConsole.core = core;
          }
        }
        else
          NetworkDebugger._interfaces[2].visible = false;
        if (NetworkDebugger._interfaces[3].pin || (double) Mouse.xConsole > (double) Layer.Console.width / 2.0 && (double) Mouse.yConsole > (double) Layer.Console.height / 2.0)
        {
          if (NetworkDebugger._instances.Count > 3)
          {
            DevConsoleCore core = DevConsole.core;
            DevConsole.core = NetworkDebugger._instances[3].consoleCore;
            NetworkDebugger._interfaces[3].Draw(NetworkDebugger._instances[3].network);
            DevConsole.core = core;
          }
        }
        else
          NetworkDebugger._interfaces[3].visible = false;
        if (this._connectionArrow == null)
        {
          this._connectionArrow = new SpriteMap("connectionArrow", 56, 13);
          this._connectionArrow.CenterOrigin();
          this._connectionX = new Sprite("connectionX");
          this._connectionX.CenterOrigin();
        }
        for (int i = 0; i < 4; ++i)
        {
          if (NetworkDebugger._instances.Count > i && NetworkDebugger._instances[i].active)
          {
            for (int j = 0; j < 4; ++j)
            {
              if (i != j && NetworkDebugger._instances.Count > j && NetworkDebugger._instances[j].active)
              {
                NetworkConnection networkConnection1 = NetworkDebugger._instances[i].network.core.connections.FirstOrDefault<NetworkConnection>((Func<NetworkConnection, bool>) (x => x.identifier == NetworkDebugger.GetID(j)));
                NetworkConnection networkConnection2 = NetworkDebugger._instances[j].network.core.connections.FirstOrDefault<NetworkConnection>((Func<NetworkConnection, bool>) (x => x.identifier == NetworkDebugger.GetID(i)));
                Vec2 vec2_1 = NetworkDebugger._interfaces[i].position + new Vec2((float) (DuckGame.Graphics.width / 4 - 10), (float) (DuckGame.Graphics.height / 4 - 10));
                Vec2 vec2_2 = NetworkDebugger._interfaces[j].position + new Vec2((float) (DuckGame.Graphics.width / 4 - 10), (float) (DuckGame.Graphics.height / 4 - 10));
                Vec2 p2 = vec2_2 - vec2_1;
                Vec2 vec2_3 = p2.normalized.Rotate(Maths.DegToRad(90f), Vec2.Zero);
                float num1 = -Maths.PointDirection(Vec2.Zero, p2);
                float num2 = 0.0f;
                if (networkConnection1 != null)
                {
                  float num3 = 0.0f;
                  float num4 = 0.0f;
                  if (networkConnection2 != null)
                  {
                    num3 = NetworkDebugger.GetReceived(networkConnection1.identifier, networkConnection2.identifier);
                    num4 = NetworkDebugger.GetSent(networkConnection1.identifier, networkConnection2.identifier);
                    num2 = 6f;
                  }
                  this._connectionArrow.frame = networkConnection1.status != ConnectionStatus.Connected ? 0 : 3;
                  this._connectionArrow.angleDegrees = num1;
                  this._connectionArrow.alpha = 1f;
                  this._connectionArrow.depth = (Depth) 0.92f;
                  Vec2 vec2_4 = (vec2_1 + vec2_2) / 2f + vec2_3 * num2;
                  DuckGame.Graphics.Draw((Sprite) this._connectionArrow, vec2_4.x, vec2_4.y);
                  if (networkConnection1.status != ConnectionStatus.Disconnecting)
                  {
                    this._connectionX.depth = (Depth) 0.98f;
                    Vec2 vec2_5 = vec2_4 - p2.normalized * 22f;
                    if ((double) (Mouse.positionConsole - vec2_5).length < 8.0)
                    {
                      this._connectionX.alpha = 1f;
                      if (this.lefpres)
                      {
                        networkConnection1.Disconnect();
                        SFX.Play("quack");
                      }
                    }
                    else
                      this._connectionX.alpha = 0.4f;
                    DuckGame.Graphics.Draw(this._connectionX, vec2_5.x, vec2_5.y);
                  }
                  if ((double) num4 > 0.0)
                  {
                    ++this._connectionArrow.frame;
                    this._connectionArrow.alpha = num4;
                    this._connectionArrow.depth = (Depth) 0.95f;
                    DuckGame.Graphics.Draw((Sprite) this._connectionArrow, vec2_4.x, vec2_4.y);
                    --this._connectionArrow.frame;
                  }
                  if ((double) num3 > 0.0)
                  {
                    this._connectionArrow.frame += 2;
                    this._connectionArrow.alpha = num3;
                    this._connectionArrow.depth = (Depth) 0.95f;
                    DuckGame.Graphics.Draw((Sprite) this._connectionArrow, vec2_4.x, vec2_4.y);
                    this._connectionArrow.frame -= 2;
                  }
                }
              }
            }
          }
        }
      }
      base.PostDrawLayer(layer);
    }
  }
}
