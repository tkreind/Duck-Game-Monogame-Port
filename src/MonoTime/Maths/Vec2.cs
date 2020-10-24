﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Vec2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using System;
using System.Globalization;

namespace DuckGame
{
  [Serializable]
  public struct Vec2 : IEquatable<Vec2>
  {
    private static Vec2 zeroVector = new Vec2(0.0f, 0.0f);
    private static Vec2 unitVector = new Vec2(1f, 1f);
    private static Vec2 unitxVector = new Vec2(1f, 0.0f);
    private static Vec2 unityVector = new Vec2(0.0f, 1f);
    public float x;
    public float y;

    public float length => this.Length();

    public float lengthSq => this.LengthSquared();

    public static Vec2 Zero => Vec2.zeroVector;

    public static Vec2 One => Vec2.unitVector;

    public static Vec2 Unitx => Vec2.unitxVector;

    public static Vec2 Unity => Vec2.unityVector;

    public Vec2(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public Vec2(float value)
    {
      this.x = value;
      this.y = value;
    }

    public Vec2(Vec2 vec)
    {
      this.x = vec.x;
      this.y = vec.y;
    }

    public static Vec2 Add(Vec2 value1, Vec2 value2)
    {
      value1.x += value2.x;
      value1.y += value2.y;
      return value1;
    }

    public static void Add(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = value1.x + value2.x;
      result.y = value1.y + value2.y;
    }

    public static Vec2 Barycentric(
      Vec2 value1,
      Vec2 value2,
      Vec2 value3,
      float amount1,
      float amount2)
    {
      return new Vec2(MathHelper.Barycentric(value1.x, value2.x, value3.x, amount1, amount2), MathHelper.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
    }

    public static void Barycentric(
      ref Vec2 value1,
      ref Vec2 value2,
      ref Vec2 value3,
      float amount1,
      float amount2,
      out Vec2 result)
    {
      result = new Vec2(MathHelper.Barycentric(value1.x, value2.x, value3.x, amount1, amount2), MathHelper.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
    }

    public static Vec2 CatmullRom(
      Vec2 value1,
      Vec2 value2,
      Vec2 value3,
      Vec2 value4,
      float amount)
    {
      return new Vec2(MathHelper.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount), MathHelper.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
    }

    public static void CatmullRom(
      ref Vec2 value1,
      ref Vec2 value2,
      ref Vec2 value3,
      ref Vec2 value4,
      float amount,
      out Vec2 result)
    {
      result = new Vec2(MathHelper.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount), MathHelper.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
    }

    public static Vec2 Clamp(Vec2 value1, Vec2 min, Vec2 max) => new Vec2(MathHelper.Clamp(value1.x, min.x, max.x), MathHelper.Clamp(value1.y, min.y, max.y));

    public static void Clamp(ref Vec2 value1, ref Vec2 min, ref Vec2 max, out Vec2 result) => result = new Vec2(MathHelper.Clamp(value1.x, min.x, max.x), MathHelper.Clamp(value1.y, min.y, max.y));

    public static float Distance(Vec2 value1, Vec2 value2)
    {
      float num1 = value1.x - value2.x;
      float num2 = value1.y - value2.y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void Distance(ref Vec2 value1, ref Vec2 value2, out float result)
    {
      float num1 = value1.x - value2.x;
      float num2 = value1.y - value2.y;
      result = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static float DistanceSquared(Vec2 value1, Vec2 value2)
    {
      float num1 = value1.x - value2.x;
      float num2 = value1.y - value2.y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void DistanceSquared(ref Vec2 value1, ref Vec2 value2, out float result)
    {
      float num1 = value1.x - value2.x;
      float num2 = value1.y - value2.y;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static Vec2 Divide(Vec2 value1, Vec2 value2)
    {
      value1.x /= value2.x;
      value1.y /= value2.y;
      return value1;
    }

    public static void Divide(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = value1.x / value2.x;
      result.y = value1.y / value2.y;
    }

    public static Vec2 Divide(Vec2 value1, float divider)
    {
      float num = 1f / divider;
      value1.x *= num;
      value1.y *= num;
      return value1;
    }

    public static void Divide(ref Vec2 value1, float divider, out Vec2 result)
    {
      float num = 1f / divider;
      result.x = value1.x * num;
      result.y = value1.y * num;
    }

    public static float Dot(Vec2 value1, Vec2 value2) => (float) ((double) value1.x * (double) value2.x + (double) value1.y * (double) value2.y);

    public static void Dot(ref Vec2 value1, ref Vec2 value2, out float result) => result = (float) ((double) value1.x * (double) value2.x + (double) value1.y * (double) value2.y);

    public override bool Equals(object obj) => obj is Vec2 other && this.Equals(other);

    public bool Equals(Vec2 other) => (double) this.x == (double) other.x && (double) this.y == (double) other.y;

    public static Vec2 Reflect(Vec2 vector, Vec2 normal)
    {
      float num = (float) (2.0 * ((double) vector.x * (double) normal.x + (double) vector.y * (double) normal.y));
      Vec2 vec2;
      vec2.x = vector.x - normal.x * num;
      vec2.y = vector.y - normal.y * num;
      return vec2;
    }

    public static void Reflect(ref Vec2 vector, ref Vec2 normal, out Vec2 result)
    {
      float num = (float) (2.0 * ((double) vector.x * (double) normal.x + (double) vector.y * (double) normal.y));
      result.x = vector.x - normal.x * num;
      result.y = vector.y - normal.y * num;
    }

    public override int GetHashCode() => this.x.GetHashCode() + this.y.GetHashCode();

    public static Vec2 Hermite(
      Vec2 value1,
      Vec2 tangent1,
      Vec2 value2,
      Vec2 tangent2,
      float amount)
    {
      Vec2 result = new Vec2();
      Vec2.Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
      return result;
    }

    public static void Hermite(
      ref Vec2 value1,
      ref Vec2 tangent1,
      ref Vec2 value2,
      ref Vec2 tangent2,
      float amount,
      out Vec2 result)
    {
      result.x = MathHelper.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
      result.y = MathHelper.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
    }

    public float Length() => (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y);

    public float LengthSquared() => (float) ((double) this.x * (double) this.x + (double) this.y * (double) this.y);

    public static Vec2 Lerp(Vec2 value1, Vec2 value2, float amount) => DuckGame.Lerp.Vec2Smooth(value1, value2, amount);

    public static void Lerp(ref Vec2 value1, ref Vec2 value2, float amount, out Vec2 result) => result = new Vec2(MathHelper.Lerp(value1.x, value2.x, amount), MathHelper.Lerp(value1.y, value2.y, amount));

    public static Vec2 Max(Vec2 value1, Vec2 value2) => new Vec2((double) value1.x > (double) value2.x ? value1.x : value2.x, (double) value1.y > (double) value2.y ? value1.y : value2.y);

    public static void Max(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = (double) value1.x > (double) value2.x ? value1.x : value2.x;
      result.y = (double) value1.y > (double) value2.y ? value1.y : value2.y;
    }

    public static Vec2 Min(Vec2 value1, Vec2 value2) => new Vec2((double) value1.x < (double) value2.x ? value1.x : value2.x, (double) value1.y < (double) value2.y ? value1.y : value2.y);

    public static void Min(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = (double) value1.x < (double) value2.x ? value1.x : value2.x;
      result.y = (double) value1.y < (double) value2.y ? value1.y : value2.y;
    }

    public static Vec2 Multiply(Vec2 value1, Vec2 value2)
    {
      value1.x *= value2.x;
      value1.y *= value2.y;
      return value1;
    }

    public static Vec2 Multiply(Vec2 value1, float scaleFactor)
    {
      value1.x *= scaleFactor;
      value1.y *= scaleFactor;
      return value1;
    }

    public static void Multiply(ref Vec2 value1, float scaleFactor, out Vec2 result)
    {
      result.x = value1.x * scaleFactor;
      result.y = value1.y * scaleFactor;
    }

    public static void Multiply(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = value1.x * value2.x;
      result.y = value1.y * value2.y;
    }

    public static Vec2 Negate(Vec2 value)
    {
      value.x = -value.x;
      value.y = -value.y;
      return value;
    }

    public static void Negate(ref Vec2 value, out Vec2 result)
    {
      result.x = -value.x;
      result.y = -value.y;
    }

    public void Normalize()
    {
      float num1 = (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y);
      if ((double) num1 == 0.0)
        return;
      float num2 = 1f / num1;
      this.x *= num2;
      this.y *= num2;
    }

    public Vec2 normalized
    {
      get
      {
        float num1 = (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y);
        if ((double) num1 == 0.0)
          return Vec2.Zero;
        float num2 = 1f / num1;
        return new Vec2(this.x * num2, this.y * num2);
      }
    }

    public static Vec2 Normalize(Vec2 value)
    {
      float num1 = (float) Math.Sqrt((double) value.x * (double) value.x + (double) value.y * (double) value.y);
      if ((double) num1 != 0.0)
      {
        float num2 = 1f / num1;
        value.x *= num2;
        value.y *= num2;
      }
      return value;
    }

    public static void Normalize(ref Vec2 value, out Vec2 result)
    {
      float num1 = (float) Math.Sqrt((double) value.x * (double) value.x + (double) value.y * (double) value.y);
      if ((double) num1 != 0.0)
      {
        float num2 = 1f / num1;
        result.x = value.x * num2;
        result.y = value.y * num2;
      }
      else
        result = Vec2.Zero;
    }

    public static Vec2 SmoothStep(Vec2 value1, Vec2 value2, float amount) => new Vec2(MathHelper.SmoothStep(value1.x, value2.x, amount), MathHelper.SmoothStep(value1.y, value2.y, amount));

    public static void SmoothStep(ref Vec2 value1, ref Vec2 value2, float amount, out Vec2 result) => result = new Vec2(MathHelper.SmoothStep(value1.x, value2.x, amount), MathHelper.SmoothStep(value1.y, value2.y, amount));

    public static Vec2 Subtract(Vec2 value1, Vec2 value2)
    {
      value1.x -= value2.x;
      value1.y -= value2.y;
      return value1;
    }

    public static void Subtract(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
    {
      result.x = value1.x - value2.x;
      result.y = value1.y - value2.y;
    }

    public static Vec2 Transform(Vec2 position, Matrix matrix)
    {
      Vec2.Transform(ref position, ref matrix, out position);
      return position;
    }

    public static void Transform(ref Vec2 position, ref Matrix matrix, out Vec2 result) => result = new Vec2((float) ((double) position.x * (double) matrix.M11 + (double) position.y * (double) matrix.M21) + matrix.M41, (float) ((double) position.x * (double) matrix.M12 + (double) position.y * (double) matrix.M22) + matrix.M42);

    public static Vec2 Transform(Vec2 position, Quaternion quat)
    {
      Vec2.Transform(ref position, ref quat, out position);
      return position;
    }

    public static void Transform(ref Vec2 position, ref Quaternion quat, out Vec2 result)
    {
      Quaternion result1 = new Quaternion(position.x, position.y, 0.0f, 0.0f);
      Quaternion result2;
      Quaternion.Inverse(ref quat, out result2);
      Quaternion result3;
      Quaternion.Multiply(ref quat, ref result1, out result3);
      Quaternion.Multiply(ref result3, ref result2, out result1);
      result = new Vec2(result1.x, result1.y);
    }

    public static void Transform(Vec2[] sourceArray, ref Matrix matrix, Vec2[] destinationArray) => Vec2.Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);

    public static void Transform(
      Vec2[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vec2[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (int index = 0; index < length; ++index)
      {
        Vec2 source = sourceArray[sourceIndex + index];
        Vec2 destination = destinationArray[destinationIndex + index];
        destination.x = (float) ((double) source.x * (double) matrix.M11 + (double) source.y * (double) matrix.M21) + matrix.M41;
        destination.y = (float) ((double) source.x * (double) matrix.M12 + (double) source.y * (double) matrix.M22) + matrix.M42;
        destinationArray[destinationIndex + index] = destination;
      }
    }

    public static Vec2 TransformNormal(Vec2 normal, Matrix matrix)
    {
      Vec2.TransformNormal(ref normal, ref matrix, out normal);
      return normal;
    }

    public static void TransformNormal(ref Vec2 normal, ref Matrix matrix, out Vec2 result) => result = new Vec2((float) ((double) normal.x * (double) matrix.M11 + (double) normal.y * (double) matrix.M21), (float) ((double) normal.x * (double) matrix.M12 + (double) normal.y * (double) matrix.M22));

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{x:{0} y:{1}}}", new object[2]
      {
        (object) this.x.ToString((IFormatProvider) currentCulture),
        (object) this.y.ToString((IFormatProvider) currentCulture)
      });
    }

    public static Vec2 operator -(Vec2 value)
    {
      value.x = -value.x;
      value.y = -value.y;
      return value;
    }

    public static bool operator ==(Vec2 value1, Vec2 value2) => (double) value1.x == (double) value2.x && (double) value1.y == (double) value2.y;

    public static bool operator !=(Vec2 value1, Vec2 value2) => (double) value1.x != (double) value2.x || (double) value1.y != (double) value2.y;

    public static Vec2 operator +(Vec2 value1, Vec2 value2)
    {
      value1.x += value2.x;
      value1.y += value2.y;
      return value1;
    }

    public static Vec2 operator -(Vec2 value1, Vec2 value2)
    {
      value1.x -= value2.x;
      value1.y -= value2.y;
      return value1;
    }

    public static Vec2 operator *(Vec2 value1, Vec2 value2)
    {
      value1.x *= value2.x;
      value1.y *= value2.y;
      return value1;
    }

    public static Vec2 operator *(Vec2 value, float scaleFactor)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      return value;
    }

    public static Vec2 operator *(float scaleFactor, Vec2 value)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      return value;
    }

    public static Vec2 operator /(Vec2 value1, Vec2 value2)
    {
      value1.x /= value2.x;
      value1.y /= value2.y;
      return value1;
    }

    public static Vec2 operator /(Vec2 value1, float divider)
    {
      float num = 1f / divider;
      value1.x *= num;
      value1.y *= num;
      return value1;
    }

    public Vec2 Rotate(float radians, Vec2 pivot)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      Vec2 vec2 = new Vec2();
      vec2.x = this.x - pivot.x;
      vec2.y = this.y - pivot.y;
      return new Vec2()
      {
        x = (float) ((double) vec2.x * (double) num1 - (double) vec2.y * (double) num2) + pivot.x,
        y = (float) ((double) vec2.x * (double) num2 + (double) vec2.y * (double) num1) + pivot.y
      };
    }

    public static implicit operator Vector2(Vec2 vec) => new Vector2(vec.x, vec.y);

    public static implicit operator Vec2(Vector2 vec) => new Vec2(vec.X, vec.Y);
  }
}
