// Decompiled with JetBrains decompiler
// Type: DuckGame.BlueprintTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("blocks")]
  public class BlueprintTileset : AutoBlock
  {
    public BlueprintTileset(float x, float y)
      : base(x, y, "blueprintTileset")
    {
      this._editorName = "Blueprint";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 16f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 16f;
    }
  }
}
