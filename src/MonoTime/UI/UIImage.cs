// Decompiled with JetBrains decompiler
// Type: DuckGame.UIImage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIImage : UIComponent
  {
    private Sprite _image;
    private float yOffset;

    public UIImage(string imageVal, UIAlign al = UIAlign.Left)
      : base(0.0f, 0.0f, -1f, -1f)
    {
      this._image = new Sprite(imageVal);
      this._collisionSize = new Vec2((float) this._image.w, (float) this._image.h);
      this._image.CenterOrigin();
      this.align = al;
    }

    public UIImage(Sprite imageVal, UIAlign al = UIAlign.Left, float s = 1f, float yOff = 0.0f)
      : base(0.0f, 0.0f, -1f, -1f)
    {
      this._image = imageVal;
      this._collisionSize = new Vec2((float) this._image.w * s, (float) this._image.h * s);
      this._image.CenterOrigin();
      this.scale = new Vec2(s);
      this.align = al;
      this.yOffset = yOff;
    }

    public override void Draw()
    {
      this._image.scale = this.scale;
      this._image.alpha = this.alpha;
      this._image.depth = this.depth;
      Graphics.Draw(this._image, this.x, this.y + this.yOffset);
      base.Draw();
    }
  }
}
