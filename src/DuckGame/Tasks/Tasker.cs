// Decompiled with JetBrains decompiler
// Type: DuckGame.Tasker
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Threading;

namespace DuckGame
{
  public static class Tasker
  {
    internal static object _taskLock = new object();
    private static Queue<Promise> _promises = new Queue<Promise>();

    public static void RunTasks(uint max = 4294967295)
    {
      lock (Tasker._taskLock)
      {
        while (Tasker._promises.Count != 0 && max > 0U)
        {
          --max;
          Tasker._promises.Dequeue().Execute();
        }
      }
    }

    private static bool IsMainThread => Thread.CurrentThread == MonoMain.mainThread;

    public static Promise<T> Task<T>(Func<T> function)
    {
      lock (Tasker._taskLock)
      {
        Promise<T> promise = new Promise<T>(function);
        if (Tasker.IsMainThread)
          promise.Execute();
        else
          Tasker._promises.Enqueue((Promise) promise);
        return promise;
      }
    }

    public static Promise Task(Action function)
    {
      lock (Tasker._taskLock)
      {
        Promise promise = new Promise(function);
        if (Tasker.IsMainThread)
          promise.Execute();
        else
          Tasker._promises.Enqueue(promise);
        return promise;
      }
    }
  }
}
