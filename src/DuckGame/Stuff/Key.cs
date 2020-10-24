// Decompiled with JetBrains decompiler
// Type: DuckGame.Key
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class Key : Holdable
  {
    private SpriteMap _sprite;

    public Key(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("key", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-7f, -4f);
      this.collisionSize = new Vec2(14f, 8f);
      this.depth = (Depth) -0.5f;
      this.thickness = 1f;
      this.weight = 3f;
      this.flammable = 0.0f;
      this.collideSounds.Add("metalRebound");
      this.physicsMaterial = PhysicsMaterial.Metal;
    }

    public override void Update()
    {
      this._sprite.flipH = this.offDir < (sbyte) 0;
      if (this.owner != null)
        Level.CheckLine<Door>(this.position + new Vec2(-10f, 0.0f), this.position + new Vec2(10f, 0.0f))?.UnlockDoor(this);
      base.Update();
    }

    public override void Terminate()
    {
    }
  }
}
