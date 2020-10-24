// Decompiled with JetBrains decompiler
// Type: DuckGame.NMGhostState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMGhostState : NetMessage
  {
    public BitBuffer data;
    public long mask;
    public NetIndex16 id = (NetIndex16) 0;
    public ushort classID;
    public byte levelIndex;
    public GhostObject ghost;
    public NetIndex8 authority = (NetIndex8) 0;
    public NetIndex16 tick = (NetIndex16) 0;
    public bool minimalState;

    public NMGhostState(BitBuffer dat)
    {
      this.data = dat;
      this.manager = BelongsToManager.GhostManager;
    }

    public NMGhostState() => this.manager = BelongsToManager.GhostManager;

    protected override void OnSerialize() => this._serializedData.WriteBufferData(this.data);

    public override void OnDeserialize(BitBuffer d)
    {
      this.id = (NetIndex16) (int) d.ReadUShort();
      this.classID = d.ReadUShort();
      this.levelIndex = d.ReadByte();
      this.authority = (NetIndex8) (int) d.ReadByte();
      this.tick = (NetIndex16) (int) d.ReadUShort();
      this.data = d.ReadBitBuffer();
    }
  }
}
