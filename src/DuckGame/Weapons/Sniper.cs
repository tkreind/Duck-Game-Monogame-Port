﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Sniper
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  public class Sniper : Gun
  {
    public StateBinding _loadStateBinding = new StateBinding(nameof (_loadState));
    public StateBinding _angleOffsetBinding = new StateBinding(nameof (_angleOffset));
    public StateBinding _netLoadBinding = (StateBinding) new NetSoundBinding(nameof (_netLoad));
    public NetSoundEffect _netLoad = new NetSoundEffect(new string[1]
    {
      "loadSniper"
    });
    public int _loadState = -1;
    public int _loadAnimation = -1;
    public float _angleOffset;

    public Sniper(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 3;
      this._ammoType = (AmmoType) new ATSniper();
      this._type = "gun";
      this.graphic = new Sprite("sniper");
      this.center = new Vec2(16f, 4f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(30f, 3f);
      this._fireSound = "sniper";
      this._kickForce = 2f;
      this.laserSight = true;
      this._laserOffsetTL = new Vec2(32f, 3.5f);
      this._manualLoad = true;
    }

    public override void Update()
    {
      base.Update();
      if (this._loadState > -1)
      {
        if (this.owner == null)
        {
          if (this._loadState == 3)
            this.loaded = true;
          this._loadState = -1;
          this._angleOffset = 0.0f;
          this.handOffset = Vec2.Zero;
        }
        if (this._loadState == 0)
        {
          if (Network.isActive)
          {
            if (this.isServerForObject)
              this._netLoad.Play();
          }
          else
            SFX.Play("loadSniper");
          ++this._loadState;
        }
        else if (this._loadState == 1)
        {
          if ((double) this._angleOffset < 0.159999996423721)
            this._angleOffset = MathHelper.Lerp(this._angleOffset, 0.2f, 0.15f);
          else
            ++this._loadState;
        }
        else if (this._loadState == 2)
        {
          this.handOffset.x += 0.4f;
          if ((double) this.handOffset.x > 4.0)
          {
            ++this._loadState;
            this.Reload();
            this.loaded = false;
          }
        }
        else if (this._loadState == 3)
        {
          this.handOffset.x -= 0.4f;
          if ((double) this.handOffset.x <= 0.0)
          {
            ++this._loadState;
            this.handOffset.x = 0.0f;
          }
        }
        else if (this._loadState == 4)
        {
          if ((double) this._angleOffset > 0.0399999991059303)
          {
            this._angleOffset = MathHelper.Lerp(this._angleOffset, 0.0f, 0.15f);
          }
          else
          {
            this._loadState = -1;
            this.loaded = true;
            this._angleOffset = 0.0f;
          }
        }
      }
      if (this.loaded && this.owner != null && this._loadState == -1)
        this.laserSight = true;
      else
        this.laserSight = false;
    }

    public override void OnPressAction()
    {
      if (this.loaded)
      {
        base.OnPressAction();
      }
      else
      {
        if (this.ammo <= 0 || this._loadState != -1)
          return;
        this._loadState = 0;
        this._loadAnimation = 0;
      }
    }

    public override void Draw()
    {
      float angle = this.angle;
      if (this.offDir > (sbyte) 0)
        this.angle -= this._angleOffset;
      else
        this.angle += this._angleOffset;
      base.Draw();
      this.angle = angle;
    }
  }
}
