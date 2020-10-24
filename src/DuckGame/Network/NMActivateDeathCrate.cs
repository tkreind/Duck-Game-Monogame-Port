// Decompiled with JetBrains decompiler
// Type: DuckGame.NMActivateDeathCrate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMActivateDeathCrate : NMEvent
  {
    public DeathCrate crate;
    public byte setting;

    public NMActivateDeathCrate()
    {
    }

    public NMActivateDeathCrate(byte sett, DeathCrate d)
    {
      this.setting = sett;
      this.crate = d;
    }

    public override void Activate()
    {
      if (this.crate == null)
        return;
      this.crate.settingIndex = this.setting;
      this.crate.setting.Activate(this.crate, false);
    }
  }
}
