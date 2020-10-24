// Decompiled with JetBrains decompiler
// Type: WinFormsGraphicsDevice.NetGraphControl
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using DuckGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsGraphicsDevice
{
  /// <summary>
  /// Example control inherits from GraphicsDeviceControl, which allows it to
  /// render using a GraphicsDevice. This control shows how to draw animating
  /// 3D graphics inside a WinForms application. It hooks the Application.Idle
  /// event, using this to invalidate the control, which will cause the animation
  /// to constantly redraw.
  /// </summary>
  public class NetGraphControl : GraphicsDeviceControl
  {
    private BasicEffect effect;
    private Stopwatch timer;
    public readonly VertexPositionColor[] Vertices = new VertexPositionColor[3]
    {
      new VertexPositionColor(new Vector3(-1f, -1f, 0.0f), Microsoft.Xna.Framework.Color.Black),
      new VertexPositionColor(new Vector3(1f, -1f, 0.0f), Microsoft.Xna.Framework.Color.Black),
      new VertexPositionColor(new Vector3(0.0f, 1f, 0.0f), Microsoft.Xna.Framework.Color.Black)
    };
    private NetGraph _target;
    public static bool _paintMe;
    /// <summary>Draws the control.</summary>
    private SpriteBatch _batch;

    /// <summary>Initializes the control.</summary>
    protected override void Initialize()
    {
      this.effect = new BasicEffect(this.GraphicsDevice);
      this.effect.VertexColorEnabled = true;
      this.timer = Stopwatch.StartNew();
      Application.Idle += (EventHandler) delegate
      {
        this.Invalidate();
      };
      this._batch = new SpriteBatch(this.GraphicsDevice);
    }

    public void UpdateGraph(NetGraph target)
    {
      this._target = target;
      this.Invalidate();
    }

    protected override void Draw()
    {
      if (this._target == null)
        return;
      DuckGame.Graphics.viewport = new Viewport(0, 0, this.Width, this.Height);
      this.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
      Camera camera = new Camera(0.0f, 0.0f, (float) this.Width, (float) this.Height);
      this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this._batch.Begin();
      this._target.batch = this._batch;
      this._target.DoDraw(this.GraphicsDevice);
      this._batch.End();
    }
  }
}
