// Decompiled with JetBrains decompiler
// Type: DuckGame.InputProfileCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class InputProfileCore
  {
    public Dictionary<string, InputProfile> _profiles = new Dictionary<string, InputProfile>();
    public Dictionary<int, InputProfile> _virtualProfiles = new Dictionary<int, InputProfile>();

    public InputProfile Add(string name)
    {
      InputProfile inputProfile1 = new InputProfile(name);
      InputProfile inputProfile2;
      if (this._profiles.TryGetValue(name, out inputProfile2))
        return inputProfile2;
      this._profiles[name] = inputProfile1;
      return inputProfile1;
    }

    public InputProfile DefaultPlayer1 => this.Get("MPPlayer1");

    public InputProfile DefaultPlayer2 => this.Get("MPPlayer2");

    public InputProfile DefaultPlayer3 => this.Get("MPPlayer3");

    public InputProfile DefaultPlayer4 => this.Get("MPPlayer4");

    public List<InputProfile> defaultProfiles => new List<InputProfile>()
    {
      this.DefaultPlayer1,
      this.DefaultPlayer2,
      this.DefaultPlayer3,
      this.DefaultPlayer4
    };

    public InputProfile Get(string name)
    {
      InputProfile inputProfile;
      return this._profiles.TryGetValue(name, out inputProfile) ? inputProfile : (InputProfile) null;
    }

    public void Update()
    {
      foreach (KeyValuePair<string, InputProfile> profile in this._profiles)
        profile.Value.UpdateTriggerStates();
    }

    public InputProfile GetVirtualInput(int index)
    {
      InputProfile inputProfile1;
      if (this._virtualProfiles.TryGetValue(index, out inputProfile1))
        return inputProfile1;
      InputProfile inputProfile2 = this.Add("virtual" + (object) index);
      inputProfile2.dindex = NetworkDebugger.networkDrawingIndex;
      VirtualInput virtualInput = new VirtualInput(index);
      virtualInput.pdraw = NetworkDebugger.networkDrawingIndex;
      for (int index1 = 0; index1 < Network.synchronizedTriggers.Count; ++index1)
        inputProfile2.Map((InputDevice) virtualInput, Network.synchronizedTriggers[index1], index1);
      virtualInput.availableTriggers = Network.synchronizedTriggers;
      this._virtualProfiles[index] = inputProfile2;
      return inputProfile2;
    }
  }
}
