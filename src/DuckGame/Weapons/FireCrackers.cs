// Decompiled with JetBrains decompiler
// Type: DuckGame.FireCrackers
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("guns|misc")]
  [BaggedProperty("isFatal", false)]
  public class FireCrackers : Gun
  {
    private SpriteMap _sprite;
    private int _ammoMax = 8;

    public FireCrackers(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = this._ammoMax;
      this._type = "gun";
      this._sprite = new SpriteMap("fireCrackers", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-4f, -4f);
      this.collisionSize = new Vec2(8f, 8f);
      this._barrelOffsetTL = new Vec2(12f, 6f);
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this.flammable = 1f;
      this.physicsMaterial = PhysicsMaterial.Paper;
    }

    public override void Initialize() => base.Initialize();

    public override void Update()
    {
      this._sprite.frame = this._ammoMax - this.ammo;
      if (this.ammo == 0 && this.owner != null && this.owner is Duck owner)
        owner.ThrowItem();
      base.Update();
    }

    public override void Draw() => base.Draw();

    public override void OnPressAction()
    {
      if (this.ammo <= 0)
        return;
      --this.ammo;
      SFX.Play("lightMatch", 0.5f, Rando.Float(0.2f) - 0.4f);
      if (!(this.owner is Duck owner))
        return;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (owner.inputProfile.Down("LEFT"))
        num1 -= 2f;
      if (owner.inputProfile.Down("RIGHT"))
        num1 += 2f;
      if (owner.inputProfile.Down("UP"))
        num2 -= 2f;
      if (owner.inputProfile.Down("DOWN"))
        num2 += 2f;
      Firecracker firecracker = new Firecracker(this.barrelPosition.x, this.barrelPosition.y);
      if (!owner.crouch)
      {
        firecracker.hSpeed = (float) this.offDir * Rando.Float(2f, 2.5f) + num1;
        firecracker.vSpeed = num2 - 1f + Rando.Float(-0.2f, 0.8f);
      }
      else
        firecracker.spinAngle = 90f;
      Level.Add((Thing) firecracker);
    }

    protected override bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      for (int index = 0; index < this.ammo; ++index)
      {
        Firecracker firecracker = new Firecracker(this.barrelPosition.x, this.barrelPosition.y);
        firecracker.hSpeed = Rando.Float(-4f, 4f);
        firecracker.vSpeed = Rando.Float(-1f, -6f);
        Level.Add((Thing) firecracker);
      }
      Level.Remove((Thing) this);
      if (this.owner is Duck owner)
        owner.ThrowItem();
      return true;
    }

    public override void Fire()
    {
    }
  }
}
