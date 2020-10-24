// Decompiled with JetBrains decompiler
// Type: DuckGame.MultiSoundUpdater
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace DuckGame
{
  public class MultiSoundUpdater : Sound
  {
    private List<MultiSound> _instances = new List<MultiSound>();
    private SoundEffectInstance _single;
    private SoundEffectInstance _multi;
    private SoundState _state = SoundState.Stopped;
    private new string _name = "";
    private int _playCount;

    public override void Kill()
    {
      for (int index = 0; index < this._playCount; ++index)
        this.Stop();
      this._killed = true;
    }

    public MultiSound GetInstance() => new MultiSound(this);

    public override bool IsDisposed => this._single.IsDisposed || this._multi.IsDisposed;

    public override float Pitch
    {
      get => this._single.Pitch;
      set
      {
        this._single.Pitch = value;
        this._multi.Pitch = value;
      }
    }

    public override float Pan
    {
      get => this._single.Pan;
      set
      {
        this._single.Pan = value;
        this._single.Pan = value;
      }
    }

    public override bool IsLooped
    {
      get => this._single.IsLooped;
      set
      {
        this._single.IsLooped = value;
        this._multi.IsLooped = value;
      }
    }

    public override SoundState State => this._state;

    public new string name => this._name;

    public void Play(MultiSound who)
    {
      if (this._killed || this._instances.Contains(who))
        return;
      if (this._playCount == 0 && SFX.PoolSound((Sound) this))
      {
        this._single.Volume = this._volume * SFX.volume;
        this._single.Play();
      }
      ++this._playCount;
      this._state = SoundState.Playing;
      this._pooled = true;
      this._instances.Add(who);
    }

    public override void Stop()
    {
      while (this._instances.Count > 0)
        this._instances[0].Stop();
      this._volume = 0.0f;
    }

    public void Stop(MultiSound who)
    {
      if (this._killed || !this._instances.Contains(who))
        return;
      if (this._state == SoundState.Playing)
        --this._playCount;
      if (this._playCount == 0)
      {
        this._single.Volume = 0.0f;
        this._single.Stop();
        this._multi.Volume = 0.0f;
        this._multi.Stop();
        this._pooled = false;
        SFX.UnpoolSound((Sound) this);
        this._state = SoundState.Stopped;
      }
      this._instances.Remove(who);
    }

    public override void Unpooled()
    {
      if (this._state != SoundState.Stopped)
      {
        this._single.Volume = 0.0f;
        this._single.Stop();
        this._multi.Volume = 0.0f;
        this._multi.Stop();
      }
      this._pooled = false;
    }

    public void Update()
    {
      float num1 = 0.0f;
      foreach (MultiSound instance in this._instances)
        num1 += instance.Volume;
      float num2 = num1 / (float) this._instances.Count;
      this._volume = Lerp.Float(this._volume, (float) ((double) num2 * 0.699999988079071 + (double) Maths.Clamp(this._instances.Count, 0, 4) / 4.0 * (double) num2 * 0.300000011920929), 0.05f);
      if (this._state != SoundState.Playing)
        return;
      if (this._playCount > 1)
      {
        if (this._multi.State == SoundState.Stopped)
          this._multi.Play();
        if (this._single.State != SoundState.Stopped)
        {
          this._single.Volume = Lerp.Float(this._single.Volume, 0.0f, 0.05f);
          if ((double) this._single.Volume < 0.0199999995529652)
          {
            this._single.Volume = 0.0f;
            this._single.Stop();
          }
        }
        this._multi.Volume = Lerp.Float(this._multi.Volume, this._volume, 0.05f);
      }
      else
      {
        if (this._playCount != 1)
          return;
        if (this._single.State == SoundState.Stopped)
          this._single.Play();
        if (this._multi.State != SoundState.Stopped)
        {
          this._multi.Volume = Lerp.Float(this._multi.Volume, 0.0f, 0.05f);
          if ((double) this._multi.Volume < 0.0199999995529652)
          {
            this._multi.Volume = 0.0f;
            this._multi.Stop();
          }
        }
        this._single.Volume = Lerp.Float(this._single.Volume, this._volume, 0.05f);
      }
    }

    public MultiSoundUpdater(string id, string single, string multi)
    {
      this._name = id;
      this._single = SFX.GetInstance(single, 0.0f, looped: true);
      this._multi = SFX.GetInstance(multi, 0.0f, looped: true);
      this._cannotBeCancelled = true;
      this._volume = 1f;
    }
  }
}
