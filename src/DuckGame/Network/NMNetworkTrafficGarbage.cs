﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMNetworkTrafficGarbage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMNetworkTrafficGarbage : NMEvent
  {
    private int _numBytes;

    public NMNetworkTrafficGarbage(int numBytes) => this._numBytes = numBytes;

    public NMNetworkTrafficGarbage()
    {
    }

    protected override void OnSerialize()
    {
      if (this._numBytes < 4)
        this._numBytes = 4;
      this._serializedData.Write(this._numBytes);
      for (int index = 0; index < this._numBytes - 4; ++index)
        this._serializedData.Write((byte) Rando.Int((int) byte.MaxValue));
      base.OnSerialize();
    }

    public override void OnDeserialize(BitBuffer msg)
    {
      this._numBytes = msg.ReadInt();
      for (int index = 0; index < this._numBytes - 4; ++index)
      {
        int num = (int) msg.ReadByte();
      }
      base.OnDeserialize(msg);
    }
  }
}
