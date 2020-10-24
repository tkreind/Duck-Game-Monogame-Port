// Decompiled with JetBrains decompiler
// Type: DuckGame.GlassDebris
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GlassDebris : PhysicsParticle
  {
    private SpriteMap _sprite;

    public GlassDebris(bool rotate, float xpos, float ypos, float h, float v, int f, int tint = 0)
      : base(xpos, ypos)
    {
      this.hSpeed = h;
      this.vSpeed = v;
      this._sprite = new SpriteMap("windowDebris", 8, 8);
      this._sprite.frame = Rando.Int(7);
      this._sprite.color = Window.windowColors[tint] * 0.6f;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(4f, 4f);
      this._bounceEfficiency = 0.3f;
      if (!rotate)
        return;
      this.angle -= 1.57f;
    }

    public override void Update()
    {
      this.alpha -= 0.01f;
      if ((double) this.alpha < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
