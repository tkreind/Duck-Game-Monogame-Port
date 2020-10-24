// Decompiled with JetBrains decompiler
// Type: DuckGame.SN76489Core
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public sealed class SN76489Core
  {
    private static float[] volumeTable = new float[64]
    {
      0.25f,
      0.2442f,
      0.194f,
      0.1541f,
      0.1224f,
      0.0972f,
      0.0772f,
      0.0613f,
      0.0487f,
      0.0386f,
      0.0307f,
      0.0244f,
      0.0193f,
      0.0154f,
      0.0122f,
      0.0f,
      -0.25f,
      -0.2442f,
      -0.194f,
      -0.1541f,
      -0.1224f,
      -0.0972f,
      -0.0772f,
      -0.0613f,
      -0.0487f,
      -0.0386f,
      -0.0307f,
      -0.0244f,
      -0.0193f,
      -0.0154f,
      -0.0122f,
      0.0f,
      0.25f,
      0.2442f,
      0.194f,
      0.1541f,
      0.1224f,
      0.0972f,
      0.0772f,
      0.0613f,
      0.0487f,
      0.0386f,
      0.0307f,
      0.0244f,
      0.0193f,
      0.0154f,
      0.0122f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f
    };
    private uint volA;
    private uint volB;
    private uint volC;
    private uint volD;
    private int divA;
    private int divB;
    private int divC;
    private int divD;
    private int cntA;
    private int cntB;
    private int cntC;
    private int cntD;
    private float outA;
    private float outB;
    private float outC;
    private float outD;
    private uint noiseLFSR;
    private uint noiseTap;
    private uint latchedChan;
    private bool latchedVolume;
    private float ticksPerSample;
    private float ticksCount;

    public SN76489Core()
    {
      this.clock(3500000f);
      this.reset();
    }

    public void clock(float f) => this.ticksPerSample = (float) ((double) f / 16.0 / 44100.0);

    public void reset()
    {
      this.volA = 15U;
      this.volB = 15U;
      this.volC = 15U;
      this.volD = 15U;
      this.outA = 0.0f;
      this.outB = 0.0f;
      this.outC = 0.0f;
      this.outD = 0.0f;
      this.latchedChan = 0U;
      this.latchedVolume = false;
      this.noiseLFSR = 32768U;
      this.ticksCount = this.ticksPerSample;
    }

    public uint getDivByNumber(uint chan)
    {
      switch (chan)
      {
        case 0:
          return (uint) this.divA;
        case 1:
          return (uint) this.divB;
        case 2:
          return (uint) this.divC;
        case 3:
          return (uint) this.divD;
        default:
          return 0;
      }
    }

    public void setDivByNumber(uint chan, uint div)
    {
      switch (chan)
      {
        case 0:
          this.divA = (int) div;
          break;
        case 1:
          this.divB = (int) div;
          break;
        case 2:
          this.divC = (int) div;
          break;
        case 3:
          this.divD = (int) div;
          break;
      }
    }

    public uint getVolByNumber(uint chan)
    {
      switch (chan)
      {
        case 0:
          return this.volA;
        case 1:
          return this.volB;
        case 2:
          return this.volC;
        case 3:
          return this.volD;
        default:
          return 0;
      }
    }

    public void setVolByNumber(uint chan, uint vol)
    {
      switch (chan)
      {
        case 0:
          this.volA = vol;
          break;
        case 1:
          this.volB = vol;
          break;
        case 2:
          this.volC = vol;
          break;
        case 3:
          this.volD = vol;
          break;
      }
    }

    public void write(int val)
    {
      int num1;
      int num2;
      if ((val & 128) != 0)
      {
        num1 = val >> 5 & 3;
        num2 = (int) this.getDivByNumber((uint) num1) & 65520 | val & 15;
        this.latchedChan = (uint) num1;
        this.latchedVolume = (val & 16) != 0;
      }
      else
      {
        num1 = (int) this.latchedChan;
        num2 = (int) this.getDivByNumber((uint) num1) & 15 | (val & 63) << 4;
      }
      if (this.latchedVolume)
      {
        this.setVolByNumber((uint) num1, (uint) ((int) this.getVolByNumber((uint) num1) & 16 | val & 15));
      }
      else
      {
        this.setDivByNumber((uint) num1, (uint) num2);
        if (num1 != 3)
          return;
        this.noiseTap = (num2 >> 2 & 1) != 0 ? 9U : 1U;
        this.noiseLFSR = 32768U;
      }
    }

    public float render()
    {
      if (0U >= 1U)
        return 0.0f;
      for (; (double) this.ticksCount > 0.0; --this.ticksCount)
      {
        --this.cntA;
        if (this.cntA < 0)
        {
          if (this.divA > 1)
          {
            this.volA ^= 16U;
            this.outA = SN76489Core.volumeTable[this.volA];
          }
          this.cntA = this.divA;
        }
        --this.cntB;
        if (this.cntB < 0)
        {
          if (this.divB > 1)
          {
            this.volB ^= 16U;
            this.outB = SN76489Core.volumeTable[this.volB];
          }
          this.cntB = this.divB;
        }
        --this.cntC;
        if (this.cntC < 0)
        {
          if (this.divC > 1)
          {
            this.volC ^= 16U;
            this.outC = SN76489Core.volumeTable[this.volC];
          }
          this.cntC = this.divC;
        }
        --this.cntD;
        if (this.cntD < 0)
        {
          uint num1 = (uint) (this.divD & 3);
          this.cntD = num1 >= 3U ? this.divC << 1 : 16 << (int) num1;
          uint num2;
          if (this.noiseTap == 9U)
          {
            uint num3 = this.noiseLFSR & this.noiseTap;
            uint num4 = num3 ^ num3 >> 8;
            uint num5 = num4 ^ num4 >> 4;
            uint num6 = num5 ^ num5 >> 2;
            num2 = (num6 ^ num6 >> 1) & 1U;
          }
          else
            num2 = this.noiseLFSR & 1U;
          this.noiseLFSR = this.noiseLFSR >> 1 | num2 << 15;
          this.volD = (uint) ((int) this.volD & 15 | ((int) this.noiseLFSR & 1 ^ 1) << 4);
          this.outD = SN76489Core.volumeTable[this.volD];
        }
      }
      this.ticksCount += this.ticksPerSample;
      return this.outA + this.outB + this.outC + this.outD;
    }
  }
}
