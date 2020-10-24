// Decompiled with JetBrains decompiler
// Type: DuckGame.GhostPack
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GhostPack : Jetpack
  {
    public GhostPack(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("jetpack", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-5f, -5f);
      this.collisionSize = new Vec2(11f, 12f);
      this._offset = new Vec2(-3f, 3f);
      this.thickness = 0.1f;
    }

    public override void Draw()
    {
      this._heat = 0.01f;
      if (this._equippedDuck != null)
      {
        this.depth = (Depth) -0.5f;
        Vec2 offset = this._offset;
        if (this.duck.offDir < (sbyte) 0)
          offset.x *= -1f;
        this.position = this.duck.position + offset;
      }
      else
        this.depth = (Depth) 0.0f;
    }
  }
}
