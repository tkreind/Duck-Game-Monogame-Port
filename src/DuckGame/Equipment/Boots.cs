// Decompiled with JetBrains decompiler
// Type: DuckGame.Boots
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("equipment")]
  public class Boots : Equipment
  {
    protected SpriteMap _sprite;
    protected Sprite _pickupSprite;

    public Boots(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._pickupSprite = new Sprite("bootsPickup");
      this._sprite = new SpriteMap("boots", 32, 32);
      this.graphic = this._pickupSprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-6f, -6f);
      this.collisionSize = new Vec2(12f, 13f);
      this._equippedDepth = 1;
    }

    public override void Update()
    {
      if (this._equippedDuck != null && !this.destroyed)
      {
        this.center = new Vec2(16f, 12f);
        this.graphic = (Sprite) this._sprite;
        this.collisionOffset = new Vec2(0.0f, -9999f);
        this.collisionSize = new Vec2(0.0f, 0.0f);
        this.solid = false;
        this._sprite.frame = this._equippedDuck._sprite.imageIndex;
        if (this._equippedDuck.ragdoll != null)
          this._sprite.frame = 12;
        this._sprite.flipH = this._equippedDuck._sprite.flipH;
      }
      else
      {
        this.center = new Vec2(8f, 8f);
        this.graphic = this._pickupSprite;
        this.collisionOffset = new Vec2(-6f, -6f);
        this.collisionSize = new Vec2(12f, 13f);
        this.solid = true;
        this._sprite.frame = 0;
        this._sprite.flipH = false;
      }
      if (this.destroyed)
        this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
