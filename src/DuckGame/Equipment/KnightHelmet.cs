// Decompiled with JetBrains decompiler
// Type: DuckGame.KnightHelmet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("equipment")]
  public class KnightHelmet : Helmet
  {
    public KnightHelmet(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._pickupSprite = new Sprite("knightHelmetPickup");
      this._sprite = new SpriteMap("knightHelmet", 32, 32);
      this.graphic = this._pickupSprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(11f, 12f);
      this._equippedCollisionOffset = new Vec2(-4f, -2f);
      this._equippedCollisionSize = new Vec2(11f, 12f);
      this._hasEquippedCollision = true;
      this._sprite.CenterOrigin();
      this.depth = (Depth) 0.0001f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._isArmor = true;
      this._equippedThickness = 3f;
    }

    public override void Update() => base.Update();
  }
}
