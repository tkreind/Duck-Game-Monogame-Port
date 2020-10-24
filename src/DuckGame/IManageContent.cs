﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.IManageContent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  /// <summary>
  /// The interface with which a mod provides loadable types to
  /// the main assembly. If your mod does not provide a content manager,
  /// it will use the default content manager.
  /// </summary>
  public interface IManageContent
  {
    /// <summary>Provide a list of types that are subclasses of T.</summary>
    /// <typeparam name="T">The type the game requires subclasses of.</typeparam>
    /// <param name="mod">A reference to this mod's Mod object.</param>
    /// <returns>An enumerable Type list.</returns>
    IEnumerable<System.Type> Compile<T>(Mod mod);
  }
}
