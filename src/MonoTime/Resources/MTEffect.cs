// Decompiled with JetBrains decompiler
// Type: DuckGame.MTEffect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class MTEffect
  {
    private Effect _base;
    private short _effectIndex;
    private string _effectName;

    public short effectIndex => this._effectIndex;

    public string effectName => this._effectName;

    public void SetEffectIndex(short index) => this._effectIndex = index;

    public Effect effect => this._base;

    public MTEffect(Effect tex, string cureffectName, short cureffectIndex = 0)
    {
      this._base = tex;
      this._effectIndex = cureffectIndex;
      this._effectName = cureffectName;
    }

    public static implicit operator Effect(MTEffect tex) => tex?._base;

    public static implicit operator MTEffect(Effect tex) => Content.GetMTEffect(tex);
  }
}
