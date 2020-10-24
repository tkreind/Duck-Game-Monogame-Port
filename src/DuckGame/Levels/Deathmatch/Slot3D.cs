// Decompiled with JetBrains decompiler
// Type: DuckGame.Slot3D
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class Slot3D
  {
    public RockThrow state;
    public Duck duck;
    public List<Duck> subDucks = new List<Duck>();
    public List<DuckAI> subAIs = new List<DuckAI>();
    public ScoreRock rock;
    public int slideWait;
    public DuckAI ai;
    public int slotIndex;
    public float startX;
    public bool follow;
    public bool showScore;
    public bool highestScore;

    public float scroll
    {
      get
      {
        if (this.slotIndex == 0)
          return (float) (-(double) this.duck.position.x * 0.665000021457672 + 100.0);
        if (this.slotIndex == 1)
          return (float) (-(double) this.duck.position.x * 0.665000021457672 + 100.0);
        return this.slotIndex == 2 ? (float) (-(double) this.duck.position.x * 0.665000021457672 + 100.0) : (float) (-(double) this.duck.position.x * 0.665000021457672 + 100.0);
      }
    }
  }
}
