// Decompiled with JetBrains decompiler
// Type: DuckGame.WallHug
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [Flags]
  public enum WallHug
  {
    None = 0,
    Left = 1,
    Right = 2,
    Ceiling = 4,
    Floor = 8,
  }
}
