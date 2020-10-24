// Decompiled with JetBrains decompiler
// Type: DuckGame.CornerDisplay
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CornerDisplay
  {
    public HUDCorner corner;
    public float slide;
    public string text;
    public bool closing;
    public Timer timer;
    public int lowTimeTick = 4;
    public FieldBinding counter;
    public int curCount;
    public int realCount;
    public int addCount;
    public float addCountWait;
    public int maxCount;
    public bool animateCount;
    public bool isControl;
    public InputProfile profile;
    public bool stack;
    public float life = 1f;
  }
}
