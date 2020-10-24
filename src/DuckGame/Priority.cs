// Decompiled with JetBrains decompiler
// Type: DuckGame.Priority
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  /// <summary>
  /// A priority listing for measuring a priority compared to something else.
  /// Higher priorities take priority over lower ones (are executed last, basically)
  /// </summary>
  public enum Priority
  {
    /// <summary>
    /// Has no side-effects and will not conflict with other content
    /// </summary>
    Inconsequential,
    /// <summary>Lowest</summary>
    Lowest,
    /// <summary>Lower</summary>
    Lower,
    /// <summary>Low</summary>
    Low,
    /// <summary>Normal</summary>
    Normal,
    /// <summary>High</summary>
    High,
    /// <summary>Higher</summary>
    Higher,
    /// <summary>Highest</summary>
    Highest,
    /// <summary>
    /// Requires everything else to be done first before this one
    /// </summary>
    Monitor,
  }
}
