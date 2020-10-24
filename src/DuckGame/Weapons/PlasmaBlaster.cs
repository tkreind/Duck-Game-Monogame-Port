﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PlasmaBlaster
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|laser")]
  [BaggedProperty("isSuperWeapon", true)]
  public class PlasmaBlaster : Gun
  {
    private SpriteMap _bigFlare;
    private bool _flared;

    public PlasmaBlaster(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this._ammoType = (AmmoType) new ATPlasmaBlaster();
      this._type = "gun";
      this.graphic = new Sprite("plasmaBlaster");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(18f, 6f);
      this._fireSound = "plasmaFire";
      this._kickForce = 1f;
      this.loseAccuracy = 0.14f;
      this.maxAccuracyLost = 0.9f;
      this._bigFlare = new SpriteMap("plasmaFlare", 32, 32);
      this._bigFlare.AddAnimation("idle", 1f, false, 0, 1, 2);
      this._bigFlare.center = new Vec2(0.0f, 16f);
      this._fullAuto = true;
      this._bulletColor = Color.Orange;
      this._bio = "Originally found in a crater next to a burnt power suit. It's origin and mechanism of action are unknown, but tests indicate that it is seriously badass.";
      this._editorName = "Plasma Blaster";
    }

    public override void Update()
    {
      this.ammo = 99;
      if ((double) this._fireWait > 6.0)
        this._fireWait = 6f;
      this._fireWait = Maths.LerpTowards(this._fireWait, 0.3f, 0.01f);
      base.Update();
    }

    public override void Draw()
    {
      this._barrelHeat = 0.0f;
      if ((double) this._flareAlpha > 0.0 && !this._flared)
      {
        this._flared = true;
        this._bigFlare.SetAnimation("idle");
        this._bigFlare.frame = 0;
        this._fireWait += 0.1f;
      }
      if (this._flared)
      {
        this.Draw((Sprite) this._bigFlare, this.barrelOffset + new Vec2(-8f, -1f));
        if (this._bigFlare.finished)
        {
          this._flared = false;
          this._flareAlpha = 0.0f;
        }
      }
      base.Draw();
    }
  }
}
