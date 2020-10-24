// Decompiled with JetBrains decompiler
// Type: DuckGame.ChokeCollar
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("equipment")]
  [BaggedProperty("canSpawn", false)]
  public class ChokeCollar : Equipment
  {
    private SpriteMap _sprite;
    private Sprite _pickupSprite;
    protected WeightBall _ball;
    public EditorProperty<bool> mace = new EditorProperty<bool>(false);

    public WeightBall ball => this._ball;

    public ChokeCollar(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._pickupSprite = new Sprite("chokeCollar");
      this._sprite = new SpriteMap("chokeCollar", 8, 8);
      this.graphic = this._pickupSprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-4f, -2f);
      this.collisionSize = new Vec2(8f, 4f);
      this._sprite.CenterOrigin();
      this.thickness = 0.1f;
      this.impactThreshold = 0.01f;
      this._equippedDepth = 3;
      this._wearOffset = new Vec2(6f, 15f);
    }

    public override void Initialize()
    {
      base.Initialize();
      if (Level.current is Editor)
        return;
      this._ball = new WeightBall(this.x, this.y, (PhysicsObject) this, this, this.mace.value);
      this.ReturnItemToWorld((Thing) this._ball);
      Level.Add((Thing) this._ball);
    }

    public override void Update()
    {
      this._ball.clip = this.clip;
      if (this._equippedDuck != null && !this.destroyed)
      {
        this.collisionOffset = new Vec2(-6f, -6f);
        this.collisionSize = new Vec2(12f, 12f);
        this.center = new Vec2(8f, 8f);
        this.solid = false;
        this._sprite.flipH = this._equippedDuck._sprite.flipH;
        this.graphic = (Sprite) this._sprite;
        this._ball.SetAttach(this.owner as PhysicsObject);
      }
      else
      {
        this.collisionOffset = new Vec2(-4f, -2f);
        this.collisionSize = new Vec2(8f, 4f);
        this.center = new Vec2((float) (this._pickupSprite.w / 2), (float) (this._pickupSprite.h / 2));
        this.solid = true;
        this._sprite.frame = 0;
        this._sprite.flipH = false;
        this.graphic = this._pickupSprite;
        this._ball.SetAttach((PhysicsObject) this);
      }
      if (this.destroyed)
        this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
