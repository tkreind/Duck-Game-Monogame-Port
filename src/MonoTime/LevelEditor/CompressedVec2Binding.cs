// Decompiled with JetBrains decompiler
// Type: DuckGame.CompressedVec2Binding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class CompressedVec2Binding : StateBinding
  {
    private int _range;

    public override int bits => 32;

    public override System.Type type => typeof (int);

    public override object GetNetValue() => (object) CompressedVec2Binding.GetCompressedVec2((Vec2) this.value, this._range);

    public static int GetCompressedVec2(Vec2 val, int range = 2147483647)
    {
      if (range != int.MaxValue)
      {
        float num = (float) ((int) short.MaxValue / range);
        val.x = Maths.Clamp(val.x, (float) -range, (float) range) * num;
        val.y = Maths.Clamp(val.y, (float) -range, (float) range) * num;
      }
      return (int) ((long) (ushort) Maths.Clamp((int) Math.Round((double) val.x), (int) short.MinValue, (int) short.MaxValue) << 16 | (long) (ushort) Maths.Clamp((int) Math.Round((double) val.y), (int) short.MinValue, (int) short.MaxValue));
    }

    public override object ReadNetValue(object val) => (object) CompressedVec2Binding.GetUncompressedVec2((int) val, this._range);

    public static Vec2 GetUncompressedVec2(int val, int range = 2147483647)
    {
      int num1 = val;
      short num2 = (short) (num1 & (int) ushort.MaxValue);
      Vec2 vec2 = new Vec2((float) (short) (num1 >> 16 & (int) ushort.MaxValue), (float) num2);
      if (range != int.MaxValue)
      {
        float num3 = (float) ((int) short.MaxValue / range);
        vec2.x /= num3;
        vec2.y /= num3;
      }
      return vec2;
    }

    public CompressedVec2Binding(string field, int range = 2147483647, bool isvelocity = false, bool doLerp = false)
      : base(field, -1, false, isvelocity)
    {
      this._range = range;
      this._lerp = doLerp;
    }

    public CompressedVec2Binding(
      GhostPriority p,
      string field,
      int range = 2147483647,
      bool isvelocity = false,
      bool doLerp = false)
      : base(field, -1, false, isvelocity)
    {
      this._range = range;
      this._priority = p;
      this._lerp = doLerp;
    }

    public CompressedVec2Binding(string field, int range, bool doLerp)
      : base(field)
    {
      this._range = range;
      this._lerp = doLerp;
    }

    public CompressedVec2Binding(string field, int range)
      : base(field)
      => this._range = range;
  }
}
