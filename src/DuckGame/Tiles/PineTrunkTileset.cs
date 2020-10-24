// Decompiled with JetBrains decompiler
// Type: DuckGame.PineTrunkTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|snow")]
  [BaggedProperty("isInDemo", false)]
  public class PineTrunkTileset : AutoPlatform
  {
    public PineTrunkTileset(float x, float y)
      : base(x, y, "pineTreeTileset")
    {
      this._editorName = "Pine Trunk";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.verticalWidth = 6f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this._hasNubs = false;
      this.depth = new Depth(-0.6f);
      this.placementLayerOverride = Layer.Blocks;
    }

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      if ((double) with.impactPowerV > 2.40000009536743)
      {
        Level.CheckPoint<PineTree>(this.x, this.y)?.KnockOffSnow(with.velocity, true);
        Level.CheckPoint<PineTree>(this.x, this.y - 16f)?.KnockOffSnow(with.velocity, true);
      }
      this.OnSoftImpact(with, from);
    }
  }
}
