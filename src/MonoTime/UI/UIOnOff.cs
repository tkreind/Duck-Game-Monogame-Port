// Decompiled with JetBrains decompiler
// Type: DuckGame.UIOnOff
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIOnOff : UIText
  {
    private FieldBinding _field;
    private FieldBinding _filterBinding;

    public UIOnOff(float wide, float high, FieldBinding field, FieldBinding filterBinding)
      : base("ON OFF", Color.White)
    {
      this._field = field;
      this._filterBinding = filterBinding;
    }

    public override void Draw()
    {
      this._font.scale = this.scale;
      this._font.alpha = this.alpha;
      float width = this._font.GetWidth("ON OFF");
      float num1 = (this.align & UIAlign.Left) <= UIAlign.Center ? ((this.align & UIAlign.Right) <= UIAlign.Center ? (float) (-(double) width / 2.0) : this.width / 2f - width) : (float) -((double) this.width / 2.0);
      float num2 = (this.align & UIAlign.Top) <= UIAlign.Center ? ((this.align & UIAlign.Bottom) <= UIAlign.Center ? (float) (-(double) this._font.height / 2.0) : this.height / 2f - this._font.height) : (float) -((double) this.height / 2.0);
      bool flag = (bool) this._field.value;
      if (this._filterBinding != null)
      {
        if (!(bool) this._filterBinding.value)
          this._font.Draw("   |DGGREEN|ANY", this.x + num1, this.y + num2, Color.White, this.depth);
        else if (flag)
          this._font.Draw("    ON", this.x + num1, this.y + num2, Color.White, this.depth);
        else
          this._font.Draw("   OFF", this.x + num1, this.y + num2, Color.White, this.depth);
      }
      else
      {
        this._font.Draw("ON", this.x + num1, this.y + num2, flag ? Color.White : new Color(70, 70, 70), this.depth);
        this._font.Draw("   OFF", this.x + num1, this.y + num2, !flag ? Color.White : new Color(70, 70, 70), this.depth);
      }
    }
  }
}
