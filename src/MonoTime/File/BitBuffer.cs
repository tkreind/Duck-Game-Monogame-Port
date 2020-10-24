// Decompiled with JetBrains decompiler
// Type: DuckGame.BitBuffer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame
{
  public class BitBuffer
  {
    private static int[] _maxMasks;
    private byte[] _buffer = new byte[64];
    private bool _dirty;
    private int _offsetPosition;
    private int _endPosition;
    private int _bitEndOffset;
    private int _bitOffsetPosition;
    private byte[] _trimmedBuffer;
    private static int[] _readMasks;
    private bool _allowPacking = true;
    private int offset;
    private int currentBit;

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.lengthInBytes; ++index)
        str = str + this._buffer[index].ToString() + "|";
      return str;
    }

    public static BitBuffer FromString(string s)
    {
      BitBuffer bitBuffer = new BitBuffer();
      try
      {
        string str1 = s;
        char[] chArray = new char[1]{ '|' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (!(str2 == ""))
            bitBuffer.Write(Convert.ToByte(str2));
        }
        bitBuffer.position = 0;
      }
      catch (Exception ex)
      {
        DevConsole.Log(DCSection.General, "BitBuffer conversion from string failed.");
        return new BitBuffer();
      }
      return bitBuffer;
    }

    public static long GetMaxValue(int bits)
    {
      if (BitBuffer._maxMasks == null)
      {
        BitBuffer._maxMasks = new int[64];
        int num1 = 0;
        for (int index = 0; index < 64; ++index)
        {
          int num2 = num1 | 1;
          BitBuffer._maxMasks[index] = num2;
          num1 = num2 << 1;
        }
      }
      return (long) BitBuffer._maxMasks[bits];
    }

    public byte[] buffer => this._buffer;

    public int position
    {
      get => this._offsetPosition;
      set
      {
        if (this._offsetPosition != value)
          this._dirty = true;
        this._offsetPosition = value;
        if (this._offsetPosition <= this._endPosition)
          return;
        this._endPosition = this._offsetPosition;
        this._bitEndOffset = 0;
      }
    }

    public int bitOffset
    {
      get => this._bitOffsetPosition;
      set
      {
        if (this._bitOffsetPosition != value)
          this._dirty = true;
        this._bitOffsetPosition = value;
        if (this._endPosition != this._offsetPosition || this._bitOffsetPosition <= this._bitEndOffset)
          return;
        this._bitEndOffset = value;
      }
    }

    public bool isPacked => this._bitEndOffset != 0;

    public int lengthInBits => this._endPosition * 8 + this._bitEndOffset;

    public int lengthInBytes => this._endPosition + (this._bitEndOffset > 0 ? 1 : 0);

    public byte[] GetBytes()
    {
      if (this._trimmedBuffer != null && !this._dirty)
        return this._trimmedBuffer;
      this._dirty = false;
      this._trimmedBuffer = new byte[this.lengthInBytes];
      for (int index = 0; index < this.lengthInBytes; ++index)
        this._trimmedBuffer[index] = this._buffer[index];
      return this._trimmedBuffer;
    }

    private void calculateReadMasks()
    {
      if (BitBuffer._readMasks != null)
        return;
      BitBuffer._readMasks = new int[64];
      int num1 = 0;
      for (int index = 0; index < 64; ++index)
      {
        int num2 = num1 | 1;
        BitBuffer._readMasks[index] = num2;
        num1 = num2 << 1;
      }
    }

    public bool allowPacking => this._allowPacking;

    public BitBuffer(bool allowPacking = true)
    {
      this.calculateReadMasks();
      this._allowPacking = allowPacking;
    }

    public BitBuffer(byte[] data, int bits = 0, bool allowPacking = true)
    {
      this._allowPacking = allowPacking;
      this.calculateReadMasks();
      this.Write(data, 0, -1);
      this.SeekToStart();
      if (bits <= 0 || this._endPosition * 8 <= bits)
        return;
      --this._endPosition;
      this._bitEndOffset = bits - this._endPosition * 8;
    }

    public BitBuffer(byte[] data, bool copyData)
    {
      this._allowPacking = false;
      this.calculateReadMasks();
      if (copyData)
      {
        this.Write(data, 0, -1);
        this.SeekToStart();
      }
      else
        this._buffer = data;
    }

    public void SeekToStart()
    {
      this.position = 0;
      this._bitOffsetPosition = 0;
    }

    public void Fill(byte[] bytes, int offset = 0, int vbitOffset = 0)
    {
      this._buffer = bytes;
      this.position = offset;
      this._bitOffsetPosition = vbitOffset;
    }

    public int ReadPackedBits(int bits)
    {
      if (bits == 0)
        return 0;
      int num1 = 0;
      if (bits <= 8 - this.bitOffset)
      {
        num1 = (int) this._buffer[this.position] >> this.bitOffset & BitBuffer._readMasks[bits - 1];
        this.bitOffset += bits;
      }
      else
      {
        int num2 = 0;
        while (true)
        {
          if (this.bitOffset > 7)
          {
            this.bitOffset = 0;
            ++this.position;
          }
          if (bits > 0)
          {
            int num3 = 8 - this.bitOffset;
            if (num3 > bits)
              num3 = bits;
            int num4 = (int) this._buffer[this.position] >> this.bitOffset & BitBuffer._readMasks[num3 - 1];
            bits -= num3;
            int num5 = num4 << num2;
            num1 |= num5;
            this.bitOffset += num3;
            num2 += num3;
          }
          else
            break;
        }
      }
      if (this.bitOffset > 7)
      {
        this.bitOffset = 0;
        ++this.position;
      }
      return num1;
    }

    public byte[] ReadPacked(int bytes)
    {
      byte[] numArray = new byte[bytes];
      for (int index = 0; index < bytes; ++index)
        numArray[index] = (byte) this.ReadPackedBits(8);
      return numArray;
    }

    public void WritePacked(int number, int bits)
    {
      if (this.lengthInBits + bits > this._buffer.Length * 8)
        this.resize(this._buffer.Length * 2);
      if (bits <= 64)
        number &= BitBuffer._readMasks[bits - 1];
      this.currentBit = 0;
      while (this.currentBit < bits)
      {
        this._buffer[this.position] |= (byte) ((uint) number >> this.currentBit << this.bitOffset);
        this.offset = Math.Min(bits - this.currentBit, 8 - this.bitOffset);
        this.bitOffset = (this.bitOffset + this.offset) % 8;
        this.currentBit += this.offset;
        if (this.bitOffset == 0)
          ++this.position;
      }
    }

    public void WritePacked(byte[] data)
    {
      foreach (int number in data)
        this.WritePacked(number, 8);
    }

    public void WritePacked(byte[] data, int bits)
    {
      if (this.position + (int) Math.Ceiling((double) bits / 8.0) > this._buffer.Length)
        this.resize((this.position + (int) Math.Ceiling((double) bits / 8.0)) * 2);
      int index = 0;
      if (!this.isPacked)
      {
        for (; bits >= 8; bits -= 8)
        {
          this._buffer[this.position] = data[index];
          ++this.position;
          ++index;
        }
      }
      else
      {
        for (; bits >= 8; bits -= 8)
        {
          this.WritePacked((int) data[index], 8);
          ++index;
        }
      }
      if (bits <= 0)
        return;
      this.WritePacked((int) data[index], bits);
    }

    public BitBuffer ReadBitBuffer(bool allowPacking = true)
    {
      ushort num1 = this.ReadUShort();
      byte[] data;
      if (allowPacking)
      {
        int length = (int) Math.Ceiling((double) num1 / 8.0);
        data = new byte[length];
        ushort num2 = num1;
        for (int index = 0; index < length; ++index)
        {
          data[index] = (byte) this.ReadPackedBits(num2 >= (ushort) 8 ? 8 : (int) num2);
          if (num2 >= (ushort) 8)
            num2 -= (ushort) 8;
        }
      }
      else
      {
        data = new byte[(int) num1];
        for (int index = 0; index < (int) num1; ++index)
          data[index] = this.ReadByte();
        num1 = (ushort) 0;
      }
      return new BitBuffer(data, (int) num1, allowPacking);
    }

    public string ReadString()
    {
      int num = (int) this.ReadUShort();
      if (num == (int) ushort.MaxValue)
      {
        int bitOffset = this.bitOffset;
        int position = this.position;
        if (this.ReadUShort() == (ushort) 42252)
        {
          num = this.ReadInt();
        }
        else
        {
          this.position = position;
          this.bitOffset = bitOffset;
        }
      }
      if (this.bitOffset != 0)
        return Encoding.UTF8.GetString(this.ReadPacked(num));
      string str = Encoding.UTF8.GetString(this._buffer, this.position, num);
      this.position += num;
      return str;
    }

    public long ReadLong()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToInt64(this.ReadPacked(8), 0);
      long int64 = BitConverter.ToInt64(this._buffer, this.position);
      this.position += 8;
      return int64;
    }

    public ulong ReadULong()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToUInt64(this.ReadPacked(8), 0);
      ulong uint64 = BitConverter.ToUInt64(this._buffer, this.position);
      this.position += 8;
      return uint64;
    }

    public int ReadInt()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToInt32(this.ReadPacked(4), 0);
      int int32 = BitConverter.ToInt32(this._buffer, this.position);
      this.position += 4;
      return int32;
    }

    public uint ReadUInt()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToUInt32(this.ReadPacked(4), 0);
      uint uint32 = BitConverter.ToUInt32(this._buffer, this.position);
      this.position += 4;
      return uint32;
    }

    public short ReadShort()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToInt16(this.ReadPacked(2), 0);
      short int16 = BitConverter.ToInt16(this._buffer, this.position);
      this.position += 2;
      return int16;
    }

    public ushort ReadUShort()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToUInt16(this.ReadPacked(2), 0);
      ushort uint16 = BitConverter.ToUInt16(this._buffer, this.position);
      this.position += 2;
      return uint16;
    }

    public float ReadFloat()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToSingle(this.ReadPacked(4), 0);
      float single = BitConverter.ToSingle(this._buffer, this.position);
      this.position += 4;
      return single;
    }

    public Vec2 ReadVec2() => new Vec2()
    {
      x = this.ReadFloat(),
      y = this.ReadFloat()
    };

    public double ReadDouble()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToDouble(this.ReadPacked(8), 0);
      double num = BitConverter.ToDouble(this._buffer, this.position);
      this.position += 8;
      return num;
    }

    public char ReadChar()
    {
      if (this.bitOffset != 0)
        return BitConverter.ToChar(this.ReadPacked(2), 0);
      char ch = BitConverter.ToChar(this._buffer, this.position);
      this.position += 2;
      return ch;
    }

    public byte ReadByte()
    {
      if (this.bitOffset != 0)
        return this.ReadPacked(1)[0];
      byte num = this._buffer[this.position];
      ++this.position;
      return num;
    }

    public sbyte ReadSByte()
    {
      if (this.bitOffset != 0)
        return (sbyte) this.ReadPacked(1)[0];
      sbyte num = (sbyte) this._buffer[this.position];
      ++this.position;
      return num;
    }

    public bool ReadBool()
    {
      if (this._allowPacking)
        return this.ReadPackedBits(1) > 0;
      return this.ReadByte() > (byte) 0;
    }

    public byte[] ReadData(int length)
    {
      byte[] numArray = new byte[length];
      Buffer.BlockCopy((Array) this.buffer, this.position, (Array) numArray, 0, length);
      this.position += length;
      return numArray;
    }

    public object Read(System.Type type, bool allowPacking = true)
    {
      if (type == typeof (string))
        return (object) this.ReadString();
      if (type == typeof (float))
        return (object) this.ReadFloat();
      if (type == typeof (double))
        return (object) this.ReadDouble();
      if (type == typeof (byte))
        return (object) this.ReadByte();
      if (type == typeof (sbyte))
        return (object) this.ReadSByte();
      if (type == typeof (bool))
        return (object) this.ReadBool();
      if (type == typeof (short))
        return (object) this.ReadShort();
      if (type == typeof (ushort))
        return (object) this.ReadUShort();
      if (type == typeof (int))
        return (object) this.ReadInt();
      if (type == typeof (uint))
        return (object) this.ReadUInt();
      if (type == typeof (long))
        return (object) this.ReadLong();
      if (type == typeof (ulong))
        return (object) this.ReadULong();
      if (type == typeof (char))
        return (object) this.ReadChar();
      if (type == typeof (Vec2))
        return (object) this.ReadVec2();
      if (type == typeof (BitBuffer))
        return (object) this.ReadBitBuffer(allowPacking);
      if (type == typeof (NetIndex16))
        return (object) new NetIndex16((int) this.ReadUShort());
      if (type == typeof (NetIndex2))
        return (object) new NetIndex2((int) this.ReadBits(typeof (int), 2));
      if (type == typeof (NetIndex4))
        return (object) new NetIndex4((int) this.ReadBits(typeof (int), 4));
      if (type == typeof (NetIndex8))
        return (object) new NetIndex8((int) this.ReadBits(typeof (int), 8));
      if (!typeof (Thing).IsAssignableFrom(type))
        throw new Exception("Trying to read unsupported type " + (object) type + " from BitBuffer!");
      byte num1 = this.ReadByte();
      ushort key = (ushort) this.ReadBits(typeof (ushort), 10);
      ushort num2 = this.ReadUShort();
      if ((int) num1 != (int) DuckNetwork.levelIndex)
        return (object) null;
      if (num2 == (ushort) 0)
      {
        if (key == (ushort) 0)
          return (object) null;
        Thing thing = Editor.CreateThing(Editor.IDToType[key]);
        Level.Add(thing);
        return (object) thing;
      }
      System.Type t = Editor.IDToType[key];
      GhostObject ghost = GhostManager.context.GetGhost((NetIndex16) (int) num2);
      if (ghost != null && ghost.thing.GetType() != t)
      {
        DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Type mismatch, removing ghost (" + num2.ToString() + " " + ghost.thing.GetType().ToString() + " " + t.ToString() + ")");
        GhostManager.context.RemoveGhost(ghost);
        ghost = (GhostObject) null;
      }
      if (ghost == null)
      {
        if (t == typeof (Duck))
          return (object) null;
        Thing thing = Editor.CreateThing(t);
        ghost = GhostManager.context.MakeGhost(thing, (int) num2);
        ghost.ClearStateMask(NetworkConnection.context);
        Level.Add(thing);
        thing.connection = NetworkConnection.context;
      }
      return (object) ghost.thing;
    }

    public object ReadBits(System.Type t, int bits) => bits == -1 ? this.Read(t) : this.ConvertType(this.ReadPackedBits(bits), t);

    public T ReadBits<T>(int bits) => bits < 1 ? default (T) : (T) this.ConvertType(this.ReadPackedBits(bits), typeof (T));

    protected object ConvertType(int obj, System.Type type)
    {
      if (type == typeof (float))
        return (object) (float) obj;
      if (type == typeof (double))
        return (object) (double) obj;
      if (type == typeof (byte))
        return (object) (byte) obj;
      if (type == typeof (sbyte))
        return (object) (sbyte) obj;
      if (type == typeof (short))
        return (object) (short) obj;
      if (type == typeof (ushort))
        return (object) (ushort) obj;
      if (type == typeof (int))
        return (object) obj;
      if (type == typeof (uint))
        return (object) (uint) obj;
      if (type == typeof (long))
        return (object) (long) obj;
      if (type == typeof (ulong))
        return (object) (ulong) obj;
      if (type == typeof (char))
        return (object) (char) obj;
      throw new Exception("unrecognized conversion type " + (object) type);
    }

    public T Read<T>() => (T) this.Read(typeof (T));

    public void WriteBufferData(BitBuffer val)
    {
      if (!val.isPacked && !this.isPacked)
      {
        if (this.position + val.lengthInBytes > this._buffer.Length)
          this.resize(this.position + val.lengthInBytes);
        for (int index = 0; index < val.lengthInBytes; ++index)
        {
          this._buffer[this.position] = val.buffer[index];
          ++this.position;
        }
      }
      else
        this.WritePacked(val.buffer, val.lengthInBits);
    }

    public void Write(BitBuffer val, bool writeLength = true)
    {
      if (writeLength)
        this.Write(val.allowPacking ? (ushort) val.lengthInBits : (ushort) val.lengthInBytes);
      this.WriteBufferData(val);
    }

    public void Write(byte[] data, int offset = 0, int length = -1)
    {
      if (!this.isPacked)
      {
        if (length < 0)
          length = data.Length;
        if (this.position + length > this._buffer.Length)
          this.resize(this.position + length);
        for (int index = 0; index < length; ++index)
        {
          this._buffer[this.position] = data[offset + index];
          ++this.position;
        }
      }
      else
        this.WritePacked(data);
    }

    public void Write(string val)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.Write((ushort) ((IEnumerable<byte>) bytes).Count<byte>());
        this.WritePacked(bytes);
      }
      else
      {
        int val1 = ((IEnumerable<byte>) bytes).Count<byte>();
        if (val1 > (int) ushort.MaxValue)
        {
          this.Write(ushort.MaxValue);
          this.Write((ushort) 42252);
          this.Write(val1);
        }
        else
          this.Write((ushort) ((IEnumerable<byte>) bytes).Count<byte>());
        int num = ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += num;
      }
    }

    public void Write(long val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(ulong val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(int val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(uint val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(short val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(ushort val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(float val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(Vec2 val)
    {
      this.Write(val.x);
      this.Write(val.y);
    }

    public void Write(double val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(char val)
    {
      byte[] bytes = BitConverter.GetBytes(val);
      if (this.bitOffset != 0)
      {
        this.WritePacked(bytes);
      }
      else
      {
        byte num = (byte) ((IEnumerable<byte>) bytes).Count<byte>();
        if (this.position + (int) num > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + (int) num);
        bytes.CopyTo((Array) this._buffer, this.position);
        this.position += ((IEnumerable<byte>) bytes).Count<byte>();
      }
    }

    public void Write(byte val)
    {
      if (this.bitOffset != 0)
      {
        this.WritePacked((int) val, 8);
      }
      else
      {
        if (this.position + 1 > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + 1);
        this._buffer[this.position] = val;
        ++this.position;
      }
    }

    public void Write(sbyte val)
    {
      if (this.bitOffset != 0)
      {
        this.WritePacked((int) val, 8);
      }
      else
      {
        if (this.position + 1 > ((IEnumerable<byte>) this._buffer).Count<byte>())
          this.resize(this.position + 1);
        this._buffer[this.position] = (byte) val;
        ++this.position;
      }
    }

    public void Write(bool val)
    {
      if (this._allowPacking)
        this.WritePacked(val ? 1 : 0, 1);
      else
        this.Write(val ? (byte) 1 : (byte) 0);
    }

    public void Write(object obj)
    {
      switch (obj)
      {
        case string _:
          this.Write((string) obj);
          break;
        case byte[] _:
          this.Write((byte[]) obj, 0, -1);
          break;
        case BitBuffer _:
          this.Write(obj as BitBuffer, true);
          break;
        case float val:
          this.Write(val);
          break;
        case double val:
          this.Write(val);
          break;
        case byte val:
          this.Write(val);
          break;
        case sbyte val:
          this.Write(val);
          break;
        case bool val:
          this.Write(val);
          break;
        case short val:
          this.Write(val);
          break;
        case ushort val:
          this.Write(val);
          break;
        case int val:
          this.Write(val);
          break;
        case uint val:
          this.Write(val);
          break;
        case long val:
          this.Write(val);
          break;
        case ulong val:
          this.Write(val);
          break;
        case char val:
          this.Write(val);
          break;
        case Vec2 val:
          this.Write(val);
          break;
        case NetIndex16 netIndex16:
          this.Write((ushort) (int) netIndex16);
          break;
        case NetIndex2 netIndex2:
          this.WriteBits((object) (ushort) (int) netIndex2, 2);
          break;
        case NetIndex4 netIndex4:
          this.WriteBits((object) (ushort) (int) netIndex4, 4);
          break;
        case NetIndex8 netIndex8:
          this.WriteBits((object) (ushort) (int) netIndex8, 8);
          break;
        case Thing _:
          this.Write(DuckNetwork.levelIndex);
          this.WriteBits((object) Editor.IDToType[(obj as Thing).GetType()], 10);
          if ((obj as Thing).ghostObject != null)
          {
            this.Write((ushort) (int) (obj as Thing).ghostObject.ghostObjectIndex);
            break;
          }
          this.Write((ushort) 0);
          break;
        case null:
          this.Write(DuckNetwork.levelIndex);
          this.WriteBits((object) (ushort) 0, 10);
          this.Write((ushort) 0);
          break;
        default:
          throw new Exception("Trying to write unsupported type " + (object) obj.GetType() + " to BitBuffer!");
      }
    }

    public void WriteBits(object obj, int bits)
    {
      if (bits == -1)
        this.Write(obj);
      else
        this.WritePacked(Convert.ToInt32(obj), bits);
    }

    private void resize(int bytes)
    {
      int length = ((IEnumerable<byte>) this._buffer).Count<byte>() * 2;
      while (length < bytes)
        length *= 2;
      byte[] numArray = new byte[length];
      this._buffer.CopyTo((Array) numArray, 0);
      this._buffer = numArray;
    }

    public void Clear()
    {
      this.position = 0;
      this._endPosition = 0;
      this._bitOffsetPosition = 0;
      this._bitEndOffset = 0;
      Array.Clear((Array) this._buffer, 0, this._buffer.Length);
    }
  }
}
