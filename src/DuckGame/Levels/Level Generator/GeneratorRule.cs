// Decompiled with JetBrains decompiler
// Type: DuckGame.GeneratorRule
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class GeneratorRule
  {
    public Func<RandomLevelData, bool> problem;
    public Func<RandomLevelData, bool> solution;
    public float chance = 1f;
    public bool mandatory;
    public SpecialRule special;

    public GeneratorRule(
      Func<RandomLevelData, bool> varProblem,
      Func<RandomLevelData, bool> varSolution,
      float varChance = 1f,
      bool varMandatory = false,
      SpecialRule varSpecial = SpecialRule.None)
    {
      this.problem = varProblem;
      this.solution = varSolution;
      this.chance = varChance;
      this.mandatory = varMandatory;
      this.special = varSpecial;
    }
  }
}
