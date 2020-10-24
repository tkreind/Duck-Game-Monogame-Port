// Decompiled with JetBrains decompiler
// Type: DuckGame.NMAssignWin
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMAssignWin : NMEvent
  {
    public List<int> indexes = new List<int>();
    public byte win;

    public NMAssignWin(List<int> idxs, byte winteam)
    {
      this.indexes = idxs;
      this.win = winteam;
    }

    public NMAssignWin()
    {
    }

    protected override void OnSerialize()
    {
      base.OnSerialize();
      this._serializedData.Write((byte) this.indexes.Count);
      foreach (byte index in this.indexes)
        this._serializedData.Write(index);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      base.OnDeserialize(d);
      byte num = d.ReadByte();
      for (int index = 0; index < (int) num; ++index)
        this.indexes.Add((int) d.ReadByte());
    }

    public override void Activate()
    {
      SFX.Play("scoreDing", 0.8f);
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (this.indexes.Contains((int) profile.networkIndex))
        {
          GameMode.lastWinners.Add(profile);
          PlusOne plusOne = new PlusOne(0.0f, 0.0f, this.win < (byte) 4 ? DuckNetwork.profiles[(int) this.win] : profile);
          plusOne.anchor = (Anchor) (Thing) profile.duck;
          plusOne.anchor.offset = new Vec2(0.0f, -16f);
          plusOne.depth = new Depth(0.95f);
          Level.Add((Thing) plusOne);
        }
      }
    }
  }
}
