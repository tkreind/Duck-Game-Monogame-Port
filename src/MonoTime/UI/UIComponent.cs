// Decompiled with JetBrains decompiler
// Type: DuckGame.UIComponent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class UIComponent : Thing
  {
    public bool debug;
    protected UIComponent _parent;
    public bool isEnabled = true;
    protected bool _didResize;
    protected bool _dirty;
    protected Vec2 _offset = Vec2.Zero;
    protected bool _vertical;
    protected List<UIComponent> _components = new List<UIComponent>();
    protected bool _canFit;
    private UIFit _fit;
    private UIAlign _align;
    public Vec2 borderSize = Vec2.Zero;
    private bool _animate;
    protected bool _close;
    protected bool _animating;
    private Vec2 _startPosition;
    private bool _startInitialized;
    private bool _initialSizingComplete;
    protected bool _autoSizeVert;
    protected bool _autoSizeHor;
    public bool inWorld;

    public UIComponent parent => this._parent;

    public UIMenu rootMenu
    {
      get
      {
        if (this is UIMenu)
          return this as UIMenu;
        return this._parent != null ? this._parent.rootMenu : (UIMenu) null;
      }
    }

    public bool dirty
    {
      get => this._dirty;
      set => this._dirty = value;
    }

    public Vec2 offset
    {
      get => this._offset;
      set => this._offset = value;
    }

    public bool vertical
    {
      get => this._vertical;
      set => this._vertical = value;
    }

    public IList<UIComponent> components => (IList<UIComponent>) this._components;

    public bool canFit => this._canFit;

    public UIFit fit
    {
      get => this._fit;
      set
      {
        if (this._fit != value)
          this._dirty = true;
        this._fit = value;
      }
    }

    public UIAlign align
    {
      get => this._align;
      set
      {
        if (this._align != value)
          this._dirty = true;
        this._align = value;
      }
    }

    public bool animate => this._animate;

    public bool open => !this._close;

    public bool animating
    {
      get => this._animating;
      set
      {
        foreach (UIComponent component in this._components)
          component.animating = value;
        this._animating = value;
      }
    }

    public bool autoSizeVert => this._autoSizeVert;

    public bool autoSizeHor => this._autoSizeHor;

    public UIComponent(float xpos, float ypos, float wide, float high)
      : base(xpos, ypos)
    {
      this._collisionSize = new Vec2(wide, high);
      this.layer = Layer.HUD;
      this.depth = (Depth) 0.0f;
      this._autoSizeHor = (double) wide < 0.0;
      this._autoSizeVert = (double) high < 0.0;
    }

    public virtual void Open()
    {
      this._close = false;
      this.animating = true;
      foreach (UIComponent component in this._components)
      {
        if (component.anchor == (Thing) this)
          component.Open();
      }
    }

    public virtual void Close()
    {
      this._close = true;
      this.animating = true;
      foreach (UIComponent component in this._components)
        component.Close();
      if (!this.inWorld && this.rootMenu == this && !MonoMain.closeMenuUpdate.Contains(this))
        MonoMain.closeMenuUpdate.Add(this);
      this.OnClose();
    }

    public override void DoUpdate() => base.DoUpdate();

    public virtual void OnClose()
    {
    }

    public override void Added(Level parent)
    {
      this.inWorld = true;
      base.Added(parent);
    }

    public virtual void UpdateParts()
    {
    }

    public override void Update()
    {
      if (!this._startInitialized)
      {
        this._startInitialized = true;
        this._startPosition = this.position;
        this.position.y = this.layer.camera.height * 2f;
      }
      if (this.anchor == (Thing) null)
      {
        float to = this._close ? this.layer.camera.height * 2f : this._startPosition.y;
        this.position.y = Lerp.FloatSmooth(this.position.y, to, 0.2f, 1.05f);
        bool flag = (double) this.position.y != (double) to;
        if (this.animating != flag)
          this.animating = flag;
      }
      if (this.open && !this.animating)
        this.UpdateParts();
      if (this._parent != null || this.open || this.animating)
      {
        this.SizeChildren();
        foreach (UIComponent component in this._components)
        {
          component.DoUpdate();
          if (component._didResize)
            this._dirty = true;
          component._didResize = false;
        }
      }
      if (this._dirty)
        this.Resize();
      this._dirty = false;
      this._initialSizingComplete = true;
    }

    protected virtual void SizeChildren()
    {
    }

    public override void DoDraw()
    {
      if (!this._initialSizingComplete || !this._animating && this._close)
        return;
      base.DoDraw();
    }

    public override void Draw()
    {
      foreach (UIComponent component in this._components)
      {
        component.depth = this.depth + 10;
        if (component.visible)
          component.Draw();
      }
      int num = this.debug ? 1 : 0;
    }

    public void Resize()
    {
      this._dirty = false;
      this._didResize = true;
      this.OnResize();
    }

    protected virtual void OnResize()
    {
    }

    public virtual void Add(UIComponent component, bool doAnchor = true)
    {
      this._components.Add(component);
      component._parent = this;
      this._dirty = true;
      component.dirty = true;
      if (!doAnchor)
        return;
      component.anchor = (Anchor) (Thing) this;
    }

    public virtual void Insert(UIComponent component, int position, bool doAnchor = true)
    {
      this._components.Insert(position, component);
      component._parent = this;
      this._dirty = true;
      component.dirty = true;
      if (!doAnchor)
        return;
      component.anchor = (Anchor) (Thing) this;
    }

    public virtual void Remove(UIComponent component)
    {
      this._components.Remove(component);
      if (component._parent == this)
        component._parent = (UIComponent) null;
      if (component.anchor == (Thing) this)
        component.anchor = (Anchor) null;
      this._dirty = true;
    }
  }
}
