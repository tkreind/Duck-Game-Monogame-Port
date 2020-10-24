// Decompiled with JetBrains decompiler
// Type: DuckGame.ScoreBoard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ScoreBoard : Thing
  {
    public ScoreBoard()
      : base()
    {
    }

    public override void Initialize()
    {
      int num = 0;
      foreach (Team team in Teams.all)
      {
        if (team.activeProfiles.Count > 0)
        {
          Level.current.AddThing((Thing) new PlayerCard((float) num * 1f, new Vec2(-400f, (float) (140 * num + 120)), new Vec2((float) (Graphics.width / 2 - 200), (float) (140 * num + 120)), team));
          ++num;
        }
      }
    }
  }
}
