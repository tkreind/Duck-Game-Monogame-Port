// Decompiled with JetBrains decompiler
// Type: DuckGame.ConsoleCommand
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ConsoleCommand
  {
    private string _command;

    public ConsoleCommand(string command) => this._command = command;

    public string NextWord()
    {
      int num = 0;
      if (this._command.Length <= 0)
        return "";
      while (this._command[num] == ' ')
      {
        ++num;
        if (num >= this._command.Length)
          return "";
      }
      int startIndex = num;
      while (this._command[num] != ' ')
      {
        ++num;
        if (num >= this._command.Length)
          break;
      }
      string str = this._command.Substring(startIndex, num - startIndex);
      this._command = this._command.Substring(num, this._command.Length - num);
      return str;
    }
  }
}
