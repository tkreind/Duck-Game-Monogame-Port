// Decompiled with JetBrains decompiler
// Type: DuckGame.Rando
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public static class Rando
  {
    private static Random _randomGenerator;

    public static Random generator
    {
      get => Rando._randomGenerator;
      set => Rando._randomGenerator = value;
    }

    public static void DoInitialize() => Rando._randomGenerator = new Random();

    public static long Long(long min = -9223372036854775808, long max = 9223372036854775807)
    {
      if (Rando._randomGenerator == null)
        Rando.DoInitialize();
      byte[] buffer = new byte[8];
      Rando._randomGenerator.NextBytes(buffer);
      return Math.Abs(BitConverter.ToInt64(buffer, 0) % (max - min)) + min;
    }

    public static double Double() => Rando._randomGenerator.NextDouble();

    public static float Float(float max) => (float) Rando._randomGenerator.NextDouble() * max;

    public static float Float(float min, float max) => min + (float) Rando._randomGenerator.NextDouble() * (max - min);

    public static int Int(int _max) => Rando._randomGenerator.Next(0, _max + 1);

    public static int Int(int min, int max) => Rando._randomGenerator.Next(min, max + 1);

    public static int ChooseInt(params int[] _ints) => _ints[Rando.Int(_ints.Length - 1)];

    public static float ChooseFloat(params float[] _ints) => _ints[Rando.Int(_ints.Length)];
  }
}
