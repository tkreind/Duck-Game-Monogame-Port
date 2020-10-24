// Decompiled with JetBrains decompiler
// Type: DuckGame.AnalogGamePad
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class AnalogGamePad : InputDevice
  {
    private const int numStates = 256;
    protected PadState _repeatState;
    protected PadState _state;
    protected PadState _statePrev;
    protected Queue<PadState> _delayBuffer = new Queue<PadState>();
    private int _curState;
    private int _numState;
    private int _realState;
    private PadState[] _states = new PadState[256];
    private static bool _repeat = false;
    private float _repeatTime;
    private bool _repeating;
    private bool _startPressed;
    public bool delay;
    public int delayIndex;
    private bool _ahead;
    private bool _behind;
    private List<PadButton> _repeatList = new List<PadButton>();
    protected Dictionary<PadButton, InputState> _leftStickStates = new Dictionary<PadButton, InputState>();
    protected static Array _xboxButtons = Enum.GetValues(typeof (PadButton));
    public static bool inputDelay = true;

    public static bool repeat
    {
      get => AnalogGamePad._repeat;
      set => AnalogGamePad._repeat = value;
    }

    public bool startWasPressed { get; set; }

    public virtual float leftTrigger => Maths.NormalizeSection(this._state.triggers.left, 0.1f, 1f);

    public virtual float rightTrigger => Maths.NormalizeSection(this._state.triggers.right, 0.1f, 1f);

    public virtual Vec2 leftStick => new Vec2(this._state.sticks.left.x, this._state.sticks.left.y);

    public virtual Vec2 rightStick => new Vec2(this._state.sticks.right.x, this._state.sticks.right.y);

    public AnalogGamePad(int idx)
      : base(idx)
    {
      this._leftStickStates[PadButton.DPadLeft] = InputState.None;
      this._leftStickStates[PadButton.DPadRight] = InputState.None;
      this._leftStickStates[PadButton.DPadUp] = InputState.None;
      this._leftStickStates[PadButton.DPadDown] = InputState.None;
      this.delayIndex = idx;
    }

    protected virtual PadState GetState(int index) => new PadState();

    public override void Update()
    {
      base.Update();
      if (this.delay)
      {
        this._states[this._curState] = this.GetState(this.delayIndex);
        this._curState = (this._curState + 1) % 256;
        ++this._numState;
        if (this._numState == 15)
          this._realState = this._curState - 15;
        if (this._numState < 15)
          return;
        this._statePrev = this._state;
        this._state = this._states[this._realState];
        this._realState = (this._realState + 1) % 256;
        if ((double) Rando.Float(1f) <= 0.5 || this._behind)
          return;
        if (!this._ahead)
        {
          this._realState = (this._realState + 1) % 256;
          this._ahead = true;
        }
        else
        {
          this._realState = (this._realState - 1) % 256;
          if (this._realState < 0)
            this._realState += 256;
          this._ahead = false;
        }
      }
      else
      {
        this.startWasPressed = this._startPressed;
        this._startPressed = false;
        this._repeatList.Clear();
        this._statePrev = this._state;
        this._state = this.GetState(this.index);
        if (AnalogGamePad._repeat)
        {
          if ((this.MapPressed(4, false) || this.MapPressed(8, false) || (this.MapPressed(1, false) || this.MapPressed(2, false))) && !this._repeating)
          {
            this._repeatTime = 2f;
            this._repeating = true;
          }
          if ((double) this._repeatTime > 0.0)
          {
            this._repeatTime -= 0.1f;
            bool flag = false;
            if (this.MapDown(4, false))
            {
              if ((double) this._repeatTime <= 0.0)
                this._repeatList.Add(PadButton.DPadLeft);
              flag = true;
            }
            if (this.MapDown(8, false))
            {
              if ((double) this._repeatTime <= 0.0)
                this._repeatList.Add(PadButton.DPadRight);
              flag = true;
            }
            if (this.MapDown(1, false))
            {
              if ((double) this._repeatTime <= 0.0)
                this._repeatList.Add(PadButton.DPadUp);
              flag = true;
            }
            if (this.MapDown(2, false))
            {
              if ((double) this._repeatTime <= 0.0)
                this._repeatList.Add(PadButton.DPadDown);
              flag = true;
            }
            if ((double) this._repeatTime <= 0.0 && flag)
              this._repeatTime = 0.3f;
            if (!flag)
            {
              this._repeating = false;
              this._repeatTime = 0.0f;
            }
          }
          else
            this._repeatTime = 0.0f;
        }
        this.StickPress(PadButton.DPadLeft, (double) this.leftStick.x < -0.600000023841858);
        this.StickPress(PadButton.DPadRight, (double) this.leftStick.x > 0.600000023841858);
        this.StickPress(PadButton.DPadUp, (double) this.leftStick.y > 0.600000023841858);
        this.StickPress(PadButton.DPadDown, (double) this.leftStick.y < -0.600000023841858);
      }
    }

    public void StartPressed()
    {
      this._state = this._statePrev;
      this._startPressed = true;
    }

    private void StickPress(PadButton b, bool press)
    {
      if (press)
      {
        if (this._leftStickStates[b] == InputState.None)
          this._leftStickStates[b] = InputState.Pressed;
        else
          this._leftStickStates[b] = InputState.Down;
      }
      else if (this._leftStickStates[b] == InputState.Down || this._leftStickStates[PadButton.DPadLeft] == InputState.Pressed)
        this._leftStickStates[b] = InputState.Released;
      else
        this._leftStickStates[b] = InputState.None;
    }

    public override bool MapPressed(int mapping, bool any = false)
    {
      PadButton padButton = (PadButton) mapping;
      if (this._leftStickStates.ContainsKey(padButton) && this._leftStickStates[padButton] == InputState.Pressed || padButton == PadButton.Start && this._startPressed)
        return true;
      if (any)
      {
        foreach (PadButton xboxButton in AnalogGamePad._xboxButtons)
        {
          if (this._state.IsButtonDown(xboxButton) && !this._statePrev.IsButtonDown(xboxButton))
            return true;
        }
        return false;
      }
      return this._state.IsButtonDown(padButton) && !this._statePrev.IsButtonDown(padButton) || this._repeatList.Contains(padButton);
    }

    public override bool MapReleased(int mapping)
    {
      PadButton padButton = (PadButton) mapping;
      return this._leftStickStates.ContainsKey(padButton) && this._leftStickStates[padButton] == InputState.Released || !this._state.IsButtonDown(padButton) && this._statePrev.IsButtonDown(padButton);
    }

    public override bool MapDown(int mapping, bool any = false)
    {
      if (any)
      {
        foreach (PadButton xboxButton in AnalogGamePad._xboxButtons)
        {
          if (this._state.IsButtonDown(xboxButton))
            return true;
        }
        return false;
      }
      PadButton padButton = (PadButton) mapping;
      return this._leftStickStates.ContainsKey(padButton) && (this._leftStickStates[padButton] == InputState.Down || this._leftStickStates[padButton] == InputState.Pressed) || this._state.IsButtonDown(padButton);
    }
  }
}
