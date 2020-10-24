// Decompiled with JetBrains decompiler
// Type: DuckGame.PlusOne
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PlusOne : Thing
  {
    private BitmapFont _font;
    private Profile _profile;
    private bool _temp;
    private float _wait = 1f;

    public PlusOne(float xpos, float ypos, Profile p, bool temp = false)
      : base(xpos, ypos)
    {
      this._font = new BitmapFont("biosFont", 8);
      this._profile = p;
      this._temp = temp;
      this.layer = Layer.Blocks;
      this.depth = new Depth(0.9f);
    }

    public override void Initialize() => base.Initialize();

    public override void Update()
    {
      if (!this._temp)
        this._wait -= 0.01f;
      if ((double) this._wait >= 0.0)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      if (this._profile == null || this._profile.persona == null || this.anchor == (Thing) null)
        return;
      this.position = this.anchor.position;
      string text = "+1";
      float xpos = this.x - this._font.GetWidth(text) / 2f;
      this._font.Draw(text, xpos - 1f, this.y - 1f, Color.Black, new Depth(0.8f));
      this._font.Draw(text, xpos + 1f, this.y - 1f, Color.Black, new Depth(0.8f));
      this._font.Draw(text, xpos - 1f, this.y + 1f, Color.Black, new Depth(0.8f));
      this._font.Draw(text, xpos + 1f, this.y + 1f, Color.Black, new Depth(0.8f));
      Color c = new Color((byte) this._profile.persona.color.x, (byte) this._profile.persona.color.y, (byte) this._profile.persona.color.z);
      this._font.Draw(text, xpos, this.y, c, new Depth(0.9f));
    }
  }
}
