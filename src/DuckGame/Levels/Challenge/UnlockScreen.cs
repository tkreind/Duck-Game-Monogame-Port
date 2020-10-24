// Decompiled with JetBrains decompiler
// Type: DuckGame.UnlockScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class UnlockScreen : Thing
  {
    private Sprite _tail;
    public bool quitOut;
    private UnlockTree _tree;
    private PlasmaLayer _plasma;
    private Layer _treeLayer;
    public static bool open;
    private UIComponent _pauseGroup;
    private UIMenu _confirmMenu;
    private MenuBoolean _confirm = new MenuBoolean();
    private UnlockData _tryBuy;

    public override bool visible
    {
      get => (double) this.alpha >= 0.00999999977648258 && base.visible;
      set => base.visible = value;
    }

    public UnlockScreen()
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
      this._tree = new UnlockTree(this, this._treeLayer);
      Level.Add((Thing) this._tree);
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

    public void ChangeSpeech()
    {
      Chancy.Clear();
      string str = "";
      if (this._tree.selected.type == UnlockType.Hat)
        str = "Hat";
      else if (this._tree.selected.type == UnlockType.Level)
        str = "Level";
      else if (this._tree.selected.type == UnlockType.Modifier)
        str = "Gameplay Modifier";
      else if (this._tree.selected.type == UnlockType.Weapon)
        str = "Weapon";
      else if (this._tree.selected.type == UnlockType.Special)
        str = "Special";
      string line = this._tree.selected.description + "^|ORANGE|" + str + "|WHITE| - ";
      if (this._tree.selected.ProfileUnlocked(Profiles.active[0]))
        line += "|GREEN|Unlocked";
      else if (this._tree.selected.parent != null)
      {
        List<UnlockData> treeLayer = Unlocks.GetTreeLayer(this._tree.selected.parent.layer);
        bool flag = false;
        foreach (UnlockData unlockData in treeLayer)
        {
          if (unlockData.children.Contains(this._tree.selected) && !unlockData.ProfileUnlocked(Profiles.active[0]))
          {
            line += "|RED|LOCKED";
            flag = true;
            List<string> stringList = new List<string>()
            {
              "Wonder what this one is?",
              "I think you're gonna like this one.",
              "This one is just perfect for you.",
              "Yeah this ones out of stock."
            };
            line = stringList[Rando.Int(stringList.Count - 1)];
            break;
          }
        }
        if (!flag)
          line = line + "|YELLOW|Costs @TICKET@ " + Convert.ToString(this._tree.selected.cost);
      }
      else
        line = line + "|YELLOW|Costs @TICKET@ " + Convert.ToString(this._tree.selected.cost);
      Chancy.Add(line);
    }

    public void SelectionChanged()
    {
      HUD.CloseCorner(HUDCorner.BottomRight);
      if (this._tree.selected.AllParentsUnlocked(Profiles.active[0]))
      {
        if (this._tree.selected.ProfileUnlocked(Profiles.active[0]))
          HUD.AddCornerMessage(HUDCorner.BottomRight, "|LIME|UNLOCKED");
        else if (this._tree.selected.cost <= Profiles.active[0].ticketCount)
          HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@|LIME|BUY");
        else
          HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@|RED|BUY");
      }
      else
        HUD.AddCornerControl(HUDCorner.BottomRight, "@SHOOT@|RED|LOCKED");
    }

    public void MakeActive()
    {
      HUD.AddCornerCounter(HUDCorner.BottomLeft, "@TICKET@ ", new FieldBinding((object) Profiles.active[0], "ticketCount"), animateCount: true);
      HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@BACK");
      this.SelectionChanged();
    }

    public override void Update()
    {
      int num = (int) Math.Round((double) Graphics.width / ((double) this._treeLayer.camera.width * 2.0));
      this._treeLayer.scissor = new Rectangle((float) (50 * num), (float) (44 * num), (float) (Graphics.width - 180 * num), (float) (214 * num));
      if (this._confirmMenu != null && !this._confirmMenu.open && this._tryBuy != null)
      {
        if (this._confirm.value)
        {
          SFX.Play("ching");
          Profiles.active[0].ticketCount -= this._tryBuy.cost;
          Profiles.active[0].unlocks.Add(this._tryBuy.id);
          Profiles.Save(Profiles.active[0]);
          this.SelectionChanged();
        }
        else
          SFX.Play("resume");
        this._tryBuy = (UnlockData) null;
      }
      if (this._confirmMenu != null && !this._confirmMenu.open && this._pauseGroup != null)
      {
        Level.Remove((Thing) this._pauseGroup);
        this._pauseGroup = (UIComponent) null;
        this._confirmMenu = (UIMenu) null;
      }
      if (!Layer.Contains((Layer) this._plasma))
        Layer.Add((Layer) this._plasma);
      if (!Layer.Contains(this._treeLayer))
        Layer.Add(this._treeLayer);
      this._plasma.alpha = this.alpha;
      this._tree.alpha = this.alpha;
      if ((double) this.alpha > 0.899999976158142)
      {
        UnlockScreen.open = true;
        if (!Input.Pressed("QUACK"))
          return;
        SFX.Play("menu_back");
        this.quitOut = true;
      }
      else
        UnlockScreen.open = false;
    }

    public override void Draw()
    {
      if ((double) this.alpha < 0.00999999977648258)
        return;
      Graphics.DrawRect(new Vec2(26f, 22f), new Vec2(Layer.HUD.width - 105f, Layer.HUD.height - 51f), new Color(20, 20, 20) * this.alpha * 0.7f, new Depth(-0.9f));
      Vec2 p1 = new Vec2(20f, 8f);
      Vec2 vec2 = new Vec2(226f, 11f);
      Graphics.DrawRect(p1, p1 + vec2, Color.Black);
      bool flag1 = this._tree.selected.ProfileUnlocked(Profiles.active[0]);
      bool flag2 = true;
      if (!this._tree.selected.AllParentsUnlocked(Profiles.active[0]))
        flag2 = false;
      string text = this._tree.selected.name;
      if (!flag2)
        text = "???";
      Graphics.DrawString(text, p1 + new Vec2((float) (((double) vec2.x - 27.0) / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), 2f), (flag1 ? new Color(163, 206, 39) : Color.Red) * this.alpha, new Depth(0.5f));
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
