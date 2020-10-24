// Decompiled with JetBrains decompiler
// Type: DuckGame.DestroyType
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public abstract class DestroyType
  {
    private static Map<byte, System.Type> _types = new Map<byte, System.Type>();
    private Thing _thing;

    public static Map<byte, System.Type> indexTypeMap => DestroyType._types;

    public static void InitializeTypes()
    {
      if (MonoMain.moddingEnabled)
      {
        byte key = 0;
        foreach (System.Type sortedType in ManagedContent.DestroyTypes.SortedTypes)
        {
          DestroyType._types.Add(key, sortedType);
          ++key;
        }
      }
      else
      {
        List<System.Type> list = Editor.GetSubclasses(typeof (DestroyType)).ToList<System.Type>();
        byte key = 0;
        foreach (System.Type type in list)
        {
          DestroyType._types.Add(key, type);
          ++key;
        }
      }
    }

    public Thing thing => this._thing;

    public System.Type killThingType => this._thing == null ? (System.Type) null : this._thing.killThingType;

    public Profile responsibleProfile => this._thing == null ? (Profile) null : this._thing.responsibleProfile;

    public DestroyType(Thing t = null) => this._thing = t;
  }
}
