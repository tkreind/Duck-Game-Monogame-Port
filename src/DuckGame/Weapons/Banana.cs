﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Banana
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("guns|explosives")]
  [BaggedProperty("isFatal", false)]
  public class Banana : Gun
  {
    public StateBinding _bananaStateBinding = (StateBinding) new StateFlagBinding(new string[2]
    {
      nameof (_pin),
      nameof (_thrown)
    });
    public StateBinding _netSplatBinding = (StateBinding) new NetSoundBinding(nameof (_netSplat));
    public StateBinding _netSlipBinding = (StateBinding) new NetSoundBinding(nameof (_netSlip));
    public NetSoundEffect _netSplat = new NetSoundEffect(new string[1]
    {
      "smallSplat"
    });
    public NetSoundEffect _netSlip = new NetSoundEffect(new string[1]
    {
      "slip"
    });
    private SpriteMap _sprite;
    public bool _pin = true;
    public bool _thrown;
    private bool _fade;
    private bool _splatted;

    public bool pin => this._pin;

    public override float angle
    {
      get
      {
        if (this.owner == null)
          return base.angle;
        return this.offDir > (sbyte) 0 ? base.angle + 1.570796f : base.angle - 1.570796f;
      }
      set => this._angle = value;
    }

    public Banana(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 1;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._type = "gun";
      this._sprite = new SpriteMap("banana", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 13f);
      this.collisionOffset = new Vec2(-6f, -3f);
      this.collisionSize = new Vec2(12f, 5f);
      this._holdOffset = new Vec2(-1f, 2f);
      this.bouncy = 0.4f;
      this.friction = 0.05f;
    }

    public override void Update()
    {
      base.Update();
      if (this._thrown && this.owner == null)
      {
        this._thrown = false;
        if ((double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 0.400000005960464)
          this.angleDegrees = 180f;
      }
      if (!this._pin && this.owner == null && !this._fade)
      {
        this._sprite.frame = 2;
        this.weight = 0.1f;
      }
      if (this._fade)
      {
        this.alpha -= 0.1f;
        if ((double) this.alpha <= 0.0)
        {
          Level.Remove((Thing) this);
          this.alpha = 0.0f;
        }
      }
      if (!this._pin && this.owner == null)
        this.canPickUp = false;
      if (!this._pin && this._grounded && !this._fade)
      {
        if (!this._splatted)
        {
          this._splatted = true;
          if (Network.isActive)
          {
            if (this.isServerForObject)
              this._netSplat.Play(pit: Rando.Float(-0.2f, 0.2f));
          }
          else
            SFX.Play("smallSplat", pitch: Rando.Float(-0.2f, 0.2f));
        }
        this.angleDegrees = 0.0f;
        this.canPickUp = false;
        foreach (Duck duck in Level.CheckLineAll<Duck>(new Vec2(this.x - 5f, this.y + 2f), new Vec2(this.x + 5f, this.y + 2f)))
        {
          if (duck.grounded && !duck.crouch && (!duck.sliding && (double) duck.bottom <= (double) this.bottom + 2.0) && (duck.isServerForObject && (double) Math.Abs(duck.hSpeed) > 2.5))
          {
            duck.Fondle((Thing) this);
            if (Network.isActive)
            {
              if (this.isServerForObject)
                this._netSlip.Play(pit: Rando.Float(-0.2f, 0.2f));
              if (Teams.active.Count > 1 && Rando.Int(100) == 1 && duck.connection == DuckNetwork.localConnection)
                DuckNetwork.GiveXP("Banana Man", 0, 5, firstCap: 20, secondCap: 30, finalCap: 40);
            }
            else
              SFX.Play("slip", pitch: Rando.Float(-0.2f, 0.2f));
            if ((double) duck.hSpeed < 0.0)
              duck.hSpeed -= 1.5f;
            else
              duck.hSpeed += 1.5f;
            duck.vSpeed -= 2.5f;
            this.hSpeed = (float) (-(double) duck.hSpeed * 0.400000005960464);
            this.friction = 0.05f;
            this.weight = 0.01f;
            duck.crippleTimer = 1.5f;
            PhysicsObject holdObject = (PhysicsObject) duck.holdObject;
            if (holdObject != null)
            {
              duck.ThrowItem(false);
              holdObject.vSpeed -= 4f;
              holdObject.hSpeed = duck.hSpeed * 0.8f;
              holdObject.clip.Add((MaterialThing) duck);
              duck.clip.Add((MaterialThing) holdObject);
            }
            duck.GoRagdoll();
            if (holdObject != null)
            {
              duck.ragdoll.part1.clip.Add((MaterialThing) holdObject);
              duck.ragdoll.part2.clip.Add((MaterialThing) holdObject);
              duck.ragdoll.part3.clip.Add((MaterialThing) holdObject);
              holdObject.clip.Add((MaterialThing) duck.ragdoll.part1);
              holdObject.clip.Add((MaterialThing) duck.ragdoll.part2);
              holdObject.clip.Add((MaterialThing) duck.ragdoll.part3);
            }
            this._sprite.frame = 3;
            this._fade = true;
            duck.ragdoll.part1.hSpeed *= 0.5f;
            duck.ragdoll.part3.hSpeed *= 1.5f;
            Level.Add((Thing) new BananaSlip(this.x, this.y + 2f, duck.offDir > (sbyte) 0));
          }
        }
      }
      if (this._triggerHeld)
      {
        if (this.duck == null)
          return;
        this.duck.quack = 20;
        if (this.offDir > (sbyte) 0)
        {
          this.handAngle = -1.099557f;
          this.handOffset = new Vec2(8f, -1f);
          this._holdOffset = new Vec2(-1f, 10f);
        }
        else
        {
          this.handAngle = 1.099557f;
          this.handOffset = new Vec2(8f, -1f);
          this._holdOffset = new Vec2(-1f, 10f);
        }
      }
      else
      {
        this.handAngle = 0.0f;
        this.handOffset = new Vec2(0.0f, 0.0f);
        this._holdOffset = new Vec2(-1f, 2f);
      }
    }

    public override void HeatUp(Vec2 location)
    {
    }

    public void EatBanana()
    {
      this._sprite.frame = 1;
      this._pin = false;
      this._holdOffset = new Vec2(-2f, 3f);
      this.collisionOffset = new Vec2(-4f, -2f);
      this.collisionSize = new Vec2(8f, 4f);
      this.weight = 0.01f;
      if (Network.isActive)
      {
        if (this.isServerForObject)
          this._netSplat.Play(pit: Rando.Float(-0.6f, 0.6f));
      }
      else
        SFX.Play("smallSplat", pitch: Rando.Float(-0.6f, 0.6f));
      this.bouncy = 0.0f;
      this.friction = 0.3f;
    }

    public override void OnPressAction()
    {
      if (!this.pin)
        return;
      this.EatBanana();
    }

    public override void OnHoldAction()
    {
    }

    public override void OnReleaseAction()
    {
    }
  }
}
