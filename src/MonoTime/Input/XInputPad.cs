// Decompiled with JetBrains decompiler
// Type: DuckGame.XInputPad
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class XInputPad : AnalogGamePad
  {
    private Dictionary<int, string> _triggerNames = new Dictionary<int, string>()
    {
      {
        4096,
        "A"
      },
      {
        8192,
        "B"
      },
      {
        16384,
        "X"
      },
      {
        32768,
        "Y"
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
      },
      {
        9999,
        "DPAD"
      }
    };
    private Dictionary<int, Sprite> _triggerImages = new Dictionary<int, Sprite>()
    {
      {
        4096,
        new Sprite("buttons/xbox/oButton")
      },
      {
        8192,
        new Sprite("buttons/xbox/aButton")
      },
      {
        16384,
        new Sprite("buttons/xbox/uButton")
      },
      {
        32768,
        new Sprite("buttons/xbox/yButton")
      },
      {
        16,
        new Sprite("buttons/xbox/startButton")
      },
      {
        32,
        new Sprite("buttons/xbox/selectButton")
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
        new Sprite("buttons/xbox/leftBumper")
      },
      {
        512,
        new Sprite("buttons/xbox/rightBumper")
      },
      {
        8388608,
        new Sprite("buttons/xbox/leftTrigger")
      },
      {
        4194304,
        new Sprite("buttons/xbox/rightTrigger")
      },
      {
        64,
        new Sprite("buttons/xbox/leftStick")
      },
      {
        128,
        new Sprite("buttons/xbox/rightStick")
      },
      {
        9999,
        new Sprite("buttons/xbox/dPad")
      }
    };

    public override bool isConnected => GamePad.GetState((PlayerIndex) this.index, GamePadDeadZone.Circular).IsConnected;

    public XInputPad(int idx)
      : base(idx)
    {
      this._name = "xbox" + (object) idx;
      this._productName = "XBOX GAMEPAD";
      this._productGUID = "";
    }

    public override Dictionary<int, string> GetTriggerNames() => this._triggerNames;

    public override Sprite GetMapImage(int map)
    {
      Sprite sprite = (Sprite) null;
      this._triggerImages.TryGetValue(map, out sprite);
      return sprite;
    }

    protected override PadState GetState(int index)
    {
      GamePadState state = GamePad.GetState((PlayerIndex) index, GamePadDeadZone.Circular);
      PadState padState = new PadState();
      foreach (object obj in Enum.GetValues(typeof (PadButton)))
      {
        if (state.IsButtonDown((Buttons) obj))
          padState.buttons |= (PadButton) obj;
      }
      padState.sticks.left = (Vec2) state.ThumbSticks.Left;
      padState.sticks.right = (Vec2) state.ThumbSticks.Right;
      padState.triggers.left = state.Triggers.Left;
      padState.triggers.right = state.Triggers.Right;
      return padState;
    }
  }
}
