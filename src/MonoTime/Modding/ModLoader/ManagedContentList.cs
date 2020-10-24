﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ManagedContentList`1
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  /// <summary>Represents a list of mod-managed content of T's</summary>
  /// <typeparam name="T">The base type of content to store</typeparam>
  public class ManagedContentList<T>
  {
    private readonly HashSet<System.Type> _types = new HashSet<System.Type>();
    private readonly Dictionary<System.Type, System.Type> _redirections = new Dictionary<System.Type, System.Type>();

    internal void Add(System.Type type) => this._types.Add(type);

    internal IEnumerable<System.Type> SortedTypes => (IEnumerable<System.Type>) this._types.OrderBy<System.Type, string>((Func<System.Type, string>) (t => t.FullName));

    /// <summary>Gets the registered types.</summary>
    /// <value>The types registered.</value>
    public IEnumerable<System.Type> Types => (IEnumerable<System.Type>) this._types;

    /// <summary>Removes a type from the type pool.</summary>
    /// <param name="type">The type.</param>
    public void Remove(System.Type type)
    {
      if (!this._types.Contains(type) || type.GetCustomAttributes(typeof (LockedContentAttribute), true).Length != 0)
        return;
      this._types.Remove(type);
    }

    /// <summary>Removes a generic type from the type pool.</summary>
    /// <typeparam name="E">The type to remove</typeparam>
    public void Remove<E>() where E : T => this.Remove(typeof (E));

    /// <summary>
    /// Redirects the a type to another type. Attempts to create an Old
    /// will result in a New being created instead.
    /// </summary>
    /// <param name="oldType">Old type, being redirected.</param>
    /// <param name="newType">The new type to redirect to.</param>
    public void Redirect(System.Type oldType, System.Type newType)
    {
      if (oldType.GetCustomAttributes(typeof (LockedContentAttribute), true).Length != 0)
        return;
      this._redirections[oldType] = newType;
    }

    /// <summary>
    /// Redirects the generic Old type to the New type. Attempts to create an Old
    /// will result in a New being created instead.
    /// </summary>
    /// <typeparam name="Old">Old type, being redirected.</typeparam>
    /// <typeparam name="New">The new type to redirect to.</typeparam>
    public void Redirect<Old, New>()
      where Old : T
      where New : Old => this.Redirect(typeof (Old), typeof (New));
  }
}
