// Decompiled with JetBrains decompiler
// Type: DuckGame.ICloneable`1
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// Represents an object that can be cloned into a specific type.
  /// </summary>
  /// <typeparam name="T">The type it can be cloned into.</typeparam>
  public interface ICloneable<T> : ICloneable
  {
    /// <summary>Clones this instance.</summary>
    /// <returns>The new instance.</returns>
    T Clone();
  }
}
