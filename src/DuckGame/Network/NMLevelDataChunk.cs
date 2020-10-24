// Decompiled with JetBrains decompiler
// Type: DuckGame.NMLevelDataChunk
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMLevelDataChunk : NMDuckNetwork
  {
    public ushort transferSession;
    private BitBuffer _buffer;

    public BitBuffer GetBuffer() => this._buffer;

    public NMLevelDataChunk(ushort tSession, BitBuffer dat)
    {
      this.transferSession = tSession;
      this._buffer = dat;
    }

    public NMLevelDataChunk()
    {
    }

    public override void MessageWasReceived() => this.connection.dataTransferProgress += this._buffer.lengthInBytes;

    protected override void OnSerialize()
    {
      this.serializedData.Write(this._buffer, true);
      base.OnSerialize();
    }

    public override void OnDeserialize(BitBuffer msg)
    {
      this._buffer = msg.ReadBitBuffer();
      base.OnDeserialize(msg);
    }
  }
}
