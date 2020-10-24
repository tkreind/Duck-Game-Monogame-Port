// Decompiled with JetBrains decompiler
// Type: DuckGame.BIOSScreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BIOSScreen : Level
  {
    private BitmapFont _font;
    private float _wait = 1f;
    private bool _playedMusic;
    private float _moveWait = 1f;
    private float _shiftText;
    private int great;

    public BIOSScreen() => this._centeredView = true;

    public override void Initialize()
    {
      if (!Steam.IsInitialized())
      {
        if (!MonoMain.breakSteam)
          Steam.InitializeCore();
        Steam.Initialize();
      }
      this._font = new BitmapFont("biosFont", 8);
      base.Initialize();
    }

    public override void Update()
    {
      this._wait -= 0.008f;
      if ((double) this._wait >= 0.0)
        return;
      if (!this._playedMusic)
      {
        Music.Play("Title");
        this._playedMusic = true;
      }
      this._moveWait -= 0.015f;
      if ((double) this._moveWait >= 0.0)
        return;
      this._shiftText += 3.5f;
      if ((double) this._shiftText <= 300.0)
        return;
      Level.current = (Level) new CorptronLogo();
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer != Layer.Game)
        return;
      this._font.Draw("   PRODUCED BY OR", 80f + this._shiftText, 66f, Color.White);
      this._font.Draw(" UNDER LICENSE FROM", 80f - this._shiftText, 82f, Color.White);
      this._font.Draw("CORPTRON SYSTEMS LTD.", 80f + this._shiftText, 98f, Color.White);
    }
  }
}
