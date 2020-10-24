// Decompiled with JetBrains decompiler
// Type: DuckGame.DoorFrame
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DoorFrame : Thing
  {
    public DoorFrame(float xpos, float ypos, bool secondaryFrame)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite(secondaryFrame ? "pyramidDoorFrame" : "doorFrame");
      this.center = new Vec2(5f, 26f);
      this.depth = new Depth(-0.95f);
      this._editorCanModify = false;
    }
  }
}
