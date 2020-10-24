// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  [BaggedProperty("isInDemo", false)]
  public class PyramidTileset : AutoBlock
  {
    public PyramidTileset(float x, float y)
      : base(x, y, "pyramidTileset")
    {
      this._editorName = "Pyramid";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 14f;
      this.verticalWidth = 12f;
      this.horizontalHeight = 13f;
    }
  }
}
