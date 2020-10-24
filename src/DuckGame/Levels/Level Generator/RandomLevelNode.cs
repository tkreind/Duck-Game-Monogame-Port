// Decompiled with JetBrains decompiler
// Type: DuckGame.RandomLevelNode
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class RandomLevelNode
  {
    private const float _width = 192f;
    private const float _height = 144f;
    public RandomLevelNode[,] map;
    public int seed;
    public RandomLevelNode up;
    public RandomLevelNode down;
    public RandomLevelNode left;
    public RandomLevelNode right;
    public bool isCentered;
    public bool symmetric;
    public bool mirror;
    public RandomLevelNode symmetricalPartner;
    private RandomLevelData _combinedData;
    public RandomLevelNode[,] tiles;
    public int tilesWide;
    public int tilesHigh;
    public bool kingTile;
    private bool leftSymmetric;
    private bool rightSymmetric;
    private bool connectionUp;
    private bool connectionDown;
    private bool connectionLeft;
    private bool connectionRight;
    private bool removeLeft;
    private bool removeRight;
    public RandomLevelData data;
    public bool visited;

    public List<RandomLevelNode> nodes => new List<RandomLevelNode>()
    {
      this.up,
      this.down,
      this.left,
      this.right
    };

    public RandomLevelData totalData
    {
      get
      {
        this._combinedData = this.Combine();
        this.ClearFlags();
        return this._combinedData;
      }
    }

    private RandomLevelData Combine()
    {
      this.visited = true;
      RandomLevelData dat = new RandomLevelData();
      if (this.left != null && this.left.data != null && !this.left.visited)
        dat = this.left.data.Combine(dat);
      if (this.right != null && this.right.data != null && !this.right.visited)
        dat = this.right.data.Combine(dat);
      if (this.up != null && this.up.data != null && !this.up.visited)
        dat = this.up.data.Combine(dat);
      if (this.down != null && this.down.data != null && !this.down.visited)
        dat = this.down.data.Combine(dat);
      return dat.Combine(this.data);
    }

    public void ClearFlags()
    {
      this.visited = false;
      if (this.up != null && this.up.visited)
        this.up.ClearFlags();
      if (this.down != null && this.down.visited)
        this.down.ClearFlags();
      if (this.left != null && this.left.visited)
        this.left.ClearFlags();
      if (this.right == null || !this.right.visited)
        return;
      this.right.ClearFlags();
    }

    public bool Reroll(Func<RandomLevelData, bool> requirements)
    {
      TileConnection requirement = TileConnection.None;
      if (this.data.up && this.up != null && this.up.data != null)
        requirement |= TileConnection.Up;
      if (this.data.down && this.down != null && this.down.data != null)
        requirement |= TileConnection.Down;
      if (this.data.left && this.left != null && this.left.data != null)
        requirement |= TileConnection.Left;
      if (this.data.right && this.right != null && this.right.data != null)
        requirement |= TileConnection.Right;
      RandomLevelData tile = LevelGenerator.GetTile(requirement, this.data, type: LevGenType.Deathmatch, lambdaReq: requirements);
      if (tile == null)
        return false;
      this.data = tile;
      if (this.symmetricalPartner != null)
        this.symmetricalPartner.data = tile.Flipped();
      return true;
    }

    public void LoadParts(float x, float y, Level level, int seed = 0)
    {
      Random generator = Rando.generator;
      if (seed != 0)
        Rando.generator = new Random(seed);
      Level.InitChanceGroups();
      this.LoadPartsRecurse(x, y, level);
      this.ClearFlags();
      Rando.generator = generator;
      for (int index1 = -1; index1 < this.tilesWide + 1; ++index1)
      {
        for (int index2 = -1; index2 < this.tilesHigh + 1; ++index2)
        {
          RandomLevelNode randomLevelNode = (RandomLevelNode) null;
          if (index1 >= 0 && index1 < this.tilesWide && (index2 >= 0 && index2 < this.tilesHigh))
            randomLevelNode = this.tiles[index1, index2];
          if (randomLevelNode == null || randomLevelNode.data == null)
            level.AddThing((Thing) new PyramidWall((float) (index1 * 192 - 192 - 8), (float) (index2 * 144 - 144 - 8)));
        }
      }
      level.things.RefreshState();
      foreach (PyramidDoor pyramidDoor in level.things[typeof (PyramidDoor)])
      {
        switch (level.CollisionLine<Block>(pyramidDoor.position + new Vec2(-16f, 0.0f), pyramidDoor.position + new Vec2(16f, 0.0f), (Thing) pyramidDoor))
        {
          case null:
          case PyramidDoor _:
          case Door _:
            continue;
          default:
            level.RemoveThing((Thing) pyramidDoor);
            Level.Add((Thing) new PyramidTileset(pyramidDoor.x, pyramidDoor.y - 16f));
            Level.Add((Thing) new PyramidTileset(pyramidDoor.x, pyramidDoor.y));
            continue;
        }
      }
      foreach (Door door in level.things[typeof (Door)])
      {
        switch (level.CollisionLine<Block>(door.position + new Vec2(-16f, 0.0f), door.position + new Vec2(16f, 0.0f), (Thing) door))
        {
          case null:
          case PyramidDoor _:
          case Door _:
            continue;
          default:
            level.RemoveThing((Thing) door);
            Level.Add((Thing) new PyramidTileset(door.x, door.y - 16f));
            Level.Add((Thing) new PyramidTileset(door.x, door.y));
            continue;
        }
      }
      LightingTwoPointOH lightingTwoPointOh = new LightingTwoPointOH();
      lightingTwoPointOh.visible = false;
      level.AddThing((Thing) lightingTwoPointOh);
    }

    private void LoadPartsRecurse(float x, float y, Level level)
    {
      this.visited = true;
      if (this.data != null)
        this.data.Load(x, y, level, this.mirror);
      if (this.up != null && !this.up.visited)
        this.up.LoadPartsRecurse(x, y - 144f, level);
      if (this.down != null && !this.down.visited)
        this.down.LoadPartsRecurse(x, y + 144f, level);
      if (this.left != null && !this.left.visited)
        this.left.LoadPartsRecurse(x - 192f, y, level);
      if (this.right == null || this.right.visited)
        return;
      this.right.LoadPartsRecurse(x + 192f, y, level);
    }

    public void GenerateTiles(RandomLevelData tile = null, LevGenType type = LevGenType.Any, bool symmetricVal = false)
    {
      this.symmetric = symmetricVal;
      this.GenerateTilesRecurse(tile != null ? tile : LevelGenerator.GetTile(TileConnection.None, tile, false, type, requiresSpawns: true), type);
      this.ClearFlags();
    }

    private TileConnection GetFilter()
    {
      TileConnection tileConnection = TileConnection.None;
      if (this.left == null)
        tileConnection |= TileConnection.Left;
      if (this.right == null)
        tileConnection |= TileConnection.Right;
      if (this.up == null)
        tileConnection |= TileConnection.Up;
      if (this.down == null)
        tileConnection |= TileConnection.Down;
      return tileConnection;
    }

    private void GenerateTilesRecurse(RandomLevelData tile, LevGenType type = LevGenType.Any)
    {
      this.visited = true;
      if (tile == null)
        return;
      this.data = tile;
      this.connectionUp = this.data.up;
      this.connectionDown = this.data.down;
      this.connectionLeft = this.data.left;
      this.connectionRight = this.data.right;
      if (this.symmetric)
      {
        if (this.kingTile)
        {
          if (this.connectionLeft && this.connectionRight || !this.connectionLeft && !this.connectionRight)
          {
            this.mirror = true;
          }
          else
          {
            if (!this.connectionLeft)
            {
              if (this.up != null)
              {
                this.up.left = (RandomLevelNode) null;
                this.up.removeRight = true;
              }
              if (this.down != null)
              {
                this.down.left = (RandomLevelNode) null;
                this.down.removeRight = true;
              }
              this.removeRight = true;
              this.left = (RandomLevelNode) null;
            }
            if (!this.connectionRight)
            {
              if (this.up != null)
              {
                this.up.right = (RandomLevelNode) null;
                this.up.removeLeft = true;
              }
              if (this.down != null)
              {
                this.down.right = (RandomLevelNode) null;
                this.down.removeLeft = true;
              }
              this.removeLeft = true;
              this.right = (RandomLevelNode) null;
            }
          }
        }
        if (this.mirror)
          this.connectionRight = this.data.left;
        if (this.up != null)
          this.up.mirror = this.mirror;
        if (this.down != null)
          this.down.mirror = this.mirror;
      }
      List<TileConnection> source = new List<TileConnection>()
      {
        TileConnection.Right,
        TileConnection.Left,
        TileConnection.Up,
        TileConnection.Down
      };
      if (this.removeLeft)
        source.Remove(TileConnection.Left);
      if (this.removeRight)
        source.Remove(TileConnection.Right);
      foreach (TileConnection tileConnection in (IEnumerable<TileConnection>) source.OrderBy<TileConnection, float>((Func<TileConnection, float>) (x => Rando.Float(1f))))
      {
        switch (tileConnection)
        {
          case TileConnection.Left:
            if (this.connectionLeft && this.left != null && this.left.data == null && (!this.mirror || !this.symmetric || !this.rightSymmetric))
            {
              if (this.mirror && this.symmetric)
              {
                this.leftSymmetric = true;
                if (this.down != null)
                {
                  this.down.leftSymmetric = this.leftSymmetric;
                  if (this.down.down != null)
                    this.down.down.leftSymmetric = this.leftSymmetric;
                }
                if (this.up != null)
                {
                  this.up.leftSymmetric = this.leftSymmetric;
                  if (this.up.up != null)
                    this.up.up.leftSymmetric = this.leftSymmetric;
                }
              }
              this.left.leftSymmetric = this.leftSymmetric;
              this.left.rightSymmetric = this.rightSymmetric;
              this.left.symmetric = this.symmetric;
              this.left.GenerateTilesRecurse(LevelGenerator.GetTile(TileConnection.Right, tile, type: type, mirror: this.left.mirror, filter: this.left.GetFilter()), type);
              continue;
            }
            continue;
          case TileConnection.Right:
            if (this.connectionRight && this.right != null && this.right.data == null && (!this.mirror || !this.symmetric || !this.leftSymmetric))
            {
              if (this.mirror && this.symmetric)
              {
                this.rightSymmetric = true;
                if (this.down != null)
                {
                  this.down.rightSymmetric = this.rightSymmetric;
                  if (this.down.down != null)
                    this.down.down.rightSymmetric = this.rightSymmetric;
                }
                if (this.up != null)
                {
                  this.up.rightSymmetric = this.rightSymmetric;
                  if (this.up.up != null)
                    this.up.up.rightSymmetric = this.rightSymmetric;
                }
              }
              this.right.leftSymmetric = this.leftSymmetric;
              this.right.rightSymmetric = this.rightSymmetric;
              this.right.symmetric = this.symmetric;
              this.right.GenerateTilesRecurse(LevelGenerator.GetTile(TileConnection.Left, tile, type: type, mirror: this.right.mirror, filter: this.right.GetFilter()), type);
              continue;
            }
            continue;
          case TileConnection.Up:
            if (this.connectionUp && this.up != null && this.up.data == null)
            {
              this.up.leftSymmetric = this.leftSymmetric;
              this.up.rightSymmetric = this.rightSymmetric;
              this.up.symmetric = this.symmetric;
              this.up.GenerateTilesRecurse(LevelGenerator.GetTile(TileConnection.Down, tile, type: type, mirror: this.mirror, filter: this.up.GetFilter()), type);
              continue;
            }
            continue;
          case TileConnection.Down:
            if (this.connectionDown && this.down != null && this.down.data == null)
            {
              this.down.leftSymmetric = this.leftSymmetric;
              this.down.rightSymmetric = this.rightSymmetric;
              this.down.symmetric = this.symmetric;
              this.down.GenerateTilesRecurse(LevelGenerator.GetTile(TileConnection.Up, tile, type: type, mirror: this.mirror, filter: this.down.GetFilter()), type);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (!this.kingTile || !this.symmetric)
        return;
      this.SolveSymmetry();
      if (this.up != null)
        this.up.SolveSymmetry();
      if (this.down == null)
        return;
      this.down.SolveSymmetry();
    }

    public void SolveSymmetry()
    {
      if (this.mirror)
      {
        if (this.leftSymmetric)
        {
          if (this.left == null || this.left.data == null || this.right == null)
            return;
          this.right.data = this.left.data.Flipped();
          this.right.symmetricalPartner = this.left;
          this.left.symmetricalPartner = this.right;
        }
        else
        {
          if (this.right == null || this.right.data == null || this.left == null)
            return;
          this.left.data = this.right.data.Flipped();
          this.right.symmetricalPartner = this.left;
          this.left.symmetricalPartner = this.right;
        }
      }
      else
      {
        if (this.data == null)
          return;
        if (this.removeRight && this.right != null)
        {
          this.right.data = this.data.Flipped();
          this.right.symmetricalPartner = this;
          this.symmetricalPartner = this.right;
        }
        if (!this.removeLeft || this.left == null)
          return;
        this.left.data = this.data.Flipped();
        this.left.symmetricalPartner = this;
        this.symmetricalPartner = this.left;
      }
    }
  }
}
