// Decompiled with JetBrains decompiler
// Type: WinFormsGraphicsDevice.GraphicsDeviceService
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace WinFormsGraphicsDevice
{
  /// <summary>
  /// Helper class responsible for creating and managing the GraphicsDevice.
  /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
  /// so even though there can be many controls, there will only ever be a single
  /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
  /// interface, which provides notification events for when the device is reset
  /// or disposed.
  /// </summary>
  public class GraphicsDeviceService : IGraphicsDeviceService
  {
    private static GraphicsDeviceService singletonInstance;
    private static int referenceCount;
    private GraphicsDevice graphicsDevice;
    private PresentationParameters parameters;

    /// <summary>
    /// Constructor is private, because this is a singleton class:
    /// client controls should use the public AddRef method instead.
    /// </summary>
    private GraphicsDeviceService(IntPtr windowHandle, int width, int height)
    {
      this.parameters = new PresentationParameters();
      this.parameters.BackBufferWidth = Math.Max(width, 1);
      this.parameters.BackBufferHeight = Math.Max(height, 1);
      this.parameters.BackBufferFormat = SurfaceFormat.Color;
      this.parameters.DepthStencilFormat = DepthFormat.Depth24;
      this.parameters.DeviceWindowHandle = windowHandle;
      this.parameters.PresentationInterval = PresentInterval.Immediate;
      this.parameters.IsFullScreen = false;
      this.graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, this.parameters);
    }

    /// <summary>Gets a reference to the singleton instance.</summary>
    public static GraphicsDeviceService AddRef(
      IntPtr windowHandle,
      int width,
      int height)
    {
      if (Interlocked.Increment(ref GraphicsDeviceService.referenceCount) == 1)
        GraphicsDeviceService.singletonInstance = new GraphicsDeviceService(windowHandle, width, height);
      return GraphicsDeviceService.singletonInstance;
    }

    /// <summary>Releases a reference to the singleton instance.</summary>
    public void Release(bool disposing)
    {
      if (Interlocked.Decrement(ref GraphicsDeviceService.referenceCount) != 0)
        return;
      if (disposing)
      {
        if (this.DeviceDisposing != null)
          this.DeviceDisposing((object) this, EventArgs.Empty);
        this.graphicsDevice.Dispose();
      }
      this.graphicsDevice = (GraphicsDevice) null;
    }

    /// <summary>
    /// Resets the graphics device to whichever is bigger out of the specified
    /// resolution or its current size. This behavior means the device will
    /// demand-grow to the largest of all its GraphicsDeviceControl clients.
    /// </summary>
    public void ResetDevice(int width, int height)
    {
      if (this.DeviceResetting != null)
        this.DeviceResetting((object) this, EventArgs.Empty);
      this.parameters.BackBufferWidth = Math.Max(this.parameters.BackBufferWidth, width);
      this.parameters.BackBufferHeight = Math.Max(this.parameters.BackBufferHeight, height);
      this.graphicsDevice.Reset(this.parameters);
      if (this.DeviceReset == null)
        return;
      this.DeviceReset((object) this, EventArgs.Empty);
    }

    /// <summary>Gets the current graphics device.</summary>
    public GraphicsDevice GraphicsDevice => this.graphicsDevice;

    public event EventHandler<EventArgs> DeviceCreated;

    public event EventHandler<EventArgs> DeviceDisposing;

    public event EventHandler<EventArgs> DeviceReset;

    public event EventHandler<EventArgs> DeviceResetting;
  }
}
