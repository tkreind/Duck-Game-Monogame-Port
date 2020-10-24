// Decompiled with JetBrains decompiler
// Type: DuckGame.ProceduralChunkData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ProceduralChunkData : BinaryClassChunk
  {
    public int sideMask;
    public float chance;
    public int maxPerLevel;
    public bool enableSingle;
    public bool enableMulti;
    public bool canMirror = true;
    public bool isMirrored;
    public int numArmor;
    public int numEquipment;
    public int numSpawns;
    public int numTeamSpawns;
    public int numLockedDoors;
    public int numKeys;
    public string weaponConfig;
    public string spawnerConfig;
  }
}
