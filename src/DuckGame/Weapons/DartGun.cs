// Decompiled with JetBrains decompiler
// Type: DuckGame.DartGun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isFatal", false)]
  [EditorGroup("guns|misc")]
  public class DartGun : Gun
  {
    public StateBinding _burnLifeBinding = new StateBinding(nameof (_burnLife));
    private SpriteMap _sprite;
    public float _burnLife = 1f;
    public float _burnWait;
    public bool burntOut;

    public DartGun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 12;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this._sprite = new SpriteMap("dartgun", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(16f, 9f);
      this._barrelOffsetTL = new Vec2(29f, 14f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this.flammable = 0.8f;
      this._barrelAngleOffset = 8f;
      this.physicsMaterial = PhysicsMaterial.Plastic;
    }

    public override void Initialize() => base.Initialize();

    public override void UpdateFirePosition(SmallFire f) => f.position = this.Offset(new Vec2(10f, 0.0f));

    public override void UpdateOnFire()
    {
      if (!this.onFire)
        return;
      this._burnWait -= 0.01f;
      if ((double) this._burnWait < 0.0)
      {
        Level.Add((Thing) SmallFire.New(10f, 0.0f, 0.0f, 0.0f, stick: ((MaterialThing) this), canMultiply: false, firedFrom: ((Thing) this)));
        this._burnWait = 1f;
      }
      if ((double) this.burnt >= 1.0)
        return;
      this.burnt += 1f / 1000f;
    }

    public override void Update()
    {
      if (!this.burntOut && (double) this.burnt >= 1.0)
      {
        this._sprite.frame = 1;
        Vec2 vec2 = this.Offset(new Vec2(10f, 0.0f));
        Level.Add((Thing) SmallSmoke.New(vec2.x, vec2.y));
        this._onFire = false;
        this.flammable = 0.0f;
        this.burntOut = true;
      }
      base.Update();
    }

    protected override bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      this.onFire = true;
      return true;
    }

    public override void Draw() => base.Draw();

    public override void OnPressAction()
    {
      if (this.ammo > 0)
      {
        if ((double) this._burnLife <= 0.0)
        {
          SFX.Play("dartStick", 0.5f, Rando.Float(0.2f) - 0.1f);
        }
        else
        {
          --this.ammo;
          SFX.Play("dartGunFire", 0.5f, Rando.Float(0.2f) - 0.1f);
          this.kick = 1f;
          if (this.receivingPress || !this.isServerForObject)
            return;
          Vec2 vec2 = this.Offset(this.barrelOffset);
          float radians = this.barrelAngle + Rando.Float(-0.1f, 0.1f);
          Dart dart = new Dart(vec2.x, vec2.y, this.owner as Duck, -radians);
          this.Fondle((Thing) dart);
          if (this.onFire)
          {
            Level.Add((Thing) SmallFire.New(0.0f, 0.0f, 0.0f, 0.0f, stick: ((MaterialThing) dart), firedFrom: ((Thing) this)));
            dart.burning = true;
            dart.onFire = true;
          }
          Vec2 vec = Maths.AngleToVec(radians);
          dart.hSpeed = vec.x * 10f;
          dart.vSpeed = vec.y * 10f;
          Level.Add((Thing) dart);
        }
      }
      else
        this.DoAmmoClick();
    }

    public override void Fire()
    {
    }
  }
}
