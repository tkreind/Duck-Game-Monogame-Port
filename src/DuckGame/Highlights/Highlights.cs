// Decompiled with JetBrains decompiler
// Type: DuckGame.Highlights
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class Highlights
  {
    private static List<Recording> _recordings = new List<Recording>();
    public static float highlightRatingMultiplier = 1f;

    public static void Initialize()
    {
      MonoMain.loadMessage = "Loading Highlights";
      for (int index = 0; index < 6; ++index)
        Highlights._recordings.Add(new Recording());
    }

    public static List<Recording> GetHighlights()
    {
      List<Recording> recordingList = new List<Recording>();
      foreach (Recording recording in Highlights._recordings)
      {
        if (Recorder.currentRecording != recording && recordingList.Count < 6)
        {
          recording.Rewind();
          recordingList.Add(recording);
        }
      }
      return recordingList;
    }

    public static void ClearHighlights()
    {
      foreach (Recording recording in Highlights._recordings)
        recording.Reset();
    }

    public static void FinishRound()
    {
      Highlights.highlightRatingMultiplier = 1f;
      Recording currentRecording = Recorder.currentRecording;
      Recorder.currentRecording = (Recording) null;
      if (currentRecording == null)
        return;
      float num = 0.0f;
      float lastMatchLength = (float) Stats.lastMatchLength;
      currentRecording.Rewind();
      while (!currentRecording.StepForward())
        num += currentRecording.GetFrameTotal();
      if ((double) lastMatchLength < 5.0)
        num *= 1.3f;
      if ((double) lastMatchLength < 7.0)
        num *= 1.2f;
      if ((double) lastMatchLength < 10.0)
        num *= 1.1f;
      currentRecording.highlightScore = num;
    }

    public static void StartRound()
    {
      Recording recording1 = Highlights._recordings[0];
      foreach (Recording recording2 in Highlights._recordings)
      {
        if (recording2.startFrame == recording2.endFrame)
        {
          recording1 = recording2;
          break;
        }
        if ((double) recording2.highlightScore < (double) recording1.highlightScore)
          recording1 = recording2;
        if ((double) recording2.highlightScore == (double) recording1.highlightScore && (double) Rando.Float(1f) >= 0.5)
          recording1 = recording2;
      }
      recording1.Reset();
      Recorder.currentRecording = recording1;
    }
  }
}
