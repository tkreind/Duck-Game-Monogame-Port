// Decompiled with JetBrains decompiler
// Type: DuckGame.DInputPad
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class DInputPad : AnalogGamePad
  {
    private Dictionary<int, string> _triggerNames = new Dictionary<int, string>()
    {
      {
        4096,
        "2"
      },
      {
        8192,
        "3"
      },
      {
        16384,
        "1"
      },
      {
        32768,
        "4"
      },
      {
        16,
        "START"
      },
      {
        32,
        "BACK"
      },
      {
        4,
        "LEFT"
      },
      {
        8,
        "RIGHT"
      },
      {
        1,
        "UP"
      },
      {
        2,
        "DOWN"
      },
      {
        2097152,
        "L{"
      },
      {
        1073741824,
        "L/"
      },
      {
        268435456,
        "L}"
      },
      {
        536870912,
        "L~"
      },
      {
        134217728,
        "R{"
      },
      {
        67108864,
        "R/"
      },
      {
        16777216,
        "R}"
      },
      {
        33554432,
        "R~"
      },
      {
        256,
        "LB"
      },
      {
        512,
        "RB"
      },
      {
        8388608,
        "LT"
      },
      {
        4194304,
        "RT"
      },
      {
        64,
        "LS"
      },
      {
        128,
        "RS"
      }
    };
    private Dictionary<int, Sprite> _triggerImages = new Dictionary<int, Sprite>()
    {
      {
        4096,
        (Sprite) new ButtonImage('\x0002')
      },
      {
        8192,
        (Sprite) new ButtonImage('\x0003')
      },
      {
        16384,
        (Sprite) new ButtonImage('\x0001')
      },
      {
        32768,
        (Sprite) new ButtonImage('\x0004')
      },
      {
        16,
        (Sprite) new ButtonImage('\n')
      },
      {
        32,
        (Sprite) new ButtonImage('\t')
      },
      {
        4,
        new Sprite("buttons/xbox/dPadLeft")
      },
      {
        8,
        new Sprite("buttons/xbox/dPadRight")
      },
      {
        1,
        new Sprite("buttons/xbox/dPadUp")
      },
      {
        2,
        new Sprite("buttons/xbox/dPadDown")
      },
      {
        256,
        (Sprite) new ButtonImage('\x0005')
      },
      {
        512,
        (Sprite) new ButtonImage('\x0006')
      },
      {
        8388608,
        (Sprite) new ButtonImage('\a')
      },
      {
        4194304,
        (Sprite) new ButtonImage('\b')
      },
      {
        64,
        (Sprite) new ButtonImage('\f')
      },
      {
        128,
        (Sprite) new ButtonImage('\r')
      },
      {
        9999,
        new Sprite("buttons/xbox/dPad")
      }
    };

    public override bool isConnected => DInput.GetState(this.index) != null;

    public DInputPad(int idx)
      : base(idx)
    {
      this._name = "dinput" + (object) idx;
      this._productName = DInput.GetProductName(this.index);
      this._productGUID = DInput.GetProductGUID(this.index);
    }

    public override Dictionary<int, string> GetTriggerNames() => this._triggerNames;

    public override Sprite GetMapImage(int map)
    {
      Sprite sprite = (Sprite) null;
      this._triggerImages.TryGetValue(map, out sprite);
      return sprite;
    }

    private PadState ConvertDInputState(DInputState state)
    {
      PadState padState = new PadState();
      if (state == null)
        return padState;
      if (state.buttons[0])
        padState.buttons |= PadButton.X;
      if (state.buttons[3])
        padState.buttons |= PadButton.Y;
      if (state.buttons[1])
        padState.buttons |= PadButton.A;
      if (state.buttons[2])
        padState.buttons |= PadButton.B;
      if (state.buttons[4])
        padState.buttons |= PadButton.LeftShoulder;
      if (state.buttons[5])
        padState.buttons |= PadButton.RightShoulder;
      if (state.buttons[6])
      {
        padState.buttons |= PadButton.LeftTrigger;
        padState.triggers.left = 1f;
      }
      if (state.buttons[7])
      {
        padState.buttons |= PadButton.RightTrigger;
        padState.triggers.right = 1f;
      }
      if (state.buttons[8])
        padState.buttons |= PadButton.Back;
      if (state.buttons[9])
        padState.buttons |= PadButton.Start;
      if (state.buttons[11])
        padState.buttons |= PadButton.LeftStick;
      if (state.buttons[12])
        padState.buttons |= PadButton.RightStick;
      if (state.left)
        padState.buttons |= PadButton.DPadLeft;
      if (state.right)
        padState.buttons |= PadButton.DPadRight;
      if (state.up)
        padState.buttons |= PadButton.DPadUp;
      if (state.down)
        padState.buttons |= PadButton.DPadDown;
      padState.sticks.left = new Vec2(state.leftX, state.leftY * -1f);
      padState.sticks.right = new Vec2(state.leftZ, -state.rightX);
      if ((double) padState.sticks.left.Length() < 0.100000001490116)
        padState.sticks.left = Vec2.Zero;
      if ((double) padState.sticks.right.Length() < 0.100000001490116)
        padState.sticks.right = Vec2.Zero;
      return padState;
    }

    protected override PadState GetState(int index) => this.ConvertDInputState(DInput.GetState(index));
  }
}
