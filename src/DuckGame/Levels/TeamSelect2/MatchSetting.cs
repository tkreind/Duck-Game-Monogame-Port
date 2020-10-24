// Decompiled with JetBrains decompiler
// Type: DuckGame.MatchSetting
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class MatchSetting
  {
    public Dictionary<int, int> stepMap;
    public string id = "";
    public string name = "";
    public string suffix = "";
    public string prefix = "";
    public string filterText = "ANY";
    public FilterMode filterMode;
    private object _value;
    public object prevValue;
    public int min;
    public int max = 10;
    public string minString;
    public int step = 1;
    public string maxSyncID = "";
    public string minSyncID = "";
    public bool filtered;
    public object defaultValue;
    public bool createOnly;
    public bool filterOnly;
    public List<string> valueStrings;
    public List<string> percentageLinks;

    public object value
    {
      get => this._value;
      set
      {
        if (this._value == null && value != null)
          this.defaultValue = value;
        this.prevValue = this._value == null ? value : this._value;
        this._value = value;
      }
    }
  }
}
