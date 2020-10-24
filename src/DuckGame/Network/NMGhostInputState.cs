// Decompiled with JetBrains decompiler
// Type: DuckGame.NMGhostInputState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMGhostInputState : NMGhostState
  {
    public List<ushort> inputStates = new List<ushort>();

    public NMGhostInputState(BitBuffer dat)
      : base(dat)
    {
    }

    public NMGhostInputState() => this.manager = BelongsToManager.GhostManager;

    public override void OnDeserialize(BitBuffer d)
    {
      for (int index = 0; index < NetworkConnection.packetsEvery; ++index)
        this.inputStates.Add(d.Read<ushort>());
      base.OnDeserialize(d);
    }
  }
}
