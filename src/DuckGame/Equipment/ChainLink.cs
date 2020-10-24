// Decompiled with JetBrains decompiler
// Type: DuckGame.ChainLink
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ChainLink : PhysicsObject
  {
    public ChainLink(float xpos, float ypos)
    {
      this.graphic = new Sprite("chainLink");
      this.center = new Vec2(3f, 2f);
      this._collisionOffset = new Vec2(-2f, -2f);
      this._collisionSize = new Vec2(4f, 4f);
      this._skipPlatforms = true;
      this.weight = 0.1f;
      this._impactThreshold = 999f;
    }
  }
}
