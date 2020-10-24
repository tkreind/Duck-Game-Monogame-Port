// Decompiled with JetBrains decompiler
// Type: DuckGame.ChestPlate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("equipment")]
  public class ChestPlate : Equipment
  {
    private SpriteMap _sprite;
    private SpriteMap _spriteOver;
    private Sprite _pickupSprite;

    public ChestPlate(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("chestPlateAnim", 32, 32);
      this._spriteOver = new SpriteMap("chestPlateAnimOver", 32, 32);
      this._pickupSprite = new Sprite("chestPlatePickup");
      this._pickupSprite.CenterOrigin();
      this.graphic = this._pickupSprite;
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(11f, 8f);
      this._equippedCollisionOffset = new Vec2(-7f, -5f);
      this._equippedCollisionSize = new Vec2(12f, 11f);
      this._hasEquippedCollision = true;
      this.center = new Vec2(8f, 8f);
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._equippedDepth = 2;
      this._wearOffset = new Vec2(1f, 1f);
      this._isArmor = true;
      this._equippedThickness = 3f;
    }

    public override void Update()
    {
      if (this._equippedDuck != null && this.duck == null)
        return;
      if (this._equippedDuck != null && !this.destroyed)
      {
        this.center = new Vec2(16f, 16f);
        this.solid = false;
        this._sprite.flipH = this.duck._sprite.flipH;
        this._spriteOver.flipH = this.duck._sprite.flipH;
        this.graphic = (Sprite) this._sprite;
      }
      else
      {
        this.center = new Vec2((float) (this._pickupSprite.w / 2), (float) (this._pickupSprite.h / 2));
        this.solid = true;
        this._sprite.frame = 0;
        this._sprite.flipH = false;
        this.graphic = this._pickupSprite;
      }
      if (this.destroyed)
        this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Draw()
    {
      base.Draw();
      if (this._equippedDuck != null && this.duck == null || this._equippedDuck == null)
        return;
      this._spriteOver.flipH = this.graphic.flipH;
      this._spriteOver.angle = this.angle;
      this._spriteOver.alpha = this.alpha;
      this._spriteOver.scale = this.scale;
      this._spriteOver.depth = this.owner.depth + (this.duck.holdObject != null ? 3 : 11);
      this._spriteOver.center = this.center;
      Graphics.Draw((Sprite) this._spriteOver, this.x, this.y);
    }
  }
}
