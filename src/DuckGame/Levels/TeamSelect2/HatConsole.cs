// Decompiled with JetBrains decompiler
// Type: DuckGame.HatConsole
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class HatConsole : Thing
  {
    public ProfileBox2 box;

    public HatConsole(float xpos, float ypos, ProfileBox2 bbox)
      : base(xpos, ypos)
      => this.box = bbox;
  }
}
