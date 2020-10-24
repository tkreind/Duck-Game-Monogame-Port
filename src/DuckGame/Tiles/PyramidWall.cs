// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidWall
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PyramidWall : Block
  {
    private Sprite _corner;
    private Sprite _corner2;

    public PyramidWall(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("pyramidEdge");
      this.collisionSize = new Vec2(200f, 153f);
      this.collisionOffset = new Vec2(-4f, -4f);
      this._corner = new Sprite("pyWallCorner");
      this._corner2 = new Sprite("pyWallCorner2");
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.depth = (Depth) -0.9f;
    }

    public override void Initialize() => base.Initialize();

    public override void Draw()
    {
      this.graphic.depth = (Depth) -0.8f;
      Graphics.Draw(this.graphic, this.x - 8f, this.y - 8f, new Rectangle(0.0f, 0.0f, 208f, 8f));
      this.graphic.depth = (Depth) -0.85f;
      Graphics.Draw(this.graphic, this.x, this.y + 144f, new Rectangle(8f, 152f, 192f, 8f));
      this.graphic.depth = (Depth) -0.86f;
      Graphics.Draw(this.graphic, this.x + 192f, this.y, new Rectangle(200f, 8f, 8f, 144f));
      Graphics.Draw(this.graphic, this.x - 8f, this.y - 8f, new Rectangle(0.0f, 0.0f, 8f, 152f));
      this._corner.depth = (Depth) -0.9f;
      Graphics.Draw(this._corner, this.x - 8f, this.y + 144f);
      this._corner2.depth = (Depth) -0.9f;
      Graphics.Draw(this._corner2, this.x + 192f, this.y + 144f);
      this.graphic.depth = (Depth) -0.7f;
      Graphics.Draw(this.graphic, this.x, this.y, new Rectangle(8f, 8f, 192f, 144f));
    }
  }
}
