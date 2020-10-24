// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpawnPlayer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMSpawnPlayer : NMObjectMessage
  {
    public float xpos;
    public float ypos;
    public int duckID;
    public bool isPlayerDuck;

    public NMSpawnPlayer()
    {
    }

    public NMSpawnPlayer(float xVal, float yVal, int duck, bool playerDuck, ushort objectID)
      : base(objectID)
    {
      this.xpos = xVal;
      this.ypos = yVal;
      this.duckID = duck;
      this.isPlayerDuck = playerDuck;
    }
  }
}
