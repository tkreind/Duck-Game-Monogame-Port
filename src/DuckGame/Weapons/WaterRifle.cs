// Decompiled with JetBrains decompiler
// Type: DuckGame.WaterRifle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  [BaggedProperty("isFatal", false)]
  public class WaterRifle : Gun
  {
    private FluidStream _stream;
    private ConstantSound _sound = new ConstantSound("demoBlaster");
    private int _wait;

    public WaterRifle(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 9;
      this._ammoType = (AmmoType) new AT9mm();
      this._type = "gun";
      this.graphic = new Sprite("waterGun");
      this.center = new Vec2(11f, 7f);
      this.collisionOffset = new Vec2(-11f, -6f);
      this.collisionSize = new Vec2(23f, 13f);
      this._barrelOffsetTL = new Vec2(24f, 6f);
      this._fireSound = "pistolFire";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-1f, 0.0f);
      this.loseAccuracy = 0.1f;
      this.maxAccuracyLost = 0.6f;
      this._bio = "";
      this._editorName = "Water Blaster";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._stream = new FluidStream(this.x, this.y, new Vec2(1f, 0.0f), 2f);
    }

    public override void Initialize() => Level.Add((Thing) this._stream);

    public override void Terminate() => Level.Remove((Thing) this._stream);

    public override void Update() => base.Update();

    public override void OnPressAction()
    {
    }

    public override void OnHoldAction()
    {
      ++this._wait;
      if (this._wait != 3)
        return;
      this._stream.sprayAngle = this.barrelVector * 2f;
      this._stream.position = this.barrelPosition;
      FluidData water = Fluid.Water;
      water.amount = 0.01f;
      this._stream.Feed(water);
      this._wait = 0;
    }
  }
}
