// Decompiled with JetBrains decompiler
// Type: DuckGame.NetRand
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public static class NetRand
  {
    private static Random _randomGenerator;
    public static int currentSeed;

    public static Random generator
    {
      get => NetRand._randomGenerator;
      set => NetRand._randomGenerator = value;
    }

    public static void Initialize(int seed)
    {
      NetRand.currentSeed = seed;
      NetRand._randomGenerator = new Random(seed);
    }

    public static void Initialize()
    {
      NetRand._randomGenerator = new Random();
      NetRand.currentSeed = Rando.Int(2147483646);
    }

    public static double Double() => NetRand._randomGenerator.NextDouble();

    public static float Float(float max) => (float) NetRand._randomGenerator.NextDouble() * max;

    public static float Float(float min, float max) => min + (float) NetRand._randomGenerator.NextDouble() * (max - min);

    public static int Int(int _max) => NetRand._randomGenerator.Next(0, _max + 1);

    public static int Int(int min, int max) => NetRand._randomGenerator.Next(min, max + 1);

    public static int ChooseInt(params int[] _ints) => _ints[Rando.Int(_ints.Length - 1)];

    public static float ChooseFloat(params float[] _ints) => _ints[Rando.Int(_ints.Length)];
  }
}
