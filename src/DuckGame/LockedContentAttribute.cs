// Decompiled with JetBrains decompiler
// Type: DuckGame.LockedContentAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// Indicates that this type is locked in the content list and cannot be modified.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  internal class LockedContentAttribute : Attribute
  {
  }
}
