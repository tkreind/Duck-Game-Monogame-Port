﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NotifyDialogue
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NotifyDialogue : ContextMenu
  {
    private new string _text = "";
    public bool result;
    private BitmapFont _font;
    private bool _hoverOk;

    public NotifyDialogue()
      : base((IContextListener) null)
    {
    }

    public override void Initialize()
    {
      this.layer = Layer.HUD;
      this.depth = new Depth(0.95f);
      float num1 = 300f;
      float num2 = 40f;
      Vec2 vec2_1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0));
      Vec2 vec2_2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0));
      this.position = vec2_1 + new Vec2(4f, 20f);
      this.itemSize = new Vec2(490f, 16f);
      this._root = true;
      this._font = new BitmapFont("biosFont", 8);
    }

    public void Open(string text, string startingText = "")
    {
      this.opened = true;
      this._text = text;
      SFX.Play("openClick", 0.4f);
    }

    public void Close() => this.opened = false;

    public override void Selected(ContextMenu item)
    {
    }

    public override void Update()
    {
      if (!this.opened)
        return;
      if (this._opening)
      {
        this._opening = false;
        this._selectedIndex = 1;
      }
      if (Keyboard.Pressed(Keys.Enter))
      {
        this.result = true;
        this.opened = false;
      }
      if (Keyboard.Pressed(Keys.Escape) || Mouse.right == InputState.Pressed)
      {
        this.result = false;
        this.opened = false;
      }
      float num1 = 300f;
      float num2 = 80f;
      Vec2 vec2_1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0));
      Vec2 vec2_2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0));
      Vec2 vec2_3 = vec2_1 + new Vec2(18f, 28f);
      Vec2 vec2_4 = new Vec2(num1 - 40f, 40f);
      Vec2 vec2_5 = vec2_1 + new Vec2(160f, 28f);
      Vec2 vec2_6 = new Vec2(120f, 40f);
      this._hoverOk = (double) Mouse.x > (double) vec2_3.x && (double) Mouse.x < (double) vec2_3.x + (double) vec2_4.x && ((double) Mouse.y > (double) vec2_3.y && (double) Mouse.y < (double) vec2_3.y + (double) vec2_4.y);
      if (!Editor.tookInput && Input.Pressed("LEFT"))
        --this._selectedIndex;
      else if (!Editor.tookInput && Input.Pressed("RIGHT"))
        ++this._selectedIndex;
      if (this._selectedIndex < 0)
        this._selectedIndex = 0;
      if (this._selectedIndex > 1)
        this._selectedIndex = 1;
      if (Editor.gamepadMode)
      {
        this._hoverOk = false;
        if (this._selectedIndex == 0)
          this._hoverOk = true;
      }
      if (Editor.tookInput || !this._hoverOk || Mouse.left != InputState.Pressed && !Input.Pressed("SELECT"))
        return;
      this.result = true;
      this.opened = false;
      Editor.tookInput = true;
    }

    public override void Draw()
    {
      if (!this.opened)
        return;
      base.Draw();
      float num1 = 300f;
      float num2 = 80f;
      Vec2 p1_1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0));
      Vec2 p2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0));
      Graphics.DrawRect(p1_1, p2, new Color(70, 70, 70), this.depth, false);
      Graphics.DrawRect(p1_1, p2, new Color(30, 30, 30), this.depth - 1);
      Graphics.DrawRect(p1_1 + new Vec2(4f, 20f), p2 + new Vec2(-4f, -4f), new Color(10, 10, 10), this.depth + 1);
      Graphics.DrawRect(p1_1 + new Vec2(2f, 2f), new Vec2(p2.x - 2f, p1_1.y + 16f), new Color(70, 70, 70), this.depth + 1);
      Graphics.DrawString(this._text, p1_1 + new Vec2(5f, 5f), Color.White, this.depth + 2);
      this._font.scale = new Vec2(2f, 2f);
      Vec2 p1_2 = p1_1 + new Vec2(18f, 28f);
      Vec2 vec2 = new Vec2(num1 - 36f, 40f);
      Graphics.DrawRect(p1_2, p1_2 + vec2, this._hoverOk ? new Color(80, 80, 80) : new Color(30, 30, 30), this.depth + 2);
      this._font.Draw("OK", (float) ((double) p1_2.x + (double) vec2.x / 2.0 - (double) this._font.GetWidth("OK") / 2.0), p1_2.y + 12f, Color.White, this.depth + 3);
      Graphics.DrawString(this._text, p1_1 + new Vec2(5f, 5f), Color.White, this.depth + 2);
    }
  }
}
