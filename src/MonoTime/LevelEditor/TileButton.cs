// Decompiled with JetBrains decompiler
// Type: DuckGame.TileButton
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class TileButton : Thing
  {
    private FieldBinding _binding;
    private FieldBinding _visibleBinding;
    private SpriteMap _sprite;
    private string _hoverText;
    private bool _hover;
    private InputProfile _focus;
    private TileButtonAlign _align;
    private Vec2 _alignOffset = Vec2.Zero;

    public override bool visible => this._visibleBinding == null ? base.visible : (bool) this._visibleBinding.value;

    public string hoverText => this._hoverText;

    public bool hover
    {
      get => this._hover;
      set => this._hover = value;
    }

    public InputProfile focus
    {
      get => this._focus;
      set => this._focus = value;
    }

    public TileButton(
      float xpos,
      float ypos,
      FieldBinding binding,
      FieldBinding visibleBinding,
      SpriteMap image,
      string hover,
      TileButtonAlign align = TileButtonAlign.None,
      float angleDeg = 0.0f)
      : base(xpos, ypos)
    {
      this._sprite = image;
      this._hoverText = hover;
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      image.center = new Vec2((float) (image.w / 2), (float) (image.h / 2));
      this._binding = binding;
      this._visibleBinding = visibleBinding;
      this._align = align;
      this._alignOffset = new Vec2(xpos, ypos);
      this.angleDegrees = angleDeg;
    }

    public override void Update()
    {
      this.position = (this._binding.thing as Editor).GetAlignOffset(this._align) + this._alignOffset;
      if (this._focus == null)
        return;
      if (this._binding.value.GetType() == typeof (float))
      {
        if (this._focus.Pressed("LEFT") || Keyboard.Pressed(Keys.Left))
        {
          this._binding.value = (object) Math.Max((float) this._binding.value - this._binding.inc, this._binding.min);
        }
        else
        {
          if (!this._focus.Pressed("RIGHT") && !Keyboard.Pressed(Keys.Right))
            return;
          this._binding.value = (object) Math.Min((float) this._binding.value + this._binding.inc, this._binding.max);
        }
      }
      else if (this._binding.value.GetType() == typeof (int))
      {
        if (this._focus.Pressed("LEFT") || Keyboard.Pressed(Keys.Left))
        {
          this._binding.value = (object) (int) Math.Max((float) (int) this._binding.value - this._binding.inc, this._binding.min);
        }
        else
        {
          if (!this._focus.Pressed("RIGHT") && !Keyboard.Pressed(Keys.Right))
            return;
          this._binding.value = (object) (int) Math.Min((float) (int) this._binding.value + this._binding.inc, this._binding.max);
        }
      }
      else if (this._binding.value.GetType() == typeof (Vec2))
      {
        if (this._focus.Pressed("LEFT") || Keyboard.Pressed(Keys.Left))
        {
          Vec2 vec2 = (Vec2) this._binding.value;
          vec2.x = Math.Max(vec2.x - this._binding.inc, this._binding.min);
          this._binding.value = (object) vec2;
        }
        else if (this._focus.Pressed("RIGHT") || Keyboard.Pressed(Keys.Right))
        {
          Vec2 vec2 = (Vec2) this._binding.value;
          vec2.x = Math.Min(vec2.x + this._binding.inc, this._binding.max);
          this._binding.value = (object) vec2;
        }
        else if (this._focus.Pressed("UP") || Keyboard.Pressed(Keys.Up))
        {
          Vec2 vec2 = (Vec2) this._binding.value;
          vec2.y = Math.Max(vec2.y - this._binding.inc, this._binding.min);
          this._binding.value = (object) vec2;
        }
        else
        {
          if (!this._focus.Pressed("DOWN") && !Keyboard.Pressed(Keys.Down))
            return;
          Vec2 vec2 = (Vec2) this._binding.value;
          vec2.y = Math.Min(vec2.y + this._binding.inc, this._binding.max);
          this._binding.value = (object) vec2;
        }
      }
      else
      {
        if (!(this._binding.value.GetType() == typeof (bool)) || !this._focus.Pressed("SELECT") && Mouse.left != InputState.Pressed)
          return;
        this._binding.value = (object) !(bool) this._binding.value;
      }
    }

    public override void Draw()
    {
      this._sprite.frame = this._hover ? 1 : 0;
      this._sprite.angle = this.angle;
      if (this._binding.value.GetType() == typeof (bool))
      {
        bool flag = (bool) this._binding.value;
        this._sprite.color = Color.White * (flag ? 1f : 0.5f);
      }
      Graphics.Draw((Sprite) this._sprite, this.x, this.y);
      if (this._binding.value.GetType() == typeof (float))
        Graphics.DrawString(((float) this._binding.value).ToString("0.00"), new Vec2(this.x + 12f, this.y - 4f), Color.White);
      if (this._binding.value.GetType() == typeof (int))
        Graphics.DrawString(((int) this._binding.value).ToString(), new Vec2(this.x + 12f, this.y - 4f), Color.White);
      this._hover = false;
      base.Draw();
    }
  }
}
