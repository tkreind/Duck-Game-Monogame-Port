// Decompiled with JetBrains decompiler
// Type: DuckGame.Input
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace DuckGame
{
  public class Input
  {
    public static InputCode konamiCode = new InputCode()
    {
      triggers = new List<string>()
      {
        "UP",
        "UP",
        "DOWN",
        "DOWN",
        "LEFT",
        "RIGHT",
        "LEFT",
        "RIGHT",
        "QUACK",
        "JUMP"
      }
    };
    private static bool _ignoreInput;
    public static InputProfile lastActiveProfile = new InputProfile();
    private static List<Sprite> _buttonStyles = new List<Sprite>();
    public static Dictionary<Keys, char> keyToChar = new Dictionary<Keys, char>();
    private static Dictionary<string, InputProfile> _profiles = new Dictionary<string, InputProfile>();
    private static List<InputDevice> _devices = new List<InputDevice>();
    private static Dictionary<string, Sprite> _triggerImageMap = new Dictionary<string, Sprite>();
    private static List<GenericController> _gamePads = new List<GenericController>();
    private static Array _keys = Enum.GetValues(typeof (Keys));
    private static List<DeviceInputMapping> _defaultInputMapping = new List<DeviceInputMapping>();
    private static List<DeviceInputMapping> _defaultInputMappingPresets = new List<DeviceInputMapping>()
    {
      new DeviceInputMapping()
      {
        deviceName = "KEYBOARD P1",
        deviceGUID = "",
        map = new Dictionary<string, int>()
        {
          {
            "LEFT",
            65
          },
          {
            "RIGHT",
            68
          },
          {
            "UP",
            87
          },
          {
            "DOWN",
            83
          },
          {
            "JUMP",
            87
          },
          {
            "SHOOT",
            86
          },
          {
            "GRAB",
            67
          },
          {
            "START",
            27
          },
          {
            "RAGDOLL",
            81
          },
          {
            "STRAFE",
            66
          },
          {
            "QUACK",
            69
          },
          {
            "SELECT",
            32
          },
          {
            "CHAT",
            13
          }
        }
      },
      new DeviceInputMapping()
      {
        deviceName = "KEYBOARD P2",
        deviceGUID = "",
        map = new Dictionary<string, int>()
        {
          {
            "LEFT",
            37
          },
          {
            "RIGHT",
            39
          },
          {
            "UP",
            38
          },
          {
            "DOWN",
            40
          },
          {
            "JUMP",
            38
          },
          {
            "SHOOT",
            186
          },
          {
            "GRAB",
            76
          },
          {
            "START",
            187
          },
          {
            "RAGDOLL",
            73
          },
          {
            "STRAFE",
            75
          },
          {
            "QUACK",
            79
          },
          {
            "SELECT",
            161
          }
        }
      },
      new DeviceInputMapping()
      {
        deviceName = "XBOX GAMEPAD",
        deviceGUID = "",
        map = new Dictionary<string, int>()
        {
          {
            "LEFT",
            4
          },
          {
            "RIGHT",
            8
          },
          {
            "UP",
            1
          },
          {
            "DOWN",
            2
          },
          {
            "JUMP",
            4096
          },
          {
            "SHOOT",
            16384
          },
          {
            "GRAB",
            32768
          },
          {
            "START",
            16
          },
          {
            "RAGDOLL",
            512
          },
          {
            "STRAFE",
            256
          },
          {
            "QUACK",
            8192
          },
          {
            "SELECT",
            4096
          },
          {
            "LTRIGGER",
            8388608
          },
          {
            "RTRIGGER",
            4194304
          },
          {
            "LSTICK",
            64
          },
          {
            "RSTICK",
            128
          }
        }
      },
      new DeviceInputMapping()
      {
        deviceName = "GENERIC GAMEPAD",
        deviceGUID = "",
        map = new Dictionary<string, int>()
        {
          {
            "LEFT",
            4
          },
          {
            "RIGHT",
            8
          },
          {
            "UP",
            1
          },
          {
            "DOWN",
            2
          },
          {
            "JUMP",
            4096
          },
          {
            "SHOOT",
            16384
          },
          {
            "GRAB",
            32768
          },
          {
            "START",
            16
          },
          {
            "RAGDOLL",
            512
          },
          {
            "STRAFE",
            256
          },
          {
            "QUACK",
            8192
          },
          {
            "SELECT",
            4096
          },
          {
            "LTRIGGER",
            8388608
          },
          {
            "RTRIGGER",
            4194304
          },
          {
            "LSTICK",
            64
          },
          {
            "RSTICK",
            128
          }
        }
      }
    };
    private static bool _dinputEnabled = false;
    private static int _updateWaitFrames = 0;
    private static Thread _gamepadThread;
    private static bool _gamepadsChanged;
    private static bool _padConnectionChange = false;
    private static object _gamepadThreadLock = new object();
    private static object _gamepadEnumLock = new object();
    private static string _changeName = "";
    private static bool _changePluggedIn = false;
    public static volatile bool devicesChanged = false;
    private static int _deviceUpdateWait = 0;
    private static volatile bool _reEnumerate = false;
    public static bool uiDevicesHaveChanged = false;

    public static bool ignoreInput
    {
      get => !Graphics.inFocus || Input._ignoreInput;
      set => Input._ignoreInput = value;
    }

    public static List<Sprite> buttonStyles => Input._buttonStyles;

    public static Sprite GetTriggerSprite(string trigger)
    {
      Sprite sprite = (Sprite) null;
      Input._triggerImageMap.TryGetValue(trigger, out sprite);
      return sprite;
    }

    public static List<InputDevice> GetInputDevices() => Input._devices;

    public static void Save()
    {
      XDocument doc = new XDocument();
      XElement xelement = new XElement((XName) "Mappings");
      foreach (DeviceInputMapping deviceInputMapping in Input._defaultInputMapping)
        xelement.Add((object) deviceInputMapping.Serialize());
      doc.Add((object) xelement);
      string path = DuckFile.optionsDirectory + "/input.dat";
      DuckFile.SaveXDocument(doc, path);
    }

    public static List<DeviceInputMapping> defaultInputMappingPresets
    {
      get
      {
        List<DeviceInputMapping> deviceInputMappingList = new List<DeviceInputMapping>();
        foreach (DeviceInputMapping inputMappingPreset in Input._defaultInputMappingPresets)
          deviceInputMappingList.Add(inputMappingPreset.Clone());
        return deviceInputMappingList;
      }
    }

    public static DeviceInputMapping GetDefaultMapping(
      string productName,
      string productGUID,
      bool presets = false,
      bool makeClone = true,
      Profile p = null)
    {
      List<DeviceInputMapping> source = Input._defaultInputMapping;
      if (p != null && p.inputMappingOverrides.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceGUID == productGUID && x.deviceName == productName)) == null)
        p = (Profile) null;
      if (presets)
        source = Input.defaultInputMappingPresets;
      if (p != null)
        source = p.inputMappingOverrides;
      foreach (DeviceInputMapping deviceInputMapping in source)
      {
        if (deviceInputMapping.deviceName == productName && deviceInputMapping.deviceGUID == productGUID)
          return deviceInputMapping;
      }
      if (p != null)
        return (DeviceInputMapping) null;
      DeviceInputMapping deviceInputMapping1 = source.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceName == "GENERIC GAMEPAD"));
      if (!makeClone)
        return deviceInputMapping1;
      if (deviceInputMapping1 == null)
        return new DeviceInputMapping();
      DeviceInputMapping deviceInputMapping2 = deviceInputMapping1.Clone();
      deviceInputMapping2.deviceName = productName;
      deviceInputMapping2.deviceGUID = productGUID;
      return deviceInputMapping2;
    }

    public static void SetDefaultMapping(DeviceInputMapping mapping, Profile overrideProfile = null)
    {
      List<DeviceInputMapping> source = Input._defaultInputMapping;
      if (overrideProfile != null)
        source = overrideProfile.inputMappingOverrides;
      DeviceInputMapping deviceInputMapping1 = source.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceName == mapping.deviceName && x.deviceGUID == mapping.deviceGUID));
      DeviceInputMapping deviceInputMapping2 = Input.defaultInputMappingPresets.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceName == mapping.deviceName && x.deviceGUID == mapping.deviceGUID));
      if (deviceInputMapping1 != null)
      {
        deviceInputMapping1.map = mapping.map;
        deviceInputMapping1.graphicMap = mapping.graphicMap;
        if (deviceInputMapping2 == null)
          return;
        foreach (KeyValuePair<string, int> keyValuePair in deviceInputMapping2.map)
        {
          if (!deviceInputMapping1.map.ContainsKey(keyValuePair.Key))
            deviceInputMapping1.map[keyValuePair.Key] = keyValuePair.Value;
        }
      }
      else
      {
        DeviceInputMapping compare = Input._defaultInputMapping.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceName == mapping.deviceName && x.deviceGUID == mapping.deviceGUID));
        if (compare != null)
        {
          if (mapping.IsEqual(compare))
            return;
          source.Add(mapping);
        }
        else
        {
          if (Input._defaultInputMapping.FirstOrDefault<DeviceInputMapping>((Func<DeviceInputMapping, bool>) (x => x.deviceName == "GENERIC GAMEPAD")).IsEqual(mapping))
            return;
          source.Add(mapping);
        }
      }
    }

    public static List<DeviceInputMapping> CloneDefaultMappings()
    {
      List<DeviceInputMapping> deviceInputMappingList = new List<DeviceInputMapping>();
      foreach (DeviceInputMapping deviceInputMapping in Input._defaultInputMapping)
        deviceInputMappingList.Add(deviceInputMapping.Clone());
      return deviceInputMappingList;
    }

    public static void SetDefaultMappings(List<DeviceInputMapping> mappings) => Input._defaultInputMapping = mappings;

    public static void ApplyDefaultMappings()
    {
      foreach (InputProfile defaultProfile in InputProfile.defaultProfiles)
        Input.ApplyDefaultMapping(defaultProfile);
    }

    public static void ApplyDefaultMapping(InputProfile p = null, Profile duckProfile = null)
    {
      GenericController device = p.GetDevice(typeof (GenericController)) as GenericController;
      p.ClearMappings();
      if (device != null)
      {
        if (device.device != null)
        {
          DeviceInputMapping deviceInputMapping = Input.GetDefaultMapping(device.device.productName, device.device.productGUID, p: duckProfile) ?? Input.GetDefaultMapping(device.device.productName, device.device.productGUID);
          if (deviceInputMapping != null)
          {
            foreach (KeyValuePair<string, int> keyValuePair in deviceInputMapping.map)
              p.Map((InputDevice) device, keyValuePair.Key, keyValuePair.Value);
          }
        }
        else
          p.Map((InputDevice) device, "", 0);
      }
      if (p == InputProfile.defaultProfiles[Options.Data.keyboard1PlayerIndex])
      {
        DeviceInputMapping deviceInputMapping = Input.GetDefaultMapping("KEYBOARD P1", "", p: duckProfile) ?? Input.GetDefaultMapping("KEYBOARD P1", "");
        if (deviceInputMapping == null)
          return;
        foreach (KeyValuePair<string, int> keyValuePair in deviceInputMapping.map)
          p.Map((InputDevice) Input.GetDevice<Keyboard>(), keyValuePair.Key, keyValuePair.Value);
      }
      else
      {
        if (p != InputProfile.defaultProfiles[Options.Data.keyboard2PlayerIndex])
          return;
        DeviceInputMapping deviceInputMapping = Input.GetDefaultMapping("KEYBOARD P2", "", p: duckProfile) ?? Input.GetDefaultMapping("KEYBOARD P2", "");
        if (deviceInputMapping == null)
          return;
        foreach (KeyValuePair<string, int> keyValuePair in deviceInputMapping.map)
          p.Map((InputDevice) Input.GetDevice<Keyboard>(1), keyValuePair.Key, keyValuePair.Value);
      }
    }

    public static void InitializeGraphics()
    {
      MonoMain.loadMessage = "Loading Input";
      foreach (Keys key in Input._keys)
      {
        char ch = KeyHelper.KeyToChar(key);
        if (ch > ' ' && ch < '\x007F')
          Input.keyToChar[key] = ch;
      }
      Input._triggerImageMap.Add("MOUSEWHEEL", new Sprite("buttons/mousewheel"));
      Input._triggerImageMap.Add("PLANET", new Sprite("smallEarth"));
      Input._triggerImageMap.Add("MOON", new Sprite("smallMoon"));
      Input._triggerImageMap.Add("PLUG", new Sprite("plugRect"));
      Input._triggerImageMap.Add("UNPLUG", new Sprite("unplugRect"));
      Input._triggerImageMap.Add("NORMALICON", new Sprite("normalIcon"));
      Input._triggerImageMap.Add("RAINBOWICON", new Sprite("rainbowIcon"));
      Input._triggerImageMap.Add("CUSTOMICON", new Sprite("customIcon"));
      Input._triggerImageMap.Add("RANDOMICON", new Sprite("randomIcons"));
      Input._triggerImageMap.Add("ESCAPE", new Sprite("buttons/keyboard/escape"));
      Input._triggerImageMap.Add("TINYLOCK", new Sprite("tinyLock"));
      Input._triggerImageMap.Add("RETICULE", new Sprite("challenge/reticule"));
      Input._triggerImageMap.Add("TICKET", new Sprite("arcade/ticket"));
      Input._triggerImageMap.Add("CHECK", new Sprite("checkIcon"));
      Input._triggerImageMap.Add("STARGOODY", new Sprite("challenge/star"));
      Input._triggerImageMap.Add("SUITCASEGOODY", new Sprite("challenge/suitcase"));
      Input._triggerImageMap.Add("LAPGOODY", new Sprite("challenge/goal"));
      Input._triggerImageMap.Add("LWING", new Sprite("arcade/titleWing"));
      Sprite sprite1 = new Sprite("arcade/titleWing");
      sprite1.flipH = true;
      sprite1.centerx = (float) sprite1.width;
      Input._triggerImageMap.Add("RWING", sprite1);
      Sprite sprite2 = new Sprite("arcade/titleWing");
      sprite2.color = new Color(96, 119, 124);
      ++sprite2.centery;
      Input._triggerImageMap.Add("LWINGGRAY", sprite2);
      Sprite sprite3 = new Sprite("arcade/titleWing");
      sprite3.flipH = true;
      sprite3.centerx = (float) sprite3.width;
      ++sprite3.centery;
      sprite3.color = new Color(96, 119, 124);
      Input._triggerImageMap.Add("RWINGGRAY", sprite3);
      Input._triggerImageMap.Add("WRENCH", new Sprite("titleWrench"));
      Input._triggerImageMap.Add("SCREWDRIVER", new Sprite("titleScrewdriver"));
      Input._triggerImageMap.Add("BASELINE", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 0
      });
      Input._triggerImageMap.Add("BRONZE", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 1
      });
      Input._triggerImageMap.Add("SILVER", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 2
      });
      Input._triggerImageMap.Add("GOLD", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 3
      });
      Input._triggerImageMap.Add("PLATINUM", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 4
      });
      Input._triggerImageMap.Add("DEVELOPER", (Sprite) new SpriteMap("challengeTrophyIcons", 16, 16)
      {
        frame = 5
      });
      Input._triggerImageMap.Add("ONLINEBAD", (Sprite) new SpriteMap("onlineStatusIcons", 7, 7)
      {
        frame = 0
      });
      Input._triggerImageMap.Add("ONLINENEUTRAL", (Sprite) new SpriteMap("onlineStatusIcons", 7, 7)
      {
        frame = 1
      });
      Input._triggerImageMap.Add("ONLINEGOOD", (Sprite) new SpriteMap("onlineStatusIcons", 7, 7)
      {
        frame = 2
      });
      Sprite sprite4 = new Sprite("crownIcon");
      sprite4.scale = new Vec2(0.5f, 0.5f);
      sprite4.centery -= 6f;
      Input._triggerImageMap.Add("HOSTCROWN", sprite4);
      Sprite sprite5 = new Sprite("subPlus");
      Input._triggerImageMap.Add("SUBPLUS", sprite5);
      Sprite sprite6 = new Sprite("steamIcon");
      sprite6.scale = new Vec2(0.25f, 0.25f);
      sprite6.centery -= 48f;
      Input._triggerImageMap.Add("STEAMICON", sprite6);
      Sprite sprite7 = new Sprite("skipSpin");
      Input._triggerImageMap.Add("SKIPSPIN", sprite7);
      Input._triggerImageMap.Add("SIGNALDEAD", (Sprite) new SpriteMap("signal", 8, 5)
      {
        frame = 0
      });
      Input._triggerImageMap.Add("SIGNALBAD", (Sprite) new SpriteMap("signal", 8, 5)
      {
        frame = 1
      });
      Input._triggerImageMap.Add("SIGNALNORMAL", (Sprite) new SpriteMap("signal", 8, 5)
      {
        frame = 2
      });
      Input._triggerImageMap.Add("SIGNALGOOD", (Sprite) new SpriteMap("signal", 8, 5)
      {
        frame = 3
      });
      Input._triggerImageMap.Add("PLUSKEY", (Sprite) new KeyImage('+'));
      Input._triggerImageMap.Add("ENTERKEY", new Sprite("buttons/keyboard/enter"));
      Input._triggerImageMap.Add("ICONGRADIENT", new Sprite("iconGradient"));
      Input._triggerImageMap.Add("LEFTMOUSE", (Sprite) new SpriteMap("buttons/mouse", 12, 15)
      {
        frame = 0
      });
      Input._triggerImageMap.Add("MIDDLEMOUSE", (Sprite) new SpriteMap("buttons/mouse", 12, 15)
      {
        frame = 1
      });
      Input._triggerImageMap.Add("RIGHTMOUSE", (Sprite) new SpriteMap("buttons/mouse", 12, 15)
      {
        frame = 2
      });
      Input._triggerImageMap.Add("LOADICON", (Sprite) new SpriteMap("iconSheet", 16, 16)
      {
        frame = 1
      });
      Input._triggerImageMap.Add("SAVEICON", (Sprite) new SpriteMap("iconSheet", 16, 16)
      {
        frame = 2
      });
      SpriteMap spriteMap1 = new SpriteMap("iconSheet", 16, 16);
      spriteMap1.scale = new Vec2(0.5f, 0.5f);
      spriteMap1.centery = -6f;
      spriteMap1.frame = 1;
      Input._triggerImageMap.Add("LOADICONTINY", (Sprite) spriteMap1);
      SpriteMap spriteMap2 = new SpriteMap("iconSheet", 16, 16);
      spriteMap2.scale = new Vec2(0.5f, 0.5f);
      spriteMap2.centery = -6f;
      spriteMap2.frame = 2;
      Input._triggerImageMap.Add("SAVEICONTINY", (Sprite) spriteMap2);
      SpriteMap spriteMap3 = new SpriteMap("iconSheet", 16, 16);
      spriteMap3.scale = new Vec2(0.5f, 0.5f);
      spriteMap3.centery = -6f;
      spriteMap3.frame = 0;
      Input._triggerImageMap.Add("NEWICONTINY", (Sprite) spriteMap3);
      SpriteMap spriteMap4 = new SpriteMap("exBox", 10, 10);
      spriteMap4.frame = 0;
      spriteMap4.scale = new Vec2(0.5f, 0.5f);
      spriteMap4.centery -= 6f;
      Input._triggerImageMap.Add("ITEMBOX", (Sprite) spriteMap4);
      SpriteMap spriteMap5 = new SpriteMap("exBox", 10, 10);
      spriteMap5.frame = 1;
      spriteMap5.scale = new Vec2(0.5f, 0.5f);
      spriteMap5.centery -= 6f;
      Input._triggerImageMap.Add("USERONLINE", (Sprite) spriteMap5);
      SpriteMap spriteMap6 = new SpriteMap("exBox", 10, 10);
      spriteMap6.frame = 2;
      spriteMap6.scale = new Vec2(0.5f, 0.5f);
      spriteMap6.centery -= 6f;
      Input._triggerImageMap.Add("USERAWAY", (Sprite) spriteMap6);
      SpriteMap spriteMap7 = new SpriteMap("exBox", 10, 10);
      spriteMap7.frame = 3;
      spriteMap7.scale = new Vec2(0.5f, 0.5f);
      spriteMap7.centery -= 6f;
      Input._triggerImageMap.Add("USERBUSY", (Sprite) spriteMap7);
      SpriteMap spriteMap8 = new SpriteMap("exBox", 10, 10);
      spriteMap8.frame = 4;
      spriteMap8.scale = new Vec2(0.5f, 0.5f);
      spriteMap8.centery -= 6f;
      Input._triggerImageMap.Add("USEROFFLINE", (Sprite) spriteMap8);
      Input._buttonStyles.Add(new Sprite("buttons/xbox/oButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/aButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/uButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/yButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/startButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/selectButton"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/dPadLeft"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/dPadRight"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/dPadUp"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/dPadDown"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/leftBumper"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/rightBumper"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/leftTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/rightTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/leftStick"));
      Input._buttonStyles.Add(new Sprite("buttons/xbox/rightStick"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/o"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/square"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/triangle"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/x"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/startButton"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/selectButton"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/leftBumper"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/rightBumper"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/leftTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/rightTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/a"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/b"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/x"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/y"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/aFami"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/bFami"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/xFami"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/yFami"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/startButton"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/selectButton"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/leftTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/SNES/rightTrigger"));
      Input._buttonStyles.Add(new Sprite("buttons/genesis/a"));
      Input._buttonStyles.Add(new Sprite("buttons/genesis/b"));
      Input._buttonStyles.Add(new Sprite("buttons/genesis/c"));
      Input._buttonStyles.Add(new Sprite("buttons/genesis/start"));
      Input._buttonStyles.Add(new Sprite("buttons/playstation/blank"));
      Input._buttonStyles.Add(new Sprite("buttons/genericButton"));
    }

    public static void InitDefaultProfiles()
    {
      for (int index = 0; index < 4; ++index)
      {
        InputProfile inputProfile = InputProfile.Add("MPPlayer" + (index + 1).ToString());
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "LEFT", 4);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "RIGHT", 8);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "UP", 1);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "DOWN", 2);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "JUMP", 4096);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "SHOOT", 16384);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "GRAB", 32768);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "QUACK", 8192);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "START", 16);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "STRAFE", 256);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "RAGDOLL", 512);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "LTRIGGER", 8388608);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "RTRIGGER", 4194304);
        inputProfile.Map((InputDevice) Input.GetDevice<GenericController>(index), "SELECT", 4096);
        if (index == 0)
          InputProfile.active = inputProfile;
      }
      Input.ApplyDefaultMappings();
      InputProfile.Add("Blank");
    }

    public static void Initialize()
    {
      foreach (DeviceInputMapping inputMappingPreset in Input._defaultInputMappingPresets)
        Input._defaultInputMapping.Add(inputMappingPreset.Clone());
      InputDevice device = (InputDevice) new Keyboard("KEYBOARD P1", 0);
      Input._devices.Add(device);
      InputDevice inputDevice1 = (InputDevice) new Keyboard("KEYBOARD P2", 1);
      Input._devices.Add(inputDevice1);
      InputDevice inputDevice2 = (InputDevice) new Mouse();
      Input._devices.Add(inputDevice2);
      InputDevice inputDevice3 = (InputDevice) new XInputPad(0);
      Input._devices.Add(inputDevice3);
      InputDevice inputDevice4 = (InputDevice) new XInputPad(1);
      Input._devices.Add(inputDevice4);
      InputDevice inputDevice5 = (InputDevice) new XInputPad(2);
      Input._devices.Add(inputDevice5);
      InputDevice inputDevice6 = (InputDevice) new XInputPad(3);
      Input._devices.Add(inputDevice6);
      try
      {
        if (DInput.Initialize())
        {
          InputDevice inputDevice7 = (InputDevice) new DInputPad(0);
          Input._devices.Add(inputDevice7);
          InputDevice inputDevice8 = (InputDevice) new DInputPad(1);
          Input._devices.Add(inputDevice8);
          InputDevice inputDevice9 = (InputDevice) new DInputPad(2);
          Input._devices.Add(inputDevice9);
          InputDevice inputDevice10 = (InputDevice) new DInputPad(3);
          Input._devices.Add(inputDevice10);
          Input._dinputEnabled = true;
        }
        else
          Input._dinputEnabled = false;
      }
      catch
      {
        Input._dinputEnabled = false;
      }
      GenericController genericController1 = new GenericController(0);
      Input._devices.Add((InputDevice) genericController1);
      Input._gamePads.Add(genericController1);
      GenericController genericController2 = new GenericController(1);
      Input._devices.Add((InputDevice) genericController2);
      Input._gamePads.Add(genericController2);
      GenericController genericController3 = new GenericController(2);
      Input._devices.Add((InputDevice) genericController3);
      Input._gamePads.Add(genericController3);
      GenericController genericController4 = new GenericController(3);
      Input._devices.Add((InputDevice) genericController4);
      Input._gamePads.Add(genericController4);
      InputProfile.Default = new InputProfile("Default");
      InputProfile.Default.Map(device, "LEFT", 37);
      InputProfile.Default.Map(device, "RIGHT", 39);
      InputProfile.Default.Map(device, "UP", 38);
      InputProfile.Default.Map(device, "DOWN", 40);
      InputProfile.Default.Map((InputDevice) Input.GetDevice<XInputPad>(), "LEFT", 4);
      InputProfile.Default.Map((InputDevice) Input.GetDevice<XInputPad>(), "RIGHT", 8);
      InputProfile.Default.Map((InputDevice) Input.GetDevice<XInputPad>(), "UP", 1);
      InputProfile.Default.Map((InputDevice) Input.GetDevice<XInputPad>(), "DOWN", 2);
      Input._profiles[InputProfile.Default.name] = InputProfile.Default;
      Input.InitDefaultProfiles();
      string str = DuckFile.optionsDirectory + "/input.dat";
      if (MonoMain.defaultControls)
      {
        DuckFile.Delete(str);
      }
      else
      {
        XDocument xdocument = DuckFile.LoadXDocument(str);
        if (xdocument == null)
          return;
        IEnumerable<XElement> source = xdocument.Elements((XName) "Mappings");
        if (source == null)
          return;
        foreach (XElement element in source.Elements<XElement>())
        {
          if (element.Name.LocalName == "InputMapping")
          {
            DeviceInputMapping mapping = new DeviceInputMapping();
            mapping.Deserialize(element);
            Input.SetDefaultMapping(mapping);
          }
        }
      }
    }

    public static T GetDevice<T>(int index = 0)
    {
      foreach (InputDevice device in Input._devices)
      {
        if (typeof (T) == device.GetType() && device.index == index)
          return (T)(Object)device;
      }
      return default (T);
    }

    public static InputDevice GetDevice(string name)
    {
      foreach (InputDevice device in Input._devices)
      {
        if (device.name == name)
          return device;
      }
      return (InputDevice) null;
    }

    private static void GamepadEnumThread()
    {
      while (Input._gamepadThread != null)
      {
        Thread.Sleep(100);
        lock (Input._gamepadThreadLock)
        {
          foreach (GenericController gamePad in Input._gamePads)
          {
            if (gamePad.device != null && !gamePad.isConnected)
            {
              if (gamePad.device is XInputPad)
              {
                Input._changePluggedIn = false;
                Input._changeName = gamePad.device.productName;
                Input._padConnectionChange = true;
              }
              gamePad.device = (AnalogGamePad) null;
              Input._gamepadsChanged = true;
            }
            if (gamePad.device == null)
            {
              foreach (InputDevice device in Input._devices)
              {
                if (!(device is GenericController) && device.isConnected && device.genericController == null)
                {
                  if (device is XInputPad)
                  {
                    gamePad.device = device as AnalogGamePad;
                    Input._gamepadsChanged = true;
                    Input._changePluggedIn = true;
                    Input._changeName = gamePad.device.productName;
                    Input._padConnectionChange = true;
                    break;
                  }
                  if (device is DInputPad)
                  {
                    gamePad.device = device as AnalogGamePad;
                    Input._gamepadsChanged = true;
                    Input._changePluggedIn = true;
                    Input._changeName = gamePad.device.productName;
                    break;
                  }
                }
              }
            }
          }
          if (Input._reEnumerate)
          {
            lock (Input._gamepadEnumLock)
            {
              List<string> stringList = new List<string>();
              foreach (InputDevice device in Input._devices)
              {
                if (device is DInputPad && device.isConnected)
                  stringList.Add(device.productName);
              }
              Input._updateWaitFrames = 4;
              Input._reEnumerate = false;
              DInput.EnumGamepads();
              DInput.Update();
              int index = 0;
              foreach (InputDevice device in Input._devices)
              {
                if (device is DInputPad && device.isConnected)
                {
                  device.productName = DInput.GetProductName(index);
                  device.productGUID = DInput.GetProductGUID(index);
                  ++index;
                  if (!stringList.Contains(device.productName))
                  {
                    Input._changePluggedIn = true;
                    Input._changeName = device.productName;
                    Input._padConnectionChange = true;
                  }
                  else
                    stringList.Remove(device.productName);
                }
              }
              if (stringList.Count > 0)
              {
                Input._changePluggedIn = false;
                Input._changeName = stringList[0];
                Input._padConnectionChange = true;
              }
              Input._gamepadsChanged = true;
            }
          }
        }
      }
    }

    public static void Update()
    {
      if (Input._gamepadThread == null)
      {
        Input._gamepadThread = new Thread(new ThreadStart(Input.GamepadEnumThread));
        Input._gamepadThread.CurrentCulture = CultureInfo.InvariantCulture;
        Input._gamepadThread.Priority = ThreadPriority.Lowest;
        Input._gamepadThread.IsBackground = true;
        Input._gamepadThread.Start();
      }
      if (Input.devicesChanged)
      {
        ++Input._deviceUpdateWait;
        if (Input._deviceUpdateWait > 30)
        {
          Input._deviceUpdateWait = 0;
          Input.devicesChanged = false;
          Input._reEnumerate = true;
        }
      }
      if (Input._gamepadsChanged)
      {
        lock (Input._gamepadThreadLock)
        {
          Input.ApplyDefaultMappings();
          Input._gamepadsChanged = false;
          Input.uiDevicesHaveChanged = true;
        }
      }
      lock (Input._gamepadEnumLock)
      {
        if (Input._updateWaitFrames > 0)
        {
          --Input._updateWaitFrames;
        }
        else
        {
          if (Input._padConnectionChange)
          {
            Input._padConnectionChange = false;
            if (MonoMain.started)
            {
              Input._changeName = Input._changeName.Trim();
              if (Input._changeName.Length > 25)
                Input._changeName = Input._changeName.Substring(0, 25) + "...";
              string str = "@PLUG@|LIME|";
              if (!Input._changePluggedIn)
                str = "@UNPLUG@|RED|";
              HUD.AddInputChangeDisplay(str + Input._changeName);
            }
          }
          if (Input._dinputEnabled)
            DInput.Update();
          bool flag = false;
          foreach (InputDevice device in Input._devices)
            device.Update();
          if (!flag)
            return;
          foreach (InputDevice device in Input._devices)
          {
            if (device is XInputPad && device.MapPressed(16))
              (device as XInputPad).StartPressed();
          }
        }
      }
    }

    public static void Terminate()
    {
      if (Input._gamepadThread != null)
        Input._gamepadThread.Abort();
      Input._gamepadThread = (Thread) null;
    }

    public static bool Pressed(string trigger, string profile = "Any")
    {
      if (profile == "Any")
      {
        foreach (KeyValuePair<string, InputProfile> profile1 in InputProfile.profiles)
        {
          if (profile1.Value.virtualDevice == null && profile1.Value.Pressed(trigger))
            return true;
        }
        return false;
      }
      InputProfile inputProfile;
      return Input._profiles.TryGetValue(profile, out inputProfile) && inputProfile.Pressed(trigger);
    }

    public static bool Released(string trigger, string profile = "Any")
    {
      if (profile == "Any")
      {
        foreach (KeyValuePair<string, InputProfile> profile1 in InputProfile.profiles)
        {
          if (profile1.Value.Released(trigger))
            return true;
        }
        return false;
      }
      InputProfile inputProfile;
      return Input._profiles.TryGetValue(profile, out inputProfile) && inputProfile.Released(trigger);
    }

    public static bool Down(string trigger, string profile = "Any")
    {
      if (profile == "Any")
      {
        foreach (KeyValuePair<string, InputProfile> profile1 in InputProfile.profiles)
        {
          if (profile1.Value.Down(trigger))
            return true;
        }
        return false;
      }
      InputProfile inputProfile;
      return Input._profiles.TryGetValue(profile, out inputProfile) && inputProfile.Down(trigger);
    }
  }
}
