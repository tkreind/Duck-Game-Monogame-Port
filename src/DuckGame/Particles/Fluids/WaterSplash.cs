// Decompiled with JetBrains decompiler
// Type: DuckGame.WaterSplash
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WaterSplash : Thing
  {
    private SpriteMap _sprite;

    public WaterSplash(float xpos, float ypos, FluidData fluid)
      : base(xpos, ypos)
    {
      this._sprite = fluid.sprite == null ? new SpriteMap("waterSplash", 16, 16) : new SpriteMap(fluid.sprite + "Splash", 16, 16);
      this._sprite.AddAnimation("splash", 0.45f, false, 0, 1, 2, 3);
      this._sprite.SetAnimation("splash");
      this.center = new Vec2(8f, 16f);
      this.graphic = (Sprite) this._sprite;
      this.depth = new Depth(0.7f);
    }

    public override void Update()
    {
      if (!this._sprite.finished)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw() => base.Draw();
  }
}
