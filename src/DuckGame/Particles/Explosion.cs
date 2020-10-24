// Decompiled with JetBrains decompiler
// Type: DuckGame.ExplosionPart
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ExplosionPart : Thing
  {
    private bool _created;
    private SpriteMap _sprite;
    private float _wait;
    private int _smokeFrame;
    private bool _smoked;

    public ExplosionPart(float xpos, float ypos, bool doWait = true)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("explosion", 64, 64);
      this._sprite.AddAnimation("explode", 1f, false, 0, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10);
      this._sprite.SetAnimation("explode");
      this.graphic = (Sprite) this._sprite;
      this._sprite.speed = 0.4f + Rando.Float(0.2f);
      this.xscale = 0.5f + Rando.Float(0.5f);
      this.yscale = this.xscale;
      this.center = new Vec2(32f, 32f);
      this._wait = Rando.Float(1f);
      this._smokeFrame = Rando.Int(1, 3);
      this.depth = new Depth(1f);
      this.vSpeed = Rando.Float(-0.2f, -0.4f);
      if (doWait)
        return;
      this._wait = 0.0f;
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
      if (!this._created)
        this._created = true;
      if (this._sprite.frame > this._smokeFrame && !this._smoked)
      {
        int num = Graphics.effectsLevel == 2 ? Rando.Int(1, 4) : 1;
        for (int index = 0; index < num; ++index)
        {
          SmallSmoke smallSmoke = SmallSmoke.New(this.x + Rando.Float(-5f, 5f), this.y + Rando.Float(-5f, 5f));
          smallSmoke.vSpeed = Rando.Float(0.0f, -0.5f);
          smallSmoke.xscale = smallSmoke.yscale = Rando.Float(0.2f, 0.7f);
          Level.Add((Thing) smallSmoke);
        }
        this._smoked = true;
      }
      if ((double) this._wait <= 0.0)
        this.y += this.vSpeed;
      if (!this._sprite.finished)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      if ((double) this._wait > 0.0)
        this._wait -= 0.2f;
      else
        base.Draw();
    }
  }
}
