// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuItemSlider
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenuItemSlider : UIMenuItem
  {
    private FieldBinding _field;
    private float _step;

    public UIMenuItemSlider(
      string text,
      UIMenuAction action = null,
      FieldBinding field = null,
      float step = 0.1f,
      Color c = default (Color))
      : base(action)
    {
      if (c == new Color())
        c = Colors.MenuOption;
      UIDivider uiDivider = new UIDivider(true, 0.0f);
      UIText uiText = new UIText(text, c);
      uiText.align = UIAlign.Left;
      uiDivider.leftSection.Add((UIComponent) uiText, true);
      UIProgressBar uiProgressBar = new UIProgressBar(30f, 7f, field, step);
      uiProgressBar.align = UIAlign.Right;
      uiDivider.rightSection.Add((UIComponent) uiProgressBar, true);
      this.rightSection.Add((UIComponent) uiDivider, true);
      this._arrow = new UIImage("contextArrowRight");
      this._arrow.align = UIAlign.Right;
      this._arrow.visible = false;
      this.leftSection.Add((UIComponent) this._arrow, true);
      this._field = field;
      this._step = step;
    }

    public override void Activate(string trigger)
    {
      float num;
      if (trigger == "LEFT")
      {
        num = Maths.Clamp((float) this._field.value - this._step, this._field.min, this._field.max);
      }
      else
      {
        if (!(trigger == "RIGHT"))
          return;
        num = Maths.Clamp((float) this._field.value + this._step, this._field.min, this._field.max);
      }
      if ((double) num != (double) (float) this._field.value)
        SFX.Play("textLetter", 0.7f);
      this._field.value = (object) num;
    }
  }
}
