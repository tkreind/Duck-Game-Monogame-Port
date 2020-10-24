// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsMaterial
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  /// <summary>
  /// Represents a material type given to physics objects, and changes
  /// how they interact with the world (metal objects become too hot to hold under heat,
  /// paper/wood burns, etc).
  /// </summary>
  public enum PhysicsMaterial
  {
    Default = 0,
    Metal = 1,
    Rubber = 2,
    Wood = 3,
    Paper = 4,
    Crust = 5,
    Plastic = 6,
    Duck = 7,
    Reserved = 255, // 0x000000FF
  }
}
