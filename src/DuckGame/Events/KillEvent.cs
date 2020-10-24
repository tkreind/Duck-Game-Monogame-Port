// Decompiled with JetBrains decompiler
// Type: DuckGame.KillEvent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class KillEvent : Event
  {
    private System.Type _weapon;

    public System.Type weapon => this._weapon;

    public KillEvent(Profile killerVal, Profile killedVal, System.Type weapon)
      : base(killerVal, killedVal)
      => this._weapon = weapon;
  }
}
