// Decompiled with JetBrains decompiler
// Type: DuckGame.ModTypeMissingException
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Runtime.Serialization;

namespace DuckGame
{
  [Serializable]
  internal class ModTypeMissingException : Exception
  {
    public ModTypeMissingException()
    {
    }

    public ModTypeMissingException(string message)
      : base(message)
    {
    }

    public ModTypeMissingException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected ModTypeMissingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
