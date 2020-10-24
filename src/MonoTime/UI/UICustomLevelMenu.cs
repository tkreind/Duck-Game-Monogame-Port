// Decompiled with JetBrains decompiler
// Type: DuckGame.UICustomLevelMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UICustomLevelMenu : UIMenuItem
  {
    public UICustomLevelMenu(UIMenuAction action = null, UIAlign al = UIAlign.Center, Color c = default (Color), bool backButton = false)
      : base("AAAAAAAAAAAAAAAAAA", action, al, c, backButton)
    {
    }

    public override void Update()
    {
      string text = this._textElement.text;
      int num = 0;
      foreach (string activatedLevel in Editor.activatedLevels)
        ++num;
      if (num == 0)
        this._textElement.text = "|DGBLUE|NO CUSTOM";
      else
        this._textElement.text = "|DGYELLOW|CUSTOM: " + num.ToString();
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
