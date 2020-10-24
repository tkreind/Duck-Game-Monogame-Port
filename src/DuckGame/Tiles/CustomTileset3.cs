// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomTileset3
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|custom")]
  [BaggedProperty("isInDemo", false)]
  public class CustomTileset3 : CustomTileset
  {
    private static CustomType _customType;

    public static string customTileset03
    {
      get => Custom.data[CustomTileset3._customType][2];
      set => Custom.data[CustomTileset3._customType][2] = value;
    }

    public CustomTileset3(float x, float y, string tset)
      : base(x, y, "CUSTOM03")
    {
      this.customIndex = 2;
      this._editorName = "Custom Block 03";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 16f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 16f;
      this.UpdateCurrentTileset();
    }
  }
}
