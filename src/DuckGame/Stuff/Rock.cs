// Decompiled with JetBrains decompiler
// Type: DuckGame.Rock
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  [BaggedProperty("isInDemo", true)]
  public class Rock : Holdable, IPlatform
  {
    private SpriteMap _sprite;

    public Rock(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("rock01", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -5f);
      this.collisionSize = new Vec2(16f, 12f);
      this.depth = new Depth(-0.5f);
      this.thickness = 4f;
      this.weight = 7f;
      this.flammable = 0.0f;
      this.collideSounds.Add("rockHitGround2");
      this.physicsMaterial = PhysicsMaterial.Metal;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal && TeamSelect2.Enabled("EXPLODEYCRATES"))
      {
        if (this.duck != null)
          this.duck.ThrowItem();
        this.Destroy((DestroyType) new DTShot(bullet));
        Level.Remove((Thing) this);
        Level.Add((Thing) new GrenadeExplosion(this.x, this.y));
      }
      return base.Hit(bullet, hitPos);
    }
  }
}
