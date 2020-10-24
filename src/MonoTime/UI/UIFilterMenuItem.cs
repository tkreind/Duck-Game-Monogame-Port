// Decompiled with JetBrains decompiler
// Type: DuckGame.UIFilterMenuItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UIFilterMenuItem : UIMenuItem
  {
    public UIFilterMenuItem(UIMenuAction action = null, UIAlign al = UIAlign.Center, Color c = default (Color), bool backButton = false)
      : base("AAAAAAAAAAAAAAAAAA", action, al, c, backButton)
    {
    }

    public override void Update()
    {
      string text = this._textElement.text;
      int num = 0;
      foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
      {
        if (matchSetting.filtered)
          ++num;
      }
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Any))
      {
        if (unlock.enabled && unlock.filtered)
          ++num;
      }
      if (num == 0)
        this._textElement.text = "|DGBLUE|NO FILTERS";
      else
        this._textElement.text = "|DGYELLOW|FILTERS: " + num.ToString();
      if (this._textElement.text != text)
      {
        this._textElement.Resize();
        this._dirty = true;
        this.rightSection.Resize();
      }
      base.Update();
    }
  }
}
