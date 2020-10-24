// Decompiled with JetBrains decompiler
// Type: DuckGame.Mouse
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Input;

namespace DuckGame
{
  public class Mouse : InputDevice
  {
    private static Vec2 _mousePos;
    private static Vec2 _mouseScreenPos;
    private static MouseState _mouseState;
    private static MouseState _mouseStatePrev;

    public override void Update()
    {
      Mouse._mouseStatePrev = Mouse._mouseState;
      Mouse._mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
      Vec3 vec3 = new Vec3((float) Mouse._mouseState.X, (float) Mouse._mouseState.Y, 0.0f);
      Mouse._mouseScreenPos = new Vec2(vec3.x / (float) Graphics.width * Layer.HUD.camera.width, vec3.y / (float) Graphics.height * Layer.HUD.camera.height);
      Mouse._mouseScreenPos.x = (float) (int) Mouse._mouseScreenPos.x;
      Mouse._mouseScreenPos.y = (float) (int) Mouse._mouseScreenPos.y;
      Mouse._mousePos = new Vec2((float) Mouse._mouseState.X, (float) Mouse._mouseState.Y);
    }

    public static InputState left
    {
      get
      {
        if (Mouse._mouseState.LeftButton == ButtonState.Pressed && Mouse._mouseStatePrev.LeftButton == ButtonState.Released)
          return InputState.Pressed;
        if (Mouse._mouseState.LeftButton == ButtonState.Pressed && Mouse._mouseStatePrev.LeftButton == ButtonState.Pressed)
          return InputState.Down;
        return Mouse._mouseState.LeftButton == ButtonState.Released && Mouse._mouseStatePrev.LeftButton == ButtonState.Pressed ? InputState.Released : InputState.None;
      }
    }

    public static InputState middle
    {
      get
      {
        if (Mouse._mouseState.MiddleButton == ButtonState.Pressed && Mouse._mouseStatePrev.MiddleButton == ButtonState.Released)
          return InputState.Pressed;
        if (Mouse._mouseState.MiddleButton == ButtonState.Pressed && Mouse._mouseStatePrev.MiddleButton == ButtonState.Pressed)
          return InputState.Down;
        return Mouse._mouseState.MiddleButton == ButtonState.Released && Mouse._mouseStatePrev.MiddleButton == ButtonState.Pressed ? InputState.Released : InputState.None;
      }
    }

    public static InputState right
    {
      get
      {
        if (Mouse._mouseState.RightButton == ButtonState.Pressed && Mouse._mouseStatePrev.RightButton == ButtonState.Released)
          return InputState.Pressed;
        if (Mouse._mouseState.RightButton == ButtonState.Pressed && Mouse._mouseStatePrev.RightButton == ButtonState.Pressed)
          return InputState.Down;
        return Mouse._mouseState.RightButton == ButtonState.Released && Mouse._mouseStatePrev.RightButton == ButtonState.Pressed ? InputState.Released : InputState.None;
      }
    }

    public static bool available => true;

    public static float scroll => (float) (Mouse._mouseStatePrev.ScrollWheelValue - Mouse._mouseState.ScrollWheelValue);

    public static float x => Mouse._mouseScreenPos.x;

    public static float y => Mouse._mouseScreenPos.y;

    public static float xScreen => Mouse.positionScreen.x;

    public static float yScreen => Mouse.positionScreen.y;

    public static float xConsole => Mouse.positionConsole.x;

    public static float yConsole => Mouse.positionConsole.y;

    public static Vec2 position => new Vec2(Mouse.x, Mouse.y);

    public static Vec2 mousePos => Mouse._mousePos;

    public static Vec2 positionScreen => Level.current.camera.transformScreenVector(Mouse._mousePos);

    public static Vec2 positionConsole => Layer.Console.camera.transformScreenVector(Mouse._mousePos);

    public Mouse()
      : base()
    {
    }
  }
}
