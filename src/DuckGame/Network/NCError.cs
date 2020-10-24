// Decompiled with JetBrains decompiler
// Type: DuckGame.NCError
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NCError
  {
    public string text;
    public NCErrorType type;

    public NCError(string s, NCErrorType tp)
    {
      this.text = s;
      this.type = tp;
    }

    public Color color
    {
      get
      {
        switch (this.type)
        {
          case NCErrorType.Success:
            return Color.Lime;
          case NCErrorType.Message:
            return Color.White;
          case NCErrorType.Warning:
            return Color.Yellow;
          case NCErrorType.Debug:
            return Color.LightPink;
          default:
            return Color.Red;
        }
      }
    }
  }
}
