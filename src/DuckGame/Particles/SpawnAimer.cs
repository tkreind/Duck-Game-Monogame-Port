// Decompiled with JetBrains decompiler
// Type: DuckGame.SpawnAimer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class SpawnAimer : Thing
  {
    private float _moveSpeed;
    private float _thickness;
    private Color _color;
    private DuckPersona _persona;
    private Vec2 targetPos;
    private float distOut = 100f;
    private float distLen = 64f;
    private float sizeWaver;
    private float rot;
    private float streamAlpha = 1f;
    private List<Vec2> prevPos = new List<Vec2>();

    public SpawnAimer(
      float xpos,
      float ypos,
      int dir,
      float moveSpeed,
      Color color,
      DuckPersona person,
      float thickness)
      : base(xpos, ypos)
    {
      this._moveSpeed = moveSpeed;
      this._color = color;
      this._thickness = thickness;
      this.offDir = (sbyte) dir;
      this._persona = person;
      this.targetPos = new Vec2(xpos, ypos);
      if (this._persona == Persona.Duck1)
        this.position = new Vec2(0.0f, 0.0f);
      else if (this._persona == Persona.Duck2)
        this.position = new Vec2(320f, 0.0f);
      else if (this._persona == Persona.Duck3)
        this.position = new Vec2(0.0f, 180f);
      else if (this._persona == Persona.Duck4)
        this.position = new Vec2(320f, 180f);
      this.prevPos.Add(this.position);
      this.layer = Layer.HUD;
    }

    public override void Update()
    {
      if (GameMode.started)
        Level.Remove((Thing) this);
      this.distOut = Lerp.FloatSmooth(this.distOut, 16f, 0.08f, 1.2f);
      this.distLen = Lerp.FloatSmooth(this.distLen, 10f, 0.08f, 1.2f);
      this.rot = Lerp.FloatSmooth(this.rot, 45f, 0.08f, 1.1f);
      if ((double) Math.Abs(this.rot - 45f) < 20.0)
      {
        this.streamAlpha -= 0.03f;
        if ((double) this.streamAlpha < 0.0)
          this.streamAlpha = 0.0f;
      }
      Vec2 to = Vec2.Transform(this.targetPos, Level.current.camera.getMatrix());
      to.x *= this.layer.camera.width / (float) Graphics.width;
      to.y *= this.layer.camera.height / (float) Graphics.height;
      this.position = Lerp.Vec2Smooth(this.position, to, 0.2f);
      if ((double) (this.position - to).length > 16.0)
        this.prevPos.Add(this.position);
      this.sizeWaver += 0.2f;
    }

    public override void Draw()
    {
      float num1 = this.distOut + (float) Math.Sin((double) this.sizeWaver) * 2f;
      float distOut = this.distOut;
      this._thickness = 2f;
      for (int index = 0; index < 4; ++index)
      {
        float deg = this.rot + (float) index * 90f;
        Vec2 vec2 = new Vec2((float) Math.Cos((double) Maths.DegToRad(deg)), (float) -Math.Sin((double) Maths.DegToRad(deg)));
        Graphics.DrawLine(this.position + vec2 * distOut, this.position + vec2 * (distOut + this.distLen), this._color * this.alpha, this._thickness, (Depth) 0.9f);
        Graphics.DrawLine(this.position + vec2 * (distOut - 1f), this.position + vec2 * (distOut + 1f + this.distLen), Color.Black, this._thickness + 2f, (Depth) 0.8f);
      }
      if ((double) this.streamAlpha <= 0.00999999977648258)
        return;
      int num2 = 0;
      Vec2 vec2_1 = Vec2.Zero;
      bool flag = false;
      foreach (Vec2 prevPo in this.prevPos)
      {
        if (flag)
        {
          Vec2 normalized = (vec2_1 - prevPo).normalized;
          Graphics.DrawLine(vec2_1 - normalized, prevPo + normalized, this._color * this.streamAlpha, (float) (4.0 + (double) num2 * 2.0), (Depth) 0.9f);
        }
        vec2_1 = prevPo;
        flag = true;
        ++num2;
      }
    }
  }
}
