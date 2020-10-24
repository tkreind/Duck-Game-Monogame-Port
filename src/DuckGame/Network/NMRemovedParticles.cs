// Decompiled with JetBrains decompiler
// Type: DuckGame.NMRemovedParticles
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMRemovedParticles : NetMessage
  {
    private List<ushort> _particles = new List<ushort>();
    public byte levelIndex;
    public int count;
    public BitBuffer data;

    public List<ushort> GetParticles() => this._particles;

    public NMRemovedParticles()
    {
      this.manager = BelongsToManager.GhostManager;
      this.levelIndex = DuckNetwork.levelIndex;
    }

    public void SetParticles(List<ushort> l) => this._particles = l;

    protected override void OnSerialize()
    {
      this._serializedData.Write((ushort) this._particles.Count);
      BitBuffer val = new BitBuffer();
      foreach (ushort particle in this._particles)
        val.Write(particle);
      this._serializedData.Write(val, true);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      this.count = (int) d.ReadUShort();
      this.data = d.ReadBitBuffer();
    }
  }
}
