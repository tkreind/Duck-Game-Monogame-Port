// Decompiled with JetBrains decompiler
// Type: DuckGame.Camera
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Camera
  {
    protected Matrix _matrix;
    protected bool _dirty = true;
    protected Vec2 _position = new Vec2();
    protected Vec2 _size = new Vec2(320f, 320f * Graphics.aspect);
    protected Vec2 _zoomPoint = new Vec2(0.0f, 0.0f);
    private Rectangle _rectangle;
    private Vec2 _viewSize = new Vec2();

    public Vec2 position
    {
      get => this._position;
      set
      {
        if (!(this._position != value))
          return;
        this._position = value;
        this._dirty = true;
      }
    }

    public float x
    {
      get => this._position.x;
      set
      {
        if ((double) this._position.x == (double) value)
          return;
        this._position.x = value;
        this._dirty = true;
      }
    }

    public float y
    {
      get => this._position.y;
      set
      {
        if ((double) this._position.y == (double) value)
          return;
        this._position.y = value;
        this._dirty = true;
      }
    }

    public Vec2 center
    {
      get => new Vec2(this.centerX, this.centerY);
      set
      {
        this.centerX = value.x;
        this.centerY = value.y;
      }
    }

    public float centerX
    {
      get => this._position.x + this.width / 2f;
      set
      {
        if ((double) this.centerX == (double) value)
          return;
        this._position.x = value - this.width / 2f;
        this._dirty = true;
      }
    }

    public float centerY
    {
      get => this._position.y + this.height / 2f;
      set
      {
        if ((double) this.centerY == (double) value)
          return;
        this._position.y = value - this.height / 2f;
        this._dirty = true;
      }
    }

    public float top => this.y;

    public float bottom => this.y + this.height;

    public float left => this.x;

    public float right => this.x + this.width;

    public Vec2 size
    {
      get => this._size;
      set
      {
        this._size = value;
        this._dirty = true;
      }
    }

    public float width
    {
      get => this._size.x;
      set
      {
        if ((double) this._size.x == (double) value)
          return;
        this._size.x = value;
        this._dirty = true;
      }
    }

    public float height
    {
      get => this._size.y;
      set
      {
        if ((double) this._size.y == (double) value)
          return;
        this._size.y = value;
        this._dirty = true;
      }
    }

    public Vec2 zoomPoint
    {
      get => this._zoomPoint;
      set => this._zoomPoint = value;
    }

    public float PercentW(float percent) => this.width * (percent / 100f);

    public float PercentH(float percent) => this.height * (percent / 100f);

    public Vec2 PercentWH(float wide, float high) => new Vec2(this.width * (wide / 100f), this.height * (high / 100f));

    public Vec2 OffsetTL(float t, float l) => new Vec2(this.x + t, this.y + l);

    public Vec2 OffsetBR(float t, float l) => new Vec2(this.x + this.width + t, this.y + this.height + l);

    public Vec2 OffsetCenter(float t, float l) => new Vec2(this.x + this.PercentW(50f) + t, this.y + this.PercentH(50f) + l);

    public Camera()
    {
    }

    public Camera(float xval, float yval, float wval = -1f, float hval = -1f)
    {
      if ((double) wval < 0.0)
        wval = 320f;
      if ((double) hval < 0.0)
        hval = 320f * Graphics.aspect;
      this.x = xval;
      this.y = yval;
      this.width = wval;
      this.height = hval;
    }

    public virtual void Update()
    {
    }

    public virtual Vec2 transformScreenVector(Vec2 vector)
    {
      Vec3 vec3 = Vec3.Transform(new Vec3(vector.x, vector.y, 0.0f), Matrix.Invert(this.getMatrix()));
      return new Vec2(vec3.x, vec3.y);
    }

    public virtual Vec2 transformTime(Vec2 vector)
    {
      Vec3 vec3 = Vec3.Transform(new Vec3(vector.x, vector.y, 0.0f), Resolution.getTransformationMatrix() * this.getMatrix());
      return new Vec2(vec3.x, vec3.y);
    }

    public virtual Vec2 transformWorldVector(Vec2 vector)
    {
      Vec3 vec3 = Vec3.Transform(new Vec3(vector.x, vector.y, 0.0f), Matrix.Invert(Resolution.getTransformationMatrix()) * this.getMatrix());
      return new Vec2(vec3.x, vec3.y);
    }

    public Rectangle rectangle => this._rectangle;

    public virtual Matrix getMatrix()
    {
      if (this._dirty || (double) Graphics.viewport.Width != (double) this._viewSize.x || (double) Graphics.viewport.Height != (double) this._viewSize.y)
      {
        this._rectangle = new Rectangle(this.left - 16f, this.top - 16f, this.size.x + 32f, this.size.y + 32f);
        this._viewSize = new Vec2((float) Graphics.viewport.Width, (float) Graphics.viewport.Height);
        Vec2 position = this.position;
        float width = this.width;
        float height = this.height;
        this._matrix = Matrix.CreateTranslation(new Vec3(-position.x, -position.y, 0.0f)) * Matrix.CreateScale(this._viewSize.x / width, this._viewSize.y / height, 1f);
        this._dirty = false;
      }
      return this._matrix;
    }
  }
}
