// Decompiled with JetBrains decompiler
// Type: DuckGame.NMWeatherData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMWeatherData : NMEvent
  {
    public BitBuffer data;

    public NMWeatherData()
    {
    }

    public NMWeatherData(BitBuffer dat) => this.data = dat;

    protected override void OnSerialize() => this._serializedData.Write(this.data, true);

    public override void OnDeserialize(BitBuffer d) => this.data = d.ReadBitBuffer();
  }
}
