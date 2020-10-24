﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.TextEntryDialog
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

// TODO: Depracated GamerServicess
// using Microsoft.Xna.Framework.GamerServices;
using System;
using System.IO;

namespace DuckGame
{
  public class TextEntryDialog : ContextMenu
  {
    private new string _text = "";
    public string result;
    public FancyBitmapFont _fancyFont;
    public float _cursorFlash;
    private MysteryTextbox _textbox;
    private char[] invalidPathChars;
    private int _maxChars = 30;
    private string _default = "";

    public TextEntryDialog()
      : base((IContextListener) null)
    {
    }

    private void DoStuff(IAsyncResult r)
    {
      // TODO: Depracated GamerServicess
      // this.result = Guide.EndShowKeyboardInput(r);
      this.opened = false;
      Editor.PopFocus();
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
      this.invalidPathChars = Path.GetInvalidPathChars();
      this._fancyFont = new FancyBitmapFont("smallFont");
      this._textbox = new MysteryTextbox(vec2_1.x + 4f, vec2_1.y + 4f, num1 - 20f, num2 - 10f);
      this._textbox.enterConfirms = true;
    }

    public void Open(string text, string startingText = "", int maxChars = 30)
    {
      this.opened = true;
      this._text = text;
      this._default = startingText;
      Keyboard.keyString = "";
      Editor.enteringText = true;
      this._maxChars = maxChars;
      Editor.PushFocus((object) this);
      SFX.Play("openClick", 0.4f);
      this._textbox.text = this._default;
      this._textbox._cursorPosition = this._textbox.text.Length;
      this._textbox.color = Color.White;
      this._textbox.cursorColor = Color.White;
    }

    public void Close()
    {
      Editor.enteringText = false;
      Editor.PopFocus();
      this.opened = false;
    }

    public override void Selected(ContextMenu item)
    {
    }

    public override void Update()
    {
      if (!this.opened)
        return;
      this._textbox.Update();
      if (this._textbox.confirmed)
      {
        this._textbox.confirmed = false;
        this.result = this._textbox.text;
        this.opened = false;
        Editor.skipFrame = true;
        Editor.PopFocus();
        Editor.enteringText = false;
      }
      if (!Keyboard.Pressed(Keys.Escape) && Mouse.right != InputState.Pressed && !Input.Pressed("QUACK"))
        return;
      this.result = this._default;
      this.opened = false;
      Editor.PopFocus();
      Editor.skipFrame = true;
      Editor.enteringText = false;
    }

    public override void Draw()
    {
      if (!this.opened)
        return;
      base.Draw();
      float num1 = 300f;
      float num2 = 72f;
      Vec2 p1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0));
      Vec2 p2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0), (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0));
      Graphics.DrawRect(p1, p2, new Color(70, 70, 70), this.depth, false, 0.95f);
      Graphics.DrawRect(p1, p2, new Color(30, 30, 30), this.depth - 1);
      Graphics.DrawRect(p1 + new Vec2(4f, 20f), p2 + new Vec2(-4f, -4f), new Color(10, 10, 10), this.depth + 1);
      Graphics.DrawRect(p1 + new Vec2(2f, 2f), new Vec2(p2.x - 2f, p1.y + 16f), new Color(70, 70, 70), this.depth + 1);
      this._textbox.depth = this.depth + 20;
      this._textbox.Draw();
      Graphics.DrawString(this._text, p1 + new Vec2(5f, 5f), Color.White, this.depth + 2);
      Graphics.DrawRect(new Vec2(p2.x - 145f, p2.y), new Vec2(p2.x, p2.y + 10f), new Color(70, 70, 70), this.depth + 8);
      if (!(InputProfile.FirstProfileWithDevice.lastActiveDevice is Keyboard))
        Graphics.DrawString("@ENTERKEY@ACCEPT  @QUACK@CANCEL", p2 + new Vec2(-147f, 1f), Color.White, this.depth + 10);
      else
        Graphics.DrawString("@ENTERKEY@ACCEPT  @ESCAPE@CANCEL", p2 + new Vec2(-147f, 1f), Color.White, this.depth + 10);
    }
  }
}
