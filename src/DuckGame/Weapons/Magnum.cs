// Decompiled with JetBrains decompiler
// Type: DuckGame.Magnum
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("guns")]
  public class Magnum : Gun
  {
    public StateBinding _angleOffsetBinding = new StateBinding(nameof (_angleOffset));
    public StateBinding _riseBinding = new StateBinding(nameof (rise));
    public float rise;
    public float _angleOffset;

    public override float angle
    {
      get => base.angle + this._angleOffset;
      set => this._angle = value;
    }

    public Magnum(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 6;
      this._ammoType = (AmmoType) new ATMagnum();
      this._type = "gun";
      this.graphic = new Sprite("magnum");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -6f);
      this.collisionSize = new Vec2(16f, 10f);
      this._barrelOffsetTL = new Vec2(25f, 12f);
      this._fireSound = "magnum";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(1f, 2f);
      this.handOffset = new Vec2(0.0f, 1f);
      this._bio = "Standard issue .44 Magnum. Pretty great for killing things, really great for killing things that are trying to hide. Watch the kick, unless you're trying to shoot the ceiling.";
      this._editorName = nameof (Magnum);
    }

    public override void Update()
    {
      base.Update();
      this._angleOffset = this.owner == null ? 0.0f : (this.offDir >= (sbyte) 0 ? -Maths.DegToRad(this.rise * 65f) : -Maths.DegToRad((float) (-(double) this.rise * 65.0)));
      if ((double) this.rise > 0.0)
        this.rise -= 0.013f;
      else
        this.rise = 0.0f;
      if (!this._raised)
        return;
      this._angleOffset = 0.0f;
    }

    public override void OnPressAction()
    {
      base.OnPressAction();
      if (this.ammo <= 0 || (double) this.rise >= 1.0)
        return;
      this.rise += 0.4f;
    }
  }
}
