// Decompiled with JetBrains decompiler
// Type: DuckGame.Resolution
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public static class Resolution
  {
    private static GraphicsDeviceManager _Device = (GraphicsDeviceManager) null;
    private static int _Width;
    private static int _Height;
    private static int _VWidth;
    private static int _VHeight;
    private static Matrix _ScaleMatrix;
    private static bool _FullScreen = false;
    private static bool _dirtyMatrix = true;
    public static Color clearColor = Color.Black;

    public static void Initialize(ref GraphicsDeviceManager device, int width, int height)
    {
      Resolution._Width = width;
      Resolution._Height = height;
      Resolution._Device = device;
      Resolution._dirtyMatrix = true;
    }

    public static Matrix getTransformationMatrix()
    {
      if (Resolution._dirtyMatrix)
        Resolution.RecreateScaleMatrix();
      return Resolution._ScaleMatrix;
    }

    public static void SetResolution(int Width, int Height, bool FullScreen)
    {
      Resolution._Width = Width;
      Resolution._Height = Height;
      Resolution._FullScreen = FullScreen;
      Resolution.ApplyResolutionSettings();
    }

    public static void SetVirtualResolution(int Width, int Height)
    {
      Resolution._VWidth = Width;
      Resolution._VHeight = Height;
      Resolution._dirtyMatrix = true;
    }

    private static void ApplyResolutionSettings()
    {
      if (!Resolution._FullScreen)
      {
        if (Resolution._Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width && Resolution._Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
        {
          Resolution._Device.PreferredBackBufferWidth = Resolution._Width;
          Resolution._Device.PreferredBackBufferHeight = Resolution._Height;
          Resolution._Device.IsFullScreen = Resolution._FullScreen;
          Resolution._Device.ApplyChanges();
          Resolution._Device.PreferredBackBufferWidth = Resolution._Width;
          Resolution._Device.PreferredBackBufferHeight = Resolution._Height;
          Resolution._Device.ApplyChanges();
        }
      }
      else
      {
        foreach (DisplayMode supportedDisplayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
        {
          if (supportedDisplayMode.Width == Resolution._Width && supportedDisplayMode.Height == Resolution._Height)
          {
            Resolution._Device.PreferredBackBufferWidth = Resolution._Width;
            Resolution._Device.PreferredBackBufferHeight = Resolution._Height;
            Resolution._Device.IsFullScreen = Resolution._FullScreen;
            Resolution._Device.ApplyChanges();
          }
        }
      }
      Resolution._dirtyMatrix = true;
      Resolution._Width = Resolution._Device.PreferredBackBufferWidth;
      Resolution._Height = Resolution._Device.PreferredBackBufferHeight;
    }

    /// <summary>
    /// Sets the device to use the draw pump
    /// Sets correct aspect ratio
    /// </summary>
    public static void BeginDraw()
    {
      Resolution.FullViewport();
      Resolution._Device.GraphicsDevice.Clear((Microsoft.Xna.Framework.Color) Resolution.clearColor);
      Resolution.ResetViewport();
      Resolution._Device.GraphicsDevice.Clear((Microsoft.Xna.Framework.Color) Resolution.clearColor);
    }

    public static void RecreateScaleMatrix()
    {
      Resolution._dirtyMatrix = false;
      Resolution._ScaleMatrix = Matrix.CreateScale((float) DuckGame.Graphics.viewport.Width / (float) Resolution._VWidth, (float) DuckGame.Graphics.viewport.Width / (float) Resolution._VWidth, 1f);
    }

    public static void FullViewport()
    {
      Viewport viewport = new Viewport();
      viewport.X = viewport.Y = 0;
      viewport.Width = Resolution._Width;
      viewport.Height = Resolution._Height;
      DuckGame.Graphics.viewport = viewport;
    }

    /// <summary>Get virtual Mode Aspect Ratio</summary>
    /// <returns>aspect ratio</returns>
    public static float getVirtualAspectRatio() => (float) Resolution._VWidth / (float) Resolution._VHeight;

    public static void ResetViewport()
    {
      float virtualAspectRatio = Resolution.getVirtualAspectRatio();
      int num1 = Resolution._Device.PreferredBackBufferWidth;
      int num2 = (int) ((double) num1 / (double) virtualAspectRatio + 0.5);
      bool flag = false;
      if (num2 > Resolution._Device.PreferredBackBufferHeight)
      {
        num2 = Resolution._Device.PreferredBackBufferHeight;
        num1 = (int) ((double) num2 * (double) virtualAspectRatio + 0.5);
        flag = true;
      }
      Viewport viewport = new Viewport();
      viewport.X = Resolution._Device.PreferredBackBufferWidth / 2 - num1 / 2;
      viewport.Y = Resolution._Device.PreferredBackBufferHeight / 2 - num2 / 2;
      viewport.Width = num1;
      viewport.Height = num2;
      viewport.MinDepth = 0.0f;
      viewport.MaxDepth = 1f;
      if (flag)
        Resolution._dirtyMatrix = true;
      DuckGame.Graphics.viewport = viewport;
    }
  }
}
