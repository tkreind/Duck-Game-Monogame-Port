// Decompiled with JetBrains decompiler
// Type: DuckGame.BinaryClassMember
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct BinaryClassMember
  {
    public string name;
    public object data;
    public bool extra;
    public static Map<System.Type, byte> typeMap = new Map<System.Type, byte>()
    {
      {
        typeof (int),
        (byte) 1
      },
      {
        typeof (uint),
        (byte) 2
      },
      {
        typeof (short),
        (byte) 3
      },
      {
        typeof (ushort),
        (byte) 4
      },
      {
        typeof (sbyte),
        (byte) 5
      },
      {
        typeof (byte),
        (byte) 6
      },
      {
        typeof (double),
        (byte) 7
      },
      {
        typeof (float),
        (byte) 8
      },
      {
        typeof (long),
        (byte) 9
      },
      {
        typeof (ulong),
        (byte) 10
      },
      {
        typeof (string),
        (byte) 11
      },
      {
        typeof (bool),
        (byte) 12
      }
    };
  }
}
