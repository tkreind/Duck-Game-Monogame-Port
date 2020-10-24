// Decompiled with JetBrains decompiler
// Type: DuckGame.TravelInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class TravelInfo
  {
    public Vec2 p1;
    public Vec2 p2;
    public float length;

    public TravelInfo(Vec2 point1, Vec2 point2, float len)
    {
      this.p1 = point1;
      this.p2 = point2;
      this.length = len;
    }
  }
}
