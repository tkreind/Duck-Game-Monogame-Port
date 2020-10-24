// Decompiled with JetBrains decompiler
// Type: DuckGame.LevelData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [MagicNumber(5033950674723417)]
  [ChunkVersion(1)]
  public class LevelData : BinaryClassChunk
  {
    private string _path;
    private LevelLocation _location;

    public void SetPath(string path) => this._path = path;

    public string GetPath() => this._path;

    public void SetLocation(LevelLocation loc) => this._location = loc;

    public LevelLocation GetLocation() => this._location;

    public LevelMetaData metaData => this.GetChunk<LevelMetaData>(nameof (metaData));

    public LevelCustomData customData => this.GetChunk<LevelCustomData>(nameof (customData));

    public WorkshopMetaData workshopData => this.GetChunk<WorkshopMetaData>(nameof (workshopData));

    public ModMetaData modData => this.GetChunk<ModMetaData>(nameof (modData));

    public ProceduralChunkData proceduralData => this.GetChunk<ProceduralChunkData>(nameof (proceduralData));

    public PreviewData previewData => this.GetChunk<PreviewData>(nameof (previewData));

    public LevelObjects objects => this.GetChunk<LevelObjects>(nameof (objects));
  }
}
