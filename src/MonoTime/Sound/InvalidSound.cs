// Decompiled with JetBrains decompiler
// Type: DuckGame.InvalidSound
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;

namespace DuckGame
{
  public class InvalidSound : Sound
  {
    private float _pitch;
    private float _pan;
    private bool _isLooped;

    public override float Pitch
    {
      get => this._pitch;
      set => this._pitch = value;
    }

    public override float Pan
    {
      get => this._pan;
      set => this._pan = value;
    }

    public override bool IsLooped
    {
      get => this._isLooped;
      set => this._isLooped = value;
    }

    public override SoundState State => SoundState.Stopped;

    public override float Volume
    {
      get => Math.Min(1f, Math.Max(0.0f, this._volume));
      set => this._volume = Math.Min(1f, Math.Max(0.0f, value));
    }

    public override void Play()
    {
    }

    public override void Stop()
    {
    }

    public override void Unpooled()
    {
    }

    public override void Pause()
    {
    }

    public InvalidSound(string sound, float vol, float pitch, float pan, bool looped)
    {
      this._name = sound;
      this._volume = vol;
      this._pitch = pitch;
      this._pan = pan;
      this._isLooped = looped;
    }
  }
}
