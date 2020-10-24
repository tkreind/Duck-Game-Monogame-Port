// Decompiled with JetBrains decompiler
// Type: DuckGame.NMPeerInformation
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Net;

namespace DuckGame
{
  public class NMPeerInformation : NMEvent
  {
    public int port;
    public IPAddress address;

    public NMPeerInformation()
    {
    }

    public NMPeerInformation(IPAddress vaddress, int vport)
    {
      this.address = vaddress;
      this.port = vport;
    }

    protected override void OnSerialize()
    {
      byte[] addressBytes = this.address.GetAddressBytes();
      BitBuffer val = new BitBuffer();
      val.Write(addressBytes, 0, -1);
      this._serializedData.Write(val, true);
      this._serializedData.Write(this.port);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      this.address = new IPAddress(d.ReadBitBuffer().buffer);
      this.port = d.ReadInt();
    }
  }
}
