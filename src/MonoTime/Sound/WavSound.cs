// Decompiled with JetBrains decompiler
// Type: DuckGame.WavSound
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WavSound : IAutoUpdate
  {
    private WavFile _file;
    private int _position;
    private float _add;
    private int _addsPerSample = 1;
    private bool _loop;
    private WavPlayState _state;
    private bool _inManager;
    private float _pitch;
    private float _desiredPitch;
    private float _sampleAdd = 1f;
    private float _panning;
    private float _leftVol = 1f;
    private float _rightVol = 1f;
    private float _leftVolDesired = 1f;
    private float _rightVolDesired = 1f;
    private float _volume;
    private float _volumeMultiplier = 1f;
    private bool _coasting;

    public bool IsLooped
    {
      get => this._loop;
      set => this._loop = value;
    }

    public WavPlayState state => this._state;

    public bool mono => this._file.stereoData[1] == null;

    public float Pitch
    {
      get => this._pitch;
      set => this._desiredPitch = value;
    }

    public float Pan
    {
      get => this._panning;
      set
      {
        this._panning = value;
        this.CalculateVolumes();
      }
    }

    public float Volume
    {
      get => this._volume;
      set
      {
        this._volume = value;
        this.CalculateVolumes();
      }
    }

    private void CalculateVolumes()
    {
      this._volume = Maths.Clamp(this._volume, 0.0f, 1f);
      if ((double) this._panning <= 0.0)
      {
        this._leftVolDesired = this._volume;
        this._rightVolDesired = (this._panning + 1f) * this._volume;
      }
      else if ((double) this._panning > 0.0)
      {
        this._rightVolDesired = this._volume;
        this._leftVolDesired = (1f - this._panning) * this._volume;
      }
      if (this.state == WavPlayState.Playing)
        return;
      this._leftVol = this._leftVolDesired;
      this._rightVol = this._rightVolDesired;
    }

    public WavSound(string file)
    {
      this._file = new WavFile(file);
      this._addsPerSample = 44100 / this._file.sampleRate;
      AutoUpdatables.Add((IAutoUpdate) this);
    }

    public WavSound(WavFile file)
    {
      this._file = file;
      this._addsPerSample = 44100 / this._file.sampleRate;
      AutoUpdatables.Add((IAutoUpdate) this);
    }

    public void Play()
    {
      if (this._state == WavPlayState.Playing)
        return;
      this._state = WavPlayState.Playing;
      AudioManager.AddSound(this);
      this._inManager = true;
      this._position = 0;
      this._add = 0.0f;
    }

    public void Stop()
    {
      this._state = WavPlayState.Stopped;
      this._coasting = false;
    }

    public void Pause()
    {
      this._state = WavPlayState.Paused;
      this._coasting = false;
    }

    public void Update()
    {
      this._volumeMultiplier = 1f;
      if ((double) this._leftVol + (double) this._rightVol < 9.99999974737875E-05)
      {
        if (this._inManager)
        {
          AudioManager.RemoveSound(this);
          this._inManager = false;
          if (this.state == WavPlayState.Stopped)
          {
            this._position = 0;
            this._add = 0.0f;
          }
        }
        if (this._state == WavPlayState.Playing)
          this._coasting = true;
      }
      if ((double) this._leftVolDesired + (double) this._rightVolDesired <= 9.99999974737875E-05 || !this._coasting || this._state != WavPlayState.Playing)
        return;
      if (!this._inManager)
      {
        AudioManager.AddSound(this);
        this._inManager = true;
      }
      this._coasting = false;
    }

    public short GetLeftSample() => (short) ((double) this._file.stereoData[0][this._position] * (double) this._leftVol);

    public short GetLeftSampleRightVolume() => (short) ((double) this._file.stereoData[0][this._position] * (double) this._rightVol);

    public short GetRightSample() => (short) ((double) this._file.stereoData[1][this._position] * (double) this._rightVol);

    public void IncrementSamplePosition()
    {
      if (this._state != WavPlayState.Playing)
        return;
      this._leftVol = Maths.LerpTowards(this._leftVol, this._leftVolDesired * this._volumeMultiplier, 0.01f);
      this._rightVol = Maths.LerpTowards(this._rightVol, this._rightVolDesired * this._volumeMultiplier, 0.01f);
      this._pitch = Maths.LerpTowards(this._pitch, this._desiredPitch, 0.01f);
      this._sampleAdd = (float) (1.0 + (double) this._pitch * 0.5);
      this._add += this._sampleAdd;
      while ((int) this._add >= this._addsPerSample)
      {
        this._add -= (float) this._addsPerSample;
        this._position = (this._position + 1) % this._file.size;
        if (!this._loop && this._position == 0)
        {
          this.Stop();
          if (this._inManager)
          {
            AudioManager.RemoveSound(this);
            this._inManager = false;
            this._position = 0;
            this._add = 0.0f;
          }
        }
      }
    }
  }
}
