// Decompiled with JetBrains decompiler
// Type: DuckGame.StartLight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class StartLight : Thing
  {
    private SpriteMap _sprite;

    public StartLight()
      : base()
    {
      this._sprite = new SpriteMap("trafficLight", 42, 23);
      this.center = new Vec2((float) (this._sprite.w / 2), (float) (this._sprite.h / 2));
      this.graphic = (Sprite) this._sprite;
      this.layer = Layer.HUD;
      this.x = Layer.HUD.camera.width / 2f;
      this.y = 20f;
    }

    public override void Update()
    {
    }
  }
}
