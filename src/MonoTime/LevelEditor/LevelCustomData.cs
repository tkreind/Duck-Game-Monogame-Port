// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelCustomData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class LevelCustomData : BinaryClassChunk
  {
    public List<string> scriptPackages = new List<string>();

    public CustomTileDataChunk customTileset01Data => this.GetChunk<CustomTileDataChunk>(nameof (customTileset01Data));

    public CustomTileDataChunk customTileset02Data => this.GetChunk<CustomTileDataChunk>(nameof (customTileset02Data));

    public CustomTileDataChunk customTileset03Data => this.GetChunk<CustomTileDataChunk>(nameof (customTileset03Data));

    public CustomTileDataChunk customBackground01Data => this.GetChunk<CustomTileDataChunk>(nameof (customBackground01Data));

    public CustomTileDataChunk customBackground02Data => this.GetChunk<CustomTileDataChunk>(nameof (customBackground02Data));

    public CustomTileDataChunk customBackground03Data => this.GetChunk<CustomTileDataChunk>(nameof (customBackground03Data));

    public CustomTileDataChunk customPlatform01Data => this.GetChunk<CustomTileDataChunk>(nameof (customPlatform01Data));

    public CustomTileDataChunk customPlatform02Data => this.GetChunk<CustomTileDataChunk>(nameof (customPlatform02Data));

    public CustomTileDataChunk customPlatform03Data => this.GetChunk<CustomTileDataChunk>(nameof (customPlatform03Data));

    public CustomTileDataChunk customParallaxData => this.GetChunk<CustomTileDataChunk>(nameof (customParallaxData));
  }
}
