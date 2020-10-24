// Decompiled with JetBrains decompiler
// Type: DuckGame.Tombstone
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  public class Tombstone : Holdable, IPlatform
  {
    private SpriteMap _sprite;

    public Tombstone(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("grave", 15, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(7f, 8f);
      this.collisionOffset = new Vec2(-7f, -8f);
      this.collisionSize = new Vec2(15f, 15f);
      this.depth = new Depth(-0.5f);
      this.thickness = 4f;
      this.weight = 7f;
      this.flammable = 0.3f;
      this.collideSounds.Add("rockHitGround2");
    }
  }
}
