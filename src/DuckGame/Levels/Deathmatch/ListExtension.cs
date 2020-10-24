// Decompiled with JetBrains decompiler
// Type: DuckGame.ListExtension
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  internal static class ListExtension
  {
    public static void Shuffle<T>(this IList<T> list)
    {
      Random random = new Random();
      int count = list.Count;
      while (count > 1)
      {
        --count;
        int index = random.Next(count + 1);
        T obj = list[index];
        list[index] = list[count];
        list[count] = obj;
      }
    }
  }
}
