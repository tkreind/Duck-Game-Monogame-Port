// Decompiled with JetBrains decompiler
// Type: DuckGame.Color
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  /// <summary>An RGBA color.</summary>
  public struct Color : IEquatable<Color>
  {
    public byte r;
    public byte g;
    public byte b;
    public byte a;
    public static readonly Color AliceBlue = new Color(240, 248, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color AntiqueWhite = new Color(250, 235, 215, (int) byte.MaxValue);
    public static readonly Color Aqua = new Color(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Aquamarine = new Color((int) sbyte.MaxValue, (int) byte.MaxValue, 212, (int) byte.MaxValue);
    public static readonly Color Azure = new Color(240, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Beige = new Color(245, 245, 220, (int) byte.MaxValue);
    public static readonly Color Bisque = new Color((int) byte.MaxValue, 228, 196, (int) byte.MaxValue);
    public static readonly Color Black = new Color(0, 0, 0, (int) byte.MaxValue);
    public static readonly Color BlanchedAlmond = new Color((int) byte.MaxValue, 235, 205, (int) byte.MaxValue);
    public static readonly Color Blue = new Color(0, 0, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color BlueViolet = new Color(138, 43, 226, (int) byte.MaxValue);
    public static readonly Color Brown = new Color(165, 42, 42, (int) byte.MaxValue);
    public static readonly Color BurlyWood = new Color(222, 184, 135, (int) byte.MaxValue);
    public static readonly Color CadetBlue = new Color(95, 158, 160, (int) byte.MaxValue);
    public static readonly Color Chartreuse = new Color((int) sbyte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
    public static readonly Color Chocolate = new Color(210, 105, 30, (int) byte.MaxValue);
    public static readonly Color Coral = new Color((int) byte.MaxValue, (int) sbyte.MaxValue, 80, (int) byte.MaxValue);
    public static readonly Color CornflowerBlue = new Color(100, 149, 237, (int) byte.MaxValue);
    public static readonly Color Cornsilk = new Color((int) byte.MaxValue, 248, 220, (int) byte.MaxValue);
    public static readonly Color Crimson = new Color(220, 20, 60, (int) byte.MaxValue);
    public static readonly Color Cyan = new Color(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color DarkBlue = new Color(0, 0, 139, (int) byte.MaxValue);
    public static readonly Color DarkCyan = new Color(0, 139, 139, (int) byte.MaxValue);
    public static readonly Color DarkGoldenrod = new Color(184, 134, 11, (int) byte.MaxValue);
    public static readonly Color DarkGray = new Color(169, 169, 169, (int) byte.MaxValue);
    public static readonly Color DarkGreen = new Color(0, 100, 0, (int) byte.MaxValue);
    public static readonly Color DarkKhaki = new Color(189, 183, 107, (int) byte.MaxValue);
    public static readonly Color DarkMagenta = new Color(139, 0, 139, (int) byte.MaxValue);
    public static readonly Color DarkOliveGreen = new Color(85, 107, 47, (int) byte.MaxValue);
    public static readonly Color DarkOrange = new Color((int) byte.MaxValue, 140, 0, (int) byte.MaxValue);
    public static readonly Color DarkOrchid = new Color(153, 50, 204, (int) byte.MaxValue);
    public static readonly Color DarkRed = new Color(139, 0, 0, (int) byte.MaxValue);
    public static readonly Color DarkSalmon = new Color(233, 150, 122, (int) byte.MaxValue);
    public static readonly Color DarkSeaGreen = new Color(143, 188, 139, (int) byte.MaxValue);
    public static readonly Color DarkSlateBlue = new Color(72, 61, 139, (int) byte.MaxValue);
    public static readonly Color DarkSlateGray = new Color(47, 79, 79, (int) byte.MaxValue);
    public static readonly Color DarkTurquoise = new Color(0, 206, 209, (int) byte.MaxValue);
    public static readonly Color DarkViolet = new Color(148, 0, 211, (int) byte.MaxValue);
    public static readonly Color DeepPink = new Color((int) byte.MaxValue, 20, 147, (int) byte.MaxValue);
    public static readonly Color DeepSkyBlue = new Color(0, 191, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color DimGray = new Color(105, 105, 105, (int) byte.MaxValue);
    public static readonly Color DodgerBlue = new Color(30, 144, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Firebrick = new Color(178, 34, 34, (int) byte.MaxValue);
    public static readonly Color FloralWhite = new Color((int) byte.MaxValue, 250, 240, (int) byte.MaxValue);
    public static readonly Color ForestGreen = new Color(34, 139, 34, (int) byte.MaxValue);
    public static readonly Color Fuchsia = new Color((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Gainsboro = new Color(220, 220, 220, (int) byte.MaxValue);
    public static readonly Color GhostWhite = new Color(248, 248, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Gold = new Color((int) byte.MaxValue, 215, 0, (int) byte.MaxValue);
    public static readonly Color Goldenrod = new Color(218, 165, 32, (int) byte.MaxValue);
    public static readonly Color Gray = new Color(128, 128, 128, (int) byte.MaxValue);
    public static readonly Color Green = new Color(0, 128, 0, (int) byte.MaxValue);
    public static readonly Color GreenYellow = new Color(173, (int) byte.MaxValue, 47, (int) byte.MaxValue);
    public static readonly Color Honeydew = new Color(240, (int) byte.MaxValue, 240, (int) byte.MaxValue);
    public static readonly Color HotPink = new Color((int) byte.MaxValue, 105, 180, (int) byte.MaxValue);
    public static readonly Color IndianRed = new Color(205, 92, 92, (int) byte.MaxValue);
    public static readonly Color Indigo = new Color(75, 0, 130, (int) byte.MaxValue);
    public static readonly Color Ivory = new Color((int) byte.MaxValue, (int) byte.MaxValue, 240, (int) byte.MaxValue);
    public static readonly Color Khaki = new Color(240, 230, 140, (int) byte.MaxValue);
    public static readonly Color Lavender = new Color(230, 230, 250, (int) byte.MaxValue);
    public static readonly Color LavenderBlush = new Color((int) byte.MaxValue, 240, 245, (int) byte.MaxValue);
    public static readonly Color LawnGreen = new Color(124, 252, 0, (int) byte.MaxValue);
    public static readonly Color LemonChiffon = new Color((int) byte.MaxValue, 250, 205, (int) byte.MaxValue);
    public static readonly Color LightBlue = new Color(173, 216, 230, (int) byte.MaxValue);
    public static readonly Color LightCoral = new Color(240, 128, 128, (int) byte.MaxValue);
    public static readonly Color LightCyan = new Color(224, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210, (int) byte.MaxValue);
    public static readonly Color LightGray = new Color(211, 211, 211, (int) byte.MaxValue);
    public static readonly Color LightGreen = new Color(144, 238, 144, (int) byte.MaxValue);
    public static readonly Color LightPink = new Color((int) byte.MaxValue, 182, 193, (int) byte.MaxValue);
    public static readonly Color LightSalmon = new Color((int) byte.MaxValue, 160, 122, (int) byte.MaxValue);
    public static readonly Color LightSeaGreen = new Color(32, 178, 170, (int) byte.MaxValue);
    public static readonly Color LightSkyBlue = new Color(135, 206, 250, (int) byte.MaxValue);
    public static readonly Color LightSlateGray = new Color(119, 136, 153, (int) byte.MaxValue);
    public static readonly Color LightSteelBlue = new Color(176, 196, 222, (int) byte.MaxValue);
    public static readonly Color LightYellow = new Color((int) byte.MaxValue, (int) byte.MaxValue, 224, (int) byte.MaxValue);
    public static readonly Color Lime = new Color(0, (int) byte.MaxValue, 0, (int) byte.MaxValue);
    public static readonly Color LimeGreen = new Color(50, 205, 50, (int) byte.MaxValue);
    public static readonly Color Linen = new Color(250, 240, 230, (int) byte.MaxValue);
    public static readonly Color Magenta = new Color((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color Maroon = new Color(128, 0, 0, (int) byte.MaxValue);
    public static readonly Color MediumAquamarine = new Color(102, 205, 170, (int) byte.MaxValue);
    public static readonly Color MediumBlue = new Color(0, 0, 205, (int) byte.MaxValue);
    public static readonly Color MediumOrchid = new Color(186, 85, 211, (int) byte.MaxValue);
    public static readonly Color MediumPurple = new Color(147, 112, 219, (int) byte.MaxValue);
    public static readonly Color MediumSeaGreen = new Color(60, 179, 113, (int) byte.MaxValue);
    public static readonly Color MediumSlateBlue = new Color(123, 104, 238, (int) byte.MaxValue);
    public static readonly Color MediumSpringGreen = new Color(0, 250, 154, (int) byte.MaxValue);
    public static readonly Color MediumTurquoise = new Color(72, 209, 204, (int) byte.MaxValue);
    public static readonly Color MediumVioletRed = new Color(199, 21, 133, (int) byte.MaxValue);
    public static readonly Color MidnightBlue = new Color(25, 25, 112, (int) byte.MaxValue);
    public static readonly Color MintCream = new Color(245, (int) byte.MaxValue, 250, (int) byte.MaxValue);
    public static readonly Color MistyRose = new Color((int) byte.MaxValue, 228, 225, (int) byte.MaxValue);
    public static readonly Color Moccasin = new Color((int) byte.MaxValue, 228, 181, (int) byte.MaxValue);
    public static readonly Color NavajoWhite = new Color((int) byte.MaxValue, 222, 173, (int) byte.MaxValue);
    public static readonly Color OldLace = new Color(253, 245, 230, (int) byte.MaxValue);
    public static readonly Color Olive = new Color(128, 128, 0, (int) byte.MaxValue);
    public static readonly Color OliveDrab = new Color(107, 142, 35, (int) byte.MaxValue);
    public static readonly Color Orange = new Color((int) byte.MaxValue, 165, 0, (int) byte.MaxValue);
    public static readonly Color OrangeRed = new Color((int) byte.MaxValue, 69, 0, (int) byte.MaxValue);
    public static readonly Color Orchid = new Color(218, 112, 214, (int) byte.MaxValue);
    public static readonly Color PaleGoldenrod = new Color(238, 232, 170, (int) byte.MaxValue);
    public static readonly Color PaleGreen = new Color(152, 251, 152, (int) byte.MaxValue);
    public static readonly Color PaleTurquoise = new Color(175, 238, 238, (int) byte.MaxValue);
    public static readonly Color PaleVioletRed = new Color(219, 112, 147, (int) byte.MaxValue);
    public static readonly Color PapayaWhip = new Color((int) byte.MaxValue, 239, 213, (int) byte.MaxValue);
    public static readonly Color PeachPuff = new Color((int) byte.MaxValue, 218, 185, (int) byte.MaxValue);
    public static readonly Color Peru = new Color(205, 133, 63, (int) byte.MaxValue);
    public static readonly Color Pink = new Color((int) byte.MaxValue, 192, 203, (int) byte.MaxValue);
    public static readonly Color Plum = new Color(221, 160, 221, (int) byte.MaxValue);
    public static readonly Color PowderBlue = new Color(176, 224, 230, (int) byte.MaxValue);
    public static readonly Color Purple = new Color(128, 0, 128, (int) byte.MaxValue);
    public static readonly Color Red = new Color((int) byte.MaxValue, 0, 0, (int) byte.MaxValue);
    public static readonly Color RosyBrown = new Color(188, 143, 143, (int) byte.MaxValue);
    public static readonly Color RoyalBlue = new Color(65, 105, 225, (int) byte.MaxValue);
    public static readonly Color SaddleBrown = new Color(139, 69, 19, (int) byte.MaxValue);
    public static readonly Color Salmon = new Color(250, 128, 114, (int) byte.MaxValue);
    public static readonly Color SandyBrown = new Color(244, 164, 96, (int) byte.MaxValue);
    public static readonly Color SeaGreen = new Color(46, 139, 87, (int) byte.MaxValue);
    public static readonly Color SeaShell = new Color((int) byte.MaxValue, 245, 238, (int) byte.MaxValue);
    public static readonly Color Sienna = new Color(160, 82, 45, (int) byte.MaxValue);
    public static readonly Color Silver = new Color(192, 192, 192, (int) byte.MaxValue);
    public static readonly Color SkyBlue = new Color(135, 206, 235, (int) byte.MaxValue);
    public static readonly Color SlateBlue = new Color(106, 90, 205, (int) byte.MaxValue);
    public static readonly Color SlateGray = new Color(112, 128, 144, (int) byte.MaxValue);
    public static readonly Color Snow = new Color((int) byte.MaxValue, 250, 250, (int) byte.MaxValue);
    public static readonly Color SpringGreen = new Color(0, (int) byte.MaxValue, (int) sbyte.MaxValue, (int) byte.MaxValue);
    public static readonly Color SteelBlue = new Color(70, 130, 180, (int) byte.MaxValue);
    public static readonly Color Tan = new Color(210, 180, 140, (int) byte.MaxValue);
    public static readonly Color Teal = new Color(0, 128, 128, (int) byte.MaxValue);
    public static readonly Color Thistle = new Color(216, 191, 216, (int) byte.MaxValue);
    public static readonly Color Tomato = new Color((int) byte.MaxValue, 99, 71, (int) byte.MaxValue);
    public static readonly Color Transparent = new Color(0, 0, 0, 0);
    public static readonly Color Turquoise = new Color(64, 224, 208, (int) byte.MaxValue);
    public static readonly Color Violet = new Color(238, 130, 238, (int) byte.MaxValue);
    public static readonly Color Wheat = new Color(245, 222, 179, (int) byte.MaxValue);
    public static readonly Color White = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color WhiteSmoke = new Color(245, 245, 245, (int) byte.MaxValue);
    public static readonly Color Yellow = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
    public static readonly Color YellowGreen = new Color(154, 205, 50, (int) byte.MaxValue);

    public Color(byte r, byte g, byte b, byte a)
      : this()
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public Color(byte r, byte g, byte b)
      : this(r, g, b, byte.MaxValue)
    {
    }

    public Color(int r, int g, int b, int a)
      : this((byte) MathHelper.Clamp(r, 0, (int) byte.MaxValue), (byte) MathHelper.Clamp(g, 0, (int) byte.MaxValue), (byte) MathHelper.Clamp(b, 0, (int) byte.MaxValue), (byte) MathHelper.Clamp(a, 0, (int) byte.MaxValue))
    {
    }

    public Color(int r, int g, int b)
      : this(r, g, b, (int) byte.MaxValue)
    {
    }

    public Color(float r, float g, float b, float a)
      : this((byte) ((double) MathHelper.Clamp(r, 0.0f, 1f) * (double) byte.MaxValue), (byte) ((double) MathHelper.Clamp(g, 0.0f, 1f) * (double) byte.MaxValue), (byte) ((double) MathHelper.Clamp(b, 0.0f, 1f) * (double) byte.MaxValue), (byte) ((double) MathHelper.Clamp(a, 0.0f, 1f) * (double) byte.MaxValue))
    {
    }

    public Color(float r, float g, float b)
      : this(r, g, b, 1f)
    {
    }

    public Color(uint hex)
      : this((byte) (hex & (uint) byte.MaxValue), (byte) ((int) hex << 8 & (int) byte.MaxValue), (byte) ((int) hex << 16 & (int) byte.MaxValue), (byte) ((int) hex << 24 & (int) byte.MaxValue))
    {
    }

    public static explicit operator int(Color color) => (int) color.r | (int) color.g >> 8 | (int) color.b >> 16 | (int) color.a >> 24;

    public static explicit operator Color(uint hex) => new Color(hex);

    public static Color operator *(Color c, float r) => new Color((byte) MathHelper.Clamp((float) c.r * r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.g * r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.b * r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.a * r, 0.0f, (float) byte.MaxValue));

    public static Color operator /(Color c, float r) => new Color((byte) MathHelper.Clamp((float) c.r / r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.g / r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.b / r, 0.0f, (float) byte.MaxValue), (byte) MathHelper.Clamp((float) c.a / r, 0.0f, (float) byte.MaxValue));

    public static bool operator ==(Color l, Color r) => l.Equals(r);

    public static bool operator !=(Color l, Color r) => !l.Equals(r);

    public bool Equals(Color other) => (int) this.r == (int) other.r && (int) this.g == (int) other.g && (int) this.b == (int) other.b && (int) this.a == (int) other.a;

    public override bool Equals(object obj) => obj is Color other ? this.Equals(other) : base.Equals(obj);

    public override int GetHashCode() => (int) this;

    public override string ToString() => string.Format("{0} {1} {2} {3}", (object) this.r, (object) this.g, (object) this.b, (object) this.a);

    public static Color Lerp(Color a, Color b, float v) => DuckGame.Lerp.ColorSmooth(a, b, v);

    public Color(Vec4 vec)
      : this(vec.x, vec.y, vec.z, vec.w)
    {
    }

    public Color(Vec3 vec)
      : this(vec.x, vec.y, vec.z)
    {
    }

    public Vec4 ToVector4() => new Vec4((float) this.r / (float) byte.MaxValue, (float) this.g / (float) byte.MaxValue, (float) this.b / (float) byte.MaxValue, (float) this.a / (float) byte.MaxValue);

    public Vec3 ToVector3() => new Vec3((float) this.r / (float) byte.MaxValue, (float) this.g / (float) byte.MaxValue, (float) this.b / (float) byte.MaxValue);

    public static implicit operator Microsoft.Xna.Framework.Color(Color c) => new Microsoft.Xna.Framework.Color((int) c.r, (int) c.g, (int) c.b, (int) c.a);

    public static implicit operator Color(Microsoft.Xna.Framework.Color c) => new Color(c.R, c.G, c.B, c.A);
  }
}
