// Decompiled with JetBrains decompiler
// Type: DuckGame.NMParticles
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMParticles : NetMessage
  {
    private List<PhysicsParticle> _particles = new List<PhysicsParticle>();
    public byte levelIndex;
    public int count;
    public System.Type type;
    public BitBuffer data;

    public List<PhysicsParticle> GetParticles() => this._particles;

    public NMParticles()
    {
      this.manager = BelongsToManager.GhostManager;
      this.levelIndex = DuckNetwork.levelIndex;
    }

    public void SetParticles(List<PhysicsParticle> l) => this._particles = l;

    protected override void OnSerialize()
    {
      this._serializedData.Write(this.levelIndex);
      this._serializedData.Write((ushort) this._particles.Count);
      this._serializedData.WriteBits((object) (int) this._particles[0].GetNetType(), 4);
      BitBuffer bitBuffer = new BitBuffer();
      foreach (PhysicsParticle particle in this._particles)
      {
        bitBuffer.Write(particle.netIndex);
        particle.NetSerialize(bitBuffer);
      }
      this._serializedData.Write(bitBuffer, true);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      this.levelIndex = d.ReadByte();
      this.count = (int) d.ReadUShort();
      this.type = PhysicsParticle.ConvertNetType((byte) d.ReadBits(typeof (byte), 4));
      this.data = d.ReadBitBuffer();
    }
  }
}
