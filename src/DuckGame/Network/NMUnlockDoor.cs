// Decompiled with JetBrains decompiler
// Type: DuckGame.NMUnlockDoor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMUnlockDoor : NMEvent
  {
    public Door door;

    public NMUnlockDoor()
    {
    }

    public NMUnlockDoor(Door d) => this.door = d;

    public override void Activate()
    {
      if (this.door == null)
        return;
      this.door.networkUnlockMessage = true;
    }
  }
}
