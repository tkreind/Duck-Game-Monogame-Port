// Decompiled with JetBrains decompiler
// Type: DuckGame.DTCrush
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DTCrush : DestroyType
  {
    private PhysicsObject _thing;

    public PhysicsObject thing => this._thing;

    public DTCrush(PhysicsObject t)
      : base((Thing) t)
      => this._thing = t;
  }
}
