// Decompiled with JetBrains decompiler
// Type: DuckGame.NMInputSettings
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  [FixedNetworkID(30232)]
  public class NMInputSettings : NMDuckNetworkEvent
  {
    private InputDevice _device;
    private InputProfile _inputProfile;
    private Profile _profile;
    private DeviceInputMapping _mapping;
    private byte _index;

    public NMInputSettings()
    {
    }

    public NMInputSettings(Profile pro)
    {
      this._device = pro.inputProfile.lastActiveDevice;
      if (this._device.productName == null && this._device.productGUID == null)
        this._device = (InputDevice) null;
      this._index = pro.networkIndex;
      this._profile = pro;
      this._inputProfile = pro.inputProfile;
    }

    protected override void OnSerialize()
    {
      this._serializedData.Write(this._index);
      if (this._device is Keyboard)
        this.serializedData.Write((byte) 0);
      else if (this._device is XInputPad)
        this.serializedData.Write((byte) 1);
      else if (this._device is GenericController)
      {
        if ((this._device as GenericController).device is XInputPad)
          this.serializedData.Write((byte) 1);
        else
          this.serializedData.Write((byte) 2);
      }
      else
        this.serializedData.Write((byte) 2);
      MultiMap<string, int> multiMap = (MultiMap<string, int>) null;
      if (this._device != null)
        multiMap = this._inputProfile.GetMappings(this._device.GetType());
      if (multiMap != null)
      {
        this.serializedData.Write(true);
        byte val1 = 0;
        foreach (KeyValuePair<string, List<int>> keyValuePair in (MultiMap<string, int, List<int>>) multiMap)
        {
          if (keyValuePair.Value.Count > 0)
            ++val1;
        }
        this.serializedData.Write(val1);
        foreach (KeyValuePair<string, List<int>> keyValuePair in (MultiMap<string, int, List<int>>) multiMap)
        {
          if (keyValuePair.Value.Count > 0)
          {
            this.serializedData.Write(Triggers.toIndex[keyValuePair.Key]);
            this.serializedData.Write(keyValuePair.Value[0]);
          }
        }
        DeviceInputMapping deviceInputMapping = this._device.overrideMap == null ? Input.GetDefaultMapping(this._device.productName, this._device.productGUID, p: this._profile) : this._device.overrideMap;
        if (deviceInputMapping.graphicMap.Count > 0)
        {
          this.serializedData.Write(true);
          this.serializedData.Write((byte) deviceInputMapping.graphicMap.Count);
          using (Dictionary<int, string>.Enumerator enumerator = deviceInputMapping.graphicMap.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, string> pair = enumerator.Current;
              Sprite sprite = Input.buttonStyles.FirstOrDefault<Sprite>((Func<Sprite, bool>) (x => x.texture != null && x.texture.textureName == pair.Value));
              byte val2 = 0;
              if (sprite != null)
                val2 = (byte) Input.buttonStyles.IndexOf(sprite);
              this.serializedData.Write(pair.Key);
              this.serializedData.Write(val2);
            }
          }
        }
        else
          this.serializedData.Write(false);
      }
      else
        this.serializedData.Write(false);
      base.OnSerialize();
    }

    public override void OnDeserialize(BitBuffer msg)
    {
      this._index = msg.ReadByte();
      byte num1 = msg.ReadByte();
      DeviceInputMapping deviceInputMapping = new DeviceInputMapping();
      switch (num1)
      {
        case 0:
          deviceInputMapping.deviceOverride = (InputDevice) new Keyboard("", 0);
          break;
        case 1:
          deviceInputMapping.deviceOverride = (InputDevice) new XInputPad(0);
          break;
        default:
          deviceInputMapping.deviceOverride = (InputDevice) new DInputPad(0);
          break;
      }
      deviceInputMapping.deviceOverride.overrideMap = deviceInputMapping;
      if (msg.ReadBool())
      {
        byte num2 = msg.ReadByte();
        for (int index = 0; index < (int) num2; ++index)
        {
          byte key = msg.ReadByte();
          int num3 = msg.ReadInt();
          deviceInputMapping.map[Triggers.fromIndex[key]] = num3;
        }
        if (msg.ReadBool())
        {
          byte num3 = msg.ReadByte();
          for (int index1 = 0; index1 < (int) num3; ++index1)
          {
            int key = msg.ReadInt();
            int index2 = (int) msg.ReadByte();
            deviceInputMapping.graphicMap[key] = Input.buttonStyles[index2].texture.textureName;
          }
        }
      }
      this._mapping = deviceInputMapping;
      base.OnDeserialize(msg);
    }

    public override void Activate()
    {
      if (this._index < (byte) 0 || this._index > (byte) 3)
        return;
      Profile profile = DuckNetwork.profiles[(int) this._index];
      profile.inputMappingOverrides.Clear();
      profile.inputMappingOverrides.Add(this._mapping);
      foreach (KeyValuePair<string, int> keyValuePair in this._mapping.map)
        profile.inputProfile.Map(this._mapping.deviceOverride, keyValuePair.Key, keyValuePair.Value, true);
      profile.inputProfile.lastActiveOverride = this._mapping.deviceOverride;
      base.Activate();
    }
  }
}
