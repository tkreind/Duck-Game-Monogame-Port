// Decompiled with JetBrains decompiler
// Type: DuckGame.Recorder
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Recorder
  {
    public static Recording _currentRecording = (Recording) null;
    public static FileRecording _globalRecording;

    public static Recording currentRecording
    {
      get => Recorder._currentRecording;
      set => Recorder._currentRecording = value;
    }

    public static void LogVelocity(float velocity)
    {
      if (Recorder._currentRecording == null)
        return;
      Recorder._currentRecording.LogVelocity(velocity);
    }

    public static void LogCoolness(int val)
    {
      if (Recorder._currentRecording == null)
        return;
      Recorder._currentRecording.LogCoolness(val);
    }

    public static void LogDeath()
    {
      if (Recorder._currentRecording == null)
        return;
      Recorder._currentRecording.LogDeath();
    }

    public static void LogAction(int num = 1)
    {
      if (Recorder._currentRecording == null)
        return;
      Recorder._currentRecording.LogAction(num);
    }

    public static void LogBonus()
    {
      if (Recorder._currentRecording == null)
        return;
      Recorder._currentRecording.LogBonus();
    }

    public static FileRecording globalRecording
    {
      get => Recorder._globalRecording;
      set => Recorder._globalRecording = value;
    }
  }
}
