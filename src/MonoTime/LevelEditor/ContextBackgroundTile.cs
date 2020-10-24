// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextBackgroundTile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckGame
{
  public class ContextBackgroundTile : ContextMenu
  {
    private Thing _thing;
    private Sprite _image;
    private bool _placement;
    protected Vec2 _hoverPos = Vec2.Zero;
    private ContextFile _file;
    private bool justOpened = true;
    public bool floatMode;

    public Thing thing => this._thing;

    public ContextBackgroundTile(Thing thing, IContextListener owner, bool placement = true)
      : base(owner)
    {
      this._placement = placement;
      this._thing = thing;
      this._image = thing.GetEditorImage();
      this.itemSize.x = 180f;
      this.itemSize.y = 16f;
      this._text = thing.editorName;
      this.itemSize.x = (float) (this._text.Length * 8 + 16);
      this._canExpand = true;
      this.depth = new Depth(0.8f);
      if (this._thing is CustomBackground)
        this._file = new ContextFile("BURG", (IContextListener) this, new FieldBinding((object) this._thing, "customBackground0" + (object) ((thing as CustomBackground).customIndex + 1)), ContextFileType.Background);
      IReadOnlyPropertyBag bag = ContentProperties.GetBag(thing.GetType());
      if (!Main.isDemo || bag.GetOrDefault<bool>("isInDemo", false))
        return;
      this.greyOut = true;
    }

    public override bool HasOpen() => this.opened;

    public override void Selected()
    {
      if (this.greyOut)
        return;
      SFX.Play("highClick", 0.3f);
      if (this._owner == null)
        return;
      this._owner.Selected((ContextMenu) this);
    }

    public override void Draw()
    {
      if (!this._root)
      {
        float num = 1f;
        if (this.greyOut)
          num = 0.3f;
        if (this._hover && !this.greyOut)
          DuckGame.Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70), new Depth(0.82f));
        DuckGame.Graphics.DrawFancyString(this._text, this.position + new Vec2(2f, 4f), Color.White * num, new Depth(0.85f));
        this._contextArrow.color = Color.White * num;
        DuckGame.Graphics.Draw(this._contextArrow, (float) ((double) this.x + (double) this.itemSize.x - 11.0), this.y + 3f, new Depth(0.85f));
      }
      if (this.opened)
      {
        SpriteMap graphic = this._thing.graphic as SpriteMap;
        this.menuSize = new Vec2((float) (graphic.texture.width + 2), (float) (graphic.texture.height + 2));
        float x = this.menuSize.x;
        float y = this.menuSize.y;
        Vec2 p1 = new Vec2(this.x, this.y);
        if (!this._root)
        {
          p1.x += this.itemSize.x + 4f;
          p1.y -= 2f;
        }
        Vec2 vec2_1 = new Vec2(graphic.position);
        this._thing.x = (float) ((double) p1.x + 1.0 + (double) graphic.w / 2.0);
        this._thing.y = (float) ((double) p1.y + 1.0 + (double) graphic.h / 2.0);
        this._thing.depth = new Depth(0.7f);
        DuckGame.Graphics.DrawRect(p1, p1 + new Vec2(x, y), new Color(70, 70, 70), new Depth(0.5f));
        DuckGame.Graphics.DrawRect(p1 + new Vec2(1f, 1f), p1 + new Vec2(x - 1f, y - 1f), new Color(30, 30, 30), new Depth(0.6f));
        DuckGame.Graphics.Draw(graphic.texture, new Vec2(this._thing.x, this._thing.y), new Rectangle?(), Color.White, 0.0f, this._thing.center, this._thing.scale, SpriteEffects.None, new Depth(0.7f));
        if (this._root && this._file != null)
        {
          Vec2 vec2_2 = new Vec2(p1 + new Vec2(x + 4f, 0.0f));
          Vec2 vec2_3 = new Vec2(p1 + new Vec2(x + 97f, 12f));
          this._file.position = vec2_2;
          this._file.Update();
          this._file.Draw();
        }
        int num1 = graphic.texture.width / graphic.w;
        int num2 = graphic.texture.height / graphic.h;
        if (Editor.gamepadMode && (this._file == null || !this._file.hover))
        {
          this._hoverPos = new Vec2((float) (this._selectedIndex % num1 * graphic.w), (float) (this._selectedIndex / num1 * graphic.h));
          if (Input.Pressed("LEFT"))
            --this._selectedIndex;
          if (Input.Pressed("RIGHT"))
          {
            if (this._file != null && this._selectedIndex == num1 - 1)
              this._file.hover = true;
            else
              ++this._selectedIndex;
          }
          if (Input.Pressed("UP"))
            this._selectedIndex -= num1;
          if (Input.Pressed("DOWN"))
            this._selectedIndex += num1;
          if (this._selectedIndex < 0)
            this._selectedIndex = 0;
          if (this._selectedIndex > num1 * num2 - 1)
            this._selectedIndex = num1 * num2 - 1;
        }
        else
          this._hoverPos = new Vec2(Mouse.x - this._thing.x, Mouse.y - this._thing.y);
        if (this._file != null && this._file.hover && Input.Pressed("LEFT"))
        {
          this._file.hover = false;
          this._selectedIndex = num1 - 1;
        }
        Editor current = Level.current as Editor;
        this._hoverPos.x = (float) Math.Round((double) this._hoverPos.x / (double) graphic.w) * (float) graphic.w;
        this._hoverPos.y = (float) Math.Round((double) this._hoverPos.y / (double) graphic.h) * (float) graphic.h;
        if ((this._file == null || !this._file.hover) && ((double) this._hoverPos.x >= 0.0 && (double) this._hoverPos.x < (double) graphic.texture.width) && ((double) this._hoverPos.y >= 0.0 && (double) this._hoverPos.y < (double) graphic.texture.height))
        {
          DuckGame.Graphics.DrawRect(this._hoverPos + p1, this._hoverPos + p1 + new Vec2((float) (graphic.w + 2), (float) (graphic.h + 2)), Color.Lime * 0.8f, new Depth(0.8f), false);
          if (Mouse.left == InputState.Pressed || Input.Pressed("SELECT"))
          {
            graphic.frame = (int) ((double) this._hoverPos.x / (double) graphic.w + (double) this._hoverPos.y / (double) graphic.h * (double) (graphic.texture.width / graphic.w));
            current.placementType = this._thing;
            current.placementType = this._thing;
            if (!this.floatMode || Editor.gamepadMode)
              current.CloseMenu();
          }
        }
        if (!this.justOpened && Input.Pressed("SHOOT"))
          current.CloseMenu();
        this.justOpened = false;
      }
      else
        this.justOpened = true;
    }
  }
}
