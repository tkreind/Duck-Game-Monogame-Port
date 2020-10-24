﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Saxaphone
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isFatal", false)]
  [EditorGroup("guns|misc")]
  public class Saxaphone : Gun
  {
    public StateBinding _notePitchBinding = new StateBinding(nameof (notePitch));
    public StateBinding _handPitchBinding = new StateBinding(nameof (handPitch));
    public float notePitch;
    public float handPitch;
    private float prevNotePitch;
    private float hitPitch;
    private Sound noteSound;
    private List<InstrumentNote> _notes = new List<InstrumentNote>();

    public Saxaphone(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this.graphic = new Sprite("saxaphone");
      this.center = new Vec2(20f, 18f);
      this.collisionOffset = new Vec2(-4f, -7f);
      this.collisionSize = new Vec2(8f, 16f);
      this.depth = (Depth) 0.6f;
      this._barrelOffsetTL = new Vec2(24f, 16f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._holdOffset = new Vec2(6f, 2f);
      this._notePitchBinding.skipLerp = true;
    }

    public override void Initialize() => base.Initialize();

    public override void Update()
    {
      if (this.owner is Duck owner)
      {
        if (this.isServerForObject)
        {
          this.handPitch = owner.inputProfile.leftTrigger;
          this.notePitch = !owner.inputProfile.Down("SHOOT") ? 0.0f : this.handPitch + 0.01f;
        }
        if ((double) this.notePitch != (double) this.prevNotePitch)
        {
          if ((double) this.notePitch != 0.0)
          {
            int num = (int) Math.Round((double) this.notePitch * 12.0);
            if (num < 0)
              num = 0;
            if (num > 12)
              num = 12;
            if (this.noteSound == null)
            {
              this.hitPitch = this.notePitch;
              this.noteSound = SFX.Play("sax" + Change.ToString((object) num));
              Level.Add((Thing) new MusicNote(this.barrelPosition.x, this.barrelPosition.y, this.barrelVector));
            }
            else
              this.noteSound.Pitch = Maths.Clamp((float) (((double) this.notePitch - (double) this.hitPitch) * 0.100000001490116), -1f, 1f);
          }
          else if (this.noteSound != null)
          {
            this.noteSound.Stop();
            this.noteSound = (Sound) null;
          }
        }
        if (this._raised)
        {
          this.handAngle = 0.0f;
          this.handOffset = new Vec2(0.0f, 0.0f);
          this._holdOffset = new Vec2(0.0f, 2f);
          this.collisionOffset = new Vec2(-4f, -7f);
          this.collisionSize = new Vec2(8f, 16f);
          this.OnReleaseAction();
        }
        else
        {
          this.handOffset = new Vec2((float) (5.0 + (1.0 - (double) this.handPitch) * 2.0), (float) ((1.0 - (double) this.handPitch) * 4.0 - 2.0));
          this.handAngle = (float) ((1.0 - (double) this.handPitch) * 0.400000005960464) * (float) this.offDir;
          this._holdOffset = new Vec2((float) (4.0 + (double) this.handPitch * 2.0), this.handPitch * 2f);
          this.collisionOffset = new Vec2(-1f, -7f);
          this.collisionSize = new Vec2(2f, 16f);
        }
      }
      else
      {
        this.collisionOffset = new Vec2(-4f, -7f);
        this.collisionSize = new Vec2(8f, 16f);
      }
      this.prevNotePitch = this.notePitch;
      base.Update();
    }

    public override void OnPressAction()
    {
    }

    public override void OnReleaseAction()
    {
    }

    public override void Fire()
    {
    }
  }
}
