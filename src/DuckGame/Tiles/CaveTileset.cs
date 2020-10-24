// Decompiled with JetBrains decompiler
// Type: DuckGame.CaveTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class CaveTileset : AutoBlock
  {
    public CaveTileset(float x, float y)
      : base(x, y, "caveTileset")
    {
      this._editorName = "Cave";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 16f;
      this.verticalWidthThick = 16f;
      this.horizontalHeight = 16f;
      this._hasNubs = false;
    }
  }
}
