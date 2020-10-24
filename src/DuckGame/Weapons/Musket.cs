// Decompiled with JetBrains decompiler
// Type: DuckGame.Musket
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  [BaggedProperty("isInDemo", true)]
  public class Musket : TampingWeapon
  {
    public Musket(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._ammoType.range = 470f;
      this._ammoType.rangeVariation = 70f;
      this._ammoType.accuracy = 0.2f;
      this._type = "gun";
      this.graphic = new Sprite("musket");
      this.center = new Vec2(19f, 5f);
      this.collisionOffset = new Vec2(-8f, -6f);
      this.collisionSize = new Vec2(16f, 10f);
      this._barrelOffsetTL = new Vec2(38f, 3f);
      this._fireSound = "shotgun";
      this._kickForce = 2f;
      this._holdOffset = new Vec2(3f, 0.0f);
    }

    public override void Update() => base.Update();

    public override void OnPressAction()
    {
      if (this._tamped)
      {
        base.OnPressAction();
        int num = 0;
        for (int index = 0; index < 14; ++index)
        {
          MusketSmoke musketSmoke = new MusketSmoke((float) ((double) this.x - 16.0 + (double) Rando.Float(32f) + (double) this.offDir * 10.0), this.y - 16f + Rando.Float(32f));
          musketSmoke.depth = (Depth) (float) (0.899999976158142 + (double) index * (1.0 / 1000.0));
          if (num < 6)
            musketSmoke.move.x -= (float) this.offDir * Rando.Float(0.1f);
          if (num > 5 && num < 10)
            musketSmoke.fly.x += (float) this.offDir * (2f + Rando.Float(7.8f));
          Level.Add((Thing) musketSmoke);
          ++num;
        }
        this._tampInc = 0.0f;
        this._tampTime = 0.0f;
        this._tamped = false;
      }
      else
      {
        if (this._raised || !(this.owner is Duck owner) || !owner.grounded)
          return;
        owner.immobilized = true;
        owner.sliding = false;
        this._rotating = true;
      }
    }
  }
}
