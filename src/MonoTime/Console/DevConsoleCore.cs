// Decompiled with JetBrains decompiler
// Type: DuckGame.DevConsoleCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class DevConsoleCore
  {
    public int logScores = -1;
    public Queue<DCLine> lines = new Queue<DCLine>();
    public List<DCLine> pendingLines = new List<DCLine>();
    public List<DCChartValue> pendingChartValues = new List<DCChartValue>();
    public BitmapFont font;
    public FancyBitmapFont fancyFont;
    public float alpha;
    public bool open;
    public string typing = "";
    public string lastLine = "";
    public bool splitScreen;
    public bool rhythmMode;
    public bool qwopMode;
    public bool showIslands;
    public bool showCollision;
    public bool shieldMode;
  }
}
