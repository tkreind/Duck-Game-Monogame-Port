// Decompiled with JetBrains decompiler
// Type: DuckGame.SuicidePistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  public class SuicidePistol : Gun
  {
    public SuicidePistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 6;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.barrelAngleDegrees = 180f;
      this._ammoType.immediatelyDeadly = true;
      this._type = "gun";
      this.graphic = new Sprite("suicidePistol");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -5f);
      this.collisionSize = new Vec2(16f, 10f);
      this._barrelOffsetTL = new Vec2(8f, 13f);
      this._fireSound = "magnum";
      this._kickForce = 2f;
      this.handOffset = new Vec2(6f, 0.0f);
      this._holdOffset = new Vec2(6f, 0.0f);
      this.loseAccuracy = 0.1f;
      this.maxAccuracyLost = 0.6f;
    }

    public override void Update()
    {
      if (this._raised)
      {
        this.handOffset = new Vec2(0.0f, 0.0f);
        this._holdOffset = new Vec2(0.0f, 0.0f);
        this.collisionOffset = new Vec2(-8f, -5f);
        this.collisionSize = new Vec2(16f, 10f);
      }
      else
      {
        this.handOffset = new Vec2(7f, 0.0f);
        this._holdOffset = new Vec2(4f, -1f);
        this.collisionOffset = new Vec2(-8f, -5f);
        this.collisionSize = new Vec2(8f, 10f);
      }
      base.Update();
    }
  }
}
