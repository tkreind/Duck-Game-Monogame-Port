﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Trombone
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("guns|misc")]
  [BaggedProperty("isFatal", false)]
  public class Trombone : Gun
  {
    public StateBinding _notePitchBinding = new StateBinding(nameof (notePitch));
    public StateBinding _handPitchBinding = new StateBinding(nameof (handPitch));
    public float notePitch;
    public float handPitch;
    private float prevNotePitch;
    private float hitPitch;
    private Sound noteSound;
    private List<InstrumentNote> _notes = new List<InstrumentNote>();
    private Sprite _slide;
    private float _slideVal;

    public Trombone(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this.graphic = new Sprite("tromboneBody");
      this.center = new Vec2(10f, 16f);
      this.collisionOffset = new Vec2(-4f, -5f);
      this.collisionSize = new Vec2(8f, 11f);
      this._barrelOffsetTL = new Vec2(19f, 14f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._holdOffset = new Vec2(6f, 2f);
      this._slide = new Sprite("tromboneSlide");
      this._slide.CenterOrigin();
      this._notePitchBinding.skipLerp = true;
    }

    public override void Initialize() => base.Initialize();

    public float NormalizePitch(float val) => val;

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
              this.noteSound = SFX.Play("trombone" + Change.ToString((object) num));
              Level.Add((Thing) new MusicNote(this.barrelPosition.x, this.barrelPosition.y, this.barrelVector));
            }
            else
              this.noteSound.Pitch = Maths.Clamp(this.notePitch - this.hitPitch, -1f, 1f);
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
        }
        else
        {
          this.handOffset = new Vec2((float) (6.0 + (1.0 - (double) this.handPitch) * 4.0), (float) ((1.0 - (double) this.handPitch) * 4.0 - 4.0));
          this.handAngle = (float) ((1.0 - (double) this.handPitch) * 0.400000005960464) * (float) this.offDir;
          this._holdOffset = new Vec2((float) (5.0 + (double) this.handPitch * 2.0), (float) ((double) this.handPitch * 2.0 - 9.0));
          this.collisionOffset = new Vec2(-4f, -7f);
          this.collisionSize = new Vec2(2f, 16f);
          this._slideVal = 1f - this.handPitch;
        }
      }
      else
      {
        this.collisionOffset = new Vec2(-4f, -5f);
        this.collisionSize = new Vec2(8f, 11f);
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

    public override void Draw()
    {
      base.Draw();
      this.Draw(this._slide, new Vec2((float) (6.0 + (double) this._slideVal * 8.0), 0.0f), -1);
    }
  }
}
