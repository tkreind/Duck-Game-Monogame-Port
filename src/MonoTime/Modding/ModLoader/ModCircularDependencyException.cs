// Decompiled with JetBrains decompiler
// Type: DuckGame.ModCircularDependencyException
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DuckGame
{
  [Serializable]
  internal class ModCircularDependencyException : Exception
  {
    public ModCircularDependencyException()
    {
    }

    public ModCircularDependencyException(string message)
      : base(message)
    {
    }

    public ModCircularDependencyException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected ModCircularDependencyException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    private static string CompileStack(Stack<string> stack)
    {
      string str1 = "A circular dependency was detected in the list. Mod load order:\r\n";
      foreach (string str2 in stack)
        str1 = str1 + " " + str2 + "\r\n";
      return str1;
    }

    public ModCircularDependencyException(Stack<string> stack)
      : base(ModCircularDependencyException.CompileStack(stack))
    {
    }
  }
}
