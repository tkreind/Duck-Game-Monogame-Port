// Decompiled with JetBrains decompiler
// Type: DuckGame.UnlockableAchievement
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class UnlockableAchievement : Unlockable
  {
    public UnlockableAchievement(
      string identifier,
      Func<bool> condition,
      string nam,
      string desc,
      string achieve)
      : base(identifier, condition, nam, desc, achieve)
    {
    }
  }
}
