// Decompiled with JetBrains decompiler
// Type: DuckGame.PriorityAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>An attribute to mark the priority of something.</summary>
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  public class PriorityAttribute : Attribute
  {
    /// <summary>Gets the priority of this target.</summary>
    /// <value>The priority.</value>
    public Priority Priority { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuckGame.PriorityAttribute" /> class.
    /// </summary>
    /// <param name="priority">The priority.</param>
    public PriorityAttribute(Priority priority) => this.Priority = priority;
  }
}
