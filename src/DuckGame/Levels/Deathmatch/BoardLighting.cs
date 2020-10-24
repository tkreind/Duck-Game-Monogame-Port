// Decompiled with JetBrains decompiler
// Type: DuckGame.BoardLighting
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BoardLighting : Thing
  {
    private Sprite _lightRay;

    public BoardLighting(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._lightRay = new Sprite("rockThrow/lightRays");
      this.center = new Vec2(305f, 0.0f);
      this.graphic = this._lightRay;
    }

    public override void Draw()
    {
      if ((double) RockWeather.lightOpacity < 0.00999999977648258 || Layer.blurry)
        return;
      base.Draw();
    }
  }
}
