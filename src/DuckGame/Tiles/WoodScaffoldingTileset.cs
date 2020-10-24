// Decompiled with JetBrains decompiler
// Type: DuckGame.WoodScaffoldingTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class WoodScaffoldingTileset : AutoPlatform
  {
    public WoodScaffoldingTileset(float x, float y)
      : base(x, y, "woodScaffolding")
    {
      this._editorName = "Wood Scaffold";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this._hasNubs = false;
      this._collideBottom = true;
    }
  }
}
