// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomTileset2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("blocks|custom")]
  public class CustomTileset2 : CustomTileset
  {
    private static CustomType _customType;

    public static string customTileset02
    {
      get => Custom.data[CustomTileset2._customType][1];
      set => Custom.data[CustomTileset2._customType][1] = value;
    }

    public CustomTileset2(float x, float y, string tset)
      : base(x, y, "CUSTOM02")
    {
      this.customIndex = 1;
      this._editorName = "Custom Block 02";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 16f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 16f;
      this.UpdateCurrentTileset();
    }
  }
}
