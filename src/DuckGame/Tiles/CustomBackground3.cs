// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomBackground3
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|custom")]
  [BaggedProperty("isInDemo", false)]
  public class CustomBackground3 : CustomBackground
  {
    private static CustomType _customType = CustomType.Background;

    public static string customBackground03
    {
      get => Custom.data[CustomBackground3._customType][2];
      set => Custom.data[CustomBackground3._customType][2] = value;
    }

    public CustomBackground3(float x, float y)
      : base(x, y)
    {
      this.customIndex = 2;
      this.graphic = (Sprite) new SpriteMap("arcadeBackground", 16, 16, true);
      this._opacityFromGraphic = true;
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._editorName = "03";
      this.UpdateCurrentTileset();
    }
  }
}
