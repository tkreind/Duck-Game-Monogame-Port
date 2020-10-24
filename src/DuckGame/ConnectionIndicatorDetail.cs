// Decompiled with JetBrains decompiler
// Type: DuckGame.ConnectionIndicatorDetail
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ConnectionIndicatorDetail
  {
    public ConnectionIndicatorType type;
    public float buildup;
    public int frames;
    public float buildupThreshold;
    public float maxBuildup = 1f;
    public int iconFrame;
    public float popOut;
    public float grow;

    public bool visible => (double) this.buildup > (double) this.buildupThreshold;
  }
}
