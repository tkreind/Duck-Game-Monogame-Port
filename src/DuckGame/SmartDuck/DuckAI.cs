// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckAI
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class DuckAI : InputProfile
  {
    private Stack<AIState> _state = new Stack<AIState>();
    private Dictionary<string, InputState> _inputState = new Dictionary<string, InputState>();
    private AILocomotion _locomotion = new AILocomotion();
    public bool canRefresh;
    public InputProfile _manualQuack;

    public AILocomotion locomotion => this._locomotion;

    public void Press(string trigger) => this._inputState[trigger] = InputState.Pressed;

    public void HoldDown(string trigger) => this._inputState[trigger] = InputState.Down;

    public void Release(string trigger) => this._inputState[trigger] = InputState.Released;

    public override bool Pressed(string trigger, bool any = false)
    {
      InputState inputState;
      return this._inputState.TryGetValue(trigger, out inputState) && inputState == InputState.Pressed;
    }

    public override bool Released(string trigger)
    {
      InputState inputState;
      return this._inputState.TryGetValue(trigger, out inputState) && inputState == InputState.Released;
    }

    public override bool Down(string trigger)
    {
      InputState inputState;
      if (!this._inputState.TryGetValue(trigger, out inputState))
        return false;
      return inputState == InputState.Pressed || inputState == InputState.Down;
    }

    public bool SetTarget(Vec2 t)
    {
      this._locomotion.target = t;
      return this._locomotion.target == Vec2.Zero;
    }

    public void TrimLastTarget() => this._locomotion.TrimLastTarget();

    public DuckAI(InputProfile manualQuacker = null)
      : base()
    {
      this._state.Push((AIState) new AIStateDeathmatchBot());
      this._manualQuack = manualQuacker;
    }

    public virtual void Update(Duck duck)
    {
      this.Release("GRAB");
      this.Release("SHOOT");
      this._locomotion.Update(this, duck);
    }

    public override void UpdateExtraInput()
    {
      if (this._inputState.ContainsKey("QUACK") && this._inputState["QUACK"] == InputState.Pressed)
        this._inputState["QUACK"] = InputState.Down;
      if (this._inputState.ContainsKey("STRAFE") && this._inputState["STRAFE"] == InputState.Pressed)
        this._inputState["STRAFE"] = InputState.Down;
      if (this._manualQuack == null)
        return;
      if (this._manualQuack.Pressed("QUACK"))
        this.Press("QUACK");
      else if (this._manualQuack.Released("QUACK"))
        this.Release("QUACK");
      if (this._manualQuack.Pressed("STRAFE"))
      {
        this.Press("STRAFE");
      }
      else
      {
        if (!this._manualQuack.Released("STRAFE"))
          return;
        this.Release("STRAFE");
      }
    }

    public override float leftTrigger => this._manualQuack != null ? this._manualQuack.leftTrigger : 0.0f;

    public void Draw()
    {
      if (this._locomotion.pathFinder.path == null)
        return;
      Vec2 p1 = Vec2.Zero;
      foreach (PathNodeLink pathNodeLink in this._locomotion.pathFinder.path)
      {
        if (p1 != Vec2.Zero)
          Graphics.DrawLine(p1, pathNodeLink.owner.position, new Color((int) byte.MaxValue, 0, (int) byte.MaxValue), 2f, new Depth(0.9f));
        p1 = pathNodeLink.owner.position;
      }
    }
  }
}
