﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.GraphicList
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class GraphicList : Sprite
  {
    private List<Sprite> _objects;

    public override int width
    {
      get
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (Sprite sprite in this._objects)
        {
          if ((double) sprite.x - (double) sprite.centerx < (double) num1)
            num1 = sprite.x - sprite.centerx;
          if ((double) sprite.x - (double) sprite.centerx + (double) sprite.width > (double) num2)
            num2 = sprite.x - sprite.centerx + (float) sprite.width;
        }
        return (int) ((double) num2 - (double) num1 + 0.5);
      }
    }

    public override int w => this.width;

    public override int height
    {
      get
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (Sprite sprite in this._objects)
        {
          if ((double) sprite.y - (double) sprite.centery < (double) num1)
            num1 = sprite.x - sprite.centery;
          if ((double) sprite.y - (double) sprite.centery + (double) sprite.height > (double) num2)
            num2 = sprite.y - sprite.centery + (float) sprite.width;
        }
        return (int) ((double) num2 - (double) num1 + 0.5);
      }
    }

    public override int h => this.height;

    public GraphicList(List<Sprite> list) => this._objects = list;

    public GraphicList() => this._objects = new List<Sprite>();

    public void Add(Sprite graphic) => this._objects.Add(graphic);

    public void Remove(Sprite graphic) => this._objects.Remove(graphic);

    public override void Draw()
    {
      foreach (Sprite sprite1 in this._objects)
      {
        Vec2 vec2_1 = new Vec2(sprite1.position);
        Sprite sprite2 = sprite1;
        sprite2.position = sprite2.position - this.center;
        sprite1.position.x *= this.xscale;
        sprite1.position.y *= this.yscale;
        Sprite sprite3 = sprite1;
        sprite3.position = sprite3.position + this.position;
        float alpha = sprite1.alpha;
        sprite1.alpha *= this.alpha;
        Vec2 vec2_2 = new Vec2(sprite1.scale);
        sprite1.xscale *= this.xscale;
        sprite1.yscale *= this.yscale;
        float angle = sprite1.angle;
        sprite1.angle *= this.angle;
        bool flipH = sprite1.flipH;
        sprite1.flipH = this.flipH;
        sprite1.Draw();
        sprite1.angle = angle;
        sprite1.scale = vec2_2;
        sprite1.alpha = alpha;
        sprite1.position = vec2_1;
        sprite1.flipH = flipH;
      }
    }
  }
}
