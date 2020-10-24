// Decompiled with JetBrains decompiler
// Type: DuckGame.PlayerCard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class PlayerCard : Thing
  {
    private float _slideWait;
    private Vec2 _start;
    private Vec2 _end;
    private List<SpriteMap> _sprites = new List<SpriteMap>();
    private Team _team;

    public PlayerCard(float slideWait, Vec2 start, Vec2 end, Team team)
      : base()
    {
      this.layer = Layer.HUD;
      this._start = start;
      this._end = end;
      this._slideWait = slideWait;
      this.position = this._start;
      this._team = team;
    }

    public override void Draw()
    {
    }
  }
}
