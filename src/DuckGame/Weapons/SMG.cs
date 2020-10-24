﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SMG
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|machine guns")]
  public class SMG : Gun
  {
    public SMG(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 30;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.range = 110f;
      this._ammoType.accuracy = 0.6f;
      this._type = "gun";
      this.graphic = new Sprite("smg");
      this.center = new Vec2(8f, 4f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(17f, 2f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this._holdOffset = new Vec2(-1f, 0.0f);
      this.loseAccuracy = 0.2f;
      this.maxAccuracyLost = 0.8f;
    }
  }
}
