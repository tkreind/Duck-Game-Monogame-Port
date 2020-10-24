// Decompiled with JetBrains decompiler
// Type: DuckGame.FlareGun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns|fire")]
  public class FlareGun : Gun
  {
    public FlareGun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 2;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.combustable = true;
      this._type = "gun";
      this.graphic = new Sprite("flareGun");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(16f, 9f);
      this._barrelOffsetTL = new Vec2(18f, 6f);
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 1f;
      this._barrelAngleOffset = 4f;
      this._editorName = "Flare Gun";
      this._bio = "For safety purposes, used to call help. What? No it's not a weapon. NO DON'T USE IT LIKE THAT!";
    }

    public override void Initialize() => base.Initialize();

    public override void Update() => base.Update();

    public override void Draw() => base.Draw();

    public override void OnPressAction()
    {
      if (this.ammo > 0)
      {
        --this.ammo;
        SFX.Play("netGunFire", 0.5f, Rando.Float(0.2f) - 0.4f);
        this.ApplyKick();
        if (this.receivingPress || !this.isServerForObject)
          return;
        Vec2 vec2 = this.Offset(this.barrelOffset);
        Flare flare = new Flare(vec2.x, vec2.y, this);
        this.Fondle((Thing) flare);
        Vec2 vec = Maths.AngleToVec(this.barrelAngle + Rando.Float(-0.2f, 0.2f));
        flare.hSpeed = vec.x * 14f;
        flare.vSpeed = vec.y * 14f;
        Level.Add((Thing) flare);
      }
      else
        this.DoAmmoClick();
    }

    public override void Fire()
    {
    }
  }
}
