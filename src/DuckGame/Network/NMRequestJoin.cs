// Decompiled with JetBrains decompiler
// Type: DuckGame.NMRequestJoin
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [FixedNetworkID(14)]
  public class NMRequestJoin : NMDuckNetwork
  {
    public string id;
    public string name;
    public byte number;
    public int randomID;
    public bool hasCustomHats;
    public bool wasInvited;
    public byte flippers;

    public NMRequestJoin()
    {
    }

    public NMRequestJoin(
      string buildID,
      int randoID,
      byte varFlippers,
      string n = "",
      byte num = 1,
      bool varHasCustomHats = false,
      bool wasInvited = false)
    {
      this.id = buildID;
      this.name = n;
      this.number = num;
      this.randomID = randoID;
      this.hasCustomHats = varHasCustomHats;
      this.flippers = varFlippers;
    }
  }
}
