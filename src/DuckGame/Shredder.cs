// Decompiled with JetBrains decompiler
// Type: DuckGame.Shredder
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class Shredder
  {
    private static List<Note> _basicScale = new List<Note>()
    {
      Note.c,
      Note.d,
      Note.f,
      Note.a
    };
    private static int _currentNote = 0;
    private static float _noteWait = 0.0f;

    public static int GetNextNote(int note, List<Note> scale)
    {
      for (int index = 0; index < scale.Count; ++index)
      {
        if (scale[index] == (Note) (Shredder._currentNote % 12))
        {
          if (index < scale.Count - 1)
          {
            int num1 = 0;
            int num2 = (int) scale[index + 1];
            int num3 = (int) scale[index];
            while (num3 != num2)
            {
              num3 = (num3 + 1) % 12;
              ++num1;
            }
            note += num1;
          }
          else
          {
            int num1 = 0;
            int num2 = (int) scale[0];
            int num3 = (int) scale[index];
            while (num3 != num2)
            {
              num3 = (num3 + 1) % 12;
              ++num1;
            }
            note += num1;
          }
        }
      }
      return note % 48;
    }

    public static int GetPrevNote(int note, List<Note> scale)
    {
      for (int index = 0; index < scale.Count; ++index)
      {
        if (scale[index] == (Note) (Shredder._currentNote % 12))
        {
          if (index > 0)
          {
            int num1 = 0;
            int num2 = (int) scale[index - 1];
            int num3 = (int) scale[index];
            while (num3 != num2)
            {
              num3 = (num3 + 1) % 12;
              ++num1;
            }
            note -= 12 - num1;
          }
          else
          {
            int num1 = 0;
            int num2 = (int) scale[scale.Count - 1];
            int num3 = (int) scale[index];
            while (num3 != num2)
            {
              num3 = (num3 + 1) % 12;
              ++num1;
            }
            note -= 12 - num1;
          }
        }
      }
      if (note < 0)
        note = 48 + note;
      return note % 48;
    }

    public static void Update()
    {
      XInputPad device = Input.GetDevice<XInputPad>();
      if ((double) Shredder._noteWait <= 0.0)
      {
        if (device.MapDown(16384, false))
        {
          Shredder._currentNote = Shredder.GetNextNote(Shredder._currentNote, Shredder._basicScale);
          SFX.Play("guitar/guitar-" + Change.ToString((object) Shredder._currentNote));
          Shredder._noteWait = 1f;
        }
        if (device.MapDown(32768, false))
        {
          Shredder._currentNote = Shredder._currentNote = (int) Shredder._basicScale[0];
          SFX.Play("guitar/guitar-" + Change.ToString((object) Shredder._currentNote));
          Shredder._noteWait = 1f;
        }
        if (device.MapDown(8192, false))
        {
          Shredder._currentNote = Shredder.GetPrevNote(Shredder._currentNote, Shredder._basicScale);
          SFX.Play("guitar/guitar-" + Change.ToString((object) Shredder._currentNote));
          Shredder._noteWait = 1f;
        }
      }
      Shredder._noteWait -= 0.15f;
    }
  }
}
