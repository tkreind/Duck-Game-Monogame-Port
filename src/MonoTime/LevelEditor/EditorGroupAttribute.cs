// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorGroupAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>Declares which group this Thing is in the editor</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class EditorGroupAttribute : Attribute
  {
    public readonly string editorGroup;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuckGame.EditorGroupAttribute" /> class.
    /// </summary>
    /// <param name="group">The editor group, in the format of "root|sub|sub|sub..."</param>
    public EditorGroupAttribute(string group) => this.editorGroup = group;
  }
}
