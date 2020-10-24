// Decompiled with JetBrains decompiler
// Type: DuckGame.NMDestroyProp
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMDestroyProp : NMEvent
  {
    public Thing prop;
    private byte _levelIndex;

    public NMDestroyProp(Thing t)
    {
      this.priority = NetMessagePriority.UnreliableUnordered;
      this.prop = t;
    }

    public NMDestroyProp()
    {
    }

    public override void Activate()
    {
      if (!(Level.current is GameLevel) || (int) DuckNetwork.levelIndex != (int) this._levelIndex || !(this.prop is MaterialThing prop))
        return;
      prop.NetworkDestroy();
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
