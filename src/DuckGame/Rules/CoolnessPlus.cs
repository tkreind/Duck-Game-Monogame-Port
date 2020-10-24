﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CoolnessPlus
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CoolnessPlus : Thing
  {
    private BitmapFont _font;
    private Profile _profile;
    private float _wait = 1f;
    private int _change;

    public float wait
    {
      get => this._wait;
      set => this._wait = value;
    }

    public int change
    {
      get => this._change;
      set
      {
        this._change = value;
        this._wait = 1f;
      }
    }

    public CoolnessPlus(float xpos, float ypos, Duck d, int c)
      : base(xpos, ypos)
    {
      this._font = new BitmapFont("biosFont", 8);
      this.change = c;
      this.anchor = (Anchor) (Thing) d;
      this.anchor.offset = new Vec2(-0.0f, -24f);
      this._profile = d.profile;
    }

    public override void Initialize() => SFX.Play("scoreDing", 0.8f);

    public override void Update()
    {
      this._wait -= 0.01f;
      if ((double) this._wait >= 0.0)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      this.position = this.anchor.position;
      string str = "";
      if (this.change > 0)
        str = "+";
      string text = str + this.change.ToString();
      float xpos = this.x - this._font.GetWidth(text) / 2f;
      this._font.Draw(text, xpos - 1f, this.y - 1f, Color.Black, (Depth) 0.8f);
      this._font.Draw(text, xpos + 1f, this.y - 1f, Color.Black, (Depth) 0.8f);
      this._font.Draw(text, xpos - 1f, this.y + 1f, Color.Black, (Depth) 0.8f);
      this._font.Draw(text, xpos + 1f, this.y + 1f, Color.Black, (Depth) 0.8f);
      Color c = new Color((byte) this._profile.persona.color.x, (byte) this._profile.persona.color.y, (byte) this._profile.persona.color.z);
      this._font.Draw(text, xpos, this.y, c, (Depth) 0.9f);
    }
  }
}
