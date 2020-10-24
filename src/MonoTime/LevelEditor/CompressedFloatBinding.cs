// Decompiled with JetBrains decompiler
// Type: DuckGame.CompressedFloatBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class CompressedFloatBinding : StateBinding
  {
    private float _range = 1f;

    public override System.Type type => typeof (int);

    public override object GetNetValue()
    {
      int num1 = (int) BitBuffer.GetMaxValue(this._bits) / 2;
      float val = (float) this.value;
      float num2;
      if (this.isRotation)
      {
        if ((double) val < 0.0)
        {
          double num3 = (double) this._range / 2.0;
          val = val % -this._range + this._range;
        }
        num2 = val % this._range / this._range;
      }
      else
        num2 = Maths.Clamp(val, -this._range, this._range) / this._range;
      return (object) (int) Math.Round((double) num2 * (double) num1);
    }

    public override object ReadNetValue(object val) => (object) (float) ((double) ((float) (int) val / (float) (BitBuffer.GetMaxValue(this._bits) / 2L)) * (double) this._range);

    public CompressedFloatBinding(string field, float range = 1f, int bits = 16, bool isRot = false, bool doLerp = false)
      : base(field, bits, isRot)
    {
      this._range = range;
      if (isRot)
        this._range = 6.283185f;
      this._lerp = doLerp;
    }

    public CompressedFloatBinding(string field, float range, int bits, bool isRot)
      : base(field, bits, isRot)
    {
      this._range = range;
      if (!isRot)
        return;
      this._range = 6.283185f;
    }

    public CompressedFloatBinding(
      GhostPriority p,
      string field,
      float range = 1f,
      int bits = 16,
      bool isRot = false,
      bool doLerp = false)
      : base(field, bits, isRot)
    {
      this._range = range;
      if (isRot)
        this._range = 6.283185f;
      this._priority = p;
      this._lerp = doLerp;
    }
  }
}
