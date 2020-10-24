// Decompiled with JetBrains decompiler
// Type: DuckGame.InputObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class InputObject : Thing, ITakeInput
  {
    public StateBinding _profileNumberBinding = new StateBinding(nameof (profileNumber));
    public StateBinding _votedBinding = new StateBinding(nameof (voted));
    public StateBinding _inputChangeIndexBinding = new StateBinding(nameof (_inputChangeIndex));
    public StateBinding _leftStickBinding = new StateBinding(true, nameof (leftStick));
    public StateBinding _rightStickBinding = new StateBinding(true, nameof (rightStick));
    public StateBinding _leftTriggerBinding = new StateBinding(nameof (leftTrigger));
    public byte _inputChangeIndex;
    private sbyte _profileNumber;
    private Vec2 _leftStick;
    private Vec2 _rightStick;
    private float _leftTrigger;
    private InputProfile _blankProfile = new InputProfile();
    public Profile duckProfile;
    public bool voted;
    private ushort prevState;

    public sbyte profileNumber
    {
      get => this._profileNumber;
      set
      {
        this._profileNumber = value;
        this.duckProfile = DuckNetwork.profiles[(int) this._profileNumber];
        if (this.duckProfile == null || this.duckProfile.connection != DuckNetwork.localConnection)
          return;
        this.connection = DuckNetwork.localConnection;
      }
    }

    public Vec2 leftStick
    {
      get => this.isServerForObject && this.inputProfile != null ? this.inputProfile.leftStick : this._leftStick;
      set => this._leftStick = value;
    }

    public Vec2 rightStick
    {
      get => this.isServerForObject && this.inputProfile != null ? this.inputProfile.rightStick : this._rightStick;
      set => this._rightStick = value;
    }

    public float leftTrigger
    {
      get => this.isServerForObject && this.inputProfile != null ? this.inputProfile.leftTrigger : this._leftTrigger;
      set => this._leftTrigger = value;
    }

    public InputProfile inputProfile => this.duckProfile != null ? this.duckProfile.inputProfile : this._blankProfile;

    public InputObject()
      : base()
    {
    }

    public override void Update()
    {
      if (this.duckProfile != null && this.duckProfile.connection == DuckNetwork.localConnection)
        Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      if (this.isServerForObject && this.inputProfile != null)
      {
        if (!Network.isServer)
          this.inputProfile.UpdateTriggerStates();
        if ((int) this.prevState != (int) this.inputProfile.state)
        {
          InputObject inputObject = this;
          inputObject.authority = ++(inputObject.authority);
          ++this._inputChangeIndex;
        }
        this.prevState = this.inputProfile.state;
      }
      RegisteredVote vote = Vote.GetVote(DuckNetwork.profiles[(int) this._profileNumber]);
      if (vote != null)
      {
        vote.leftStick = this.leftStick;
        vote.rightStick = this.rightStick;
      }
      if (Level.current is RockScoreboard)
      {
        foreach (Slot3D slot in (Level.current as RockScoreboard)._slots)
        {
          if (slot.duck != null && slot.duck.profile == this.duckProfile)
          {
            slot.ai._manualQuack = this.inputProfile;
            slot.duck.manualQuackPitch = true;
            slot.duck.quackPitch = (byte) ((double) this.leftTrigger * (double) byte.MaxValue);
          }
        }
      }
      base.Update();
    }
  }
}
