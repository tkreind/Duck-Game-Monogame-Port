// Decompiled with JetBrains decompiler
// Type: DuckGame.TourneyGroup
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class TourneyGroup
  {
    public List<Team> players = new List<Team>();
    public List<bool> assigned = new List<bool>();
    public TourneyGroup next;
    public int groupIndex;
    public int depth;

    public void AddPlayer(Team p, bool ass = false)
    {
      this.players.Add(p);
      this.assigned.Add(ass);
    }
  }
}
