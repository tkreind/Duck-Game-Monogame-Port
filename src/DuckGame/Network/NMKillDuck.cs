// Decompiled with JetBrains decompiler
// Type: DuckGame.NMKillDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMKillDuck : NMEvent
  {
    public byte index;
    public bool crush;
    public bool cook;
    private byte _levelIndex;

    public NMKillDuck(byte idx, bool wasCrush, bool wasCook)
    {
      this.index = idx;
      this.crush = wasCrush;
      this.cook = wasCook;
    }

    public NMKillDuck()
    {
    }

    public override void Activate()
    {
      if (!(Level.current is GameLevel) || (int) DuckNetwork.levelIndex != (int) this._levelIndex)
        return;
      Profile profile = DuckNetwork.profiles[(int) this.index];
      if (profile.duck == null)
        return;
      DestroyType type = !this.crush ? (DestroyType) new DTImpact((Thing) null) : (DestroyType) new DTCrush((PhysicsObject) null);
      profile.duck.isKillMessage = true;
      profile.duck.Kill(type);
      if (!this.cook)
        profile.duck.GoRagdoll();
      profile.duck.isKillMessage = false;
      Thing.Fondle((Thing) profile.duck, this.connection);
      if (profile.duck._ragdollInstance != null)
        Thing.Fondle((Thing) profile.duck._ragdollInstance, this.connection);
      if (profile.duck._trappedInstance != null)
        Thing.Fondle((Thing) profile.duck._trappedInstance, this.connection);
      if (profile.duck._cookedInstance == null)
        return;
      Thing.Fondle((Thing) profile.duck._cookedInstance, this.connection);
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
