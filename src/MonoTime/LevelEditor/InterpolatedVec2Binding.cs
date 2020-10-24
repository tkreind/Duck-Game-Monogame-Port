// Decompiled with JetBrains decompiler
// Type: DuckGame.InterpolatedVec2Binding
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class InterpolatedVec2Binding : CompressedVec2Binding
  {
    public InterpolatedVec2Binding(string field, int range = 2147483647, bool real = true)
      : base(field, range)
      => this._priority = GhostPriority.High;
  }
}
