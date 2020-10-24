// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuItemToggle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIMenuItemToggle : UIMenuItem
  {
    private FieldBinding _field;
    private FieldBinding _filterField;
    private bool _compressed;
    private List<string> _multiToggle;
    private UIMultiToggle _multiToggleElement;

    public void SetFieldBinding(FieldBinding f)
    {
      this._field = f;
      if (this._multiToggleElement == null)
        return;
      this._multiToggleElement.SetFieldBinding(f);
    }

    public UIMenuItemToggle(
      string text,
      UIMenuAction action = null,
      FieldBinding field = null,
      Color c = default (Color),
      FieldBinding filterBinding = null,
      List<string> multi = null,
      bool compressedMulti = false,
      bool tiny = false)
      : base(action)
    {
      if (c == new Color())
        c = Colors.MenuOption;
      BitmapFont f = (BitmapFont) null;
      if (tiny)
        f = new BitmapFont("smallBiosFontUI", 7, 5);
      UIDivider uiDivider = new UIDivider(true, 0.0f);
      if (text != "")
      {
        UIText uiText = new UIText(text, c);
        if (tiny)
          uiText.SetFont(f);
        uiText.align = UIAlign.Left;
        uiDivider.leftSection.Add((UIComponent) uiText, true);
      }
      if (multi != null)
      {
        this._multiToggleElement = new UIMultiToggle(-1f, -1f, field, multi, compressedMulti);
        this._multiToggleElement.align = compressedMulti ? UIAlign.Right : UIAlign.Right;
        if (text != "")
        {
          uiDivider.rightSection.Add((UIComponent) this._multiToggleElement, true);
        }
        else
        {
          uiDivider.leftSection.Add((UIComponent) this._multiToggleElement, true);
          this._multiToggleElement.align = UIAlign.Left;
        }
        if (tiny)
          this._multiToggleElement.SetFont(f);
        this._multiToggle = multi;
        this._compressed = compressedMulti;
      }
      else
      {
        UIOnOff uiOnOff = new UIOnOff(-1f, -1f, field, filterBinding);
        if (tiny)
          uiOnOff.SetFont(f);
        uiOnOff.align = UIAlign.Right;
        uiDivider.rightSection.Add((UIComponent) uiOnOff, true);
      }
      this.rightSection.Add((UIComponent) uiDivider, true);
      if (tiny)
        this._arrow = new UIImage("littleContextArrowRight");
      else
        this._arrow = new UIImage("contextArrowRight");
      this._arrow.align = UIAlign.Right;
      this._arrow.visible = false;
      this.leftSection.Add((UIComponent) this._arrow, true);
      this._field = field;
      this._filterField = filterBinding;
    }

    public override void Activate(string trigger)
    {
      if (this._filterField != null)
      {
        if (!(bool) this._filterField.value && trigger == "RIGHT")
        {
          SFX.Play("textLetter", 0.7f);
          this._filterField.value = (object) true;
          this._field.value = (object) false;
          return;
        }
        if ((bool) this._filterField.value && trigger == "LEFT" && !(bool) this._field.value)
        {
          SFX.Play("textLetter", 0.7f);
          this._filterField.value = (object) false;
          return;
        }
      }
      if (this._multiToggle != null)
      {
        if (trigger == "LEFT")
        {
          int num = (int) this._field.value - 1;
          if (num < 0)
            num = !this._compressed ? 0 : this._multiToggle.Count - 1;
          this._field.value = (object) num;
          SFX.Play("textLetter", 0.7f);
          if (this._action != null)
            this._action.Activate();
        }
        if (!(trigger == "RIGHT"))
          return;
        int num1 = (int) this._field.value + 1;
        if (num1 >= this._multiToggle.Count)
          num1 = !this._compressed ? this._multiToggle.Count - 1 : 0;
        this._field.value = (object) num1;
        SFX.Play("textLetter", 0.7f);
        if (this._action == null)
          return;
        this._action.Activate();
      }
      else
      {
        if (!(trigger == "LEFT") && !(trigger == "RIGHT") && !(trigger == "SELECT"))
          return;
        this._field.value = (object) !(bool) this._field.value;
        SFX.Play("textLetter", 0.7f);
      }
    }
  }
}
