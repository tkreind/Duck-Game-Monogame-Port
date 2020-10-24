// Decompiled with JetBrains decompiler
// Type: DuckGame.BufferedGhostProperty
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class BufferedGhostProperty
  {
    public object value;
    public StateBinding binding;
    public bool valid = true;
    public bool isNetValue;

    public override string ToString() => this.binding.GetDebugString(this.value);
  }
}
