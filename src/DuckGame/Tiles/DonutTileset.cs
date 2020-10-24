// Decompiled with JetBrains decompiler
// Type: DuckGame.DonutTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("blocks")]
  public class DonutTileset : AutoBlock
  {
    public DonutTileset(float x, float y)
      : base(x, y, "donutTileset")
    {
      this._editorName = "Donut";
      this.physicsMaterial = PhysicsMaterial.Crust;
      this.verticalWidthThick = 15f;
      this.verticalWidth = 12f;
      this.horizontalHeight = 15f;
    }
  }
}
