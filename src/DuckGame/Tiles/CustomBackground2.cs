// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomBackground2
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("background|custom")]
  public class CustomBackground2 : CustomBackground
  {
    private static CustomType _customType = CustomType.Background;

    public static string customBackground02
    {
      get => Custom.data[CustomBackground2._customType][1];
      set => Custom.data[CustomBackground2._customType][1] = value;
    }

    public CustomBackground2(float x, float y)
      : base(x, y)
    {
      this.customIndex = 1;
      this.graphic = (Sprite) new SpriteMap("arcadeBackground", 16, 16, true);
      this._opacityFromGraphic = true;
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._editorName = "02";
      this.UpdateCurrentTileset();
    }
  }
}
