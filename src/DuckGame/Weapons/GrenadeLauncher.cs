// Decompiled with JetBrains decompiler
// Type: DuckGame.GrenadeLauncher
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|explosives")]
  public class GrenadeLauncher : Gun
  {
    public StateBinding _fireAngleState = new StateBinding(nameof (_fireAngle));
    public StateBinding _aimAngleState = new StateBinding(nameof (_aimAngle));
    public StateBinding _aimWaitState = new StateBinding(nameof (_aimWait));
    public StateBinding _aimingState = new StateBinding(nameof (_aiming));
    public StateBinding _cooldownState = new StateBinding(nameof (_cooldown));
    public float _fireAngle;
    public float _aimAngle;
    public float _aimWait;
    public bool _aiming;
    public float _cooldown;

    public override float angle
    {
      get => base.angle + this._aimAngle;
      set => this._angle = value;
    }

    public GrenadeLauncher(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 6;
      this._type = "gun";
      this.graphic = new Sprite("grenadeLauncher");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(16f, 7f);
      this._barrelOffsetTL = new Vec2(28f, 14f);
      this._fireSound = "pistol";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(4f, 0.0f);
      this._ammoType = (AmmoType) new ATGrenade();
      this._fireSound = "deepMachineGun";
      this._bulletColor = Color.White;
    }

    public override void Update()
    {
      base.Update();
      if (this._aiming && (double) this._aimWait <= 0.0 && (double) this._fireAngle < 90.0)
        this._fireAngle += 3f;
      if ((double) this._aimWait > 0.0)
        this._aimWait -= 0.9f;
      if ((double) this._cooldown > 0.0)
        this._cooldown -= 0.1f;
      else
        this._cooldown = 0.0f;
      if (this.owner != null)
      {
        this._aimAngle = -Maths.DegToRad(this._fireAngle);
        if (this.offDir < (sbyte) 0)
          this._aimAngle = -this._aimAngle;
      }
      else
      {
        this._aimWait = 0.0f;
        this._aiming = false;
        this._aimAngle = 0.0f;
        this._fireAngle = 0.0f;
      }
      if (!this._raised)
        return;
      this._aimAngle = 0.0f;
    }

    public override void OnPressAction()
    {
      if ((double) this._cooldown != 0.0)
        return;
      if (this.ammo > 0)
      {
        this._aiming = true;
        this._aimWait = 1f;
      }
      else
        SFX.Play("click");
    }

    public override void OnReleaseAction()
    {
      if ((double) this._cooldown != 0.0 || this.ammo <= 0)
        return;
      this._aiming = false;
      this.Fire();
      this._cooldown = 1f;
      this.angle = 0.0f;
      this._fireAngle = 0.0f;
    }
  }
}
