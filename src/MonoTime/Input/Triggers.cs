// Decompiled with JetBrains decompiler
// Type: DuckGame.Triggers
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class Triggers
  {
    public const string Left = "LEFT";
    public const string Right = "RIGHT";
    public const string Up = "UP";
    public const string Down = "DOWN";
    public const string Jump = "JUMP";
    public const string Quack = "QUACK";
    public const string Shoot = "SHOOT";
    public const string Grab = "GRAB";
    public const string Ragdoll = "RAGDOLL";
    public const string Strafe = "STRAFE";
    public const string Start = "START";
    public const string Select = "SELECT";
    public const string Chat = "CHAT";
    public const string LeftTrigger = "LTRIGGER";
    public const string RightTrigger = "RTRIGGER";
    public const string LeftStick = "LSTICK";
    public const string RightStick = "RSTICK";
    public const string Any = "ANY";
    public static Dictionary<byte, string> fromIndex = new Dictionary<byte, string>()
    {
      {
        (byte) 0,
        "LEFT"
      },
      {
        (byte) 1,
        "RIGHT"
      },
      {
        (byte) 2,
        "UP"
      },
      {
        (byte) 3,
        "DOWN"
      },
      {
        (byte) 4,
        "JUMP"
      },
      {
        (byte) 5,
        "QUACK"
      },
      {
        (byte) 6,
        "SHOOT"
      },
      {
        (byte) 7,
        "GRAB"
      },
      {
        (byte) 8,
        "RAGDOLL"
      },
      {
        (byte) 9,
        "STRAFE"
      },
      {
        (byte) 10,
        "START"
      },
      {
        (byte) 11,
        "SELECT"
      },
      {
        (byte) 12,
        "CHAT"
      },
      {
        (byte) 13,
        "LTRIGGER"
      },
      {
        (byte) 14,
        "RTRIGGER"
      },
      {
        (byte) 15,
        "LSTICK"
      },
      {
        (byte) 16,
        "RSTICK"
      },
      {
        (byte) 17,
        "ANY"
      }
    };
    public static Dictionary<string, byte> toIndex = new Dictionary<string, byte>()
    {
      {
        "LEFT",
        (byte) 0
      },
      {
        "RIGHT",
        (byte) 1
      },
      {
        "UP",
        (byte) 2
      },
      {
        "DOWN",
        (byte) 3
      },
      {
        "JUMP",
        (byte) 4
      },
      {
        "QUACK",
        (byte) 5
      },
      {
        "SHOOT",
        (byte) 6
      },
      {
        "GRAB",
        (byte) 7
      },
      {
        "RAGDOLL",
        (byte) 8
      },
      {
        "STRAFE",
        (byte) 9
      },
      {
        "START",
        (byte) 10
      },
      {
        "SELECT",
        (byte) 11
      },
      {
        "CHAT",
        (byte) 12
      },
      {
        "LTRIGGER",
        (byte) 13
      },
      {
        "RTRIGGER",
        (byte) 14
      },
      {
        "LSTICK",
        (byte) 15
      },
      {
        "RSTICK",
        (byte) 16
      },
      {
        "ANY",
        (byte) 17
      }
    };

    public static bool IsTrigger(string val) => val == "DPAD" || val == "LEFT" || (val == "RIGHT" || val == "UP") || (val == "DOWN" || val == "JUMP" || (val == "QUACK" || val == "SHOOT")) || (val == "GRAB" || val == "RAGDOLL" || (val == "STRAFE" || val == "START") || (val == "SELECT" || val == "CHAT" || (val == "LTRIGGER" || val == "RTRIGGER"))) || (val == "LSTICK" || val == "RSTICK");
  }
}
