// Decompiled with JetBrains decompiler
// Type: DuckGame.Shotgun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("guns|shotguns")]
  public class Shotgun : Gun
  {
    public sbyte _loadProgress = 100;
    public float _loadAnimation = 1f;
    public StateBinding _loadProgressBinding = new StateBinding(nameof (_loadProgress));
    protected SpriteMap _loaderSprite;

    public Shotgun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 2;
      this._ammoType = (AmmoType) new ATShotgun();
      this._type = "gun";
      this.graphic = new Sprite("shotgun");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(30f, 14f);
      this._fireSound = "shotgunFire2";
      this._kickForce = 4f;
      this._numBulletsPerFire = 6;
      this._manualLoad = true;
      this._loaderSprite = new SpriteMap("shotgunLoader", 8, 8);
      this._loaderSprite.center = new Vec2(4f, 4f);
    }

    public override void Update()
    {
      base.Update();
      if ((double) this._loadAnimation == -1.0)
      {
        SFX.Play("shotgunLoad");
        this._loadAnimation = 0.0f;
      }
      if ((double) this._loadAnimation >= 0.0)
      {
        if ((double) this._loadAnimation == 0.5 && this.ammo != 0)
          this._ammoType.PopShell(this.x, this.y, (int) -this.offDir);
        if ((double) this._loadAnimation < 1.0)
          this._loadAnimation += 0.1f;
        else
          this._loadAnimation = 1f;
      }
      if (this._loadProgress < (sbyte) 0)
        return;
      if (this._loadProgress == (sbyte) 50)
        this.Reload(false);
      if (this._loadProgress < (sbyte) 100)
        this._loadProgress += (sbyte) 10;
      else
        this._loadProgress = (sbyte) 100;
    }

    public override void OnPressAction()
    {
      if (this.loaded)
      {
        base.OnPressAction();
        this._loadProgress = (sbyte) -1;
        this._loadAnimation = -0.01f;
      }
      else
      {
        if (this._loadProgress != (sbyte) -1)
          return;
        this._loadProgress = (sbyte) 0;
        this._loadAnimation = -1f;
      }
    }

    public override void Draw()
    {
      base.Draw();
      Vec2 vec2 = new Vec2(13f, -2f);
      float num = (float) Math.Sin((double) this._loadAnimation * 3.14000010490417) * 3f;
      this.Draw((Sprite) this._loaderSprite, new Vec2(vec2.x - 8f - num, vec2.y + 4f));
    }
  }
}
