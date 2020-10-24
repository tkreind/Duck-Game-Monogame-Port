// Decompiled with JetBrains decompiler
// Type: DuckGame.ResultData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct ResultData
  {
    public string name;
    public bool multi;
    public object data;
    public int score;
    public BitmapFont font;

    public ResultData(Team t)
    {
      this.font = Profiles.EnvironmentProfile.font;
      if (t.activeProfiles.Count > 1)
      {
        this.name = t.name;
        this.multi = true;
      }
      else
      {
        this.name = !Profiles.IsDefault(t.activeProfiles[0]) ? t.activeProfiles[0].name : t.name;
        this.font = t.activeProfiles[0].font;
        this.multi = false;
      }
      this.data = (object) t;
      this.score = t.score;
    }
  }
}
