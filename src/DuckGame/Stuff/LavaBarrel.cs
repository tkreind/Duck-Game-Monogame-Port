// Decompiled with JetBrains decompiler
// Type: DuckGame.LavaBarrel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  [BaggedProperty("noRandomSpawningOnline", true)]
  public class LavaBarrel : YellowBarrel
  {
    public LavaBarrel(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("lavaBarrel");
      this.center = new Vec2(7f, 8f);
      this._melting = new Sprite("blueBarrelMelting");
      this._editorName = "Barrel B";
      this.flammable = 0.0f;
      this._fluid = Fluid.Lava;
      this._toreUp = new SpriteMap("blueBarrelToreUp", 14, 12);
      this._toreUp.frame = 1;
      this._toreUp.center = new Vec2(0.0f, -8f);
    }
  }
}
