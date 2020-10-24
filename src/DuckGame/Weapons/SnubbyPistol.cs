// Decompiled with JetBrains decompiler
// Type: DuckGame.SnubbyPistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  [BaggedProperty("isInDemo", true)]
  public class SnubbyPistol : Gun
  {
    public new StateBinding _loadedBinding = new StateBinding(nameof (_loaded));
    public StateBinding _loadBurstBinding = new StateBinding(nameof (_loadBurst));
    public StateBinding _angleOffsetBinding = new StateBinding(nameof (_angleOffset));
    private SpriteMap _sprite;
    public bool _loaded;
    public float _loadBurst;
    public float _angleOffset;

    public override float angle
    {
      get => base.angle + this._angleOffset * (float) this.offDir;
      set => this._angle = value;
    }

    public SnubbyPistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 6;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.range = 130f;
      this._ammoType.rangeVariation = 20f;
      this._ammoType.accuracy = 0.95f;
      this._ammoType.penetration = 0.4f;
      this._type = "gun";
      this._sprite = new SpriteMap("snubby", 14, 10);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(7f, 4f);
      this.collisionOffset = new Vec2(-7f, -4f);
      this.collisionSize = new Vec2(14f, 9f);
      this._barrelOffsetTL = new Vec2(13f, 3f);
      this._fireSound = "snubbyFire";
      this._kickForce = 0.0f;
      this._holdOffset = new Vec2(-1f, -1f);
      this._loaded = true;
    }

    public override void OnPressAction()
    {
      if (this._loaded)
      {
        base.OnPressAction();
        this._loaded = false;
        this._sprite.frame = 0;
      }
      else
      {
        this._loaded = true;
        this._sprite.frame = 1;
        this._loadBurst = 1f;
        SFX.Play("snubbyLoad", pitch: Rando.Float(-0.1f, 0.1f));
      }
    }

    public override void Update()
    {
      this._angleOffset = (float) (-(double) this._loadBurst * 0.300000011920929);
      this._loadBurst = Lerp.FloatSmooth(this._loadBurst, 0.0f, 0.18f);
      if ((double) this._loadBurst < 0.100000001490116)
        this._loadBurst = 0.0f;
      base.Update();
    }
  }
}
