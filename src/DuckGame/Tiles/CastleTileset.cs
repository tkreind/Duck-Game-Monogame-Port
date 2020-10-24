// Decompiled with JetBrains decompiler
// Type: DuckGame.CastleTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class CastleTileset : AutoBlock
  {
    public CastleTileset(float x, float y)
      : base(x, y, "castle")
    {
      this._editorName = "Castle";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 10f;
      this.verticalWidthThick = 14f;
      this.horizontalHeight = 14f;
      this._hasNubs = false;
    }
  }
}
