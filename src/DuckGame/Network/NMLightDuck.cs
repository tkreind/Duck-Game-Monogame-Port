// Decompiled with JetBrains decompiler
// Type: DuckGame.NMLightDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMLightDuck : NMEvent
  {
    public byte index;
    private byte _levelIndex;

    public NMLightDuck(byte idx) => this.index = idx;

    public NMLightDuck()
    {
    }

    public override void Activate()
    {
      if (!(Level.current is GameLevel) || (int) DuckNetwork.levelIndex != (int) this._levelIndex)
        return;
      Profile profile = DuckNetwork.profiles[(int) this.index];
      if (profile.duck == null)
        return;
      profile.duck.isBurnMessage = true;
      profile.duck.Burn(profile.duck.position, (Thing) null);
      profile.duck.isBurnMessage = false;
    }

    protected override void OnSerialize()
    {
      base.OnSerialize();
      this._serializedData.Write(DuckNetwork.levelIndex);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      base.OnDeserialize(d);
      this._levelIndex = d.ReadByte();
    }
  }
}
