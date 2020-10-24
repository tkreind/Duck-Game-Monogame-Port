﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.FurniShopScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class FurniShopScreen : Thing
  {
    public static bool close = false;
    private Sprite _tail;
    public bool quitOut;
    private PlasmaLayer _plasma;
    private Layer _treeLayer;
    public static bool open = false;
    public static VincentProduct attemptBuy = (VincentProduct) null;
    public static bool giveYoYo = false;
    public static int attemptBuyIndex;
    private UIComponent _pauseGroup;
    private UIMenu _confirmMenu;
    private MenuBoolean _confirm = new MenuBoolean();
    private UnlockData _tryBuy;

    public override bool visible
    {
      get => (double) this.alpha >= 0.00999999977648258 && base.visible;
      set => base.visible = value;
    }

    public FurniShopScreen()
      : base()
    {
      this._tail = new Sprite("arcade/bubbleTail");
      this.layer = Layer.HUD;
    }

    public override void Initialize()
    {
      this._plasma = new PlasmaLayer("PLASMA", -85);
      Layer.Add((Layer) this._plasma);
      this._treeLayer = new Layer("TREE", -95, new Camera());
      Layer.Add(this._treeLayer);
    }

    public void OpenBuyConfirmation(UnlockData unlock)
    {
      if (this._pauseGroup != null)
      {
        Level.Remove((Thing) this._pauseGroup);
        this._pauseGroup = (UIComponent) null;
      }
      this._confirm.value = false;
      this._pauseGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      this._confirmMenu = new UIMenu("UNLOCK FEATURE", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, conString: "@SELECT@BUY @QUACK@CANCEL");
      this._confirmMenu.Add((UIComponent) new UIText(unlock.name, Color.Green), true);
      this._confirmMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      float num = 190f;
      string str1 = unlock.longDescription;
      string textVal = "";
      string str2 = "";
      while (true)
      {
        if (str1.Length > 0 && str1[0] != ' ')
        {
          str2 += (string) (object) str1[0];
        }
        else
        {
          if ((double) ((textVal.Length + str2.Length) * 8) > (double) num)
          {
            this._confirmMenu.Add((UIComponent) new UIText(textVal, Color.White, UIAlign.Left), true);
            textVal = "";
          }
          if (textVal.Length > 0)
            textVal += " ";
          textVal += str2;
          str2 = "";
        }
        if (str1.Length != 0)
          str1 = str1.Remove(0, 1);
        else
          break;
      }
      if (str2.Length > 0)
      {
        if (textVal.Length > 0)
          textVal += " ";
        textVal += str2;
      }
      if (textVal.Length > 0)
        this._confirmMenu.Add((UIComponent) new UIText(textVal, Color.White, UIAlign.Left), true);
      this._confirmMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("BUY UNLOCK |WHITE|(|LIME|" + unlock.cost.ToString() + "|WHITE| TICKETS)", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(this._pauseGroup, this._confirm)), true);
      this._confirmMenu.Add((UIComponent) new UIMenuItem("CANCEL", (UIMenuAction) new UIMenuActionCloseMenu(this._pauseGroup), c: Colors.MenuOption, backButton: true), true);
      this._confirmMenu.Close();
      this._pauseGroup.Add((UIComponent) this._confirmMenu, false);
      this._pauseGroup.Close();
      Level.Add((Thing) this._pauseGroup);
      for (int index = 0; index < 10; ++index)
      {
        this._pauseGroup.Update();
        this._confirmMenu.Update();
      }
      this._pauseGroup.Open();
      this._confirmMenu.Open();
      MonoMain.pauseMenu = this._pauseGroup;
      SFX.Play("pause", 0.6f);
      this._tryBuy = unlock;
    }

    public void ChangeSpeech() => Chancy.Clear();

    public void MakeActive()
    {
      HUD.AddCornerCounter(HUDCorner.BottomLeft, "@TICKET@ ", new FieldBinding((object) Profiles.active[0], "ticketCount"), animateCount: true);
      HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@BACK");
    }

    public override void Update()
    {
    }

    public override void Draw()
    {
      if ((double) this.alpha < 0.00999999977648258)
        return;
      Graphics.DrawRect(new Vec2(26f, 22f), new Vec2(Layer.HUD.width - 105f, Layer.HUD.height - 51f), new Color(20, 20, 20) * this.alpha * 0.7f, new Depth(-0.9f));
      Vec2 p1 = new Vec2(20f, 8f);
      Vec2 vec2 = new Vec2(226f, 11f);
      Graphics.DrawRect(p1, p1 + vec2, Color.Black, new Depth(0.96f));
      string text = "what a name";
      Graphics.DrawString(text, p1 + new Vec2((float) (((double) vec2.x - 27.0) / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), 2f), new Color(163, 206, 39) * this.alpha, new Depth(0.97f));
      this._tail.depth = new Depth(0.5f);
      this._tail.alpha = this.alpha;
      this._tail.flipH = false;
      this._tail.flipV = false;
      Graphics.Draw(this._tail, 222f, 18f);
      Chancy.alpha = this.alpha;
      Chancy.Draw();
    }
  }
}
