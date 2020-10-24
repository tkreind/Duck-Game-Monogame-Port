﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Trumpet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isFatal", false)]
  [EditorGroup("guns|misc")]
  public class Trumpet : Gun
  {
    public StateBinding _notePitchBinding = new StateBinding(nameof (notePitch));
    public float notePitch;
    private float prevNotePitch;
    private float hitPitch;
    private Sound noteSound;
    private List<InstrumentNote> _notes = new List<InstrumentNote>();
    private int currentPitch = -1;
    private bool leftPressed;
    private bool rightPressed;

    public Trumpet(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 4;
      this._ammoType = (AmmoType) new ATLaser();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._type = "gun";
      this.graphic = new Sprite("trumpet");
      this.center = new Vec2(12f, 5f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(12f, 8f);
      this.depth = new Depth(0.6f);
      this._barrelOffsetTL = new Vec2(24f, 4f);
      this._fireSound = "smg";
      this._fullAuto = true;
      this._fireWait = 1f;
      this._kickForce = 3f;
      this._holdOffset = new Vec2(6f, 2f);
      this.hoverRaise = false;
      this.ignoreHands = true;
      this._notePitchBinding.skipLerp = true;
    }

    public override void Initialize() => base.Initialize();

    public override void Update()
    {
      if (this.owner is Duck owner)
      {
        if (this.isServerForObject)
        {
          if (owner.inputProfile.Pressed("SHOOT"))
            this.currentPitch = 2;
          if (owner.inputProfile.Pressed("STRAFE"))
            this.currentPitch = 0;
          if (owner.inputProfile.Pressed("RAGDOLL"))
            this.currentPitch = 1;
          if ((double) owner.inputProfile.leftTrigger > 0.5 && !this.leftPressed)
          {
            this.currentPitch = 2;
            this.leftPressed = true;
          }
          if ((double) owner.inputProfile.rightTrigger > 0.5 && !this.rightPressed)
          {
            this.currentPitch = 3;
            this.rightPressed = true;
          }
          if (owner.inputProfile.Released("STRAFE") && this.currentPitch == 0)
            this.currentPitch = -1;
          if (owner.inputProfile.Released("SHOOT") && this.currentPitch == 2)
            this.currentPitch = -1;
          if (owner.inputProfile.Released("RAGDOLL") && this.currentPitch == 1)
            this.currentPitch = -1;
          if ((double) owner.inputProfile.leftTrigger <= 0.5)
          {
            if (this.currentPitch == 2 && this.leftPressed)
              this.currentPitch = -1;
            this.leftPressed = false;
          }
          if ((double) owner.inputProfile.rightTrigger <= 0.5)
          {
            if (this.currentPitch == 3 && this.rightPressed)
              this.currentPitch = -1;
            this.rightPressed = false;
          }
          this.notePitch = this.currentPitch < 0 || this._raised ? 0.0f : (float) ((double) this.currentPitch / 3.0 + 0.00999999977648258);
        }
        if ((double) this.notePitch != (double) this.prevNotePitch)
        {
          if ((double) this.notePitch != 0.0)
          {
            if (this.noteSound != null)
            {
              this.noteSound.Stop();
              this.noteSound = (Sound) null;
            }
            int num = (int) Math.Round((double) this.notePitch * 3.0);
            if (num < 0)
              num = 0;
            if (num > 12)
              num = 12;
            if (this.noteSound == null)
            {
              this.hitPitch = this.notePitch;
              this.noteSound = SFX.Play("trumpet0" + Change.ToString((object) (num + 1)), 0.8f);
              Level.Add((Thing) new MusicNote(this.barrelPosition.x, this.barrelPosition.y, this.barrelVector));
            }
            else
              this.noteSound.Pitch = Maths.Clamp((float) (((double) this.notePitch - (double) this.hitPitch) * 0.00999999977648258), -1f, 1f);
          }
          else if (this.noteSound != null)
          {
            this.noteSound.Stop();
            this.noteSound = (Sound) null;
          }
        }
        if (this._raised)
        {
          this.collisionOffset = new Vec2(4f, -4f);
          this.collisionSize = new Vec2(8f, 8f);
          this._holdOffset = new Vec2(0.0f, 0.0f);
          this.handOffset = new Vec2(0.0f, 0.0f);
          this.OnReleaseAction();
        }
        else
        {
          this.collisionOffset = new Vec2(-6f, -4f);
          this.collisionSize = new Vec2(8f, 8f);
          this._holdOffset = new Vec2(10f, -2f);
          this.handOffset = new Vec2(5f, -2f);
        }
      }
      else
      {
        this.leftPressed = false;
        this.rightPressed = false;
        this.currentPitch = -1;
        this.collisionOffset = new Vec2(-6f, -4f);
        this.collisionSize = new Vec2(8f, 8f);
        this._holdOffset = new Vec2(6f, 2f);
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
      if (this.duck != null && !this.raised)
      {
        SpriteMap fingerPositionSprite = this.duck.profile.persona.fingerPositionSprite;
        fingerPositionSprite.frame = this.currentPitch + 1;
        fingerPositionSprite.depth = this.depth - 100;
        fingerPositionSprite.flipH = this.offDir <= (sbyte) 0;
        Vec2 vec2 = this.Offset(new Vec2(-8f, -2f));
        Graphics.Draw((Sprite) fingerPositionSprite, vec2.x, vec2.y);
      }
      base.Draw();
    }
  }
}
