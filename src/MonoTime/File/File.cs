﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.BinaryFile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.IO;

namespace DuckGame
{
  public class BinaryFile
  {
    private Stream _stream;
    private byte[] _readShort = new byte[2];
    private byte[] _readInt = new byte[4];

    public BinaryFile(string name, BinaryFileMode m) => this._stream = (Stream) new FileStream(name, (FileMode) m);

    public BinaryFile(byte[] data) => this._stream = (Stream) new MemoryStream(data);

    public void Close() => this._stream.Close();

    public void SkipBytes(int bytes) => this._stream.Seek((long) bytes, SeekOrigin.Current);

    public void ResetPosition() => this._stream.Seek(0L, SeekOrigin.Begin);

    public byte ReadByte() => (byte) this._stream.ReadByte();

    public byte[] ReadBytes(int num)
    {
      byte[] buffer = new byte[num];
      this._stream.Read(buffer, 0, num);
      return buffer;
    }

    public short ReadShort()
    {
      this._stream.Read(this._readShort, 0, 2);
      return BitConverter.ToInt16(this._readShort, 0);
    }

    public int ReadInt()
    {
      this._stream.Read(this._readInt, 0, 4);
      return BitConverter.ToInt32(this._readInt, 0);
    }

    public void WriteByte(byte b) => this._stream.WriteByte(b);

    public void WriteBytes(byte[] bytes, int length) => this._stream.Write(bytes, 0, length);

    public void WriteUShort(ushort b)
    {
      this._readShort = BitConverter.GetBytes(b);
      foreach (byte num in this._readShort)
        this._stream.WriteByte(num);
    }

    public void WriteInt(int b)
    {
      this._readInt = BitConverter.GetBytes(b);
      foreach (byte num in this._readInt)
        this._stream.WriteByte(num);
    }
  }
}
