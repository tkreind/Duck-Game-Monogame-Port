// Decompiled with JetBrains decompiler
// Type: DuckGame.ChainPart
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  [BaggedProperty("isOnlineCapable", false)]
  public class ChainPart : Vine
  {
    public ChainPart(float xpos, float ypos, float init)
      : base(xpos, ypos, init)
    {
      this._sprite = new SpriteMap("chain", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-5f, -4f);
      this.collisionSize = new Vec2(11f, 7f);
      this.weight = 0.1f;
      this.thickness = 0.1f;
      this.canPickUp = false;
      this.initLength = init;
      this.depth = (Depth) -0.5f;
      this._vinePartSprite = new Sprite("chain");
      this._vinePartSprite.center = new Vec2(8f, 0.0f);
    }
  }
}
