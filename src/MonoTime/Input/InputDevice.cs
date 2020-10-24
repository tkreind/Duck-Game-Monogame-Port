// Decompiled with JetBrains decompiler
// Type: DuckGame.InputDevice
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class InputDevice
  {
    protected string _name;
    protected int _index;
    protected string _productName;
    protected string _productGUID;
    private volatile GenericController _genericController;
    public DeviceInputMapping overrideMap;

    public string name
    {
      get => this._name;
      set => this._name = value;
    }

    public int index => this._index;

    public virtual string productName
    {
      get => this._productName;
      set => this._productName = value;
    }

    public virtual string productGUID
    {
      get => this._productGUID;
      set => this._productGUID = value;
    }

    public GenericController genericController
    {
      get => this._genericController;
      set => this._genericController = value;
    }

    public virtual bool isConnected => true;

    public InputDevice(int idx = 0) => this._index = idx;

    public virtual Dictionary<int, string> GetTriggerNames() => (Dictionary<int, string>) null;

    public virtual Sprite DoGetMapImage(int map, bool skipStyleCheck = false)
    {
      if (skipStyleCheck)
        return this.GetMapImage(map);
      DeviceInputMapping deviceInputMapping = this.overrideMap;
      if (this.overrideMap == null)
        deviceInputMapping = Input.GetDefaultMapping(this.productName, this.productGUID, makeClone: false);
      return deviceInputMapping.GetSprite(map) ?? this.GetMapImage(map);
    }

    public virtual Sprite GetMapImage(int map) => (Sprite) null;

    public virtual void Update()
    {
    }

    public virtual bool MapPressed(int mapping, bool any = false) => false;

    public virtual bool MapReleased(int mapping) => false;

    public virtual bool MapDown(int mapping, bool any = false) => false;
  }
}
