// Decompiled with JetBrains decompiler
// Type: DuckGame.Drum
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Drum : Thing
  {
    protected string _sound = "";
    protected string _alternateSound = "";
    protected float _shake;
    private SinWave _shakeWave = (SinWave) 1.1f;

    public Drum(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public void Hit()
    {
      SFX.Play(this._sound, 0.9f + Rando.Float(0.1f), Rando.Float(-0.05f, 0.05f));
      this._shake = 1f;
    }

    public void AlternateHit()
    {
      SFX.Play(this._alternateSound, 0.9f + Rando.Float(0.1f), Rando.Float(-0.05f, 0.05f));
      this._shake = 1f;
    }

    public override void Update()
    {
      this._shake = Lerp.Float(this._shake, 0.0f, 0.08f);
      base.Update();
    }

    public override void Draw()
    {
      this.position.x += (float) this._shakeWave * this._shake;
      base.Draw();
      this.position.x -= (float) this._shakeWave * this._shake;
    }
  }
}
