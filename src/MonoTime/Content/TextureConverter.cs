// Decompiled with JetBrains decompiler
// Type: DuckGame.TextureConverter
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DuckGame
{
  internal static class TextureConverter
  {
    private const int _fromColor = -65281;
    private const int _toColor = 0;

    internal static unsafe Texture2D LoadPNGWithPinkAwesomeness(
      GraphicsDevice device,
      Bitmap bitmap,
      bool process)
    {
      BitmapData bitmapdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      int num1 = bitmapdata.Width * bitmapdata.Height;
      int num2 = 0;
      int* scan0 = (int*) (void*) bitmapdata.Scan0;
      while (num2 < num1)
      {
        if (process && *scan0 == -65281)
        {
          *scan0 = 0;
        }
        else
        {
          byte* numPtr = (byte*) scan0;
          byte num3 = *numPtr;
          *numPtr = numPtr[2];
          numPtr[2] = num3;
          float num4 = (float) numPtr[3] / (float) byte.MaxValue;
          for (int index = 0; index < 3; ++index)
            numPtr[index] = (byte) ((double) numPtr[index] * (double) num4);
        }
        ++num2;
        ++scan0;
      }
      int[] numArray = new int[bitmapdata.Width * bitmapdata.Height];
      Marshal.Copy(bitmapdata.Scan0, numArray, 0, numArray.Length);
      Texture2D texture2D = new Texture2D(device, bitmapdata.Width, bitmapdata.Height);
      texture2D.SetData<int>(numArray);
      bitmap.UnlockBits(bitmapdata);
      return texture2D;
    }

    internal static Texture2D LoadPNGWithPinkAwesomeness(
      GraphicsDevice device,
      Stream stream,
      bool process)
    {
      using (Bitmap bitmap = new Bitmap(stream))
        return TextureConverter.LoadPNGWithPinkAwesomeness(device, bitmap, process);
    }

    internal static Texture2D LoadPNGWithPinkAwesomeness(
      GraphicsDevice device,
      string fileName,
      bool process)
    {
      using (Bitmap bitmap = new Bitmap(fileName))
        return TextureConverter.LoadPNGWithPinkAwesomeness(device, bitmap, process);
    }
  }
}
