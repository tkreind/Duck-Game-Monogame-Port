// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsParticle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public abstract class PhysicsParticle : Thing
  {
    public ushort netIndex;
    protected bool _grounded;
    protected float _spinAngle;
    protected string _bounceSound = "";
    protected float _bounceEfficiency = 0.5f;
    protected float _gravMult = 1f;
    protected float _sticky;
    protected bool _foreverGrounded;
    protected float _stickDir;
    public bool gotMessage;
    public bool needsSynchronization;
    protected bool _hit;
    protected bool _touchedFloor;
    private float _framesAlive;
    private bool _waitForNoCollide;
    protected float _airFriction = 0.03f;
    protected float _life = 1f;
    public Vec2 lerpPos = Vec2.Zero;
    public Vec2 lerpSpeed = Vec2.Zero;
    protected Vec2 netLerpPosition = Vec2.Zero;
    public bool onlyDieWhenGrounded;

    public float spinAngle
    {
      get => this._spinAngle;
      set => this._spinAngle = value;
    }

    public float life
    {
      get => this._life;
      set => this._life = value;
    }

    public PhysicsParticle(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public void LerpPosition(Vec2 pos) => this.lerpPos = pos;

    public void LerpSpeed(Vec2 speed) => this.lerpSpeed = speed;

    public byte GetNetType()
    {
      switch (this)
      {
        case SmallFire _:
          return 0;
        case ExtinguisherSmoke _:
          return 1;
        default:
          return byte.MaxValue;
      }
    }

    public static System.Type ConvertNetType(byte type)
    {
      if (type == (byte) 0)
        return typeof (SmallFire);
      return type == (byte) 1 ? typeof (ExtinguisherSmoke) : (System.Type) null;
    }

    public virtual void NetSerialize(BitBuffer b)
    {
      b.Write((short) this.x);
      b.Write((short) this.y);
    }

    public virtual void NetDeserialize(BitBuffer d) => this.netLerpPosition = new Vec2((float) d.ReadShort(), (float) d.ReadShort());

    public override void ResetProperties()
    {
      this._life = 1f;
      this._grounded = false;
      this._spinAngle = 0.0f;
      this._foreverGrounded = false;
      this.alpha = 1f;
      this._airFriction = 0.03f;
      this.vSpeed = 0.0f;
      this.hSpeed = 0.0f;
      this._framesAlive = 0.0f;
      this._waitForNoCollide = false;
      this.globalIndex = Thing.GetGlobalIndex();
      this.gotMessage = false;
      this.isLocal = true;
      this.netIndex = (ushort) 0;
      base.ResetProperties();
    }

    public override void Update()
    {
      if (!this.isLocal)
      {
        Vec2 position = this.position;
        Vec2 netLerpPosition = this.netLerpPosition;
        if ((double) (position - netLerpPosition).lengthSq > 2048.0 || (double) (position - netLerpPosition).lengthSq < 1.0)
          this.position = netLerpPosition;
        else
          this.position = Lerp.Vec2Smooth(position, netLerpPosition, 0.2f);
      }
      else if (Network.isActive && ((double) this.y < -200.0 || (double) this.y > (double) Level.current.lowestPoint + 200.0))
      {
        Level.Remove((Thing) this);
      }
      else
      {
        this._hit = false;
        this._touchedFloor = false;
        ++this._framesAlive;
        if (!this.onlyDieWhenGrounded || this._grounded || (double) this._framesAlive > 400.0)
        {
          this._life -= 0.005f;
          if ((double) this._life < 0.0)
          {
            this.alpha -= 0.1f;
            if ((double) this.alpha < 0.0)
              Level.Remove((Thing) this);
          }
        }
        if (this._foreverGrounded)
        {
          this._grounded = true;
          if ((double) Rando.Float(250f) < 1.0 - (double) this._sticky)
          {
            this._foreverGrounded = false;
            this._grounded = false;
            this.hSpeed = -this._stickDir * Rando.Float(0.8f);
          }
        }
        if (!this._grounded)
        {
          if ((double) this.hSpeed > 0.0)
            this.hSpeed -= this._airFriction;
          if ((double) this.hSpeed < 0.0)
            this.hSpeed += this._airFriction;
          if ((double) this.hSpeed < (double) this._airFriction && (double) this.hSpeed > -(double) this._airFriction)
            this.hSpeed = 0.0f;
          if ((double) this.vSpeed < 4.0)
            this.vSpeed += 0.1f * this._gravMult;
          if (float.IsNaN(this.hSpeed))
            this.hSpeed = 0.0f;
          this._spinAngle -= (float) (10 * Math.Sign(this.hSpeed));
          Thing thing = (Thing) Level.CheckPoint<Block>(this.x + this.hSpeed, this.y + this.vSpeed);
          if (thing != null && (double) this._framesAlive < 2.0)
            this._waitForNoCollide = true;
          if (thing != null && this._waitForNoCollide)
            thing = (Thing) null;
          else if (thing == null && this._waitForNoCollide)
            this._waitForNoCollide = false;
          if (thing != null)
          {
            this._touchedFloor = true;
            if (this._bounceSound != "" && ((double) Math.Abs(this.vSpeed) > 1.0 || (double) Math.Abs(this.hSpeed) > 1.0))
              SFX.Play(this._bounceSound, 0.5f, Rando.Float(0.2f) - 0.1f);
            if ((double) this.vSpeed > 0.0 && (double) thing.top > (double) this.y)
            {
              this.vSpeed = (float) -((double) this.vSpeed * (double) this._bounceEfficiency);
              this._hit = true;
              if ((double) Math.Abs(this.vSpeed) < 0.5)
              {
                this.vSpeed = 0.0f;
                this._grounded = true;
              }
            }
            else if ((double) this.vSpeed < 0.0 && (double) thing.bottom < (double) this.y)
            {
              this.vSpeed = (float) -((double) this.vSpeed * (double) this._bounceEfficiency);
              this._hit = true;
            }
            if ((double) this.hSpeed > 0.0 && (double) thing.left > (double) this.x)
            {
              this.hSpeed = (float) -((double) this.hSpeed * (double) this._bounceEfficiency);
              this._hit = true;
              if ((double) this._sticky > 0.0 && (double) Rando.Float(1f) < (double) this._sticky)
              {
                this.hSpeed = 0.0f;
                this.vSpeed = 0.0f;
                this._foreverGrounded = true;
                this._stickDir = 1f;
              }
            }
            else if ((double) this.hSpeed < 0.0 && (double) thing.right < (double) this.x)
            {
              this.hSpeed = (float) -((double) this.hSpeed * (double) this._bounceEfficiency);
              this._hit = true;
              if ((double) this._sticky > 0.0 && (double) Rando.Float(1f) < (double) this._sticky)
              {
                this.hSpeed = 0.0f;
                this.vSpeed = 0.0f;
                this._foreverGrounded = true;
                this._stickDir = -1f;
              }
            }
            if (!this._hit)
              this._grounded = true;
          }
          else
          {
            this.x += this.hSpeed;
            this.y += this.vSpeed;
          }
        }
        if ((double) this._spinAngle > 360.0)
          this._spinAngle -= 360f;
        if ((double) this._spinAngle >= 0.0)
          return;
        this._spinAngle += 360f;
      }
    }
  }
}
