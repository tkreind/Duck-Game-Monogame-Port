// Decompiled with JetBrains decompiler
// Type: DuckGame.UISlotEditor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class UISlotEditor : UIMenu
  {
    private UIMenu _closeMenu;
    private Rectangle _captureRectangle;
    private BitmapFont _littleFont;
    private BitmapFont _littleFont2;
    private int _slot;
    private Vec2 _rectPosition;
    public bool finished;
    private bool _selectionChanged = true;
    private bool _showWarning;
    private bool _showedWarning;
    public static int hoveringSlot = -1;

    public UISlotEditor(UIMenu closeMenu, float xpos, float ypos, float wide = -1f, float high = -1f)
      : base("", xpos, ypos, wide, high)
    {
      float num = 38f;
      this._captureRectangle = new Rectangle((float) (int) ((double) Layer.HUD.camera.width / 2.0 - (double) num / 2.0), (float) (int) ((double) Layer.HUD.camera.height / 2.0 - (double) num / 2.0), (float) (int) num, (float) (int) num);
      this._closeMenu = closeMenu;
      this._littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
      this._littleFont2 = new BitmapFont("smallBiosFont", 7, 6);
    }

    public override void Open()
    {
      HUD.CloseAllCorners();
      this._showedWarning = false;
      this._showWarning = false;
      HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@EXIT");
      MonoMain.doPauseFade = false;
      base.Open();
    }

    public override void Close()
    {
      HUD.CloseAllCorners();
      UISlotEditor.hoveringSlot = -1;
      MonoMain.doPauseFade = true;
      base.Close();
    }

    public override void Update()
    {
      if (this.open)
      {
        if (this._showWarning)
        {
          this._selectionChanged = true;
          if (Input.Pressed("QUACK"))
          {
            SFX.Play("consoleCancel");
            this._showWarning = false;
          }
          else if (Input.Pressed("GRAB"))
          {
            SFX.Play("death");
            this._showedWarning = true;
            this._showWarning = false;
            if (Level.core.gameInProgress)
            {
              Main.ResetGameStuff();
              Main.ResetMatchStuff();
              Level.core.gameInProgress = false;
              Send.Message((NetMessage) new NMResetGameSettings());
            }
          }
        }
        else
        {
          int slot = this._slot;
          if (Input.Pressed("LEFT") && (this._slot == 1 || this._slot == 3))
            --this._slot;
          if (Input.Pressed("RIGHT") && (this._slot == 0 || this._slot == 2))
            ++this._slot;
          if (Input.Pressed("UP") && (this._slot == 2 || this._slot == 3))
            this._slot -= 2;
          if (Input.Pressed("DOWN") && (this._slot == 0 || this._slot == 1))
            this._slot += 2;
          UISlotEditor.hoveringSlot = this._slot;
          if (this._slot != slot)
            this._selectionChanged = true;
          if (this._slot == 0)
            this._rectPosition = new Vec2(0.0f, 0.0f);
          else if (this._slot == 1)
            this._rectPosition = new Vec2(178f, 0.0f);
          else if (this._slot == 2)
            this._rectPosition = new Vec2(0.0f, 90f);
          else if (this._slot == 3)
            this._rectPosition = new Vec2(178f, 90f);
          if (this._selectionChanged)
          {
            if (DuckNetwork.profiles[this._slot].connection != null && DuckNetwork.profiles[this._slot].connection != DuckNetwork.localConnection)
            {
              HUD.CloseCorner(HUDCorner.TopLeft);
              HUD.AddCornerControl(HUDCorner.TopLeft, "@GRAB@KICK");
            }
            else
              HUD.CloseCorner(HUDCorner.TopLeft);
            if (DuckNetwork.profiles[this._slot].connection != DuckNetwork.localConnection)
            {
              HUD.CloseCorner(HUDCorner.BottomLeft);
              HUD.AddCornerControl(HUDCorner.BottomLeft, "TOGGLE MODE@SELECT@");
            }
            else
              HUD.CloseCorner(HUDCorner.BottomLeft);
            this._selectionChanged = false;
          }
          if (Input.Pressed("SELECT") && DuckNetwork.profiles[this._slot].connection != DuckNetwork.localConnection)
          {
            if (!this._showedWarning && Level.core.gameInProgress)
            {
              this._showWarning = true;
              SFX.Play("pause");
              HUD.CloseAllCorners();
              HUD.AddCornerControl(HUDCorner.BottomLeft, "YEAH OK!@GRAB@");
              HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@CANCEL");
            }
            else
            {
              int num = (int) (DuckNetwork.profiles[this._slot].slotType + 1);
              if (DuckNetwork.profiles[this._slot].reservedUser != null && num == 5)
                ++num;
              if (DuckNetwork.profiles[this._slot].reservedUser == null && num >= 5 || DuckNetwork.profiles[this._slot].reservedUser != null && num > 6)
                num = 0;
              DuckNetwork.profiles[this._slot].slotType = (SlotType) num;
              DuckNetwork.ChangeSlotSettings();
              SFX.Play("menuBlip01");
            }
          }
          else if (Input.Pressed("GRAB") && DuckNetwork.profiles[this._slot].connection != DuckNetwork.localConnection)
            DuckNetwork.Kick(DuckNetwork.profiles[this._slot]);
          else if (Input.Pressed("QUACK"))
          {
            SFX.Play("consoleCancel");
            new UIMenuActionOpenMenu((UIComponent) this, (UIComponent) this._closeMenu).Activate();
          }
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      if (!this.open)
        return;
      Vec2 rectPosition = this._rectPosition;
      if (Graphics.sixteenTen)
        rectPosition.y += Layer.HUD.barSize;
      Vec2 p1_1 = rectPosition;
      Graphics.DrawRect(p1_1, p1_1 + new Vec2(142f, 90f), Color.White, new Depth(0.8f), false);
      p1_1 = new Vec2(rectPosition.x - 200f, rectPosition.y - 200f);
      Graphics.DrawRect(p1_1, p1_1 + new Vec2(200f, 400f), Color.Black * 0.5f, new Depth(0.8f));
      p1_1 = new Vec2(rectPosition.x + 142f, rectPosition.y - 200f);
      Graphics.DrawRect(p1_1, p1_1 + new Vec2(200f, 400f), Color.Black * 0.5f, new Depth(0.8f));
      p1_1 = new Vec2(rectPosition.x, rectPosition.y + 90f);
      Graphics.DrawRect(p1_1, p1_1 + new Vec2(142f, 200f), Color.Black * 0.5f, new Depth(0.8f));
      p1_1 = new Vec2(rectPosition.x, rectPosition.y - 200f);
      Graphics.DrawRect(p1_1, p1_1 + new Vec2(142f, 200f), Color.Black * 0.5f, new Depth(0.8f));
      string text1 = "FRIENDS ONLY";
      if (DuckNetwork.profiles[this._slot].slotType == SlotType.Open)
        text1 = "OPEN SLOT";
      else if (DuckNetwork.profiles[this._slot].slotType == SlotType.Closed)
        text1 = "CLOSED SLOT";
      else if (DuckNetwork.profiles[this._slot].slotType == SlotType.Invite)
        text1 = "INVITE ONLY";
      else if (DuckNetwork.profiles[this._slot].slotType == SlotType.Friend)
        text1 = "FRIENDS ONLY";
      if (DuckNetwork.profiles[this._slot].connection == DuckNetwork.localConnection || DuckNetwork.profiles[this._slot].slotType == SlotType.Local)
        text1 = "LOCAL SLOT";
      Vec2 p1_2 = new Vec2(rectPosition.x + 1f, rectPosition.y + 1f);
      Graphics.DrawRect(p1_2, p1_2 + new Vec2(this._littleFont.GetWidth(text1) + 4f, 7f), Color.Black, new Depth(0.9f));
      this._littleFont.depth = new Depth(0.93f);
      this._littleFont.Draw(text1, p1_2 + new Vec2(1f, 1f), Color.White, new Depth(0.93f));
      if (!this._showWarning)
        return;
      Vec2 vec2 = new Vec2(160f, 30f);
      Graphics.DrawRect(new Vec2(0.0f, 0.0f), new Vec2(Layer.HUD.camera.width, Layer.HUD.camera.height), Color.Black * 0.4f, new Depth(0.95f));
      Graphics.DrawRect(new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) vec2.x / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 - (double) vec2.y / 2.0)), new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 + (double) vec2.x / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 + (double) vec2.y / 2.0)), Color.Black, new Depth(0.96f));
      string text2 = "WARNING!";
      this._littleFont.depth = new Depth(0.98f);
      this._littleFont.Draw(text2, new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) this._littleFont.GetWidth(text2) / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 - ((double) vec2.y / 2.0 - 2.0))), Color.White, new Depth(0.98f));
      string text3 = "Changing slot settings\nwill reset all scores\nin the current match!";
      this._littleFont2.depth = new Depth(0.98f);
      this._littleFont2.Draw(text3, new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) this._littleFont.GetWidth(text3) / 2.0), (float) ((double) Layer.HUD.camera.height / 2.0 - ((double) vec2.y / 2.0 - 10.0))), Color.White, new Depth(0.98f));
    }
  }
}
