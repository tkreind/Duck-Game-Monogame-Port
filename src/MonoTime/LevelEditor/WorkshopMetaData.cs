// Decompiled with JetBrains decompiler
// Type: DuckGame.WorkshopMetaData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class WorkshopMetaData : BinaryClassChunk
  {
    public string name;
    public string description;
    public string author;
    public RemoteStoragePublishedFileVisibility visibility;
    public List<string> tags = new List<string>();

    public LevelMetaData metaData => this.GetChunk<LevelMetaData>(nameof (metaData));
  }
}
