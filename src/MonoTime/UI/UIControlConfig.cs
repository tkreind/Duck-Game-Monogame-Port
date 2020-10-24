// Decompiled with JetBrains decompiler
// Type: DuckGame.UIControlConfig
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIControlConfig : UIMenu
  {
    public int playerSelected;
    public int inputMode;
    public int inputConfigType;
    private List<UIBox> _playerBoxes = new List<UIBox>();
    private UIMenuItemToggle _configuringToggle;
    private List<string> inputTypes = new List<string>()
    {
      "GAMEPAD",
      "KEYBOARD"
    };
    private List<DeviceInputMapping> inputMaps = new List<DeviceInputMapping>();
    public UIMenu _confirmMenu;
    private UIBox _controlBox;
    private UIMenu _openOnClose;
    private List<UIControlElement> _controlElements = new List<UIControlElement>();
    private bool _showingMenu;

    public void SwitchPlayerProfile()
    {
      this.inputTypes.Clear();
      this.inputMaps.Clear();
      for (int index = 0; index < 4; ++index)
      {
        XInputPad device = Input.GetDevice<XInputPad>(index);
        if (device != null && device.isConnected)
        {
          this.inputTypes.Add("XBOX GAMEPAD");
          this.inputMaps.Add(Input.GetDefaultMapping(device.productName, device.productGUID).Clone());
          break;
        }
      }
      List<string> stringList = new List<string>();
      for (int index = 0; index < 8; ++index)
      {
        if (DInput.GetState(index) != null)
        {
          string productName = DInput.GetProductName(index);
          string productGuid = DInput.GetProductGUID(index);
          string str = productName + productGuid;
          if (!stringList.Contains(str))
          {
            stringList.Add(str);
            this.inputMaps.Add(Input.GetDefaultMapping(productName, productGuid).Clone());
            if (productName.Length > 24)
              productName = productName.Substring(0, 24);
            this.inputTypes.Add(productName);
          }
        }
      }
      this.inputTypes.Add("KEYBOARD P1");
      this.inputMaps.Add(Input.GetDefaultMapping("KEYBOARD P1", "").Clone());
      this.inputTypes.Add("KEYBOARD P2");
      this.inputMaps.Add(Input.GetDefaultMapping("KEYBOARD P2", "").Clone());
      this.inputConfigType = 0;
      this.SwitchConfigType();
    }

    public void SwitchConfigType()
    {
      foreach (UIControlElement controlElement in this._controlElements)
      {
        if (this.inputConfigType < this.inputMaps.Count)
          controlElement.inputMapping = this.inputMaps[this.inputConfigType];
      }
    }

    public void ResetToDefault()
    {
      if (this.inputConfigType < this.inputMaps.Count)
        this.inputMaps[this.inputConfigType] = Input.GetDefaultMapping(this.inputMaps[this.inputConfigType].deviceName, this.inputMaps[this.inputConfigType].deviceGUID, true).Clone();
      this.SwitchConfigType();
    }

    public void CloseMenu()
    {
      this._showingMenu = false;
      this.Close();
      this._openOnClose.Open();
      this._confirmMenu.Close();
      this.inputMaps.Clear();
      HUD.CloseAllCorners();
    }

    public void CloseMenuSaving()
    {
      this._showingMenu = false;
      foreach (DeviceInputMapping inputMap in this.inputMaps)
        Input.SetDefaultMapping(inputMap);
      Input.ApplyDefaultMappings();
      Input.Save();
      this.Close();
      this._openOnClose.Open();
      this._confirmMenu.Close();
      this.inputMaps.Clear();
      HUD.CloseAllCorners();
    }

    public UIControlConfig(
      UIMenu openOnClose,
      string title,
      float xpos,
      float ypos,
      float wide = -1f,
      float high = -1f,
      string conString = "",
      InputProfile conProfile = null)
      : base(title, xpos, ypos, wide, high, conString, conProfile)
    {
      this._openOnClose = openOnClose;
      List<string> stringList1 = new List<string>()
      {
        "P1   ",
        "P2   ",
        "P3   ",
        "P4"
      };
      List<string> stringList2 = new List<string>()
      {
        "GAMEPAD",
        "KEYBOARD",
        "PAD + KEYS"
      };
      BitmapFont bitmapFont = new BitmapFont("smallBiosFontUI", 7, 5);
      UIBox uiBox = new UIBox(isVisible: false);
      this._configuringToggle = new UIMenuItemToggle("", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.SwitchConfigType)), new FieldBinding((object) this, nameof (inputConfigType)), multi: this.inputTypes, compressedMulti: true, tiny: true);
      uiBox.Add((UIComponent) this._configuringToggle, true);
      UIText uiText1 = new UIText(" ", Color.White);
      this._controlElements.Add(new UIControlElement("|DGBLUE|{LEFT", "LEFT", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|/RIGHT", "RIGHT", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|}UP", "UP", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|~DOWN", "DOWN", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|START", "START", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|ACCEPT", "SELECT", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|JUMP", "JUMP", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|GRAB", "GRAB", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|TRIP", "RAGDOLL", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|FIRE", "SHOOT", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|STRAFE", "STRAFE", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGBLUE|QUACK", "QUACK", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGGREEN|L STICK", "LSTICK", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGGREEN|R STICK", "RSTICK", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGGREEN|L TRIGGER", "LTRIGGER", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      this._controlElements.Add(new UIControlElement("|DGGREEN|R TRIGGER", "RTRIGGER", new DeviceInputMapping(), field: new FieldBinding((object) Options.Data, "sfxVolume")));
      uiBox.Add((UIComponent) this._controlElements[this._controlElements.Count - 1], true);
      UIMenuItem uiMenuItem = new UIMenuItem("|RED|REVERT TO DEFAULT", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.ResetToDefault)));
      uiMenuItem.SetFont(bitmapFont);
      uiBox.Add((UIComponent) uiMenuItem, true);
      UIText uiText2 = new UIText(" ", Color.White);
      uiText2.SetFont(bitmapFont);
      uiBox.Add((UIComponent) uiText2, true);
      UIText uiText3 = new UIText("PERSONAL CONTROLS CAN BE", Color.White);
      uiText3.SetFont(bitmapFont);
      uiBox.Add((UIComponent) uiText3, true);
      UIText uiText4 = new UIText("SET IN PROFILE SCREEN", Color.White);
      uiText4.SetFont(bitmapFont);
      uiBox.Add((UIComponent) uiText4, true);
      this._controlBox = uiBox;
      this._playerBoxes.Add(uiBox);
      this.Add((UIComponent) this._playerBoxes[0], true);
      this._confirmMenu = new UIMenu("SAVE CHANGES?", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@SELECT@SELECT");
      this._confirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.CloseMenuSaving))), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(this.CloseMenu))), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("CANCEL!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) this._confirmMenu, (UIComponent) this)), true);
      this._confirmMenu.Close();
    }

    public override void Open()
    {
      this.SwitchPlayerProfile();
      base.Open();
    }

    public override void Update()
    {
      if (this.open)
      {
        if (!UIMenu.globalUILock && (Input.Pressed("QUACK") || Keyboard.Pressed(Keys.Escape)))
        {
          new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._confirmMenu).Activate();
          return;
        }
        if (Input.uiDevicesHaveChanged)
        {
          this.SwitchPlayerProfile();
          Input.uiDevicesHaveChanged = false;
        }
      }
      if (this._controlBox.selection > 0 && this._controlBox.selection < 17)
      {
        if (!this._showingMenu && this.inputConfigType < this.inputMaps.Count && (this.inputMaps[this.inputConfigType].deviceName != "KEYBOARD P1" && this.inputMaps[this.inputConfigType].deviceName != "KEYBOARD P2"))
        {
          HUD.AddCornerControl(HUDCorner.BottomLeft, "STYLE@GRAB@");
          this._showingMenu = true;
        }
      }
      else if (this._showingMenu)
      {
        HUD.CloseAllCorners();
        this._showingMenu = false;
      }
      base.Update();
    }
  }
}
