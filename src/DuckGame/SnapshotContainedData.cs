// Decompiled with JetBrains decompiler
// Type: DuckGame.SnapshotContainedData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [Flags]
  public enum SnapshotContainedData
  {
    None = 0,
    Position = 1,
    Angle = 2,
    Velocity = 4,
    Frame = 8,
    EndOfData = 255, // 0x000000FF
  }
}
