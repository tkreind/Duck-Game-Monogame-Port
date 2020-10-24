// Decompiled with JetBrains decompiler
// Type: DuckGame.LSFilterMods
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class LSFilterMods : IFilterLSItems
  {
    private bool _isOnline;
    private static Dictionary<string, Dictionary<bool, bool>> _filters = new Dictionary<string, Dictionary<bool, bool>>();

    public LSFilterMods(bool isOnline) => this._isOnline = isOnline;

    private bool Cache(string lev, bool result)
    {
      Dictionary<bool, bool> dictionary = (Dictionary<bool, bool>) null;
      if (!LSFilterMods._filters.TryGetValue(lev, out dictionary))
        LSFilterMods._filters[lev] = dictionary = new Dictionary<bool, bool>();
      dictionary[this._isOnline] = result;
      return result;
    }

    public bool Filter(string lev, LevelLocation location = LevelLocation.Any)
    {
      try
      {
        Dictionary<bool, bool> dictionary = (Dictionary<bool, bool>) null;
        bool flag;
        if (LSFilterMods._filters.TryGetValue(lev, out dictionary) && dictionary.TryGetValue(this._isOnline, out flag))
          return flag;
        LevelData levelData = (LevelData) null;
        if (!LSItem.bullshitLevelCache.TryGetValue(lev, out levelData))
        {
          levelData = DuckFile.LoadLevel(lev);
          LSItem.bullshitLevelCache[lev] = levelData;
        }
        if (levelData == null)
          return this.Cache(lev, false);
        ModMetaData modData = levelData.modData;
        if (this._isOnline)
        {
          if (modData.hasLocalMods)
            return this.Cache(lev, false);
          HashSet<ulong> ulongSet = new HashSet<ulong>();
          foreach (Mod mod in ModLoader.accessibleMods.Where<Mod>((Func<Mod, bool>) (a => a.configuration.isWorkshop)))
            ulongSet.Add(mod.configuration.workshopID);
          if (!modData.workshopIDs.IsSubsetOf((IEnumerable<ulong>) ulongSet))
            return this.Cache(lev, false);
        }
        return this.Cache(lev, true);
      }
      catch
      {
        return this.Cache(lev, false);
      }
    }
  }
}
