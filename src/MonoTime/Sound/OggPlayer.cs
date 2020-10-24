// Decompiled with JetBrains decompiler
// Type: DuckGame.OggPlayer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class OggPlayer
  {
    private DynamicSoundEffectInstance _instance;
    private byte[] _buffer;
    private float[] _floatBuffer;
    private bool _didLooped;
    private bool _iSaidStop;
    private float _volume = 1f;
    private bool _looped;
    private VorbisReader _activeSong;
    private TimeSpan _prevTime;
    private Timer _tickTimer = new Timer();
    private bool _valid = true;
    private volatile bool gettingBuffer;

    public SoundState state => !this._valid || this._iSaidStop ? SoundState.Stopped : this._instance.State;

    public float volume
    {
      get => this._volume;
      set
      {
        this._volume = MathHelper.Clamp(value, 0.0f, 1f);
        if (!this._valid || this._instance == null || this._instance.State != SoundState.Playing)
          return;
        this._instance.Volume = this._volume;
      }
    }

    public bool looped
    {
      get => this._looped;
      set => this._looped = value;
    }

    public TimeSpan position => this._activeSong != null && this._valid ? this._prevTime + this._tickTimer.elapsed : new TimeSpan();

    public OggPlayer()
    {
      try
      {
        this._instance = new DynamicSoundEffectInstance(44100, AudioChannels.Stereo);
        this._buffer = new byte[this._instance.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(100.0))];
        this._floatBuffer = new float[this._buffer.Length / 2];
        this._instance.BufferNeeded += new EventHandler<EventArgs>(this.StreamOgg);
        this._tickTimer.Start();
      }
      catch
      {
        DevConsole.Log(DCSection.General, "Music player failed to initialize.");
        this._valid = false;
      }
    }

    public void Terminate()
    {
      if (!this._valid)
        return;
      this._instance.Dispose();
    }

    public void SetOgg(MemoryStream ogg)
    {
      if (!this._valid)
        return;
      try
      {
        this.Stop();
        this._didLooped = false;
        int num = 0;
        while (this.gettingBuffer)
        {
          ++num;
          if (num > 100)
            break;
        }
        this.gettingBuffer = true;
        if (this._activeSong != null)
          this._activeSong.Dispose();
        this._activeSong = new VorbisReader((Stream) ogg, false);
        this.gettingBuffer = false;
      }
      catch
      {
        this._activeSong = (VorbisReader) null;
        this.gettingBuffer = false;
      }
    }

    public void Play()
    {
      if (!this._valid)
        return;
      this._instance.Play();
      this._instance.Volume = this._volume;
      this._iSaidStop = false;
    }

    public void Pause()
    {
      if (!this._valid)
        return;
      this._instance.Pause();
    }

    public void Resume()
    {
      if (!this._valid)
        return;
      this._instance.Resume();
      this._instance.Volume = this._volume;
      this._iSaidStop = false;
    }

    public void Stop()
    {
      if (!this._valid)
        return;
      this._instance.Stop();
      if (this._activeSong != null)
      {
        do
          ;
        while (this.gettingBuffer);
        this.gettingBuffer = true;
        this._activeSong.DecodedTime = TimeSpan.Zero;
        this.gettingBuffer = false;
      }
      this._iSaidStop = true;
    }

    public void Update()
    {
    }

    private void StreamOgg(object sender, EventArgs e)
    {
      if ((double) this.volume == 0.0 || !this._valid)
      {
        for (int index = 0; index < ((IEnumerable<byte>) this._buffer).Count<byte>(); ++index)
          this._buffer[index] = (byte) 0;
        this._instance.SubmitBuffer(this._buffer, 0, ((IEnumerable<byte>) this._buffer).Count<byte>());
      }
      else
      {
        do
          ;
        while (this.gettingBuffer);
        this.gettingBuffer = true;
        if (this._activeSong == null)
        {
          this.gettingBuffer = false;
        }
        else
        {
          this._prevTime = this._activeSong.DecodedTime;
          this._tickTimer.Restart();
          int num1 = this._activeSong.ReadSamples(this._floatBuffer, 0, this._floatBuffer.Length);
          if (!this._looped && this._didLooped)
          {
            this._didLooped = false;
            this.gettingBuffer = false;
            this.Stop();
          }
          else
          {
            this._didLooped = false;
            if (num1 == 0)
            {
              if (this._looped)
              {
                this._activeSong.DecodedTime = TimeSpan.Zero;
                num1 = this._activeSong.ReadSamples(this._floatBuffer, 0, this._floatBuffer.Length);
              }
              else
              {
                for (int index = 0; index < this._floatBuffer.Length / 2; ++index)
                {
                  this._floatBuffer[index * 2] = 0.0f;
                  this._floatBuffer[index * 2 + 1] = 0.0f;
                }
                num1 = this._floatBuffer.Length;
              }
              this._didLooped = true;
            }
            if (num1 > 0)
            {
              if ((double) num1 / 4.0 - (double) (int) ((double) num1 / 4.0) > 0.0)
                num1 -= 2;
              for (int index = 0; index < num1; ++index)
              {
                short num2 = (short) Math.Max(Math.Min((float) short.MaxValue * this._floatBuffer[index], (float) short.MaxValue), (float) short.MinValue);
                this._buffer[index * 2] = (byte) ((uint) num2 & (uint) byte.MaxValue);
                this._buffer[index * 2 + 1] = (byte) ((int) num2 >> 8 & (int) byte.MaxValue);
              }
              this._instance.SubmitBuffer(this._buffer, 0, num1);
              this._instance.SubmitBuffer(this._buffer, num1, num1);
            }
            this.gettingBuffer = false;
          }
        }
      }
    }
  }
}
