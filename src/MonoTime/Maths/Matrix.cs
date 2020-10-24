// Decompiled with JetBrains decompiler
// Type: DuckGame.Matrix
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [Serializable]
  public struct Matrix : IEquatable<Matrix>
  {
    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;
    private static Matrix identity = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);

    public static Matrix Identity => Matrix.identity;

    public Vec3 Backward
    {
      get => new Vec3(this.M31, this.M32, this.M33);
      set
      {
        this.M31 = value.x;
        this.M32 = value.y;
        this.M33 = value.z;
      }
    }

    public Vec3 Down
    {
      get => new Vec3(-this.M21, -this.M22, -this.M23);
      set
      {
        this.M21 = -value.x;
        this.M22 = -value.y;
        this.M23 = -value.z;
      }
    }

    public Vec3 Forward
    {
      get => new Vec3(-this.M31, -this.M32, -this.M33);
      set
      {
        this.M31 = -value.x;
        this.M32 = -value.y;
        this.M33 = -value.z;
      }
    }

    public Vec3 Left
    {
      get => new Vec3(-this.M11, -this.M12, -this.M13);
      set
      {
        this.M11 = -value.x;
        this.M12 = -value.y;
        this.M13 = -value.z;
      }
    }

    public Vec3 Right
    {
      get => new Vec3(this.M11, this.M12, this.M13);
      set
      {
        this.M11 = value.x;
        this.M12 = value.y;
        this.M13 = value.z;
      }
    }

    public Vec3 Translation
    {
      get => new Vec3(this.M41, this.M42, this.M43);
      set
      {
        this.M41 = value.x;
        this.M42 = value.y;
        this.M43 = value.z;
      }
    }

    public Vec3 Up
    {
      get => new Vec3(this.M21, this.M22, this.M23);
      set
      {
        this.M21 = value.x;
        this.M22 = value.y;
        this.M23 = value.z;
      }
    }

    /// <summary>Constructor for 4x4 Matrix</summary>
    /// <param name="m11">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m12">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m13">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m14">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m21">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m22">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m23">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m24">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m31">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m32">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m33">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m34">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m41">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m42">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m43">
    /// A <see cref="T:System.Single" />
    /// </param>
    /// <param name="m44">
    /// A <see cref="T:System.Single" />
    /// </param>
    public Matrix(
      float m11,
      float m12,
      float m13,
      float m14,
      float m21,
      float m22,
      float m23,
      float m24,
      float m31,
      float m32,
      float m33,
      float m34,
      float m41,
      float m42,
      float m43,
      float m44)
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M13 = m13;
      this.M14 = m14;
      this.M21 = m21;
      this.M22 = m22;
      this.M23 = m23;
      this.M24 = m24;
      this.M31 = m31;
      this.M32 = m32;
      this.M33 = m33;
      this.M34 = m34;
      this.M41 = m41;
      this.M42 = m42;
      this.M43 = m43;
      this.M44 = m44;
    }

    public static Matrix CreateWorld(Vec3 position, Vec3 forward, Vec3 up)
    {
      Matrix result;
      Matrix.CreateWorld(ref position, ref forward, ref up, out result);
      return result;
    }

    public static void CreateWorld(
      ref Vec3 position,
      ref Vec3 forward,
      ref Vec3 up,
      out Matrix result)
    {
      Vec3 result1;
      Vec3.Normalize(ref forward, out result1);
      Vec3 result2;
      Vec3.Cross(ref forward, ref up, out result2);
      Vec3 result3;
      Vec3.Cross(ref result2, ref forward, out result3);
      result2.Normalize();
      result3.Normalize();
      result = new Matrix();
      result.Right = result2;
      result.Up = result3;
      result.Forward = result1;
      result.Translation = position;
      result.M44 = 1f;
    }

    public static Matrix CreateShadow(Vec3 lightDirection, Plane plane)
    {
      Matrix result;
      Matrix.CreateShadow(ref lightDirection, ref plane, out result);
      return result;
    }

    public static void CreateShadow(ref Vec3 lightDirection, ref Plane plane, out Matrix result)
    {
      Plane plane1 = Plane.Normalize(plane);
      float num = Vec3.Dot(plane1.normal, lightDirection);
      result.M11 = -1f * plane1.normal.x * lightDirection.x + num;
      result.M12 = -1f * plane1.normal.x * lightDirection.y;
      result.M13 = -1f * plane1.normal.x * lightDirection.z;
      result.M14 = 0.0f;
      result.M21 = -1f * plane1.normal.y * lightDirection.x;
      result.M22 = -1f * plane1.normal.y * lightDirection.y + num;
      result.M23 = -1f * plane1.normal.y * lightDirection.z;
      result.M24 = 0.0f;
      result.M31 = -1f * plane1.normal.z * lightDirection.x;
      result.M32 = -1f * plane1.normal.z * lightDirection.y;
      result.M33 = -1f * plane1.normal.z * lightDirection.z + num;
      result.M34 = 0.0f;
      result.M41 = -1f * plane1.d * lightDirection.x;
      result.M42 = -1f * plane1.d * lightDirection.y;
      result.M43 = -1f * plane1.d * lightDirection.z;
      result.M44 = num;
    }

    public static void CreateReflection(ref Plane value, out Matrix result)
    {
      Plane plane = Plane.Normalize(value);
      result.M11 = (float) (-2.0 * (double) plane.normal.x * (double) plane.normal.x + 1.0);
      result.M12 = -2f * plane.normal.x * plane.normal.y;
      result.M13 = -2f * plane.normal.x * plane.normal.z;
      result.M14 = 0.0f;
      result.M21 = -2f * plane.normal.y * plane.normal.x;
      result.M22 = (float) (-2.0 * (double) plane.normal.y * (double) plane.normal.y + 1.0);
      result.M23 = -2f * plane.normal.y * plane.normal.z;
      result.M24 = 0.0f;
      result.M31 = -2f * plane.normal.z * plane.normal.x;
      result.M32 = -2f * plane.normal.z * plane.normal.y;
      result.M33 = (float) (-2.0 * (double) plane.normal.z * (double) plane.normal.z + 1.0);
      result.M34 = 0.0f;
      result.M41 = -2f * plane.d * plane.normal.x;
      result.M42 = -2f * plane.d * plane.normal.y;
      result.M43 = -2f * plane.d * plane.normal.z;
      result.M44 = 1f;
    }

    public static Matrix CreateReflection(Plane value)
    {
      Matrix result;
      Matrix.CreateReflection(ref value, out result);
      return result;
    }

    public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix result2;
      Matrix.CreateFromQuaternion(ref result1, out result2);
      return result2;
    }

    public static void CreateFromYawPitchRoll(
      float yaw,
      float pitch,
      float roll,
      out Matrix result)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix.CreateFromQuaternion(ref result1, out result);
    }

    public static void Transform(ref Matrix value, ref Quaternion rotation, out Matrix result)
    {
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(rotation);
      Matrix.Multiply(ref value, ref fromQuaternion, out result);
    }

    public static Matrix Transform(Matrix value, Quaternion rotation)
    {
      Matrix result;
      Matrix.Transform(ref value, ref rotation, out result);
      return result;
    }

    public bool Decompose(out Vec3 scale, out Quaternion rotation, out Vec3 translation)
    {
      translation.x = this.M41;
      translation.y = this.M42;
      translation.z = this.M43;
      float num1 = Math.Sign(this.M11 * this.M12 * this.M13 * this.M14) >= 0 ? 1f : -1f;
      float num2 = Math.Sign(this.M21 * this.M22 * this.M23 * this.M24) >= 0 ? 1f : -1f;
      float num3 = Math.Sign(this.M31 * this.M32 * this.M33 * this.M34) >= 0 ? 1f : -1f;
      scale.x = num1 * (float) Math.Sqrt((double) this.M11 * (double) this.M11 + (double) this.M12 * (double) this.M12 + (double) this.M13 * (double) this.M13);
      scale.y = num2 * (float) Math.Sqrt((double) this.M21 * (double) this.M21 + (double) this.M22 * (double) this.M22 + (double) this.M23 * (double) this.M23);
      scale.z = num3 * (float) Math.Sqrt((double) this.M31 * (double) this.M31 + (double) this.M32 * (double) this.M32 + (double) this.M33 * (double) this.M33);
      if ((double) scale.x == 0.0 || (double) scale.y == 0.0 || (double) scale.z == 0.0)
      {
        rotation = Quaternion.Identity;
        return false;
      }
      Matrix matrix = new Matrix(this.M11 / scale.x, this.M12 / scale.x, this.M13 / scale.x, 0.0f, this.M21 / scale.y, this.M22 / scale.y, this.M23 / scale.y, 0.0f, this.M31 / scale.z, this.M32 / scale.z, this.M33 / scale.z, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
      rotation = Quaternion.CreateFromRotationMatrix(matrix);
      return true;
    }

    /// <summary>Adds second matrix to the first.</summary>
    /// <param name="matrix1">
    /// A <see cref="T:DuckGame.Matrix" />
    /// </param>
    /// <param name="matrix2">
    /// A <see cref="T:DuckGame.Matrix" />
    /// </param>
    /// <returns>
    /// A <see cref="T:DuckGame.Matrix" />
    /// </returns>
    public static Matrix Add(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 += matrix2.M11;
      matrix1.M12 += matrix2.M12;
      matrix1.M13 += matrix2.M13;
      matrix1.M14 += matrix2.M14;
      matrix1.M21 += matrix2.M21;
      matrix1.M22 += matrix2.M22;
      matrix1.M23 += matrix2.M23;
      matrix1.M24 += matrix2.M24;
      matrix1.M31 += matrix2.M31;
      matrix1.M32 += matrix2.M32;
      matrix1.M33 += matrix2.M33;
      matrix1.M34 += matrix2.M34;
      matrix1.M41 += matrix2.M41;
      matrix1.M42 += matrix2.M42;
      matrix1.M43 += matrix2.M43;
      matrix1.M44 += matrix2.M44;
      return matrix1;
    }

    /// <summary>Adds two Matrix and save to the result Matrix</summary>
    /// <param name="matrix1">
    /// A <see cref="T:DuckGame.Matrix" />
    /// </param>
    /// <param name="matrix2">
    /// A <see cref="T:DuckGame.Matrix" />
    /// </param>
    /// <param name="result">
    /// A <see cref="T:DuckGame.Matrix" />
    /// </param>
    public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = matrix1.M11 + matrix2.M11;
      result.M12 = matrix1.M12 + matrix2.M12;
      result.M13 = matrix1.M13 + matrix2.M13;
      result.M14 = matrix1.M14 + matrix2.M14;
      result.M21 = matrix1.M21 + matrix2.M21;
      result.M22 = matrix1.M22 + matrix2.M22;
      result.M23 = matrix1.M23 + matrix2.M23;
      result.M24 = matrix1.M24 + matrix2.M24;
      result.M31 = matrix1.M31 + matrix2.M31;
      result.M32 = matrix1.M32 + matrix2.M32;
      result.M33 = matrix1.M33 + matrix2.M33;
      result.M34 = matrix1.M34 + matrix2.M34;
      result.M41 = matrix1.M41 + matrix2.M41;
      result.M42 = matrix1.M42 + matrix2.M42;
      result.M43 = matrix1.M43 + matrix2.M43;
      result.M44 = matrix1.M44 + matrix2.M44;
    }

    public static Matrix CreateBillboard(
      Vec3 objectPosition,
      Vec3 cameraPosition,
      Vec3 cameraUpVector,
      Vec3? cameraForwardVector)
    {
      Matrix result;
      Matrix.CreateBillboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, cameraForwardVector, out result);
      return result;
    }

    public static void CreateBillboard(
      ref Vec3 objectPosition,
      ref Vec3 cameraPosition,
      ref Vec3 cameraUpVector,
      Vec3? cameraForwardVector,
      out Matrix result)
    {
      Vec3 vec3 = objectPosition - cameraPosition;
      Vec3 result1;
      Vec3.Normalize(ref vec3, out result1);
      Vec3 result2;
      Vec3.Normalize(ref cameraUpVector, out result2);
      Vec3 result3;
      Vec3.Cross(ref result1, ref result2, out result3);
      Vec3.Cross(ref result1, ref result3, out result2);
      result = Matrix.Identity;
      result.Backward = result1;
      result.Right = result3;
      result.Up = result2;
      result.Translation = vec3;
    }

    public static Matrix CreateConstrainedBillboard(
      Vec3 objectPosition,
      Vec3 cameraPosition,
      Vec3 rotateAxis,
      Vec3? cameraForwardVector,
      Vec3? objectForwardVector)
    {
      throw new NotImplementedException();
    }

    public static void CreateConstrainedBillboard(
      ref Vec3 objectPosition,
      ref Vec3 cameraPosition,
      ref Vec3 rotateAxis,
      Vec3? cameraForwardVector,
      Vec3? objectForwardVector,
      out Matrix result)
    {
      throw new NotImplementedException();
    }

    public static Matrix CreateFromAxisAngle(Vec3 axis, float angle) => throw new NotImplementedException();

    public static void CreateFromAxisAngle(ref Vec3 axis, float angle, out Matrix result) => throw new NotImplementedException();

    public static Matrix CreateFromQuaternion(Quaternion quaternion)
    {
      Matrix result;
      Matrix.CreateFromQuaternion(ref quaternion, out result);
      return result;
    }

    public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = (float) (1.0 - 2.0 * ((double) quaternion.y * (double) quaternion.y + (double) quaternion.z * (double) quaternion.z));
      result.M12 = (float) (2.0 * ((double) quaternion.x * (double) quaternion.y + (double) quaternion.w * (double) quaternion.z));
      result.M13 = (float) (2.0 * ((double) quaternion.x * (double) quaternion.z - (double) quaternion.w * (double) quaternion.y));
      result.M21 = (float) (2.0 * ((double) quaternion.x * (double) quaternion.y - (double) quaternion.w * (double) quaternion.z));
      result.M22 = (float) (1.0 - 2.0 * ((double) quaternion.x * (double) quaternion.x + (double) quaternion.z * (double) quaternion.z));
      result.M23 = (float) (2.0 * ((double) quaternion.y * (double) quaternion.z + (double) quaternion.w * (double) quaternion.x));
      result.M31 = (float) (2.0 * ((double) quaternion.x * (double) quaternion.z + (double) quaternion.w * (double) quaternion.y));
      result.M32 = (float) (2.0 * ((double) quaternion.y * (double) quaternion.z - (double) quaternion.w * (double) quaternion.x));
      result.M33 = (float) (1.0 - 2.0 * ((double) quaternion.x * (double) quaternion.x + (double) quaternion.y * (double) quaternion.y));
    }

    public static Matrix CreateLookAt(
      Vec3 cameraPosition,
      Vec3 cameraTarget,
      Vec3 cameraUpVector)
    {
      Matrix result;
      Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out result);
      return result;
    }

    public static void CreateLookAt(
      ref Vec3 cameraPosition,
      ref Vec3 cameraTarget,
      ref Vec3 cameraUpVector,
      out Matrix result)
    {
      Vec3 vec3_1 = Vec3.Normalize(cameraPosition - cameraTarget);
      Vec3 vec3_2 = Vec3.Normalize(Vec3.Cross(cameraUpVector, vec3_1));
      Vec3 vector1 = Vec3.Cross(vec3_1, vec3_2);
      result = Matrix.Identity;
      result.M11 = vec3_2.x;
      result.M12 = vector1.x;
      result.M13 = vec3_1.x;
      result.M21 = vec3_2.y;
      result.M22 = vector1.y;
      result.M23 = vec3_1.y;
      result.M31 = vec3_2.z;
      result.M32 = vector1.z;
      result.M33 = vec3_1.z;
      result.M41 = -Vec3.Dot(vec3_2, cameraPosition);
      result.M42 = -Vec3.Dot(vector1, cameraPosition);
      result.M43 = -Vec3.Dot(vec3_1, cameraPosition);
    }

    public static Matrix CreateOrthographic(
      float width,
      float height,
      float zNearPlane,
      float zFarPlane)
    {
      Matrix result;
      Matrix.CreateOrthographic(width, height, zNearPlane, zFarPlane, out result);
      return result;
    }

    public static void CreateOrthographic(
      float width,
      float height,
      float zNearPlane,
      float zFarPlane,
      out Matrix result)
    {
      result.M11 = 2f / width;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = 2f / height;
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      result.M34 = 0.0f;
      result.M41 = 0.0f;
      result.M42 = 0.0f;
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1f;
    }

    public static Matrix CreateOrthographicOffCenter(
      float left,
      float right,
      float bottom,
      float top,
      float zNearPlane,
      float zFarPlane)
    {
      Matrix result;
      Matrix.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane, out result);
      return result;
    }

    public static void CreateOrthographicOffCenter(
      float left,
      float right,
      float bottom,
      float top,
      float zNearPlane,
      float zFarPlane,
      out Matrix result)
    {
      result.M11 = (float) (2.0 / ((double) right - (double) left));
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M14 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = (float) (2.0 / ((double) top - (double) bottom));
      result.M23 = 0.0f;
      result.M24 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = (float) (1.0 / ((double) zNearPlane - (double) zFarPlane));
      result.M34 = 0.0f;
      result.M41 = (float) (((double) left + (double) right) / ((double) left - (double) right));
      result.M42 = (float) (((double) bottom + (double) top) / ((double) bottom - (double) top));
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1f;
    }

    public static Matrix CreatePerspective(
      float width,
      float height,
      float zNearPlane,
      float zFarPlane)
    {
      throw new NotImplementedException();
    }

    public static void CreatePerspective(
      float width,
      float height,
      float zNearPlane,
      float zFarPlane,
      out Matrix result)
    {
      throw new NotImplementedException();
    }

    public static Matrix CreatePerspectiveFieldOfView(
      float fieldOfView,
      float aspectRatio,
      float nearPlaneDistance,
      float farPlaneDistance)
    {
      Matrix result;
      Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out result);
      return result;
    }

    public static void CreatePerspectiveFieldOfView(
      float fieldOfView,
      float aspectRatio,
      float nearPlaneDistance,
      float farPlaneDistance,
      out Matrix result)
    {
      result = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
      if ((double) fieldOfView < 0.0 || (double) fieldOfView > 3.14159250259399)
        throw new ArgumentOutOfRangeException(nameof (fieldOfView), "fieldOfView takes a value between 0 and Pi (180 degrees) in radians.");
      if ((double) nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "You should specify positive value for nearPlaneDistance.");
      if ((double) farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), "You should specify positive value for farPlaneDistance.");
      if ((double) farPlaneDistance <= (double) nearPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "Near plane distance is larger than Far plane distance. Near plane distance must be smaller than Far plane distance.");
      float num1 = 1f / (float) Math.Tan((double) fieldOfView / 2.0);
      float num2 = num1 / aspectRatio;
      result.M11 = num2;
      result.M22 = num1;
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M34 = -1f;
      result.M43 = (float) ((double) nearPlaneDistance * (double) farPlaneDistance / ((double) nearPlaneDistance - (double) farPlaneDistance));
    }

    public static Matrix CreatePerspectiveOffCenter(
      float left,
      float right,
      float bottom,
      float top,
      float zNearPlane,
      float zFarPlane)
    {
      throw new NotImplementedException();
    }

    public static void CreatePerspectiveOffCenter(
      float left,
      float right,
      float bottom,
      float top,
      float nearPlaneDistance,
      float farPlaneDistance,
      out Matrix result)
    {
      throw new NotImplementedException();
    }

    public static Matrix CreateRotationX(float radians)
    {
      Matrix identity = Matrix.Identity;
      identity.M22 = (float) Math.Cos((double) radians);
      identity.M23 = (float) Math.Sin((double) radians);
      identity.M32 = -identity.M23;
      identity.M33 = identity.M22;
      return identity;
    }

    public static void CreateRotationX(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      result.M22 = (float) Math.Cos((double) radians);
      result.M23 = (float) Math.Sin((double) radians);
      result.M32 = -result.M23;
      result.M33 = result.M22;
    }

    public static Matrix CreateRotationY(float radians)
    {
      Matrix identity = Matrix.Identity;
      identity.M11 = (float) Math.Cos((double) radians);
      identity.M13 = (float) Math.Sin((double) radians);
      identity.M31 = -identity.M13;
      identity.M33 = identity.M11;
      return identity;
    }

    public static void CreateRotationY(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = (float) Math.Cos((double) radians);
      result.M13 = (float) Math.Sin((double) radians);
      result.M31 = -result.M13;
      result.M33 = result.M11;
    }

    public static Matrix CreateRotationZ(float radians)
    {
      Matrix identity = Matrix.Identity;
      identity.M11 = (float) Math.Cos((double) radians);
      identity.M12 = (float) Math.Sin((double) radians);
      identity.M21 = -identity.M12;
      identity.M22 = identity.M11;
      return identity;
    }

    public static void CreateRotationZ(float radians, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = (float) Math.Cos((double) radians);
      result.M12 = (float) Math.Sin((double) radians);
      result.M21 = -result.M12;
      result.M22 = result.M11;
    }

    public static Matrix CreateScale(float scale)
    {
      Matrix identity = Matrix.Identity;
      identity.M11 = scale;
      identity.M22 = scale;
      identity.M33 = scale;
      return identity;
    }

    public static void CreateScale(float scale, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = scale;
      result.M22 = scale;
      result.M33 = scale;
    }

    public static Matrix CreateScale(float xScale, float yScale, float zScale)
    {
      Matrix identity = Matrix.Identity;
      identity.M11 = xScale;
      identity.M22 = yScale;
      identity.M33 = zScale;
      return identity;
    }

    public static void CreateScale(float xScale, float yScale, float zScale, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = xScale;
      result.M22 = yScale;
      result.M33 = zScale;
    }

    public static Matrix CreateScale(Vec3 scales)
    {
      Matrix identity = Matrix.Identity;
      identity.M11 = scales.x;
      identity.M22 = scales.y;
      identity.M33 = scales.z;
      return identity;
    }

    public static void CreateScale(ref Vec3 scales, out Matrix result)
    {
      result = Matrix.Identity;
      result.M11 = scales.x;
      result.M22 = scales.y;
      result.M33 = scales.z;
    }

    public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
    {
      Matrix identity = Matrix.Identity;
      identity.M41 = xPosition;
      identity.M42 = yPosition;
      identity.M43 = zPosition;
      return identity;
    }

    public static void CreateTranslation(
      float xPosition,
      float yPosition,
      float zPosition,
      out Matrix result)
    {
      result = Matrix.Identity;
      result.M41 = xPosition;
      result.M42 = yPosition;
      result.M43 = zPosition;
    }

    public static Matrix CreateTranslation(Vec3 position)
    {
      Matrix identity = Matrix.Identity;
      identity.M41 = position.x;
      identity.M42 = position.y;
      identity.M43 = position.z;
      return identity;
    }

    public static void CreateTranslation(ref Vec3 position, out Matrix result)
    {
      result = Matrix.Identity;
      result.M41 = position.x;
      result.M42 = position.y;
      result.M43 = position.z;
    }

    public static Matrix Divide(Matrix matrix1, Matrix matrix2)
    {
      Matrix matrix3 = Matrix.Invert(matrix2);
      Matrix matrix4;
      matrix4.M11 = (float) ((double) matrix1.M11 * (double) matrix3.M11 + (double) matrix1.M12 * (double) matrix3.M21 + (double) matrix1.M13 * (double) matrix3.M31 + (double) matrix1.M14 * (double) matrix3.M41);
      matrix4.M12 = (float) ((double) matrix1.M11 * (double) matrix3.M12 + (double) matrix1.M12 * (double) matrix3.M22 + (double) matrix1.M13 * (double) matrix3.M32 + (double) matrix1.M14 * (double) matrix3.M42);
      matrix4.M13 = (float) ((double) matrix1.M11 * (double) matrix3.M13 + (double) matrix1.M12 * (double) matrix3.M23 + (double) matrix1.M13 * (double) matrix3.M33 + (double) matrix1.M14 * (double) matrix3.M43);
      matrix4.M14 = (float) ((double) matrix1.M11 * (double) matrix3.M14 + (double) matrix1.M12 * (double) matrix3.M24 + (double) matrix1.M13 * (double) matrix3.M34 + (double) matrix1.M14 * (double) matrix3.M44);
      matrix4.M21 = (float) ((double) matrix1.M21 * (double) matrix3.M11 + (double) matrix1.M22 * (double) matrix3.M21 + (double) matrix1.M23 * (double) matrix3.M31 + (double) matrix1.M24 * (double) matrix3.M41);
      matrix4.M22 = (float) ((double) matrix1.M21 * (double) matrix3.M12 + (double) matrix1.M22 * (double) matrix3.M22 + (double) matrix1.M23 * (double) matrix3.M32 + (double) matrix1.M24 * (double) matrix3.M42);
      matrix4.M23 = (float) ((double) matrix1.M21 * (double) matrix3.M13 + (double) matrix1.M22 * (double) matrix3.M23 + (double) matrix1.M23 * (double) matrix3.M33 + (double) matrix1.M24 * (double) matrix3.M43);
      matrix4.M24 = (float) ((double) matrix1.M21 * (double) matrix3.M14 + (double) matrix1.M22 * (double) matrix3.M24 + (double) matrix1.M23 * (double) matrix3.M34 + (double) matrix1.M24 * (double) matrix3.M44);
      matrix4.M31 = (float) ((double) matrix1.M31 * (double) matrix3.M11 + (double) matrix1.M32 * (double) matrix3.M21 + (double) matrix1.M33 * (double) matrix3.M31 + (double) matrix1.M34 * (double) matrix3.M41);
      matrix4.M32 = (float) ((double) matrix1.M31 * (double) matrix3.M12 + (double) matrix1.M32 * (double) matrix3.M22 + (double) matrix1.M33 * (double) matrix3.M32 + (double) matrix1.M34 * (double) matrix3.M42);
      matrix4.M33 = (float) ((double) matrix1.M31 * (double) matrix3.M13 + (double) matrix1.M32 * (double) matrix3.M23 + (double) matrix1.M33 * (double) matrix3.M33 + (double) matrix1.M34 * (double) matrix3.M43);
      matrix4.M34 = (float) ((double) matrix1.M31 * (double) matrix3.M14 + (double) matrix1.M32 * (double) matrix3.M24 + (double) matrix1.M33 * (double) matrix3.M34 + (double) matrix1.M34 * (double) matrix3.M44);
      matrix4.M41 = (float) ((double) matrix1.M41 * (double) matrix3.M11 + (double) matrix1.M42 * (double) matrix3.M21 + (double) matrix1.M43 * (double) matrix3.M31 + (double) matrix1.M44 * (double) matrix3.M41);
      matrix4.M42 = (float) ((double) matrix1.M41 * (double) matrix3.M12 + (double) matrix1.M42 * (double) matrix3.M22 + (double) matrix1.M43 * (double) matrix3.M32 + (double) matrix1.M44 * (double) matrix3.M42);
      matrix4.M43 = (float) ((double) matrix1.M41 * (double) matrix3.M13 + (double) matrix1.M42 * (double) matrix3.M23 + (double) matrix1.M43 * (double) matrix3.M33 + (double) matrix1.M44 * (double) matrix3.M43);
      matrix4.M44 = (float) ((double) matrix1.M41 * (double) matrix3.M14 + (double) matrix1.M42 * (double) matrix3.M24 + (double) matrix1.M43 * (double) matrix3.M34 + (double) matrix1.M44 * (double) matrix3.M44);
      return matrix4;
    }

    public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      Matrix matrix = Matrix.Invert(matrix2);
      result.M11 = (float) ((double) matrix1.M11 * (double) matrix.M11 + (double) matrix1.M12 * (double) matrix.M21 + (double) matrix1.M13 * (double) matrix.M31 + (double) matrix1.M14 * (double) matrix.M41);
      result.M12 = (float) ((double) matrix1.M11 * (double) matrix.M12 + (double) matrix1.M12 * (double) matrix.M22 + (double) matrix1.M13 * (double) matrix.M32 + (double) matrix1.M14 * (double) matrix.M42);
      result.M13 = (float) ((double) matrix1.M11 * (double) matrix.M13 + (double) matrix1.M12 * (double) matrix.M23 + (double) matrix1.M13 * (double) matrix.M33 + (double) matrix1.M14 * (double) matrix.M43);
      result.M14 = (float) ((double) matrix1.M11 * (double) matrix.M14 + (double) matrix1.M12 * (double) matrix.M24 + (double) matrix1.M13 * (double) matrix.M34 + (double) matrix1.M14 * (double) matrix.M44);
      result.M21 = (float) ((double) matrix1.M21 * (double) matrix.M11 + (double) matrix1.M22 * (double) matrix.M21 + (double) matrix1.M23 * (double) matrix.M31 + (double) matrix1.M24 * (double) matrix.M41);
      result.M22 = (float) ((double) matrix1.M21 * (double) matrix.M12 + (double) matrix1.M22 * (double) matrix.M22 + (double) matrix1.M23 * (double) matrix.M32 + (double) matrix1.M24 * (double) matrix.M42);
      result.M23 = (float) ((double) matrix1.M21 * (double) matrix.M13 + (double) matrix1.M22 * (double) matrix.M23 + (double) matrix1.M23 * (double) matrix.M33 + (double) matrix1.M24 * (double) matrix.M43);
      result.M24 = (float) ((double) matrix1.M21 * (double) matrix.M14 + (double) matrix1.M22 * (double) matrix.M24 + (double) matrix1.M23 * (double) matrix.M34 + (double) matrix1.M24 * (double) matrix.M44);
      result.M31 = (float) ((double) matrix1.M31 * (double) matrix.M11 + (double) matrix1.M32 * (double) matrix.M21 + (double) matrix1.M33 * (double) matrix.M31 + (double) matrix1.M34 * (double) matrix.M41);
      result.M32 = (float) ((double) matrix1.M31 * (double) matrix.M12 + (double) matrix1.M32 * (double) matrix.M22 + (double) matrix1.M33 * (double) matrix.M32 + (double) matrix1.M34 * (double) matrix.M42);
      result.M33 = (float) ((double) matrix1.M31 * (double) matrix.M13 + (double) matrix1.M32 * (double) matrix.M23 + (double) matrix1.M33 * (double) matrix.M33 + (double) matrix1.M34 * (double) matrix.M43);
      result.M34 = (float) ((double) matrix1.M31 * (double) matrix.M14 + (double) matrix1.M32 * (double) matrix.M24 + (double) matrix1.M33 * (double) matrix.M34 + (double) matrix1.M34 * (double) matrix.M44);
      result.M41 = (float) ((double) matrix1.M41 * (double) matrix.M11 + (double) matrix1.M42 * (double) matrix.M21 + (double) matrix1.M43 * (double) matrix.M31 + (double) matrix1.M44 * (double) matrix.M41);
      result.M42 = (float) ((double) matrix1.M41 * (double) matrix.M12 + (double) matrix1.M42 * (double) matrix.M22 + (double) matrix1.M43 * (double) matrix.M32 + (double) matrix1.M44 * (double) matrix.M42);
      result.M43 = (float) ((double) matrix1.M41 * (double) matrix.M13 + (double) matrix1.M42 * (double) matrix.M23 + (double) matrix1.M43 * (double) matrix.M33 + (double) matrix1.M44 * (double) matrix.M43);
      result.M44 = (float) ((double) matrix1.M41 * (double) matrix.M14 + (double) matrix1.M42 * (double) matrix.M24 + (double) matrix1.M43 * (double) matrix.M34 + (double) matrix1.M44 * (double) matrix.M44);
    }

    public static Matrix Divide(Matrix matrix1, float divider)
    {
      float num = 1f / divider;
      matrix1.M11 *= num;
      matrix1.M12 *= num;
      matrix1.M13 *= num;
      matrix1.M14 *= num;
      matrix1.M21 *= num;
      matrix1.M22 *= num;
      matrix1.M23 *= num;
      matrix1.M24 *= num;
      matrix1.M31 *= num;
      matrix1.M32 *= num;
      matrix1.M33 *= num;
      matrix1.M34 *= num;
      matrix1.M41 *= num;
      matrix1.M42 *= num;
      matrix1.M43 *= num;
      matrix1.M44 *= num;
      return matrix1;
    }

    public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
    {
      float num = 1f / divider;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M14 = matrix1.M14 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M24 = matrix1.M24 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
      result.M34 = matrix1.M34 * num;
      result.M41 = matrix1.M41 * num;
      result.M42 = matrix1.M42 * num;
      result.M43 = matrix1.M43 * num;
      result.M44 = matrix1.M44 * num;
    }

    public static Matrix Invert(Matrix matrix)
    {
      Matrix.Invert(ref matrix, out matrix);
      return matrix;
    }

    public static void Invert(ref Matrix matrix, out Matrix result)
    {
      float num1 = (float) ((double) matrix.M11 * (double) matrix.M22 - (double) matrix.M12 * (double) matrix.M21);
      float num2 = (float) ((double) matrix.M11 * (double) matrix.M23 - (double) matrix.M13 * (double) matrix.M21);
      float num3 = (float) ((double) matrix.M11 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M21);
      float num4 = (float) ((double) matrix.M12 * (double) matrix.M23 - (double) matrix.M13 * (double) matrix.M22);
      float num5 = (float) ((double) matrix.M12 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M22);
      float num6 = (float) ((double) matrix.M13 * (double) matrix.M24 - (double) matrix.M14 * (double) matrix.M23);
      float num7 = (float) ((double) matrix.M31 * (double) matrix.M42 - (double) matrix.M32 * (double) matrix.M41);
      float num8 = (float) ((double) matrix.M31 * (double) matrix.M43 - (double) matrix.M33 * (double) matrix.M41);
      float num9 = (float) ((double) matrix.M31 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M41);
      float num10 = (float) ((double) matrix.M32 * (double) matrix.M43 - (double) matrix.M33 * (double) matrix.M42);
      float num11 = (float) ((double) matrix.M32 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M42);
      float num12 = (float) ((double) matrix.M33 * (double) matrix.M44 - (double) matrix.M34 * (double) matrix.M43);
      float num13 = 1f / (float) ((double) num1 * (double) num12 - (double) num2 * (double) num11 + (double) num3 * (double) num10 + (double) num4 * (double) num9 - (double) num5 * (double) num8 + (double) num6 * (double) num7);
      Matrix matrix1;
      matrix1.M11 = (float) ((double) matrix.M22 * (double) num12 - (double) matrix.M23 * (double) num11 + (double) matrix.M24 * (double) num10) * num13;
      matrix1.M12 = (float) (-(double) matrix.M12 * (double) num12 + (double) matrix.M13 * (double) num11 - (double) matrix.M14 * (double) num10) * num13;
      matrix1.M13 = (float) ((double) matrix.M42 * (double) num6 - (double) matrix.M43 * (double) num5 + (double) matrix.M44 * (double) num4) * num13;
      matrix1.M14 = (float) (-(double) matrix.M32 * (double) num6 + (double) matrix.M33 * (double) num5 - (double) matrix.M34 * (double) num4) * num13;
      matrix1.M21 = (float) (-(double) matrix.M21 * (double) num12 + (double) matrix.M23 * (double) num9 - (double) matrix.M24 * (double) num8) * num13;
      matrix1.M22 = (float) ((double) matrix.M11 * (double) num12 - (double) matrix.M13 * (double) num9 + (double) matrix.M14 * (double) num8) * num13;
      matrix1.M23 = (float) (-(double) matrix.M41 * (double) num6 + (double) matrix.M43 * (double) num3 - (double) matrix.M44 * (double) num2) * num13;
      matrix1.M24 = (float) ((double) matrix.M31 * (double) num6 - (double) matrix.M33 * (double) num3 + (double) matrix.M34 * (double) num2) * num13;
      matrix1.M31 = (float) ((double) matrix.M21 * (double) num11 - (double) matrix.M22 * (double) num9 + (double) matrix.M24 * (double) num7) * num13;
      matrix1.M32 = (float) (-(double) matrix.M11 * (double) num11 + (double) matrix.M12 * (double) num9 - (double) matrix.M14 * (double) num7) * num13;
      matrix1.M33 = (float) ((double) matrix.M41 * (double) num5 - (double) matrix.M42 * (double) num3 + (double) matrix.M44 * (double) num1) * num13;
      matrix1.M34 = (float) (-(double) matrix.M31 * (double) num5 + (double) matrix.M32 * (double) num3 - (double) matrix.M34 * (double) num1) * num13;
      matrix1.M41 = (float) (-(double) matrix.M21 * (double) num10 + (double) matrix.M22 * (double) num8 - (double) matrix.M23 * (double) num7) * num13;
      matrix1.M42 = (float) ((double) matrix.M11 * (double) num10 - (double) matrix.M12 * (double) num8 + (double) matrix.M13 * (double) num7) * num13;
      matrix1.M43 = (float) (-(double) matrix.M41 * (double) num4 + (double) matrix.M42 * (double) num2 - (double) matrix.M43 * (double) num1) * num13;
      matrix1.M44 = (float) ((double) matrix.M31 * (double) num4 - (double) matrix.M32 * (double) num2 + (double) matrix.M33 * (double) num1) * num13;
      result = matrix1;
    }

    public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount) => throw new NotImplementedException();

    public static void Lerp(
      ref Matrix matrix1,
      ref Matrix matrix2,
      float amount,
      out Matrix result)
    {
      throw new NotImplementedException();
    }

    public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
    {
      Matrix matrix;
      matrix.M11 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      matrix.M12 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      matrix.M13 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      matrix.M14 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      matrix.M21 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      matrix.M22 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      matrix.M23 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      matrix.M24 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      matrix.M31 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      matrix.M32 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      matrix.M33 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      matrix.M34 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      matrix.M41 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      matrix.M42 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      matrix.M43 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      matrix.M44 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
      return matrix;
    }

    public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      result.M12 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      result.M13 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      result.M14 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      result.M21 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      result.M22 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      result.M23 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      result.M24 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      result.M31 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      result.M32 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      result.M33 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      result.M34 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      result.M41 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      result.M42 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      result.M43 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      result.M44 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
    }

    public static Matrix Multiply(Matrix matrix1, float factor)
    {
      matrix1.M11 *= factor;
      matrix1.M12 *= factor;
      matrix1.M13 *= factor;
      matrix1.M14 *= factor;
      matrix1.M21 *= factor;
      matrix1.M22 *= factor;
      matrix1.M23 *= factor;
      matrix1.M24 *= factor;
      matrix1.M31 *= factor;
      matrix1.M32 *= factor;
      matrix1.M33 *= factor;
      matrix1.M34 *= factor;
      matrix1.M41 *= factor;
      matrix1.M42 *= factor;
      matrix1.M43 *= factor;
      matrix1.M44 *= factor;
      return matrix1;
    }

    public static void Multiply(ref Matrix matrix1, float factor, out Matrix result)
    {
      result.M11 = matrix1.M11 * factor;
      result.M12 = matrix1.M12 * factor;
      result.M13 = matrix1.M13 * factor;
      result.M14 = matrix1.M14 * factor;
      result.M21 = matrix1.M21 * factor;
      result.M22 = matrix1.M22 * factor;
      result.M23 = matrix1.M23 * factor;
      result.M24 = matrix1.M24 * factor;
      result.M31 = matrix1.M31 * factor;
      result.M32 = matrix1.M32 * factor;
      result.M33 = matrix1.M33 * factor;
      result.M34 = matrix1.M34 * factor;
      result.M41 = matrix1.M41 * factor;
      result.M42 = matrix1.M42 * factor;
      result.M43 = matrix1.M43 * factor;
      result.M44 = matrix1.M44 * factor;
    }

    public static Matrix Negate(Matrix matrix)
    {
      matrix.M11 = -matrix.M11;
      matrix.M12 = -matrix.M12;
      matrix.M13 = -matrix.M13;
      matrix.M14 = -matrix.M14;
      matrix.M21 = -matrix.M21;
      matrix.M22 = -matrix.M22;
      matrix.M23 = -matrix.M23;
      matrix.M24 = -matrix.M24;
      matrix.M31 = -matrix.M31;
      matrix.M32 = -matrix.M32;
      matrix.M33 = -matrix.M33;
      matrix.M34 = -matrix.M34;
      matrix.M41 = -matrix.M41;
      matrix.M42 = -matrix.M42;
      matrix.M43 = -matrix.M43;
      matrix.M44 = -matrix.M44;
      return matrix;
    }

    public static void Negate(ref Matrix matrix, out Matrix result)
    {
      result.M11 = matrix.M11;
      result.M12 = matrix.M12;
      result.M13 = matrix.M13;
      result.M14 = matrix.M14;
      result.M21 = matrix.M21;
      result.M22 = matrix.M22;
      result.M23 = matrix.M23;
      result.M24 = matrix.M24;
      result.M31 = matrix.M31;
      result.M32 = matrix.M32;
      result.M33 = matrix.M33;
      result.M34 = matrix.M34;
      result.M41 = matrix.M41;
      result.M42 = matrix.M42;
      result.M43 = matrix.M43;
      result.M44 = matrix.M44;
    }

    public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 -= matrix2.M11;
      matrix1.M12 -= matrix2.M12;
      matrix1.M13 -= matrix2.M13;
      matrix1.M14 -= matrix2.M14;
      matrix1.M21 -= matrix2.M21;
      matrix1.M22 -= matrix2.M22;
      matrix1.M23 -= matrix2.M23;
      matrix1.M24 -= matrix2.M24;
      matrix1.M31 -= matrix2.M31;
      matrix1.M32 -= matrix2.M32;
      matrix1.M33 -= matrix2.M33;
      matrix1.M34 -= matrix2.M34;
      matrix1.M41 -= matrix2.M41;
      matrix1.M42 -= matrix2.M42;
      matrix1.M43 -= matrix2.M43;
      matrix1.M44 -= matrix2.M44;
      return matrix1;
    }

    public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
      result.M11 = matrix1.M11 - matrix2.M11;
      result.M12 = matrix1.M12 - matrix2.M12;
      result.M13 = matrix1.M13 - matrix2.M13;
      result.M14 = matrix1.M14 - matrix2.M14;
      result.M21 = matrix1.M21 - matrix2.M21;
      result.M22 = matrix1.M22 - matrix2.M22;
      result.M23 = matrix1.M23 - matrix2.M23;
      result.M24 = matrix1.M24 - matrix2.M24;
      result.M31 = matrix1.M31 - matrix2.M31;
      result.M32 = matrix1.M32 - matrix2.M32;
      result.M33 = matrix1.M33 - matrix2.M33;
      result.M34 = matrix1.M34 - matrix2.M34;
      result.M41 = matrix1.M41 - matrix2.M41;
      result.M42 = matrix1.M42 - matrix2.M42;
      result.M43 = matrix1.M43 - matrix2.M43;
      result.M44 = matrix1.M44 - matrix2.M44;
    }

    public static Matrix Transpose(Matrix matrix)
    {
      Matrix matrix1;
      matrix1.M11 = matrix.M11;
      matrix1.M12 = matrix.M21;
      matrix1.M13 = matrix.M31;
      matrix1.M14 = matrix.M41;
      matrix1.M21 = matrix.M12;
      matrix1.M22 = matrix.M22;
      matrix1.M23 = matrix.M32;
      matrix1.M24 = matrix.M42;
      matrix1.M31 = matrix.M13;
      matrix1.M32 = matrix.M23;
      matrix1.M33 = matrix.M33;
      matrix1.M34 = matrix.M43;
      matrix1.M41 = matrix.M14;
      matrix1.M42 = matrix.M24;
      matrix1.M43 = matrix.M34;
      matrix1.M44 = matrix.M44;
      return matrix1;
    }

    public static void Transpose(ref Matrix matrix, out Matrix result)
    {
      result.M11 = matrix.M11;
      result.M12 = matrix.M21;
      result.M13 = matrix.M31;
      result.M14 = matrix.M41;
      result.M21 = matrix.M12;
      result.M22 = matrix.M22;
      result.M23 = matrix.M32;
      result.M24 = matrix.M42;
      result.M31 = matrix.M13;
      result.M32 = matrix.M23;
      result.M33 = matrix.M33;
      result.M34 = matrix.M43;
      result.M41 = matrix.M14;
      result.M42 = matrix.M24;
      result.M43 = matrix.M34;
      result.M44 = matrix.M44;
    }

    public float Determinant()
    {
      float num1 = (float) ((double) this.M31 * (double) this.M42 - (double) this.M32 * (double) this.M41);
      float num2 = (float) ((double) this.M31 * (double) this.M43 - (double) this.M33 * (double) this.M41);
      float num3 = (float) ((double) this.M31 * (double) this.M44 - (double) this.M34 * (double) this.M41);
      float num4 = (float) ((double) this.M32 * (double) this.M43 - (double) this.M33 * (double) this.M42);
      float num5 = (float) ((double) this.M32 * (double) this.M44 - (double) this.M34 * (double) this.M42);
      float num6 = (float) ((double) this.M33 * (double) this.M44 - (double) this.M34 * (double) this.M43);
      return (float) ((double) this.M11 * ((double) this.M22 * (double) num6 - (double) this.M23 * (double) num5 + (double) this.M24 * (double) num4) - (double) this.M12 * ((double) this.M21 * (double) num6 - (double) this.M23 * (double) num3 + (double) this.M24 * (double) num2) + (double) this.M13 * ((double) this.M21 * (double) num5 - (double) this.M22 * (double) num3 + (double) this.M24 * (double) num1) - (double) this.M14 * ((double) this.M21 * (double) num4 - (double) this.M22 * (double) num2 + (double) this.M23 * (double) num1));
    }

    public bool Equals(Matrix other) => (double) this.M11 == (double) other.M11 && (double) this.M12 == (double) other.M12 && ((double) this.M13 == (double) other.M13 && (double) this.M14 == (double) other.M14) && ((double) this.M21 == (double) other.M21 && (double) this.M22 == (double) other.M22 && ((double) this.M23 == (double) other.M23 && (double) this.M24 == (double) other.M24)) && ((double) this.M31 == (double) other.M31 && (double) this.M32 == (double) other.M32 && ((double) this.M33 == (double) other.M33 && (double) this.M34 == (double) other.M34) && ((double) this.M41 == (double) other.M41 && (double) this.M42 == (double) other.M42 && (double) this.M43 == (double) other.M43)) && (double) this.M44 == (double) other.M44;

    public static Matrix operator +(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 += matrix2.M11;
      matrix1.M12 += matrix2.M12;
      matrix1.M13 += matrix2.M13;
      matrix1.M14 += matrix2.M14;
      matrix1.M21 += matrix2.M21;
      matrix1.M22 += matrix2.M22;
      matrix1.M23 += matrix2.M23;
      matrix1.M24 += matrix2.M24;
      matrix1.M31 += matrix2.M31;
      matrix1.M32 += matrix2.M32;
      matrix1.M33 += matrix2.M33;
      matrix1.M34 += matrix2.M34;
      matrix1.M41 += matrix2.M41;
      matrix1.M42 += matrix2.M42;
      matrix1.M43 += matrix2.M43;
      matrix1.M44 += matrix2.M44;
      return matrix1;
    }

    public static Matrix operator /(Matrix matrix1, Matrix matrix2)
    {
      Matrix matrix3 = Matrix.Invert(matrix2);
      Matrix matrix4;
      matrix4.M11 = (float) ((double) matrix1.M11 * (double) matrix3.M11 + (double) matrix1.M12 * (double) matrix3.M21 + (double) matrix1.M13 * (double) matrix3.M31 + (double) matrix1.M14 * (double) matrix3.M41);
      matrix4.M12 = (float) ((double) matrix1.M11 * (double) matrix3.M12 + (double) matrix1.M12 * (double) matrix3.M22 + (double) matrix1.M13 * (double) matrix3.M32 + (double) matrix1.M14 * (double) matrix3.M42);
      matrix4.M13 = (float) ((double) matrix1.M11 * (double) matrix3.M13 + (double) matrix1.M12 * (double) matrix3.M23 + (double) matrix1.M13 * (double) matrix3.M33 + (double) matrix1.M14 * (double) matrix3.M43);
      matrix4.M14 = (float) ((double) matrix1.M11 * (double) matrix3.M14 + (double) matrix1.M12 * (double) matrix3.M24 + (double) matrix1.M13 * (double) matrix3.M34 + (double) matrix1.M14 * (double) matrix3.M44);
      matrix4.M21 = (float) ((double) matrix1.M21 * (double) matrix3.M11 + (double) matrix1.M22 * (double) matrix3.M21 + (double) matrix1.M23 * (double) matrix3.M31 + (double) matrix1.M24 * (double) matrix3.M41);
      matrix4.M22 = (float) ((double) matrix1.M21 * (double) matrix3.M12 + (double) matrix1.M22 * (double) matrix3.M22 + (double) matrix1.M23 * (double) matrix3.M32 + (double) matrix1.M24 * (double) matrix3.M42);
      matrix4.M23 = (float) ((double) matrix1.M21 * (double) matrix3.M13 + (double) matrix1.M22 * (double) matrix3.M23 + (double) matrix1.M23 * (double) matrix3.M33 + (double) matrix1.M24 * (double) matrix3.M43);
      matrix4.M24 = (float) ((double) matrix1.M21 * (double) matrix3.M14 + (double) matrix1.M22 * (double) matrix3.M24 + (double) matrix1.M23 * (double) matrix3.M34 + (double) matrix1.M24 * (double) matrix3.M44);
      matrix4.M31 = (float) ((double) matrix1.M31 * (double) matrix3.M11 + (double) matrix1.M32 * (double) matrix3.M21 + (double) matrix1.M33 * (double) matrix3.M31 + (double) matrix1.M34 * (double) matrix3.M41);
      matrix4.M32 = (float) ((double) matrix1.M31 * (double) matrix3.M12 + (double) matrix1.M32 * (double) matrix3.M22 + (double) matrix1.M33 * (double) matrix3.M32 + (double) matrix1.M34 * (double) matrix3.M42);
      matrix4.M33 = (float) ((double) matrix1.M31 * (double) matrix3.M13 + (double) matrix1.M32 * (double) matrix3.M23 + (double) matrix1.M33 * (double) matrix3.M33 + (double) matrix1.M34 * (double) matrix3.M43);
      matrix4.M34 = (float) ((double) matrix1.M31 * (double) matrix3.M14 + (double) matrix1.M32 * (double) matrix3.M24 + (double) matrix1.M33 * (double) matrix3.M34 + (double) matrix1.M34 * (double) matrix3.M44);
      matrix4.M41 = (float) ((double) matrix1.M41 * (double) matrix3.M11 + (double) matrix1.M42 * (double) matrix3.M21 + (double) matrix1.M43 * (double) matrix3.M31 + (double) matrix1.M44 * (double) matrix3.M41);
      matrix4.M42 = (float) ((double) matrix1.M41 * (double) matrix3.M12 + (double) matrix1.M42 * (double) matrix3.M22 + (double) matrix1.M43 * (double) matrix3.M32 + (double) matrix1.M44 * (double) matrix3.M42);
      matrix4.M43 = (float) ((double) matrix1.M41 * (double) matrix3.M13 + (double) matrix1.M42 * (double) matrix3.M23 + (double) matrix1.M43 * (double) matrix3.M33 + (double) matrix1.M44 * (double) matrix3.M43);
      matrix4.M44 = (float) ((double) matrix1.M41 * (double) matrix3.M14 + (double) matrix1.M42 * (double) matrix3.M24 + (double) matrix1.M43 * (double) matrix3.M34 + (double) matrix1.M44 * (double) matrix3.M44);
      return matrix4;
    }

    public static Matrix operator /(Matrix matrix1, float divider)
    {
      float num = 1f / divider;
      matrix1.M11 *= num;
      matrix1.M12 *= num;
      matrix1.M13 *= num;
      matrix1.M14 *= num;
      matrix1.M21 *= num;
      matrix1.M22 *= num;
      matrix1.M23 *= num;
      matrix1.M24 *= num;
      matrix1.M31 *= num;
      matrix1.M32 *= num;
      matrix1.M33 *= num;
      matrix1.M34 *= num;
      matrix1.M41 *= num;
      matrix1.M42 *= num;
      matrix1.M43 *= num;
      matrix1.M44 *= num;
      return matrix1;
    }

    public static bool operator ==(Matrix matrix1, Matrix matrix2) => (double) matrix1.M11 == (double) matrix2.M11 && (double) matrix1.M12 == (double) matrix2.M12 && ((double) matrix1.M13 == (double) matrix2.M13 && (double) matrix1.M14 == (double) matrix2.M14) && ((double) matrix1.M21 == (double) matrix2.M21 && (double) matrix1.M22 == (double) matrix2.M22 && ((double) matrix1.M23 == (double) matrix2.M23 && (double) matrix1.M24 == (double) matrix2.M24)) && ((double) matrix1.M31 == (double) matrix2.M31 && (double) matrix1.M32 == (double) matrix2.M32 && ((double) matrix1.M33 == (double) matrix2.M33 && (double) matrix1.M34 == (double) matrix2.M34) && ((double) matrix1.M41 == (double) matrix2.M41 && (double) matrix1.M42 == (double) matrix2.M42 && (double) matrix1.M43 == (double) matrix2.M43)) && (double) matrix1.M44 == (double) matrix2.M44;

    public static bool operator !=(Matrix matrix1, Matrix matrix2) => (double) matrix1.M11 != (double) matrix2.M11 || (double) matrix1.M12 != (double) matrix2.M12 || ((double) matrix1.M13 != (double) matrix2.M13 || (double) matrix1.M14 != (double) matrix2.M14) || ((double) matrix1.M21 != (double) matrix2.M21 || (double) matrix1.M22 != (double) matrix2.M22 || ((double) matrix1.M23 != (double) matrix2.M23 || (double) matrix1.M24 != (double) matrix2.M24)) || ((double) matrix1.M31 != (double) matrix2.M31 || (double) matrix1.M32 != (double) matrix2.M32 || ((double) matrix1.M33 != (double) matrix2.M33 || (double) matrix1.M34 != (double) matrix2.M34) || ((double) matrix1.M41 != (double) matrix2.M41 || (double) matrix1.M42 != (double) matrix2.M42 || (double) matrix1.M43 != (double) matrix2.M43)) || (double) matrix1.M44 != (double) matrix2.M44;

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
      Matrix matrix;
      matrix.M11 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31 + (double) matrix1.M14 * (double) matrix2.M41);
      matrix.M12 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32 + (double) matrix1.M14 * (double) matrix2.M42);
      matrix.M13 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33 + (double) matrix1.M14 * (double) matrix2.M43);
      matrix.M14 = (float) ((double) matrix1.M11 * (double) matrix2.M14 + (double) matrix1.M12 * (double) matrix2.M24 + (double) matrix1.M13 * (double) matrix2.M34 + (double) matrix1.M14 * (double) matrix2.M44);
      matrix.M21 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31 + (double) matrix1.M24 * (double) matrix2.M41);
      matrix.M22 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32 + (double) matrix1.M24 * (double) matrix2.M42);
      matrix.M23 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33 + (double) matrix1.M24 * (double) matrix2.M43);
      matrix.M24 = (float) ((double) matrix1.M21 * (double) matrix2.M14 + (double) matrix1.M22 * (double) matrix2.M24 + (double) matrix1.M23 * (double) matrix2.M34 + (double) matrix1.M24 * (double) matrix2.M44);
      matrix.M31 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31 + (double) matrix1.M34 * (double) matrix2.M41);
      matrix.M32 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32 + (double) matrix1.M34 * (double) matrix2.M42);
      matrix.M33 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33 + (double) matrix1.M34 * (double) matrix2.M43);
      matrix.M34 = (float) ((double) matrix1.M31 * (double) matrix2.M14 + (double) matrix1.M32 * (double) matrix2.M24 + (double) matrix1.M33 * (double) matrix2.M34 + (double) matrix1.M34 * (double) matrix2.M44);
      matrix.M41 = (float) ((double) matrix1.M41 * (double) matrix2.M11 + (double) matrix1.M42 * (double) matrix2.M21 + (double) matrix1.M43 * (double) matrix2.M31 + (double) matrix1.M44 * (double) matrix2.M41);
      matrix.M42 = (float) ((double) matrix1.M41 * (double) matrix2.M12 + (double) matrix1.M42 * (double) matrix2.M22 + (double) matrix1.M43 * (double) matrix2.M32 + (double) matrix1.M44 * (double) matrix2.M42);
      matrix.M43 = (float) ((double) matrix1.M41 * (double) matrix2.M13 + (double) matrix1.M42 * (double) matrix2.M23 + (double) matrix1.M43 * (double) matrix2.M33 + (double) matrix1.M44 * (double) matrix2.M43);
      matrix.M44 = (float) ((double) matrix1.M41 * (double) matrix2.M14 + (double) matrix1.M42 * (double) matrix2.M24 + (double) matrix1.M43 * (double) matrix2.M34 + (double) matrix1.M44 * (double) matrix2.M44);
      return matrix;
    }

    public static Matrix operator *(Matrix matrix, float scaleFactor)
    {
      matrix.M11 *= scaleFactor;
      matrix.M12 *= scaleFactor;
      matrix.M13 *= scaleFactor;
      matrix.M14 *= scaleFactor;
      matrix.M21 *= scaleFactor;
      matrix.M22 *= scaleFactor;
      matrix.M23 *= scaleFactor;
      matrix.M24 *= scaleFactor;
      matrix.M31 *= scaleFactor;
      matrix.M32 *= scaleFactor;
      matrix.M33 *= scaleFactor;
      matrix.M34 *= scaleFactor;
      matrix.M41 *= scaleFactor;
      matrix.M42 *= scaleFactor;
      matrix.M43 *= scaleFactor;
      matrix.M44 *= scaleFactor;
      return matrix;
    }

    public static Matrix operator *(float scaleFactor, Matrix matrix)
    {
      matrix.M11 *= scaleFactor;
      matrix.M12 *= scaleFactor;
      matrix.M13 *= scaleFactor;
      matrix.M14 *= scaleFactor;
      matrix.M21 *= scaleFactor;
      matrix.M22 *= scaleFactor;
      matrix.M23 *= scaleFactor;
      matrix.M24 *= scaleFactor;
      matrix.M31 *= scaleFactor;
      matrix.M32 *= scaleFactor;
      matrix.M33 *= scaleFactor;
      matrix.M34 *= scaleFactor;
      matrix.M41 *= scaleFactor;
      matrix.M42 *= scaleFactor;
      matrix.M43 *= scaleFactor;
      matrix.M44 *= scaleFactor;
      return matrix;
    }

    public static Matrix operator -(Matrix matrix1, Matrix matrix2)
    {
      matrix1.M11 -= matrix2.M11;
      matrix1.M12 -= matrix2.M12;
      matrix1.M13 -= matrix2.M13;
      matrix1.M14 -= matrix2.M14;
      matrix1.M21 -= matrix2.M21;
      matrix1.M22 -= matrix2.M22;
      matrix1.M23 -= matrix2.M23;
      matrix1.M24 -= matrix2.M24;
      matrix1.M31 -= matrix2.M31;
      matrix1.M32 -= matrix2.M32;
      matrix1.M33 -= matrix2.M33;
      matrix1.M34 -= matrix2.M34;
      matrix1.M41 -= matrix2.M41;
      matrix1.M42 -= matrix2.M42;
      matrix1.M43 -= matrix2.M43;
      matrix1.M44 -= matrix2.M44;
      return matrix1;
    }

    public static Matrix operator -(Matrix matrix)
    {
      matrix.M11 = -matrix.M11;
      matrix.M12 = -matrix.M12;
      matrix.M13 = -matrix.M13;
      matrix.M14 = -matrix.M14;
      matrix.M21 = -matrix.M21;
      matrix.M22 = -matrix.M22;
      matrix.M23 = -matrix.M23;
      matrix.M24 = -matrix.M24;
      matrix.M31 = -matrix.M31;
      matrix.M32 = -matrix.M32;
      matrix.M33 = -matrix.M33;
      matrix.M34 = -matrix.M34;
      matrix.M41 = -matrix.M41;
      matrix.M42 = -matrix.M42;
      matrix.M43 = -matrix.M43;
      matrix.M44 = -matrix.M44;
      return matrix;
    }

    public override bool Equals(object obj) => obj is Matrix matrix && this == matrix;

    public override int GetHashCode() => throw new NotImplementedException();

    public override string ToString() => "{ {M11:" + (object) this.M11 + " M12:" + (object) this.M12 + " M13:" + (object) this.M13 + " M14:" + (object) this.M14 + "} {M21:" + (object) this.M21 + " M22:" + (object) this.M22 + " M23:" + (object) this.M23 + " M24:" + (object) this.M24 + "} {M31:" + (object) this.M31 + " M32:" + (object) this.M32 + " M33:" + (object) this.M33 + " M34:" + (object) this.M34 + "} {M41:" + (object) this.M41 + " M42:" + (object) this.M42 + " M43:" + (object) this.M43 + " M44:" + (object) this.M44 + "} }";

    public static implicit operator Microsoft.Xna.Framework.Matrix(Matrix m) => new Microsoft.Xna.Framework.Matrix(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);

    public static implicit operator Matrix(Microsoft.Xna.Framework.Matrix m) => new Matrix(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
  }
}
