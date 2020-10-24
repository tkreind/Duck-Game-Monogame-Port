// Decompiled with JetBrains decompiler
// Type: DuckGame.SN76489
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class SN76489
  {
    private SN76489Core _chip;

    public SN76489() => this._chip = new SN76489Core();

    public void Initialize(double clock) => this._chip.clock((float) clock);

    public void Update(int[] buffer, int length)
    {
      for (int index = 0; index < length; ++index)
      {
        short num = (short) ((double) this._chip.render() * 8000.0);
        buffer[index * 2] = (int) num;
        buffer[index * 2 + 1] = (int) num;
      }
    }

    public void Write(int value) => this._chip.write(value);
  }
}
