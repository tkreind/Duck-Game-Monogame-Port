// Decompiled with JetBrains decompiler
// Type: DuckGame.MultiMap`2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  /// <summary>Type alias for MultiMap</summary>
  /// <typeparam name="TKey">The type of the key.</typeparam>
  /// <typeparam name="TElement">The type of the element.</typeparam>
  public class MultiMap<TKey, TElement> : MultiMap<TKey, TElement, List<TElement>>
  {
  }
}
