﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Quaternion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Text;

namespace DuckGame
{
  [Serializable]
  public struct Quaternion : IEquatable<Quaternion>
  {
    public float x;
    public float y;
    public float z;
    public float w;
    private static Quaternion identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);

    public Quaternion(float x, float y, float z, float w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public Quaternion(Vec3 vectorPart, float scalarPart)
    {
      this.x = vectorPart.x;
      this.y = vectorPart.y;
      this.z = vectorPart.z;
      this.w = scalarPart;
    }

    public static Quaternion Identity => Quaternion.identity;

    public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
    {
      quaternion1.x += quaternion2.x;
      quaternion1.y += quaternion2.y;
      quaternion1.z += quaternion2.z;
      quaternion1.w += quaternion2.w;
      return quaternion1;
    }

    public static void Add(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      out Quaternion result)
    {
      result.w = quaternion1.w + quaternion2.w;
      result.x = quaternion1.x + quaternion2.x;
      result.y = quaternion1.y + quaternion2.y;
      result.z = quaternion1.z + quaternion2.z;
    }

    public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
    {
      Quaternion quaternion;
      quaternion.x = (float) ((double) value2.x * (double) value1.w + (double) value1.x * (double) value2.w + (double) value2.y * (double) value1.z - (double) value2.z * (double) value1.y);
      quaternion.y = (float) ((double) value2.y * (double) value1.w + (double) value1.y * (double) value2.w + (double) value2.z * (double) value1.x - (double) value2.x * (double) value1.z);
      quaternion.z = (float) ((double) value2.z * (double) value1.w + (double) value1.z * (double) value2.w + (double) value2.x * (double) value1.y - (double) value2.y * (double) value1.x);
      quaternion.w = (float) ((double) value2.w * (double) value1.w - ((double) value2.x * (double) value1.x + (double) value2.y * (double) value1.y) + (double) value2.z * (double) value1.z);
      return quaternion;
    }

    public void Conjugate()
    {
      this.x = -this.x;
      this.y = -this.y;
      this.z = -this.z;
    }

    public static Quaternion Conjugate(Quaternion value)
    {
      Quaternion quaternion;
      quaternion.x = -value.x;
      quaternion.y = -value.y;
      quaternion.z = -value.z;
      quaternion.w = value.w;
      return quaternion;
    }

    public static void Conjugate(ref Quaternion value, out Quaternion result)
    {
      result.x = -value.x;
      result.y = -value.y;
      result.z = -value.z;
      result.w = value.w;
    }

    public static void Concatenate(
      ref Quaternion value1,
      ref Quaternion value2,
      out Quaternion result)
    {
      result.x = (float) ((double) value2.x * (double) value1.w + (double) value1.x * (double) value2.w + (double) value2.y * (double) value1.z - (double) value2.z * (double) value1.y);
      result.y = (float) ((double) value2.y * (double) value1.w + (double) value1.y * (double) value2.w + (double) value2.z * (double) value1.x - (double) value2.x * (double) value1.z);
      result.z = (float) ((double) value2.z * (double) value1.w + (double) value1.z * (double) value2.w + (double) value2.x * (double) value1.y - (double) value2.y * (double) value1.x);
      result.w = (float) ((double) value2.w * (double) value1.w - ((double) value2.x * (double) value1.x + (double) value2.y * (double) value1.y) + (double) value2.z * (double) value1.z);
    }

    public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      Quaternion quaternion;
      quaternion.x = (float) (Math.Cos((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) + Math.Sin((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
      quaternion.y = (float) (Math.Sin((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) - Math.Cos((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
      quaternion.z = (float) (Math.Cos((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Sin((double) roll * 0.5) - Math.Sin((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Cos((double) roll * 0.5));
      quaternion.w = (float) (Math.Cos((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) + Math.Sin((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
      return quaternion;
    }

    public static void CreateFromYawPitchRoll(
      float yaw,
      float pitch,
      float roll,
      out Quaternion result)
    {
      result.x = (float) (Math.Cos((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) + Math.Sin((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
      result.y = (float) (Math.Sin((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) - Math.Cos((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
      result.z = (float) (Math.Cos((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Sin((double) roll * 0.5) - Math.Sin((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Cos((double) roll * 0.5));
      result.w = (float) (Math.Cos((double) yaw * 0.5) * Math.Cos((double) pitch * 0.5) * Math.Cos((double) roll * 0.5) + Math.Sin((double) yaw * 0.5) * Math.Sin((double) pitch * 0.5) * Math.Sin((double) roll * 0.5));
    }

    public static Quaternion CreateFromAxisAngle(Vec3 axis, float angle)
    {
      float num = (float) Math.Sin((double) angle / 2.0);
      return new Quaternion(axis.x * num, axis.y * num, axis.z * num, (float) Math.Cos((double) angle / 2.0));
    }

    public static void CreateFromAxisAngle(ref Vec3 axis, float angle, out Quaternion result)
    {
      float num = (float) Math.Sin((double) angle / 2.0);
      result.x = axis.x * num;
      result.y = axis.y * num;
      result.z = axis.z * num;
      result.w = (float) Math.Cos((double) angle / 2.0);
    }

    public static Quaternion CreateFromRotationMatrix(Matrix matrix)
    {
      if ((double) matrix.M11 + (double) matrix.M22 + (double) matrix.M33 > 0.0)
      {
        float num1 = (float) Math.Sqrt((double) matrix.M11 + (double) matrix.M22 + (double) matrix.M33 + 1.0);
        Quaternion quaternion;
        quaternion.w = num1 * 0.5f;
        float num2 = 0.5f / num1;
        quaternion.x = (matrix.M23 - matrix.M32) * num2;
        quaternion.y = (matrix.M31 - matrix.M13) * num2;
        quaternion.z = (matrix.M12 - matrix.M21) * num2;
        return quaternion;
      }
      if ((double) matrix.M11 >= (double) matrix.M22 && (double) matrix.M11 >= (double) matrix.M33)
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        float num2 = 0.5f / num1;
        Quaternion quaternion;
        quaternion.x = 0.5f * num1;
        quaternion.y = (matrix.M12 + matrix.M21) * num2;
        quaternion.z = (matrix.M13 + matrix.M31) * num2;
        quaternion.w = (matrix.M23 - matrix.M32) * num2;
        return quaternion;
      }
      if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        float num2 = 0.5f / num1;
        Quaternion quaternion;
        quaternion.x = (matrix.M21 + matrix.M12) * num2;
        quaternion.y = 0.5f * num1;
        quaternion.z = (matrix.M32 + matrix.M23) * num2;
        quaternion.w = (matrix.M31 - matrix.M13) * num2;
        return quaternion;
      }
      float num3 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
      float num4 = 0.5f / num3;
      Quaternion quaternion1;
      quaternion1.x = (matrix.M31 + matrix.M13) * num4;
      quaternion1.y = (matrix.M32 + matrix.M23) * num4;
      quaternion1.z = 0.5f * num3;
      quaternion1.w = (matrix.M12 - matrix.M21) * num4;
      return quaternion1;
    }

    public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
    {
      if ((double) matrix.M11 + (double) matrix.M22 + (double) matrix.M33 > 0.0)
      {
        float num1 = (float) Math.Sqrt((double) matrix.M11 + (double) matrix.M22 + (double) matrix.M33 + 1.0);
        result.w = num1 * 0.5f;
        float num2 = 0.5f / num1;
        result.x = (matrix.M23 - matrix.M32) * num2;
        result.y = (matrix.M31 - matrix.M13) * num2;
        result.z = (matrix.M12 - matrix.M21) * num2;
      }
      else if ((double) matrix.M11 >= (double) matrix.M22 && (double) matrix.M11 >= (double) matrix.M33)
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        float num2 = 0.5f / num1;
        result.x = 0.5f * num1;
        result.y = (matrix.M12 + matrix.M21) * num2;
        result.z = (matrix.M13 + matrix.M31) * num2;
        result.w = (matrix.M23 - matrix.M32) * num2;
      }
      else if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        float num2 = 0.5f / num1;
        result.x = (matrix.M21 + matrix.M12) * num2;
        result.y = 0.5f * num1;
        result.z = (matrix.M32 + matrix.M23) * num2;
        result.w = (matrix.M31 - matrix.M13) * num2;
      }
      else
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
        float num2 = 0.5f / num1;
        result.x = (matrix.M31 + matrix.M13) * num2;
        result.y = (matrix.M32 + matrix.M23) * num2;
        result.z = 0.5f * num1;
        result.w = (matrix.M12 - matrix.M21) * num2;
      }
    }

    public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = (float) (1.0 / ((double) quaternion2.x * (double) quaternion2.x + (double) quaternion2.y * (double) quaternion2.y + (double) quaternion2.z * (double) quaternion2.z + (double) quaternion2.w * (double) quaternion2.w));
      float num2 = -quaternion2.x * num1;
      float num3 = -quaternion2.y * num1;
      float num4 = -quaternion2.z * num1;
      float num5 = quaternion2.w * num1;
      Quaternion quaternion;
      quaternion.x = (float) ((double) quaternion1.x * (double) num5 + (double) num2 * (double) quaternion1.w + ((double) quaternion1.y * (double) num4 - (double) quaternion1.z * (double) num3));
      quaternion.y = (float) ((double) quaternion1.y * (double) num5 + (double) num3 * (double) quaternion1.w + ((double) quaternion1.z * (double) num2 - (double) quaternion1.x * (double) num4));
      quaternion.z = (float) ((double) quaternion1.z * (double) num5 + (double) num4 * (double) quaternion1.w + ((double) quaternion1.x * (double) num3 - (double) quaternion1.y * (double) num2));
      quaternion.w = (float) ((double) quaternion1.w * (double) quaternion2.w * (double) num1 - ((double) quaternion1.x * (double) num2 + (double) quaternion1.y * (double) num3 + (double) quaternion1.z * (double) num4));
      return quaternion;
    }

    public static void Divide(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      out Quaternion result)
    {
      float num1 = (float) (1.0 / ((double) quaternion2.x * (double) quaternion2.x + (double) quaternion2.y * (double) quaternion2.y + (double) quaternion2.z * (double) quaternion2.z + (double) quaternion2.w * (double) quaternion2.w));
      float num2 = -quaternion2.x * num1;
      float num3 = -quaternion2.y * num1;
      float num4 = -quaternion2.z * num1;
      float num5 = quaternion2.w * num1;
      result.x = (float) ((double) quaternion1.x * (double) num5 + (double) num2 * (double) quaternion1.w + ((double) quaternion1.y * (double) num4 - (double) quaternion1.z * (double) num3));
      result.y = (float) ((double) quaternion1.y * (double) num5 + (double) num3 * (double) quaternion1.w + ((double) quaternion1.z * (double) num2 - (double) quaternion1.x * (double) num4));
      result.z = (float) ((double) quaternion1.z * (double) num5 + (double) num4 * (double) quaternion1.w + ((double) quaternion1.x * (double) num3 - (double) quaternion1.y * (double) num2));
      result.w = (float) ((double) quaternion1.w * (double) quaternion2.w * (double) num1 - ((double) quaternion1.x * (double) num2 + (double) quaternion1.y * (double) num3 + (double) quaternion1.z * (double) num4));
    }

    public static float Dot(Quaternion quaternion1, Quaternion quaternion2) => (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w);

    public static void Dot(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      out float result)
    {
      result = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w);
    }

    public override bool Equals(object obj) => obj is Quaternion quaternion && this == quaternion;

    public bool Equals(Quaternion other) => (double) this.x == (double) other.x && (double) this.y == (double) other.y && (double) this.z == (double) other.z && (double) this.w == (double) other.w;

    public override int GetHashCode() => this.x.GetHashCode() + this.y.GetHashCode() + this.z.GetHashCode() + this.w.GetHashCode();

    public static Quaternion Inverse(Quaternion quaternion)
    {
      float num = (float) (1.0 / ((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z + (double) quaternion.w * (double) quaternion.w));
      Quaternion quaternion1;
      quaternion1.x = -quaternion.x * num;
      quaternion1.y = -quaternion.y * num;
      quaternion1.z = -quaternion.z * num;
      quaternion1.w = quaternion.w * num;
      return quaternion1;
    }

    public static void Inverse(ref Quaternion quaternion, out Quaternion result)
    {
      float num = (float) (1.0 / ((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z + (double) quaternion.w * (double) quaternion.w));
      result.x = -quaternion.x * num;
      result.y = -quaternion.y * num;
      result.z = -quaternion.z * num;
      result.w = quaternion.w * num;
    }

    public float Length() => (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y + (double) this.z * (double) this.z + (double) this.w * (double) this.w);

    public float LengthSquared() => (float) ((double) this.x * (double) this.x + (double) this.y * (double) this.y + (double) this.z * (double) this.z + (double) this.w * (double) this.w);

    public static Quaternion Lerp(
      Quaternion quaternion1,
      Quaternion quaternion2,
      float amount)
    {
      float num1 = 1f - amount;
      Quaternion quaternion;
      if ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w >= 0.0)
      {
        quaternion.x = (float) ((double) num1 * (double) quaternion1.x + (double) amount * (double) quaternion2.x);
        quaternion.y = (float) ((double) num1 * (double) quaternion1.y + (double) amount * (double) quaternion2.y);
        quaternion.z = (float) ((double) num1 * (double) quaternion1.z + (double) amount * (double) quaternion2.z);
        quaternion.w = (float) ((double) num1 * (double) quaternion1.w + (double) amount * (double) quaternion2.w);
      }
      else
      {
        quaternion.x = (float) ((double) num1 * (double) quaternion1.x - (double) amount * (double) quaternion2.x);
        quaternion.y = (float) ((double) num1 * (double) quaternion1.y - (double) amount * (double) quaternion2.y);
        quaternion.z = (float) ((double) num1 * (double) quaternion1.z - (double) amount * (double) quaternion2.z);
        quaternion.w = (float) ((double) num1 * (double) quaternion1.w - (double) amount * (double) quaternion2.w);
      }
      float num2 = 1f / (float) Math.Sqrt((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z + (double) quaternion.w * (double) quaternion.w);
      quaternion.x *= num2;
      quaternion.y *= num2;
      quaternion.z *= num2;
      quaternion.w *= num2;
      return quaternion;
    }

    public static void Lerp(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      float amount,
      out Quaternion result)
    {
      float num1 = 1f - amount;
      if ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w >= 0.0)
      {
        result.x = (float) ((double) num1 * (double) quaternion1.x + (double) amount * (double) quaternion2.x);
        result.y = (float) ((double) num1 * (double) quaternion1.y + (double) amount * (double) quaternion2.y);
        result.z = (float) ((double) num1 * (double) quaternion1.z + (double) amount * (double) quaternion2.z);
        result.w = (float) ((double) num1 * (double) quaternion1.w + (double) amount * (double) quaternion2.w);
      }
      else
      {
        result.x = (float) ((double) num1 * (double) quaternion1.x - (double) amount * (double) quaternion2.x);
        result.y = (float) ((double) num1 * (double) quaternion1.y - (double) amount * (double) quaternion2.y);
        result.z = (float) ((double) num1 * (double) quaternion1.z - (double) amount * (double) quaternion2.z);
        result.w = (float) ((double) num1 * (double) quaternion1.w - (double) amount * (double) quaternion2.w);
      }
      float num2 = 1f / (float) Math.Sqrt((double) result.x * (double) result.x + (double) result.y * (double) result.y + (double) result.z * (double) result.z + (double) result.w * (double) result.w);
      result.x *= num2;
      result.y *= num2;
      result.z *= num2;
      result.w *= num2;
    }

    public static Quaternion Slerp(
      Quaternion quaternion1,
      Quaternion quaternion2,
      float amount)
    {
      float num1 = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w);
      bool flag = false;
      if ((double) num1 < 0.0)
      {
        flag = true;
        num1 = -num1;
      }
      float num2;
      float num3;
      if ((double) num1 > 0.999998986721039)
      {
        num2 = 1f - amount;
        num3 = flag ? -amount : amount;
      }
      else
      {
        float num4 = (float) Math.Acos((double) num1);
        float num5 = (float) (1.0 / Math.Sin((double) num4));
        num2 = (float) Math.Sin((1.0 - (double) amount) * (double) num4) * num5;
        num3 = flag ? (float) -Math.Sin((double) amount * (double) num4) * num5 : (float) Math.Sin((double) amount * (double) num4) * num5;
      }
      Quaternion quaternion;
      quaternion.x = (float) ((double) num2 * (double) quaternion1.x + (double) num3 * (double) quaternion2.x);
      quaternion.y = (float) ((double) num2 * (double) quaternion1.y + (double) num3 * (double) quaternion2.y);
      quaternion.z = (float) ((double) num2 * (double) quaternion1.z + (double) num3 * (double) quaternion2.z);
      quaternion.w = (float) ((double) num2 * (double) quaternion1.w + (double) num3 * (double) quaternion2.w);
      return quaternion;
    }

    public static void Slerp(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      float amount,
      out Quaternion result)
    {
      float num1 = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w);
      bool flag = false;
      if ((double) num1 < 0.0)
      {
        flag = true;
        num1 = -num1;
      }
      float num2;
      float num3;
      if ((double) num1 > 0.999998986721039)
      {
        num2 = 1f - amount;
        num3 = flag ? -amount : amount;
      }
      else
      {
        float num4 = (float) Math.Acos((double) num1);
        float num5 = (float) (1.0 / Math.Sin((double) num4));
        num2 = (float) Math.Sin((1.0 - (double) amount) * (double) num4) * num5;
        num3 = flag ? (float) -Math.Sin((double) amount * (double) num4) * num5 : (float) Math.Sin((double) amount * (double) num4) * num5;
      }
      result.x = (float) ((double) num2 * (double) quaternion1.x + (double) num3 * (double) quaternion2.x);
      result.y = (float) ((double) num2 * (double) quaternion1.y + (double) num3 * (double) quaternion2.y);
      result.z = (float) ((double) num2 * (double) quaternion1.z + (double) num3 * (double) quaternion2.z);
      result.w = (float) ((double) num2 * (double) quaternion1.w + (double) num3 * (double) quaternion2.w);
    }

    public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
    {
      quaternion1.x -= quaternion2.x;
      quaternion1.y -= quaternion2.y;
      quaternion1.z -= quaternion2.z;
      quaternion1.w -= quaternion2.w;
      return quaternion1;
    }

    public static void Subtract(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      out Quaternion result)
    {
      result.x = quaternion1.x - quaternion2.x;
      result.y = quaternion1.y - quaternion2.y;
      result.z = quaternion1.z - quaternion2.z;
      result.w = quaternion1.w - quaternion2.w;
    }

    public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = (float) ((double) quaternion1.y * (double) quaternion2.z - (double) quaternion1.z * (double) quaternion2.y);
      float num2 = (float) ((double) quaternion1.z * (double) quaternion2.x - (double) quaternion1.x * (double) quaternion2.z);
      float num3 = (float) ((double) quaternion1.x * (double) quaternion2.y - (double) quaternion1.y * (double) quaternion2.x);
      float num4 = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z);
      Quaternion quaternion;
      quaternion.x = (float) ((double) quaternion1.x * (double) quaternion2.w + (double) quaternion2.x * (double) quaternion1.w) + num1;
      quaternion.y = (float) ((double) quaternion1.y * (double) quaternion2.w + (double) quaternion2.y * (double) quaternion1.w) + num2;
      quaternion.z = (float) ((double) quaternion1.z * (double) quaternion2.w + (double) quaternion2.z * (double) quaternion1.w) + num3;
      quaternion.w = quaternion1.w * quaternion2.w - num4;
      return quaternion;
    }

    public static Quaternion Multiply(Quaternion quaternion1, float scaleFactor)
    {
      quaternion1.x *= scaleFactor;
      quaternion1.y *= scaleFactor;
      quaternion1.z *= scaleFactor;
      quaternion1.w *= scaleFactor;
      return quaternion1;
    }

    public static void Multiply(
      ref Quaternion quaternion1,
      float scaleFactor,
      out Quaternion result)
    {
      result.x = quaternion1.x * scaleFactor;
      result.y = quaternion1.y * scaleFactor;
      result.z = quaternion1.z * scaleFactor;
      result.w = quaternion1.w * scaleFactor;
    }

    public static void Multiply(
      ref Quaternion quaternion1,
      ref Quaternion quaternion2,
      out Quaternion result)
    {
      float num1 = (float) ((double) quaternion1.y * (double) quaternion2.z - (double) quaternion1.z * (double) quaternion2.y);
      float num2 = (float) ((double) quaternion1.z * (double) quaternion2.x - (double) quaternion1.x * (double) quaternion2.z);
      float num3 = (float) ((double) quaternion1.x * (double) quaternion2.y - (double) quaternion1.y * (double) quaternion2.x);
      float num4 = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z);
      result.x = (float) ((double) quaternion1.x * (double) quaternion2.w + (double) quaternion2.x * (double) quaternion1.w) + num1;
      result.y = (float) ((double) quaternion1.y * (double) quaternion2.w + (double) quaternion2.y * (double) quaternion1.w) + num2;
      result.z = (float) ((double) quaternion1.z * (double) quaternion2.w + (double) quaternion2.z * (double) quaternion1.w) + num3;
      result.w = quaternion1.w * quaternion2.w - num4;
    }

    public static Quaternion Negate(Quaternion quaternion)
    {
      Quaternion quaternion1;
      quaternion1.x = -quaternion.x;
      quaternion1.y = -quaternion.y;
      quaternion1.z = -quaternion.z;
      quaternion1.w = -quaternion.w;
      return quaternion1;
    }

    public static void Negate(ref Quaternion quaternion, out Quaternion result)
    {
      result.x = -quaternion.x;
      result.y = -quaternion.y;
      result.z = -quaternion.z;
      result.w = -quaternion.w;
    }

    public void Normalize()
    {
      float num = 1f / (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y + (double) this.z * (double) this.z + (double) this.w * (double) this.w);
      this.x *= num;
      this.y *= num;
      this.z *= num;
      this.w *= num;
    }

    public static Quaternion Normalize(Quaternion quaternion)
    {
      float num = 1f / (float) Math.Sqrt((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z + (double) quaternion.w * (double) quaternion.w);
      Quaternion quaternion1;
      quaternion1.x = quaternion.x * num;
      quaternion1.y = quaternion.y * num;
      quaternion1.z = quaternion.z * num;
      quaternion1.w = quaternion.w * num;
      return quaternion1;
    }

    public static void Normalize(ref Quaternion quaternion, out Quaternion result)
    {
      float num = 1f / (float) Math.Sqrt((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z + (double) quaternion.w * (double) quaternion.w);
      result.x = quaternion.x * num;
      result.y = quaternion.y * num;
      result.z = quaternion.z * num;
      result.w = quaternion.w * num;
    }

    public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
    {
      quaternion1.x += quaternion2.x;
      quaternion1.y += quaternion2.y;
      quaternion1.z += quaternion2.z;
      quaternion1.w += quaternion2.w;
      return quaternion1;
    }

    public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = (float) (1.0 / ((double) quaternion2.x * (double) quaternion2.x + (double) quaternion2.y * (double) quaternion2.y + (double) quaternion2.z * (double) quaternion2.z + (double) quaternion2.w * (double) quaternion2.w));
      float num2 = -quaternion2.x * num1;
      float num3 = -quaternion2.y * num1;
      float num4 = -quaternion2.z * num1;
      float num5 = quaternion2.w * num1;
      Quaternion quaternion;
      quaternion.x = (float) ((double) quaternion1.x * (double) num5 + (double) num2 * (double) quaternion1.w + ((double) quaternion1.y * (double) num4 - (double) quaternion1.z * (double) num3));
      quaternion.y = (float) ((double) quaternion1.y * (double) num5 + (double) num3 * (double) quaternion1.w + ((double) quaternion1.z * (double) num2 - (double) quaternion1.x * (double) num4));
      quaternion.z = (float) ((double) quaternion1.z * (double) num5 + (double) num4 * (double) quaternion1.w + ((double) quaternion1.x * (double) num3 - (double) quaternion1.y * (double) num2));
      quaternion.w = (float) ((double) quaternion1.w * (double) quaternion2.w * (double) num1 - ((double) quaternion1.x * (double) num2 + (double) quaternion1.y * (double) num3 + (double) quaternion1.z * (double) num4));
      return quaternion;
    }

    public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2) => (double) quaternion1.x == (double) quaternion2.x && (double) quaternion1.y == (double) quaternion2.y && (double) quaternion1.z == (double) quaternion2.z && (double) quaternion1.w == (double) quaternion2.w;

    public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2) => (double) quaternion1.x != (double) quaternion2.x || (double) quaternion1.y != (double) quaternion2.y || (double) quaternion1.z != (double) quaternion2.z || (double) quaternion1.w != (double) quaternion2.w;

    public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
    {
      float num1 = (float) ((double) quaternion1.y * (double) quaternion2.z - (double) quaternion1.z * (double) quaternion2.y);
      float num2 = (float) ((double) quaternion1.z * (double) quaternion2.x - (double) quaternion1.x * (double) quaternion2.z);
      float num3 = (float) ((double) quaternion1.x * (double) quaternion2.y - (double) quaternion1.y * (double) quaternion2.x);
      float num4 = (float) ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z);
      Quaternion quaternion;
      quaternion.x = (float) ((double) quaternion1.x * (double) quaternion2.w + (double) quaternion2.x * (double) quaternion1.w) + num1;
      quaternion.y = (float) ((double) quaternion1.y * (double) quaternion2.w + (double) quaternion2.y * (double) quaternion1.w) + num2;
      quaternion.z = (float) ((double) quaternion1.z * (double) quaternion2.w + (double) quaternion2.z * (double) quaternion1.w) + num3;
      quaternion.w = quaternion1.w * quaternion2.w - num4;
      return quaternion;
    }

    public static Quaternion operator *(Quaternion quaternion1, float scaleFactor)
    {
      quaternion1.x *= scaleFactor;
      quaternion1.y *= scaleFactor;
      quaternion1.z *= scaleFactor;
      quaternion1.w *= scaleFactor;
      return quaternion1;
    }

    public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
    {
      quaternion1.x -= quaternion2.x;
      quaternion1.y -= quaternion2.y;
      quaternion1.z -= quaternion2.z;
      quaternion1.w -= quaternion2.w;
      return quaternion1;
    }

    public static Quaternion operator -(Quaternion quaternion)
    {
      quaternion.x = -quaternion.x;
      quaternion.y = -quaternion.y;
      quaternion.z = -quaternion.z;
      quaternion.w = -quaternion.w;
      return quaternion;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(32);
      stringBuilder.Append("{X:");
      stringBuilder.Append(this.x);
      stringBuilder.Append(" Y:");
      stringBuilder.Append(this.y);
      stringBuilder.Append(" Z:");
      stringBuilder.Append(this.z);
      stringBuilder.Append(" W:");
      stringBuilder.Append(this.w);
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }
  }
}
