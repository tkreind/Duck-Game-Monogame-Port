﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.TampingWeapon
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class TampingWeapon : Gun
  {
    public StateBinding _tampedBinding = (StateBinding) new StateFlagBinding(new string[3]
    {
      nameof (_tamped),
      "tamping",
      nameof (_rotating)
    });
    public StateBinding _tampIncBinding = new StateBinding(nameof (_tampInc));
    public StateBinding _tampTimeBinding = new StateBinding(nameof (_tampTime));
    public StateBinding _offsetYBinding = new StateBinding(nameof (_offsetY));
    public StateBinding _rotAngleBinding = new StateBinding(nameof (_rotAngle));
    public bool _tamped = true;
    public float _tampInc;
    public float _tampTime;
    public bool _rotating;
    public float _offsetY;
    public float _rotAngle;
    public float _tampBoost = 1f;
    private Sprite _tampingHand;
    private bool _puffed;

    public override float angle
    {
      get => base.angle + Maths.DegToRad(-this._rotAngle);
      set => this._angle = value;
    }

    public TampingWeapon(float xval, float yval)
      : base(xval, yval)
    {
      this._tampingHand = new Sprite("tampingHand");
      this._tampingHand.center = new Vec2(4f, 8f);
    }

    public override void Update()
    {
      base.Update();
      this._tampBoost = Lerp.Float(this._tampBoost, 1f, 0.01f);
      if (this.owner is Duck owner && owner.inputProfile != null && (this.duck != null && this.duck.profile != null))
      {
        if (owner.inputProfile.Pressed("SHOOT"))
          this._tampBoost += 0.14f;
        if (this.duck.immobilized)
          this.duck.profile.stats.timeSpentReloadingOldTimeyWeapons += Maths.IncFrameTimer();
        if (this._rotating)
        {
          if (this.offDir < (sbyte) 0)
          {
            if ((double) this._rotAngle > -90.0)
              this._rotAngle -= 3f;
            if ((double) this._rotAngle <= -90.0)
            {
              this.tamping = true;
              this._tampInc += 0.2f * this._tampBoost;
              this.tampPos = (float) Math.Sin((double) this._tampInc) * 2f;
              if ((double) this.tampPos < -1.0 && !this._puffed)
              {
                Vec2 vec2 = this.Offset(this.barrelOffset) - this.barrelVector * 8f;
                Level.Add((Thing) SmallSmoke.New(vec2.x, vec2.y));
                this._puffed = true;
              }
              if ((double) this.tampPos > -1.0)
                this._puffed = false;
              this._tampTime += 0.005f * this._tampBoost;
            }
            if ((double) this._tampTime >= 1.0)
            {
              this._rotAngle += 8f;
              if ((double) this._offsetY > 0.0)
                this._offsetY -= 2f;
              this.tamping = false;
              if ((double) this._rotAngle >= 0.0)
              {
                this._rotAngle = 0.0f;
                this._rotating = false;
                this._tamped = true;
                this._offsetY = 0.0f;
                owner.immobilized = false;
              }
            }
          }
          else
          {
            if ((double) this._rotAngle < 90.0)
              this._rotAngle += 3f;
            if ((double) this._rotAngle >= 90.0)
            {
              this.tamping = true;
              this._tampInc += 0.2f * this._tampBoost;
              this.tampPos = (float) Math.Sin((double) this._tampInc) * 2f;
              if ((double) this.tampPos < -1.0 && !this._puffed)
              {
                Vec2 vec2 = this.Offset(this.barrelOffset) - this.barrelVector * 8f;
                Level.Add((Thing) SmallSmoke.New(vec2.x, vec2.y));
                this._puffed = true;
              }
              if ((double) this.tampPos > -1.0)
                this._puffed = false;
              this._tampTime += 0.005f * this._tampBoost;
            }
            if ((double) this._tampTime >= 1.0)
            {
              this._rotAngle -= 8f;
              if ((double) this._offsetY > 0.0)
                this._offsetY -= 2f;
              this.tamping = false;
              if ((double) this._rotAngle <= 0.0)
              {
                this._rotAngle = 0.0f;
                this._rotating = false;
                this._tamped = true;
                this._offsetY = 0.0f;
                owner.immobilized = false;
              }
            }
          }
          if ((double) this._offsetY >= 10.0)
            return;
          ++this._offsetY;
        }
        else
          this._tampBoost = 1f;
      }
      else
      {
        if (this.prevOwner == null || !(this.prevOwner is Duck) || !(this.prevOwner is Duck prevOwner))
          return;
        prevOwner.immobilized = false;
        this.tamping = false;
        this._rotAngle = 0.0f;
        this._rotating = false;
        this._offsetY = 0.0f;
        this._prevOwner = (Thing) null;
      }
    }

    public override void Draw()
    {
      this.y += this._offsetY;
      base.Draw();
      if (this.duck != null && this.tamping)
      {
        if (this.offDir < (sbyte) 0)
        {
          this._tampingHand.x = this.x + 3f;
          this._tampingHand.y = this.y - 16f + this.tampPos;
          this._tampingHand.flipH = true;
        }
        else
        {
          this._tampingHand.x = this.x - 3f;
          this._tampingHand.y = this.y - 16f + this.tampPos;
          this._tampingHand.flipH = false;
        }
        this._tampingHand.depth = this.depth - 1;
        float angle = this.duck._spriteArms.angle;
        Vec2 vec2 = this.Offset(this.barrelOffset);
        Vec2 p2 = vec2 + this.barrelVector * (float) ((double) this.tampPos * 2.0 + 3.0);
        Graphics.DrawLine(vec2 - this.barrelVector * 6f, p2, Color.Gray, depth: (this.depth - 2));
        this.duck._spriteArms.depth = this.depth - 1;
        Graphics.Draw((Sprite) this.duck._spriteArms, p2.x, p2.y);
        this.duck._spriteArms.angle = angle;
      }
      this.position = new Vec2(this.position.x, this.position.y - this._offsetY);
    }
  }
}
