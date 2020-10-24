// Decompiled with JetBrains decompiler
// Type: DuckGame.NublessSnowTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("blocks|snow")]
  public class NublessSnowTileset : SnowTileset
  {
    public NublessSnowTileset(float x, float y)
      : base(x, y, "nublessSnow")
    {
      this._editorName = "Snow NONUBS";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 15f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 15f;
      this._tileset = "snowTileset";
      this.willHeat = true;
      this._impactThreshold = -1f;
      this._hasNubs = false;
      this.meltedTileset = "nublessSnow";
      this.frozenTileset = "nublessIceTileset";
    }
  }
}
