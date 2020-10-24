// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomPlatform3
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|custom")]
  [BaggedProperty("isInDemo", false)]
  public class CustomPlatform3 : CustomPlatform
  {
    private static CustomType _customType = CustomType.Platform;

    public static string customPlatform03
    {
      get => Custom.data[CustomPlatform3._customType][2];
      set => Custom.data[CustomPlatform3._customType][2] = value;
    }

    public CustomPlatform3(float x, float y, string tset)
      : base(x, y, "CUSTOMPLAT03")
    {
      this.customIndex = 2;
      this._editorName = "Custom Platform 03";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this.UpdateCurrentTileset();
    }
  }
}
