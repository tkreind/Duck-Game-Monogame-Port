// Decompiled with JetBrains decompiler
// Type: DuckGame.ModConfigurationException
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Runtime.Serialization;

namespace DuckGame
{
  [Serializable]
  internal class ModConfigurationException : Exception
  {
    public ModConfigurationException()
    {
    }

    public ModConfigurationException(string message)
      : base(message)
    {
    }

    public ModConfigurationException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected ModConfigurationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
