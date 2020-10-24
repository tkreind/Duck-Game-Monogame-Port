// Decompiled with JetBrains decompiler
// Type: DuckGame.BeamParticle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class BeamParticle : Thing
  {
    public float _wave;
    public float _sinVal;
    private bool _inverse;
    private float _size;
    private Color _color;

    public BeamParticle(float xpos, float ypos, float spd, bool inverse, Color c)
      : base(xpos, ypos)
    {
      this.depth = (Depth) 0.9f;
      this.vSpeed = Rando.Float(-0.5f, -1.5f);
      this.y += Rando.Float(10f);
      this._inverse = inverse;
      this._size = 0.5f + Rando.Float(0.8f);
      this._color = c;
    }

    public override void Update()
    {
      this._wave += 0.1f;
      this._sinVal = (float) Math.Sin((double) this._wave);
      this.y += this.vSpeed;
      if ((double) this._sinVal < -0.800000011920929 && this.depth > 0.0f)
        this.depth = (Depth) -0.8f;
      else if ((double) this._sinVal > 0.800000011920929 && this.depth < 0.0f)
        this.depth = (Depth) 0.8f;
      if ((double) this.y < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void Draw()
    {
      Vec2 vec2 = this.position + new Vec2((float) (16.0 * (double) this._sinVal * (this._inverse ? -1.0 : 1.0)), 0.0f);
      Graphics.DrawRect(vec2 - new Vec2(this._size, this._size), vec2 + new Vec2(this._size, this._size), this._color * 0.4f, this.depth);
      base.Draw();
    }
  }
}
