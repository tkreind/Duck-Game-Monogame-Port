﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CowboyPistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("guns|explosives")]
  [BaggedProperty("canSpawn", false)]
  public class CowboyPistol : Gun
  {
    private float rise;
    private float _angleOffset;

    public override float angle
    {
      get
      {
        if (this._raised || this.duck == null)
          return base.angle;
        Vec2 p2 = this.duck.inputProfile.rightStick;
        if ((double) p2.length < 0.100000001490116)
        {
          p2 = Vec2.Zero;
          return base.angle;
        }
        return this.offDir > (sbyte) 0 ? Maths.DegToRad(Maths.PointDirection(Vec2.Zero, p2)) : Maths.DegToRad(Maths.PointDirection(Vec2.Zero, p2) + 180f);
      }
      set => this._angle = value;
    }

    public CowboyPistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 6;
      this._ammoType = (AmmoType) new ATMagnum();
      this._type = "gun";
      this.graphic = new Sprite("cowboyPistol");
      this.center = new Vec2(6f, 7f);
      this.collisionOffset = new Vec2(-5f, -7f);
      this.collisionSize = new Vec2(18f, 11f);
      this._barrelOffsetTL = new Vec2(21f, 3f);
      this._fireSound = "magnum";
      this._kickForce = 3f;
      this._holdOffset = new Vec2(-2f, 1f);
      this._bio = "Standard issue .44 Magnum. Pretty great for killing things, really great for killing things that are trying to hide. Watch the kick, unless you're trying to shoot the ceiling.";
      this._editorName = "Cowboy Pistol";
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
