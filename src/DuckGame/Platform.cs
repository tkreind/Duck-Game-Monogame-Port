// Decompiled with JetBrains decompiler
// Type: DuckGame.Platform
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Platform : MaterialThing, IPlatform
  {
    public Platform(float x, float y)
      : base(x, y)
    {
      this.collisionSize = new Vec2(16f, 16f);
      this.thickness = 10f;
    }

    public Platform(float x, float y, float wid, float hi)
      : base(x, y)
    {
      this.collisionSize = new Vec2(wid, hi);
      this.thickness = 10f;
    }
  }
}
