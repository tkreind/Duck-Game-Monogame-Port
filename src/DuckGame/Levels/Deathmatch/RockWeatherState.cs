// Decompiled with JetBrains decompiler
// Type: DuckGame.RockWeatherState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class RockWeatherState
  {
    public Vec3 add;
    public Vec3 multiply;
    public Vec3 sky;
    public Vec2 sunPos;
    public float lightOpacity;
    public float sunGlow;
    public float sunOpacity = 1f;
    public float rainbowLight;
    public float rainbowLight2;

    public RockWeatherState Copy() => new RockWeatherState()
    {
      add = this.add,
      multiply = this.multiply,
      sky = this.sky,
      sunPos = this.sunPos,
      lightOpacity = this.lightOpacity,
      sunGlow = this.sunGlow,
      sunOpacity = this.sunOpacity,
      rainbowLight = this.rainbowLight,
      rainbowLight2 = this.rainbowLight2
    };
  }
}
