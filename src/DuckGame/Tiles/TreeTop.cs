// Decompiled with JetBrains decompiler
// Type: DuckGame.TreeTop
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details")]
  [BaggedProperty("isInDemo", true)]
  public class TreeTop : Thing
  {
    private Sprite _treeInside;

    public TreeTop(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("treeTop");
      this._treeInside = new Sprite("treeTopInside");
      this._treeInside.center = new Vec2(24f, 24f);
      this._treeInside.alpha = 0.8f;
      this._treeInside.depth = (Depth) 0.9f;
      this.center = new Vec2(24f, 24f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Left | WallHug.Right | WallHug.Ceiling | WallHug.Floor;
    }

    public override void Draw()
    {
      this.graphic.flipH = this.offDir <= (sbyte) 0;
      base.Draw();
    }
  }
}
