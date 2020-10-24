// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|custom")]
  public class CustomBackground : BackgroundTile
  {
    private static CustomType _customType = CustomType.Background;
    public int customIndex;
    private string _currentTileset = "";

    public static string customBackground01
    {
      get => Custom.data[CustomBackground._customType][0];
      set => Custom.data[CustomBackground._customType][0] = value;
    }

    public CustomBackground(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.customIndex = 0;
      this.graphic = (Sprite) new SpriteMap("arcadeBackground", 16, 16, true);
      this._opacityFromGraphic = true;
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._editorName = "01";
      this.UpdateCurrentTileset();
    }

    public void UpdateCurrentTileset()
    {
      CustomTileData data = Custom.GetData(this.customIndex, CustomBackground._customType);
      int num = 0;
      if (this.graphic is SpriteMap)
        num = (this.graphic as SpriteMap).frame;
      if (data != null && data.texture != null)
        this.graphic = (Sprite) new SpriteMap((Tex2D) data.texture, 16, 16);
      else
        this.graphic = (Sprite) new SpriteMap("blueprintTileset", 16, 16);
      (this.graphic as SpriteMap).frame = num;
      this._currentTileset = Custom.data[CustomBackground._customType][this.customIndex];
    }

    public override void Draw()
    {
      if (Level.current is Editor && this._currentTileset != Custom.data[CustomBackground._customType][this.customIndex])
        this.UpdateCurrentTileset();
      base.Draw();
    }
  }
}
