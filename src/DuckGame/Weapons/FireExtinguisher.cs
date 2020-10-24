// Decompiled with JetBrains decompiler
// Type: DuckGame.FireExtinguisher
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|fire")]
  [BaggedProperty("isFatal", false)]
  public class FireExtinguisher : Gun
  {
    public StateBinding _firingBinding = new StateBinding(nameof (_firing));
    private SpriteMap _guage;
    public bool _firing;
    private bool _smoke = true;
    private ConstantSound _sound = new ConstantSound("flameThrowing");
    private int _maxAmmo = 200;

    public FireExtinguisher(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = this._maxAmmo;
      this._type = "gun";
      this.graphic = new Sprite("extinguisher");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-3f, -8f);
      this.collisionSize = new Vec2(6f, 16f);
      this._barrelOffsetTL = new Vec2(15f, 2f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this._guage = new SpriteMap("netGunGuage", 8, 8);
      this._holdOffset = new Vec2(0.0f, 2f);
      if (!Network.isActive)
        return;
      this.ammo = 120;
    }

    public override void Update()
    {
      base.Update();
      if (this.isServerForObject && this._firing && this.ammo > 0)
      {
        if (this._smoke)
        {
          Vec2 vec = Maths.AngleToVec(this.barrelAngle + Rando.Float(-0.5f, 0.5f));
          Vec2 vec2 = new Vec2(vec.x * Rando.Float(0.9f, 3f), vec.y * Rando.Float(0.9f, 3f));
          ExtinguisherSmoke extinguisherSmoke = new ExtinguisherSmoke(this.barrelPosition.x, this.barrelPosition.y);
          extinguisherSmoke.hSpeed = vec2.x;
          extinguisherSmoke.vSpeed = vec2.y;
          --this.ammo;
          this._guage.frame = 3 - (int) ((double) this.ammo / (double) this._maxAmmo * 4.0);
          Level.Add((Thing) extinguisherSmoke);
        }
        this._smoke = !this._smoke;
      }
      else
        this._smoke = true;
      this._sound.lerpVolume = this._firing ? 0.5f : 0.0f;
    }

    public override void Draw()
    {
      base.Draw();
      this._guage.flipH = this.graphic.flipH;
      this._guage.alpha = this.graphic.alpha;
      this._guage.depth = this.depth + 1;
      this.Draw((Sprite) this._guage, new Vec2(-6f, -8f));
    }

    public override void OnPressAction() => this._firing = true;

    public override void OnReleaseAction() => this._firing = false;

    public override void Fire()
    {
    }
  }
}
