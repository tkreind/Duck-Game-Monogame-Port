﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.StaticRenderer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class StaticRenderer
  {
    private static MultiMap<Layer, StaticRenderSection> _targets = new MultiMap<Layer, StaticRenderSection>();
    private static Vec2 _position = new Vec2((float) sbyte.MinValue, (float) sbyte.MinValue);
    private static int _size = 128;
    private static int _numSections = 8;

    public static void InitializeLayer(Layer layer)
    {
      if (StaticRenderer._targets.ContainsKey(layer))
        return;
      for (int index1 = 0; index1 < StaticRenderer._numSections; ++index1)
      {
        for (int index2 = 0; index2 < StaticRenderer._numSections; ++index2)
          StaticRenderer._targets.Add(layer, new StaticRenderSection()
          {
            target = new RenderTarget2D(StaticRenderer._size, StaticRenderer._size),
            position = new Vec2(StaticRenderer._position.x + (float) (index2 * StaticRenderer._size), StaticRenderer._position.y + (float) (index1 * StaticRenderer._size))
          });
      }
    }

    public static void ProcessThing(Thing t)
    {
      Layer background = Layer.Background;
      Vec2 vec2_1 = t.position - t.center - StaticRenderer._position;
      int num1 = (int) Math.Floor((double) vec2_1.x / (double) StaticRenderer._size);
      int num2 = (int) Math.Floor((double) vec2_1.y / (double) StaticRenderer._size);
      StaticRenderer.InitializeLayer(background);
      Vec2 vec2_2 = t.position - t.center + new Vec2((float) t.graphic.width, (float) t.graphic.height) - StaticRenderer._position;
      int num3 = (int) Math.Floor((double) vec2_2.x / (double) StaticRenderer._size);
      int num4 = (int) Math.Floor((double) vec2_2.y / (double) StaticRenderer._size);
      StaticRenderer._targets[background][num2 * StaticRenderer._numSections + num1].things.Add(t);
      if (num1 != num3)
        StaticRenderer._targets[background][num2 * StaticRenderer._numSections + num3].things.Add(t);
      if (num2 != num4)
        StaticRenderer._targets[background][num4 * StaticRenderer._numSections + num1].things.Add(t);
      if (num1 == num3 || num2 == num4)
        return;
      StaticRenderer._targets[background][num4 * StaticRenderer._numSections + num3].things.Add(t);
    }

    public static void Update()
    {
    }

    public static void RenderLayer(Layer layer)
    {
    }
  }
}
