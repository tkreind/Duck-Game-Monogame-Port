// Decompiled with JetBrains decompiler
// Type: DuckGame.SunLight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class SunLight : Thing
  {
    private Color _lightColor;
    private float _range;
    private GeometryItem _geo;
    private bool _strangeFalloff;
    private bool _vertical;
    private Dictionary<Door, bool> _doors = new Dictionary<Door, bool>();
    private List<Door> _doorList = new List<Door>();
    private new bool _initialized;

    public SunLight(
      float xpos,
      float ypos,
      Color c,
      float range,
      List<LightOccluder> occluders = null,
      bool strangeFalloff = false,
      bool vertical = false)
      : base(xpos, ypos)
    {
      this.layer = Layer.Lighting;
      this._lightColor = c;
      this._range = range;
      this._vertical = vertical;
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
      if (this._vertical)
      {
        Vec2 zero1 = Vec2.Zero;
        Color white = Color.White;
        Color black = Color.Black;
        Math.Sqrt(512.0);
        Vec2 vec2_1 = Level.current.topLeft + new Vec2(-512f, -1000f);
        Vec2 zero2 = Vec2.Zero;
        for (int index = 0; index < 128; ++index)
        {
          float leftTrigger = Input.GetDevice<XInputPad>().leftTrigger;
          float num = 0.5f;
          Vec2 start = vec2_1 + new Vec2(num * 16f, 0.0f);
          vec2_1 += new Vec2(16f, 0.0f);
          this._lightColor.a = (byte) 0;
          Vec2 hitPos = Vec2.Zero;
          Vec2 vec2_2 = new Vec2(0.0f, 5000f);
          if ((Block) Level.CheckRay<AutoBlock>(start, start + vec2_2, out hitPos) == null)
            hitPos = start + vec2_2;
          Vec2 p1 = start + new Vec2(-19f, 0.0f);
          Vec2 p3 = hitPos + new Vec2(17f, 0.0f);
          this._geo.AddTriangle(p1, new Vec2(p3.x, p1.y), p3, this._lightColor, this._lightColor, this._lightColor);
          this._geo.AddTriangle(p1, new Vec2(p1.x, p3.y), p3, this._lightColor, this._lightColor, this._lightColor);
        }
      }
      else
      {
        Vec2 p2 = Vec2.Zero;
        Color white = Color.White;
        bool flag = false;
        Color black = Color.Black;
        black.a = (byte) 0;
        float num1 = (float) Math.Sqrt(512.0);
        Vec2 vec2_1 = Level.current.topLeft + new Vec2(-512f, 0.0f);
        Vec2 p1 = Vec2.Zero;
        for (int index = 0; index < 128; ++index)
        {
          float leftTrigger = Input.GetDevice<XInputPad>().leftTrigger;
          float num2 = 0.25f;
          Vec2 vec2_2 = vec2_1 + new Vec2(32f * num2, 0.0f);
          vec2_1 += new Vec2(8f, -8f);
          this._lightColor.a = (byte) 0;
          Vec2 hitPos = Vec2.Zero;
          Vec2 vec2_3 = new Vec2(5000f, 5000f);
          if ((Block) Level.CheckRay<AutoBlock>(vec2_2, vec2_2 + vec2_3, out hitPos) == null)
            hitPos = vec2_2 + vec2_3;
          else if (Level.CheckRay<AutoBlock>(hitPos + new Vec2(-4f, -4f), hitPos + new Vec2(-4f, -4f) + new Vec2(0.0f, -1000f)) != null)
          {
            Vec2 vec2_4 = new Vec2(hitPos.x - 2f, hitPos.y - 2f);
          }
          if (flag)
          {
            Vec2 vec2_5 = vec2_2 + new Vec2((float) -((double) num1 / 2.0), 0.0f);
            Vec2 vec2_6 = vec2_2 + new Vec2(num1 / 2f, 0.0f);
            Vec2 vec2_7 = hitPos + new Vec2((float) -((double) num1 / 2.0), 0.0f);
            Vec2 vec2_8 = hitPos + new Vec2(num1 / 2f, 0.0f);
            this._geo.AddTriangle(p1, vec2_2, hitPos, this._lightColor, this._lightColor, this._lightColor);
            this._geo.AddTriangle(p1, p2, hitPos, this._lightColor, this._lightColor, this._lightColor);
            if ((double) (hitPos - p2).lengthSq > 300.0)
            {
              float x = 8f;
              this._geo.AddTriangle(hitPos, hitPos + new Vec2(x, 0.0f), p2 + new Vec2(8f + x, 8f), this._lightColor, black, black);
              this._geo.AddTriangle(hitPos, p2, p2 + new Vec2(8f + x, 8f), this._lightColor, this._lightColor, black);
              this._geo.AddTriangle(hitPos, hitPos + new Vec2(0.0f, 8f), p2 - new Vec2(x, 0.0f), this._lightColor, black, black);
              this._geo.AddTriangle(hitPos, p2, p2 - new Vec2(x, 0.0f), this._lightColor, this._lightColor, black);
            }
          }
          flag = true;
          p1 = vec2_2;
          p2 = hitPos;
        }
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
