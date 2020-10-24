// Decompiled with JetBrains decompiler
// Type: DuckGame.InputProfile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class InputProfile
  {
    public const string SinglePlayer = "SinglePlayer";
    public const string MPPlayer1 = "MPPlayer1";
    public const string MPPlayer2 = "MPPlayer2";
    public const string MPPlayer3 = "MPPlayer3";
    public const string MPPlayer4 = "MPPlayer4";
    public const string Blank = "Blank";
    public InputProfile swapBack;
    private static InputProfileCore _core = new InputProfileCore();
    private static InputProfile _active;
    private InputDevice _lastActiveDevice;
    public InputDevice lastActiveOverride;
    private string _name;
    private Dictionary<InputDevice, MultiMap<string, int>> _mappings = new Dictionary<InputDevice, MultiMap<string, int>>();
    public static InputProfile Default;
    public int dindex;
    private bool _virtualInputInitialized;
    private VirtualInput _virtualInput;
    private ushort _state;
    private ushort _prevState;

    public static InputProfileCore core
    {
      get => InputProfile._core;
      set => InputProfile._core = value;
    }

    public static InputProfile active
    {
      get => InputProfile._active;
      set => InputProfile._active = value;
    }

    public static List<InputProfile> defaultProfiles => InputProfile._core.defaultProfiles;

    public static InputProfile FirstProfileWithDevice
    {
      get
      {
        foreach (InputProfile defaultProfile in InputProfile.defaultProfiles)
        {
          if (defaultProfile.lastActiveDevice != null && defaultProfile.lastActiveDevice.productName != null)
            return defaultProfile;
        }
        return InputProfile.DefaultPlayer1;
      }
    }

    public static InputProfile DefaultPlayer1 => InputProfile._core.DefaultPlayer1;

    public static InputProfile DefaultPlayer2 => InputProfile._core.DefaultPlayer2;

    public static InputProfile DefaultPlayer3 => InputProfile._core.DefaultPlayer3;

    public static InputProfile DefaultPlayer4 => InputProfile._core.DefaultPlayer4;

    public GenericController genericController
    {
      get
      {
        foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
        {
          if (mapping.Key is GenericController)
            return mapping.Key as GenericController;
        }
        return (GenericController) null;
      }
    }

    public static Dictionary<string, InputProfile> profiles => InputProfile._core._profiles;

    public static InputProfile Add(string name) => InputProfile._core.Add(name);

    public static void Update() => InputProfile._core.Update();

    public static InputProfile Get(string name) => InputProfile._core.Get(name);

    public InputDevice lastActiveDevice
    {
      get
      {
        if (this.lastActiveOverride != null)
          return this.lastActiveOverride;
        if (!MonoMain.started)
          return new InputDevice();
        if (this._lastActiveDevice == null)
        {
          foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
          {
            if (this._lastActiveDevice == null && mapping.Key is Keyboard)
              this._lastActiveDevice = mapping.Key;
            else if (mapping.Key is GenericController && (mapping.Key as GenericController).device is XInputPad)
              this._lastActiveDevice = mapping.Key;
          }
          if (this._lastActiveDevice == null)
            return new InputDevice();
        }
        return this._lastActiveDevice;
      }
      set => this._lastActiveDevice = value;
    }

    public string name => this._name;

    public void ClearMappings() => this._mappings.Clear();

    public MultiMap<string, int> GetMappings(System.Type t)
    {
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
      {
        if (mapping.Key.GetType() == t)
          return mapping.Value;
      }
      return (MultiMap<string, int>) null;
    }

    public InputDevice GetDevice(System.Type t)
    {
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
      {
        if (mapping.Key.GetType() == t)
          return mapping.Key;
      }
      return (InputDevice) null;
    }

    public int GetMapping(System.Type t, string trigger)
    {
      MultiMap<string, int> mappings = this.GetMappings(t);
      if (mappings == null)
        return -1;
      List<int> list = (List<int>) null;
      mappings.TryGetValue(trigger, out list);
      return list != null && list.Count > 0 ? list[0] : -1;
    }

    public string GetMappingString(System.Type t, string trigger)
    {
      int mapping1 = this.GetMapping(t, trigger);
      if (mapping1 == -1)
        return "";
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping2 in this._mappings)
      {
        if (mapping2.Key.GetType() == t)
        {
          Dictionary<int, string> triggerNames = mapping2.Key.GetTriggerNames();
          if (triggerNames != null)
          {
            string str = (string) null;
            return triggerNames.TryGetValue(mapping1, out str) ? str : "???";
          }
        }
      }
      return "";
    }

    public virtual Sprite GetTriggerImage(string trigger)
    {
      System.Type t = typeof (Keyboard);
      if (this.lastActiveDevice is GenericController)
        t = typeof (GenericController);
      else if (this.lastActiveDevice is DInputPad)
        t = typeof (DInputPad);
      else if (this.lastActiveDevice is XInputPad)
        t = typeof (XInputPad);
      int map = this.GetMapping(t, trigger);
      if (trigger == "DPAD")
        map = 9999;
      if (map != -1)
        return this.lastActiveDevice.DoGetMapImage(map);
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping1 in this._mappings)
      {
        int mapping2 = this.GetMapping(mapping1.Key.GetType(), trigger);
        if (mapping2 != -1)
          return mapping1.Key.DoGetMapImage(mapping2);
      }
      return (Sprite) null;
    }

    public InputProfile(string profile = "") => this._name = profile;

    public static InputProfile GetVirtualInput(int index) => InputProfile._core.GetVirtualInput(index);

    public VirtualInput virtualDevice
    {
      get
      {
        if (!this._virtualInputInitialized)
        {
          foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
          {
            if (mapping.Key is VirtualInput)
            {
              this._virtualInput = mapping.Key as VirtualInput;
              break;
            }
          }
          this._virtualInputInitialized = true;
        }
        return this._virtualInput;
      }
    }

    public virtual void Map(InputDevice device, string trigger, int mapping, bool clearExisting = false)
    {
      if (!this._mappings.ContainsKey(device))
        this._mappings[device] = new MultiMap<string, int>();
      if (clearExisting && this._mappings[device].ContainsKey(trigger))
        this._mappings[device][trigger].Clear();
      this._mappings[device].Add(trigger, mapping);
    }

    public InputProfile Clone()
    {
      InputProfile inputProfile = new InputProfile();
      inputProfile._name = this.name;
      inputProfile._state = this._state;
      inputProfile._prevState = this._prevState;
      inputProfile.dindex = this.dindex;
      inputProfile.swapBack = this.swapBack;
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
      {
        inputProfile._mappings[mapping.Key] = new MultiMap<string, int>();
        foreach (KeyValuePair<string, List<int>> keyValuePair in (MultiMap<string, int, List<int>>) mapping.Value)
          inputProfile._mappings[mapping.Key].AddRange(keyValuePair.Key, (ICollection<int>) keyValuePair.Value);
      }
      return inputProfile;
    }

    public virtual void UpdateExtraInput()
    {
    }

    public bool CheckCode(InputCode c) => c.Update(this);

    public virtual bool Pressed(string trigger, bool any = false)
    {
      if (Input.ignoreInput)
        return false;
      if (trigger == "ANY")
        any = true;
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping1 in this._mappings)
      {
        if (any)
        {
          if (mapping1.Key.MapPressed(-1, true))
            return true;
        }
        else
        {
          List<int> list;
          if (mapping1.Value.TryGetValue(trigger, out list))
          {
            foreach (int mapping2 in list)
            {
              if (mapping1.Key.MapPressed(mapping2, any))
                return true;
            }
          }
        }
      }
      return false;
    }

    public virtual bool Released(string trigger)
    {
      if (Input.ignoreInput)
        return false;
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping1 in this._mappings)
      {
        List<int> list;
        if (mapping1.Value.TryGetValue(trigger, out list))
        {
          foreach (int mapping2 in list)
          {
            if (mapping1.Key.MapReleased(mapping2))
              return true;
          }
        }
      }
      return false;
    }

    public virtual bool Down(string trigger)
    {
      if (Input.ignoreInput)
        return false;
      foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping1 in this._mappings)
      {
        List<int> list;
        if (mapping1.Value.TryGetValue(trigger, out list))
        {
          foreach (int mapping2 in list)
          {
            if (mapping1.Key.MapDown(mapping2))
            {
              if ((!(mapping1.Key is Keyboard) || !DuckNetwork.core.enteringText) && !(mapping1.Key is VirtualInput))
              {
                this._lastActiveDevice = mapping1.Key;
                Input.lastActiveProfile = this;
              }
              return true;
            }
          }
        }
      }
      return false;
    }

    public virtual float leftTrigger
    {
      get
      {
        if (Input.ignoreInput)
          return 0.0f;
        foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
        {
          if (mapping.Key is GenericController key)
          {
            List<int> list = (List<int>) null;
            if (!mapping.Value.TryGetValue("LTRIGGER", out list) || list.Count <= 0)
              return key.leftTrigger;
            switch (list[0])
            {
              case 4194304:
                return key.rightTrigger;
              case 8388608:
                return key.leftTrigger;
              default:
                continue;
            }
          }
        }
        return 0.0f;
      }
    }

    public float rightTrigger
    {
      get
      {
        if (Input.ignoreInput)
          return 0.0f;
        foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
        {
          if (mapping.Key is GenericController key)
          {
            List<int> list = (List<int>) null;
            if (!mapping.Value.TryGetValue("RTRIGGER", out list) || list.Count <= 0)
              return key.rightTrigger;
            switch (list[0])
            {
              case 4194304:
                return key.rightTrigger;
              case 8388608:
                return key.leftTrigger;
              default:
                continue;
            }
          }
        }
        return 0.0f;
      }
    }

    public Vec2 leftStick
    {
      get
      {
        if (Input.ignoreInput)
          return Vec2.Zero;
        foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
        {
          if (mapping.Key is GenericController key)
          {
            List<int> list = (List<int>) null;
            if (!mapping.Value.TryGetValue("LSTICK", out list) || list.Count <= 0)
              return key.leftStick;
            switch (list[0])
            {
              case 64:
                return key.leftStick;
              case 128:
                return key.rightStick;
              default:
                continue;
            }
          }
        }
        return new Vec2(0.0f, 0.0f);
      }
    }

    public Vec2 rightStick
    {
      get
      {
        if (Input.ignoreInput)
          return Vec2.Zero;
        foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in this._mappings)
        {
          if (mapping.Key is GenericController key)
          {
            List<int> list = (List<int>) null;
            if (!mapping.Value.TryGetValue("RSTICK", out list) || list.Count <= 0)
              return key.rightStick;
            switch (list[0])
            {
              case 64:
                return key.leftStick;
              case 128:
                return key.rightStick;
              default:
                continue;
            }
          }
        }
        return new Vec2(0.0f, 0.0f);
      }
    }

    public ushort state => this._state;

    public ushort prevState => this._prevState;

    public void UpdateTriggerStates()
    {
      this._prevState = this._state;
      this._state = (ushort) 0;
      foreach (string synchronizedTrigger in Network.synchronizedTriggers)
      {
        this._state |= this.Down(synchronizedTrigger) ? (ushort) 1 : (ushort) 0;
        this._state <<= 1;
      }
    }
  }
}
