﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Phaser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|laser")]
  public class Phaser : Gun
  {
    private float _charge;
    private int _chargeLevel;
    private float _chargeFade;
    private SinWave _chargeWaver = (SinWave) 0.4f;
    private SpriteMap _phaserCharge;

    public Phaser(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 30;
      this._ammoType = (AmmoType) new ATPhaser();
      this._type = "gun";
      this.graphic = new Sprite("phaser");
      this.center = new Vec2(7f, 4f);
      this.collisionOffset = new Vec2(-7f, -4f);
      this.collisionSize = new Vec2(15f, 9f);
      this._barrelOffsetTL = new Vec2(14f, 3f);
      this._fireSound = "laserRifle";
      this._fullAuto = false;
      this._fireWait = 0.0f;
      this._kickForce = 0.5f;
      this._holdOffset = new Vec2(0.0f, 0.0f);
      this._flare = new SpriteMap("laserFlare", 16, 16);
      this._flare.center = new Vec2(0.0f, 8f);
      this._phaserCharge = new SpriteMap("phaserCharge", 8, 8);
      this._phaserCharge.frame = 1;
    }

    public override void Update()
    {
      if (this.owner == null || this.ammo <= 0)
      {
        this._charge = 0.0f;
        this._chargeLevel = 0;
      }
      this._chargeFade = Lerp.Float(this._chargeFade, (float) this._chargeLevel / 3f, 0.06f);
      base.Update();
    }

    public override void OnPressAction()
    {
    }

    public override void Draw()
    {
      base.Draw();
      if ((double) this._chargeFade <= 0.00999999977648258)
        return;
      float alpha = this.alpha;
      this.alpha = (float) (((double) this._chargeFade * 0.600000023841858 + (double) this._chargeFade * (double) this._chargeWaver.normalized * 0.400000005960464) * 0.800000011920929);
      this.Draw((Sprite) this._phaserCharge, new Vec2((float) (3.0 + (double) this._chargeFade * (double) (float) this._chargeWaver * 0.5), -4f), -1);
      this.alpha = alpha;
    }

    public override void OnHoldAction()
    {
      if (this.ammo <= 0)
        return;
      this._charge += 0.03f;
      if ((double) this._charge > 1.0)
        this._charge = 1f;
      if (this._chargeLevel == 0)
        this._chargeLevel = 1;
      else if ((double) this._charge > 0.400000005960464 && this._chargeLevel == 1)
      {
        this._chargeLevel = 2;
        SFX.Play("phaserCharge02", 0.5f);
      }
      else
      {
        if ((double) this._charge <= 0.800000011920929 || this._chargeLevel != 2)
          return;
        this._chargeLevel = 3;
        SFX.Play("phaserCharge03", 0.6f);
      }
    }

    public override void OnReleaseAction()
    {
      if (this.ammo <= 0)
        return;
      if (this.owner != null)
      {
        this._ammoType.range = (float) this._chargeLevel * 80f;
        this._ammoType.bulletThickness = (float) (0.200000002980232 + (double) this._charge * 0.400000005960464);
        this._ammoType.penetration = (float) this._chargeLevel;
        this._ammoType.accuracy = (float) (0.400000005960464 + (double) this._charge * 0.5);
        this._ammoType.bulletSpeed = (float) (8.0 + (double) this._charge * 10.0);
        if (this._chargeLevel == 1)
          this._fireSound = "phaserSmall";
        else if (this._chargeLevel == 2)
          this._fireSound = "phaserMedium";
        else if (this._chargeLevel == 3)
          this._fireSound = "phaserLarge";
        this.Fire();
        this._charge = 0.0f;
        this._chargeLevel = 0;
      }
      base.OnReleaseAction();
    }
  }
}
