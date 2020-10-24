// Decompiled with JetBrains decompiler
// Type: DuckGame.NMTeamSetDenied
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMTeamSetDenied : NMDuckNetwork
  {
    public byte duck;
    public byte team;

    public NMTeamSetDenied(byte d, byte t)
    {
      this.duck = d;
      this.team = t;
    }

    public NMTeamSetDenied()
    {
    }
  }
}
