// Decompiled with JetBrains decompiler
// Type: DuckGame.UIMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIMenu : UIBox
  {
    public static bool globalUILock;
    protected UIDivider _splitter;
    private UIBox _section;
    private UIText _controlText;
    private string _controlString;
    private InputProfile _controlProfile;

    public UIMenu(
      string title,
      float xpos,
      float ypos,
      float wide = -1f,
      float high = -1f,
      string conString = "",
      InputProfile conProfile = null,
      bool tiny = false)
      : base(xpos, ypos, wide, high)
    {
      this._controlProfile = conProfile;
      this._splitter = new UIDivider(false, 0.0f, 4f);
      this._section = this._splitter.rightSection;
      UIText uiText = new UIText(title, Color.White);
      if (tiny)
      {
        BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
        uiText.SetFont(f);
      }
      uiText.align |= UIAlign.Top;
      this._splitter.topSection.Add((UIComponent) uiText, true);
      this._controlString = conString;
      if (this._controlString != "" && this._controlString != null)
      {
        UIDivider uiDivider = new UIDivider(false, 0.0f, 4f);
        this._controlText = new UIText(this._controlString, Color.White, heightAdd: 4f, controlProfile: this._controlProfile);
        uiDivider.bottomSection.Add((UIComponent) this._controlText, true);
        this.Add((UIComponent) uiDivider, true);
        this._section = uiDivider.topSection;
      }
      base.Add((UIComponent) this._splitter, true);
    }

    public string title
    {
      get => ((UIText) this._splitter.topSection.components[0]).text;
      set => ((UIText) this._splitter.topSection.components[0]).text = value;
    }

    public override void SelectLastMenuItem() => this._section.SelectLastMenuItem();

    public override void Add(UIComponent component, bool doAnchor = true)
    {
      this._section.Add(component, doAnchor);
      this._dirty = true;
    }

    public override void Insert(UIComponent component, int position, bool doAnchor = true)
    {
      this._section.Insert(component, position, doAnchor);
      this._dirty = true;
    }

    public override void Update()
    {
      if (this._controlText != null)
        this._controlText.text = this._section._hoverControlString != null ? this._section._hoverControlString : this._controlString;
      base.Update();
    }

    public void AddMatchSetting(MatchSetting m, bool filterMenu, bool enabled = true)
    {
      UIComponent component = (UIComponent) null;
      if (m.value is int)
      {
        FieldBinding upperBoundField = (FieldBinding) null;
        if (m.maxSyncID != null)
        {
          foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
          {
            if (matchSetting.id == m.maxSyncID)
              upperBoundField = new FieldBinding((object) matchSetting, "value");
          }
        }
        FieldBinding lowerBoundField = (FieldBinding) null;
        if (m.minSyncID != null)
        {
          foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
          {
            if (matchSetting.id == m.minSyncID)
              lowerBoundField = new FieldBinding((object) matchSetting, "value");
          }
        }
        component = (UIComponent) new UIMenuItemNumber(m.name, field: new FieldBinding((object) m, "value", (float) m.min, (float) m.max), step: m.step, upperBoundField: upperBoundField, lowerBoundField: lowerBoundField, append: m.suffix, filterField: (filterMenu ? new FieldBinding((object) m, "filtered") : (FieldBinding) null), valStrings: m.valueStrings, setting: m);
        if (m.percentageLinks != null)
        {
          foreach (string percentageLink in m.percentageLinks)
          {
            MatchSetting matchSetting = TeamSelect2.GetMatchSetting(percentageLink);
            (component as UIMenuItemNumber).percentageGroup.Add(new FieldBinding((object) matchSetting, "value", (float) matchSetting.min, (float) matchSetting.max, (float) matchSetting.step));
          }
        }
      }
      else if (m.value is bool)
        component = (UIComponent) new UIMenuItemToggle(m.name, field: new FieldBinding((object) m, "value"), filterBinding: (filterMenu ? new FieldBinding((object) m, "filtered") : (FieldBinding) null));
      if (component == null)
        return;
      component.isEnabled = enabled;
      this._section.Add(component, true);
      this._dirty = true;
    }

    public override void Remove(UIComponent component)
    {
      this._section.Remove(component);
      this._dirty = true;
    }
  }
}
