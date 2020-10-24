// Decompiled with JetBrains decompiler
// Type: DuckGame.LSFilterLevelType
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class LSFilterLevelType : IFilterLSItems
  {
    private static Dictionary<string, LevelType> _types = new Dictionary<string, LevelType>();
    private LevelType _type;
    private bool _needsDeathmatchTag;

    public LSFilterLevelType(LevelType type, bool needsDeathmatchTag = false)
    {
      this._type = type;
      this._needsDeathmatchTag = needsDeathmatchTag;
    }

    public bool Filter(string lev, LevelLocation location = LevelLocation.Any)
    {
      try
      {
        LevelType levelType = LevelType.Invalid;
        if (LSFilterLevelType._types.TryGetValue(lev, out levelType))
          return levelType == this._type;
        LevelData levelData = (LevelData) null;
        if (!LSItem.bullshitLevelCache.TryGetValue(lev, out levelData))
        {
          levelData = DuckFile.LoadLevel(lev);
          LSItem.bullshitLevelCache[lev] = levelData;
        }
        if (levelData == null)
        {
          LSFilterLevelType._types[lev] = LevelType.Invalid;
          return false;
        }
        if (this._needsDeathmatchTag && location == LevelLocation.Workshop && (levelData.metaData.workshopID != 0UL && !levelData.metaData.deathmatchReady))
        {
          LSFilterLevelType._types[lev] = LevelType.Strange;
          return false;
        }
        LevelType type = levelData.metaData.type;
        LSFilterLevelType._types[lev] = type;
        return type == this._type;
      }
      catch
      {
        LSFilterLevelType._types[lev] = LevelType.Invalid;
        return false;
      }
    }
  }
}
