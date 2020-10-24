// Decompiled with JetBrains decompiler
// Type: DuckGame.Stats
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Stats
  {
    private static float _averageRoundTime;
    private static float _totalRoundTime;
    private static int _numberOfRounds;

    public static float averageRoundTime => Stats._totalRoundTime / (float) Stats.numberOfRounds;

    public static float totalRoundTime => Stats._averageRoundTime;

    public static int numberOfRounds => Stats._numberOfRounds;

    public static int lastMatchLength
    {
      get
      {
        DateTime dateTime = DateTime.Now;
        for (int index = Event.events.Count - 1; index > 0; --index)
        {
          Event @event = Event.events[index];
          switch (@event)
          {
            case RoundEndEvent _:
              dateTime = @event.timestamp;
              break;
            case RoundStartEvent _:
              return (int) (dateTime - @event.timestamp).TotalSeconds;
          }
        }
        return 99;
      }
    }

    public static void CalculateStats()
    {
      DateTime dateTime = DateTime.Now;
      Stats._totalRoundTime = 0.0f;
      Stats._numberOfRounds = 0;
      foreach (Event @event in Event.events)
      {
        if (@event is RoundStartEvent)
          dateTime = @event.timestamp;
        else if (@event is RoundEndEvent)
        {
          Stats._totalRoundTime += (float) (int) (@event.timestamp - dateTime).TotalSeconds;
          ++Stats._numberOfRounds;
        }
      }
    }
  }
}
