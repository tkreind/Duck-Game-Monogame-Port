// Decompiled with JetBrains decompiler
// Type: DuckGame.Factory`1
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  internal static class Factory<T> where T : new()
  {
    private static int kMaxObjects = 1024;
    private static T[] _objects = new T[Factory<T>.kMaxObjects];
    private static int _lastActiveObject = 0;

    static Factory()
    {
      for (int index = 0; index < Factory<T>.kMaxObjects; ++index)
        Factory<T>._objects[index] = new T();
    }

    public static T New()
    {
      T obj1 = default (T);
      T obj2 = Factory<T>._objects[Factory<T>._lastActiveObject];
      Factory<T>._lastActiveObject = (Factory<T>._lastActiveObject + 1) % Factory<T>.kMaxObjects;
      return obj2;
    }
  }
}
