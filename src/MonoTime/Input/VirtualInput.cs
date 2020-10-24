// Decompiled with JetBrains decompiler
// Type: DuckGame.VirtualInput
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class VirtualInput : InputDevice
  {
    public int pdraw;
    private ushort _state;
    private ushort _prevState;
    private List<string> _availableTriggers = new List<string>();
    public bool setThisFrame;

    public ushort state
    {
      get => this._state;
      set => this._state = value;
    }

    public ushort prevState
    {
      get => this._prevState;
      set => this._prevState = value;
    }

    public List<string> availableTriggers
    {
      get => this._availableTriggers;
      set => this._availableTriggers = value;
    }

    public VirtualInput(int idx)
      : base(idx)
      => this._name = "virtual" + (object) idx;

    public override void Update()
    {
    }

    private bool GetState(int mapping, bool prev = false) => ((prev ? (int) this._prevState : (int) this._state) & 1 << this._availableTriggers.Count - mapping) != 0;

    public void SetState(ushort val, bool flagPrev = true)
    {
      if (flagPrev)
        this._prevState = this._state;
      this._state = val;
      this.setThisFrame = true;
    }

    public override bool MapPressed(int mapping, bool any = false)
    {
      if (any)
      {
        for (int mapping1 = 0; mapping1 < this._availableTriggers.Count; ++mapping1)
        {
          if (this.MapPressed(mapping1, false))
            return true;
        }
        return false;
      }
      return this.GetState(mapping) && !this.GetState(mapping, true);
    }

    public override bool MapReleased(int mapping) => !this.GetState(mapping) && this.GetState(mapping, true);

    public override bool MapDown(int mapping, bool any = false)
    {
      if (any)
        return this._state != (ushort) 0;
      return this.GetState(mapping);
    }
  }
}
