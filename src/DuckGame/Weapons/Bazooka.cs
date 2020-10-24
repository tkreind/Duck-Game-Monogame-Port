// Decompiled with JetBrains decompiler
// Type: DuckGame.Bazooka
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  [BaggedProperty("isInDemo", true)]
  public class Bazooka : TampingWeapon
  {
    public Bazooka(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this._ammoType = (AmmoType) new ATMissile();
      this._type = "gun";
      this.graphic = new Sprite("bazooka");
      this.center = new Vec2(15f, 5f);
      this.collisionOffset = new Vec2(-15f, -4f);
      this.collisionSize = new Vec2(30f, 10f);
      this._barrelOffsetTL = new Vec2(29f, 4f);
      this._fireSound = "missile";
      this._kickForce = 4f;
      this._holdOffset = new Vec2(-2f, -2f);
      this.loseAccuracy = 0.1f;
      this.maxAccuracyLost = 0.6f;
      this._bio = "Old faithful, the 9MM pistol.";
      this._editorName = nameof (Bazooka);
      this.physicsMaterial = PhysicsMaterial.Metal;
    }

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
        this._tampTime = 0.5f;
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
