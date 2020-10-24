// Decompiled with JetBrains decompiler
// Type: DuckGame.BetaScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BetaScreen : Level
  {
    private FancyBitmapFont _font;
    private BitmapFont _bigFont;
    private float _wait = 1f;
    private bool _fading;
    private bool _drmSuccess;

    public BetaScreen() => this._centeredView = true;

    public override void Initialize()
    {
      this._drmSuccess = DG.InitializeDRM();
      this._font = new FancyBitmapFont("smallFont");
      this._bigFont = new BitmapFont("biosFont", 8);
      Graphics.fade = 0.0f;
    }

    public override void Update()
    {
      if (!this._fading)
      {
        if ((double) Graphics.fade < 1.0)
          Graphics.fade += 0.03f;
        else
          Graphics.fade = 1f;
      }
      else if ((double) Graphics.fade > 0.0)
      {
        Graphics.fade -= 0.03f;
      }
      else
      {
        Graphics.fade = 0.0f;
        Level.current = (Level) new TitleScreen();
      }
      this._wait -= 0.02f;
      if (DG.buildExpired || !this._drmSuccess || ((double) this._wait >= 0.0 || !Input.Pressed("START")))
        return;
      this._fading = true;
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer != Layer.Game)
        return;
      string text1 = "|DGYELLOW|HEY!";
      float ypos1 = 55f;
      this._font.Draw(text1, (float) ((double) layer.width / 2.0 - (double) this._font.GetWidth(text1) / 2.0), ypos1, Color.White);
      if (!this._drmSuccess)
      {
        float ypos2 = ypos1 + 10f;
        string text2 = "|WHITE|Woah! DRM is enabled since this is a pre release build.\nMake sure you're connected to steam, and that you're\nSupposed to have this build!";
        this._font.Draw(text2, (float) ((double) layer.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), ypos2, Color.White);
      }
      else if (DG.buildExpired)
      {
        float ypos2 = ypos1 + 10f;
        string text2 = "|WHITE|Sorry, this build was a limited beta build.\nIt appears to have expired X(.\nShould be easy to get around, or the game\nshould be out on steam now, go get it!";
        this._font.Draw(text2, (float) ((double) layer.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), ypos2, Color.White);
      }
      else
      {
        if (!DG.betaBuild)
          return;
        float ypos2 = ypos1 + 15f;
        string text2 = "|WHITE|This is a near final release of |RED|DUCK GAME|WHITE|!\n|WHITE|Some stuff is still getting finished up, so\nplease bear with me |PINK|{|WHITE|.";
        this._font.Draw(text2, (float) ((double) layer.width / 2.0 - (double) this._font.GetWidth(text2) / 2.0), ypos2, Color.White);
        string text3 = "|WHITE|Press @START@ to continue...";
        this._bigFont.Draw(text3, new Vec2((float) ((double) layer.width / 2.0 - (double) this._bigFont.GetWidth(text3) / 2.0), ypos2 + 55f), Color.White);
      }
    }
  }
}
