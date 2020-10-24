// Decompiled with JetBrains decompiler
// Type: DuckGame.UIBox
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UIBox : UIComponent
  {
    private SpriteMap _sections;
    private float _seperation = 1f;
    public string _hoverControlString;
    private bool _borderVisible = true;
    protected int _selection;
    public bool _isMenu;
    public UIMenuItem _backButton;
    private bool _willSelectLast;

    public int selection => this._selection;

    public UIBox(float xpos, float ypos, float wide = -1f, float high = -1f, bool vert = true, bool isVisible = true)
      : base(xpos, ypos, wide, high)
    {
      this._sections = new SpriteMap("uiBox", 10, 10);
      this._vertical = vert;
      this._borderVisible = isVisible;
      this.borderSize = this._borderVisible ? new Vec2(8f, 8f) : Vec2.Zero;
      this._canFit = true;
    }

    public UIBox(bool vert = true, bool isVisible = true)
      : base(0.0f, 0.0f, -1f, -1f)
    {
      this._sections = new SpriteMap("uiBox", 10, 10);
      this._vertical = vert;
      this._borderVisible = isVisible;
      this.borderSize = this._borderVisible ? new Vec2(8f, 8f) : Vec2.Zero;
      this._canFit = true;
    }

    public override void Add(UIComponent component, bool doAnchor = true)
    {
      if (component is UIMenuItem)
      {
        this._isMenu = true;
        if ((component as UIMenuItem).isBackButton)
          this._backButton = component as UIMenuItem;
      }
      base.Add(component, doAnchor);
    }

    public override void Insert(UIComponent component, int position, bool doAnchor = true)
    {
      if (component is UIMenuItem)
      {
        this._isMenu = true;
        if ((component as UIMenuItem).isBackButton)
          this._backButton = component as UIMenuItem;
      }
      base.Insert(component, position, doAnchor);
    }

    public override void Open()
    {
      Graphics.fade = 1f;
      this._selection = 0;
      if (this._willSelectLast)
        this._selection = this._components.Where<UIComponent>((Func<UIComponent, bool>) (val => val is UIMenuItem)).ToList<UIComponent>().Count - 1;
      base.Open();
    }

    protected override void SizeChildren()
    {
      foreach (UIComponent component in this._components)
      {
        if (component.canFit)
        {
          if (this.vertical)
            component.collisionSize = new Vec2(this.collisionSize.x - this.borderSize.x * 2f, component.collisionSize.y);
          else
            component.collisionSize = new Vec2(component.collisionSize.x, this.collisionSize.y - this.borderSize.y * 2f);
        }
      }
    }

    protected override void OnResize()
    {
      if (this._vertical)
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (UIComponent component in this._components)
        {
          num2 += component.collisionSize.y + this._seperation;
          if ((double) component.collisionSize.x > (double) num1)
            num1 = component.collisionSize.x;
        }
        float num3 = num1 + this.borderSize.x * 2f;
        float num4 = num2 - this._seperation + this.borderSize.y * 2f;
        if (this._autoSizeHor && (this.fit & UIFit.Horizontal) == UIFit.None && (double) num3 > (double) this._collisionSize.x)
          this._collisionSize.x = num3;
        if (this._autoSizeVert && (this.fit & UIFit.Vertical) == UIFit.None && (double) num4 > (double) this._collisionSize.y)
          this._collisionSize.y = num4;
        float num5 = (float) (-(double) num4 / 2.0) + this.borderSize.y;
        foreach (UIComponent component in this._components)
        {
          component.anchor.offset.x = 0.0f;
          if ((component.align & UIAlign.Left) > UIAlign.Center)
            component.anchor.offset.x = (float) (-(double) this.collisionSize.x / 2.0 + (double) this.borderSize.x + (double) component.collisionSize.x / 2.0);
          else if ((component.align & UIAlign.Right) > UIAlign.Center)
            component.anchor.offset.x = (float) ((double) this.collisionSize.x / 2.0 - (double) this.borderSize.x - (double) component.collisionSize.x / 2.0);
          component.anchor.offset.y = (float) ((double) num5 * (double) this.scale.y + (double) component.height / 2.0);
          num5 += component.collisionSize.y + this._seperation;
        }
      }
      else
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (UIComponent component in this._components)
        {
          num1 += component.collisionSize.x + this._seperation;
          if ((double) component.collisionSize.y > (double) num2)
            num2 = component.collisionSize.y;
        }
        float num3 = num2 + this.borderSize.y * 2f;
        float num4 = num1 - this._seperation + this.borderSize.x * 2f;
        if (this._autoSizeHor && (this.fit & UIFit.Horizontal) == UIFit.None && (double) num4 > (double) this._collisionSize.x)
          this._collisionSize.x = num4;
        if (this._autoSizeVert && (this.fit & UIFit.Vertical) == UIFit.None && (double) num3 > (double) this._collisionSize.y)
          this._collisionSize.y = num3;
        float num5 = (float) (-(double) num4 / 2.0) + this.borderSize.x;
        foreach (UIComponent component in this._components)
        {
          component.anchor.offset.x = (float) ((double) num5 * (double) this.scale.x + (double) component.width / 2.0);
          component.anchor.offset.y = 0.0f;
          num5 += component.collisionSize.x + this._seperation;
        }
      }
    }

    public virtual void SelectLastMenuItem()
    {
      this._selection = this._components.Where<UIComponent>((Func<UIComponent, bool>) (val => val is UIMenuItem)).ToList<UIComponent>().Count - 1;
      this._willSelectLast = true;
    }

    public override void Update()
    {
      if (!UIMenu.globalUILock && this._isMenu && !this._close)
      {
        bool flag = false;
        if (this._vertical)
        {
          if (!this._animating && Input.Pressed("UP"))
          {
            --this._selection;
            flag = true;
          }
          if (!this._animating && Input.Pressed("DOWN"))
          {
            ++this._selection;
            flag = true;
          }
        }
        else
        {
          if (!this._animating && Input.Pressed("LEFT"))
          {
            --this._selection;
            flag = true;
          }
          if (!this._animating && Input.Pressed("RIGHT"))
          {
            ++this._selection;
            flag = true;
          }
        }
        if (this._backButton != null && !this._animating && Input.Pressed("QUACK"))
          this._backButton.Activate("SELECT");
        List<UIComponent> list = this._components.Where<UIComponent>((Func<UIComponent, bool>) (val => val is UIMenuItem)).ToList<UIComponent>();
        if (this._selection >= list.Count)
          this._selection = 0;
        else if (this._selection < 0)
          this._selection = list.Count - 1;
        if (flag)
          SFX.Play("textLetter", 0.7f);
        this._hoverControlString = (string) null;
        for (int index = 0; index < list.Count; ++index)
        {
          UIMenuItem uiMenuItem = list[index] as UIMenuItem;
          uiMenuItem.selected = index == this._selection;
          if (index == this._selection)
          {
            this._hoverControlString = uiMenuItem.controlString;
            if (uiMenuItem.isEnabled)
            {
              if (!this._animating && (Input.Pressed("SELECT") || Input.Pressed("SHOOT") || Input.Pressed("START")))
              {
                uiMenuItem.Activate("SELECT");
                SFX.Play("rockHitGround", 0.7f);
              }
              else if (!this._animating && Input.Pressed("GRAB"))
                uiMenuItem.Activate("GRAB");
              else if (!this._animating && Input.Pressed("LEFT"))
                uiMenuItem.Activate("LEFT");
              else if (!this._animating && Input.Pressed("RIGHT"))
                uiMenuItem.Activate("RIGHT");
            }
          }
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      if (this._borderVisible)
      {
        this._sections.scale = this.scale;
        this._sections.alpha = this.alpha;
        this._sections.depth = this.depth;
        this._sections.frame = 0;
        Graphics.Draw((Sprite) this._sections, -this.halfWidth + this.x, -this.halfHeight + this.y);
        this._sections.frame = 2;
        Graphics.Draw((Sprite) this._sections, (float) ((double) this.halfWidth + (double) this.x - (double) this._sections.w * (double) this.scale.x), -this.halfHeight + this.y);
        this._sections.frame = 1;
        this._sections.xscale = (this._collisionSize.x - (float) (this._sections.w * 2)) / (float) this._sections.w * this.xscale;
        Graphics.Draw((Sprite) this._sections, (float) (-(double) this.halfWidth + (double) this.x + (double) this._sections.w * (double) this.scale.x), -this.halfHeight + this.y);
        this._sections.xscale = this.xscale;
        this._sections.frame = 3;
        this._sections.yscale = (this._collisionSize.y - (float) (this._sections.h * 2)) / (float) this._sections.h * this.yscale;
        Graphics.Draw((Sprite) this._sections, -this.halfWidth + this.x, (float) (-(double) this.halfHeight + (double) this.y + (double) this._sections.h * (double) this.scale.y));
        this._sections.frame = 5;
        Graphics.Draw((Sprite) this._sections, (float) ((double) this.halfWidth + (double) this.x - (double) this._sections.w * (double) this.scale.x), (float) (-(double) this.halfHeight + (double) this.y + (double) this._sections.h * (double) this.scale.y));
        this._sections.frame = 4;
        this._sections.xscale = (this._collisionSize.x - (float) (this._sections.w * 2)) / (float) this._sections.w * this.xscale;
        Graphics.Draw((Sprite) this._sections, (float) (-(double) this.halfWidth + (double) this.x + (double) this._sections.w * (double) this.scale.x), (float) (-(double) this.halfHeight + (double) this.y + (double) this._sections.h * (double) this.scale.y));
        this._sections.xscale = this.xscale;
        this._sections.yscale = this.yscale;
        this._sections.frame = 6;
        Graphics.Draw((Sprite) this._sections, -this.halfWidth + this.x, (float) ((double) this.halfHeight + (double) this.y - (double) this._sections.h * (double) this.scale.y));
        this._sections.frame = 8;
        Graphics.Draw((Sprite) this._sections, (float) ((double) this.halfWidth + (double) this.x - (double) this._sections.w * (double) this.scale.x), (float) ((double) this.halfHeight + (double) this.y - (double) this._sections.h * (double) this.scale.y));
        this._sections.frame = 7;
        this._sections.xscale = (this._collisionSize.x - (float) (this._sections.w * 2)) / (float) this._sections.w * this.xscale;
        Graphics.Draw((Sprite) this._sections, (float) (-(double) this.halfWidth + (double) this.x + (double) this._sections.w * (double) this.scale.x), (float) ((double) this.halfHeight + (double) this.y - (double) this._sections.h * (double) this.scale.y));
      }
      base.Draw();
    }
  }
}
