// Decompiled with JetBrains decompiler
// Type: DuckGame.DamageMap
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class DamageMap
  {
    private const int size = 256;
    public Thing thing;
    public byte[] bytes = new byte[256];

    public bool InRange(int x, int y)
    {
      x = (int) ((double) x - (double) this.thing.left);
      y = (int) ((double) y - (double) this.thing.top);
      x += y * 16;
      return x >= 0 && x < 256;
    }

    public bool InRange(float x, float y)
    {
      x = (float) Math.Round((double) x);
      y = (float) Math.Round((double) y);
      return this.InRange((int) x, (int) y);
    }

    public bool CheckPoint(int x, int y)
    {
      x = (int) ((double) x - (double) this.thing.left);
      y = (int) ((double) y - (double) this.thing.top);
      x += y * 16;
      return x < 0 || x >= 256 || this.bytes[x] > (byte) 0;
    }

    public bool CheckPointRelative(int x, int y)
    {
      x += y * 16;
      return x < 0 || x >= 256 || this.bytes[x] > (byte) 0;
    }

    public bool CheckPoint(float x, float y)
    {
      x = (float) Math.Round((double) x);
      y = (float) Math.Round((double) y);
      return this.CheckPoint((int) x, (int) y);
    }

    public void SetPoint(int x, int y, bool val)
    {
      x += y * 16;
      if (x < 0 || x >= 256)
        return;
      this.bytes[x] = val ? (byte) 1 : (byte) 0;
    }

    public void Damage(Vec2 point, float radius)
    {
      point.x -= this.thing.left;
      point.y -= this.thing.top;
      for (int y = 0; y < 16; ++y)
      {
        for (int x = 0; x < 16; ++x)
        {
          if ((double) (new Vec2((float) x, (float) y) - point).length <= (double) radius)
            this.SetPoint(x, y, false);
        }
      }
    }

    public void Clear()
    {
      for (int index = 0; index < 256; ++index)
        this.bytes[index] = (byte) 1;
    }
  }
}
