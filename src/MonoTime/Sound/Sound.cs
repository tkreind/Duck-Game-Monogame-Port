// Decompiled with JetBrains decompiler
// Type: DuckGame.Sound
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;

namespace DuckGame
{
  public class Sound
  {
    protected bool _killed;
    protected bool _cannotBeCancelled;
    protected SoundEffectInstance _instance;
    protected bool _pooled;
    protected string _name = "";
    protected float _volume = 1f;

    public virtual void Kill()
    {
      this.Stop();
      this._killed = true;
    }

    public bool cannotBeCancelled
    {
      get => this._cannotBeCancelled;
      set => this._cannotBeCancelled = value;
    }

    public virtual bool IsDisposed => this._instance.IsDisposed;

    public virtual float Pitch
    {
      get => this._instance.Pitch;
      set => this._instance.Pitch = value;
    }

    public virtual float Pan
    {
      get => this._instance.Pan;
      set => this._instance.Pan = value;
    }

    public virtual bool IsLooped
    {
      get => this._instance.IsLooped;
      set => this._instance.IsLooped = value;
    }

    public virtual SoundState State => this._instance.State;

    public string name => this._name;

    public virtual float Volume
    {
      get => Math.Min(1f, Math.Max(0.0f, this._volume));
      set => this._volume = this._instance.Volume = Math.Min(1f, Math.Max(0.0f, value));
    }

    public virtual void Play()
    {
      if (this._killed || !SFX.PoolSound(this))
        return;
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogSound(this.name, this._volume, this.Pitch, this.Pan);
      if (Recorder.globalRecording != null)
        Recorder.globalRecording.LogSound(this.name, this._volume, this.Pitch, this.Pan);
      this._instance.Volume = Math.Min(1f, Math.Max(0.0f, this._volume * SFX.volume));
      this._instance.Play();
      this._pooled = true;
    }

    public virtual void Stop()
    {
      if (this._killed)
        return;
      if (this.State == SoundState.Playing)
      {
        this._instance.Volume = 0.0f;
        this._instance.Stop();
      }
      this._pooled = false;
      SFX.UnpoolSound(this);
    }

    public virtual void Unpooled()
    {
      if (this.State != SoundState.Stopped)
      {
        this._instance.Volume = 0.0f;
        this._instance.Stop();
      }
      this._pooled = false;
    }

    public virtual void Pause()
    {
      if (this._killed)
        return;
      this._instance.Volume = 0.0f;
      this._instance.Pause();
      this._pooled = false;
      SFX.UnpoolSound(this);
    }

    public Sound(string sound, float vol, float pitch, float pan, bool looped)
    {
      this._name = sound;
      this._volume = vol;
      this._instance = SFX.GetInstance(sound, this._volume * SFX.volume, pitch, pan, looped);
    }

    public Sound()
    {
    }
  }
}
