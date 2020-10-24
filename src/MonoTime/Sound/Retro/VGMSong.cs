﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.VGMSong
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace DuckGame
{
  public class VGMSong
  {
    private const uint FCC_VGM = 544040790;
    private DynamicSoundEffectInstance _instance;
    private byte[] _buffer;
    private int[] _intBuffer;
    private YM2612 _chip = new YM2612();
    private SN76489 _psg = new SN76489();
    private bool _iSaidStop;
    private float _volume = 1f;
    private bool _looped = true;
    private float _playbackSpeed = 1f;
    private uint _VGMDataLen;
    private VGM_HEADER _VGMHead;
    private BinaryReader _vgmReader;
    private byte[] _DACData;
    private byte[] _VGMData;
    private int _DACOffset;
    private int _VGMDataOffset;
    private byte _lastCommand;
    private int _wait;
    private float _waitInc;

    public SoundState state => !this._iSaidStop ? this._instance.State : SoundState.Stopped;

    public float volume
    {
      get => this._volume;
      set
      {
        this._volume = MathHelper.Clamp(value, 0.0f, 1f);
        if (this._instance == null || this._instance.State != SoundState.Playing)
          return;
        this._instance.Volume = this._volume;
      }
    }

    public bool looped
    {
      get => this._looped;
      set => this._looped = value;
    }

    public bool gameFroze { get; set; }

    public float playbackSpeed
    {
      get => this._playbackSpeed;
      set => this._playbackSpeed = value;
    }

    public VGMSong(string file)
    {
      this._instance = new DynamicSoundEffectInstance(44100, AudioChannels.Stereo);
      this._buffer = new byte[this._instance.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(150.0))];
      this._intBuffer = new int[this._buffer.Length / 2];
      this._instance.BufferNeeded += new EventHandler<EventArgs>(this.StreamVGM);
      this.OpenVGMFile(file);
      this._chip.Initialize((double) (int) this._VGMHead.lngHzYM2612, 44100);
      this._psg.Initialize((double) this._VGMHead.lngHzPSG);
    }

    public void Terminate() => this._instance.Dispose();

    public void Play()
    {
      this._instance.Stop();
      this._instance.Play();
      this._instance.Volume = this._volume;
      this._iSaidStop = false;
    }

    public void Pause()
    {
      this._instance.Pause();
      this._iSaidStop = true;
    }

    public void Resume()
    {
      this._instance.Resume();
      this._instance.Volume = this._volume;
      this._iSaidStop = false;
    }

    public void Stop()
    {
      this._instance.Stop();
      this._instance.Volume = 0.0f;
      this._iSaidStop = true;
      this._vgmReader.BaseStream.Seek(0L, SeekOrigin.Begin);
    }

    private static VGM_HEADER ReadVGMHeader(BinaryReader hFile)
    {
      VGM_HEADER vgmHeader = new VGM_HEADER();
      foreach (FieldInfo field in typeof (VGM_HEADER).GetFields())
      {
        if (field.FieldType == typeof (uint))
        {
          uint num = hFile.ReadUInt32();
          field.SetValue((object) vgmHeader, (object) num);
        }
        else if (field.FieldType == typeof (ushort))
        {
          ushort num = hFile.ReadUInt16();
          field.SetValue((object) vgmHeader, (object) num);
        }
        else if (field.FieldType == typeof (char))
        {
          char ch = hFile.ReadChar();
          field.SetValue((object) vgmHeader, (object) ch);
        }
        else if (field.FieldType == typeof (byte))
        {
          byte num = hFile.ReadByte();
          field.SetValue((object) vgmHeader, (object) num);
        }
      }
      if (vgmHeader.lngVersion < 257U)
        vgmHeader.lngRate = 0U;
      if (vgmHeader.lngVersion < 272U)
      {
        vgmHeader.shtPSG_Feedback = (ushort) 0;
        vgmHeader.bytPSG_SRWidth = (byte) 0;
        vgmHeader.lngHzYM2612 = vgmHeader.lngHzYM2413;
        vgmHeader.lngHzYM2151 = vgmHeader.lngHzYM2413;
      }
      if (vgmHeader.lngHzPSG != 0U)
      {
        if (vgmHeader.shtPSG_Feedback == (ushort) 0)
          vgmHeader.shtPSG_Feedback = (ushort) 9;
        if (vgmHeader.bytPSG_SRWidth == (byte) 0)
          vgmHeader.bytPSG_SRWidth = (byte) 16;
      }
      return vgmHeader;
    }

    private bool OpenVGMFile(string fileName)
    {
      bool flag = fileName.Contains(".vgz");
      FileStream fileStream = File.Open(fileName, FileMode.Open);
      uint num1;
      if (flag)
      {
        fileStream.Position = fileStream.Length - 4L;
        byte[] buffer = new byte[4];
        fileStream.Read(buffer, 0, 4);
        num1 = BitConverter.ToUInt32(buffer, 0);
        fileStream.Position = 0L;
        this._vgmReader = new BinaryReader((Stream) new GZipStream((Stream) fileStream, CompressionMode.Decompress));
      }
      else
      {
        num1 = (uint) fileStream.Length;
        this._vgmReader = new BinaryReader((Stream) fileStream);
      }
      if (this._vgmReader.ReadUInt32() != 544040790U)
        return false;
      this._VGMDataLen = num1;
      this._VGMHead = VGMSong.ReadVGMHeader(this._vgmReader);
      if (flag)
      {
        this._vgmReader.Close();
        fileStream = File.Open(fileName, FileMode.Open);
        this._vgmReader = new BinaryReader((Stream) new GZipStream((Stream) fileStream, CompressionMode.Decompress));
      }
      else
        this._vgmReader.BaseStream.Seek(0L, SeekOrigin.Begin);
      int count = (int) this._VGMHead.lngDataOffset;
      switch (count)
      {
        case 0:
        case 12:
          count = 64;
          break;
      }
      this._VGMDataOffset = count;
      this._vgmReader.ReadBytes(count);
      this._VGMData = this._vgmReader.ReadBytes((int) ((long) num1 - (long) count));
      this._vgmReader = new BinaryReader((Stream) new MemoryStream(this._VGMData));
      if ((byte) this._vgmReader.PeekChar() == (byte) 103)
      {
        int num2 = (int) this._vgmReader.ReadByte();
        if ((byte) this._vgmReader.PeekChar() == (byte) 102)
        {
          int num3 = (int) this._vgmReader.ReadByte();
          int num4 = (int) this._vgmReader.ReadByte();
          this._DACData = this._vgmReader.ReadBytes((int) this._vgmReader.ReadUInt32());
        }
      }
      fileStream.Close();
      return true;
    }

    private void StreamVGM(object sender, EventArgs e)
    {
      if (this._iSaidStop)
        return;
      if (this._lastCommand == (byte) 102 && !this._looped)
      {
        this._lastCommand = (byte) 0;
        this._instance.Volume = 0.0f;
        this._iSaidStop = true;
        this.Stop();
      }
      else
      {
        int[] buffer = new int[2];
        int num1 = 0;
        int num2 = this._intBuffer.Length / 2;
        bool flag1 = false;
        while (num1 != num2)
        {
          bool flag2;
          if (this._wait == 0 && !this.gameFroze)
          {
            flag2 = false;
            byte num3 = this._vgmReader.ReadByte();
            this._lastCommand = num3;
            switch (num3)
            {
              case 79:
                int num4 = (int) this._vgmReader.ReadByte();
                break;
              case 80:
                this._psg.Write((int) this._vgmReader.ReadByte());
                break;
              case 82:
                this._chip.WritePort0((int) this._vgmReader.ReadByte(), (int) this._vgmReader.ReadByte());
                break;
              case 83:
                this._chip.WritePort1((int) this._vgmReader.ReadByte(), (int) this._vgmReader.ReadByte());
                break;
              case 97:
                this._wait = (int) this._vgmReader.ReadUInt16();
                if (this._wait != 0)
                {
                  flag2 = true;
                  break;
                }
                break;
              case 98:
                this._wait = 735;
                flag2 = true;
                break;
              case 99:
                this._wait = 882;
                flag2 = true;
                break;
              case 102:
                if (!this._looped)
                {
                  this._vgmReader.BaseStream.Seek(0L, SeekOrigin.Begin);
                  flag1 = true;
                  break;
                }
                if (this._VGMHead.lngLoopOffset != 0U)
                {
                  this._vgmReader.BaseStream.Seek((long) this._VGMHead.lngLoopOffset - (long) this._VGMDataOffset, SeekOrigin.Begin);
                  break;
                }
                this._vgmReader.BaseStream.Seek(0L, SeekOrigin.Begin);
                break;
              case 103:
                int num5 = (int) this._vgmReader.ReadByte();
                int num6 = (int) this._vgmReader.ReadByte();
                this._vgmReader.BaseStream.Position += (long) this._vgmReader.ReadUInt32();
                break;
              case 224:
                this._DACOffset = (int) this._vgmReader.ReadUInt32();
                break;
            }
            if (num3 >= (byte) 112 && num3 <= (byte) 127)
            {
              this._wait = ((int) num3 & 15) + 1;
              if (this._wait != 0)
                flag2 = true;
            }
            else if (num3 >= (byte) 128 && num3 <= (byte) 143)
            {
              this._wait = (int) num3 & 15;
              this._chip.WritePort0(42, (int) this._DACData[this._DACOffset]);
              ++this._DACOffset;
              if (this._wait != 0)
                flag2 = true;
            }
            if (this._wait != 0)
              --this._wait;
          }
          else
          {
            flag2 = true;
            if (this._wait > 0)
            {
              for (this._waitInc += this._playbackSpeed; this._wait > 0 && (double) this._waitInc >= 1.0; --this._wait)
                --this._waitInc;
            }
          }
          if (!flag1)
          {
            if (flag2)
            {
              this._chip.Update(buffer, 1);
              short num3 = (short) buffer[0];
              short num4 = (short) buffer[1];
              this._psg.Update(buffer, 1);
              short num5 = (short) buffer[0];
              short num6 = (short) buffer[1];
              this._intBuffer[num1 * 2] = Maths.Clamp(((int) num3 + (int) num5) * 2, (int) short.MinValue, (int) short.MaxValue);
              this._intBuffer[num1 * 2 + 1] = Maths.Clamp(((int) num4 + (int) num6) * 2, (int) short.MinValue, (int) short.MaxValue);
              ++num1;
              if (num1 == num2)
                break;
            }
          }
          else
            break;
        }
        for (int index = 0; index < this._intBuffer.Length; ++index)
        {
          short num3 = (short) this._intBuffer[index];
          this._buffer[index * 2] = (byte) ((uint) num3 & (uint) byte.MaxValue);
          this._buffer[index * 2 + 1] = (byte) ((int) num3 >> 8 & (int) byte.MaxValue);
        }
        int num7 = num1 * 2;
        if ((double) num7 / 4.0 - (double) (int) ((double) num7 / 4.0) > 0.0)
          num7 -= 2;
        this._instance.SubmitBuffer(this._buffer, 0, num7);
        this._instance.SubmitBuffer(this._buffer, num7, num7);
      }
    }
  }
}
