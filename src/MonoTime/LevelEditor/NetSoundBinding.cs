// Decompiled with JetBrains decompiler
// Type: DuckGame.NetSoundBinding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NetSoundBinding : StateBinding
  {
    public override System.Type type => typeof (byte);

    public override object value
    {
      get => (object) (byte) (this._accessor.getAccessor(this._thing) as NetSoundEffect).index;
      set => (this._accessor.getAccessor(this._thing) as NetSoundEffect).index = (int) (byte) value;
    }

    public NetSoundBinding(string field)
      : base(field, 2)
    {
    }

    public NetSoundBinding(GhostPriority p, string field)
      : base(field, 2)
      => this._priority = p;
  }
}
