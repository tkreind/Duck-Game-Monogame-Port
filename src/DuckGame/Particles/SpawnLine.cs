// Decompiled with JetBrains decompiler
// Type: DuckGame.SpawnLine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class SpawnLine : Thing
  {
    private float _moveSpeed;
    private float _thickness;
    private Color _color;

    public SpawnLine(
      float xpos,
      float ypos,
      int dir,
      float moveSpeed,
      Color color,
      float thickness)
      : base(xpos, ypos)
    {
      this._moveSpeed = moveSpeed;
      this._color = color;
      this._thickness = thickness;
      this.offDir = (sbyte) dir;
      this.layer = Layer.Foreground;
      this.depth = (Depth) 0.9f;
    }

    public override void Update()
    {
      this.alpha -= 0.03f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      this.x += this._moveSpeed;
    }

    public override void Draw() => Graphics.DrawLine(this.position, this.position + new Vec2(0.0f, -1200f), this._color * this.alpha, this._thickness, (Depth) 0.9f);
  }
}
