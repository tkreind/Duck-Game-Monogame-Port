// Decompiled with JetBrains decompiler
// Type: DuckGame.GameModeCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GameModeCore
  {
    public int roundsBetweenIntermission = 10;
    public int winsPerSet = 10;
    public bool _started;
    public bool getReady;
    public int _numMatchesPlayed;
    public bool showdown;
    public string previousLevel;
    public string _currentMusic = "";
    public bool firstDead;
    public bool playedGame;
  }
}
