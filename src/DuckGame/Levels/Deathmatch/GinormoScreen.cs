// Decompiled with JetBrains decompiler
// Type: DuckGame.GinormoScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class GinormoScreen : Thing
  {
    private BitmapFont _font;

    public GinormoScreen(float xpos, float ypos, BoardMode mode)
      : base(xpos, ypos)
    {
      this.layer = Layer.Foreground;
      this.depth = (Depth) 0.0f;
      this._font = new BitmapFont("biosFont", 8);
      this._collisionSize = new Vec2(184f, 102f);
      List<Team> teamList = new List<Team>();
      int idx = 0;
      foreach (Team team in Teams.all)
      {
        if (team.activeProfiles.Count > 0)
          teamList.Add(team);
      }
      teamList.Sort((Comparison<Team>) ((a, b) =>
      {
        if (a.score == b.score)
          return 0;
        return a.score >= b.score ? -1 : 1;
      }));
      foreach (Team team in teamList)
      {
        float y = this.y + 2f + (float) (25 * idx);
        if ((double) Graphics.aspect > 0.589999973773956)
          y += 10f;
        Level.current.AddThing((Thing) new GinormoCard((float) idx * 1f, new Vec2(300f, y), new Vec2(this.x + (mode == BoardMode.Points ? 2f : 2f), y), team, mode, idx));
        ++idx;
      }
    }

    public override void Draw()
    {
    }
  }
}
