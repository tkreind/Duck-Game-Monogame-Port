// Decompiled with JetBrains decompiler
// Type: DuckGame.MuzzleFlare
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class MuzzleFlare : Thing
  {
    private SpriteMap _sprite;

    public MuzzleFlare(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("smallFlare", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(0.0f, 8f);
    }

    public override void Update()
    {
      this.alpha -= 0.1f;
      if ((double) this.alpha >= 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
