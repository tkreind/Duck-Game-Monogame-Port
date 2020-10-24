// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorCam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EditorCam : Camera
  {
    protected new Vec2 _zoomPoint = new Vec2();
    protected float _zoomInc;
    protected float _zoom = 1f;

    public new Vec2 zoomPoint
    {
      get => this._zoomPoint;
      set
      {
        if (!(this._zoomPoint != value))
          return;
        this._zoomPoint = value;
        this._dirty = true;
      }
    }

    public float zoomInc
    {
      get => this._zoomInc;
      set
      {
        if ((double) this._zoomInc == (double) value)
          return;
        this._zoomInc = value;
        this._dirty = true;
      }
    }

    public float zoom => this._zoom;

    public override void Update()
    {
    }
  }
}
