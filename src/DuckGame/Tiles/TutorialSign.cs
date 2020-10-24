// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public abstract class TutorialSign : Thing
  {
    public TutorialSign(float xpos, float ypos, string image, string name)
      : base(xpos, ypos)
    {
      if (image == null)
        return;
      this.graphic = new Sprite(image);
      this.center = new Vec2((float) (this.graphic.w / 2), (float) (this.graphic.h / 2));
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) -0.5f;
      this._editorName = name;
      this.layer = Layer.Background;
    }
  }
}
