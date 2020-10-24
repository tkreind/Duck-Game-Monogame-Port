// Decompiled with JetBrains decompiler
// Type: DuckGame.Hat
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public abstract class Hat : Equipment
  {
    private Vec2 _hatOffset = Vec2.Zero;
    protected SpriteMap _sprite;
    protected Sprite _pickupSprite;

    public Vec2 hatOffset
    {
      get => this._hatOffset;
      set => this._hatOffset = value;
    }

    public SpriteMap sprite
    {
      get => this._sprite;
      set => this._sprite = value;
    }

    public Sprite pickupSprite
    {
      get => this._pickupSprite;
      set => this._pickupSprite = value;
    }

    public Hat(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-6f, -6f);
      this.collisionSize = new Vec2(12f, 12f);
      this._autoOffset = false;
      this.thickness = 0.1f;
      this._sprite = new SpriteMap("hats/burgers", 32, 32);
      this._pickupSprite = (Sprite) new SpriteMap("hats/burgers", 32, 32);
      this._equippedDepth = 4;
    }

    public virtual void Quack(float volume, float pitch) => SFX.Play("quack", volume, pitch);

    public virtual void OpenHat()
    {
    }

    public virtual void CloseHat()
    {
    }

    public override void Update()
    {
      if (this._equippedDuck != null && !this.destroyed)
      {
        this.center = new Vec2((float) (this._sprite.w / 2), (float) (this._sprite.h / 2));
        this.graphic = (Sprite) this._sprite;
        this.solid = false;
        this.visible = false;
      }
      else
      {
        this._sprite.frame = 0;
        this.center = new Vec2((float) (this._pickupSprite.w / 2), (float) (this._pickupSprite.h / 2));
        this.graphic = this._pickupSprite;
        this.solid = true;
        this._sprite.flipH = false;
        this.visible = true;
      }
      if (this.destroyed)
        this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
