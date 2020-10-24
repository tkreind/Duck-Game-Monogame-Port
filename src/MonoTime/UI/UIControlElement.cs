// Decompiled with JetBrains decompiler
// Type: DuckGame.UIControlElement
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIControlElement : UIMenuItem
  {
    private FieldBinding _field;
    private List<string> _captionList = new List<string>();
    private bool _editing;
    private bool _skipStep;
    public int randomAssIntField;
    public DeviceInputMapping inputMapping;
    private Sprite _styleBubble;
    private Sprite _styleTray;
    private string _realTrigger;
    private bool _selectStyle;
    private int _selectionIndex;
    private UIText _uiText;

    public string _trigger
    {
      set => this._realTrigger = value;
      get
      {
        if (this._realTrigger == "LSTICK" && this.inputMapping != null && this.inputMapping.device is Keyboard)
          return "CHAT";
        return this._realTrigger == "RTRIGGER" && this.inputMapping != null && this.inputMapping.device is Keyboard ? "PLAYERINDEX" : this._realTrigger;
      }
    }

    public UIControlElement(
      string text,
      string trigger,
      DeviceInputMapping map,
      UIMenuAction action = null,
      FieldBinding field = null,
      Color c = default (Color))
      : base(action)
    {
      this._trigger = trigger;
      if (c == new Color())
        c = Colors.MenuOption;
      BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
      UIDivider uiDivider = new UIDivider(true, 0.0f);
      this._uiText = new UIText(text, c);
      this._uiText.SetFont(f);
      this._uiText.align = UIAlign.Left;
      uiDivider.leftSection.Add((UIComponent) this._uiText, true);
      UIMultiToggle uiMultiToggle = new UIMultiToggle(-1f, -1f, new FieldBinding((object) this, nameof (randomAssIntField)), this._captionList, true);
      uiMultiToggle.SetFont(f);
      uiMultiToggle.align = UIAlign.Right;
      uiDivider.rightSection.Add((UIComponent) uiMultiToggle, true);
      this.rightSection.Add((UIComponent) uiDivider, true);
      this._arrow = new UIImage("littleContextArrowRight");
      this._arrow.align = UIAlign.Right;
      this._arrow.visible = false;
      this.leftSection.Add((UIComponent) this._arrow, true);
      this._styleBubble = new Sprite("buttons/styleBubble");
      this._styleBubble.center = new Vec2(0.0f, 11f);
      this._styleTray = new Sprite("buttons/styleTray");
      this._styleTray.CenterOrigin();
      this._field = field;
      this.inputMapping = map;
    }

    public override void Update()
    {
      this._captionList.Clear();
      if (!this._editing)
      {
        string str = !(this.inputMapping.device is Keyboard) || !(this._trigger == "LSTICK") && !(this._trigger == "RSTICK") && (!(this._trigger == "LTRIGGER") && !(this._trigger == "RTRIGGER")) ? (this._trigger == "LSTICK" || this._trigger == "RSTICK" || (this._trigger == "LTRIGGER" || this._trigger == "RTRIGGER") ? "|DGYELLOW|" : "|WHITE|") : "|GRAY|";
        if (this._trigger == "LSTICK")
        {
          this._uiText.text = "|DGGREEN|L STICK";
          if (this.inputMapping.device is Keyboard)
            this._uiText.text = "|GRAY|L STICK";
        }
        if (this._trigger == "RSTICK")
        {
          this._uiText.text = "|DGGREEN|R STICK";
          if (this.inputMapping.device is Keyboard)
            this._uiText.text = "|GRAY|R STICK";
        }
        if (this._trigger == "LTRIGGER")
        {
          this._uiText.text = "|DGGREEN|L TRIGGER";
          if (this.inputMapping.device is Keyboard)
            this._uiText.text = "|GRAY|L TRIGGER";
        }
        if (this._trigger == "RTRIGGER")
        {
          this._uiText.text = "|DGGREEN|R TRIGGER";
          if (this.inputMapping.device is Keyboard)
            this._uiText.text = "|GRAY|R TRIGGER";
        }
        string mappingString = this.inputMapping.GetMappingString(this._trigger);
        if (this._trigger == "CHAT")
          this._uiText.text = "|PINK|CHAT   ";
        if (this._trigger == "PLAYERINDEX")
        {
          this._uiText.text = "|LIME|PLAYER#  ";
          if (this.inputMapping.device.productName == "KEYBOARD P1")
            mappingString = (Options.Data.keyboard1PlayerIndex + 1).ToString();
          else if (this.inputMapping.device.productName == "KEYBOARD P2")
            mappingString = (Options.Data.keyboard2PlayerIndex + 1).ToString();
        }
        this._captionList.Add(str + mappingString);
      }
      else
      {
        if (this._skipStep)
        {
          this._skipStep = false;
          return;
        }
        if (!this._selectStyle)
        {
          this._captionList.Add("_");
          if (Keyboard.Pressed(Keys.Escape))
          {
            this._editing = false;
            UIMenu.globalUILock = false;
            HUD.CloseAllCorners();
          }
          else if (this.inputMapping.RunMappingUpdate(this._trigger))
          {
            this._editing = false;
            UIMenu.globalUILock = false;
            HUD.CloseAllCorners();
            if (!(this.inputMapping.deviceName != "KEYBOARD P1") || !(this.inputMapping.deviceName != "KEYBOARD P1"))
              return;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "STYLE@GRAB@");
            return;
          }
        }
        else
        {
          bool flag = false;
          if (Input.Pressed("LEFT"))
          {
            --this._selectionIndex;
            SFX.Play("textLetter", 0.7f);
          }
          if (Input.Pressed("RIGHT"))
          {
            ++this._selectionIndex;
            SFX.Play("textLetter", 0.7f);
          }
          if (Input.Pressed("UP"))
          {
            this._selectionIndex -= 4;
            SFX.Play("textLetter", 0.7f);
          }
          if (Input.Pressed("DOWN"))
          {
            this._selectionIndex += 4;
            SFX.Play("textLetter", 0.7f);
          }
          if (this._selectionIndex < 0)
            this._selectionIndex = 0;
          if (this._selectionIndex >= Input.buttonStyles.Count)
            this._selectionIndex = Input.buttonStyles.Count - 1;
          if (Input.Pressed("QUACK"))
          {
            flag = true;
            SFX.Play("consoleError");
          }
          if (Input.Pressed("SELECT"))
          {
            flag = true;
            int key = -1;
            if (this.inputMapping.map.TryGetValue(this._trigger, out key))
            {
              this.inputMapping.graphicMap[key] = Input.buttonStyles[this._selectionIndex].texture.textureName;
              SFX.Play("consoleSelect");
            }
          }
          if (flag)
          {
            this._editing = false;
            this._selectStyle = false;
            UIMenu.globalUILock = false;
            HUD.CloseAllCorners();
            if (!(this.inputMapping.deviceName != "KEYBOARD P1") || !(this.inputMapping.deviceName != "KEYBOARD P1"))
              return;
            HUD.AddCornerControl(HUDCorner.BottomLeft, "STYLE@GRAB@");
            return;
          }
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      if (this._arrow.visible)
      {
        this._styleBubble.depth = new Depth(0.9f);
        Vec2 vec2_1 = new Vec2(this.x + 90f, this.y);
        if (this._selectStyle)
        {
          vec2_1 = new Vec2(this.x + 85f, this.y);
          this._styleBubble.flipH = true;
        }
        else
          this._styleBubble.flipH = false;
        Graphics.Draw(this._styleBubble, vec2_1.x, vec2_1.y);
        if (this.inputMapping.map.ContainsKey(this._trigger))
        {
          Sprite g = this.inputMapping.GetSprite(this.inputMapping.map[this._trigger]) ?? this.inputMapping.device.DoGetMapImage(this.inputMapping.map[this._trigger], true);
          if (g != null)
          {
            g.depth = new Depth(0.95f);
            Graphics.Draw(g, vec2_1.x + (this._selectStyle ? -22f : 9f), vec2_1.y - 7f);
          }
        }
        if (this._selectStyle)
        {
          this._styleTray.depth = new Depth(0.92f);
          Graphics.Draw(this._styleTray, this.x + 118f, Layer.HUD.camera.height / 2f);
          Vec2 vec2_2 = new Vec2(this.x + 90f, (float) ((double) Layer.HUD.camera.height / 2.0 - 80.0));
          int num = 0;
          foreach (Sprite buttonStyle in Input.buttonStyles)
          {
            Vec2 vec2_3 = vec2_2 + new Vec2((float) (num % 4 * 14), (float) (num / 4 * 14));
            buttonStyle.depth = new Depth(0.95f);
            buttonStyle.color = Color.White * (num == this._selectionIndex ? 1f : 0.4f);
            Graphics.Draw(buttonStyle, vec2_3.x, vec2_3.y);
            ++num;
          }
        }
      }
      base.Draw();
    }

    public override void Activate(string trigger)
    {
      if (trigger == "RIGHT")
      {
        if (this._trigger == "PLAYERINDEX")
        {
          if (this.inputMapping.device.productName == "KEYBOARD P1")
          {
            ++Options.Data.keyboard1PlayerIndex;
            if (Options.Data.keyboard1PlayerIndex > 3)
              Options.Data.keyboard1PlayerIndex = 0;
          }
          else if (this.inputMapping.device.productName == "KEYBOARD P2")
          {
            ++Options.Data.keyboard2PlayerIndex;
            if (Options.Data.keyboard2PlayerIndex > 3)
              Options.Data.keyboard2PlayerIndex = 0;
          }
          SFX.Play("consoleSelect");
        }
      }
      else if (trigger == "LEFT" && this._trigger == "PLAYERINDEX")
      {
        if (this.inputMapping.device.productName == "KEYBOARD P1")
        {
          --Options.Data.keyboard1PlayerIndex;
          if (Options.Data.keyboard1PlayerIndex < 0)
            Options.Data.keyboard1PlayerIndex = 3;
        }
        else if (this.inputMapping.device.productName == "KEYBOARD P2")
        {
          --Options.Data.keyboard2PlayerIndex;
          if (Options.Data.keyboard2PlayerIndex < 0)
            Options.Data.keyboard2PlayerIndex = 3;
        }
        SFX.Play("consoleSelect");
      }
      if (trigger == "SELECT")
      {
        if (this.inputMapping.device is Keyboard && (this._trigger == "LSTICK" || this._trigger == "RSTICK" || (this._trigger == "LTRIGGER" || this._trigger == "RTRIGGER")))
          SFX.Play("consoleError");
        else if (this._trigger == "PLAYERINDEX")
        {
          if (this.inputMapping.device.productName == "KEYBOARD P1")
          {
            ++Options.Data.keyboard1PlayerIndex;
            if (Options.Data.keyboard1PlayerIndex > 3)
              Options.Data.keyboard1PlayerIndex = 0;
          }
          else if (this.inputMapping.device.productName == "KEYBOARD P2")
          {
            ++Options.Data.keyboard2PlayerIndex;
            if (Options.Data.keyboard2PlayerIndex > 3)
              Options.Data.keyboard2PlayerIndex = 0;
          }
          SFX.Play("consoleSelect");
        }
        else
        {
          UIMenu.globalUILock = true;
          this._editing = true;
          this._skipStep = true;
          SFX.Play("consoleSelect");
          HUD.CloseAllCorners();
          HUD.AddCornerControl(HUDCorner.TopLeft, "CANCEL@ESCAPE@");
        }
      }
      else
      {
        if (!(trigger == "GRAB") || !(this.inputMapping.deviceName != "KEYBOARD P1") || !(this.inputMapping.deviceName != "KEYBOARD P2"))
          return;
        this._selectStyle = true;
        UIMenu.globalUILock = true;
        this._editing = true;
        this._skipStep = true;
        int mapping = -1;
        if (this.inputMapping.map.TryGetValue(this._trigger, out mapping))
        {
          int num = 0;
          Sprite sprite = this.inputMapping.GetSprite(mapping);
          if (sprite != null)
          {
            foreach (Sprite buttonStyle in Input.buttonStyles)
            {
              if (sprite.texture != null && sprite.texture.textureName == buttonStyle.texture.textureName)
              {
                this._selectionIndex = num;
                break;
              }
              ++num;
            }
          }
        }
        HUD.CloseAllCorners();
        HUD.AddCornerControl(HUDCorner.TopLeft, "CANCEL@QUACK@");
        HUD.AddCornerControl(HUDCorner.BottomLeft, "SELECT@SELECT@");
      }
    }
  }
}
