// Decompiled with JetBrains decompiler
// Type: DuckGame.UIText
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIText : UIComponent
  {
    protected Color _color;
    protected BitmapFont _font;
    protected string _text;
    private float _heightAdd;
    private InputProfile _controlProfile;

    public virtual string text
    {
      get => this._text;
      set
      {
        this._text = value;
        this._collisionSize = new Vec2(this._font.GetWidth(this._text), this._font.height + this._heightAdd);
      }
    }

    public void SetFont(BitmapFont f)
    {
      if (f != null)
        this._font = f;
      this._collisionSize = new Vec2(this._font.GetWidth(this._text), this._font.height + this._heightAdd);
    }

    public UIText(
      string textVal,
      Color c,
      UIAlign al = UIAlign.Center,
      float heightAdd = 0.0f,
      InputProfile controlProfile = null)
      : base(0.0f, 0.0f, 0.0f, 0.0f)
    {
      this._heightAdd = heightAdd;
      this._font = new BitmapFont("biosFontUI", 8, 7);
      this.text = textVal;
      this._color = c;
      this.align = al;
      this._controlProfile = controlProfile;
    }

    public override void Draw()
    {
      this._font.scale = this.scale;
      this._font.alpha = this.alpha;
      float width = this._font.GetWidth(this._text);
      this._font.Draw(this._text, this.x + ((this.align & UIAlign.Left) <= UIAlign.Center ? ((this.align & UIAlign.Right) <= UIAlign.Center ? (float) (-(double) width / 2.0) : this.width / 2f - width) : (float) -((double) this.width / 2.0)), this.y + ((this.align & UIAlign.Top) <= UIAlign.Center ? ((this.align & UIAlign.Bottom) <= UIAlign.Center ? (float) (-(double) this._font.height / 2.0) : this.height / 2f - this._font.height) : (float) -((double) this.height / 2.0)), this._color, this.depth, this._controlProfile);
      base.Draw();
    }
  }
}
