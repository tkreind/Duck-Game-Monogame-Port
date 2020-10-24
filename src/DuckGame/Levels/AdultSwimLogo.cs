// Decompiled with JetBrains decompiler
// Type: DuckGame.AdultSwimLogo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class AdultSwimLogo : Level
  {
    private BitmapFont _font;
    private Sprite _logo;
    private float _wait = 1f;
    private bool _fading;

    public override void Initialize()
    {
      this._font = new BitmapFont("biosFont", 8);
      this._logo = new Sprite("aslogo");
      Graphics.fade = 0.0f;
    }

    public override void Update()
    {
      if (!this._fading)
      {
        if ((double) Graphics.fade < 1.0)
          Graphics.fade += 0.013f;
        else
          Graphics.fade = 1f;
      }
      else if ((double) Graphics.fade > 0.0)
      {
        Graphics.fade -= 0.013f;
      }
      else
      {
        Graphics.fade = 0.0f;
        Level.current = !MonoMain.startInEditor ? (Level) new TitleScreen() : (Level) Main.editor;
      }
      this._wait -= 3f / 500f;
      if ((double) this._wait >= 0.0 && !Input.Pressed("START") && !Input.Pressed("SELECT"))
        return;
      this._fading = true;
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer != Layer.Game)
        return;
      this._logo.scale = new Vec2(0.25f, 0.25f);
      Graphics.Draw(this._logo, 110f, 38f);
    }
  }
}
