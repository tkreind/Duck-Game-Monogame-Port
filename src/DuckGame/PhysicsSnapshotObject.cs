// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsSnapshotObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class PhysicsSnapshotObject
  {
    public PhysicsObject thing;
    public ushort networkID;
    public System.Type type;
    public SnapshotContainedData data;
    public Vec2 position;
    public Vec2 velocity;
    public float angle;
    public byte frame;
    public double serverTime;
    public double clientTime;
    public int inputState;
    public object classData;
    public PhysicsSnapshotDuckProperties duckProps;
  }
}
