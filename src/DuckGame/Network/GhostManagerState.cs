// Decompiled with JetBrains decompiler
// Type: DuckGame.GhostManagerState
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GhostManagerState : Thing
  {
    public StateBinding _predictionIndexBinding = new StateBinding(nameof (predictionIndex));
    public NetIndex16 predictionIndex = new NetIndex16((int) short.MaxValue);

    public GhostManagerState()
      : base()
    {
    }
  }
}
