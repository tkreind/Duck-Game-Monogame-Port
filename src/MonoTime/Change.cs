// Decompiled with JetBrains decompiler
// Type: DuckGame.Change
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Globalization;

namespace DuckGame
{
  public class Change
  {
    public static float ToSingle(object value) => Convert.ToSingle(value, (IFormatProvider) CultureInfo.InvariantCulture);

    public static ulong ToUInt64(object value) => Convert.ToUInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);

    public static int ToInt32(object value) => Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture);

    public static bool ToBoolean(object value) => Convert.ToBoolean(value, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(object value) => Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
