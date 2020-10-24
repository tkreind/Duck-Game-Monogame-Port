// Decompiled with JetBrains decompiler
// Type: DuckGame.ModDependencyNotFoundException
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Runtime.Serialization;

namespace DuckGame
{
  [Serializable]
  internal class ModDependencyNotFoundException : Exception
  {
    public ModDependencyNotFoundException()
    {
    }

    public ModDependencyNotFoundException(string message)
      : base(message)
    {
    }

    public ModDependencyNotFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected ModDependencyNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public ModDependencyNotFoundException(string mod, string missing)
      : base("Mod " + mod + " is missing hard dependency " + missing)
    {
    }
  }
}
