// Decompiled with JetBrains decompiler
// Type: DuckGame.UIChangingText
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIChangingText : UIText
  {
    private FieldBinding _field;
    private FieldBinding _filterBinding;

    public UIChangingText(float wide, float high, FieldBinding field, FieldBinding filterBinding)
      : base("ON OFF  ", Color.White)
    {
      this._field = field;
      this._filterBinding = filterBinding;
    }

    public override string text
    {
      get => this._text;
      set => this._text = value;
    }

    public override void Draw()
    {
      this._font.scale = this.scale;
      this._font.alpha = this.alpha;
      float width = this._font.GetWidth("ON OFF  ");
      float num1 = (this.align & UIAlign.Left) <= UIAlign.Center ? ((this.align & UIAlign.Right) <= UIAlign.Center ? (float) (-(double) width / 2.0) : this.width / 2f - width) : (float) -((double) this.width / 2.0);
      float num2 = (this.align & UIAlign.Top) <= UIAlign.Center ? ((this.align & UIAlign.Bottom) <= UIAlign.Center ? (float) (-(double) this._font.height / 2.0) : this.height / 2f - this._font.height) : (float) -((double) this.height / 2.0);
      string text = this.text;
      while (text.Length < 8)
        text = " " + text;
      this._font.Draw(text, this.x + num1, this.y + num2, Color.White, this.depth);
    }
  }
}
