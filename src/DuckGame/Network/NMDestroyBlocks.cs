// Decompiled with JetBrains decompiler
// Type: DuckGame.NMDestroyBlocks
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class NMDestroyBlocks : NMEvent
  {
    public HashSet<ushort> blocks = new HashSet<ushort>();
    private byte _levelIndex;

    public NMDestroyBlocks(HashSet<ushort> varBlocks) => this.blocks = varBlocks;

    public NMDestroyBlocks()
    {
    }

    public override void Activate()
    {
      if (!(Level.current is GameLevel) || (int) DuckNetwork.levelIndex != (int) this._levelIndex)
        return;
      foreach (BlockGroup blockGroup in Level.current.things[typeof (BlockGroup)])
      {
        bool flag = false;
        using (HashSet<ushort>.Enumerator enumerator = this.blocks.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ushort u = enumerator.Current;
            Block block = blockGroup.blocks.FirstOrDefault<Block>((Func<Block, bool>) (x => x is AutoBlock && (int) (x as AutoBlock).blockIndex == (int) u));
            if (block != null)
            {
              block.shouldWreck = true;
              flag = true;
            }
          }
        }
        if (flag)
          blockGroup.Wreck();
      }
      foreach (AutoBlock autoBlock in Level.current.things[typeof (AutoBlock)])
      {
        if (this.blocks.Contains(autoBlock.blockIndex))
        {
          this.blocks.Remove(autoBlock.blockIndex);
          autoBlock.shouldWreck = true;
          autoBlock.skipWreck = true;
        }
      }
    }

    protected override void OnSerialize()
    {
      base.OnSerialize();
      this._serializedData.Write(DuckNetwork.levelIndex);
      this._serializedData.Write((byte) this.blocks.Count);
      foreach (ushort block in this.blocks)
        this._serializedData.Write(block);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      base.OnDeserialize(d);
      this._levelIndex = d.ReadByte();
      byte num = d.ReadByte();
      for (int index = 0; index < (int) num; ++index)
        this.blocks.Add(d.ReadUShort());
    }
  }
}
