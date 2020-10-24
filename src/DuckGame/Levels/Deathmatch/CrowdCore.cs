// Decompiled with JetBrains decompiler
// Type: DuckGame.CrowdCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CrowdCore
  {
    public Mood _mood;
    public Mood _newMood;
    public float _moodWait = 1f;
    public List<List<CrowdDuck>> _members = new List<List<CrowdDuck>>();
    public int fansUsed;

    public Mood mood
    {
      get => this._mood;
      set => this._newMood = value;
    }
  }
}
