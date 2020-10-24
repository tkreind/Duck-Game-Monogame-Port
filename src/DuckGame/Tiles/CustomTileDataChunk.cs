// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomTileDataChunk
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CustomTileDataChunk : BinaryClassChunk
  {
    public string path;
    public string textureData;
    public int verticalWidthThick;
    public int verticalWidth;
    public int horizontalHeight;
    public bool leftNubber;
    public bool rightNubber;
    public uint textureChecksum;

    public CustomTileData GetTileData()
    {
      CustomTileData customTileData = new CustomTileData();
      if (this.textureData == null)
        return customTileData;
      customTileData.path = this.path;
      customTileData.texture = Editor.StringToTexture(this.textureData);
      customTileData.verticalWidthThick = this.verticalWidthThick;
      customTileData.verticalWidth = this.verticalWidth;
      customTileData.horizontalHeight = this.horizontalHeight;
      customTileData.leftNubber = this.leftNubber;
      customTileData.rightNubber = this.rightNubber;
      customTileData.checksum = this.textureChecksum;
      return customTileData;
    }
  }
}
