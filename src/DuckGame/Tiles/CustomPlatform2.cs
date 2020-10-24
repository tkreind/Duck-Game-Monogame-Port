// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomPlatform2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|custom")]
  [BaggedProperty("isInDemo", false)]
  public class CustomPlatform2 : CustomPlatform
  {
    private static CustomType _customType = CustomType.Platform;

    public static string customPlatform02
    {
      get => Custom.data[CustomPlatform2._customType][1];
      set => Custom.data[CustomPlatform2._customType][1] = value;
    }

    public CustomPlatform2(float x, float y, string tset)
      : base(x, y, "CUSTOMPLAT02")
    {
      this.customIndex = 1;
      this._editorName = "Custom Platform 02";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this.UpdateCurrentTileset();
    }
  }
}
