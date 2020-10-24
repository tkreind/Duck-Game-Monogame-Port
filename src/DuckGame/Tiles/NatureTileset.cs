// Decompiled with JetBrains decompiler
// Type: DuckGame.NatureTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  [BaggedProperty("isInDemo", true)]
  public class NatureTileset : AutoBlock
  {
    public NatureTileset(float x, float y)
      : base(x, y, "natureTileset")
    {
      this._editorName = "Nature";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 15f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 15f;
    }
  }
}
