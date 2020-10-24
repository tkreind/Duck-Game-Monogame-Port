﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Lerp
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public static class Lerp
  {
    public static float Float(float current, float to, float amount)
    {
      if ((double) to > (double) current)
      {
        current += amount;
        if ((double) to < (double) current)
          current = to;
      }
      else if ((double) to < (double) current)
      {
        current -= amount;
        if ((double) to > (double) current)
          current = to;
      }
      return current;
    }

    public static float FloatSmooth(float current, float to, float amount, float toMul = 1f)
    {
      float num1 = to - (1f - toMul) * to;
      if ((double) to < (double) current)
        num1 = to + (1f - toMul) * to;
      float num2 = current + amount * (num1 - current);
      if ((double) to >= (double) current && (double) num2 > (double) to || (double) to <= (double) current && (double) num2 < (double) to)
        num2 = to;
      return num2;
    }

    public static DuckGame.Vec2 Vec2(DuckGame.Vec2 current, DuckGame.Vec2 to, float amount)
    {
      DuckGame.Vec2 vec2_1 = current;
      DuckGame.Vec2 vec2_2 = to;
      DuckGame.Vec2 vec2_3 = vec2_2 - vec2_1;
      if ((double) vec2_3.Length() < 9.99999974737875E-05)
        return current;
      vec2_3.Normalize();
      DuckGame.Vec2 vec2_4 = vec2_1 + vec2_3 * amount;
      if ((double) vec2_2.x > (double) vec2_1.x && (double) vec2_4.x > (double) vec2_2.x)
        vec2_4.x = vec2_2.x;
      if ((double) vec2_2.x < (double) vec2_1.x && (double) vec2_4.x < (double) vec2_2.x)
        vec2_4.x = vec2_2.x;
      if ((double) vec2_2.y > (double) vec2_1.y && (double) vec2_4.y > (double) vec2_2.y)
        vec2_4.y = vec2_2.y;
      if ((double) vec2_2.y < (double) vec2_1.y && (double) vec2_4.y < (double) vec2_2.y)
        vec2_4.y = vec2_2.y;
      return vec2_4;
    }

    public static DuckGame.Vec2 Vec2Smooth(DuckGame.Vec2 current, DuckGame.Vec2 to, float amount) => current + amount * (to - current);

    public static DuckGame.Vec3 Vec3(DuckGame.Vec3 current, DuckGame.Vec3 to, float amount)
    {
      DuckGame.Vec3 vec3_1 = current;
      DuckGame.Vec3 vec3_2 = to;
      DuckGame.Vec3 vec3_3 = vec3_2 - vec3_1;
      if ((double) vec3_3.Length() < 9.99999974737875E-05)
        return current;
      vec3_3.Normalize();
      DuckGame.Vec3 vec3_4 = vec3_1 + vec3_3 * amount;
      if ((double) vec3_2.x > (double) vec3_1.x && (double) vec3_4.x > (double) vec3_2.x)
        vec3_4.x = vec3_2.x;
      if ((double) vec3_2.x < (double) vec3_1.x && (double) vec3_4.x < (double) vec3_2.x)
        vec3_4.x = vec3_2.x;
      if ((double) vec3_2.y > (double) vec3_1.y && (double) vec3_4.y > (double) vec3_2.y)
        vec3_4.y = vec3_2.y;
      if ((double) vec3_2.y < (double) vec3_1.y && (double) vec3_4.y < (double) vec3_2.y)
        vec3_4.y = vec3_2.y;
      if ((double) vec3_2.z > (double) vec3_1.z && (double) vec3_4.z > (double) vec3_2.z)
        vec3_4.z = vec3_2.z;
      if ((double) vec3_2.z < (double) vec3_1.z && (double) vec3_4.z < (double) vec3_2.z)
        vec3_4.z = vec3_2.z;
      return vec3_4;
    }

    public static DuckGame.Color Color(DuckGame.Color current, DuckGame.Color to, float amount)
    {
      Vec4 vector4_1 = current.ToVector4();
      Vec4 vector4_2 = to.ToVector4();
      Vec4 vec4_1 = vector4_2 - vector4_1;
      if ((double) vec4_1.Length() < 9.99999974737875E-05)
        return current;
      vec4_1.Normalize();
      Vec4 vec4_2 = vector4_1 + vec4_1 * amount;
      if ((double) vector4_2.x > (double) vector4_1.x && (double) vec4_2.x > (double) vector4_2.x)
        vec4_2.x = vector4_2.x;
      if ((double) vector4_2.x < (double) vector4_1.x && (double) vec4_2.x < (double) vector4_2.x)
        vec4_2.x = vector4_2.x;
      if ((double) vector4_2.y > (double) vector4_1.y && (double) vec4_2.y > (double) vector4_2.y)
        vec4_2.y = vector4_2.y;
      if ((double) vector4_2.y < (double) vector4_1.y && (double) vec4_2.y < (double) vector4_2.y)
        vec4_2.y = vector4_2.y;
      if ((double) vector4_2.z > (double) vector4_1.z && (double) vec4_2.z > (double) vector4_2.z)
        vec4_2.z = vector4_2.z;
      if ((double) vector4_2.z < (double) vector4_1.z && (double) vec4_2.z < (double) vector4_2.z)
        vec4_2.z = vector4_2.z;
      if ((double) vector4_2.w > (double) vector4_1.w && (double) vec4_2.w > (double) vector4_2.w)
        vec4_2.w = vector4_2.w;
      if ((double) vector4_2.w < (double) vector4_1.w && (double) vec4_2.w < (double) vector4_2.w)
        vec4_2.w = vector4_2.w;
      return new DuckGame.Color(vec4_2.x, vec4_2.y, vec4_2.z, vec4_2.w);
    }

    public static DuckGame.Color ColorSmooth(DuckGame.Color current, DuckGame.Color to, float amount)
    {
      Vec4 vector4_1 = current.ToVector4();
      Vec4 vector4_2 = to.ToVector4();
      Vec4 vec4 = vector4_1 + (vector4_2 - vector4_1) * amount;
      return new DuckGame.Color(vec4.x, vec4.y, vec4.z, vec4.w);
    }
  }
}
