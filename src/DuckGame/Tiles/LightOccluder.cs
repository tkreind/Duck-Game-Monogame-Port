// Decompiled with JetBrains decompiler
// Type: DuckGame.LightOccluder
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class LightOccluder
  {
    public Vec2 p1;
    public Vec2 p2;
    public Color color;

    public LightOccluder(Vec2 p, Vec2 pp, Color c)
    {
      this.p1 = p;
      this.p2 = pp;
      this.color = c;
    }
  }
}
