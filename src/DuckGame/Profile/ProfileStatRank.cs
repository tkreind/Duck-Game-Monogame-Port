// Decompiled with JetBrains decompiler
// Type: DuckGame.ProfileStatRank
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ProfileStatRank
  {
    public StatInfo stat;
    public float value;
    public List<Profile> profiles = new List<Profile>();

    public ProfileStatRank(StatInfo s, float val, Profile pro = null)
    {
      if (pro != null)
        this.profiles.Add(pro);
      this.value = val;
      this.stat = s;
    }
  }
}
