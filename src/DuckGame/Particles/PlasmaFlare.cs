// Decompiled with JetBrains decompiler
// Type: DuckGame.PlasmaFlare
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PlasmaFlare : Thing
  {
    private SpriteMap _sprite;

    public PlasmaFlare(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("plasmaFlare", 16, 16);
      this._sprite.AddAnimation("idle", 0.7f, false, 0, 1, 2);
      this._sprite.SetAnimation("idle");
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(0.0f, 16f);
    }

    public override void Update()
    {
      if (!this._sprite.finished)
        return;
      Level.Remove((Thing) this);
    }
  }
}
