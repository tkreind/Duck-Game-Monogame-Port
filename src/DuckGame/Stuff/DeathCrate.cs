﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DeathCrate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", true)]
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("stuff|props")]
  public class DeathCrate : Holdable, IPlatform
  {
    public StateBinding _settingIndexBinding = new StateBinding(nameof (settingIndex));
    public StateBinding _activatedBinding = new StateBinding(nameof (activated));
    public StateBinding _beepsBinding = new StateBinding(nameof (_beeps));
    private SpriteMap _sprite;
    public byte _beeps;
    private static List<DeathCrateSetting> _settings = new List<DeathCrateSetting>();
    private bool _didActivation;
    public bool activated;
    public byte settingIndex;

    public static void Initialize()
    {
      if (MonoMain.moddingEnabled)
      {
        foreach (System.Type sortedType in ManagedContent.DeathCrateSettings.SortedTypes)
          DeathCrate._settings.Add(Activator.CreateInstance(sortedType) as DeathCrateSetting);
      }
      else
      {
        foreach (System.Type type in (IEnumerable<System.Type>) Editor.GetSubclasses(typeof (DeathCrateSetting)).ToList<System.Type>())
          DeathCrate._settings.Add(Activator.CreateInstance(type) as DeathCrateSetting);
      }
    }

    public DeathCrateSetting setting => (int) this.settingIndex < DeathCrate._settings.Count ? DeathCrate._settings[(int) this.settingIndex] : (DeathCrateSetting) new DCSwordAdventure();

    public DeathCrate(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._maxHealth = 15f;
      this._hitPoints = 15f;
      this._sprite = new SpriteMap("deathcrate", 16, 19);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 11f);
      this.collisionOffset = new Vec2(-8f, -11f);
      this.collisionSize = new Vec2(16f, 18f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Death Crate";
      this.thickness = 2f;
      this.weight = 5f;
      this._sprite.AddAnimation("idle", 1f, true, new int[1]);
      this._sprite.AddAnimation("activate", 0.35f, false, 1, 2, 3, 4, 4, 5, 4, 4, 5, 6, 6, 6, 6, 6, 6, 6, 6, 5, 7, 7, 7, 7, 7, 7, 7, 7, 5, 8, 8, 8, 8, 8, 8, 8, 8, 5, 9, 9, 5);
      this._sprite.SetAnimation("idle");
      this._holdOffset = new Vec2(2f, 0.0f);
      this.flammable = 0.0f;
      this.collideSounds.Add("crateHit");
      this.settingIndex = (byte) Rando.Int(DeathCrate._settings.Count - 1);
    }

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      with.Fondle((Thing) this);
      if (from == ImpactedFrom.Top && (double) with.totalImpactPower + (double) this.totalImpactPower > 0.100000001490116 && this._sprite.currentAnimation == "idle")
      {
        this.activated = true;
        this._sprite.SetAnimation("activate");
        SFX.Play("click");
        this.collisionOffset = new Vec2(-8f, -8f);
        this.collisionSize = new Vec2(16f, 15f);
      }
      base.OnSolidImpact(with, from);
    }

    public override void Terminate()
    {
      if (this.duck != null)
        this.duck.ThrowItem();
      base.Terminate();
    }

    public override void Update()
    {
      if (this.activated && this._sprite.currentAnimation != "activate")
        this._sprite.SetAnimation("activate");
      if (this._sprite.imageIndex == 6 && this._beeps == (byte) 0)
      {
        SFX.Play("singleBeep");
        ++this._beeps;
      }
      if (this._sprite.imageIndex == 7 && this._beeps == (byte) 1)
      {
        SFX.Play("singleBeep");
        ++this._beeps;
      }
      if (this._sprite.imageIndex == 8 && this._beeps == (byte) 2)
      {
        SFX.Play("singleBeep");
        ++this._beeps;
      }
      if (this._sprite.imageIndex == 5 && this._beeps == (byte) 3)
      {
        SFX.Play("doubleBeep", pitch: 0.2f);
        ++this._beeps;
      }
      if (this.isServerForObject && this._sprite.currentAnimation == "activate" && (this._sprite.finished && !this._didActivation))
      {
        this._didActivation = true;
        this.setting.Activate(this);
        Send.Message((NetMessage) new NMActivateDeathCrate(this.settingIndex, this));
      }
      base.Update();
    }

    public override void Draw()
    {
      sbyte offDir = this.offDir;
      this.offDir = (sbyte) 1;
      base.Draw();
      this.offDir = offDir;
    }

    protected override bool OnDestroy(DestroyType type = null) => false;
  }
}
