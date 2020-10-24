// Decompiled with JetBrains decompiler
// Type: DuckGame.ScaffoldingTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class ScaffoldingTileset : AutoPlatform
  {
    public ScaffoldingTileset(float x, float y)
      : base(x, y, "scaffolding")
    {
      this._editorName = "Scaffold";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this._collideBottom = true;
    }
  }
}
