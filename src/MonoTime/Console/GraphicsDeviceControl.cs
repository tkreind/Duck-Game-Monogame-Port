// Decompiled with JetBrains decompiler
// Type: WinFormsGraphicsDevice.GraphicsDeviceControl
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsGraphicsDevice
{
  /// <summary>
  /// Custom control uses the XNA Framework GraphicsDevice to render onto
  /// a Windows Form. Derived classes can override the Initialize and Draw
  /// methods to add their own drawing code.
  /// </summary>
  public abstract class GraphicsDeviceControl : Control
  {
    private GraphicsDeviceService graphicsDeviceService;
    private ServiceContainer services = new ServiceContainer();

    /// <summary>
    /// Gets a GraphicsDevice that can be used to draw onto this control.
    /// </summary>
    public GraphicsDevice GraphicsDevice => this.graphicsDeviceService.GraphicsDevice;

    /// <summary>
    /// Gets an IServiceProvider containing our IGraphicsDeviceService.
    /// This can be used with components such as the ContentManager,
    /// which use this service to look up the GraphicsDevice.
    /// </summary>
    public ServiceContainer Services => this.services;

    /// <summary>Initializes the control.</summary>
    protected override void OnCreateControl()
    {
      if (!this.DesignMode)
      {
        this.graphicsDeviceService = GraphicsDeviceService.AddRef(this.Handle, this.ClientSize.Width, this.ClientSize.Height);
        this.services.AddService<IGraphicsDeviceService>((IGraphicsDeviceService) this.graphicsDeviceService);
        this.Initialize();
      }
      base.OnCreateControl();
    }

    /// <summary>Disposes the control.</summary>
    protected override void Dispose(bool disposing)
    {
      if (this.graphicsDeviceService != null)
      {
        this.graphicsDeviceService.Release(disposing);
        this.graphicsDeviceService = (GraphicsDeviceService) null;
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Redraws the control in response to a WinForms paint message.
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
      string text = this.BeginDraw();
      if (string.IsNullOrEmpty(text))
      {
        this.Draw();
        this.EndDraw();
      }
      else
        this.PaintUsingSystemDrawing(e.Graphics, text);
    }

    /// <summary>
    /// Attempts to begin drawing the control. Returns an error message string
    /// if this was not possible, which can happen if the graphics device is
    /// lost, or if we are running inside the Form designer.
    /// </summary>
    private string BeginDraw()
    {
      if (this.graphicsDeviceService == null)
        return this.Text + "\n\n" + (object) this.GetType();
      string str = this.HandleDeviceReset();
      if (!string.IsNullOrEmpty(str))
        return str;
      DuckGame.Graphics.viewport = new Viewport()
      {
        X = 0,
        Y = 0,
        Width = this.ClientSize.Width,
        Height = this.ClientSize.Height,
        MinDepth = 0.0f,
        MaxDepth = 1f
      };
      return (string) null;
    }

    /// <summary>
    /// Ends drawing the control. This is called after derived classes
    /// have finished their Draw method, and is responsible for presenting
    /// the finished image onto the screen, using the appropriate WinForms
    /// control handle to make sure it shows up in the right place.
    /// </summary>
    private void EndDraw()
    {
      try
      {
                // Probably a big TODO, limitation of monogame
                this.GraphicsDevice.Present();
        //this.GraphicsDevice.Present(new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height)), new Microsoft.Xna.Framework.Rectangle?(), this.Handle);
      }
      catch
      {
      }
    }

    /// <summary>
    /// Helper used by BeginDraw. This checks the graphics device status,
    /// making sure it is big enough for drawing the current control, and
    /// that the device is not lost. Returns an error string if the device
    /// could not be reset.
    /// </summary>
    private string HandleDeviceReset()
    {
      bool flag;
      switch (this.GraphicsDevice.GraphicsDeviceStatus)
      {
        case GraphicsDeviceStatus.Lost:
          return "Graphics device lost";
        case GraphicsDeviceStatus.NotReset:
          flag = true;
          break;
        default:
          PresentationParameters presentationParameters = this.GraphicsDevice.PresentationParameters;
          flag = this.ClientSize.Width > presentationParameters.BackBufferWidth || this.ClientSize.Height > presentationParameters.BackBufferHeight;
          break;
      }
      if (flag)
      {
        try
        {
          this.graphicsDeviceService.ResetDevice(this.ClientSize.Width, this.ClientSize.Height);
        }
        catch (Exception ex)
        {
          return "Graphics device reset failed\n\n" + (object) ex;
        }
      }
      return (string) null;
    }

    /// <summary>
    /// If we do not have a valid graphics device (for instance if the device
    /// is lost, or if we are running inside the Form designer), we must use
    /// regular System.Drawing method to display a status message.
    /// </summary>
    protected virtual void PaintUsingSystemDrawing(System.Drawing.Graphics graphics, string text)
    {
      graphics.Clear(System.Drawing.Color.CornflowerBlue);
      using (Brush brush = (Brush) new SolidBrush(System.Drawing.Color.Black))
      {
        using (StringFormat format = new StringFormat())
        {
          format.Alignment = StringAlignment.Center;
          format.LineAlignment = StringAlignment.Center;
          graphics.DrawString(text, this.Font, brush, (RectangleF) this.ClientRectangle, format);
        }
      }
    }

    /// <summary>
    /// Ignores WinForms paint-background messages. The default implementation
    /// would clear the control to the current background color, causing
    /// flickering when our OnPaint implementation then immediately draws some
    /// other color over the top using the XNA Framework GraphicsDevice.
    /// </summary>
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    /// <summary>
    /// Derived classes override this to initialize their drawing code.
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// Derived classes override this to draw themselves using the GraphicsDevice.
    /// </summary>
    protected abstract void Draw();
  }
}
