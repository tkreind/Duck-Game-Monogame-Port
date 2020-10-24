// Decompiled with JetBrains decompiler
// Type: DuckGame.IFilterLSItems
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  /// <summary>
  /// Represents an interface for filtering level select items from the list based on
  /// conditions.
  /// </summary>
  public interface IFilterLSItems
  {
    /// <summary>Filters the specified level.</summary>
    /// <param name="level">The level.</param>
    /// <param name="location">The location.</param>
    /// <returns>true to keep, false to remove</returns>
    bool Filter(string level, LevelLocation location = LevelLocation.Any);
  }
}
