// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamHat
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  public class TeamHat : Hat
  {
    public bool hasBeenStolen;
    private float _timeOpen;
    private int _prevFrame;
    private Sprite _specialSprite;
    private SinWave _wave = (SinWave) 0.1f;
    private float _fade;
    public StateBinding _netTeamIndexBinding = new StateBinding(nameof (netTeamIndex));
    private Team _team;
    private Cape _cape;

    public byte netTeamIndex
    {
      get => this._team == null ? (byte) 0 : (byte) Teams.IndexOf(this._team);
      set
      {
        value = (byte) Maths.Clamp((int) value, 0, Teams.all.Count - 1);
        Team team1 = Teams.all[(int) value];
        Team team2 = this.team;
        this.team = Teams.all[(int) value];
      }
    }

    public Team team
    {
      get => this._team;
      set
      {
        bool flag = this._team != value;
        this._team = value;
        if (this._team == null)
          return;
        this.sprite = this._team.hat.CloneMap();
        this.pickupSprite = this._team.hat.Clone();
        this.sprite.center = new Vec2(16f, 16f);
        this.hatOffset = this._team.hatOffset;
        if (!flag)
          return;
        this.UpdateCape();
      }
    }

    public TeamHat(float xpos, float ypos, Team t)
      : base(xpos, ypos)
      => this.team = t;

    public override void Initialize()
    {
      this.UpdateCape();
      base.Initialize();
    }

    public void UpdateCape()
    {
      if (this._team == null)
        return;
      if (this._cape != null)
        Level.Remove((Thing) this._cape);
      if (this._team.capeTexture != null)
      {
        this._cape = new Cape(this.x, this.y, (PhysicsObject) this);
        this._cape.SetCapeTexture(this._team.capeTexture);
        Level.Add((Thing) this._cape);
      }
      else
      {
        if (!(this._sprite.texture.textureName == "hats/devhat"))
          return;
        this._cape = new Cape(this.x, this.y, (PhysicsObject) this);
        Level.Add((Thing) this._cape);
      }
    }

    public override void Terminate()
    {
      if (this._cape != null)
        Level.Remove((Thing) this._cape);
      base.Terminate();
    }

    public override void Update()
    {
      if (this._equippedDuck != null && !this.destroyed)
      {
        if (this._prevFrame == 0 && this._sprite.frame == 1)
          this.OpenHat();
        else if (this._prevFrame == 1 && this._sprite.frame == 0)
          this.CloseHat();
        if (this._sprite.frame == 1)
          this._timeOpen += 0.1f;
        else
          this._timeOpen = 0.0f;
      }
      this._prevFrame = this._sprite.frame;
      if (this.destroyed)
        this.alpha -= 0.05f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Quack(float volume, float pitch)
    {
      if (this.duck != null && this._sprite.texture.textureName == "hats/hearts")
      {
        SFX.Play("heartfart", volume, Math.Min(pitch + 0.4f - Rando.Float(0.1f), 1f));
        HeartPuff heartPuff = new HeartPuff(this.x, this.y);
        heartPuff.anchor = (Anchor) (Thing) this;
        Level.Add((Thing) heartPuff);
        for (int index = 0; index < 2; ++index)
        {
          SmallSmoke smallSmoke = SmallSmoke.New(this.x, this.y);
          smallSmoke.sprite.color = Color.Green * (0.4f + Rando.Float(0.3f));
          Level.Add((Thing) smallSmoke);
        }
      }
      else
        SFX.Play("quack", volume, pitch);
    }

    public override void OpenHat()
    {
      if (this.duck == null)
        return;
      if (this._sprite.texture.textureName == "hats/burgers")
      {
        FluidData ketchup = Fluid.Ketchup;
        ketchup.amount = Rando.Float(0.0005f, 1f / 1000f);
        int num = Rando.Int(4) + 1;
        for (int index = 0; index < num; ++index)
        {
          Fluid fluid = new Fluid(this.x + (float) this.duck.offDir * (2f + Rando.Float(0.0f, 7f)), this.y + 3f + Rando.Float(0.0f, 3f), new Vec2((float) this.duck.offDir * Rando.Float(0.5f, 3f), Rando.Float(0.0f, -2f)), ketchup, thickMult: 2.5f);
          fluid.depth = this.depth + 1;
          Level.Add((Thing) fluid);
        }
      }
      else if (this._sprite.texture.textureName == "hats/divers" || this._sprite.texture.textureName == "hats/fridge")
      {
        FluidData water = Fluid.Water;
        water.amount = Rando.Float(0.0001f, 0.0005f);
        int num = Rando.Int(3) + 1;
        for (int index = 0; index < num; ++index)
        {
          Fluid fluid = new Fluid(this.x + (float) this.duck.offDir * (2f + Rando.Float(0.0f, 4f)), this.y + 3f + Rando.Float(0.0f, 3f), new Vec2((float) this.duck.offDir * Rando.Float(0.5f, 3f), Rando.Float(0.0f, -2f)), water, thickMult: 5f);
          fluid.depth = this.depth + 1;
          Level.Add((Thing) fluid);
        }
      }
      else if (this._sprite.texture.textureName == "hats/gross")
      {
        FluidData water = Fluid.Water;
        water.amount = Rando.Float(0.0002f, 0.0007f);
        int num = Rando.Int(6) + 2;
        for (int index = 0; index < num; ++index)
        {
          Fluid fluid = new Fluid(this.x + (float) this.duck.offDir * (6f + Rando.Float(-2f, 4f)), this.y + Rando.Float(-2f, 4f), new Vec2((float) this.duck.offDir * Rando.Float(1.2f, 4f), Rando.Float(0.0f, -2.8f)), water, thickMult: 5f);
          fluid.depth = this.depth + 1;
          Level.Add((Thing) fluid);
        }
      }
      else
      {
        if (!(this._sprite.texture.textureName == "hats/tube"))
          return;
        for (int index = 0; index < 4; ++index)
        {
          TinyBubble tinyBubble = new TinyBubble(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(0.0f, 4f), Rando.Float(-1.5f, 1.5f), this.y - 12f, true);
          tinyBubble.depth = this.depth + 1;
          Level.Add((Thing) tinyBubble);
        }
      }
    }

    public override void CloseHat()
    {
      if (this.duck == null)
        return;
      if (this._sprite.texture.textureName == "hats/burgers")
      {
        if ((double) this._timeOpen <= 1.0)
          return;
        FluidData ketchup = Fluid.Ketchup;
        ketchup.amount = Rando.Float(0.0005f, 1f / 1000f);
        int num = Rando.Int(3) + 1;
        for (int index = 0; index < num; ++index)
        {
          Fluid fluid = new Fluid(this.x + (float) this.duck.offDir * (3f + Rando.Float(0.0f, 6f)), this.y + 4f + Rando.Float(0.0f, 1f), new Vec2((float) this.duck.offDir * Rando.Float(-2f, 2f), Rando.Float(-1f, -2f)), ketchup, thickMult: 2.5f);
          fluid.depth = this.depth + 1;
          Level.Add((Thing) fluid);
        }
        SFX.Play("smallSplat", 0.9f, Rando.Float(-0.4f, 0.4f));
      }
      else
      {
        if (!(this._sprite.texture.textureName == "hats/divers") && !(this._sprite.texture.textureName == "hats/fridge") || (double) this._timeOpen <= 2.0)
          return;
        SFX.Play("smallDoorShut", pitch: Rando.Float(-0.1f, 0.1f));
      }
    }

    public override void Draw()
    {
      if (Network.isActive && Level.current is TeamSelect2 && this._team != null)
      {
        if (this.sprite == null)
        {
          this.sprite = this._team.hat.CloneMap();
          this.pickupSprite = this._team.hat.Clone();
          this.sprite.center = new Vec2(16f, 16f);
          this.hatOffset = this._team.hatOffset;
        }
        this.graphic = (Sprite) this.sprite;
      }
      base.Draw();
      if (this.duck == null || !(this._sprite.texture.textureName == "hats/sensei"))
        return;
      if (this._specialSprite == null)
      {
        this._specialSprite = new Sprite("hats/senpaiStar");
        this._specialSprite.CenterOrigin();
      }
      this._fade = Lerp.Float(this._fade, this.frame == 1 ? 1f : 0.0f, 0.1f);
      if ((double) this._fade <= 0.00999999977648258)
        return;
      this._specialSprite.alpha = (float) ((double) this.alpha * 0.699999988079071 * (0.5 + (double) this._wave.normalized * 0.5)) * this._fade;
      this._specialSprite.scale = this.scale;
      this._specialSprite.depth = this.depth - 10;
      this._specialSprite.angle += 0.02f;
      float num = (float) (0.800000011920929 + (double) this._wave.normalized * 0.200000002980232);
      this._specialSprite.scale = new Vec2(num, num);
      Vec2 vec2 = this.Offset(new Vec2(2f, 4f));
      Graphics.Draw(this._specialSprite, vec2.x, vec2.y);
    }
  }
}
