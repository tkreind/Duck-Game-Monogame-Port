﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UIProgressBar
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class UIProgressBar : UIComponent
  {
    private FieldBinding _field;
    private Vec2 _barSize;
    private float _step;

    public UIProgressBar(float wide, float high, FieldBinding field, float increment, Color c = default (Color))
      : base(0.0f, 0.0f, 0.0f, 0.0f)
    {
      this._field = field;
      this._barSize = new Vec2(wide, high);
      this._collisionSize = this._barSize;
      this._step = increment;
    }

    public override void Draw()
    {
      float num1 = this._barSize.x * this.scale.x;
      float num2 = this._barSize.y * this.scale.y;
      int num3 = (int) Math.Ceiling(((double) this._field.max - (double) this._field.min) / (double) this._step);
      for (int index = 0; index < num3; ++index)
      {
        Vec2 p1 = this.position - new Vec2(this.halfWidth, num2 / 2f) + new Vec2((float) index * (num1 / (float) num3), 0.0f);
        Vec2 p2 = this.position - new Vec2(this.halfWidth, (float) (-(double) num2 / 2.0)) + new Vec2((float) ((double) (index + 1) * ((double) num1 / (double) num3) - 2.0), 0.0f);
        int align = (int) this.align;
        if (0 > 0)
        {
          p1.x += this.halfWidth - num1 / 2f;
          p2.x += this.halfWidth - num1 / 2f;
        }
        else if ((this.align & UIAlign.Right) > UIAlign.Center)
        {
          p1.x += this.width - num1;
          p2.x += this.width - num1;
        }
        float num4 = (float) this._field.value;
        Graphics.DrawRect(p1, p2, (double) num4 > (double) index * (double) this._step ? Color.White : new Color(70, 70, 70), this.depth);
      }
    }
  }
}
