// Decompiled with JetBrains decompiler
// Type: DuckGame.Elevator
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Elevator : MaterialThing, IPlatform
  {
    private SpriteMap _sprite;

    public Elevator(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("elevator", 32, 37);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -6f);
      this.collisionSize = new Vec2(16f, 13f);
      this.depth = (Depth) -0.5f;
      this.thickness = 4f;
      this.weight = 7f;
      this.flammable = 0.3f;
      this.collideSounds.Add("rockHitGround2");
    }
  }
}
