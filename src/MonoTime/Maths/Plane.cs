﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Plane
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [Serializable]
  public struct Plane : IEquatable<Plane>
  {
    public float d;
    public Vec3 normal;

    public Plane(Vec4 value)
      : this(new Vec3(value.x, value.y, value.z), value.w)
    {
    }

    public Plane(Vec3 normal, float d)
    {
      this.normal = normal;
      this.d = d;
    }

    public Plane(Vec3 a, Vec3 b, Vec3 c)
    {
      Vec3 vector1 = Vec3.Cross(b - a, c - a);
      this.normal = Vec3.Normalize(vector1);
      this.d = -Vec3.Dot(vector1, a);
    }

    public Plane(float a, float b, float c, float d)
      : this(new Vec3(a, b, c), d)
    {
    }

    public float Dot(Vec4 value) => (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z + (double) this.d * (double) value.w);

    public void Dot(ref Vec4 value, out float result) => result = (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z + (double) this.d * (double) value.w);

    public float DotCoordinate(Vec3 value) => (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z) + this.d;

    public void DotCoordinate(ref Vec3 value, out float result) => result = (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z) + this.d;

    public float DotNormal(Vec3 value) => (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z);

    public void DotNormal(ref Vec3 value, out float result) => result = (float) ((double) this.normal.x * (double) value.x + (double) this.normal.y * (double) value.y + (double) this.normal.z * (double) value.z);

    public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result) => throw new NotImplementedException();

    public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result) => throw new NotImplementedException();

    public static Plane Transform(Plane plane, Quaternion rotation) => throw new NotImplementedException();

    public static Plane Transform(Plane plane, Matrix matrix) => throw new NotImplementedException();

    public void Normalize()
    {
      Vec3 normal = this.normal;
      this.normal = Vec3.Normalize(this.normal);
      this.d *= (float) Math.Sqrt((double) this.normal.x * (double) this.normal.x + (double) this.normal.y * (double) this.normal.y + (double) this.normal.z * (double) this.normal.z) / (float) Math.Sqrt((double) normal.x * (double) normal.x + (double) normal.y * (double) normal.y + (double) normal.z * (double) normal.z);
    }

    public static Plane Normalize(Plane value)
    {
      Plane result;
      Plane.Normalize(ref value, out result);
      return result;
    }

    public static void Normalize(ref Plane value, out Plane result)
    {
      result.normal = Vec3.Normalize(value.normal);
      float num = (float) Math.Sqrt((double) result.normal.x * (double) result.normal.x + (double) result.normal.y * (double) result.normal.y + (double) result.normal.z * (double) result.normal.z) / (float) Math.Sqrt((double) value.normal.x * (double) value.normal.x + (double) value.normal.y * (double) value.normal.y + (double) value.normal.z * (double) value.normal.z);
      result.d = value.d * num;
    }

    public static bool operator !=(Plane plane1, Plane plane2) => !plane1.Equals(plane2);

    public static bool operator ==(Plane plane1, Plane plane2) => plane1.Equals(plane2);

    public override bool Equals(object other) => other is Plane other1 && this.Equals(other1);

    public bool Equals(Plane other) => this.normal == other.normal && (double) this.d == (double) other.d;

    public override int GetHashCode() => this.normal.GetHashCode() ^ this.d.GetHashCode();

    public override string ToString() => string.Format("{{Normal:{0} D:{1}}}", (object) this.normal, (object) this.d);
  }
}
