// Decompiled with JetBrains decompiler
// Type: DuckGame.MagicNumberAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// Declares a magic number to be written for identifying a BinaryClassChunk
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public sealed class MagicNumberAttribute : Attribute
  {
    public readonly long magicNumber;

    public MagicNumberAttribute(long number) => this.magicNumber = number;
  }
}
