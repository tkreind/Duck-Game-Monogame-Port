// Decompiled with JetBrains decompiler
// Type: DuckGame.AutoUpdatables
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public static class AutoUpdatables
  {
    private static List<WeakReference> _updateables = new List<WeakReference>();

    public static void Add(IAutoUpdate update) => AutoUpdatables._updateables.Add(new WeakReference((object) update));

    public static void Clear() => AutoUpdatables._updateables.Clear();

    public static void ClearSounds()
    {
      for (int index = 0; index < AutoUpdatables._updateables.Count; ++index)
      {
        if (AutoUpdatables._updateables[index] != null && AutoUpdatables._updateables[index].Target != null && AutoUpdatables._updateables[index].Target is ConstantSound)
          (AutoUpdatables._updateables[index].Target as ConstantSound).Kill();
      }
    }

    public static void Update()
    {
      int num = 25;
      for (int index = 0; index < AutoUpdatables._updateables.Count; ++index)
      {
        if (AutoUpdatables._updateables[index] == null)
        {
          if (num > 0)
          {
            AutoUpdatables._updateables.RemoveAt(index);
            --index;
            --num;
          }
        }
        else
        {
          IAutoUpdate target = AutoUpdatables._updateables[index].Target as IAutoUpdate;
          if (!AutoUpdatables._updateables[index].IsAlive || target == null)
            AutoUpdatables._updateables[index] = (WeakReference) null;
          else
            target.Update();
        }
      }
    }
  }
}
