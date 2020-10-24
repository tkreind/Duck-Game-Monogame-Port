// Decompiled with JetBrains decompiler
// Type: DuckGame.FloorWindow
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class FloorWindow : Window
  {
    public FloorWindow(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.angle = -1.570796f;
      this.collisionSize = new Vec2(32f, 6f);
      this.collisionOffset = new Vec2(-16f, -2f);
      this._editorName = nameof (FloorWindow);
      this.center = new Vec2(2f, 16f);
      this.editorOffset = new Vec2(8f, -6f);
      this.floor = true;
      this.UpdateHeight();
    }
  }
}
