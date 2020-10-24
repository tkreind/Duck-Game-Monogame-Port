// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class LevelCore
  {
    public bool sendCustomLevels;
    private Level _nextLevel;
    private Level _currentLevel;
    public bool gameInProgress;
    public Queue<List<DrawCall>> drawCalls = new Queue<List<DrawCall>>();
    public List<DrawCall> currentFrameCalls = new List<DrawCall>();
    public Thing currentDrawingObject;
    public bool skipFrameLog;

    public Level nextLevel
    {
      get => this._nextLevel;
      set => this._nextLevel = value;
    }

    public Level currentLevel
    {
      get => this._currentLevel;
      set => this._currentLevel = value;
    }
  }
}
