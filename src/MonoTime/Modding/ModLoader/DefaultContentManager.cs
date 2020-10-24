// Decompiled with JetBrains decompiler
// Type: DuckGame.DefaultContentManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  /// <summary>
  /// The quick and easy default implementation. Pulls all exported types
  /// that are subclassed by the requested Type.
  /// </summary>
  internal class DefaultContentManager : IManageContent
  {
    public IEnumerable<System.Type> Compile<T>(Mod mod) => ((IEnumerable<System.Type>) mod.configuration.assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (type => type.IsSubclassOf(typeof (T))));
  }
}
