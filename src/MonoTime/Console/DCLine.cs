// Decompiled with JetBrains decompiler
// Type: DuckGame.DCLine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class DCLine
  {
    public string line;
    public Color color;
    public float scale = 2f;
    public int threadIndex;
    public DateTime timestamp;
    public DCSection section;
    public Verbosity verbosity;

    public override string ToString() => this.line + " | " + this.timestamp.ToLongTimeString();

    public string SectionString()
    {
      switch (this.section)
      {
        case DCSection.NetCore:
          return "|DGBLUE|NETCORE ";
        case DCSection.DuckNet:
          return "|PINK|DUCKNET ";
        case DCSection.GhostMan:
          return "|DGPURPLE|GHOSTMAN ";
        case DCSection.Steam:
          return "|DGORANGE|STEAM ";
        case DCSection.Mod:
          return "|DGGREEN|MOD ";
        default:
          return "";
      }
    }
  }
}
