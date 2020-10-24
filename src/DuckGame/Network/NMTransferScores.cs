// Decompiled with JetBrains decompiler
// Type: DuckGame.NMTransferScores
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMTransferScores : NMEvent
  {
    public List<int> scores = new List<int>();

    public NMTransferScores(List<int> scrs) => this.scores = scrs;

    public NMTransferScores()
    {
    }

    protected override void OnSerialize()
    {
      base.OnSerialize();
      this._serializedData.Write((byte) this.scores.Count);
      foreach (byte score in this.scores)
        this._serializedData.Write(score);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      base.OnDeserialize(d);
      byte num = d.ReadByte();
      for (int index = 0; index < (int) num; ++index)
        this.scores.Add((int) d.ReadByte());
    }

    public override void Activate()
    {
      int index = 0;
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.team != null)
          profile.team.score = this.scores[index];
        ++index;
      }
      GameMode.RunPostRound(false);
      Send.Message((NetMessage) new NMScoresReceived());
    }
  }
}
