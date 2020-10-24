// Decompiled with JetBrains decompiler
// Type: DuckGame.Depth
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct Depth
  {
    public static float kDepthSpanMax = 0.01f;
    public static float kSpanIncrement = 0.0001f;
    private static float _currentSpan = 0.0f;
    public float value;
    public float span;

    public static void ResetSpan() => Depth._currentSpan = 0.0f;

    public Depth(float val)
    {
      this.value = val;
      this.span = Depth._currentSpan + Depth.kSpanIncrement * 0.5f;
      Depth._currentSpan = (Depth._currentSpan + Depth.kSpanIncrement) % Depth.kDepthSpanMax;
    }

    public Depth(float val, float s)
    {
      this.value = val;
      this.span = s;
    }

    public Depth Add(int val) => new Depth(this.value + Depth.kSpanIncrement / 20f * (float) val, this.span);

    public static implicit operator Depth(float val) => new Depth(val);

    public static Depth operator +(Depth c1, int c2) => c1.Add(c2);

    public static Depth operator -(Depth c1, int c2) => c1.Add(-c2);

    public static bool operator <(Depth c1, float c2) => (double) c1.value < (double) c2;

    public static bool operator >(Depth c1, float c2) => (double) c1.value > (double) c2;
  }
}
