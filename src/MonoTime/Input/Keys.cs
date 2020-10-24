﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.KeyHelper
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class KeyHelper
  {
    public static char KeyToChar(Keys key)
    {
      switch (key)
      {
        case Keys.D0:
          return '0';
        case Keys.D1:
          return '1';
        case Keys.D2:
          return '2';
        case Keys.D3:
          return '3';
        case Keys.D4:
          return '4';
        case Keys.D5:
          return '5';
        case Keys.D6:
          return '6';
        case Keys.D7:
          return '7';
        case Keys.D8:
          return '8';
        case Keys.D9:
          return '9';
        case Keys.A:
          return 'A';
        case Keys.B:
          return 'B';
        case Keys.C:
          return 'C';
        case Keys.D:
          return 'D';
        case Keys.E:
          return 'E';
        case Keys.F:
          return 'F';
        case Keys.G:
          return 'G';
        case Keys.H:
          return 'H';
        case Keys.I:
          return 'I';
        case Keys.J:
          return 'J';
        case Keys.K:
          return 'K';
        case Keys.L:
          return 'L';
        case Keys.M:
          return 'M';
        case Keys.N:
          return 'N';
        case Keys.O:
          return 'O';
        case Keys.P:
          return 'P';
        case Keys.Q:
          return 'Q';
        case Keys.R:
          return 'R';
        case Keys.S:
          return 'S';
        case Keys.T:
          return 'T';
        case Keys.U:
          return 'U';
        case Keys.V:
          return 'V';
        case Keys.W:
          return 'W';
        case Keys.X:
          return 'X';
        case Keys.Y:
          return 'Y';
        case Keys.Z:
          return 'Z';
        case Keys.NumPad0:
          return '0';
        case Keys.NumPad1:
          return '1';
        case Keys.NumPad2:
          return '2';
        case Keys.NumPad3:
          return '3';
        case Keys.NumPad4:
          return '4';
        case Keys.NumPad5:
          return '5';
        case Keys.NumPad6:
          return '6';
        case Keys.NumPad7:
          return '7';
        case Keys.NumPad8:
          return '8';
        case Keys.NumPad9:
          return '9';
        case Keys.OemSemicolon:
          return ';';
        case Keys.OemPlus:
          return '+';
        case Keys.OemComma:
          return ',';
        case Keys.OemMinus:
          return '-';
        case Keys.OemPeriod:
          return '.';
        case Keys.OemQuestion:
          return '?';
        case Keys.OemTilde:
          return '~';
        case Keys.OemOpenBrackets:
          return '[';
        case Keys.OemPipe:
          return '|';
        case Keys.OemCloseBrackets:
          return ']';
        case Keys.OemQuotes:
          return '"';
        case Keys.OemBackslash:
          return '\\';
        default:
          return ' ';
      }
    }
  }
}
