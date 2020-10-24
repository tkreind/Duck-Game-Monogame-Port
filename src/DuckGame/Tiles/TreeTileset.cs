// Decompiled with JetBrains decompiler
// Type: DuckGame.TreeTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("blocks")]
  public class TreeTileset : AutoPlatform
  {
    public TreeTileset(float x, float y)
      : base(x, y, "treeTileset")
    {
      this._editorName = "Tree";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.verticalWidth = 6f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this._hasNubs = false;
      this.depth = (Depth) -0.15f;
      this.placementLayerOverride = Layer.Blocks;
    }
  }
}
