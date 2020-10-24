// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsChain
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [BaggedProperty("canSpawn", false)]
  [EditorGroup("stuff|ropes")]
  public class PhysicsChain : PhysicsRope
  {
    public PhysicsChain(float xpos, float ypos, PhysicsRope next = null)
      : base(xpos, ypos)
    {
      this.chain = true;
      this._vine = (Sprite) new SpriteMap("chain", 16, 16);
      this.graphic = this._vine;
      this.center = new Vec2(8f, 8f);
      this._vineEnd = new Sprite("chainStretchEnd");
      this._vineEnd.center = new Vec2(8f, 0.0f);
      this.collisionOffset = new Vec2(-5f, -4f);
      this.collisionSize = new Vec2(11f, 7f);
      this.graphic = this._vine;
      this._beam = new Sprite("chainStretch");
      this._editorName = "Chain";
    }

    public override Vine GetSection(float x, float y, int div) => (Vine) new ChainPart(x, y, (float) div);
  }
}
