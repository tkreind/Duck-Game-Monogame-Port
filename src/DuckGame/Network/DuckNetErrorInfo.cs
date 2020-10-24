// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckNetErrorInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DuckNetErrorInfo
  {
    public string message;
    public bool tooNew;
    public Profile user;
    public DuckNetError error;

    public DuckNetErrorInfo()
    {
    }

    public DuckNetErrorInfo(DuckNetError e, string msg)
    {
      this.message = msg;
      this.error = e;
    }
  }
}
