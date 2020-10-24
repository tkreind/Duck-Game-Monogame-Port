// Decompiled with JetBrains decompiler
// Type: DuckGame.BaggedPropertyAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// Mark a property to be added to the initial property bag for this class.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  public class BaggedPropertyAttribute : Attribute
  {
    internal string Property { get; private set; }

    internal object Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuckGame.BaggedPropertyAttribute" /> class.
    /// </summary>
    /// <param name="prop">The property.</param>
    /// <param name="val">The value.</param>
    public BaggedPropertyAttribute(string prop, object val)
    {
      this.Property = prop;
      this.Value = val;
    }
  }
}
