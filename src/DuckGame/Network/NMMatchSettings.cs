// Decompiled with JetBrains decompiler
// Type: DuckGame.NMMatchSettings
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class NMMatchSettings : NMDuckNetworkEvent
  {
    public byte winsPerSet;
    public byte roundsPerIntermission;
    public byte randomPercent;
    public byte workshopPercent;
    public bool teams;
    public bool initialSettings;
    public byte customPercent;
    public byte normalPercent;
    public bool wallmode;
    public int customLevels;
    private List<byte> _enabledModifiers = new List<byte>();

    public NMMatchSettings()
    {
    }

    public NMMatchSettings(
      bool initial,
      byte varWinsPerSet,
      byte varRoundsPerIntermission,
      byte varRandomPercent,
      byte varWorkshopPercent,
      byte varNormalPercent,
      bool varTeams,
      byte varCustomPercent,
      int varCustomLevels,
      bool varWallmode,
      List<byte> enabledModifiers)
    {
      this.roundsPerIntermission = varRoundsPerIntermission;
      this.winsPerSet = varWinsPerSet;
      this.randomPercent = varRandomPercent;
      this.workshopPercent = varWorkshopPercent;
      this.teams = varTeams;
      this._enabledModifiers = enabledModifiers;
      this.initialSettings = initial;
      this.customPercent = varCustomPercent;
      this.customLevels = varCustomLevels;
      this.normalPercent = varNormalPercent;
      this.wallmode = varWallmode;
    }

    protected override void OnSerialize()
    {
      base.OnSerialize();
      this._serializedData.Write((byte) this._enabledModifiers.Count);
      foreach (byte enabledModifier in this._enabledModifiers)
        this._serializedData.Write(enabledModifier);
    }

    public override void OnDeserialize(BitBuffer d)
    {
      base.OnDeserialize(d);
      byte num = d.ReadByte();
      for (int index = 0; index < (int) num; ++index)
        this._enabledModifiers.Add(d.ReadByte());
    }

    public override void Activate()
    {
      TeamSelect2.GetMatchSetting("requiredwins").value = (object) (int) this.winsPerSet;
      TeamSelect2.GetMatchSetting("restsevery").value = (object) (int) this.roundsPerIntermission;
      TeamSelect2.GetMatchSetting("randommaps").value = (object) (int) this.randomPercent;
      TeamSelect2.GetMatchSetting("workshopmaps").value = (object) (int) this.workshopPercent;
      TeamSelect2.GetMatchSetting("normalmaps").value = (object) (int) this.normalPercent;
      TeamSelect2.GetMatchSetting("custommaps").value = (object) (int) this.customPercent;
      TeamSelect2.GetMatchSetting("wallmode").value = (object) this.wallmode;
      RockScoreboard.wallMode = this.wallmode;
      TeamSelect2.GetOnlineSetting("teams").value = (object) this.teams;
      TeamSelect2.prevCustomLevels = TeamSelect2.customLevels;
      TeamSelect2.customLevels = this.customLevels;
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        unlock.enabled = this._enabledModifiers.Contains(Unlocks.modifierToByte[unlock.id]);
        if (this.initialSettings)
          unlock.prevEnabled = unlock.enabled;
      }
      GameMode.roundsBetweenIntermission = (int) this.roundsPerIntermission;
      GameMode.winsPerSet = (int) this.winsPerSet;
      TeamSelect2.UpdateModifierStatus();
      if (this.initialSettings)
      {
        foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
          matchSetting.prevValue = matchSetting.value;
        foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
          onlineSetting.prevValue = onlineSetting.value;
      }
      base.Activate();
    }
  }
}
