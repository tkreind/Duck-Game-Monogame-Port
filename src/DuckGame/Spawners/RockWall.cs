// Decompiled with JetBrains decompiler
// Type: DuckGame.RockWall
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("spawns")]
  public class RockWall : Block
  {
    private Sprite _wall;

    public RockWall(float xpos, float ypos, System.Type c = null)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("laserSpawner");
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(12f, 12f);
      this.collisionOffset = new Vec2(-6f, -6f);
      this.depth = (Depth) -0.6f;
      this.hugWalls = WallHug.None;
      this.layer = Layer.Foreground;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._visibleInGame = true;
      this._wall = new Sprite("rockWall");
      this._wall.center = new Vec2((float) (this._wall.w - 4), (float) (this._wall.h / 2));
    }

    public override void Initialize()
    {
      if (!(Level.current is Editor))
      {
        this.collisionSize = new Vec2(64f, 1024f);
        this.collisionOffset = new Vec2(-61f, -512f);
      }
      base.Initialize();
    }

    public override void Draw()
    {
      Graphics.DrawLine(this.position, this.position + new Vec2(this.flipHorizontal ? 16f : -16f, 0.0f), Color.Red);
      this._wall.flipH = this.flipHorizontal;
      if (!(Level.current is Editor))
        Graphics.Draw(this._wall, this.x, this.y);
      else
        base.Draw();
    }
  }
}
