// Decompiled with JetBrains decompiler
// Type: DuckGame.Chaingun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [BaggedProperty("isSuperWeapon", true)]
  [EditorGroup("guns|machine guns")]
  public class Chaingun : Gun
  {
    public StateBinding _fireWaitBinding = new StateBinding("_fireWait");
    public StateBinding _spinBinding = new StateBinding(nameof (_spin));
    public StateBinding _spinningBinding = new StateBinding(nameof (_spinning));
    private SpriteMap _tip;
    private SpriteMap _sprite;
    public float _spin;
    private ChaingunBullet _bullets;
    private ChaingunBullet _topBullet;
    private Sound _spinUp;
    private Sound _spinDown;
    private int bulletsTillRemove = 10;
    private int numHanging = 10;
    private bool _spinning;

    public Chaingun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 100;
      this._ammoType = (AmmoType) new AT9mm();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.5f;
      this._type = "gun";
      this._sprite = new SpriteMap("chaingun", 42, 28);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(14f, 14f);
      this.collisionOffset = new Vec2(-8f, -3f);
      this.collisionSize = new Vec2(24f, 10f);
      this._tip = new SpriteMap("chaingunTip", 42, 28);
      this._barrelOffsetTL = new Vec2(39f, 14f);
      this._fireSound = "pistolFire";
      this._fullAuto = true;
      this._fireWait = 0.7f;
      this._kickForce = 1f;
      this.weight = 8f;
      this._spinUp = SFX.Get("chaingunSpinUp");
      this._spinDown = SFX.Get("chaingunSpinDown");
      this._holdOffset = new Vec2(0.0f, 2f);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._bullets = new ChaingunBullet(this.x, this.y);
      this._bullets.parentThing = (Thing) this;
      this._topBullet = this._bullets;
      float num = 0.1f;
      ChaingunBullet chaingunBullet1 = (ChaingunBullet) null;
      for (int index = 0; index < 9; ++index)
      {
        ChaingunBullet chaingunBullet2 = new ChaingunBullet(this.x, this.y);
        chaingunBullet2.parentThing = (Thing) this._bullets;
        this._bullets = chaingunBullet2;
        chaingunBullet2.waveAdd = num;
        num += 0.4f;
        if (index == 0)
          this._topBullet.childThing = (Thing) chaingunBullet2;
        else
          chaingunBullet1.childThing = (Thing) chaingunBullet2;
        chaingunBullet1 = chaingunBullet2;
      }
    }

    public override void Terminate()
    {
    }

    public override void OnHoldAction()
    {
      if (!this._spinning)
      {
        this._spinning = true;
        this._spinDown.Volume = 0.0f;
        this._spinDown.Stop();
        this._spinUp.Volume = 1f;
        this._spinUp.Play();
      }
      if ((double) this._spin < 1.0)
      {
        this._spin += 0.04f;
      }
      else
      {
        this._spin = 1f;
        base.OnHoldAction();
      }
    }

    public override void OnReleaseAction()
    {
      if (!this._spinning)
        return;
      this._spinning = false;
      this._spinUp.Volume = 0.0f;
      this._spinUp.Stop();
      if ((double) this._spin <= 0.899999976158142)
        return;
      this._spinDown.Volume = 1f;
      this._spinDown.Play();
    }

    public override void Update()
    {
      if (this._topBullet != null)
      {
        this._topBullet.DoUpdate();
        int num = (int) ((double) this.ammo / (double) this.bulletsTillRemove);
        if (num < this.numHanging)
        {
          this._topBullet = this._topBullet.childThing as ChaingunBullet;
          if (this._topBullet != null)
            this._topBullet.parentThing = (Thing) this;
        }
        this.numHanging = num;
      }
      this._fireWait = (float) (0.699999988079071 + (double) Maths.NormalizeSection(this._barrelHeat, 5f, 9f) * 5.0);
      if ((double) this._barrelHeat > 11.0)
        this._barrelHeat = 11f;
      this._barrelHeat -= 0.005f;
      if ((double) this._barrelHeat < 0.0)
        this._barrelHeat = 0.0f;
      this._sprite.speed = this._spin;
      this._tip.speed = this._spin;
      if ((double) this._spin > 0.0)
        this._spin -= 0.01f;
      else
        this._spin = 0.0f;
      base.Update();
      if (this._topBullet == null)
        return;
      if (!this.graphic.flipH)
        this._topBullet.chainOffset = new Vec2(1f, 5f);
      else
        this._topBullet.chainOffset = new Vec2(-1f, 5f);
    }

    public override void Draw()
    {
      base.Draw();
      this._tip.flipH = this.graphic.flipH;
      this._tip.center = this.graphic.center;
      this._tip.depth = this.depth + 1;
      this._tip.alpha = Math.Min((float) ((double) this._barrelHeat * 1.5 / 10.0), 1f);
      this._tip.angle = this.angle;
      Graphics.Draw((Sprite) this._tip, this.x, this.y);
      if (this._topBullet == null)
        return;
      this._topBullet.DoDraw();
    }
  }
}
