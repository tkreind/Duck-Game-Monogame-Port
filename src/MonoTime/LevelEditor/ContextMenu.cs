// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class ContextMenu : Thing, IContextListener
  {
    public string tooltip;
    protected List<ContextMenu> _items = new List<ContextMenu>();
    public Vec2 menuSize = new Vec2();
    protected Sprite _contextArrow;
    public Vec2 itemSize = new Vec2();
    public Thing contextThing;
    protected string _text = "";
    protected string _data = "";
    protected bool _canExpand;
    protected int _selectedIndex;
    protected bool _showBackground = true;
    public bool greyOut;
    public bool drawControls = true;
    public bool disabled;
    private bool _opened;
    public float _openedOffset;
    protected bool _dragMode;
    public bool _opening;
    protected bool _hover;
    protected bool _root;
    protected bool _collectionChanged;
    protected IContextListener _owner;
    public Vec2 offset = new Vec2();
    private SpriteMap _image;
    private bool _hasToproot;
    public Vec2 _toprootPosition;
    public bool isToggle;
    private bool _takingInput;
    public bool closeOnRight;
    public bool fancy;

    public string text
    {
      get => this._text;
      set => this._text = value;
    }

    public string data
    {
      get => this._data;
      set => this._data = value;
    }

    public int selectedIndex
    {
      get => this._selectedIndex;
      set => this._selectedIndex = value;
    }

    public bool opened
    {
      get => this._opened;
      set
      {
        if (!this._opened && value)
        {
          foreach (ContextMenu contextMenu in this._items)
            contextMenu.opened = false;
          this._openedOffset = 0.0f;
          this.PositionItems();
          this._selectedIndex = 0;
          if (this._items.Count > 0)
          {
            while (this._selectedIndex < this._items.Count<ContextMenu>() - 1 && this._items[this._selectedIndex].greyOut)
              ++this._selectedIndex;
            if (!this._items[this._selectedIndex].greyOut)
              this._opening = true;
          }
          else
            this._opening = true;
          this.PushLeft();
        }
        if (this._opened && !value)
        {
          foreach (ContextMenu contextMenu in this._items)
            contextMenu.opened = false;
          this.Closed();
        }
        if (!value)
        {
          foreach (ContextMenu contextMenu in this._items)
            contextMenu.ParentCloseAction();
        }
        this._opened = value;
      }
    }

    public bool hover
    {
      get => this._hover;
      set => this._hover = value;
    }

    public bool root
    {
      get => this._root;
      set => this._root = value;
    }

    public virtual void ParentCloseAction()
    {
    }

    public SpriteMap image
    {
      get => this._image;
      set => this._image = value;
    }

    public ContextMenu(IContextListener owner, SpriteMap img = null, bool hasToproot = false, Vec2 topRoot = default (Vec2))
      : base()
    {
      this._owner = owner;
      this.layer = Layer.HUD;
      this._contextArrow = new Sprite("contextArrowRight");
      this.itemSize.x = 140f;
      this.itemSize.y = 16f;
      this._root = owner == null;
      this._image = img;
      this.depth = (Depth) 0.8f;
      this._toprootPosition = topRoot;
      this._hasToproot = hasToproot;
    }

    public override void Initialize()
    {
    }

    public virtual bool HasOpen()
    {
      foreach (ContextMenu contextMenu in this._items)
      {
        if (contextMenu.opened)
          return true;
      }
      return false;
    }

    public virtual void Toggle(ContextMenu item)
    {
      if (this._owner == null)
        return;
      this.isToggle = true;
      this._owner.Selected(item);
      this.isToggle = false;
    }

    public bool IsPartOf(ContextMenu menu)
    {
      if (this == menu)
        return true;
      return this._owner != null && this._owner is ContextMenu owner && owner.IsPartOf(menu);
    }

    public override void DoUpdate()
    {
      if (Editor.clickedMenu)
        return;
      base.DoUpdate();
    }

    public void PushLeft()
    {
      ContextMenu contextMenu1 = (ContextMenu) null;
      foreach (ContextMenu contextMenu2 in this._items)
      {
        if (contextMenu2.opened)
        {
          contextMenu2.PushLeft();
          contextMenu1 = contextMenu2;
        }
      }
      Vec2 vec2_1 = new Vec2(this.x, this.y);
      Vec2 vec2_2 = new Vec2(0.0f, 0.0f);
      if (!this._root)
        vec2_2 = new Vec2(this.itemSize.x + 4f, -2f);
      Vec2 vec2_3 = vec2_1 + vec2_2;
      bool flag = false;
      if ((double) vec2_3.x + (double) this.menuSize.x + 4.0 > (double) Layer.HUD.camera.width)
      {
        vec2_3.x = (float) ((double) Layer.HUD.camera.width - (double) this.menuSize.x - 4.0);
        flag = true;
      }
      if (contextMenu1 != null && (double) vec2_3.x + (double) this.menuSize.x + 2.0 > (double) contextMenu1.x + (double) this.menuSize.x)
      {
        vec2_3.x = (float) ((double) contextMenu1.x + (double) this.menuSize.x - ((double) this.menuSize.x + 2.0));
        flag = true;
      }
      vec2_3 -= vec2_2;
      this.position = vec2_3;
      if (!flag)
        return;
      this.PositionItems();
    }

    public override void Update()
    {
      if (!this.visible || this.disabled || this._opening)
      {
        this._opening = false;
      }
      else
      {
        if (this.opened)
        {
          this.PushLeft();
          int count = this._items.Count;
          for (int index = 0; index < count; ++index)
          {
            this._items[index].DoUpdate();
            if (this._collectionChanged)
            {
              this._collectionChanged = false;
              return;
            }
          }
        }
        if (this._items.Count > 0)
          this._canExpand = true;
        bool flag1 = false;
        if (!Editor.HasFocus())
        {
          if (this._hover && this._dragMode && Mouse.left == InputState.Down)
            flag1 = true;
          if (Mouse.right == InputState.Pressed && this.closeOnRight)
          {
            this._owner.Selected((ContextMenu) null);
            this.opened = false;
            return;
          }
        }
        if (this._hover && this.tooltip != null)
          Editor.tooltip = this.tooltip;
        if (!Editor.HasFocus() && (Editor.lockInput == null || this.IsPartOf(Editor.lockInput)) && (!Editor.tookInput && Editor.gamepadMode))
        {
          bool flag2 = this.HasOpen();
          if (flag2 && Input.Pressed("LEFT") && this._canExpand)
          {
            bool flag3 = false;
            foreach (ContextMenu contextMenu in this._items)
            {
              if (contextMenu.HasOpen())
              {
                flag3 = true;
                break;
              }
            }
            if (!flag3)
            {
              Editor.tookInput = true;
              this.Selected((ContextMenu) null);
            }
          }
          this._takingInput = false;
          if (!flag2)
          {
            if (this.opened && this._items.Count > 0)
            {
              this._takingInput = true;
              if (Input.Pressed("UP"))
              {
                --this._selectedIndex;
                for (int index = 0; index < this._items.Count; ++index)
                {
                  if (this._selectedIndex < 0)
                    this._selectedIndex = this._items.Count - 1;
                  if (this._items[this._selectedIndex].greyOut)
                    --this._selectedIndex;
                  else
                    break;
                }
              }
              else if (Input.Pressed("DOWN"))
              {
                ++this._selectedIndex;
                for (int index = 0; index < this._items.Count; ++index)
                {
                  if (this._selectedIndex > this._items.Count - 1)
                    this._selectedIndex = 0;
                  if (this._items[this._selectedIndex].greyOut)
                    ++this._selectedIndex;
                  else
                    break;
                }
              }
              int num = 0;
              foreach (ContextMenu contextMenu in this._items)
              {
                if (num == this._selectedIndex)
                {
                  contextMenu._hover = true;
                  contextMenu.Hover();
                }
                else
                  contextMenu._hover = false;
                ++num;
              }
            }
            if (this._hover && (Input.Pressed("SELECT") || this._canExpand && Input.Pressed("RIGHT")))
              flag1 = true;
          }
        }
        else if (!Editor.gamepadMode && !Editor.HasFocus())
        {
          if ((double) Mouse.x >= (double) this.x && (double) Mouse.x <= (double) this.x + (double) this.itemSize.x && ((double) Mouse.y >= (double) this.y + 1.0 && (double) Mouse.y <= (double) this.y + (double) this.itemSize.y - 1.0))
          {
            if (Mouse.left == InputState.Pressed)
              flag1 = true;
            this._hover = true;
            if (this.owner is ContextMenu owner)
            {
              int num = 0;
              foreach (ContextMenu contextMenu in owner._items)
              {
                if (contextMenu == this)
                {
                  owner._selectedIndex = num;
                  break;
                }
                ++num;
              }
            }
          }
          else if (!this._dragMode || Mouse.left != InputState.Down)
            this._hover = false;
        }
        if (!flag1)
          return;
        Level level = this._level;
        Editor.clickedMenu = true;
        this.Selected();
      }
    }

    public override void Terminate()
    {
      Level level = this._level;
    }

    public void ClearItems()
    {
      this._collectionChanged = true;
      this._items.Clear();
    }

    public virtual void Hover()
    {
    }

    public override void Draw()
    {
      ContextMenu contextMenu1 = this;
      contextMenu1.position = contextMenu1.position + this.offset;
      if (!this._root)
      {
        float num1 = 1f;
        if (this.greyOut)
          num1 = 0.3f;
        float num2 = num1 * this.alpha;
        if (this._hover && !this.greyOut)
          Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70) * this.alpha, this.depth);
        if (this._image != null)
        {
          this._image.depth = this.depth + 3;
          this._image.x = this.x + 1f;
          this._image.y = this.y;
          this._image.color = Color.White * num2;
          this._image.Draw();
          Graphics.DrawString(this._text, this.position + new Vec2(20f, 4f), Color.White * num2, this.depth + 1);
        }
        else if (this._text == "custom")
          Graphics.DrawString(this._text, this.position + new Vec2(2f, 4f), Colors.DGBlue * num2, this.depth + 1);
        else if (this.fancy)
          Graphics.DrawFancyString(this._text, this.position + new Vec2(2f, 4f), Color.White * num2, this.depth + 1);
        else
          Graphics.DrawString(this._text, this.position + new Vec2(2f, 4f), Color.White * num2, this.depth + 1);
        if (this._items.Count > 0)
        {
          this._contextArrow.color = Color.White * num2;
          Graphics.Draw(this._contextArrow, (float) ((double) this.x + (double) this.itemSize.x - 11.0), this.y + 3f, this.depth + 1);
        }
      }
      if (this.opened)
      {
        bool flag1 = false;
        if ((double) this.y + (double) this._openedOffset + (double) this.menuSize.y + 16.0 > (double) this.layer.height)
        {
          this._openedOffset = (float) ((double) this.layer.height - (double) this.menuSize.y - (double) this.y - 16.0);
          flag1 = true;
        }
        if ((double) this.y + (double) this._openedOffset < 0.0)
        {
          this._openedOffset = -this.y;
          flag1 = true;
        }
        if (flag1)
          this.PositionItems();
        float x = this.menuSize.x;
        float y = this.menuSize.y;
        Vec2 p1 = new Vec2(this.x, this.y + this._openedOffset);
        if (!this._root)
        {
          p1.x += this.itemSize.x + 4f;
          p1.y -= 2f;
        }
        if (this._showBackground)
        {
          Graphics.DrawRect(p1, p1 + new Vec2(x, y), new Color(70, 70, 70) * this.alpha, this.depth);
          Graphics.DrawRect(p1 + new Vec2(1f, 1f), p1 + new Vec2(x - 1f, y - 1f), new Color(30, 30, 30) * this.alpha, this.depth + 1);
        }
        if (Editor.gamepadMode && this.drawControls && this._takingInput)
        {
          string text = "";
          bool flag2 = false;
          foreach (ContextMenu contextMenu2 in this._items)
          {
            if (contextMenu2.hover)
            {
              if (contextMenu2._items.Count > 0 || contextMenu2 is ContextBackgroundTile)
              {
                text = "@SELECT@@RIGHT@EXPAND";
              }
              else
              {
                switch (contextMenu2)
                {
                  case ContextSlider _:
                    if ((contextMenu2 as ContextSlider).adjust)
                    {
                      text = "@DPAD@EDIT @SHOOT@FAST @STRAFE@SLOW";
                      flag2 = true;
                      continue;
                    }
                    text = "@SELECT@EDIT";
                    continue;
                  case ContextTextbox _:
                    text = "@SELECT@ENTER TEXT";
                    continue;
                  case ContextRadio _:
                  case ContextCheckBox _:
                    text = "@SELECT@TOGGLE";
                    continue;
                  default:
                    text = "@SELECT@SELECT";
                    continue;
                }
              }
            }
          }
          if (!this._root && !flag2)
            text += "  @LEFT@BACK";
          Graphics.DrawRect(p1 + new Vec2(0.0f, y), p1 + new Vec2(x, y + 15f), Color.Black * this.alpha, this.depth);
          Graphics.DrawString(text, p1 + new Vec2(0.0f, y + 4f), Color.White * this.alpha, this.depth + 1);
        }
        if (this._hasToproot)
          Graphics.DrawRect(this._toprootPosition, this._toprootPosition + new Vec2(16f, 32f), new Color(70, 70, 70) * this.alpha, this.depth - 4);
        foreach (ContextMenu contextMenu2 in this._items)
        {
          if (contextMenu2.visible)
            contextMenu2.DoDraw();
        }
      }
      ContextMenu contextMenu3 = this;
      contextMenu3.position = contextMenu3.position - this.offset;
    }

    public virtual void Selected()
    {
      if (this.greyOut || this._owner == null)
        return;
      this._owner.Selected(this);
    }

    public virtual void Selected(ContextMenu item)
    {
      if (this.greyOut)
        return;
      foreach (ContextMenu contextMenu in this._items)
      {
        if (contextMenu != item)
          contextMenu.opened = false;
      }
      if (item == null)
        return;
      item.opened = true;
      SFX.Play("highClick", 0.3f);
    }

    public void AddItem(ContextMenu item)
    {
      item.Initialize();
      this._items.Add(item);
      item.owner = (Thing) this;
      this.PositionItems();
    }

    public void PositionItems()
    {
      float num1 = 0.0f;
      float num2 = this.y + 3f + this._openedOffset;
      if (!this._root)
        --num2;
      for (int index = 0; index < this._items.Count; ++index)
      {
        ContextMenu contextMenu = this._items[index];
        if (!this._root)
          contextMenu.x = (float) ((double) this.x + 3.0 + (double) this.itemSize.x + 3.0);
        else
          contextMenu.x = this.x + 2f;
        contextMenu.y = num2;
        num2 += contextMenu.itemSize.y + 1f;
        if (!this._root)
          contextMenu.y -= 2f;
        else
          --contextMenu.y;
        if ((double) contextMenu.itemSize.x < 140.0)
          contextMenu.itemSize.x = 140f;
        if ((double) contextMenu.itemSize.x + 4.0 > (double) this.menuSize.x)
          this.menuSize.x = contextMenu.itemSize.x + 4f;
        contextMenu.depth = this.depth + 2;
        if ((double) contextMenu.itemSize.x > (double) num1)
          num1 = contextMenu.itemSize.x;
      }
      float num3 = 0.0f;
      foreach (ContextMenu contextMenu in this._items)
      {
        num3 += contextMenu.itemSize.y + 1f;
        contextMenu.itemSize.x = num1;
      }
      this.menuSize.y = num3 + 3f;
    }

    public void CloseMenus()
    {
      foreach (ContextMenu contextMenu in this._items)
        contextMenu.opened = false;
    }

    public virtual void Closed()
    {
    }
  }
}
