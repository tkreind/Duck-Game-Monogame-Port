// Decompiled with JetBrains decompiler
// Type: DuckGame.FluidData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct FluidData
  {
    public float amount;
    public Vec4 color;
    public float flammable;
    public string sprite;
    public float heat;
    public float transparent;

    public FluidData(float am, Vec4 col, float flam, string spr = "", float h = 0.0f, float trans = 0.7f)
    {
      this.amount = am;
      this.color = col;
      this.flammable = flam;
      this.sprite = spr;
      this.heat = h;
      this.transparent = trans;
    }

    public void Mix(FluidData with)
    {
      float num1 = with.amount + this.amount;
      if ((double) with.amount > 0.0)
      {
        float num2 = this.amount / num1;
        float num3 = with.amount / num1;
        this.flammable = (float) ((double) num2 * (double) this.flammable + (double) num3 * (double) with.flammable);
        this.color = this.color * num2 + with.color * num3;
        this.heat = (float) ((double) this.heat * (double) num2 + (double) with.heat * (double) num3);
        this.transparent = (float) ((double) this.transparent * (double) num2 + (double) with.transparent * (double) num3);
      }
      this.amount = num1;
    }

    public FluidData Take(float val)
    {
      if ((double) val > (double) this.amount)
        val = this.amount;
      this.amount -= val;
      FluidData fluidData = this;
      fluidData.amount = val;
      return fluidData;
    }
  }
}
