// Decompiled with JetBrains decompiler
// Type: DuckGame.BlueBarrel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("noRandomSpawningOnline", true)]
  [EditorGroup("stuff|props")]
  public class BlueBarrel : YellowBarrel
  {
    public BlueBarrel(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("blueBarrel");
      this.center = new Vec2(7f, 8f);
      this._melting = new Sprite("blueBarrelMelting");
      this._editorName = "Barrel B";
      this.flammable = 0.3f;
      this._fluid = Fluid.Water;
      this._toreUp = new SpriteMap("blueBarrelToreUp", 14, 12);
      this._toreUp.frame = 1;
      this._toreUp.center = new Vec2(0.0f, -8f);
    }
  }
}
