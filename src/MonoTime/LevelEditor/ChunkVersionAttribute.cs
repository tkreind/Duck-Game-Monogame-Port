// Decompiled with JetBrains decompiler
// Type: DuckGame.ChunkVersionAttribute
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>
  /// Declares a version number to be written for identifying the version of a BinaryClassChunk
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public sealed class ChunkVersionAttribute : Attribute
  {
    public readonly ushort version;

    public ChunkVersionAttribute(ushort version) => this.version = version;
  }
}
