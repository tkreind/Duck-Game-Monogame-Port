// Decompiled with JetBrains decompiler
// Type: DuckGame.WeatherParticle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public abstract class WeatherParticle
  {
    public Vec2 position;
    public float z;
    public Vec2 velocity;
    public float zSpeed;
    public float alpha = 1f;
    public bool die;

    public WeatherParticle(Vec2 pos) => this.position = pos;

    public abstract void Draw();

    public abstract void Update();
  }
}
