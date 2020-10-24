// Decompiled with JetBrains decompiler
// Type: DuckGame.SpaceTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class SpaceTileset : AutoBlock
  {
    public SpaceTileset(float x, float y)
      : base(x, y, "spaceTileset")
    {
      this._editorName = "Space";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 10f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 15f;
    }
  }
}
