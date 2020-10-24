// Decompiled with JetBrains decompiler
// Type: DuckGame.NMProfileInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMProfileInfo : NMDuckNetworkEvent
  {
    public byte index;
    public int fans;
    public int loyalFans;

    public NMProfileInfo()
    {
    }

    public NMProfileInfo(byte idx, int numFans, int numLoyalFans)
    {
      this.index = idx;
      this.fans = numFans;
      this.loyalFans = numLoyalFans;
    }

    public override void Activate()
    {
      if (this.index >= (byte) 0 && this.index < (byte) 4)
      {
        DuckNetwork.profiles[(int) this.index].stats.unloyalFans = this.fans;
        DuckNetwork.profiles[(int) this.index].stats.loyalFans = this.loyalFans;
      }
      base.Activate();
    }
  }
}
