// Decompiled with JetBrains decompiler
// Type: DuckGame.IContainThings
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  /// <summary>
  /// Defines an object which contains, or may contain, these type of objects.
  /// </summary>
  public interface IContainThings
  {
    IEnumerable<System.Type> contains { get; }
  }
}
