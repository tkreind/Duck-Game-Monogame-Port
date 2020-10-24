// Decompiled with JetBrains decompiler
// Type: DuckGame.NMFullGhostState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DuckGame
{
  public class NMFullGhostState : NetMessage
  {
    private List<GhostObject> _ghosts = new List<GhostObject>();
    public byte levelIndex;
    public List<NMGhostState> states = new List<NMGhostState>();

    public NMFullGhostState(BitBuffer dat) => this.manager = BelongsToManager.GhostManager;

    public NMFullGhostState() => this.manager = BelongsToManager.GhostManager;

    public void Add(GhostObject g) => this._ghosts.Add(g);

    protected override void OnSerialize()
    {
      BitBuffer bitBuffer = new BitBuffer();
      List<GhostObject> ghostObjectList = new List<GhostObject>();
      foreach (GhostObject ghostObject in (IEnumerable<GhostObject>) this._ghosts.OrderBy<GhostObject, ushort>((Func<GhostObject, ushort>) (x => x.thing.ghostType)))
      {
        if (ghostObject.thing.ghostType != (ushort) 0)
          ghostObjectList.Add(ghostObject);
      }
      bitBuffer.Write(DuckNetwork.levelIndex);
      if (this._ghosts.Count > 0)
      {
        bitBuffer.Write(true);
        ushort val = ushort.MaxValue;
        foreach (GhostObject ghostObject in ghostObjectList)
        {
          if (val == ushort.MaxValue || (int) val != (int) ghostObject.thing.ghostType)
          {
            if (val != ushort.MaxValue)
            {
              bitBuffer.Write(false);
              bitBuffer.Write(true);
            }
            val = ghostObject.thing.ghostType;
            bitBuffer.Write(val);
          }
          else
            bitBuffer.Write(true);
          BitBuffer networkStateData = ghostObject.GetNetworkStateData((NetworkConnection) null, true);
          bitBuffer.Write(networkStateData, true);
        }
        bitBuffer.Write(false);
        bitBuffer.Write(false);
      }
      else
        bitBuffer.Write(false);
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress));
      binaryWriter.Write((ushort) bitBuffer.lengthInBytes);
      binaryWriter.Write(bitBuffer.buffer, 0, bitBuffer.lengthInBytes);
      binaryWriter.Close();
      byte[] array = memoryStream.ToArray();
      this._serializedData.Write((ushort) array.Length);
      this._serializedData.Write(array, 0, -1);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      ushort num1 = d.ReadUShort();
      BinaryReader binaryReader = new BinaryReader((Stream) new GZipStream((Stream) new MemoryStream(d.ReadPacked((int) num1)), CompressionMode.Decompress));
      ushort num2 = binaryReader.ReadUInt16();
      BitBuffer bitBuffer1 = new BitBuffer(binaryReader.ReadBytes((int) num2));
      this.levelIndex = bitBuffer1.ReadByte();
      if (!bitBuffer1.ReadBool())
        return;
      do
      {
        ushort num3 = bitBuffer1.ReadUShort();
        do
        {
          BitBuffer bitBuffer2 = bitBuffer1.ReadBitBuffer();
          NMGhostState nmGhostState = new NMGhostState();
          nmGhostState.levelIndex = this.levelIndex;
          nmGhostState.mask = long.MaxValue;
          nmGhostState.connection = this.connection;
          nmGhostState.id = (NetIndex16) (int) bitBuffer2.ReadUShort();
          nmGhostState.classID = num3;
          nmGhostState.data = bitBuffer2.ReadBitBuffer();
          nmGhostState.minimalState = true;
          this.states.Add(nmGhostState);
        }
        while (bitBuffer1.ReadBool());
      }
      while (bitBuffer1.ReadBool());
    }
  }
}
