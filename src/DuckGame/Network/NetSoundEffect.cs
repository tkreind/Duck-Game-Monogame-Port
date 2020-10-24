// Decompiled with JetBrains decompiler
// Type: DuckGame.NetSoundEffect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NetSoundEffect
  {
    public NetSoundEffect.Function function;
    public FieldBinding pitchBinding;
    public FieldBinding appendBinding;
    private List<string> _sounds = new List<string>();
    private List<string> _rareSounds = new List<string>();
    public float volume = 1f;
    public float pitch;
    public float pitchVariationHigh;
    public float pitchVariationLow;
    private int _localIndex;
    private int _index;

    public NetSoundEffect()
    {
    }

    public NetSoundEffect(params string[] sounds) => this._sounds = new List<string>((IEnumerable<string>) sounds);

    public NetSoundEffect(List<string> sounds, List<string> rareSounds)
    {
      this._sounds = sounds;
      this._rareSounds = rareSounds;
    }

    public int index
    {
      get => this._index;
      set
      {
        this._index = value;
        if (this._localIndex == this._index)
          return;
        this._localIndex = this._index;
        this.PlaySound();
      }
    }

    public void Play(float vol = 1f, float pit = 0.0f)
    {
      this.PlaySound(vol, pit);
      ++this._index;
      if (this._index > 3)
        this._index = 0;
      this._localIndex = this._index;
    }

    private void PlaySound(float vol = 1f, float pit = 0.0f)
    {
      if (this.function != null)
        this.function();
      vol *= this.volume;
      pit += this.pitch;
      pit += Rando.Float(this.pitchVariationLow, this.pitchVariationHigh);
      if ((double) pit < -1.0)
        pit = -1f;
      if ((double) pit > 1.0)
        pit = 1f;
      if (this._sounds.Count <= 0)
        return;
      if (this.pitchBinding != null)
        pit = (float) (byte) this.pitchBinding.value / (float) byte.MaxValue;
      string str = "";
      if (this.appendBinding != null)
        str = ((byte) this.appendBinding.value).ToString();
      if (this._rareSounds.Count > 0 && (double) Rando.Float(1f) > 0.899999976158142)
        SFX.Play(this._rareSounds[Rando.Int(this._rareSounds.Count - 1)] + str, vol, pit);
      else
        SFX.Play(this._sounds[Rando.Int(this._sounds.Count - 1)] + str, vol, pit);
    }

    public delegate void Function();
  }
}
