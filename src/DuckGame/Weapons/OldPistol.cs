// Decompiled with JetBrains decompiler
// Type: DuckGame.OldPistol
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("guns")]
  public class OldPistol : Gun
  {
    public StateBinding _loadStateBinding = new StateBinding(nameof (_loadState));
    public StateBinding _netClickBinding = (StateBinding) new NetSoundBinding(nameof (_netClick));
    public StateBinding _netSwipeBinding = (StateBinding) new NetSoundBinding(nameof (_netSwipe));
    public StateBinding _netSwipe2Binding = (StateBinding) new NetSoundBinding(nameof (_netSwipe2));
    public StateBinding _netLoadBinding = (StateBinding) new NetSoundBinding(nameof (_netLoad));
    public NetSoundEffect _netClick = new NetSoundEffect(new string[1]
    {
      "click"
    })
    {
      volume = 1f,
      pitch = 0.5f
    };
    public NetSoundEffect _netSwipe = new NetSoundEffect(new string[1]
    {
      "swipe"
    })
    {
      volume = 0.6f,
      pitch = -0.3f
    };
    public NetSoundEffect _netSwipe2 = new NetSoundEffect(new string[1]
    {
      "swipe"
    })
    {
      volume = 0.7f
    };
    public NetSoundEffect _netLoad = new NetSoundEffect(new string[1]
    {
      "shotgunLoad"
    });
    public int _loadState = -1;
    public float _angleOffset;
    private SpriteMap _sprite;

    public OldPistol(float xval, float yval)
      : base(xval, yval)
    {
      this.ammo = 2;
      this._ammoType = (AmmoType) new ATShrapnel();
      this._ammoType.range = 170f;
      this._ammoType.accuracy = 0.8f;
      this._ammoType.penetration = 0.4f;
      this._type = "gun";
      this._sprite = new SpriteMap("oldPistol", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 17f);
      this.collisionOffset = new Vec2(-8f, -4f);
      this.collisionSize = new Vec2(16f, 8f);
      this._barrelOffsetTL = new Vec2(24f, 16f);
      this._fireSound = "shotgun";
      this._kickForce = 2f;
      this._manualLoad = true;
      this._holdOffset = new Vec2(2f, 0.0f);
    }

    public override void Update()
    {
      base.Update();
      this._sprite.frame = this.ammo <= 1 ? 1 : 0;
      if (this._loadState <= -1)
        return;
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
            this._netSwipe.Play();
        }
        else
          SFX.Play("swipe", 0.6f, -0.3f);
        ++this._loadState;
      }
      else if (this._loadState == 1)
      {
        if ((double) this._angleOffset < 0.159999996423721)
          this._angleOffset = MathHelper.Lerp(this._angleOffset, 0.2f, 0.08f);
        else
          ++this._loadState;
      }
      else if (this._loadState == 2)
      {
        this.handOffset.y -= 0.28f;
        if ((double) this.handOffset.y >= -4.0)
          return;
        ++this._loadState;
        this.ammo = 2;
        this.loaded = false;
        if (Network.isActive)
        {
          if (!this.isServerForObject)
            return;
          this._netLoad.Play();
        }
        else
          SFX.Play("shotgunLoad");
      }
      else if (this._loadState == 3)
      {
        this.handOffset.y += 0.15f;
        if ((double) this.handOffset.y < 0.0)
          return;
        ++this._loadState;
        this.handOffset.y = 0.0f;
        if (Network.isActive)
        {
          if (!this.isServerForObject)
            return;
          this._netSwipe2.Play();
        }
        else
          SFX.Play("swipe", 0.7f);
      }
      else
      {
        if (this._loadState != 4)
          return;
        if ((double) this._angleOffset > 0.0399999991059303)
        {
          this._angleOffset = MathHelper.Lerp(this._angleOffset, 0.0f, 0.08f);
        }
        else
        {
          this._loadState = -1;
          this.loaded = true;
          this._angleOffset = 0.0f;
          if (Network.isActive)
          {
            if (!this.isServerForObject)
              return;
            this._netClick.Play();
          }
          else
            SFX.Play("click", pitch: 0.5f);
        }
      }
    }

    public override void OnPressAction()
    {
      if (this.loaded && this.ammo > 1)
      {
        base.OnPressAction();
        for (int index = 0; index < 4; ++index)
          Level.Add((Thing) Spark.New(this.offDir > (sbyte) 0 ? this.x - 9f : this.x + 9f, this.y - 6f, new Vec2(Rando.Float(-1f, 1f), -0.5f), 0.05f));
        for (int index = 0; index < 4; ++index)
          Level.Add((Thing) SmallSmoke.New(this.barrelPosition.x + (float) this.offDir * 4f, this.barrelPosition.y));
        this.ammo = 1;
      }
      else
      {
        if (this._loadState != -1)
          return;
        this._loadState = 0;
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
