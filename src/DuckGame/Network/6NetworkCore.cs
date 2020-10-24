// Decompiled with JetBrains decompiler
// Type: DuckGame.NMCheckVersion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [FixedNetworkID(41)]
  public class NMCheckVersion : NMNetworkCoreMessage
  {
    public string version;

    public NMCheckVersion()
    {
    }

    public NMCheckVersion(string v) => this.version = v;
  }
}
