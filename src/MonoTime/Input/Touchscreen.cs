// Decompiled with JetBrains decompiler
// Type: DuckGame.Touchscreen
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace DuckGame
{
  public class Touchscreen : InputDevice
  {
    private static List<Vec2> _touches = new List<Vec2>();
    private static int _framesWithTouches = 0;
    private static bool _tap = false;
    private static bool _wasTap = false;

    public override void Update()
    {
      Touchscreen._wasTap = Touchscreen._tap;
      Touchscreen._tap = false;
      Touchscreen._touches.Clear();
      foreach (TouchLocation touchLocation in TouchPanel.GetState())
      {
        if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Moved)
          Touchscreen._touches.Add(new Vec2(touchLocation.Position.X, touchLocation.Position.Y));
      }
      if (Touchscreen._touches.Count > 0)
      {
        ++Touchscreen._framesWithTouches;
      }
      else
      {
        if (Touchscreen._framesWithTouches > 0 && Touchscreen._framesWithTouches < 10)
          Touchscreen._tap = true;
        Touchscreen._framesWithTouches = 0;
      }
    }

    public static InputState left
    {
      get
      {
        if (Touchscreen._tap)
          return InputState.Pressed;
        return Touchscreen._wasTap ? InputState.Released : InputState.None;
      }
    }

    public static InputState middle => InputState.None;

    public static InputState right => InputState.None;

    public static float scroll => 0.0f;

    public static float x => Touchscreen._touches.Count <= 0 ? 0.0f : Touchscreen._touches[0].x;

    public static float y => Touchscreen._touches.Count <= 0 ? 0.0f : Touchscreen._touches[0].y;

    public static float xScreen => Touchscreen.positionScreen.x;

    public static float yScreen => Touchscreen.positionScreen.y;

    public static Vec2 position => new Vec2(Touchscreen.x, Touchscreen.y);

    public static Vec2 positionScreen => Level.current.camera.transformScreenVector(Touchscreen.position);

    public Touchscreen()
      : base()
    {
    }
  }
}
