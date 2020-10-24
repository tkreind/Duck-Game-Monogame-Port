// Decompiled with JetBrains decompiler
// Type: DuckGame.RandomLevelData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DuckGame
{
  public class RandomLevelData
  {
    public Dictionary<System.Type, int> weapons = new Dictionary<System.Type, int>();
    public Dictionary<System.Type, int> spawners = new Dictionary<System.Type, int>();
    public int numWeapons;
    public int numSuperWeapons;
    public int numFatalWeapons;
    public int numPermanentWeapons;
    public int numPermanentSuperWeapons;
    public int numPermanentFatalWeapons;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public float chance;
    public int max = 2;
    public bool single;
    public bool multi;
    public bool canMirror = true;
    public bool isMirrored;
    public int numArmor;
    public int numEquipment;
    public int numSpawns;
    public int numTeamSpawns;
    public int numLockedDoors;
    public int numKeys;
    public List<BinaryClassChunk> data;
    public bool flip;
    public bool symmetry;
    public string file = "";
    private Vec2 posBeforeTranslate = Vec2.Zero;
    private bool mainLoad = true;

    public RandomLevelData Flipped() => new RandomLevelData()
    {
      data = this.data,
      flip = !this.flip,
      left = this.right,
      right = this.left,
      up = this.up,
      down = this.down,
      chance = this.chance,
      max = this.max,
      file = this.file,
      canMirror = this.canMirror,
      isMirrored = this.isMirrored,
      numWeapons = this.numWeapons,
      numSuperWeapons = this.numSuperWeapons,
      numFatalWeapons = this.numFatalWeapons,
      numPermanentWeapons = this.numPermanentWeapons,
      numPermanentSuperWeapons = this.numPermanentSuperWeapons,
      numPermanentFatalWeapons = this.numPermanentFatalWeapons,
      numArmor = this.numArmor,
      numEquipment = this.numEquipment,
      numSpawns = this.numSpawns,
      numTeamSpawns = this.numTeamSpawns,
      numLockedDoors = this.numLockedDoors,
      numKeys = this.numKeys
    };

    public RandomLevelData Symmetric()
    {
      RandomLevelData randomLevelData = new RandomLevelData()
      {
        data = this.data,
        flip = this.flip,
        left = this.right,
        right = this.left,
        up = this.up,
        down = this.down,
        symmetry = true,
        file = this.file,
        canMirror = this.canMirror,
        isMirrored = this.isMirrored
      };
      randomLevelData.right = randomLevelData.left;
      randomLevelData.chance = this.chance;
      randomLevelData.max = this.max;
      randomLevelData.numWeapons = this.numWeapons;
      randomLevelData.numSuperWeapons = this.numSuperWeapons;
      randomLevelData.numFatalWeapons = this.numFatalWeapons;
      randomLevelData.numPermanentWeapons = this.numPermanentWeapons;
      randomLevelData.numPermanentSuperWeapons = this.numPermanentSuperWeapons;
      randomLevelData.numPermanentFatalWeapons = this.numPermanentFatalWeapons;
      randomLevelData.numArmor = this.numArmor;
      randomLevelData.numEquipment = this.numEquipment;
      randomLevelData.numSpawns = this.numSpawns;
      randomLevelData.numTeamSpawns = this.numTeamSpawns;
      randomLevelData.numLockedDoors = this.numLockedDoors;
      randomLevelData.numKeys = this.numKeys;
      return randomLevelData;
    }

    public void ApplyWeaponData(string data)
    {
      this.weapons.Clear();
      this.numWeapons = 0;
      this.numSuperWeapons = 0;
      this.numFatalWeapons = 0;
      string str = data;
      char[] chArray = new char[1]{ '|' };
      foreach (string name in str.Split(chArray))
      {
        if (name != "")
        {
          try
          {
            System.Type type = Editor.GetType(name);
            if (!this.weapons.ContainsKey(type))
              this.weapons[type] = 0;
            Dictionary<System.Type, int> weapons;
            System.Type key;
            (weapons = this.weapons)[key = type] = weapons[key] + 1;
            ++this.numWeapons;
            IReadOnlyPropertyBag bag = ContentProperties.GetBag(type);
            if (bag.GetOrDefault<bool>("isSuperWeapon", false))
              ++this.numSuperWeapons;
            if (bag.GetOrDefault<bool>("isFatal", true))
              ++this.numFatalWeapons;
          }
          catch
          {
          }
        }
      }
    }

    public void ApplySpawnerData(string data)
    {
      this.spawners.Clear();
      this.numPermanentWeapons = 0;
      this.numPermanentSuperWeapons = 0;
      this.numPermanentFatalWeapons = 0;
      string str = data;
      char[] chArray = new char[1]{ '|' };
      foreach (string name in str.Split(chArray))
      {
        if (name != "")
        {
          try
          {
            System.Type type = Editor.GetType(name);
            if (!this.spawners.ContainsKey(type))
              this.spawners[type] = 0;
            Dictionary<System.Type, int> spawners;
            System.Type key;
            (spawners = this.spawners)[key = type] = spawners[key] + 1;
            ++this.numPermanentWeapons;
            IReadOnlyPropertyBag bag = ContentProperties.GetBag(type);
            if (bag.GetOrDefault<bool>("isSuperWeapon", false))
              ++this.numPermanentSuperWeapons;
            if (bag.GetOrDefault<bool>("isFatal", true))
              ++this.numPermanentFatalWeapons;
          }
          catch
          {
          }
        }
      }
    }

    public void ApplyItemData(string data)
    {
      string[] strArray = data.Split('|');
      int num = 0;
      foreach (string str in strArray)
      {
        switch (num)
        {
          case 0:
            this.numArmor = Convert.ToInt32(str);
            break;
          case 1:
            this.numEquipment = Convert.ToInt32(str);
            break;
          case 2:
            this.numSpawns = Convert.ToInt32(str);
            break;
          case 3:
            this.numTeamSpawns = Convert.ToInt32(str);
            break;
          case 4:
            this.numLockedDoors = Convert.ToInt32(str);
            break;
          case 5:
            this.numKeys = Convert.ToInt32(str);
            break;
        }
        ++num;
      }
    }

    public RandomLevelData Combine(RandomLevelData dat)
    {
      RandomLevelData randomLevelData = new RandomLevelData();
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      foreach (FieldInfo field in this.GetType().GetFields(bindingAttr))
      {
        if (field.FieldType == typeof (int))
          field.SetValue((object) randomLevelData, (object) ((int) field.GetValue((object) this) + (int) field.GetValue((object) dat)));
        if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof (Dictionary<,>))
        {
          Dictionary<System.Type, int> dictionary1 = field.GetValue((object) this) as Dictionary<System.Type, int>;
          Dictionary<System.Type, int> dictionary2 = field.GetValue((object) dat) as Dictionary<System.Type, int>;
          Dictionary<System.Type, int> dictionary3 = new Dictionary<System.Type, int>();
          foreach (KeyValuePair<System.Type, int> keyValuePair in dictionary1)
          {
            if (!dictionary3.ContainsKey(keyValuePair.Key))
              dictionary3[keyValuePair.Key] = 0;
            Dictionary<System.Type, int> dictionary4;
            System.Type key;
            (dictionary4 = dictionary3)[key = keyValuePair.Key] = dictionary4[key] + keyValuePair.Value;
          }
          foreach (KeyValuePair<System.Type, int> keyValuePair in dictionary2)
          {
            if (!dictionary3.ContainsKey(keyValuePair.Key))
              dictionary3[keyValuePair.Key] = 0;
            Dictionary<System.Type, int> dictionary4;
            System.Type key;
            (dictionary4 = dictionary3)[key = keyValuePair.Key] = dictionary4[key] + keyValuePair.Value;
          }
          field.SetValue((object) randomLevelData, (object) dictionary3);
        }
      }
      return randomLevelData;
    }

    private Thing ProcessThing(Thing t, float x, float y, Level level)
    {
      if (!t.visibleInGame)
        t.visible = false;
      bool flag = this.flip;
      if (Level.symmetry)
        flag = false;
      if (Level.loadingOppositeSymmetry)
        flag = !flag;
      if (this.mainLoad && Level.symmetry && !(t is ThingContainer))
      {
        if (Level.leftSymmetry && (double) t.x > 88.0)
          return (Thing) null;
        if (!Level.leftSymmetry && (double) t.x < 88.0)
          return (Thing) null;
      }
      if (flag)
      {
        switch (t)
        {
          case ThingContainer _:
            break;
          case BackgroundTile _:
label_14:
            t.flipHorizontal = true;
            BackgroundTile backgroundTile = t as BackgroundTile;
            break;
          default:
            t.SetTranslation(new Vec2((float) (-(double) t.x + (192.0 - (double) t.x) - 16.0), 0.0f));
            goto label_14;
        }
      }
      if (t is BackgroundTile && (t as BackgroundTile).isFlipped)
        t.flipHorizontal = true;
      if (t is BackgroundTile && !(t as BackgroundTile).oppositeSymmetry)
      {
        int num = this.flip ? 1 : 0;
      }
      this.posBeforeTranslate = t.position;
      if (!(t is BackgroundTile))
        t.SetTranslation(new Vec2(x, y));
      level.AddThing(t);
      return t;
    }

    public void Load(float x, float y, Level level, bool symmetric)
    {
      if (symmetric && this.isMirrored)
        symmetric = false;
      if (!symmetric && this.flip)
        Level.flipH = true;
      if (this.symmetry || symmetric)
        Level.symmetry = true;
      if (this.data != null)
      {
        foreach (BinaryClassChunk node in this.data)
        {
          Thing t1 = Thing.LoadThing(node);
          if (t1 != null && (ContentProperties.GetBag(t1.GetType()).GetOrDefault<bool>("isOnlineCapable", true) || !Network.isActive))
          {
            Level.leftSymmetry = true;
            Level.loadingOppositeSymmetry = false;
            this.mainLoad = true;
            Thing t2 = this.ProcessThing(t1, x, y, level);
            this.mainLoad = false;
            if (t2 != null && Network.isActive && t2.isStateObject)
            {
              GhostManager.context.MakeGhost(t2, initLevel: true);
              t2.ghostType = Editor.IDToType[t2.GetType()];
              if (DuckNetwork.hostDuckIndex >= 0 && DuckNetwork.hostDuckIndex < 4 && DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection != null)
                t2.connection = DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection;
            }
            if (!(t1 is ThingContainer))
            {
              if (t2 != null && Level.symmetry && ((double) this.posBeforeTranslate.x - 8.0 < 80.0 || (double) this.posBeforeTranslate.x - 8.0 > 96.0))
              {
                this.mainLoad = false;
                Level.leftSymmetry = false;
                Level.loadingOppositeSymmetry = true;
                Thing t3 = this.ProcessThing(Thing.LoadThing(node, false), x, y, level);
                if (t3 != null && Network.isActive && t3.isStateObject)
                {
                  GhostManager.context.MakeGhost(t3, initLevel: true);
                  t3.ghostType = Editor.IDToType[t3.GetType()];
                  if (DuckNetwork.hostDuckIndex >= 0 && DuckNetwork.hostDuckIndex < 4 && DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection != null)
                    t3.connection = DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection;
                }
              }
            }
            else
            {
              foreach (Thing thing in (t1 as ThingContainer).things)
              {
                if (thing is BackgroundTile || thing is ForegroundTile)
                {
                  Thing t3 = this.ProcessThing(thing, x, y, level);
                  if (t3 != null && Network.isActive && t3.isStateObject)
                  {
                    GhostManager.context.MakeGhost(t3, initLevel: true);
                    t3.ghostType = Editor.IDToType[t3.GetType()];
                    if (DuckNetwork.hostDuckIndex >= 0 && DuckNetwork.hostDuckIndex < 4 && DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection != null)
                      t3.connection = DuckNetwork.profiles[DuckNetwork.hostDuckIndex].connection;
                  }
                }
              }
            }
          }
        }
        level.things.RefreshState();
      }
      Level.flipH = false;
      Level.symmetry = false;
    }
  }
}
