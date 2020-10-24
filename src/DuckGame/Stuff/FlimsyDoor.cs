// Decompiled with JetBrains decompiler
// Type: DuckGame.FlimsyDoor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|pyramid")]
  public class FlimsyDoor : Door
  {
    public FlimsyDoor(float xpos, float ypos)
      : base(xpos, ypos)
      => this._editorName = "Flimsy Door";

    public override void Initialize()
    {
      this.secondaryFrame = true;
      this._sprite = new SpriteMap("flimsyDoor", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.colWide = 4f;
      base.Initialize();
    }
  }
}
