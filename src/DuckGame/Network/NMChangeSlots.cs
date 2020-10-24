// Decompiled with JetBrains decompiler
// Type: DuckGame.NMChangeSlots
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [FixedNetworkID(30231)]
  public class NMChangeSlots : NMDuckNetworkEvent
  {
    public byte slot1;
    public byte slot2;
    public byte slot3;
    public byte slot4;

    public NMChangeSlots()
    {
    }

    public NMChangeSlots(byte varslot1, byte varslot2, byte varslot3, byte varslot4)
    {
      this.slot1 = varslot1;
      this.slot2 = varslot2;
      this.slot3 = varslot3;
      this.slot4 = varslot4;
    }

    public override void Activate()
    {
      if (!Network.isServer)
      {
        DuckNetwork.profiles[0].slotType = (SlotType) this.slot1;
        DuckNetwork.profiles[1].slotType = (SlotType) this.slot2;
        DuckNetwork.profiles[2].slotType = (SlotType) this.slot3;
        DuckNetwork.profiles[3].slotType = (SlotType) this.slot4;
      }
      base.Activate();
    }
  }
}
