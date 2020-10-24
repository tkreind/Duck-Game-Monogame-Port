// Decompiled with JetBrains decompiler
// Type: DuckGame.CircularBuffer`1
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class CircularBuffer<T>
  {
    protected T[] _data;
    protected int _size;
    protected int _begin;
    protected int _length;

    public CircularBuffer(int size = 100)
    {
      this._data = new T[size];
      this._size = size;
      this._begin = 0;
      this._length = 0;
    }

    public void Add(T val)
    {
      if (this._length >= this._size)
        this.AdvanceBuffer();
      this._data[(this._begin + this._length) % this._size] = val;
      ++this._length;
    }

    public void AdvanceBuffer()
    {
      this._begin = (this._begin + 1) % this._size;
      --this._length;
      if (this._length >= 0)
        return;
      this._length = 0;
    }

    public T this[int key]
    {
      get
      {
        if (key >= this._length || key < 0)
          throw new Exception("Array Index Out Of Range");
        return this._data[(this._begin + key) % this._size];
      }
      set => this._data[(this._begin + key) % this._size] = value;
    }

    public int Count => this._length;

    public void Clear() => this._length = 0;
  }
}
