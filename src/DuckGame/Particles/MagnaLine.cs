// Decompiled with JetBrains decompiler
// Type: DuckGame.MagnaLine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class MagnaLine : Thing
  {
    private Gun _attach;
    private float _length;
    private float _startLength;
    private float _move = 1.570796f;
    public bool show;
    public float dist;
    public float _alphaFade;

    public MagnaLine(float xpos, float ypos, Gun attach, float length, float percent)
      : base(xpos, ypos)
    {
      this._attach = attach;
      this._length = length;
      this._startLength = length;
      this._move = 1.570796f * percent;
      this.alpha = 0.0f;
    }

    public override void Update()
    {
    }

    public override void Draw()
    {
      this._move = Lerp.Float(this._move, 0.0f, 0.04f);
      if ((double) this._move <= 0.00999999977648258)
        this._move += 1.570796f;
      if ((double) this._length > (double) this.dist)
        this.show = false;
      this._alphaFade = Lerp.Float(this._alphaFade, this.show ? 1f : 0.0f, 0.1f);
      this._length = this._startLength * (float) Math.Sin((double) this._move);
      this.alpha = (float) (1.0 - (double) this._length / (double) this._startLength) * this._alphaFade;
      if ((double) this.alpha < 0.00999999977648258)
        return;
      this.position = this._attach.barrelPosition + this._attach.barrelVector * this._length;
      Vec2 vec2 = this._attach.barrelVector.Rotate(Maths.DegToRad(90f), Vec2.Zero);
      Graphics.DrawLine(this.position + vec2 * 7f, this.position - vec2 * 7f, Color.Blue * this.alpha, (float) (1.0 + (1.0 - (double) this._length / (double) this._startLength) * 4.0), new Depth(0.9f));
    }
  }
}
