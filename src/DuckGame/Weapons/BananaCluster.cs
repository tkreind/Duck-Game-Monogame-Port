// Decompiled with JetBrains decompiler
// Type: DuckGame.BananaCluster
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isFatal", false)]
  [EditorGroup("guns|explosives")]
  public class BananaCluster : Gun
  {
    private SpriteMap _sprite;
    private int _ammoMax = 3;

    public BananaCluster(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 3;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._type = "gun";
      this._sprite = new SpriteMap("banana", 16, 16);
      this._sprite.frame = 4;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(12f, 11f);
      this._holdOffset = new Vec2(0.0f, 2f);
      this.bouncy = 0.4f;
      this.friction = 0.05f;
    }

    public override void Update()
    {
      this._sprite.frame = 4 + this._ammoMax - this.ammo;
      if (this.ammo == 0 && this.owner != null)
      {
        if (this.owner is Duck owner)
          owner.ThrowItem();
        Level.Remove((Thing) this);
      }
      if (this.owner == null && this.ammo == 1)
      {
        Banana banana = new Banana(this.x, this.y);
        banana.hSpeed = this.hSpeed;
        banana.vSpeed = this.vSpeed;
        Level.Remove((Thing) this);
        Level.Add((Thing) banana);
      }
      base.Update();
    }

    public override void OnPressAction()
    {
      if (this.ammo <= 0)
        return;
      --this.ammo;
      SFX.Play("smallSplat", pitch: Rando.Float(-0.6f, 0.6f));
      if (!(this.owner is Duck owner))
        return;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (owner.inputProfile.Down("LEFT"))
        num1 -= 3f;
      if (owner.inputProfile.Down("RIGHT"))
        num1 += 3f;
      if (owner.inputProfile.Down("UP"))
        num2 -= 3f;
      if (owner.inputProfile.Down("DOWN"))
        num2 += 3f;
      if (!this.isServerForObject)
        return;
      Banana banana = new Banana(this.barrelPosition.x, this.barrelPosition.y);
      if (!owner.crouch)
      {
        banana.hSpeed = (float) this.offDir * Rando.Float(3f, 3.5f) + num1;
        banana.vSpeed = num2 - 1.5f + Rando.Float(-0.5f, -1f);
      }
      banana.EatBanana();
      banana.clip.Add((MaterialThing) owner);
      owner.clip.Add((MaterialThing) banana);
      Level.Add((Thing) banana);
    }
  }
}
