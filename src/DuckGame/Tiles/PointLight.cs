// Decompiled with JetBrains decompiler
// Type: DuckGame.PointLight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class PointLight : Thing
  {
    public Material material;
    private List<LightOccluder> _occluders = new List<LightOccluder>();
    private Color _lightColor;
    private float _range;
    private GeometryItem _geo;
    private bool _strangeFalloff;
    private Dictionary<Door, bool> _doors = new Dictionary<Door, bool>();
    private List<Door> _doorList = new List<Door>();
    private new bool _initialized;

    public PointLight(
      float xpos,
      float ypos,
      Color c,
      float range,
      List<LightOccluder> occluders = null,
      bool strangeFalloff = false)
      : base(xpos, ypos)
    {
      this.layer = Layer.Lighting;
      this._occluders = occluders;
      this._lightColor = c;
      if (this._occluders == null)
        this._occluders = new List<LightOccluder>();
      this._range = range;
      this._strangeFalloff = strangeFalloff;
    }

    public override void Initialize() => Layer.lighting = true;

    public override void Update()
    {
      Layer.lighting = true;
      if (this._initialized)
        return;
      this._geo = MTSpriteBatch.CreateGeometryItem();
      this.DrawLight();
      this._initialized = true;
    }

    private void DrawLight()
    {
      Vec2 vec2_1 = Vec2.Zero;
      Color c3 = Color.White;
      bool flag1 = false;
      int num1 = 64;
      for (int index = 0; index <= num1; ++index)
      {
        Color c2 = Color.Black;
        float deg = (float) ((double) index / (double) num1 * 360.0);
        Vec2 vec2_2 = new Vec2((float) Math.Cos((double) Maths.DegToRad(deg)), -(float) Math.Sin((double) Maths.DegToRad(deg)));
        Vec2 hitPos = Vec2.Zero;
        Vec2 end = this.position + vec2_2 * this._range;
        if (this._strangeFalloff)
          hitPos = end;
        else
          Level.CheckRay<Block>(this.position, end, out hitPos);
        Color c = this._lightColor;
        float length = (hitPos - this.position).length;
        if (this._strangeFalloff)
          length += 30f;
        float num2;
        if (this._strangeFalloff)
        {
          float num3 = 1f - Math.Max(length - 30f, 0.0f) / this._range;
          num2 = num3 * num3;
        }
        else
          num2 = (float) (1.0 - (double) length / (double) this._range);
        bool flag2 = false;
        Color color = Color.White;
        foreach (LightOccluder occluder in this._occluders)
        {
          if (Collision.LineIntersect(occluder.p1, occluder.p2, this.position, hitPos) && (!flag1 || Collision.LineIntersect(occluder.p1, occluder.p2, this.position, vec2_1)))
          {
            Vec3 vector3 = (c * 0.7f).ToVector3();
            color = occluder.color;
            c = new Color(vector3 * occluder.color.ToVector3());
            flag2 = true;
            break;
          }
        }
        c2 = this._lightColor * num2;
        if (flag2)
          c2 = new Color((c2 * 0.7f).ToVector3() * color.ToVector3());
        c2.a = (byte) 0;
        c.a = (byte) 0;
        if (flag1)
        {
          if (!Layer.lightingTwoPointOh)
          {
            hitPos += vec2_2;
            hitPos.x = (float) Math.Round((double) hitPos.x);
            hitPos.y = (float) Math.Round((double) hitPos.y);
          }
          this._geo.AddTriangle(this.position, hitPos, vec2_1, c, c2, c3);
        }
        flag1 = true;
        vec2_1 = hitPos;
        c3 = c2;
      }
    }

    public override void Draw()
    {
      if (this._geo == null)
        return;
      Graphics.screen.SubmitGeometry(this._geo);
    }
  }
}
