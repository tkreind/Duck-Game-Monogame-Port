// Decompiled with JetBrains decompiler
// Type: DuckGame.SpookyTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class SpookyTileset : AutoBlock
  {
    public SpookyTileset(float x, float y)
      : base(x, y, "spookyTileset")
    {
      this._editorName = "Spooky";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.verticalWidth = 10f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 14f;
    }

    public override void Draw() => base.Draw();
  }
}
