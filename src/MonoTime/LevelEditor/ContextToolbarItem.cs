// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextToolbarItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ContextToolbarItem : ContextMenu, IContextListener
  {
    private ToolbarButton _newButton;
    private ToolbarButton _saveButton;
    private ToolbarButton _loadButton;
    private ToolbarButton _playButton;
    private ToolbarButton _gridButton;
    private ToolbarButton _quitButton;
    private ToolbarButton _steamButton;
    private ContextMenu _newMenu;
    private ContextMenu _saveMenu;
    private ContextMenu _gridMenu;
    private ContextMenu _quitMenu;
    private ContextMenu _uploadMenu;
    private List<ToolbarButton> _buttons = new List<ToolbarButton>();

    public ContextToolbarItem(ContextMenu owner)
      : base((IContextListener) owner)
    {
    }

    public override void Selected(ContextMenu item)
    {
      if (item.text == "ARCADE")
      {
        base.Selected(item);
      }
      else
      {
        if (item.text == "CANCEL")
          (Level.current as Editor).CloseMenu();
        if (item.text == "NEW")
        {
          Editor current = Level.current as Editor;
          current.ClearEverything();
          current.saveName = "";
          Editor.onlineMode = false;
          current.CloseMenu();
        }
        if (item.text == "NEW ONLINE")
        {
          Editor current = Level.current as Editor;
          current.ClearEverything();
          current.saveName = "";
          Editor.onlineMode = true;
          current.CloseMenu();
        }
        if (item.text == "SAVE")
        {
          Editor current = Level.current as Editor;
          current.Save();
          current.CloseMenu();
        }
        if (item.text == "SAVE AS")
        {
          Editor current = Level.current as Editor;
          current.SaveAs();
          current.CloseMenu();
        }
        if (item.text == "8x8")
        {
          Editor current = Level.current as Editor;
          current.cellSize = 8f;
          current.CloseMenu();
        }
        if (item.text == "16x16")
        {
          Editor current = Level.current as Editor;
          current.cellSize = 16f;
          current.CloseMenu();
        }
        if (item.text == "32x32")
        {
          Editor current = Level.current as Editor;
          current.cellSize = 32f;
          current.CloseMenu();
        }
        if (!(item.text == "QUIT"))
          return;
        Editor current1 = Level.current as Editor;
        current1.Quit();
        current1.CloseMenu();
      }
    }

    public override void Hover() => this.opened = true;

    public override void Initialize()
    {
      this._newButton = new ToolbarButton(this, 0, "New Level");
      this._saveButton = new ToolbarButton(this, 2, "Save Level");
      this._loadButton = new ToolbarButton(this, 1, "Load Level");
      this._playButton = new ToolbarButton(this, 10, "Test Level");
      this._gridButton = new ToolbarButton(this, 11, "Change Grid");
      this._steamButton = new ToolbarButton(this, 99, "Workshop Publish");
      this._quitButton = new ToolbarButton(this, 12, "Exit Editor");
      this.itemSize.y = 16f;
      float x = this.position.x;
      this._newButton.x = x;
      this._newButton.y = this.position.y;
      this._buttons.Add(this._newButton);
      float num1 = x + 18f;
      this._saveButton.x = num1;
      this._saveButton.y = this.position.y;
      this._buttons.Add(this._saveButton);
      float num2 = num1 + 18f;
      this._loadButton.x = num2;
      this._loadButton.y = this.position.y;
      this._buttons.Add(this._loadButton);
      float num3 = num2 + 18f;
      this._playButton.x = num3;
      this._playButton.y = this.position.y;
      this._buttons.Add(this._playButton);
      float num4 = num3 + 18f;
      this._gridButton.x = num4;
      this._gridButton.y = this.position.y;
      this._buttons.Add(this._gridButton);
      if (Steam.IsInitialized())
      {
        num4 += 18f;
        this._steamButton.x = num4;
        this._steamButton.y = this.position.y;
        this._buttons.Add(this._steamButton);
      }
      this._quitButton.x = num4 + 18f;
      this._quitButton.y = this.position.y;
      this._buttons.Add(this._quitButton);
    }

    public override void ParentCloseAction()
    {
      this._selectedIndex = -1;
      foreach (ToolbarButton button in this._buttons)
        button.hover = false;
    }

    public override void Update()
    {
      if (!this._opening && this.opened && Editor.gamepadMode)
      {
        if (this._gridMenu != null && this._gridMenu.opened || this._saveMenu != null && this._saveMenu.opened || (this._newMenu != null && this._newMenu.opened || this._quitMenu != null && this._quitMenu.opened) || this._uploadMenu != null && this._uploadMenu.opened)
          return;
        if (Input.Pressed("UP"))
        {
          this.opened = false;
          if (this.owner is ContextMenu owner)
          {
            owner._opening = true;
            foreach (ToolbarButton button in this._buttons)
              button.hover = false;
            Editor.infoText = "";
            return;
          }
        }
        if (Input.Pressed("DOWN"))
        {
          this.opened = false;
          if (this.owner is ContextMenu owner)
          {
            ++owner.selectedIndex;
            foreach (ToolbarButton button in this._buttons)
              button.hover = false;
            Editor.infoText = "";
            return;
          }
        }
        if (Input.Pressed("LEFT"))
          --this._selectedIndex;
        else if (Input.Pressed("RIGHT"))
          ++this._selectedIndex;
        this._selectedIndex = Maths.Clamp(this._selectedIndex, 0, this._buttons.Count - 1);
        int num = 0;
        foreach (ToolbarButton button in this._buttons)
        {
          if (this._selectedIndex == num)
          {
            button.hover = true;
            Editor.infoText = button.hoverText;
            if (Input.Pressed("SELECT"))
              this.ButtonPressed(button);
          }
          else
            button.hover = false;
          ++num;
        }
      }
      float x = this.position.x;
      this._newButton.x = x;
      this._newButton.y = this.position.y;
      float num1 = x + 18f;
      this._saveButton.x = num1;
      this._saveButton.y = this.position.y;
      float num2 = num1 + 18f;
      this._loadButton.x = num2;
      this._loadButton.y = this.position.y;
      float num3 = num2 + 18f;
      this._playButton.x = num3;
      this._playButton.y = this.position.y;
      float num4 = num3 + 18f;
      this._gridButton.x = num4;
      this._gridButton.y = this.position.y;
      if (Steam.IsInitialized())
      {
        num4 += 18f;
        this._steamButton.x = num4;
        this._steamButton.y = this.position.y;
      }
      this._quitButton.x = num4 + 18f;
      this._quitButton.y = this.position.y;
      foreach (Thing button in this._buttons)
        button.DoUpdate();
      base.Update();
    }

    public override void Terminate()
    {
      Level.current.RemoveThing((Thing) this._newButton);
      Level.current.RemoveThing((Thing) this._saveButton);
      Level.current.RemoveThing((Thing) this._loadButton);
      Level.current.RemoveThing((Thing) this._playButton);
      Level.current.RemoveThing((Thing) this._gridButton);
      Level.current.RemoveThing((Thing) this._quitButton);
      if (this._steamButton != null)
        Level.current.RemoveThing((Thing) this._steamButton);
      this.Closed();
      base.Terminate();
    }

    public override bool HasOpen() => this.opened;

    public void ButtonPressed(ToolbarButton button)
    {
      SFX.Play("highClick", 0.3f);
      ContextMenu contextMenu1 = (ContextMenu) null;
      Vec2 vec2 = new Vec2(2f, 21f);
      if (button == this._newButton)
      {
        this.Closed();
        this._newMenu = new ContextMenu((IContextListener) this, hasToproot: true, topRoot: button.position);
        this._newMenu.x = this.position.x - vec2.x;
        this._newMenu.y = this.position.y + vec2.y;
        this._newMenu.root = true;
        this._newMenu.depth = this.depth + 10;
        this.Selected();
        this._newMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "NEW"
        });
        this._newMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "NEW ONLINE"
        });
        ContextMenu contextMenu2 = new ContextMenu((IContextListener) this);
        contextMenu2.itemSize.x = 60f;
        contextMenu2.text = "ARCADE";
        contextMenu2.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "NEW ARCADE"
        });
        contextMenu2.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "NEW CHALLENGE"
        });
        contextMenu2.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "NEW ARCADE MACHINE"
        });
        this._newMenu.AddItem(contextMenu2);
        this._newMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "CANCEL"
        });
        Level.Add((Thing) this._newMenu);
        this._newMenu.opened = true;
        contextMenu1 = this._newMenu;
      }
      if (button == this._saveButton)
      {
        this.Closed();
        this._saveMenu = new ContextMenu((IContextListener) this, hasToproot: true, topRoot: button.position);
        this._saveMenu.x = this.position.x - vec2.x;
        this._saveMenu.y = this.position.y + vec2.y;
        this._saveMenu.root = true;
        this._saveMenu.depth = this.depth + 10;
        this.Selected();
        this._saveMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "SAVE"
        });
        this._saveMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "SAVE AS"
        });
        Level.Add((Thing) this._saveMenu);
        this._saveMenu.opened = true;
        contextMenu1 = this._saveMenu;
      }
      if (button == this._gridButton)
      {
        this.Closed();
        this._gridMenu = new ContextMenu((IContextListener) this, hasToproot: true, topRoot: button.position);
        this._gridMenu.x = this.position.x - vec2.x;
        this._gridMenu.y = this.position.y + vec2.y;
        this._gridMenu.root = true;
        this._gridMenu.depth = this.depth + 10;
        this.Selected();
        this._gridMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "8x8"
        });
        this._gridMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "16x16"
        });
        this._gridMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "32x32"
        });
        Level.Add((Thing) this._gridMenu);
        this._gridMenu.opened = true;
        contextMenu1 = this._gridMenu;
      }
      if (button == this._loadButton)
      {
        Editor current = Level.current as Editor;
        current.Load();
        current.CloseMenu();
      }
      if (button == this._steamButton)
      {
        Editor current = Level.current as Editor;
        current.SteamUpload();
        current.CloseMenu();
      }
      if (button == this._playButton)
        (Level.current as Editor).Play();
      if (button == this._quitButton)
      {
        this.Closed();
        this._quitMenu = new ContextMenu((IContextListener) this, hasToproot: true, topRoot: button.position);
        this._quitMenu.x = this.position.x - vec2.x;
        this._quitMenu.y = this.position.y + vec2.y;
        this._quitMenu.root = true;
        this._quitMenu.depth = this.depth + 10;
        this.Selected();
        this._quitMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "QUIT"
        });
        this._quitMenu.AddItem(new ContextMenu((IContextListener) this)
        {
          itemSize = {
            x = 60f
          },
          text = "CANCEL"
        });
        Level.Add((Thing) this._quitMenu);
        this._quitMenu.opened = true;
        contextMenu1 = this._quitMenu;
      }
      if (contextMenu1 == null || (double) contextMenu1.y + (double) contextMenu1.menuSize.y <= (double) Layer.HUD.camera.height - 4.0)
        return;
      float y = contextMenu1.y;
      contextMenu1.y = Layer.HUD.camera.height - 4f - contextMenu1.menuSize.y;
      contextMenu1._toprootPosition.y += contextMenu1.y - y;
      if (this.owner is ContextMenu owner)
      {
        owner._openedOffset = 0.0f;
        owner.y = contextMenu1.y - 16f - owner.menuSize.y;
      owner.PositionItems();
      }
      contextMenu1.PositionItems();
    }

    public override void Closed()
    {
      if (this._newMenu != null)
      {
        Level.Remove((Thing) this._newMenu);
        this._newMenu = (ContextMenu) null;
      }
      if (this._saveMenu != null)
      {
        Level.Remove((Thing) this._saveMenu);
        this._saveMenu = (ContextMenu) null;
      }
      if (this._gridMenu != null)
      {
        Level.Remove((Thing) this._gridMenu);
        this._gridMenu = (ContextMenu) null;
      }
      if (this._quitMenu != null)
      {
        Level.Remove((Thing) this._quitMenu);
        this._quitMenu = (ContextMenu) null;
      }
      if (this._uploadMenu == null)
        return;
      Level.Remove((Thing) this._uploadMenu);
      this._uploadMenu = (ContextMenu) null;
    }

    public override void Draw()
    {
      float x = this.position.x;
      this._newButton.x = x;
      this._newButton.y = this.position.y;
      float num1 = x + 18f;
      this._saveButton.x = num1;
      this._saveButton.y = this.position.y;
      float num2 = num1 + 18f;
      this._loadButton.x = num2;
      this._loadButton.y = this.position.y;
      float num3 = num2 + 18f;
      this._playButton.x = num3;
      this._playButton.y = this.position.y;
      float num4 = num3 + 18f;
      this._gridButton.x = num4;
      this._gridButton.y = this.position.y;
      if (Steam.IsInitialized())
      {
        num4 += 18f;
        this._steamButton.x = num4;
        this._steamButton.y = this.position.y;
      }
      this._quitButton.x = num4 + 18f;
      this._quitButton.y = this.position.y;
      foreach (Thing button in this._buttons)
        button.DoDraw();
    }
  }
}
