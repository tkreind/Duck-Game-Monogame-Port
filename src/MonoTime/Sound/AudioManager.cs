// Decompiled with JetBrains decompiler
// Type: DuckGame.AudioManager
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class AudioManager
  {
    private static List<WavSound> _stereoSounds = new List<WavSound>();
    private static List<WavSound> _monoSounds = new List<WavSound>();
    private DynamicSoundEffectInstance _effect;
    private byte[] _buffer;
    private int _bufferSize;

    public AudioManager()
    {
      this._effect = new DynamicSoundEffectInstance(44100, AudioChannels.Stereo);
      this._bufferSize = this._effect.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(28.0));
      this._buffer = new byte[this._bufferSize];
      this._effect.BufferNeeded += new EventHandler<EventArgs>(this.StreamThread);
      this._effect.Play();
    }

    public static void AddSound(WavSound sound)
    {
      if (sound.mono)
      {
        if (AudioManager._monoSounds.Contains(sound))
          return;
        AudioManager._monoSounds.Add(sound);
      }
      else
      {
        if (AudioManager._stereoSounds.Contains(sound))
          return;
        AudioManager._stereoSounds.Add(sound);
      }
    }

    public static void RemoveSound(WavSound sound)
    {
      if (sound.mono)
        AudioManager._monoSounds.Remove(sound);
      else
        AudioManager._stereoSounds.Remove(sound);
    }

    private short MixSamples(short a, short b) => a >= (short) 0 || b >= (short) 0 ? (a <= (short) 0 || b <= (short) 0 ? (short) ((int) a + (int) b) : (short) ((int) a + (int) b - (int) a * (int) b / (int) short.MaxValue)) : (short) ((int) a + (int) b - (int) a * (int) b / (int) short.MinValue);

    private void StreamThread(object sender, EventArgs e)
    {
      List<WavSound> wavSoundList1 = new List<WavSound>();
      List<WavSound> wavSoundList2 = new List<WavSound>();
      wavSoundList1.AddRange((IEnumerable<WavSound>) AudioManager._monoSounds);
      wavSoundList2.AddRange((IEnumerable<WavSound>) AudioManager._stereoSounds);
      for (int index = 0; index < this._bufferSize / 2; index += 2)
      {
        short a1 = 0;
        short a2 = 0;
        foreach (WavSound wavSound in wavSoundList2)
        {
          a1 = this.MixSamples(a1, wavSound.GetLeftSample());
          a2 = this.MixSamples(a2, wavSound.GetRightSample());
          wavSound.IncrementSamplePosition();
        }
        foreach (WavSound wavSound in wavSoundList1)
        {
          a1 = this.MixSamples(a1, wavSound.GetLeftSample());
          a2 = this.MixSamples(a2, wavSound.GetLeftSampleRightVolume());
          wavSound.IncrementSamplePosition();
        }
        this._buffer[index * 2] = (byte) ((uint) a1 & (uint) byte.MaxValue);
        this._buffer[index * 2 + 1] = (byte) ((int) a1 >> 8 & (int) byte.MaxValue);
        this._buffer[(index + 1) * 2] = (byte) ((uint) a2 & (uint) byte.MaxValue);
        this._buffer[(index + 1) * 2 + 1] = (byte) ((int) a2 >> 8 & (int) byte.MaxValue);
      }
      this._effect.SubmitBuffer(this._buffer, 0, this._bufferSize / 2);
      this._effect.SubmitBuffer(this._buffer, this._bufferSize / 2, this._bufferSize / 2);
    }
  }
}
