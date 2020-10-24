// Decompiled with JetBrains decompiler
// Type: DuckGame.TV
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  public class TV : Holdable, IPlatform
  {
    private SpriteMap _sprite;
    private Sprite _frame;
    private Sprite _damaged;
    public bool _ruined;
    public StateBinding _ruinedBinding = new StateBinding(nameof (_ruined));
    public StateBinding _channelBinding = new StateBinding(nameof (channel));
    private float _ghostWait = 1f;
    private bool _madeGhost;
    public bool channel;
    private int _switchFrames;
    private Sprite _rainbow;
    private Cape _cape;
    private List<Vec2> trail = new List<Vec2>();
    private SpriteMap _channels;
    private SpriteMap _tvNoise;
    private int wait;

    public TV(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("plasma2", 16, 16);
      this._sprite.speed = 0.2f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -7f);
      this.collisionSize = new Vec2(16f, 14f);
      this.depth = (Depth) -0.5f;
      this._editorName = nameof (TV);
      this.thickness = 2f;
      this.weight = 5f;
      this.flammable = 0.3f;
      this._frame = new Sprite("tv2");
      this._frame.CenterOrigin();
      this._damaged = new Sprite("tvBroken");
      this._damaged.CenterOrigin();
      this._holdOffset = new Vec2(2f, 0.0f);
      this._breakForce = 4f;
      this.collideSounds.Add("landTV");
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._channels = new SpriteMap("channels", 8, 6);
      this._channels.depth = this.depth + 5;
      this._tvNoise = new SpriteMap("tvNoise", 8, 6);
      this._tvNoise.AddAnimation("noise", 0.6f, true, 0, 1, 2);
      this._tvNoise.currentAnimation = "noise";
      this._rainbow = new Sprite("rainbowGradient");
    }

    public override void Initialize()
    {
      this._cape = new Cape(this.x, this.y, (PhysicsObject) this, true);
      this._cape._capeTexture = new Sprite("rainbowCarp").texture;
      Level.Add((Thing) this._cape);
      base.Initialize();
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (type.thing != null && type.thing is Duck || this._ruined)
        return false;
      this._ruined = true;
      this.graphic = this._damaged;
      SFX.Play("breakTV");
      for (int index = 0; index < 8; ++index)
        Level.Add((Thing) new GlassParticle(this.x + Rando.Float(-8f, 8f), this.y + Rando.Float(-8f, 8f), new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
      this.collideSounds.Clear();
      this.collideSounds.Add("deadTVLand");
      return false;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (this._ruined || !bullet.isLocal)
        return base.Hit(bullet, hitPos);
      this.OnDestroy((DestroyType) new DTShot(bullet));
      return base.Hit(bullet, hitPos);
    }

    public override void Update()
    {
      if (this._switchFrames > 0)
        --this._switchFrames;
      if (this._ruined)
      {
        if (this._cape != null)
        {
          Level.Remove((Thing) this._cape);
          this._cape = (Cape) null;
        }
        this.graphic = this._damaged;
        if ((double) this._ghostWait > 0.0)
        {
          this._ghostWait -= 0.4f;
        }
        else
        {
          if (!this._madeGhost)
          {
            Level.Add((Thing) new EscapingGhost(this.x, this.y - 6f));
            for (int index = 0; index < 8; ++index)
              Level.Add((Thing) Spark.New(this.x + Rando.Float(-8f, 8f), this.y + Rando.Float(-8f, 8f), new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
          }
          this._madeGhost = true;
        }
      }
      base.Update();
    }

    public override void OnPressAction()
    {
      if (!this._ruined)
      {
        this.channel = !this.channel;
        this._switchFrames = 8;
        SFX.Play("switchchannel", 0.7f, 0.5f);
      }
      base.OnPressAction();
    }

    public override void Draw()
    {
      base.Draw();
      if (this._ruined)
        return;
      this._frame.depth = this.depth + 1;
      Graphics.Draw(this._frame, this.x, this.y);
      this._channels.alpha = Lerp.Float(this._channels.alpha, this.owner != null ? 1f : 0.0f, 0.1f);
      this._channels.depth = this.depth + 4;
      this._channels.frame = this.channel ? 1 : 0;
      Graphics.Draw((Sprite) this._channels, this.x - 4f, this.y - 4f);
      if (this.owner != null)
      {
        Vec2 p1 = Vec2.Zero;
        bool flag = false;
        foreach (Vec2 p2 in this.trail)
        {
          if (!flag)
            flag = true;
          else
            Graphics.DrawTexturedLine(this._rainbow.texture, p1, p2, Color.White, depth: ((Depth) 0.1f));
          p1 = p2;
        }
      }
      if (this._switchFrames > 0)
        this._tvNoise.alpha = 1f;
      else
        this._tvNoise.alpha = 0.2f;
      this._tvNoise.depth = this.depth + 8;
      Graphics.Draw((Sprite) this._tvNoise, this.x - 4f, this.y - 4f);
    }
  }
}
