// Decompiled with JetBrains decompiler
// Type: DuckGame.MainOptions
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class MainOptions : Thing
  {
    private List<string> _options;
    private BitmapFont _font = new BitmapFont("biosFont", 8);
    private float _menuWidth;

    public MainOptions(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.layer = Layer.HUD;
      this._font.scale = new Vec2(4f, 4f);
      this._options = new List<string>()
      {
        "MULTIPLAYER",
        "OPTIONS",
        "QUIT"
      };
      float num = 0.0f;
      foreach (string option in this._options)
      {
        float width = this._font.GetWidth(option);
        if ((double) width > (double) num)
          num = width;
      }
      this._menuWidth = num + 80f;
    }

    public override void Draw()
    {
      Graphics.DrawRect(new Vec2((float) ((double) Graphics.width / 2.0 - (double) this._menuWidth / 2.0), this.y), new Vec2((float) ((double) Graphics.width / 2.0 + (double) this._menuWidth / 2.0), this.y + 250f), Color.Black, (Depth) 0.9f);
      int num = 0;
      foreach (string option in this._options)
      {
        float width = this._font.GetWidth(option);
        this._font.Draw(option, (float) ((double) Graphics.width / 2.0 - (double) width / 2.0), this.y + 30f + (float) (num * 60), Color.White);
        ++num;
      }
    }
  }
}
