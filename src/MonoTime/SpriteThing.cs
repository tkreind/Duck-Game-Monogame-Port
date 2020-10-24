// Decompiled with JetBrains decompiler
// Type: DuckGame.SpriteThing
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class SpriteThing : Thing
  {
    public DuckPersona persona;
    public Color color;

    public SpriteThing(float xpos, float ypos, Sprite spr)
      : base(xpos, ypos, spr)
    {
      this.collisionSize = new Vec2((float) spr.width, (float) spr.height);
      this.center = new Vec2((float) (spr.w / 2), (float) (spr.h / 2));
      this.collisionOffset = new Vec2((float) -(spr.w / 2), (float) -(spr.h / 2));
      this.color = Color.White;
    }

    public override void Draw()
    {
      this.graphic.flipH = this.flipHorizontal;
      this.graphic.color = this.color;
      base.Draw();
    }
  }
}
