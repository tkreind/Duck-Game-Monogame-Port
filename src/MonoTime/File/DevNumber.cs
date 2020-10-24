// Decompiled with JetBrains decompiler
// Type: DuckGame.DevNumber
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DevNumber
  {
    private static float[] _numbers = new float[8];

    public static float one => DevNumber._numbers[0];

    public static float two => DevNumber._numbers[1];

    public static float three => DevNumber._numbers[2];

    public static float four => DevNumber._numbers[3];

    public static float five => DevNumber._numbers[4];

    public static float six => DevNumber._numbers[5];

    public static float seven => DevNumber._numbers[6];

    public static float eight => DevNumber._numbers[7];

    public static void Initialize()
    {
    }

    public static void Update()
    {
    }
  }
}
