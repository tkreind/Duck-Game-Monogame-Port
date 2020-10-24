// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpecialHat
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMSpecialHat : NMDuckNetwork
  {
    private Team _team;
    private byte[] _data;
    public ulong link;

    public byte[] GetData() => this._data;

    public NMSpecialHat(Team t, ulong linkVal)
    {
      this._team = t;
      this.link = linkVal;
    }

    public NMSpecialHat()
    {
    }

    protected override void OnSerialize()
    {
      if (this._team != null)
      {
        this.serializedData.Write(true);
        this.serializedData.Write(new BitBuffer(this._team.customData), true);
      }
      else
        this.serializedData.Write(false);
      base.OnSerialize();
    }

    public override void OnDeserialize(BitBuffer data)
    {
      if (data.ReadBool())
        this._data = data.ReadBitBuffer().GetBytes();
      base.OnDeserialize(data);
    }
  }
}
