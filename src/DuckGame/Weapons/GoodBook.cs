// Decompiled with JetBrains decompiler
// Type: DuckGame.GoodBook
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("guns|explosives")]
  [BaggedProperty("isFatal", false)]
  public class GoodBook : Gun
  {
    public StateBinding _raiseArmBinding = new StateBinding(nameof (_raiseArm));
    public StateBinding _timerBinding = new StateBinding(nameof (_timer));
    public StateBinding _netPreachBinding = (StateBinding) new NetSoundBinding(nameof (_netPreach));
    public StateBinding _ringPulseBinding = new StateBinding(nameof (_ringPulse));
    public StateBinding _controlStateBinding = (StateBinding) new StateFlagBinding(new string[4]
    {
      nameof (controlling1),
      nameof (controlling2),
      nameof (controlling3),
      nameof (controlling4)
    });
    public bool controlling1;
    public bool controlling2;
    public bool controlling3;
    public bool controlling4;
    public bool prevControlling1;
    public bool prevControlling2;
    public bool prevControlling3;
    public bool prevControlling4;
    public NetSoundEffect _netPreach = new NetSoundEffect(new string[6]
    {
      "preach0",
      "preach1",
      "preach2",
      "preach3",
      "preach4",
      "preach5"
    })
    {
      pitchVariationLow = -0.3f,
      pitchVariationHigh = -0.2f
    };
    private SpriteMap _sprite;
    public float _timer = 1.2f;
    public float _raiseArm;
    private Sprite _halo;
    private float _preachWait;
    private float _haloAlpha;
    private SinWave _haloWave = (SinWave) 0.05f;
    public float _ringPulse;

    public GoodBook(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 1;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._ammoType.penetration = 0.4f;
      this._ammoType.range = 40f;
      this._type = "gun";
      this._sprite = new SpriteMap("goodBook", 17, 12);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 6f);
      this.collisionOffset = new Vec2(-5f, -4f);
      this.collisionSize = new Vec2(10f, 8f);
      this._halo = new Sprite("halo");
      this._halo.CenterOrigin();
      this._holdOffset = new Vec2(3f, 4f);
      this.handOffset = new Vec2(1f, 1f);
      this._hasTrigger = false;
      this.bouncy = 0.4f;
      this.friction = 0.05f;
      this.flammable = 1f;
      this._editorName = "Good Book";
      this._bio = "A collection of words, maybe other ducks should hear them?";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this._netPreach.function = new NetSoundEffect.Function(this.DoPreach);
    }

    public void DoPreach()
    {
    }

    public Duck GetDuck(int index)
    {
      foreach (Duck duck in Level.current.things[typeof (Duck)])
      {
        if (duck.profile != null && (int) duck.profile.networkIndex == index)
          return duck;
      }
      return (Duck) null;
    }

    public override void Update()
    {
      base.Update();
      this._sprite.frame = this._owner == null || this._raised ? 0 : 1;
      this._raiseArm = Lerp.Float(this._raiseArm, 0.0f, 0.05f);
      this._preachWait = Lerp.Float(this._preachWait, 0.0f, 0.06f);
      this._ringPulse = Lerp.Float(this._ringPulse, 0.0f, 0.05f);
      if (Network.isActive)
      {
        if (this.isServerForObject)
        {
          if (this.controlling1)
          {
            Duck duck = this.GetDuck(0);
            if (duck != null)
            {
              if (duck.listenTime <= 0)
              {
                this.controlling1 = false;
              }
              else
              {
                this.Fondle((Thing) duck);
                this.Fondle((Thing) duck.holdObject);
                foreach (Thing t in duck._equipment)
                  this.Fondle(t);
                this.Fondle((Thing) duck._ragdollInstance);
                this.Fondle((Thing) duck._trappedInstance);
                this.Fondle((Thing) duck._cookedInstance);
              }
            }
          }
          if (this.controlling2)
          {
            Duck duck = this.GetDuck(1);
            if (duck != null)
            {
              if (duck.listenTime <= 0)
              {
                this.controlling2 = false;
              }
              else
              {
                this.Fondle((Thing) duck);
                this.Fondle((Thing) duck.holdObject);
                foreach (Thing t in duck._equipment)
                  this.Fondle(t);
                this.Fondle((Thing) duck._ragdollInstance);
                this.Fondle((Thing) duck._trappedInstance);
                this.Fondle((Thing) duck._cookedInstance);
              }
            }
          }
          if (this.controlling3)
          {
            Duck duck = this.GetDuck(2);
            if (duck != null)
            {
              if (duck.listenTime <= 0)
              {
                this.controlling3 = false;
              }
              else
              {
                this.Fondle((Thing) duck);
                this.Fondle((Thing) duck.holdObject);
                foreach (Thing t in duck._equipment)
                  this.Fondle(t);
                this.Fondle((Thing) duck._ragdollInstance);
                this.Fondle((Thing) duck._trappedInstance);
                this.Fondle((Thing) duck._cookedInstance);
              }
            }
          }
          if (this.controlling4)
          {
            Duck duck = this.GetDuck(3);
            if (duck != null)
            {
              if (duck.listenTime <= 0)
              {
                this.controlling4 = false;
              }
              else
              {
                this.Fondle((Thing) duck);
                this.Fondle((Thing) duck.holdObject);
                foreach (Thing t in duck._equipment)
                  this.Fondle(t);
                this.Fondle((Thing) duck._ragdollInstance);
                this.Fondle((Thing) duck._trappedInstance);
                this.Fondle((Thing) duck._cookedInstance);
              }
            }
          }
        }
        else
        {
          Duck duck1 = this.GetDuck(0);
          if (duck1 != null)
          {
            if (this.controlling1)
            {
              duck1.listening = true;
              duck1.Fondle((Thing) duck1.holdObject);
              foreach (Equipment equipment in duck1._equipment)
                duck1.Fondle((Thing) equipment);
              duck1.Fondle((Thing) duck1._ragdollInstance);
              duck1.Fondle((Thing) duck1._trappedInstance);
              duck1.Fondle((Thing) duck1._cookedInstance);
            }
            if (!this.controlling1 && this.prevControlling1)
              duck1.listening = false;
            this.prevControlling1 = this.controlling1;
          }
          Duck duck2 = this.GetDuck(1);
          if (duck2 != null)
          {
            if (this.controlling2)
            {
              duck2.listening = true;
              duck2.Fondle((Thing) duck2.holdObject);
              foreach (Equipment equipment in duck2._equipment)
                duck2.Fondle((Thing) equipment);
              duck2.Fondle((Thing) duck2._ragdollInstance);
              duck2.Fondle((Thing) duck2._trappedInstance);
              duck2.Fondle((Thing) duck2._cookedInstance);
            }
            if (!this.controlling2 && this.prevControlling2)
              duck2.listening = false;
            this.prevControlling2 = this.controlling2;
          }
          Duck duck3 = this.GetDuck(2);
          if (duck3 != null)
          {
            if (this.controlling3)
            {
              duck3.listening = true;
              duck3.Fondle((Thing) duck3.holdObject);
              foreach (Equipment equipment in duck3._equipment)
                duck3.Fondle((Thing) equipment);
              duck3.Fondle((Thing) duck3._ragdollInstance);
              duck3.Fondle((Thing) duck3._trappedInstance);
              duck3.Fondle((Thing) duck3._cookedInstance);
            }
            if (!this.controlling3 && this.prevControlling3)
              duck3.listening = false;
            this.prevControlling3 = this.controlling3;
          }
          Duck duck4 = this.GetDuck(3);
          if (duck4 != null)
          {
            if (this.controlling4)
            {
              duck4.listening = true;
              duck4.Fondle((Thing) duck4.holdObject);
              foreach (Equipment equipment in duck4._equipment)
                duck4.Fondle((Thing) equipment);
              duck4.Fondle((Thing) duck4._ragdollInstance);
              duck4.Fondle((Thing) duck4._trappedInstance);
              duck4.Fondle((Thing) duck4._cookedInstance);
            }
            if (!this.controlling4 && this.prevControlling4)
              duck4.listening = false;
            this.prevControlling4 = this.controlling4;
          }
        }
      }
      if (this._triggerHeld && this.isServerForObject && (this.duck != null && (double) this._preachWait <= 0.0 & this.duck.quack < 1) && this.duck.grounded)
      {
        if (Network.isActive)
          this._netPreach.Play();
        else
          SFX.Play("preach" + (object) Rando.Int(5), Rando.Float(0.8f, 1f), Rando.Float(-0.2f, -0.3f));
        this.duck.quack = (int) (byte) Rando.Int(12, 30);
        this.duck.profile.stats.timePreaching += (float) this.duck.quack / 0.1f * Maths.IncFrameTimer();
        this._preachWait = Rando.Float(1.8f, 2.5f);
        this._ringPulse = 1f;
        if (Rando.Int(1) == 0)
          this._raiseArm = Rando.Float(1.2f, 2f);
        Ragdoll ragdoll = Level.Nearest<Ragdoll>(this.x, this.y, (Thing) this);
        if (ragdoll != null && ragdoll.captureDuck != null && (ragdoll.captureDuck.dead && Level.CheckLine<Block>(this.duck.position, ragdoll.position) == null) && (double) (ragdoll.position - this.duck.position).length < (double) this._ammoType.range)
        {
          if (Network.isActive)
          {
            this.Fondle((Thing) ragdoll.captureDuck);
            this.Fondle((Thing) ragdoll);
            Send.Message((NetMessage) new NMLayToRest(ragdoll.captureDuck.profile.networkIndex));
          }
          ragdoll.captureDuck.LayToRest(this.duck.profile);
        }
        foreach (Duck duck in Level.current.things[typeof (Duck)])
        {
          if (duck != this.duck && duck.grounded && (!(duck.holdObject is GoodBook) && Level.CheckLine<Block>(this.duck.position, duck.position) == null) && (double) (duck.position - this.duck.position).length < (double) this._ammoType.range)
          {
            if (duck.dead)
            {
              this.Fondle((Thing) duck);
              duck.LayToRest(this.duck.profile);
            }
            else if (duck.converted != this.duck && this.duck.converted != duck && duck.profile.team != this.duck.profile.team)
            {
              if (Network.isActive)
              {
                if (duck.profile.networkIndex == (byte) 0)
                  this.controlling1 = true;
                if (duck.profile.networkIndex == (byte) 1)
                  this.controlling2 = true;
                if (duck.profile.networkIndex == (byte) 2)
                  this.controlling3 = true;
                if (duck.profile.networkIndex == (byte) 3)
                  this.controlling4 = true;
              }
              duck.listening = true;
              this.Fondle((Thing) duck);
              this.Fondle((Thing) duck.holdObject);
              foreach (Thing t in duck._equipment)
                this.Fondle(t);
              this.Fondle((Thing) duck._ragdollInstance);
              this.Fondle((Thing) duck._trappedInstance);
              this.Fondle((Thing) duck._cookedInstance);
              duck.listenTime = 80;
              if ((double) this.owner.x < (double) duck.x)
                duck.offDir = (sbyte) -1;
              else
                duck.offDir = (sbyte) 1;
              duck.ThrowItem(false);
              duck.conversionResistance -= 30;
              if (duck.conversionResistance <= 0)
              {
                duck.ConvertDuck(this.duck.converted != null ? this.duck.converted : this.duck);
                if (Network.isActive)
                  Send.Message((NetMessage) new NMConversion(duck.profile.networkIndex, this.duck.profile.networkIndex));
                duck.conversionResistance = 50;
              }
            }
          }
        }
      }
      this._haloAlpha = Lerp.Float(this._haloAlpha, !this._triggerHeld || this.duck == null || !this.duck.grounded ? 0.0f : 1f, 0.05f);
    }

    public override void OnPressAction()
    {
    }

    public override void Draw()
    {
      if (this.duck != null && !this._raised && (double) this._raiseArm > 0.0)
      {
        SpriteMap spriteArms = this.duck._spriteArms;
        bool flipH = spriteArms.flipH;
        float angle = spriteArms.angle;
        spriteArms.flipH = (int) this.offDir * -1 < 0;
        spriteArms.angle = 0.7f * (float) this.offDir;
        Graphics.Draw((Sprite) spriteArms, this.owner.x - (float) (5 * (int) this.offDir), (float) ((double) this.owner.y + 3.0 + (this.duck.crouch ? 3.0 : 0.0) + (this.duck.sliding ? 3.0 : 0.0)));
        spriteArms.angle = angle;
        spriteArms.flipH = flipH;
        this.handOffset = new Vec2(9999f, 9999f);
      }
      else
        this.handOffset = new Vec2(1f, 1f);
      if (this.owner != null && (double) this._haloAlpha > 0.00999999977648258)
      {
        this._halo.alpha = (float) ((double) this._haloAlpha * 0.400000005960464 + (double) (float) this._haloWave * 0.100000001490116);
        this._halo.depth = new Depth(-0.2f);
        this._halo.xscale = this._halo.yscale = (float) (0.949999988079071 + (double) (float) this._haloWave * 0.0500000007450581);
        this._halo.angle += 0.01f;
        Graphics.Draw(this._halo, this.owner.x, this.owner.y);
        if ((double) this._ringPulse > 0.0)
        {
          int num1 = 16;
          Vec2 vec2_1 = Vec2.Zero;
          float num2 = (float) ((double) this._ammoType.range * 0.100000001490116 + (1.0 - (double) this._ringPulse) * ((double) this._ammoType.range * 0.899999976158142));
          for (int index = 0; index < num1; ++index)
          {
            float rad = Maths.DegToRad((float) (360 / (num1 - 1) * index));
            Vec2 vec2_2 = new Vec2((float) Math.Cos((double) rad) * num2, (float) -Math.Sin((double) rad) * num2);
            if (index > 0)
              Graphics.DrawLine(this.owner.position + vec2_2, this.owner.position + vec2_1, Color.White * (this._ringPulse * 0.6f), this._ringPulse * 10f);
            vec2_1 = vec2_2;
          }
        }
      }
      base.Draw();
    }
  }
}
