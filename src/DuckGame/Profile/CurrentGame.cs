// Decompiled with JetBrains decompiler
// Type: DuckGame.CurrentGame
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CurrentGame
  {
    private int _kills;
    private int _cash;

    public int kills
    {
      get => this._kills;
      set => this._kills = value;
    }

    public int cash
    {
      get => this._cash;
      set => this._cash = value;
    }
  }
}
