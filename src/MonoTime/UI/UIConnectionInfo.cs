// Decompiled with JetBrains decompiler
// Type: DuckGame.UIConnectionInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Linq;

namespace DuckGame
{
  public class UIConnectionInfo : UIMenuItem
  {
    private UIMenu _kickMenu;
    private UIMenu _rootMenu;
    private Profile _profile;
    private BitmapFont _littleFont;
    private string _nameText;

    public UIConnectionInfo(Profile p, UIMenu rootMenu, UIMenu kickMenu)
      : base(p.name)
    {
      this._profile = p;
      this._littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
      this.SetFont(this._littleFont);
      string str1 = "|WHITE|";
      if (p.networkIndex == (byte) 1)
        str1 = "|LIGHTGRAY|";
      else if (p.networkIndex == (byte) 2)
        str1 = "|DGYELLOW|";
      else if (p.networkIndex == (byte) 3)
        str1 = "|MENUORANGE|";
      string source1 = p.name;
      int num1 = source1.Count<char>();
      bool flag1 = false;
      if (p.connection != null && p.connection.isHost)
      {
        flag1 = true;
        ++num1;
      }
      int num2 = 17;
      if (flag1)
        num2 = 16;
      if (num1 > num2)
      {
        source1 = source1.Substring(0, num2 - 1) + ".";
        num1 = num2;
      }
      for (; num1 < num2 + 2; ++num1)
        source1 += " ";
      if (flag1)
        source1 = "@HOSTCROWN@" + source1;
      this._nameText = str1 + source1;
      bool flag2 = false;
      int num3;
      if (this._profile.connection != null)
      {
        num3 = (int) Math.Round((double) this._profile.connection.manager.ping * 1000.0);
        flag2 = this._profile.connection.status == ConnectionStatus.Connected;
      }
      else
        num3 = 1000;
      string source2 = num3.ToString() + "|WHITE|MS";
      int num4 = source2.Count<char>();
      string str2 = num3 >= 150 ? (num3 >= 250 ? (!flag2 ? "|DGRED|" + source2 + "@SIGNALDEAD@" : "|DGRED|" + source2 + "@SIGNALBAD@") : "|DGYELLOW|" + source2 + "@SIGNALNORMAL@") : "|DGGREEN|" + source2 + "@SIGNALGOOD@";
      for (; num4 < 5; ++num4)
        str2 = " " + str2;
      this._textElement.text = str1 + source1 + str2;
      if (Network.isServer && p.connection != DuckNetwork.localConnection)
        this.controlString = "@DPAD@MOVE @GRAB@KICK";
      this._kickMenu = kickMenu;
      this._rootMenu = rootMenu;
    }

    public override void Activate(string trigger)
    {
      if (Network.isServer && this._profile.connection != null && (this._profile.connection != DuckNetwork.localConnection && trigger == "GRAB"))
      {
        DuckNetwork.kickContext = this._profile;
        this._rootMenu.Close();
        this._kickMenu.Open();
        if (MonoMain.pauseMenu == this._rootMenu)
          MonoMain.pauseMenu = (UIComponent) this._kickMenu;
      }
      base.Activate(trigger);
    }

    public override void Draw()
    {
      this._textElement.text = "";
      this._littleFont.Draw(this._nameText, this.position + new Vec2(-88f, -3f), Color.White, this.depth + 10);
      bool flag = false;
      int num;
      if (this._profile.connection != null)
      {
        num = (int) Math.Round((double) this._profile.connection.manager.ping * 1000.0);
        flag = this._profile.connection.status == ConnectionStatus.Connected;
      }
      else
        num = 1000;
      string source = num.ToString() + "|WHITE|MS";
      source.Count<char>();
      string text = num >= 150 ? (num >= 250 ? (!flag ? "|DGRED|" + source + "@SIGNALDEAD@" : "|DGRED|" + source + "@SIGNALBAD@") : "|DGYELLOW|" + source + "@SIGNALNORMAL@") : "|DGGREEN|" + source + "@SIGNALGOOD@";
      this._littleFont.Draw(text, this.position + new Vec2(90f - this._littleFont.GetWidth(text), -3f), Color.White, this.depth + 10);
      base.Draw();
    }

    public override void Update() => base.Update();
  }
}
