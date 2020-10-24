// Decompiled with JetBrains decompiler
// Type: DuckGame.PadState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>The state of a pad.</summary>
  public struct PadState
  {
    public PadButton buttons;
    public PadState.TriggerStates triggers;
    public PadState.StickStates sticks;

    public bool IsButtonDown(PadButton butt) => this.buttons.HasFlag((Enum) butt);

    public bool IsButtonUp(PadButton butt) => !this.buttons.HasFlag((Enum) butt);

    public struct TriggerStates
    {
      public float left;
      public float right;
    }

    public struct StickStates
    {
      public Vec2 left;
      public Vec2 right;
    }
  }
}
