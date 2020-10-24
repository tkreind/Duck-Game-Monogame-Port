// Decompiled with JetBrains decompiler
// Type: DuckGame.UndergroundTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks")]
  public class UndergroundTileset : AutoBlock
  {
    public UndergroundTileset(float x, float y)
      : base(x, y, "undergroundTileset")
    {
      this._editorName = "Bunker";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 10f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 15f;
    }

    public override void Draw() => base.Draw();
  }
}
