// Decompiled with JetBrains decompiler
// Type: DuckGame.GlobalData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Reflection;

namespace DuckGame
{
  public class GlobalData : DataClass
  {
    private string _boughtHats = "";
    public StatBinding matchesPlayed;
    public StatBinding ducksCrushed;
    public StatBinding kills;
    public StatBinding longestMatchPlayed;
    public StatBinding onlineWins;
    public StatBinding drawsPlayed;
    public StatBinding littleDraws;
    public StatBinding timesSpawned;
    public StatBinding winsAsSwack;
    public StatBinding disarms;
    public StatBinding jetFuelUsed;
    public StatBinding laserBulletsFired;
    public StatBinding strafeDistance;
    public StatBinding quacks;
    public StatBinding giantLaserKills;
    public Dictionary<string, int> hatWins = new Dictionary<string, int>();
    public Dictionary<string, int> customMapPlayCount = new Dictionary<string, int>();

    public int angleShots { get; set; }

    public int bootedSinceUpdate { get; set; }

    public int hatsStolen { get; set; }

    public int levelsPlayed { get; set; }

    public int killsAsSwack { get; set; }

    public bool gotMineAchievement { get; set; }

    public bool gotBookAchievement { get; set; }

    public int highestNewsCast { get; set; }

    public string boughtHats
    {
      get => this._boughtHats;
      set => this._boughtHats = value;
    }

    public int GetHatMatchWins(string hat) => this.hatWins.ContainsKey(hat) ? this.hatWins[hat] : 0;

    public GlobalData()
    {
      this._nodeName = "Global";
      foreach (FieldInfo field in this.GetType().GetFields())
      {
        if (field.FieldType == typeof (StatBinding))
        {
          StatBinding statBinding = new StatBinding();
          statBinding.BindName(field.Name);
          field.SetValue((object) this, (object) statBinding);
        }
      }
    }
  }
}
