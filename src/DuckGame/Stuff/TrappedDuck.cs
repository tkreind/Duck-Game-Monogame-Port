// Decompiled with JetBrains decompiler
// Type: DuckGame.TrappedDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class TrappedDuck : Holdable, IPlatform, IAmADuck
  {
    public StateBinding _duckOwnerBinding = new StateBinding(nameof (_duckOwner));
    public Duck _duckOwner;
    public float _trapTime = 1f;
    public float _shakeMult;
    private float _shakeInc;
    public byte funNum;
    public bool infinite;
    private float jumpCountdown;
    private bool _prevVisible;

    public Duck captureDuck => this._duckOwner;

    public override bool visible
    {
      get => base.visible;
      set
      {
        if (NetworkDebugger.networkDrawingIndex == 0 && this._duckOwner.netProfileIndex == (byte) 1 && (value || !base.visible) && value)
        {
          int num1 = base.visible ? 1 : 0;
        }
        if (this._duckOwner.netProfileIndex == (byte) 1 && this._duckOwner.isServerForObject && (value || !base.visible) && value)
        {
          int num2 = base.visible ? 1 : 0;
        }
        if (value && (double) this._trapTime < 0.0)
        {
          this._trapTime = 1f;
          this.owner = (Thing) null;
        }
        base.visible = value;
      }
    }

    public TrappedDuck(float xpos, float ypos, Duck duckowner)
      : base(xpos, ypos)
    {
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this.thickness = 0.5f;
      this.weight = 5f;
      this.flammable = 1f;
      this.burnSpeed = 0.0f;
      this._duckOwner = duckowner;
      this.InitializeStuff();
    }

    public void InitializeStuff() => this._trapTime = 1f;

    protected override bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      if (this._duckOwner != null)
        this._duckOwner.Burn(firePosition, litBy);
      return base.OnBurn(firePosition, litBy);
    }

    public override void Terminate() => base.Terminate();

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (this._duckOwner == null)
        return false;
      if (!this.destroyed)
      {
        this._duckOwner.hSpeed = this.hSpeed;
        bool flag = type != null;
        if (!flag && (double) this.jumpCountdown > 0.00999999977648258)
          this._duckOwner.vSpeed = Duck.JumpSpeed;
        else
          this._duckOwner.vSpeed = flag ? this.vSpeed - 1f : -3f;
        this._duckOwner.x = this.x;
        this._duckOwner.y = this.y - 10f;
        for (int index = 0; index < 4; ++index)
        {
          SmallSmoke smallSmoke = SmallSmoke.New(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f));
          smallSmoke.hSpeed += this.hSpeed * Rando.Float(0.3f, 0.5f);
          smallSmoke.vSpeed -= Rando.Float(0.1f, 0.2f);
          Level.Add((Thing) smallSmoke);
        }
        if (this.owner != null && this.owner is Duck owner)
          owner.holdObject = (Holdable) null;
        if (Network.isActive)
        {
          if (!flag)
          {
            this._duckOwner.Fondle((Thing) this);
            TrappedDuck trappedDuck = this;
            trappedDuck.authority = trappedDuck.authority + 45;
          }
          this.active = false;
          this.visible = false;
          this.owner = (Thing) null;
        }
        else
          Level.Remove((Thing) this);
        if (flag && !this._duckOwner.killingNet)
        {
          this._duckOwner.killingNet = true;
          this._duckOwner.Destroy(type);
        }
        this._duckOwner._trapped = (TrappedDuck) null;
      }
      return true;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal)
        this.OnDestroy((DestroyType) new DTShot(bullet));
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
    }

    public override void Update()
    {
      if (Network.isActive && this._prevVisible && !this.visible)
      {
        for (int index = 0; index < 4; ++index)
        {
          SmallSmoke smallSmoke = SmallSmoke.New(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f));
          smallSmoke.hSpeed += this.hSpeed * Rando.Float(0.3f, 0.5f);
          smallSmoke.vSpeed -= Rando.Float(0.1f, 0.2f);
          Level.Add((Thing) smallSmoke);
        }
      }
      if (this._duckOwner == null)
        return;
      ++this._framesSinceTransfer;
      base.Update();
      if ((double) this.y > (double) Level.current.lowestPoint + 100.0)
        this.OnDestroy((DestroyType) new DTFall());
      this.jumpCountdown -= Maths.IncFrameTimer();
      this._prevVisible = this.visible;
      this._shakeInc += 0.8f;
      this._shakeMult = Lerp.Float(this._shakeMult, 0.0f, 0.05f);
      if (Network.isActive && this._duckOwner._trapped == this && (!this._duckOwner.isServerForObject && this._duckOwner.inputProfile.Pressed("JUMP")))
        this._shakeMult = 1f;
      if (!this._duckOwner.isServerForObject || this._duckOwner._trapped != this)
        return;
      if (!this.visible)
        this.y = -9999f;
      if (!this.infinite)
      {
        this._duckOwner.profile.stats.timeInNet += Maths.IncFrameTimer();
        if (this._duckOwner.inputProfile.Pressed("JUMP"))
        {
          this._shakeMult = 1f;
          this._trapTime -= 0.007f;
          this.jumpCountdown = 0.25f;
        }
        if (this.grounded && this._duckOwner.inputProfile.Pressed("JUMP"))
        {
          this._shakeMult = 1f;
          this._trapTime -= 0.028f;
          if (this.owner == null)
          {
            if ((double) Math.Abs(this.hSpeed) < 1.0 && this._framesSinceTransfer > 30)
              this._duckOwner.Fondle((Thing) this);
            this.vSpeed -= Rando.Float(0.8f, 1.1f);
            if (this._duckOwner.inputProfile.Down("LEFT") && (double) this.hSpeed > -1.0)
              this.hSpeed -= Rando.Float(0.6f, 0.8f);
            if (this._duckOwner.inputProfile.Down("RIGHT") && (double) this.hSpeed < 1.0)
              this.hSpeed += Rando.Float(0.6f, 0.8f);
          }
        }
        if (this._duckOwner.inputProfile.Pressed("JUMP") && this._duckOwner.HasEquipment(typeof (Jetpack)))
          this._duckOwner.GetEquipment(typeof (Jetpack)).PressAction();
        if (this._duckOwner.inputProfile.Released("JUMP") && this._duckOwner.HasEquipment(typeof (Jetpack)))
          this._duckOwner.GetEquipment(typeof (Jetpack)).ReleaseAction();
        this._trapTime -= 0.0028f;
        if ((double) this._trapTime <= 0.0 || this._duckOwner.dead)
          this.OnDestroy((DestroyType) null);
      }
      if (this.owner == null)
        this.depth = this._duckOwner.depth;
      this._duckOwner.position = this.position;
      this._duckOwner.UpdateSkeleton();
    }

    public override void Draw()
    {
      if (this._duckOwner == null)
        return;
      this._duckOwner._sprite.SetAnimation("netted");
      this._duckOwner._sprite.imageIndex = 14;
      this._duckOwner._spriteQuack.frame = this._duckOwner._sprite.frame;
      this._duckOwner._sprite.depth = this.depth;
      this._duckOwner._spriteQuack.depth = this.depth;
      float num = 0.0f;
      if (this.owner != null)
        num = (float) (Math.Sin((double) this._shakeInc) * (double) this._shakeMult * 1.0);
      if (this._duckOwner.quack > 0)
        Graphics.Draw(this._duckOwner._spriteQuack, this._duckOwner._sprite.imageIndex, this.x + num, this.y - 8f);
      else
        Graphics.Draw((Sprite) this._duckOwner._sprite, this.x + num, this.y - 8f);
      base.Draw();
    }
  }
}
