// Decompiled with JetBrains decompiler
// Type: DuckGame.NMRemoteJoinDuckNetwork
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMRemoteJoinDuckNetwork : NMJoinDuckNetwork
  {
    public string identifier;
    public string name;
    public bool hasCustomHats;
    public byte team;
    public byte flippers;

    public NMRemoteJoinDuckNetwork()
    {
    }

    public NMRemoteJoinDuckNetwork(
      byte varDuckIndex,
      string id,
      string duckName,
      bool varHasCustomHats,
      byte varTeam,
      byte varFlippers)
      : base(varDuckIndex)
    {
      this.identifier = id;
      this.name = duckName;
      this.hasCustomHats = varHasCustomHats;
      this.team = varTeam;
      this.flippers = varFlippers;
    }
  }
}
