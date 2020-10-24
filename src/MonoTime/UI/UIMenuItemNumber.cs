// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenuItemNumber
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIMenuItemNumber : UIMenuItem
  {
    private FieldBinding _field;
    private int _step;
    private FieldBinding _upperBoundField;
    private FieldBinding _lowerBoundField;
    private FieldBinding _filterField;
    private UIText _textItem;
    public List<FieldBinding> percentageGroup = new List<FieldBinding>();
    private List<string> _valueStrings;
    private MatchSetting _setting;

    public UIMenuItemNumber(
      string text,
      UIMenuAction action = null,
      FieldBinding field = null,
      int step = 1,
      Color c = default (Color),
      FieldBinding upperBoundField = null,
      FieldBinding lowerBoundField = null,
      string append = "",
      FieldBinding filterField = null,
      List<string> valStrings = null,
      MatchSetting setting = null)
      : base(action)
    {
      this._setting = setting;
      if (c == new Color())
        c = Colors.MenuOption;
      this._valueStrings = valStrings;
      UIDivider uiDivider = new UIDivider(true, this._valueStrings != null ? 0.0f : 0.8f);
      UIText uiText = new UIText(text, c);
      uiText.align = UIAlign.Left;
      uiDivider.leftSection.Add((UIComponent) uiText, true);
      if (this._valueStrings != null)
      {
        if (text == "" || text == null)
        {
          uiDivider.leftSection.align = UIAlign.Left;
          this._textItem = uiText;
          int index = (int) field.value;
          if (index >= 0 && index < this._valueStrings.Count)
            this._textItem.text = this._valueStrings[index];
        }
        else
        {
          this._textItem = (UIText) new UIChangingText(-1f, -1f, field, (FieldBinding) null);
          int index = (int) field.value;
          if (index >= 0 && index < this._valueStrings.Count)
            this._textItem.text = this._valueStrings[index];
          this._textItem.align = UIAlign.Right;
          uiDivider.rightSection.Add((UIComponent) this._textItem, true);
        }
      }
      else
      {
        UINumber uiNumber = new UINumber(-1f, -1f, field, append, filterField, this._setting);
        uiNumber.align = UIAlign.Right;
        uiDivider.rightSection.Add((UIComponent) uiNumber, true);
      }
      this.rightSection.Add((UIComponent) uiDivider, true);
      this._arrow = new UIImage("contextArrowRight");
      this._arrow.align = UIAlign.Right;
      this._arrow.visible = false;
      this.leftSection.Add((UIComponent) this._arrow, true);
      this._field = field;
      this._step = step;
      this._upperBoundField = upperBoundField;
      this._lowerBoundField = lowerBoundField;
      this._filterField = filterField;
    }

    private int GetStep(int current, bool up)
    {
      if (this._setting == null || this._setting.stepMap == null)
        return this._step;
      int num = 0;
      foreach (KeyValuePair<int, int> step in this._setting.stepMap)
      {
        num = step.Value;
        if (up)
        {
          if (step.Key > current)
            break;
        }
        if (!up)
        {
          if (step.Key >= current)
            break;
        }
      }
      return num;
    }

    public override void Activate(string trigger)
    {
      if (this._filterField != null)
      {
        if (!(bool) this._filterField.value && trigger == "RIGHT")
        {
          SFX.Play("textLetter", 0.7f);
          this._filterField.value = (object) true;
          this._field.value = (object) (int) this._field.min;
          return;
        }
        if ((bool) this._filterField.value && trigger == "LEFT" && (double) (int) this._field.value == (double) this._field.min)
        {
          SFX.Play("textLetter", 0.7f);
          this._filterField.value = (object) false;
          return;
        }
        if (this._setting != null && trigger == "GRAB")
        {
          SFX.Play("textLetter", 0.7f);
          if (this._setting.filterMode == FilterMode.GreaterThan)
          {
            this._setting.filterMode = FilterMode.Equal;
            return;
          }
          if (this._setting.filterMode == FilterMode.Equal)
          {
            this._setting.filterMode = FilterMode.LessThan;
            return;
          }
          if (this._setting.filterMode != FilterMode.LessThan)
            return;
          this._setting.filterMode = FilterMode.GreaterThan;
          return;
        }
      }
      int num1 = (int) this._field.value;
      if (trigger == "LEFT")
        this._field.value = (object) ((int) this._field.value - this.GetStep((int) this._field.value, false));
      else if (trigger == "RIGHT")
        this._field.value = (object) ((int) this._field.value + this.GetStep((int) this._field.value, true));
      int index = (int) Maths.Clamp((float) (int) this._field.value, this._field.min, this._field.max);
      if (this._upperBoundField != null && index > (int) this._upperBoundField.value)
        this._upperBoundField.value = (object) index;
      if (this._lowerBoundField != null && index < (int) this._lowerBoundField.value)
        this._lowerBoundField.value = (object) index;
      if (num1 != (int) this._field.value)
        SFX.Play("textLetter", 0.7f);
      int num2 = index - num1;
      this._field.value = (object) index;
      if (num2 > 0)
      {
        int num3 = num2;
        using (List<FieldBinding>.Enumerator enumerator = this.percentageGroup.GetEnumerator())
        {
label_29:
          while (enumerator.MoveNext())
          {
            FieldBinding current = enumerator.Current;
            while (true)
            {
              if ((double) (int) current.value > (double) current.min && num3 > 0)
              {
                int num4 = (int) current.value - (int) current.inc;
                current.value = (object) num4;
                num3 -= (int) current.inc;
              }
              else
                goto label_29;
            }
          }
        }
      }
      else if (num2 < 0)
      {
        int num3 = num2;
        using (List<FieldBinding>.Enumerator enumerator = this.percentageGroup.GetEnumerator())
        {
label_37:
          while (enumerator.MoveNext())
          {
            FieldBinding current = enumerator.Current;
            while (true)
            {
              if ((double) (int) current.value < (double) current.max && num3 < 0)
              {
                int num4 = (int) current.value + (int) current.inc;
                current.value = (object) num4;
                num3 += (int) current.inc;
              }
              else
                goto label_37;
            }
          }
        }
      }
      if (this._textItem == null || index < 0 || index >= this._valueStrings.Count)
        return;
      this._textItem.text = this._valueStrings[index];
    }
  }
}
