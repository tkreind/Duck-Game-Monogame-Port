// Decompiled with JetBrains decompiler
// Type: DuckGame.MagnetGun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isFatal", false)]
  [EditorGroup("guns|misc")]
  public class MagnetGun : Gun
  {
    public StateBinding _grabbedBinding = new StateBinding(nameof (_grabbed));
    public StateBinding _magnetActiveBinding = new StateBinding(nameof (_magnetActive));
    public StateBinding _keepRaisedBinding = new StateBinding("_keepRaised");
    public StateBinding _attachIndexBinding = new StateBinding(nameof (attachIndex));
    public NetIndex4 attachIndex = new NetIndex4(0);
    public NetIndex4 localAttachIndex = new NetIndex4(0);
    private Sprite _magnet;
    private SinWave _wave = (SinWave) 0.8f;
    private float _waveMult;
    public Thing _grabbed;
    private Block _stuck;
    private Vec2 _stickPos = Vec2.Zero;
    private Vec2 _stickNormal = Vec2.Zero;
    private Sound _beamSound;
    public bool _magnetActive;
    private List<MagnaLine> _lines = new List<MagnaLine>();
    private Vec2 _rayHit;
    private bool _hasRay;
    public Thing _prevGrabDuck;

    public MagnetGun(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 99;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 150f;
      this._ammoType.accuracy = 0.8f;
      this._ammoType.penetration = -1f;
      this._type = "gun";
      this.graphic = new Sprite("magnetGun");
      this.center = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(14f, 9f);
      this._barrelOffsetTL = new Vec2(24f, 14f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._magnet = new Sprite("magnet");
      this._magnet.CenterOrigin();
      this._bio = "Nope.";
      this._editorName = "Magnet Gun";
      this._holdOffset = new Vec2(3f, 1f);
      this._lowerOnFire = false;
    }

    public override void Initialize()
    {
      this._beamSound = SFX.Get("magnetBeam", 0.0f, looped: true);
      int num = 10;
      for (int index = 0; index < num; ++index)
        this._lines.Add(new MagnaLine(0.0f, 0.0f, (Gun) this, this._ammoType.range, (float) index / (float) num));
      base.Initialize();
    }

    public override void CheckIfHoldObstructed()
    {
      if (this._stuck != null)
        return;
      base.CheckIfHoldObstructed();
    }

    public override void Update()
    {
      this._waveMult = Lerp.Float(this._waveMult, 0.0f, 0.1f);
      if (this.isServerForObject)
        this._magnetActive = this.action;
      else if (this._magnetActive)
        this._waveMult = 1f;
      if ((double) this._beamSound.Volume > 0.00999999977648258 && this._beamSound.State != SoundState.Playing)
        this._beamSound.Play();
      else if ((double) this._beamSound.Volume < 0.00999999977648258 && this._beamSound.State == SoundState.Playing)
        this._beamSound.Stop();
      this._beamSound.Volume = Maths.LerpTowards(this._beamSound.Volume, this._magnetActive ? 0.1f : 0.0f, 0.1f);
      Vec2 p1_1 = this.Offset(this.barrelOffset);
      if (this._magnetActive && this.duck != null && this.duck.holdObject == this)
      {
        foreach (MagnaLine line in this._lines)
        {
          line.Update();
          line.show = true;
          float num = this._ammoType.range;
          if (this._hasRay)
            num = (this.barrelPosition - this._rayHit).length;
          line.dist = num;
        }
        if (this._grabbed == null && this._stuck == null)
        {
          Holdable holdable1 = (Holdable) null;
          float val1 = 0.0f;
          Vec2 normalized1 = this.barrelVector.Rotate(Maths.DegToRad(90f), Vec2.Zero).normalized;
          for (int index = 0; index < 3; ++index)
          {
            Vec2 p1_2 = p1_1;
            if (index == 0)
              p1_2 += normalized1 * 8f;
            else if (index == 2)
              p1_2 -= normalized1 * 8f;
            foreach (Holdable holdable2 in Level.CheckLineAll<Holdable>(p1_2, p1_2 + this.barrelVector * this._ammoType.range))
            {
              if (holdable2 != this && holdable2 != this.owner && (holdable2.owner != this.owner && holdable2.physicsMaterial == PhysicsMaterial.Metal) && (holdable2.duck == null || !(holdable2.duck.holdObject is MagnetGun)))
              {
                float length = (holdable2.position - p1_1).length;
                if (holdable1 == null || (double) length < (double) val1)
                {
                  val1 = length;
                  holdable1 = holdable2;
                }
              }
            }
          }
          this._hasRay = false;
          if (holdable1 != null && Level.CheckLine<Block>(p1_1, holdable1.position) == null)
          {
            float num = (float) ((1.0 - (double) Math.Min(val1, this._ammoType.range) / (double) this._ammoType.range) * 0.800000011920929);
            if (holdable1.owner is Duck duck && !(duck.holdObject is MagnetGun) && (double) num > 0.300000011920929)
            {
              if (!(holdable1 is Equipment) || holdable1.equippedDuck == null)
              {
                duck.ThrowItem(false);
                duck = (Duck) null;
              }
              else if (holdable1 is TinfoilHat)
              {
                duck.Unequip(holdable1 as Equipment);
                duck = (Duck) null;
              }
            }
            Vec2 normalized2 = (p1_1 - holdable1.position).normalized;
            // TODO if (duck != null && holdable1 is Equipment)
            if (false)
            {
              if (duck.ragdoll != null)
              {
                duck.ragdoll.makeActive = true;
                return;
              }
              if (!(holdable1.owner.realObject is Duck) && Network.isActive)
                return;
              holdable1.owner.realObject.hSpeed += normalized2.x * num;
              holdable1.owner.realObject.vSpeed += (float) ((double) normalized2.y * (double) num * 4.0);
              if ((holdable1.owner.realObject as PhysicsObject).grounded && (double) holdable1.owner.realObject.vSpeed > 0.0)
                holdable1.owner.realObject.vSpeed = 0.0f;
            }
            else
            {
              this.Fondle((Thing) holdable1);
              holdable1.hSpeed += normalized2.x * num;
              holdable1.vSpeed += (float) ((double) normalized2.y * (double) num * 4.0);
              if (holdable1.grounded && (double) holdable1.vSpeed > 0.0)
                holdable1.vSpeed = 0.0f;
            }
            this._hasRay = true;
            this._rayHit = holdable1.position;
            if (this.isServerForObject && (double) val1 < 20.0)
            {
              if (holdable1 is Equipment && holdable1.duck != null)
              {
                this._grabbed = holdable1.owner.realObject;
                holdable1.duck.immobilized = true;
                holdable1.duck.gripped = true;
                holdable1.duck.ThrowItem();
                if (!(holdable1.owner.realObject is Duck))
                {
                  holdable1.owner.realObject.owner = this.owner;
                  Thing.SuperFondle(holdable1.owner.realObject, DuckNetwork.localConnection);
                }
              }
              else
              {
                this._grabbed = (Thing) holdable1;
                holdable1.owner = this.owner;
                if (holdable1 is Grenade)
                  (holdable1 as Grenade).OnPressAction();
              }
              this.attachIndex += 1;
            }
          }
          else if (this.isServerForObject && this._stuck == null && ((double) Math.Abs(this.angle) < 0.0500000007450581 || (double) Math.Abs(this.angle) > 1.5))
          {
            Vec2 position = this.owner.position;
            if (this.duck.sliding)
              position.y += 4f;
            Vec2 hitPos;
            Block block = Level.CheckRay<Block>(position, position + this.barrelVector * this._ammoType.range, out hitPos);
            this._hasRay = true;
            this._rayHit = hitPos;
            if (block != null && block.physicsMaterial == PhysicsMaterial.Metal)
            {
              float num = (float) ((1.0 - (double) Math.Min((block.position - position).length, this._ammoType.range) / (double) this._ammoType.range) * 0.800000011920929);
              Vec2 vec2 = hitPos - this.duck.position;
              float length = vec2.length;
              vec2.Normalize();
              this.owner.hSpeed += vec2.x * num;
              this.owner.vSpeed += vec2.y * num;
              if ((double) length < 20.0)
              {
                this._stuck = block;
                this._stickPos = hitPos;
                this._stickNormal = -this.barrelVector;
                this.attachIndex += 1;
              }
            }
          }
        }
      }
      else
      {
        if (this.isServerForObject)
        {
          if (this._grabbed != null)
          {
            this._grabbed.angle = 0.0f;
            if (this._grabbed is Holdable grabbed)
            {
              grabbed.owner = (Thing) null;
              grabbed.ReturnToWorld();
              this.ReturnItemToWorld((Thing) grabbed);
            }
            if (this._grabbed is Duck grabbed2)
            {
              grabbed2.immobilized = false;
              grabbed2.gripped = false;
              grabbed2.crippleTimer = 1f;
            }
            this._grabbed.visible = true;
            this._grabbed.enablePhysics = true;
            this._grabbed.hSpeed = this.barrelVector.x * 5f;
            this._grabbed.vSpeed = this.barrelVector.y * 5f;
            this._grabbed = (Thing) null;
            this._collisionSize = new Vec2(14f, this._collisionSize.y);
          }
          if (this._stuck != null)
          {
            this._stuck = (Block) null;
            if (this.owner != null && !this._raised)
              this.duck._groundValid = 6;
          }
        }
        foreach (MagnaLine line in this._lines)
          line.show = false;
      }
      if (Network.isActive)
      {
        if (this._grabbed != null)
        {
          if (this._grabbed is TrappedDuck && this._grabbed.connection != this.connection)
          {
            this._grabbed = (Thing) (this._grabbed as TrappedDuck)._duckOwner;
            if (this._grabbed != null)
            {
              Duck grabbed2 = this._grabbed as Duck;
              grabbed2.immobilized = true;
              grabbed2.gripped = true;
              grabbed2.ThrowItem();
              grabbed2._trapped = (TrappedDuck) null;
            }
          }
          if (this._grabbed is Duck grabbed)
          {
            grabbed.isGrabbedByMagnet = true;
            if (this.isServerForObject)
            {
              this.Fondle((Thing) grabbed);
              this.Fondle((Thing) grabbed.holdObject);
              foreach (Thing t in grabbed._equipment)
                this.Fondle(t);
              this.Fondle((Thing) grabbed._ragdollInstance);
              this.Fondle((Thing) grabbed._trappedInstance);
              this.Fondle((Thing) grabbed._cookedInstance);
            }
          }
        }
        if (this._grabbed == null && this._prevGrabDuck != null && this._prevGrabDuck is Duck)
          (this._prevGrabDuck as Duck).isGrabbedByMagnet = false;
        this._prevGrabDuck = this._grabbed;
      }
      if (this._grabbed != null && this.owner != null)
      {
        if (this.isServerForObject)
          this.Fondle(this._grabbed);
        this._grabbed.hSpeed = this.owner.hSpeed;
        this._grabbed.vSpeed = this.owner.vSpeed;
        this._grabbed.angle = this.angle;
        this._grabbed.visible = false;
        this._grabbed.offDir = this.offDir;
        this._grabbed.enablePhysics = false;
        this._collisionSize = new Vec2(16f + this._grabbed.width, this._collisionSize.y);
        if (this._grabbed is Duck grabbed)
        {
          grabbed.grounded = true;
          grabbed.sliding = false;
          grabbed.crouch = false;
        }
        else
          this._grabbed.owner = (Thing) this;
      }
      if (this.localAttachIndex < this.attachIndex)
      {
        for (int index = 0; index < 2; ++index)
          Level.Add((Thing) SmallSmoke.New(p1_1.x + Rando.Float(-1f, 1f), p1_1.y + Rando.Float(-1f, 1f)));
        SFX.Play("grappleHook");
        for (int index = 0; index < 6; ++index)
          Level.Add((Thing) Spark.New(p1_1.x - this.barrelVector.x * 2f + Rando.Float(-1f, 1f), p1_1.y - this.barrelVector.y * 2f + Rando.Float(-1f, 1f), this.barrelVector + new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
        this.localAttachIndex = this.attachIndex;
      }
      if (this.isServerForObject)
      {
        if (this._magnetActive && this._raised && (this.duck != null && !this.duck.grounded) && this._grabbed == null)
          this._keepRaised = true;
        else
          this._keepRaised = false;
        if (this._stuck != null && this.duck != null)
        {
          if ((double) this._stickPos.y < (double) this.owner.position.y - 8.0)
          {
            this.owner.position = this._stickPos + this._stickNormal * 12f;
            this._raised = true;
            this._keepRaised = true;
          }
          else
          {
            this.owner.position = this._stickPos + this._stickNormal * 16f;
            this._raised = false;
            this._keepRaised = false;
          }
          this.owner.hSpeed = this.owner.vSpeed = 0.0f;
          this.duck.moveLock = true;
        }
        else if (this._stuck == null && this.duck != null)
          this.duck.moveLock = false;
        if (this.owner == null && this.prevOwner != null)
        {
          if (this.prevOwner is Duck prevOwner)
            prevOwner.moveLock = false;
          this._prevOwner = (Thing) null;
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      base.Draw();
      this.Draw(this._magnet, new Vec2(5f, (float) ((double) (float) this._wave * (double) this._waveMult - 2.0)));
      if (this._grabbed != null)
      {
        if (this._grabbed is Duck)
        {
          this._grabbed.position = this.Offset(this.barrelOffset + new Vec2(0.0f, -6f)) + this.barrelVector * this._grabbed.halfWidth;
          (this._grabbed as Duck).UpdateSkeleton();
          (this._grabbed as Duck).gripped = true;
          this._grabbed.Draw();
        }
        else
        {
          this._grabbed.position = this.Offset(this.barrelOffset) + this.barrelVector * this._grabbed.halfWidth;
          this._grabbed.Draw();
        }
      }
      foreach (Thing line in this._lines)
        line.Draw();
    }

    public override void OnPressAction()
    {
      this._waveMult = 1f;
      if (!this.raised)
        return;
      this._keepRaised = true;
    }

    public override void OnHoldAction()
    {
    }

    public override void OnReleaseAction() => this._keepRaised = false;

    public override void Fire()
    {
    }
  }
}
