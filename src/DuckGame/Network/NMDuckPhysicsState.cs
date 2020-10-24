// Decompiled with JetBrains decompiler
// Type: DuckGame.NMDuckPhysicsState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMDuckPhysicsState : NMPhysicsState
  {
    public int inputState;
    public ushort holding;

    public NMDuckPhysicsState()
    {
    }

    public NMDuckPhysicsState(
      Vec2 Position,
      Vec2 Velocity,
      ushort ObjectID,
      int ClientFrame,
      int InputState,
      ushort Holding)
      : base(Position, Velocity, ObjectID, ClientFrame)
    {
      this.inputState = InputState;
      this.holding = Holding;
    }
  }
}
