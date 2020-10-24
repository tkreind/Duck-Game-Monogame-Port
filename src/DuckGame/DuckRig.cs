// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckRig
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace DuckGame
{
  public class DuckRig
  {
    private static List<Vec2> _hatPoints = new List<Vec2>();
    private static List<Vec2> _chestPoints = new List<Vec2>();

    public static void Initialize()
    {
      try
      {
        DuckRig._hatPoints.Clear();
        DuckRig._chestPoints.Clear();
        BinaryReader binaryReader = new BinaryReader((Stream) File.OpenRead(Content.path + "rig_duckRig.rig"));
        int num = binaryReader.ReadInt32();
        for (int index = 0; index < num; ++index)
        {
          DuckRig._hatPoints.Add(new Vec2()
          {
            x = (float) binaryReader.ReadInt32(),
            y = (float) binaryReader.ReadInt32()
          });
          DuckRig._chestPoints.Add(new Vec2()
          {
            x = (float) binaryReader.ReadInt32(),
            y = (float) binaryReader.ReadInt32()
          });
        }
        binaryReader.Close();
        binaryReader.Dispose();
      }
      catch (Exception ex)
      {
        Program.LogLine(MonoMain.GetExceptionString((object) ex));
      }
    }

    public static Vec2 GetHatPoint(int frame) => DuckRig._hatPoints[frame];

    public static Vec2 GetChestPoint(int frame) => DuckRig._chestPoints[frame];
  }
}
