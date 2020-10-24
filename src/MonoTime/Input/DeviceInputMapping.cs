// Decompiled with JetBrains decompiler
// Type: DuckGame.DeviceInputMapping
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class DeviceInputMapping : DataClass
  {
    public string deviceName;
    public string deviceGUID;
    public Dictionary<string, int> map = new Dictionary<string, int>();
    public Dictionary<int, string> graphicMap = new Dictionary<int, string>();
    private Dictionary<string, Sprite> _spriteMap = new Dictionary<string, Sprite>();
    public InputDevice deviceOverride;

    public InputDevice device
    {
      get
      {
        if (this.deviceOverride != null)
          return this.deviceOverride;
        if (this.deviceName == "XBOX GAMEPAD")
          return (InputDevice) Input.GetDevice<XInputPad>();
        foreach (InputDevice inputDevice in Input.GetInputDevices())
        {
          if (inputDevice.productName == this.deviceName && inputDevice.productGUID == this.deviceGUID)
            return inputDevice;
        }
        return new InputDevice();
      }
    }

    public List<InputDevice> devices => this.deviceName == "XBOX GAMEPAD" ? Input.GetInputDevices().Where<InputDevice>((Func<InputDevice, bool>) (x => x is XInputPad)).ToList<InputDevice>() : Input.GetInputDevices().Where<InputDevice>((Func<InputDevice, bool>) (x => x.productName == this.deviceName && x.productGUID == this.deviceGUID)).ToList<InputDevice>();

    public Sprite GetSprite(int mapping)
    {
      string str = (string) null;
      if (!this.graphicMap.TryGetValue(mapping, out str))
        return (Sprite) null;
      Sprite sprite1 = (Sprite) null;
      if (this._spriteMap.TryGetValue(str, out sprite1))
        return sprite1;
      Sprite sprite2 = new Sprite(str);
      this._spriteMap[str] = sprite2;
      return sprite2;
    }

    public DeviceInputMapping() => this._nodeName = "InputMapping";

    public string GetMappingString(string trigger)
    {
      int key = 0;
      if (!this.map.TryGetValue(trigger, out key))
        return "";
      Dictionary<int, string> triggerNames = this.device.GetTriggerNames();
      string str = "???";
      triggerNames?.TryGetValue(key, out str);
      return str;
    }

    public bool IsEqual(DeviceInputMapping compare)
    {
      if (this.map.Count != compare.map.Count)
        return false;
      foreach (KeyValuePair<string, int> keyValuePair in this.map)
      {
        int num1 = -1;
        int num2 = -1;
        this.map.TryGetValue(keyValuePair.Key, out num1);
        compare.map.TryGetValue(keyValuePair.Key, out num2);
        if (num1 != num2)
          return false;
      }
      if (this.graphicMap.Count != compare.graphicMap.Count)
        return false;
      foreach (KeyValuePair<int, string> graphic in this.graphicMap)
      {
        string str1 = "";
        string str2 = "";
        this.graphicMap.TryGetValue(graphic.Key, out str1);
        compare.graphicMap.TryGetValue(graphic.Key, out str2);
        if (str1 != str2)
          return false;
      }
      return true;
    }

    public DeviceInputMapping Clone()
    {
      DeviceInputMapping deviceInputMapping = new DeviceInputMapping();
      deviceInputMapping.deviceName = this.deviceName;
      deviceInputMapping.deviceGUID = this.deviceGUID;
      foreach (KeyValuePair<string, int> keyValuePair in this.map)
        deviceInputMapping.map[keyValuePair.Key] = keyValuePair.Value;
      foreach (KeyValuePair<int, string> graphic in this.graphicMap)
        deviceInputMapping.graphicMap[graphic.Key] = graphic.Value;
      return deviceInputMapping;
    }

    public bool RunMappingUpdate(string trigger)
    {
      bool flag = false;
      if (this.device is AnalogGamePad)
      {
        AnalogGamePad device = this.device as AnalogGamePad;
        if (trigger == "LSTICK" || trigger == "RSTICK")
        {
          if ((double) device.leftStick.length > 0.100000001490116)
          {
            this.map[trigger] = 64;
            flag = true;
          }
          else if ((double) device.rightStick.length > 0.100000001490116)
          {
            this.map[trigger] = 128;
            flag = true;
          }
        }
        else if (trigger == "LTRIGGER" || trigger == "RTRIGGER")
        {
          if ((double) device.leftTrigger > 0.100000001490116)
          {
            this.map[trigger] = 8388608;
            flag = true;
          }
          else if ((double) device.rightTrigger > 0.100000001490116)
          {
            this.map[trigger] = 4194304;
            flag = true;
          }
        }
        else
        {
          foreach (PadButton padButton in Enum.GetValues(typeof (PadButton)).Cast<PadButton>())
          {
            switch (padButton)
            {
              case PadButton.LeftThumbstickLeft:
              case PadButton.LeftThumbstickUp:
              case PadButton.LeftThumbstickDown:
              case PadButton.LeftThumbstickRight:
                continue;
              default:
                if (this.device.MapPressed((int) padButton))
                {
                  this.map[trigger] = (int) padButton;
                  flag = true;
                  continue;
                }
                continue;
            }
          }
        }
      }
      else if (this.device is Keyboard)
      {
        foreach (Keys keys in Enum.GetValues(typeof (Keys)).Cast<Keys>())
        {
          if (this.device.MapPressed((int) keys))
          {
            this.map[trigger] = (int) keys;
            flag = true;
          }
        }
      }
      return flag;
    }
  }
}
