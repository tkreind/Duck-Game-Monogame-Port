// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMultiToggle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIMultiToggle : UIText
  {
    private FieldBinding _field;
    private List<string> _captions;
    private bool _compressed;

    public void SetFieldBinding(FieldBinding f) => this._field = f;

    public UIMultiToggle(
      float wide,
      float high,
      FieldBinding field,
      List<string> captions,
      bool compressed = false)
      : base("AAAAAAAAA", Color.White)
    {
      this._field = field;
      this._captions = captions;
      this._compressed = compressed;
    }

    public override void Draw()
    {
      this._font.scale = this.scale;
      this._font.alpha = this.alpha;
      int index = (int) this._field.value;
      string text = "";
      if (this._compressed && index < this._captions.Count)
      {
        text = this._captions[index];
      }
      else
      {
        int num = 0;
        foreach (string caption in this._captions)
        {
          if (num != 0)
            text += " ";
          text = num != index ? text + "|GRAY|" : text + "|WHITE|";
          text += caption;
          ++num;
        }
      }
      float width = this._font.GetWidth(text);
      float num1 = (this.align & UIAlign.Left) <= UIAlign.Center ? ((this.align & UIAlign.Right) <= UIAlign.Center ? (float) (-(double) width / 2.0) : this.width / 2f - width) : (float) -((double) this.width / 2.0);
      float num2 = (this.align & UIAlign.Top) <= UIAlign.Center ? ((this.align & UIAlign.Bottom) <= UIAlign.Center ? (float) (-(double) this._font.height / 2.0) : this.height / 2f - this._font.height) : (float) -((double) this.height / 2.0);
      this._font.Draw(text, this.x + num1, this.y + num2, Color.White, this.depth);
    }
  }
}
