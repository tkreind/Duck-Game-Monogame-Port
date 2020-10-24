// Decompiled with JetBrains decompiler
// Type: DuckGame.NetworkInstance
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NetworkInstance
  {
    public Network network;
    public InputProfileCore inputProfile;
    public float waitJoin = 1f;
    public bool joined;
    public DuckNetworkCore duckNetworkCore;
    public TeamsCore teamsCore;
    public LayerCore layerCore;
    public VirtualTransitionCore virtualCore;
    public LevelCore levelCore;
    public ProfilesCore profileCore;
    public DevConsoleCore consoleCore;
    public CrowdCore crowdCore;
    public GameModeCore gameModeCore;
    public MonoMainCore monoCore;
    public ConnectionStatusUICore connectionUICore;
    public float fade = 1f;
    public InputProfile ipro;
    public bool active = true;
  }
}
