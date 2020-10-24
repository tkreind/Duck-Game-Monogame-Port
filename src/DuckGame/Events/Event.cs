// Decompiled with JetBrains decompiler
// Type: DuckGame.Event
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class Event
  {
    private static List<Event> _events = new List<Event>();
    protected DateTime _timestamp;
    protected Profile _victim;
    protected Profile _dealer;

    public static List<Event> events => Event._events;

    public static void Log(Event e) => Event._events.Add(e);

    public Event(Profile dealerVal, Profile victimVal)
    {
      this._victim = victimVal;
      this._dealer = dealerVal;
      this._timestamp = DateTime.Now;
    }

    public DateTime timestamp => this._timestamp;

    public Profile victim => this._victim;

    public Profile dealer => this._dealer;
  }
}
