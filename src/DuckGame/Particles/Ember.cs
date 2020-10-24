// Decompiled with JetBrains decompiler
// Type: DuckGame.Ember
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Ember : PhysicsParticle
  {
    private SinWave _wave = new SinWave(0.1f + Rando.Float(0.1f));
    private Color _col;
    private float _initialLife = 1f;

    public Ember(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.vSpeed = (float) -(0.200000002980232 + (double) Rando.Float(0.7f));
      this.hSpeed = Rando.Float(0.4f) - 0.2f;
      this._col = (double) Rando.Float(1f) >= 0.400000005960464 ? ((double) Rando.Float(1f) >= 0.400000005960464 ? Color.Gray : Color.Orange) : Color.Yellow;
      if ((double) Rando.Float(1f) < 0.200000002980232)
        this._initialLife += Rando.Float(10f);
      this.alpha = 0.7f;
    }

    public override void Update()
    {
      this.position.x += this._wave.value * 0.2f;
      this.position.x += this.hSpeed;
      this.position.y += this.vSpeed;
      this._initialLife -= 0.1f;
      if ((double) this._initialLife >= 0.0)
        return;
      this.alpha -= 0.025f;
      if ((double) this.alpha >= 0.0)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw() => Graphics.DrawRect(this.position, this.position + new Vec2(1f, 1f), this._col * this.alpha, this.depth);
  }
}
