// Decompiled with JetBrains decompiler
// Type: DuckGame.Animation
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct Animation
  {
    public string name;
    public float speed;
    public int[] frames;
    public bool looping;

    public Animation(string nameVal, float speedVal, bool loopVal, int[] framesVal)
    {
      this.name = nameVal;
      this.speed = speedVal;
      this.frames = framesVal;
      this.looping = loopVal;
    }
  }
}
