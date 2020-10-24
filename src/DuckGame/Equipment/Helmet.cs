// Decompiled with JetBrains decompiler
// Type: DuckGame.Helmet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("equipment")]
  [BaggedProperty("isInDemo", true)]
  public class Helmet : Hat
  {
    public StateBinding _crushedBinding = new StateBinding(nameof (crushed));
    public bool crushed;

    public Helmet(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._pickupSprite = new Sprite("helmetPickup");
      this._sprite = new SpriteMap("helmet", 32, 32);
      this.graphic = this._pickupSprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-5f, -2f);
      this.collisionSize = new Vec2(12f, 8f);
      this._sprite.CenterOrigin();
      this._isArmor = true;
      this._equippedThickness = 3f;
    }

    public virtual void Crush() => this.crushed = true;

    public override void Update() => base.Update();

    public override void Draw()
    {
      int frame = this._sprite.frame;
      this._sprite.frame = this.crushed ? 1 : 0;
      base.Draw();
      this._sprite.frame = frame;
    }
  }
}
