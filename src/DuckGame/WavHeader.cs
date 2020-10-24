// Decompiled with JetBrains decompiler
// Type: DuckGame.WavHeader
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct WavHeader
  {
    public byte[] riffID;
    public uint size;
    public byte[] wavID;
    public byte[] fmtID;
    public uint fmtSize;
    public ushort format;
    public ushort channels;
    public uint sampleRate;
    public uint bytePerSec;
    public ushort blockSize;
    public ushort bit;
    public byte[] dataID;
    public uint dataSize;
  }
}
