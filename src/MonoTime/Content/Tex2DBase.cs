﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Tex2DBase
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public abstract class Tex2DBase : IDisposable
  {
    protected short _textureIndex;
    protected string _textureName;
    protected float _frameWidth;
    protected float _frameHeight;
    protected int _currentObjectIndex = (int) Thing.GetGlobalIndex();

    public bool IsDisposed { get; private set; }

    public short textureIndex => this._textureIndex;

    internal void SetTextureIndex(short index) => this._textureIndex = index;

    public string textureName => this._textureName;

    public float frameWidth
    {
      get => this._frameWidth;
      set => this._frameWidth = value;
    }

    public float frameHeight
    {
      get => this._frameHeight;
      set => this._frameHeight = value;
    }

    public int currentObjectIndex
    {
      get => this._currentObjectIndex;
      set => this._currentObjectIndex = value;
    }

    public abstract object nativeObject { get; }

    public abstract int width { get; }

    public abstract int height { get; }

    public int w => this.width;

    public int h => this.height;

    protected Tex2DBase(string texName, short curTexIndex)
    {
      this._textureIndex = curTexIndex;
      this._textureName = texName;
    }

    ~Tex2DBase()
    {
      Content.GetTex2DFromIndex(this._textureIndex);
      Tex2DBase tex2Dbase = this;
      this.Dispose();
    }

    public abstract void GetData<T>(T[] data) where T : struct;

    public abstract Color[] GetData();

    public abstract void SetData<T>(T[] colors) where T : struct;

    public abstract void SetData(Color[] colors);

    public void Dispose()
    {
      if (this.IsDisposed)
        return;
      this.DisposeNative();
      this.IsDisposed = true;
    }

    protected abstract void DisposeNative();
  }
}
