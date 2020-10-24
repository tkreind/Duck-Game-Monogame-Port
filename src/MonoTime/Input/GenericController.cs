// Decompiled with JetBrains decompiler
// Type: DuckGame.GenericController
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class GenericController : InputDevice
  {
    private volatile AnalogGamePad _device;

    public AnalogGamePad device
    {
      get => this._device;
      set
      {
        if (this._device != null)
          this._device.genericController = (GenericController) null;
        this._device = value;
        if (this._device == null)
          return;
        this._device.genericController = this;
      }
    }

    public override Dictionary<int, string> GetTriggerNames() => this._device != null ? this._device.GetTriggerNames() : (Dictionary<int, string>) null;

    public override Sprite GetMapImage(int map) => this._device != null ? this._device.GetMapImage(map) : (Sprite) null;

    public override string productName
    {
      get => this._device == null ? this._productName : this._device.productName;
      set => this._productName = value;
    }

    public override string productGUID
    {
      get => this._device == null ? this._productGUID : this._device.productGUID;
      set => this._productName = value;
    }

    public override bool isConnected => this._device == null || this._device.isConnected;

    public float leftTrigger => this._device == null ? 0.0f : this._device.leftTrigger;

    public float rightTrigger => this._device == null ? 0.0f : this._device.rightTrigger;

    public Vec2 leftStick => this._device == null ? Vec2.Zero : this._device.leftStick;

    public Vec2 rightStick => this._device == null ? Vec2.Zero : this._device.rightStick;

    public GenericController(int index)
      : base(index)
    {
    }

    public override bool MapPressed(int mapping, bool any = false) => this._device != null && this._device.MapPressed(mapping, any);

    public override bool MapReleased(int mapping) => this._device != null && this._device.MapReleased(mapping);

    public override bool MapDown(int mapping, bool any = false) => this._device != null && this._device.MapDown(mapping, any);
  }
}
