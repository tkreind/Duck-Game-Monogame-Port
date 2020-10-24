// Decompiled with JetBrains decompiler
// Type: DuckGame.BlockCorner
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class BlockCorner
  {
    public Vec2 corner;
    public Block block;
    public bool wallCorner;
    public List<BlockCorner> testedCorners = new List<BlockCorner>();

    public BlockCorner(Vec2 c, Block b, bool wall = false)
    {
      this.corner = c;
      this.block = b;
      this.wallCorner = wall;
    }

    public BlockCorner Copy() => new BlockCorner(this.corner, this.block, this.wallCorner);
  }
}
