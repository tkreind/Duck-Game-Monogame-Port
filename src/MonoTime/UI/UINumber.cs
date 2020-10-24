// Decompiled with JetBrains decompiler
// Type: DuckGame.UINumber
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UINumber : UIText
  {
    private FieldBinding _field;
    private string _append = "";
    private FieldBinding _filterField;
    private MatchSetting _setting;

    public UINumber(
      float wide,
      float high,
      FieldBinding field,
      string append = "",
      FieldBinding filterField = null,
      MatchSetting setting = null)
      : base("0", Color.White)
    {
      this._setting = setting;
      this._field = field;
      this._append = append;
      this._filterField = filterField;
    }

    public override void Draw()
    {
      string str = "";
      if (this._setting != null && this._filterField != null)
      {
        if (this._setting.filterMode == FilterMode.GreaterThan)
          str = ">=";
        else if (this._setting.filterMode == FilterMode.LessThan)
          str = "<=";
      }
      if (this._setting != null && this._field.value is int && ((int) this._field.value == this._setting.min && this._setting.minString != null))
      {
        this._text = this._setting.minString;
      }
      else
      {
        this._text = str + Change.ToString((object) (int) this._field.value) + this._append;
        if (this._filterField != null && !(bool) this._filterField.value)
          this._text = "|DGGREEN|ANY";
      }
      base.Draw();
    }
  }
}
