// Decompiled with JetBrains decompiler
// Type: DuckGame.WavFile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.IO;

namespace DuckGame
{
  public class WavFile
  {
    private WavHeader _header;
    private short[][] _stereoData = new short[2][];
    private string _fileName = "";

    public short[][] stereoData => this._stereoData;

    public int size => (int) (this._header.dataSize / (uint) this._header.blockSize);

    public int sampleRate => (int) this._header.sampleRate;

    public WavFile(string file)
    {
      this._fileName = file;
      this._header = new WavHeader();
      using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          try
          {
            this._header.riffID = binaryReader.ReadBytes(4);
            this._header.size = binaryReader.ReadUInt32();
            this._header.wavID = binaryReader.ReadBytes(4);
            this._header.fmtID = binaryReader.ReadBytes(4);
            this._header.fmtSize = binaryReader.ReadUInt32();
            this._header.format = binaryReader.ReadUInt16();
            this._header.channels = binaryReader.ReadUInt16();
            this._header.sampleRate = binaryReader.ReadUInt32();
            this._header.bytePerSec = binaryReader.ReadUInt32();
            this._header.blockSize = binaryReader.ReadUInt16();
            this._header.bit = binaryReader.ReadUInt16();
            while (true)
            {
              this._header.dataID = binaryReader.ReadBytes(4);
              this._header.dataSize = binaryReader.ReadUInt32();
              if (this._header.dataID[0] != (byte) 100)
                binaryReader.ReadBytes((int) this._header.dataSize);
              else
                break;
            }
            if (this._header.channels == (ushort) 1)
            {
              uint num = this._header.dataSize / (uint) this._header.blockSize;
              this._stereoData[0] = new short[(IntPtr) num];
              for (int index = 0; (long) index < (long) num; ++index)
                this._stereoData[0][index] = (short) binaryReader.ReadUInt16();
            }
            else
            {
              if (this._header.channels != (ushort) 2)
                return;
              uint num = this._header.dataSize / (uint) this._header.blockSize;
              this._stereoData[0] = new short[(IntPtr) num];
              this._stereoData[1] = new short[(IntPtr) num];
              for (int index = 0; (long) index < (long) num; ++index)
              {
                this._stereoData[0][index] = (short) binaryReader.ReadUInt16();
                this._stereoData[1][index] = (short) binaryReader.ReadUInt16();
              }
            }
          }
          finally
          {
            binaryReader?.Close();
            fileStream?.Close();
          }
        }
      }
    }
  }
}
