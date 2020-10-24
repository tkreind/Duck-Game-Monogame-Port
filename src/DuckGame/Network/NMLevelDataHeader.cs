// Decompiled with JetBrains decompiler
// Type: DuckGame.NMLevelDataHeader
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMLevelDataHeader : NMDuckNetwork
  {
    public ushort transferSession;
    public int length;

    public NMLevelDataHeader(ushort tSession, int dataLength)
    {
      this.transferSession = tSession;
      this.length = dataLength;
    }

    public NMLevelDataHeader()
    {
    }

    public override void MessageWasReceived() => this.connection.dataTransferSize = this.length;
  }
}
